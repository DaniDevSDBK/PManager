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
    /// Lógica de interacción para HelpView.xaml
    /// </summary>
    public partial class HelpView : UserControl
    {
        public HelpView()
        {
            InitializeComponent();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Hyperlink hp = (Hyperlink)sender;

            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {

                FileName = hp.NavigateUri.ToString(),
                UseShellExecute = true
            });
        }
    }
}
