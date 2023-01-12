using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.IO.Packaging;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using TodoWPFClient.Models;

namespace TodoWPFClient.Services;

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

    public async Task CreateTodoItemAsync(TodoItem item, CancellationToken token)
    {
        try
        {
            await Task.Delay(3000, token);
            var response = await _client.PostAsJsonAsync("TodoItems", item, token);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(@"\tERROR {0}", ex.Message);
        }
    }

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

    public async Task<TodoItem> GetTodoItemAsync(string id, CancellationToken token)
    {
        TodoItem? item = new();
        try
        {
            HttpResponseMessage response = await _client.GetAsync($"TodoItems/{id}", token);
            if (response.IsSuccessStatusCode)
            {
                item = await response.Content.ReadFromJsonAsync<TodoItem>(_serializerOptions, token);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(@"\tERROR {0}", ex.Message);
        }

        return item ?? new TodoItem();
    }
    
}
