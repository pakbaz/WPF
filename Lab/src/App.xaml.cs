using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using TodoWPFClient.Services;
using TodoWPFClient.ViewModels;

namespace TodoWPFClient;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public new static App Current => (App)Application.Current;
    public App()
    {
        Services = ConfigureServices();

        InitializeComponent();
    }
    public IServiceProvider Services { get; }

    private static IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();

        services.AddSingleton<ITodoService, TodoService>();
        services.AddTransient<TodoItemListViewModel>();
        services.AddTransient<AddTodoItemViewModel>();

        return services.BuildServiceProvider();
    }
}
