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

namespace PManager.MyControls
{
    /// <summary>
    /// Lógica de interacción para ButtonDay.xaml
    /// </summary>
    public partial class ButtonDay : UserControl
    {

        public static readonly DependencyProperty BackgroundProperty =
        DependencyProperty.Register("Background", typeof(Brush), typeof(ButtonDay));

        public static readonly DependencyProperty ContentValueProperty =
        DependencyProperty.Register("ContentValue", typeof(string), typeof(ButtonDay));
        
        public static readonly DependencyProperty BorderBrushProperty =
        DependencyProperty.Register("BorderBrush", typeof(Brush), typeof(ButtonDay));
        
        public static readonly DependencyProperty ForegroundProperty =
        DependencyProperty.Register("Foreground", typeof(Brush), typeof(ButtonDay));

        public Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        public string ContentValue
        {
            get { return (string)GetValue(ContentValueProperty); }
            set { SetValue(ContentValueProperty, value); }
        }
        
        public Brush BorderBrush
        {
            get { return (Brush)GetValue(BorderBrushProperty); }
            set { SetValue(BorderBrushProperty, value); }
        }
        
        public Brush Foreground
        {
            get { return (Brush)GetValue(ForegroundProperty); }
            set { SetValue(ForegroundProperty, value); }
        }


        public ButtonDay()
        {
            InitializeComponent();
        }
    }
}
