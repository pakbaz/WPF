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

namespace WPF_New.Controls
{
    /// <summary>
    /// Interaction logic for CustomControlDemo.xaml
    /// </summary>
    public partial class CustomControlDemo : UserControl
    {
        public event EventHandler<ColorRoutedEventArgs> ColorChanged;
        public CustomControlDemo()
        {
            InitializeComponent();
            colorPicker.NewColorCustom += ColorPicker_NewColorCustom;
        }

        private void ColorPicker_NewColorCustom(object sender, ColorRoutedEventArgs e)
        {
            ColorChanged(this, e);
        }
    }
}
