﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DestinyNetViews
{
    /// <summary>
    /// Interaction logic for TreeView.xaml
    /// </summary>
    public partial class ToDoView : UserControl
    {
        public ToDoView()
        {
            InitializeComponent();
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //var window = Window.GetWindow(this);
            //window.KeyDown += HandleKeyPress;
            //window.KeyUp += HandleKeyPress;
        }
    }
}
