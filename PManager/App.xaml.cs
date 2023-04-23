﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using PManager.View;

namespace PManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected void ApplicationStart(object sender, StartupEventArgs e)
        {
            sessionClose();
        }

        private void sessionClose()
        {

            var loginView = new LogIn();
            loginView.Show();
            loginView.IsVisibleChanged += (s, ev) =>
            {
                if (!loginView.IsVisible && loginView.IsLoaded)
                {
                    var mainView = new MainWindow();
                    mainView.Show();
                    mainView.ResizeMode = ResizeMode.CanResize;
                    loginView.Close();
                    mainView.IsVisibleChanged += (s, ev) =>
                    {
                        if (!mainView.IsVisible && mainView.IsLoaded)
                        {
                            sessionClose();
                        }
                    };
                }
            };
        }
    }
}
