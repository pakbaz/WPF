using Microsoft.Extensions.DependencyInjection;
using MVVMToolkitWPFNew.Services;
using MVVMToolkitWPFNew.ViewModels;
using System;
using System.Windows;

namespace MVVMToolkitWPFNew
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public new static App Current => (App)Application.Current;
        public App()
        {
            Services = ConfigureServices();

            this.InitializeComponent();
        }
        public IServiceProvider Services { get; }

        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            services.AddSingleton<IDataStore, DataStore>();
            services.AddTransient<MainWindowViewModel>();

            return services.BuildServiceProvider();
        }

    }
}
