using CommunityToolkit.Mvvm.Messaging;
using System.Windows;
using TodoWPFClient.Messages;

namespace TodoWPFClient;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        WeakReferenceMessenger.Default.Send(new RefreshRequestMessage(true));
    }
}
