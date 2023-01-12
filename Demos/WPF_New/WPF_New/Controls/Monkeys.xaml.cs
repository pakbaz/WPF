using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF_New.Controls
{
    /// <summary>
    /// Interaction logic for Monkeys.xaml
    /// </summary>
    public partial class MonkeysUserControl : UserControl
    {
        public ObservableCollection<Monkey> Monkeys { get; set; }

        public MonkeysUserControl()
        {
            InitializeComponent();
            Monkeys = new ObservableCollection<Monkey>();
            DataContext = this;
        }
        private async Task<List<Monkey>> GetMonkeysAsync()
        {
            var client = new HttpClient();
            return await client.GetFromJsonAsync<List<Monkey>>("https://raw.githubusercontent.com/jamesmontemagno/app-monkeys/master/MonkeysApp/monkeydata.json");

        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // Retrieve the list of monkeys from the JSON file
            var monkeys = await GetMonkeysAsync();

            // Add the monkeys to the Monkeys collection
            foreach (var monkey in monkeys)
            {
                Monkeys.Add(monkey);
            }
        }
    }

    public class Monkey
    {
        public string? Name { get; set; }
        public string? Location { get; set; }
        public string? Details { get; set; }
        public string? Image { get; set; }
    }
}
