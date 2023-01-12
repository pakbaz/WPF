using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MVVMToolkitWPFNew.Messages;
using MVVMToolkitWPFNew.Services;
using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MVVMToolkitWPFNew.ViewModels;

[ObservableObject]
public partial class MainWindowViewModel : IRecipient<UserStoredMessage>
{
    private readonly IDataStore dataStore;
    public MainWindowViewModel(IDataStore dataStore)
    {
        WeakReferenceMessenger.Default.Register(this);
        this.dataStore = dataStore;
    }


    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ClickCommand))]
    public string? firstName;

    [RelayCommand]
    private async Task Click()
    {
        await Task.Delay(500);
        FirstName = "Hello World";

    }
    
    [RelayCommand(IncludeCancelCommand = true)]
    private async Task Store(CancellationToken cancellationToken)
    {
        try
        {
            await dataStore.StoreUser(cancellationToken);
        }
        catch (OperationCanceledException)
        {
            FirstName = "Cancelled";
        }
        
    }

    public void Receive(UserStoredMessage message)
    {
        FirstName = message.Value;
    }
}
