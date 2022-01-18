using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using onscreen.ViewModel;
using Paint.UndoRedo;
using Paint.UndoRedo.Model;

namespace onscreen
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly IMementoCaretaker _undoRedoCaretaker;

        public MainWindow()
        {
            InitializeComponent();

            CommandBindings.Add(new CommandBinding(ApplicationCommands.Undo, OnExecutedCommands));
            paintWorkspace.MouseUp += paintWorkspace_MouseUp;

            GlobalHotKey.RegisterHotKey("Alt + Space",
                () => { Visibility = Visibility == Visibility.Visible ? Visibility.Hidden : Visibility.Visible; });

            var mementoDesigner = new InkCanvasMementoDesigner(paintWorkspace);
            _undoRedoCaretaker = new UndoRedoCaretaker(mementoDesigner);
            _undoRedoCaretaker.Initialize();
        }

        void paintWorkspace_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                _undoRedoCaretaker.StoreState();
            }
        }

        private void OnExecutedCommands(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Undo)
            {
                _undoRedoCaretaker.Undo();
            }
        }
    }
}