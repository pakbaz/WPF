using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_New.Models
{
    public record class Place(string Name, string State);

    public class Places : ObservableCollection<Place>
    {
        public Places()
        {
            Add(new Place("Bellevue", "WA"));
            Add(new Place("Gold Beach", "OR"));
            Add(new Place("Kirkland", "WA"));
            Add(new Place("Los Angeles", "CA"));
            Add(new Place("Portland", "ME"));
            Add(new Place("Portland", "OR"));
            Add(new Place("Redmond", "WA"));
            Add(new Place("San Diego", "CA"));
            Add(new Place("San Francisco", "CA"));
            Add(new Place("San Jose", "CA"));
            Add(new Place("Seattle", "WA"));
        }
    }
}               

