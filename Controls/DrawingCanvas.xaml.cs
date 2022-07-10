using System;
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
                Console.WriteLine(CurrentTool.ToolType);
            }
            base.OnPropertyChanged(e);
        }
        
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
            CurrentTool.Process(this, new DrawingProperties(e.GetPosition(this)));
        }
    }
}