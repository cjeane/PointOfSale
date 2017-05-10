using System;
using System.Windows;

namespace ScrumStore
{
    /// <summary>
    /// Interaction logic for Cart.xaml
    /// </summary>
    public partial class Cart : Window
    {
        public BookOrder thisOrder { get; set; }
        public Cart()
        {

            InitializeComponent();
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            int i;
            if (this.orderListView.SelectedItem != null)
            {
                var selectedOrderItem = this.orderListView.SelectedItem as OrderItem;
                Int32.TryParse(textBoxAmount.Text, out i);
                thisOrder.AddItem(selectedOrderItem.BookID, i);
                orderListView.ItemsSource = null;
                orderListView.ItemsSource = thisOrder.OrderItemList;
            }
        }

        private void buttonRemove_Click(object sender, RoutedEventArgs e)
        {
            int i;
            if (this.orderListView.SelectedItem != null)
            {
                var selectedOrderItem = this.orderListView.SelectedItem as OrderItem;
                Int32.TryParse(textBoxAmount.Text, out i);
                thisOrder.RemoveItem(selectedOrderItem.BookID, i);
                orderListView.ItemsSource = null;
                orderListView.ItemsSource = thisOrder.OrderItemList;

            }
        }

        private void checkout_Click(object sender, RoutedEventArgs e)
        {
            if (thisOrder.OrderItemList.Count == 0)
            {
                MessageBox.Show("Your cart is empy.");
                return;
            }

            else if (UserData.getInstance().LoggedIn && thisOrder.validateOrder() == null)
            {
                Checkout checkOutWin = new Checkout(thisOrder);
                this.Close();
                checkOutWin.Show();
            }
            else if (thisOrder.validateOrder() != null)
            {
                MessageBox.Show(thisOrder.validateOrder());
            }
            else

            {
                MessageBox.Show("Please log in before proceeding.");

            }
        }
    }
}
