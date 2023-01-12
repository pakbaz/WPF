using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using TodoWPFClient.Messages;
using TodoWPFClient.Models;
using TodoWPFClient.Services;

namespace TodoWPFClient.ViewModels;

public partial class TodoItemListViewModel : ObservableObject, 
                                                IRecipient<AddTodoItemMessage>, 
                                                IRecipient<RefreshRequestMessage>
{
    private readonly ITodoService TodoService;

    [ObservableProperty]
    public ObservableCollection<TodoItem> todoItems;

    public TodoItemListViewModel(ITodoService todoService)
    {
        WeakReferenceMessenger.Default.RegisterAll(this);
        TodoService = todoService;
        todoItems = new();
        Task.Run(Load);
    }
    
    public async Task Load()
    {
        TodoItems = await TodoService.GetTodoItemsAsync(CancellationToken.None);
    }

    [RelayCommand(IncludeCancelCommand = true)]
    public async Task ToggleCompletion(TodoItem item, CancellationToken token)
    {
        await TodoService.ToggleCompletionTodoItemAsync(item.Id, item.IsComplete, token);
    }

    [RelayCommand(IncludeCancelCommand = true)]
    public async Task DeleteItem(TodoItem item, CancellationToken token)
    {
        await TodoService.DeleteTodoItemAsync(item.Id, token);
        TodoItems.Remove(item);
    }

    void IRecipient<AddTodoItemMessage>.Receive(AddTodoItemMessage message)
    {
        TodoItems.Add(message.Value);
    }

    void IRecipient<RefreshRequestMessage>.Receive(RefreshRequestMessage message)
    {
        Task.Run(Load);
    }
}
