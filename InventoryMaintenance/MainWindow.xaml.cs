using System.Windows;
using System.Linq;
using System.Xml.Linq;
using System.Xml;

namespace InventoryMaintenance
{

    public record Item
    {
        public int itemNumber;
        public string description;
        public double price;

        public override string ToString()
        {
            return $"{itemNumber.ToString().PadLeft(7, '0')}; {description}, ${price}";
        }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        List<Item> items = new List<Item>();

        public MainWindow()
        {
            InitializeComponent();
            inventoryListbox.ItemsSource = items;

            #region XML Loading
            XElement TopLevelElement = XElement.Load("InventoryItems.xml");
            /* THEY WILL NOT GO AWAY!!
            List<string> TopLevelSplit = TopLevelElement.ToString().Split(System.Environment.NewLine).ToList();
            for (int i = 0; i < TopLevelSplit.Count; i++)
            {
                if (TopLevelSplit[i] == "<Item />")
                {
                    TopLevelSplit.RemoveAt(i);
                }
                if (TopLevelSplit[i] == System.Environment.NewLine)
                {
                    TopLevelSplit.RemoveAt(i);
                }
            }
            MessageBox.Show(string.Join(System.Environment.NewLine, TopLevelSplit));
            */
            
            foreach (var item in TopLevelElement.Elements())
            {
                List<XElement> elements = item.Elements().ToList();
                if (elements.Count == 0) //THEY WONT GO AWAY
                {
                    item.Remove();
                    continue;
                }
                items.Add(new Item()
                {
                    itemNumber = (int)elements[0],
                    description = (string)elements[1],
                    price = (double)elements[2]
                });
                
            }
            #endregion
        }

        private void AddItem(Object sender, RoutedEventArgs e)
        {
            NewItem newItem = new NewItem(items.Select(ti => ti.itemNumber).ToList());
            newItem.SelectedItem += AddToList;
            newItem.Show();
        }

        private void AddToList(Item i)
        {

            items.Add(i);
            inventoryListbox.Items.Refresh();
            XDocument doc = XDocument.Load("InventoryItems.xml");
            XElement element = doc.Element("Items");
            element.Add(new XElement("Item",
                new XElement("ItemNo", i.itemNumber.ToString()),
                new XElement("Description", i.description),
                new XElement("Price", i.price.ToString())));
            doc.Save("InventoryItems.xml");
        }

        private void DeleteItem(Object sender, RoutedEventArgs e)
        {
            Item ItemToRemove = items[inventoryListbox.SelectedIndex];
            items.Remove(ItemToRemove);
            inventoryListbox.Items.Refresh();

            

            XDocument doc = XDocument.Load("InventoryItems.xml");
            IEnumerable<XElement> itemElements = doc.Elements().Elements();
            foreach (var item in itemElements)
            {
                IEnumerable<XElement> elements = item.Elements();
                if (item.ToString() == "<Item />") //They will not go away i have no idea why but I only have 30 min left so bandaid
                {
                    elements.Remove();
                    continue;
                }
                if (elements.ToList()[0].Value.ToString() == ItemToRemove.itemNumber.ToString())
                {
                    elements.Remove();
                    doc.Save("InventoryITems.xml");
                }

            }
        }

        private void Exit(Object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}