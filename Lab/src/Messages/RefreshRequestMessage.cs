using CommunityToolkit.Mvvm.Messaging.Messages;

namespace TodoWPFClient.Messages;

public class RefreshRequestMessage : ValueChangedMessage<bool>
{
    public RefreshRequestMessage(bool value) : base(value)
    {
    }
}
