using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using onscreen.API;
using onscreen.UndoRedo;
using onscreen.UndoRedo.Model;
using Paint.UndoRedo;
using Paint.UndoRedo.Model;

namespace onscreen.Controls
{
    public partial class DrawingCanvas : InkCanvas
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
            }

            base.OnPropertyChanged(e);
        }

        private bool _isActiveDrawing;

        private Control _lastCreatedControl;

        public DrawingCanvas()
        {
            InitializeComponent();

            paintWorkspace.MouseUp += DrawingCanvas_OnMouseUp;

            var mementoDesigner = new InkCanvasMementoDesigner(paintWorkspace);
            _undoRedoCaretaker = new UndoRedoCaretaker(mementoDesigner);
            _undoRedoCaretaker.Initialize();
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
    }
}