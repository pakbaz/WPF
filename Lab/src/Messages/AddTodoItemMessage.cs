using CommunityToolkit.Mvvm.Messaging.Messages;
using TodoWPFClient.Models;

namespace TodoWPFClient.Messages;

public class AddTodoItemMessage : ValueChangedMessage<TodoItem>
{
    public AddTodoItemMessage(TodoItem value) : base(value)
    {
    }
}
