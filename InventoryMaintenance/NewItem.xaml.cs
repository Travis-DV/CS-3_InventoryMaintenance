using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace InventoryMaintenance
{
    /// <summary>
    /// Interaction logic for NewItem.xaml
    /// </summary>
    public partial class NewItem : Window
    {

        public delegate void AddItem(Item i);
        public event AddItem SelectedItem;
        private List<int> illigals;

        public NewItem(List<int> illigals)
        {
            InitializeComponent();
            this.illigals = illigals;
        }

        private void NUMBERSONLY(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void DOUBLEONLY(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9,.]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void SaveItem(object sender, RoutedEventArgs e)
        {
            if (itemNoTB.Text == "" || descriptionTB.Text == "" || priceTB.Text == "")
            {
                return;
            }
            if (illigals.Contains(int.Parse(itemNoTB.Text)))
            {
                MessageBox.Show("That ID is already in use!");
                return;
            }

            SelectedItem?.Invoke(new Item()
            {
                itemNumber = int.Parse(itemNoTB.Text),
                description = descriptionTB.Text,
                price = double.Parse(priceTB.Text)
            });

            this.Close();
        }

        private void CancelItem(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
