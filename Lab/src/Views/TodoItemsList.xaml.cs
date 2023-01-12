using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;
using TodoWPFClient.Models;
using TodoWPFClient.ViewModels;

namespace TodoWPFClient.Views;

/// <summary>
/// Interaction logic for TodoItemsList.xaml
/// </summary>
public partial class TodoItemsList : UserControl
{
    public TodoItemsList()
    {
        InitializeComponent();
        DataContext = App.Current.Services.GetService<TodoItemListViewModel>();
    }
}
