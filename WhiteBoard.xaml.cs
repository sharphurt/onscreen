﻿using System;
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
    public partial class WhiteBoard
    {
        public WhiteBoard()
        {
            InitializeComponent();

            GlobalHotKey.RegisterHotKey("Alt + Space",
                () => { Visibility = Visibility == Visibility.Visible ? Visibility.Hidden : Visibility.Visible; });

            CommandBindings.Add(new CommandBinding(ApplicationCommands.Undo, DrawingCanvas.OnExecutedCommands));
        }

        private void WhiteBoard_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            DrawingCanvas.Focus();
        }
    }
}