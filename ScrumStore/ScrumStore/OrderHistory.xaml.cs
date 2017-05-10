using System.Data;
using System.Windows;

namespace ScrumStore
{
    /// <summary>
    /// Interaction logic for OrderHistory.xaml
    /// </summary>
    public partial class OrderHistory : Window
    {
        private DALUserSettings stng = new DALUserSettings();
        private DataSet ret;
        public OrderHistory()
        {
            InitializeComponent();
            UserData session = UserData.getInstance();
            ret = stng.getOrderHistory(session.UserID);
            DataContext = ret.Tables["Orders"];
        }

        private void backbutton_Click(object sender, RoutedEventArgs e)
        {
            this.Owner.Show();
            this.Hide();
        }

        private void ordersComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }
    }
}
