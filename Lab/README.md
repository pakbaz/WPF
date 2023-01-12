# Create a WPF Application that Works with Remote API

## Summary

This workshop shows you how to create your own Desktop application in WPF (Windows Presentation Foundation).
you will use a local or remote API to pull and update information, create layout and interfaces for capturing and displaying remote data, create reusable UI component and events with User Controls while taking advantage of coding best practices such as MVVM, Dependency Injection and Single Responsibility principle. We are going to Use Community Toolkit MVVM Library which is standard in .NET apps.


## Languages
C# and XAML

## Prerequisites
Visual Studio 2022 and Latest .NET

## Author
https://github.com/pakbaz


## Part 1: Fetch and Display Data from remote API


### Task 1 - Setup or Access Online API


For API Backend, we will be using a simple Todo Items Rest API based on [This public docker image](https://hub.docker.com/repository/docker/pakbaz82/demo-restapi-todo). The instructor will give you a URL for a hosted service based on that image or alternatively you can host your own API using that image. You can also host it locally using docker desktop by running the following command:
  
```bash
  docker run -d -p 8080:80 pakbaz82/demo-restapi-todo:latest
```
  
This will run the API locally at port 8080. Feel free to change that port. Access the API by going to:
  
```
  localhost:8080/swagger 

  or

  https://Onlinehost/swagger
```

If you see API definition page, your API has been setup successfully
 
### Task 2 - Explore the API and Add todo Items

Use swagger tool as API document to make some requests and add data to use later. Feel free to browse code and API signature to get familiar with it. Use swagger to enter some todo Items

![image](https://user-images.githubusercontent.com/4333815/211395901-49ff9640-bee8-4c23-9a77-a08172a3ddf8.png)

then use swagger to query todo items. make sure you add at least one item.

![image](https://user-images.githubusercontent.com/4333815/211396592-9b32de9e-ec9a-4954-9a71-bc1d6406b48c.png)



### Task 3 - Setup WPF App

1. Open Visual Studio 2022 and create a new project for WPF Application. If you can't find it make sure run Visual Studio Setup and add .NET Desktop Development Workload.

    ![image](https://user-images.githubusercontent.com/4333815/211169158-fbb522ed-6f60-408c-bc81-a723209d3a3c.png)

2. After Creating the application, Run WPF app once to make sure everything works. You should see a Blank Window Only

3. Add Required Packages either through Console or nuget explorer:

```shell
  dotnet add Microsoft.Extensions.DependencyInjection
  dotnet add CommunityToolkit.Mvvm
```

4. Add four folders to solution as follows **Services**, **Models**, **Views**, **ViewModels**



5. Add Application Configuration file **App.Config** to Project and Modify it to Add the API Url as shown below:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
	<appSettings>
		<add key="ApiAddress" value="[replace with your API url]"/>
	</appSettings>
</configuration>
```

### Task 4 - Setup the Service for getting Todo Items and Dependencies

1. Under Models folder create a **Todo** Class as below:

```cs
  public class TodoItem
  {
      public string Id { get; set; }  = Guid.NewGuid().ToString();
      public string? Name { get; set; }
      public bool IsComplete { get; set; }
  }
```

2. Under Services folder creare an **ITodoService** interface as below:

```cs
  public interface ITodoService
  {
	public Task<ObservableCollection<TodoItem>> GetTodoItemsAsync(CancellationToken token);
	public Task<TodoItem> GetTodoItemAsync(string id, CancellationToken token);
	public Task CreateTodoItemAsync(TodoItem item, CancellationToken token);
	public Task ToggleCompletionTodoItemAsync(string id, bool isComplete, CancellationToken token);
	public Task DeleteTodoItemAsync(string id, CancellationToken token);
  }
```

3. Create a service class **TodoService** that implements the interface we just defined in the same folder/namespace:

```cs
  public class TodoService : ITodoService
  {
      public async Task<ObservableCollection<TodoItem>> GetTodoItemsAsync(CancellationToken token)
      {
          throw new NotImplementedException();
      }

      public async Task ToggleCompletionTodoItemAsync(string id, bool isComplete, CancellationToken token)
      {
          throw new NotImplementedException();
      }

      public async Task CreateTodoItemAsync(TodoItem item, CancellationToken token)
      {
          throw new NotImplementedException();
      }

      public async Task DeleteTodoItemAsync(string id, CancellationToken token)
      {
          throw new NotImplementedException();
      }

      public async Task<TodoItem> GetTodoItemAsync(string id, CancellationToken token)
      {
          throw new NotImplementedException();
      }

  }
```

4. Implement just GetTodoItemsAsync as below:

```cs
public class TodoService : ITodoService
{
    HttpClient _client;
    JsonSerializerOptions _serializerOptions;
    public TodoService()
    {
        string baseUrl = ConfigurationManager.AppSettings["ApiAddress"] ?? string.Empty;
        _client = new HttpClient
        {
            BaseAddress = new Uri(baseUrl)
        };
        _serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
    }
    public async Task<ObservableCollection<TodoItem>> GetTodoItemsAsync(CancellationToken token)
    {
        var items = new ObservableCollection<TodoItem>();
        
        try
        {
            HttpResponseMessage response = await _client.GetAsync("TodoItems", token);
            if (response.IsSuccessStatusCode)
            {
                items = await response.Content.ReadFromJsonAsync<ObservableCollection<TodoItem>>(_serializerOptions,token);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(@"\tERROR {0}", ex.Message);
        }

        return items ?? new ObservableCollection<TodoItem>();
    }
    
    public async Task ToggleCompletionTodoItemAsync(string id, bool isComplete, CancellationToken token)
    {
        throw new NotImplementedException();
    }

    public async Task CreateTodoItemAsync(TodoItem item, CancellationToken token)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteTodoItemAsync(string id, CancellationToken token)
    {
        throw new NotImplementedException();
    }

    public async Task<TodoItem> GetTodoItemAsync(string id, CancellationToken token)
    {
        throw new NotImplementedException();
    }
    
}
```

5. Open **App.Xaml.cs** File (code behind file of App.Xaml which is the main Entry to our app), Add singleton dependency to ToDoService and expose Application as a static object. Eventually, it will look like this:

```cs
	public partial class App : Application
	{
	    public new static App Current => (App)Application.Current;
	    public App()
	    {
		Services = ConfigureServices();

		InitializeComponent();
	    }
	    public IServiceProvider Services { get; }

	    private static IServiceProvider ConfigureServices()
	    {
		var services = new ServiceCollection();

		services.AddSingleton<ITodoService, TodoService>();

		return services.BuildServiceProvider();
	    }
	}
```

### Task 5 - Create View and ViewModel for Todo List

1. Under ViewModels folder create a new class called **TodoItemListViewModel.cs** as below:

```cs
public partial class TodoItemListViewModel : ObservableObject
{
    private readonly ITodoService TodoService;

    [ObservableProperty]
    public ObservableCollection<TodoItem> todoItems;

    public TodoItemListViewModel(ITodoService todoService)
    {
        TodoService = todoService;
        todoItems = new();
        Task.Run(Load);
    }

    public async Task Load()
    {
        TodoItems = await TodoService.GetTodoItemsAsync(CancellationToken.None);
    }

}

```

> **Remarks:** ObservableObject base class is in CommunityToolkit.Mvvm.ComponentModel namespace and is used to provide code generations for ObservableProperties and other RelayCommands which is why class must be partial. notice that we put ObservableProperty on a field variable because it will generate the property associated with it automatically using MVVM Community Toolkit source generators.

2. Add the ViewModel in Application DI Container as Transient under ConfigureServices method after the TodoService definition:

```cs
services.AddTransient<TodoItemListViewModel>();
```

3. Under Views Create a new WPF User Control and call it TodoItemsList.xaml. Replace the XAML code with the code below:

```xml
<UserControl x:Class="TodoWPFClient.Views.TodoItemsList"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006" 
             xmlns:d="http://schemas.microsoft.com/expression/blend/2008" 
             d:DataContext="{d:DesignInstance Type=viewmodels:TodoItemListViewModel}"
             xmlns:local="clr-namespace:TodoWPFClient.Views"
             xmlns:viewmodels="clr-namespace:TodoWPFClient.ViewModels"
             mc:Ignorable="d" d:DesignHeight="450" d:DesignWidth="800">
    <ListView ItemsSource="{Binding TodoItems}" HorizontalContentAlignment="Stretch" ScrollViewer.VerticalScrollBarVisibility="Auto">
        <ListView.ItemTemplate>
            <DataTemplate>
                <Grid>
                    <Grid.ColumnDefinitions>
                        <ColumnDefinition Width="50"/>
                        <ColumnDefinition Width="*"/>
                    </Grid.ColumnDefinitions>
                    <CheckBox ToolTip="Toggle Item's Done State" Grid.Column="0" VerticalAlignment="Center" Width="20" Height="20" 
                               IsChecked="{Binding IsComplete}" />
                    <TextBlock Text="{Binding Name}" Grid.Column="1" VerticalAlignment="Center" FontSize="20" />
                </Grid>
            </DataTemplate>
        </ListView.ItemTemplate>
    </ListView>
</UserControl>

```

> **Remarks:** Make sure to replace the namespace in x:Class and other Xml namespaces to match your solution's namespace. also, Adding d:DataContext and making a reference to ViewModel will provide better IntelliSense and design time feedback in XAML file

4. In the code behind file of TodoItemsList.xaml.cs add TodoItemListViewModel as DataContext using DI container we setup earlier. You must add it in the constructor right after InitializeComponent method:

```cs
DataContext = App.Current.Services.GetService<TodoItemListViewModel>();
```

5. In the MainWindow.xaml add the view to display data as below:

```xml
<Window x:Class="TodoWPFClient.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        xmlns:local="clr-namespace:TodoWPFClient"
        xmlns:views="clr-namespace:TodoWPFClient.Views"
        mc:Ignorable="d"
        Title="Todo Items WPF Client" Height="450" Width="800">
    <Grid>
        <views:TodoItemsList />
    </Grid>
</Window>

```

6. Run the application. You should be able to see List items you added earlier using swagger.

## Part 2: Create Todo Item Client

### Task 1 - Implement CreateTodoItemAsync in TodoService class:

open TodoService class under services folder and update CreateTodoItemAsync as below:

```cs
    public async Task CreateTodoItemAsync(TodoItem item, CancellationToken token)
    {
        try
        {
            var response = await _client.PostAsJsonAsync("TodoItems", item, token);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(@"\tERROR {0}", ex.Message);
        }
    }
```

### Task 2 - Create ViewModel for adding Todo Items

Under ViewModels folder create a new class and name it AddTodoItemViewModel as below:

```cs
public partial class AddTodoItemViewModel : ObservableValidator
{
    private readonly ITodoService TodoService;

    [Required]
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
        }
    }
}
```

> **Remarks:** Note that this View Model inherits from ObservableValidator which itself inherits from ObservableObject but since this is for capturing information we want to perform validation and need INotifyDataErrorInfo which is precisely why we use this base class instead. Data validation is so simple in MVVM CommunityToolkit thanks to source generators. all you need is validation attribute from System.ComponentModel.DataAnnotations and NotifyDataErrorInfo from CommunityToolkit.Mvvm.ComponentModel namespaces. Other thing is the RelayCommand attribute which generates appropriate commands and routes optional parameters and cancellation token into the custom defined method which is an Async AddItem method that we created here.


Add the View Model to DI container in *App.xaml.cs* like the TodoItemListViewModel. The ConfigureServices method should look like this:

```cs
    private static IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();

        services.AddSingleton<ITodoService, TodoService>();
        services.AddTransient<TodoItemListViewModel>();
        services.AddTransient<AddTodoItemViewModel>();

        return services.BuildServiceProvider();
    }
```

### Task 3 - Create View for adding Todo Items

Under Views folder create AddTodoItem User Control for WPF and replace the XAML with following:

```xml
<UserControl x:Class="TodoWPFClient.Views.AddTodoItem"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006" 
             xmlns:d="http://schemas.microsoft.com/expression/blend/2008" 
             d:DataContext="{d:DesignInstance Type=viewmodels:AddTodoItemViewModel}"
             xmlns:local="clr-namespace:TodoWPFClient.Views"
             xmlns:viewmodels="clr-namespace:TodoWPFClient.ViewModels"
             mc:Ignorable="d" 
             d:DesignHeight="50" d:DesignWidth="800">
    
    <StackPanel Orientation="Horizontal">
        <TextBlock Text="Add Todo Item:" FontSize="20" FontWeight="ExtraBold" Margin="0,0,10,0" VerticalAlignment="Center"/>
        <TextBox Width="300" Height="50" Padding="10" FontSize="20" 
                     Text="{Binding Title, Mode=TwoWay,UpdateSourceTrigger=PropertyChanged}" />
        <Button Width="120" Content="Add Item" Padding="10" FontWeight="SemiBold" Height="50" FontSize="20" Command="{Binding AddItemCommand}" />
    </StackPanel> 
</UserControl>
    

```

> **Remarks:** The reason we added PropertyChanged UpdateSourceTrigger for binding textbox is because we want validation to take place upon every keystroke for Textbox rather than focus out event which in this case don't make sense because we only have one textbox

Now bind the DataContext in code behind to the View Model we created for AddTodoItem by adding this line:

```cs
DataContext = App.Current.Services.GetService<AddTodoItemViewModel>();
```

### Task 4 - Update Main Window to include add Item and Run the application

Now Update the main window to include both views for add and displaying the content as below:

```xml
<Window x:Class="TodoWPFClient.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        xmlns:local="clr-namespace:TodoWPFClient"
        xmlns:views="clr-namespace:TodoWPFClient.Views"
        mc:Ignorable="d"
        Title="Todo Items WPF Client" Height="450" Width="800">
    <Grid>
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="*"/>
        </Grid.RowDefinitions>
        <views:AddTodoItem Grid.Row="0" Margin="10" />
        <views:TodoItemsList Grid.Row="1" Margin="10" />
    </Grid>
</Window>

```

Now run the application. you can try adding Todo Items and you can use Swagger to confirm that adding happens; However, the list doesn't update automatically which is a bad user experience.

## Part 3: View Model Communication using Messaging

MVVM Community toolkit, like many other MVVM frameworks, has built-in capability for messaging. The messaging can be sent using WeakReferenceMessenger or StrongReferenceMessenger. strong reference provides better performance but even WeakReferenceMessenger provides excellent performance compared to other frameworks as [described here](https://devblogs.microsoft.com/dotnet/announcing-the-dotnet-community-toolkit-800/).
For our needs we are going to use WeakReferenceMessenger which is easier.

### Task 1 - Create Message Signature

Add a folder called **Messages** and add a class called **AddTodoItemMessage**. Class should derive from ValueChangedMessage<T> that is in CommunityToolkit.Mvvm.Messaging.Messages namespace and T will be our TodoItem model that we already have. We must implement the constructor, but we don't need to add anything else in this class. It should look like this:

```cs
public class AddTodoItemMessage : ValueChangedMessage<TodoItem>
{
    public AddTodoItemMessage(TodoItem value) : base(value)
    {
    }
}
```

### Task 2 - Send Message from Source

Update the AddTodoItemViewModel and update AddItem method to look like this:

```cs 
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
```

> WeakReferenceMessenger is in CommunityToolkit.Mvvm.Messaging namespace and AddTodoItemMessage is the Message class that we just created

### Task 3 - Handle Message

Open TodoItemListViewModel class and add the register method in the constructor to notify we are going to receive message as below:

```cs
WeakReferenceMessenger.Default.RegisterAll(this);
```

Then we have the class Implement IRecipient<T> interface which T is AddTodoItemMessage that we defined earlier. It will add a method which we use to add our Todo Item:

```cs
void IRecipient<AddTodoItemMessage>.Receive(AddTodoItemMessage message)
{
   TodoItems.Add(message.Value);
}
```

the class should look like this:

```cs

public partial class TodoItemListViewModel : ObservableObject, IRecipient<AddTodoItemMessage>
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
    
    ...

    void IRecipient<AddTodoItemMessage>.Receive(AddTodoItemMessage message)
    {
        TodoItems.Add(message.Value);
    }
}
```

### Task 4 - Implement Refresh Button for the Todo List

Update the main window XAML to look like below:

```xml
<Window x:Class="TodoWPFClient.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        xmlns:local="clr-namespace:TodoWPFClient"
        xmlns:views="clr-namespace:TodoWPFClient.Views"
        mc:Ignorable="d"
        Title="Todo Items WPF Client" Height="450" Width="800">
    <Grid>
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="*"/>
        </Grid.RowDefinitions>
        <Grid.ColumnDefinitions>
            <ColumnDefinition Width="Auto"/>
            <ColumnDefinition Width="*" />
        </Grid.ColumnDefinitions>
        <Button Width="80" Height="50" Grid.Row="0" Grid.Column="1" Content="Refresh" />
        <views:AddTodoItem Grid.Row="0" Margin="10" />
        <views:TodoItemsList Grid.Row="1" Grid.ColumnSpan="2" Margin="10" />
    </Grid>
</Window>

```


use blow Message signature to create a message and receive it by TodoItemListViewModel to refresh the list:

```cs
public class RefreshRequestMessage : ValueChangedMessage<bool>
{
    public RefreshRequestMessage(bool value) : base(value)
    {
    }
}
```

## Part 4: Adding Update and Delete Capability

### Task 1: Update Todo Item Completion

1. Open TodoService class and let's implement ToggleCompletionTodoItemAsync as below:


```cs
    public async Task ToggleCompletionTodoItemAsync(string id, bool isComplete, CancellationToken token)
    {
        try
        {
            string queryParam = isComplete ? "?isComplete=true" : "?isComplete=false";
            HttpResponseMessage response = await _client.PatchAsync($"TodoItems/{id}{queryParam}", null, token);
            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine(@"\tTodoItem successfully completed.");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(@"\tERROR {0}", ex.Message);
        }
    }
```

2. Update the TodoItemsList.Xaml file to include toggle Command and include CommandParameter too. Checkbox markp should look like this:

```xml
<CheckBox ToolTip="Toggle Item's Done State" Grid.Column="0" VerticalAlignment="Center" Width="20" Height="20" IsChecked="{Binding IsComplete}" Command="{Binding DataContext.ToggleCompletionCommand, RelativeSource={RelativeSource AncestorType=ListView}}" CommandParameter="{Binding}" />
```

> **Remarks:** We need to include the CommandParameter to give enough information to ViewModel method to update Datasource


3. Finally, add the method for completing Todo Item in TodoItemListViewModel class. Luckily, thanks to MVVM Community toolkit this is extremely easy as it does most of the hardwork for us. we just need to add an Async method to capture Todo Item as below:

```cs
    [RelayCommand(IncludeCancelCommand = true)]
    public async Task ToggleCompletion(TodoItem item, CancellationToken token)
    {
        await TodoService.ToggleCompletionTodoItemAsync(item.Id, item.IsComplete, token);
    }
```

### Task 2 - Deleting Todo Item

1. Open TodoService class and let's implement DeleteTodoItemAsync as below:

```cs
    public async Task DeleteTodoItemAsync(string id, CancellationToken token)
    {
        try
        {
            HttpResponseMessage response = await _client.DeleteAsync($"TodoItems/{id}", token);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(@"\tERROR {0}", ex.Message);
        }
    }
```

2. Update the TodoItemsList.Xaml file to include delete button. final Xaml should look like below:

```xml
<UserControl x:Class="TodoWPFClient.Views.TodoItemsList"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006" 
             xmlns:d="http://schemas.microsoft.com/expression/blend/2008" 
             d:DataContext="{d:DesignInstance Type=viewmodels:TodoItemListViewModel}"
             xmlns:local="clr-namespace:TodoWPFClient.Views"
             xmlns:viewmodels="clr-namespace:TodoWPFClient.ViewModels"
             mc:Ignorable="d" d:DesignHeight="450" d:DesignWidth="800">
    <ListView ItemsSource="{Binding TodoItems}" HorizontalContentAlignment="Stretch" ScrollViewer.VerticalScrollBarVisibility="Auto">
        <ListView.ItemTemplate>
            <DataTemplate>
                <Grid>
                    <Grid.ColumnDefinitions>
                        <ColumnDefinition Width="50"/>
                        <ColumnDefinition Width="*"/>
                        <ColumnDefinition Width="60"/>
                    </Grid.ColumnDefinitions>
                    <CheckBox ToolTip="Toggle Item's Done State" Grid.Column="0" VerticalAlignment="Center" Width="20" Height="20" IsChecked="{Binding IsComplete}" 
                              Command="{Binding DataContext.ToggleCompletionCommand, RelativeSource={RelativeSource AncestorType=ListView}}" CommandParameter="{Binding}" />
                    <TextBlock Text="{Binding Name}" Grid.Column="1" Opacity="{Binding IsComplete}" VerticalAlignment="Center" FontSize="20" />
                    <Button ToolTip="Delete Item" Content="X" FontWeight="ExtraBold" Grid.Column="2" VerticalAlignment="Center" Width="50" 
                            Command="{Binding DataContext.DeleteItemCommand, RelativeSource={RelativeSource AncestorType=ListView}}" CommandParameter="{Binding}" />
                </Grid>
            </DataTemplate>
        </ListView.ItemTemplate>
    </ListView>
</UserControl>

```

> **Remarks:** Make sure to update namespaces and feel free to enhance the look and feel of the controls in the list.

3. Finally, add the method for deleting Todo Item in TodoItemListViewModel class. Luckily, thanks to MVVM Community toolkit this is extremely easy as it does most of the hardwork for us. we just need to add an Async method to capture Todo Item as below:

```cs
    [RelayCommand(IncludeCancelCommand = true)]
    public async Task DeleteItem(TodoItem item, CancellationToken token)
    {
        await TodoService.DeleteTodoItemAsync(item.Id, token);
        TodoItems.Remove(item);
    }
```

## Part 5: Enhance Application by styles, Converters and Other feedbacks

### Task 1 - Adding Styles

Application styles mostly go in App.Xaml Under Application.Resources node so it can be shared across the application. You can also have specific style for a window or even user control or element, but we will be adding it in App.Xaml.

For changing the look and feel of all buttons in our application, we can change control template of the button as below:

```xml
<Style TargetType="Button">
    <Setter Property="Background" Value="#fff"/>
    <Setter Property="Foreground" Value="#000"/>
    <Setter Property="FontSize" Value="15"/>
    <Setter Property="Margin" Value="5"/>
    <Setter Property="Template">
        <Setter.Value>
            <ControlTemplate TargetType="Button">
                <Border Background="{TemplateBinding Background}"
                        CornerRadius="5"
                        BorderThickness="1"
                        Padding="5"
                        BorderBrush="#000">
                    <ContentPresenter HorizontalAlignment="Center" VerticalAlignment="Center">

                    </ContentPresenter>
                </Border>
            </ControlTemplate>
        </Setter.Value>
    </Setter>
    <Style.Triggers>
        <Trigger Property="IsMouseOver" Value="True">
            <Setter Property="Background" Value="DarkGray"/>
            <Setter Property="Foreground" Value="#fff"/>
        </Trigger>
    </Style.Triggers>
</Style>
```


For Displaying something when all Todo Items are deleted, we can add style trigger for ListView in Application Resources:

```xml
        <Style TargetType="ListView">
            <Style.Triggers>
                <!-- Use ListBox.HasItems instead of Binding -->
                <Trigger Property="HasItems" Value="False">
                    <Setter Property="Template">
                        <Setter.Value>
                            <ControlTemplate>
                                <TextBlock FontSize="20" FontWeight="ExtraBold" Margin="50" HorizontalAlignment="Center" VerticalAlignment="Center">No items to display</TextBlock>
                            </ControlTemplate>
                        </Setter.Value>
                    </Setter>
                </Trigger>
            </Style.Triggers>
        </Style>
```

Finally, we can change the look and feel of the Checkbox to something completely different. this is an example:

```xml
<Style TargetType="CheckBox" >
    <Setter Property="Cursor" Value="Hand"></Setter>
    <Setter Property="Content" Value=""></Setter>
    <Setter Property="Template">
        <Setter.Value>
            <ControlTemplate TargetType="{x:Type CheckBox}">
                <Grid>
                    <Ellipse x:Name="outerEllipse">
                        <Ellipse.Fill>
                            <RadialGradientBrush>
                                <GradientStop Offset="0" Color="Red"/>
                                <GradientStop Offset="0.88" Color="LightCoral"/>
                                <GradientStop Offset="1" Color="DarkRed"/>
                            </RadialGradientBrush>
                        </Ellipse.Fill>
                    </Ellipse>
                    <Ellipse Margin="10" x:Name="highlightCircle" >
                        <Ellipse.Fill >
                            <LinearGradientBrush >
                                <GradientStop Offset="0" Color="Green"/>
                                <GradientStop Offset="0.5" Color="LightGreen"/>
                                <GradientStop Offset="1" Color="DarkGreen"/>
                            </LinearGradientBrush>
                        </Ellipse.Fill>
                    </Ellipse>
                    <ContentPresenter x:Name="content" HorizontalAlignment="Center" VerticalAlignment="Center"/>
                </Grid>
                <ControlTemplate.Triggers>
                    <Trigger Property="IsChecked" Value="True">
                        <Setter TargetName="highlightCircle" Property="Fill">
                            <Setter.Value>
                                <LinearGradientBrush StartPoint="0.3,0" EndPoint="0.7,1">
                                    <GradientStop Offset="0" Color="Green"/>
                                    <GradientStop Offset="0.5" Color="LightGreen"/>
                                    <GradientStop Offset="1" Color="DarkGreen"/>
                                </LinearGradientBrush>
                            </Setter.Value>
                        </Setter>
                        <Setter TargetName="outerEllipse" Property="Fill">
                            <Setter.Value>
                                <RadialGradientBrush>
                                    <GradientStop Offset="0" Color="Green"/>
                                    <GradientStop Offset="0.88" Color="LightGreen"/>
                                    <GradientStop Offset="1" Color="DarkGreen"/>
                                </RadialGradientBrush>
                            </Setter.Value>
                        </Setter>
                    </Trigger>
                    <Trigger Property="IsChecked" Value="False">
                        <Setter TargetName="highlightCircle" Property="Fill">
                            <Setter.Value>
                                <LinearGradientBrush StartPoint="0.3,0" EndPoint="0.7,1">
                                    <GradientStop Offset="0" Color="Red"/>
                                    <GradientStop Offset="0.5" Color="LightCoral"/>
                                    <GradientStop Offset="1" Color="DarkRed"/>
                                </LinearGradientBrush>
                            </Setter.Value>
                        </Setter>
                    </Trigger>
                </ControlTemplate.Triggers>
            </ControlTemplate>
        </Setter.Value>
    </Setter>
</Style>
```

### Task 2 - Adding Converters:

1. Create a folder called **Converters** and make two converters for Boolean data types in our binding. one to Change Binding to Opacity and other to change Binding to Visibility to use in Xaml Binding. These two classes are defined as below:

```cs
public class BoolToOpacityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if((bool)value)
        {
            return 0.07;
        }
        else
        {
            return 1.0;
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
```

> **Remarks:** IValueConverter is the interface that implements two methods of Convert and ConvertBack. We will not need to use ConvertBack as that one is used for two-way binding situations.

```cs
public class BoolToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (bool)value ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
```

2. Add Converters to Application Resources Dictionary (same place you added styles). First you need to add XML namespace for the Converter folder and then add them as Static Resource and give them a key as below:

```xml
<Application x:Class="TodoWPFClient.App"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:local="clr-namespace:TodoWPFClient"
             xmlns:convert="clr-namespace:TodoWPFClient.Converters"
             StartupUri="MainWindow.xaml">
    <Application.Resources>
        <convert:BoolToOpacityConverter x:Key="CompletedConverter"/>
        <convert:BoolToVisibilityConverter x:Key="boolConvertor"/>
	
	....
```

3. Modify TodoItemsList.Xaml to make Text style change whenever Todo Item is complete:

```xml
<UserControl x:Class="TodoWPFClient.Views.TodoItemsList"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006" 
             xmlns:d="http://schemas.microsoft.com/expression/blend/2008" 
             d:DataContext="{d:DesignInstance Type=viewmodels:TodoItemListViewModel}"
             xmlns:local="clr-namespace:TodoWPFClient.Views"
             xmlns:viewmodels="clr-namespace:TodoWPFClient.ViewModels"
             mc:Ignorable="d" d:DesignHeight="450" d:DesignWidth="800">
    <ListView ItemsSource="{Binding TodoItems}" HorizontalContentAlignment="Stretch" ScrollViewer.VerticalScrollBarVisibility="Auto">
        <ListView.ItemTemplate>
            <DataTemplate>
                <Grid>
                    <Grid.ColumnDefinitions>
                        <ColumnDefinition Width="50"/>
                        <ColumnDefinition Width="*"/>
                        <ColumnDefinition Width="60"/>
                    </Grid.ColumnDefinitions>
                    <CheckBox ToolTip="Toggle Item's Done State" Grid.Column="0" VerticalAlignment="Center" Width="20" Height="20" IsChecked="{Binding IsComplete}" 
                              Command="{Binding DataContext.ToggleCompletionCommand, RelativeSource={RelativeSource AncestorType=ListView}}" CommandParameter="{Binding}" />
                    <TextBlock Text="{Binding Name}" Grid.Column="1" Opacity="{Binding IsComplete, Converter={StaticResource CompletedConverter}}" VerticalAlignment="Center" FontSize="20" />
                    <Button ToolTip="Delete Item" Content="X" FontWeight="ExtraBold" Grid.Column="2" VerticalAlignment="Center" Width="50" 
                            Command="{Binding DataContext.DeleteItemCommand, RelativeSource={RelativeSource AncestorType=ListView}}" CommandParameter="{Binding}" />
                </Grid>
            </DataTemplate>
        </ListView.ItemTemplate>
    </ListView>
</UserControl>

```

### Task 3 - Adding Progress Bar and Validation Message:

Modify the AddTodoItem.xaml to include the validation message and progress bar and control their visibility using BoolToVisibility Converter we created on previous Task. The final code will be as below:

```xml
<UserControl x:Class="TodoWPFClient.Views.AddTodoItem"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006" 
             xmlns:d="http://schemas.microsoft.com/expression/blend/2008" 
             d:DataContext="{d:DesignInstance Type=viewmodels:AddTodoItemViewModel}"
             xmlns:local="clr-namespace:TodoWPFClient.Views"
             xmlns:viewmodels="clr-namespace:TodoWPFClient.ViewModels"
             mc:Ignorable="d" 
             d:DesignHeight="50" d:DesignWidth="800">
    
    <StackPanel Orientation="Horizontal">
        <TextBlock Text="Add Todo Item:" FontSize="20" FontWeight="ExtraBold" Margin="0,0,10,0" VerticalAlignment="Center"/>
        <Grid>
            <TextBox Width="300" Height="50" Padding="10" FontSize="20" 
                     Text="{Binding Title, Mode=TwoWay,UpdateSourceTrigger=PropertyChanged}" />
            <TextBlock Width="100" VerticalAlignment="Center" FontSize="10" Text="Title Cannot be Empty" Foreground="Red" 
                       Visibility="{Binding HasErrors, Converter={StaticResource boolConvertor}}" />
        </Grid>
        <Grid>
            <Button Width="120" Content="Add Item" Padding="10" FontWeight="SemiBold" Height="50" FontSize="20" Command="{Binding AddItemCommand}" />
            <ProgressBar Width="120" Height="10" VerticalAlignment="Top" Visibility="{Binding AddItemCommand.IsRunning, Converter={StaticResource boolConvertor}}" IsIndeterminate="True" />
        </Grid>
    </StackPanel> 
</UserControl>
    
```

Finally, we can add Property trigger style for our TextBox or any other future textboxes to display tooltip that contain validation message that is generated for that input. Add following style to App.Xaml 

```xml
<Style TargetType="TextBox">
    <Setter Property="FontSize" Value="20"/>
    <Setter Property="Padding" Value="10"/>
    <Setter Property="Height" Value="50"/>
    <Setter Property="Width" Value="300"/>
    <Style.Triggers>
        <Trigger Property="Validation.HasError" Value="True">
            <Setter Property="ToolTip" Value="{Binding RelativeSource={RelativeSource Self}, Path=(Validation.Errors)[0].ErrorContent}"/>
        </Trigger>
    </Style.Triggers>
</Style>
```

Feel free to modify the code to enhance style or incorporate Ideas.