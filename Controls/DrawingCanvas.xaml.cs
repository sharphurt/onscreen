using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using onscreen.API;
using onscreen.API.Tools;
using onscreen.UndoRedo;
using onscreen.UndoRedo.Model;
using Paint.UndoRedo;
using Color = System.Windows.Media.Color;

namespace onscreen.Controls
{
    public partial class DrawingCanvas : Canvas
    {
        private readonly IMementoCaretaker _undoRedoCaretaker;

        public ITool CurrentTool
        {
            get { return (ITool)GetValue(CurrentToolProperty); }
            set { SetValue(CurrentToolProperty, value); }
        }

        public static readonly DependencyProperty CurrentToolProperty =
            DependencyProperty.Register("CurrentTool", typeof(ITool), typeof(DrawingCanvas));

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.Property == CurrentToolProperty)
            {
                CurrentTool.Initialize(this);
                RemoveAllEmptyControls(this);
            }

            base.OnPropertyChanged(e);
        }

        private bool _isActiveDrawing;

        private Control _lastCreatedControl;

        public DrawingCanvas()
        {
            InitializeComponent();

            paintWorkspace.MouseUp += DrawingCanvas_OnMouseUp;

            SizeChanged += (_, _) =>
            {
                var left = (ActualWidth - StackPanel.ActualWidth) / 2;
                var top = (ActualHeight - StackPanel.ActualHeight);

                StackPanel.Margin = new Thickness(left, top, 0, 0);
            };

            GenerateColorPalleteButtons(new List<Color>
            {
                Color.FromRgb(255, 255, 255),
                Color.FromRgb(128, 128, 128),
                Color.FromRgb(192, 0, 0),
                Color.FromRgb(255, 0, 0),
                Color.FromRgb(255, 138, 0),
                Color.FromRgb(241, 233, 34),
                Color.FromRgb(0, 176, 80),

                Color.FromRgb(204, 204, 204),
                Color.FromRgb(0, 0, 0),
                Color.FromRgb(0, 176, 240),
                Color.FromRgb(0, 112, 192),
                Color.FromRgb(0, 32, 96),
                Color.FromRgb(112, 48, 160),
                Color.FromRgb(255, 64, 196)
            });

            var mementoDesigner = new InkCanvasMementoDesigner(InkCanvas);
            _undoRedoCaretaker = new UndoRedoCaretaker(mementoDesigner);
            _undoRedoCaretaker.Initialize();
        }

        void GenerateColorPalleteButtons(List<Color> colors)
        {
            var colorButtonControlTemplate =
                (ControlTemplate)Application.Current.Resources["ColorPalleteRadioButtonControlTemplate"];
            var whiteButtonControlTemplate =
                (ControlTemplate)Application.Current.Resources["ColorPaletteWhiteRadioButtonControlTemplate"];

            foreach (var button in colors.Select(color => new RadioButton
                     {
                         Margin = new Thickness(0, 0, 14, 14),
                         Width = 30,
                         Height = 30,
                         Background = new SolidColorBrush(color),
                         Template = color == Colors.White ? whiteButtonControlTemplate : colorButtonControlTemplate,
                         Focusable = false
                     }))
            {
                ColorPalleteWrapPanel.Children.Add(button);
            }
        }

        void DrawingCanvas_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                _undoRedoCaretaker.StoreState();
            }
        }

        public void OnExecutedCommands(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Undo)
            {
                _undoRedoCaretaker.Undo();
            }
        }

        private void DrawingCanvas_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!Util.IsClickedToExistingControl(this, e.GetPosition(this)))
                RemoveAllEmptyControls(this);

            if (CurrentTool.ToolType != ToolType.Pen && !Util.IsClickedToExistingControl(this, e.GetPosition(this)))
            {
                _lastCreatedControl =
                    CurrentTool.ProcessCreating(this, new DrawingProperties { Position = e.GetPosition(this) });
                _isActiveDrawing = true;
                e.Handled = true;
            }
        }

        private void DrawingCanvas_OnPreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && _lastCreatedControl != null && _isActiveDrawing)
            {
                CurrentTool.ProcessResizing(this, new DrawingProperties
                {
                    Position = e.GetPosition(this),
                    Control = _lastCreatedControl
                });

                e.Handled = true;
            }
        }

        private void DrawingCanvas_OnPreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            _isActiveDrawing = false;
        }

        private void RemoveAllEmptyControls(DrawingCanvas canvas)
        {
            TextTool.RemoveAllEmpty(canvas);
        }
    }
}