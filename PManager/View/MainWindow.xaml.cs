﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void pnlControlBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)=> DragMove();

        private void btnCloseWindow_Click(object sender, RoutedEventArgs e)=> this.Close();

        private void btnMaximizedWindow_Click(object sender, RoutedEventArgs e)
        {

           if(!this.WindowState.Equals(WindowState.Maximized)) {

                this.WindowState = WindowState.Maximized;
            }
            else
            {
                this.WindowState = WindowState.Normal;
            }
        }

        private void btnMinimizedWindow_Click(object sender, RoutedEventArgs e)=> this.WindowState = WindowState.Minimized;
    }
}
