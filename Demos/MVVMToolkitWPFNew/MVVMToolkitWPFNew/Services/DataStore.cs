using CommunityToolkit.Mvvm.Messaging;
using MVVMToolkitWPFNew.Messages;
using System.Threading;
using System.Threading.Tasks;

namespace MVVMToolkitWPFNew.Services;

public class DataStore : IDataStore
{

    public async Task StoreUser(CancellationToken token)
    {
        WeakReferenceMessenger.Default.Send(new UserStoredMessage("Processing"));
        //Simulate Stored into DB
        await Task.Delay(5_000, token);

        WeakReferenceMessenger.Default.Send(new UserStoredMessage("User Stored"));
    }
}
