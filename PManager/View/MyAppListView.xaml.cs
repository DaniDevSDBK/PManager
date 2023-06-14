using PManager.Repositorios;
using System;
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

namespace PManager.View
{
    /// <summary>
    /// Lógica de interacción para MyAppListView.xaml
    /// </summary>
    public partial class MyAppListView : UserControl
    {

        private UserContext user = UserContext.Instance;

        public MyAppListView()
        {
            InitializeComponent();
        }
    }
}
