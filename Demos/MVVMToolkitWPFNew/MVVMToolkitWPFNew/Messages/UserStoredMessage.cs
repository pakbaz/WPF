using CommunityToolkit.Mvvm.Messaging.Messages;

namespace MVVMToolkitWPFNew.Messages;

public class UserStoredMessage : ValueChangedMessage<string>
{
    public UserStoredMessage(string value) : base(value)
    {
    }
}
