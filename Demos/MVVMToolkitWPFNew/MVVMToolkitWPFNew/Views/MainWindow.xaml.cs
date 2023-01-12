using Microsoft.Extensions.DependencyInjection;
using MVVMToolkitWPFNew.ViewModels;
using System.Windows;

namespace MVVMToolkitWPFNew.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            DataContext = App.Current.Services.GetService<MainWindowViewModel>();
        }
    }
}
    