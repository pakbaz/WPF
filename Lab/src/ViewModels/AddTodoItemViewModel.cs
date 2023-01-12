using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using TodoWPFClient.Messages;
using TodoWPFClient.Models;
using TodoWPFClient.Services;

namespace TodoWPFClient.ViewModels;

public partial class AddTodoItemViewModel : ObservableValidator
{
    private readonly ITodoService TodoService;

    [Required(ErrorMessage = "Title Cannot be Empty")]
    [ObservableProperty]
    [NotifyDataErrorInfo]
    public string? title;

    public AddTodoItemViewModel(ITodoService todoService)
    {
        TodoService = todoService;
    }
    
    [RelayCommand(IncludeCancelCommand = true)]
    public async Task AddItem(CancellationToken cancellationToken)
    {
        ValidateAllProperties();
        if (!HasErrors)
        {
            var todo = new TodoItem { Name = Title };
            await TodoService.CreateTodoItemAsync(todo, cancellationToken);
            Title = string.Empty;
            WeakReferenceMessenger.Default.Send(new AddTodoItemMessage(todo));
        }
    }
}
