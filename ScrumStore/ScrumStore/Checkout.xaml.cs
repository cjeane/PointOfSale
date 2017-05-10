using System.Globalization;
using System.Windows;
using System.Windows.Media;
using static ScrumStore.Ultity;


namespace ScrumStore
{
    /// <summary>
    /// Interaction logic for Checkout.xaml
    /// </summary>
    public partial class Checkout : Window
    {

        UserData userData;
        string[] months;// = new DateTimeFormatInfo();
        BookOrder order;

        public Checkout(BookOrder bOrder)
        {
            InitializeComponent();
            userData = UserData.getInstance();
            shipName.Content = userData.FullName;
            shipAddress.Content = userData.Address;
            shipPostal.Content = userData.City + ", " + userData.Province + ", " + userData.Postal;
            shipCountry.Content = "Canada";
            months = DateTimeFormatInfo.InvariantInfo.MonthNames;
            order = bOrder;
            foreach (var s in months)
            {
                monComboBox.Items.Add(s);
            }

            for (int i = 2016; i <= 2024; i++)
            {
                yearComboBox.Items.Add(i);
            }

            if (userData.infoSet())
            {
                tab2.IsSelected = true;
                tab3.Visibility = Visibility.Collapsed;
                tab4.Visibility = Visibility.Collapsed;
            }
            else
            {
                tab2.Visibility = Visibility.Collapsed;
                tab3.Visibility = Visibility.Collapsed;
                tab4.Visibility = Visibility.Collapsed;
            }
        }

        private void buttonShipEdit_Click(object sender, RoutedEventArgs e)
        {
            tab1.IsSelected = true;
            nameEditTextBox.Text = userData.FullName;
            addressEditTextBox.Text = userData.Address;
            cityEditTextBox.Text = userData.City;
            postalEditTextBox.Text = userData.Postal;
            provinceEditTextBox.Text = userData.Province;
        }

        private void buttonShip_Click(object sender, RoutedEventArgs e)
        {
            tab4.IsSelected = true;
            tab4.Visibility = Visibility.Visible;
        }

        private void editConfirm_Click(object sender, RoutedEventArgs e)
        {
            userData.FullName = nameEditTextBox.Text;
            userData.Address = addressEditTextBox.Text;
            userData.City = cityEditTextBox.Text;
            userData.Postal = postalEditTextBox.Text;
            userData.Province = provinceEditTextBox.Text;
            if (userData.validateShippingInfo() == null)
            {
                shipName.Content = userData.FullName;
                shipAddress.Content = userData.Address;
                shipPostal.Content = userData.City + ", " + userData.Province + ", " + userData.Postal;
                userData.updateShippingInfo();
                tab2.IsSelected = true;
                tab2.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show(userData.validateShippingInfo());

            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {

            bool err = false;
            cc1.Background = Brushes.White;
            cc2.Background = Brushes.White;
            cc3.Background = Brushes.White;
            cc4.Background = Brushes.White;

            if (checkForNonDigit(cc1.Text) || cc1.Text.Length > 4)
            {
                cc1.Background = Brushes.Red;
                err = true;
            }
            if (checkForNonDigit(cc2.Text) || cc2.Text.Length > 4)
            {
                cc2.Background = Brushes.Red;
                err = true;
            }
            if (checkForNonDigit(cc3.Text) || cc3.Text.Length > 4)
            {
                cc3.Background = Brushes.Red;
                err = true;
            }
            if (checkForNonDigit(cc4.Text) || cc4.Text.Length > 4)
            {
                cc4.Background = Brushes.Red;
                err = true;
            }
            if (checkForNonDigit(cvcCCText.Text) || cvcCCText.Text.Length != 3)
            {
                cvcCCText.Background = Brushes.Red;
                err = true;
            }
            if (monComboBox.SelectedIndex == -1 || yearComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Please check your credit expiry date.");
                err = true;
            }

            if (err)
                return;

            tab3.IsSelected = true;
            tab3.Visibility = Visibility.Visible;

        }

        private void tabControl_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (tab4.IsSelected)
            {
                orderSubTotal.Content = "$" + order.GetOrderTotal().ToString();
                orderTax.Content = "$" + order.getOrderTax().ToString();
                orderDiscount.Content = "$00"; //place holder
                creditAmount.Content = "$00"; //place holder
                orderTotal.Content = "$" + order.getGrandTotal(0);
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            int orderNum;
            DALOrder makeOrder = new DALOrder();
            makeOrder.processOrder(userData.UserID.ToString());
            orderNum = makeOrder.retrieveOrderID(userData.UserID.ToString());
            foreach (var item in order.OrderItemList)
            {
                makeOrder.updateOrderDB(item.BookID, item.Quantity);
                makeOrder.processBooks(orderNum.ToString(), item.BookID, item.Quantity.ToString());
            }

            string msg = "Thank you for you order!";
            Receipt rcpt = new Receipt();
            if (rcpt.sendReceiptEmail(orderNum))
                msg += "\nA receipt has been emailed to " + userData.Email;
            else
                msg = "Something went terribly wrong. We'll get back to you.";

            MessageBox.Show(msg);
            this.Close();
            order.OrderItemList.Clear();
        }
    }
}
