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

namespace WPF_New.Controls
{
    #region ColorPickerControl CLASS
    /// <summary>
    /// A simple color picker control, with a custom event that uses
    /// the standard RoutedEventArgs.
    /// <br/>
    /// NOTE: I also tried to create a custom event with custom inherited
    /// RoutedEventArgs, but this didn't seem to work, 
    /// so this event is commented out. But if anyone knows how to do this 
    /// please let me know, as far as I know
    /// I am doing everything correctly
    /// </summary>
    public class ColorPickerControl : ListBox
    {
        #region InstanceFields
        //A RoutedEvent using standard RoutedEventArgs, event declaration
        //The actual event routing
        public static readonly RoutedEvent NewColorEvent =
            EventManager.RegisterRoutedEvent
        ("NewColor", RoutingStrategy.Bubble,
                   typeof(RoutedEventHandler), typeof(ColorPickerControl));

        //A RoutedEvent using standard custom ColorRoutedEventArgs, 
        //event declaration

        ////the event handler delegate
        public delegate void NewColorCustomEventHandler
            (object sender, ColorRoutedEventArgs e);

        ////The actual event routing
        public static readonly RoutedEvent NewColorCustomEvent =
             EventManager.RegisterRoutedEvent
        ("NewColorCustom", RoutingStrategy.Bubble,
                   typeof(NewColorCustomEventHandler),
        typeof(ColorPickerControl));
        //******************************************************************
        //string array or colors
        private string[] _sColors =
        {
            "Black", "Brown", "DarkGreen", "MidnightBlue",
                "Navy", "DarkBlue", "Indigo", "DimGray",
            "DarkRed", "OrangeRed", "Olive", "Green",
                "Teal", "Blue", "SlateGray", "Gray",
            "Red", "Orange", "YellowGreen", "SeaGreen",
                "Aqua", "LightBlue", "Violet", "DarkGray",
            "Pink", "Gold", "Yellow", "Lime",
                "Turquoise", "SkyBlue", "Plum", "LightGray",
            "LightPink", "Tan", "LightYellow", "LightGreen",
                "LightCyan", "LightSkyBlue", "Lavender", "White"
        };
        #endregion
        #region Constructor
        /// <summary>
        /// Constructor for ColorPickerControl, which is a ListBox subclass
        /// </summary>
        public ColorPickerControl()
        {
            // Define a template for the Items, 
            // used the lazy FrameworkElementFactory method
            FrameworkElementFactory fGrid = new
                FrameworkElementFactory
        (typeof(System.Windows.Controls.Primitives.UniformGrid));
            fGrid.SetValue
       (System.Windows.Controls.Primitives.UniformGrid.ColumnsProperty, 10);
            // update the ListBox ItemsPanel with the new 
            // ItemsPanelTemplate just created
            ItemsPanel = new ItemsPanelTemplate(fGrid);

            // Create individual items
            foreach (string clr in _sColors)
            {
                // Creat bounding rectangle for items data
                Rectangle rItem = new Rectangle();
                rItem.Width = 10;
                rItem.Height = 10;
                rItem.Margin = new Thickness(1);
                rItem.Fill =
            (Brush)typeof(Brushes).GetProperty(clr).GetValue(null, null);
                //add rectangle to ListBox Items
                Items.Add(rItem);

                //add a tooltip
                ToolTip t = new ToolTip();
                t.Content = clr;
                rItem.ToolTip = t;
            }
            //Indicate that SelectedValue is Fill property of Rectangle item.
            //Kind of like an XPath query, 
            //this is the string name of the property
            //to use as the selected item value from the actual item data. 
            //The item data being a Rectangle in this case
            SelectedValuePath = "Fill";
        }
        #endregion
        #region Events
        // Provide CLR accessors for the event
        public event RoutedEventHandler NewColor
        {
            add { AddHandler(NewColorEvent, value); }
            remove { RemoveHandler(NewColorEvent, value); }
        }

        // This method raises the NewColor event
        private void RaiseNewColorEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(NewColorEvent);
            RaiseEvent(newEventArgs);
        }

        // Provide CLR accessors for the event
        public event NewColorCustomEventHandler NewColorCustom
        {
            add { AddHandler(NewColorCustomEvent, value); }
            remove { RemoveHandler(NewColorCustomEvent, value); }
        }

        // This method raises the NewColorCustom event
        private void RaiseNewColorCustomEvent()
        {
            ToolTip t = (ToolTip)(SelectedItem as Rectangle).ToolTip;
            ColorRoutedEventArgs newEventArgs =
        new ColorRoutedEventArgs(t.Content.ToString());
            newEventArgs.RoutedEvent = ColorPickerControl.NewColorCustomEvent;
            RaiseEvent(newEventArgs);
        }
        //*******************************************************************
        #endregion
        #region Overrides
        /// <summary>
        /// Overrides the OnSelectionChanged ListBox inherited method, and
        /// raises the NewColorEvent
        /// </summary>
        /// <param name="e">the event args</param>
        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);
            //raise the event with standard RoutedEventArgs event args
            RaiseNewColorEvent();
            //raise the event with the custom ColorRoutedEventArgs event args
            RaiseNewColorCustomEvent();
            //****************************************************************
        }
        #endregion
    }
    #endregion
    #region ColorRoutedEventArgs CLASS
    /// <summary>
    /// ColorRoutedEventArgs : a custom event argument class
    /// </summary>
    public class ColorRoutedEventArgs : RoutedEventArgs
    {
        #region Instance fields
        private string _ColorName = "";
        #endregion
        #region Constructor
        /// <summary>
        /// Constructs a new ColorRoutedEventArgs object
        /// using the parameters provided
        /// </summary>
        /// <param name="clrName">the color name string</param>
        public ColorRoutedEventArgs(string clrName)
        {
            this._ColorName = clrName;
        }
        #endregion
        #region Public properties
        /// <summary>
        /// Gets the stored color name
        /// </summary>
        public string ColorName
        {
            get { return _ColorName; }
        }
        #endregion
    }
    #endregion
}
