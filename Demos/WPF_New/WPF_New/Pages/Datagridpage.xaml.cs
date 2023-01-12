using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using WPF_New.Models;

namespace WPF_New.Pages
{
    /// <summary>
    /// Interaction logic for datagridpage.xaml
    /// </summary>
    public partial class Datagridpage : Page
    {
        private readonly List<Item> items = new();
        public Datagridpage()
        {
            InitializeComponent();

            items.Add(new Item { Name = "Item 1", Description = "Item 1 Description" });
            items.Add(new Item { Name = "Item 2", Description = "Item 2 Description" });
            items.Add(new Item { Name = "Item 3", Description = "Item 3 Description" });
            items.Add(new Item { Name = "Item 4", Description = "Item 4 Description" });
            items.Add(new Item { Name = "Item 5", Description = "Item 5 Description" });

            dg.ItemsSource = items;
            
        }
    }
}
