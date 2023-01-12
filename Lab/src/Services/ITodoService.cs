using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TodoWPFClient.Models;

namespace TodoWPFClient.Services;

public interface ITodoService
{
    public Task<ObservableCollection<TodoItem>> GetTodoItemsAsync(CancellationToken token);
    public Task<TodoItem> GetTodoItemAsync(string id, CancellationToken token);
    public Task CreateTodoItemAsync(TodoItem item, CancellationToken token);
    public Task ToggleCompletionTodoItemAsync(string id, bool isComplete, CancellationToken token);
    public Task DeleteTodoItemAsync(string id, CancellationToken token);
}
