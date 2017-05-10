/* **********************************************************************************
 * For use by students taking 60-422 (Fall, 2014) to work on assignments and project.
 * Permission required material. Contact: xyuan@uwindsor.ca 
 * **********************************************************************************/
using System.Windows;
using System.Data;
using System;
using System.Diagnostics;

namespace ScrumStore
{
    /// Interaction logic for MainWindow.xaml
    public partial class MainWindow : Window
    {
        BookRec testing;
        DataSet dsBookCat;
        DataSet dsTopBooks;
        UserData userData;
        BookOrder bookOrder;
        RatingInfo ratingInfo;
        Cart cartView;
        AdminView admin;
        UserSettings Sttngs;

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            var userData = UserData.getInstance();

            // If user is logged in and has a session, 
            // the "Login" button becomes "Settings" -kevin

            if (userData.LoggedIn == false)
            {
                var dlg = new LoginDialog();
                dlg.Owner = this;
                dlg.ShowDialog();
                // Process data entered by user if dialog box is accepted
                if (dlg.DialogResult == true)
                {
                    if (userData.LogIn(dlg.nameTextBox.Text, dlg.passwordTextBox.Password) == true)
                    {
                        if (userData.FullName.Length > 0)
                            this.statusTextBlock.Text = "You are logged in as User: " + userData.FullName;
                        else
                            this.statusTextBlock.Text = "You are logged in as Account ID: " + userData.UserID;

                        this.loginButton.Content = "Settings";
                        this.exitButton.Content = "Logout";
                        userData.loadAppearance();
                        if (userData.Manager)
                            adminButton.Visibility = Visibility.Visible;
                        updateRec();
                    }
                    else
                    {
                        MessageBox.Show("You could not be verified. Please try again.");
                        this.loginButton_Click(this, e);
                    }
                }
            }
            else if (userData.LoggedIn == true)
            {
                var usrSttngDlg = new UserSettingsDialog();
                usrSttngDlg.Owner = this;
                usrSttngDlg.ShowDialog();
            }
        }

        private void updateRec()
        {
            BookRec rec = new BookRec();

            Recommendation.Text = rec.getRecommendationString(bookOrder, userData.UserID);
        }
        private void adminButton_Click(object sender, RoutedEventArgs e)
        {
            if (userData.LoggedIn && userData.Manager)
            {
                if (admin == null || !admin.IsVisible)
                {
                    admin = new AdminView();
                    admin.Show();
                    admin.Owner = this;
                }
                else
                    admin.Focus();
            }
            else
            {
                admin = null;
                MessageBox.Show("You are not admin.");
            }
        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {

            Search search = new Search();

            dsBookCat = search.getBooks();
            DataContext = dsBookCat.Tables["Category"];
            keywords.Text = "";
            rating.SelectedIndex = 0;
        }
        private void searchButton_Click(object sender, RoutedEventArgs e)
        {

            Search search = new Search();

            dsBookCat = search.getBooksSearch(categoriesComboBox.Text, keywords.Text, rating.Text);
            DataContext = dsBookCat.Tables["Category"];
        }
        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            var usr = UserData.getInstance();
            if (usr.LoggedIn)
            {
                usr.Logout();
                usr.setDefaultAppearance();
                bookOrder.OrderItemList.Clear();
                loginButton.Content = "Login";
                exitButton.Content = "Register";
                updateRec();
                statusTextBlock.Text = "";
                adminButton.Visibility = Visibility.Hidden;
                if (admin != null)
                {
                    admin.Close();
                    admin = null;
                }
            }
            else
            {
                RegisterDialog registerDialog = new RegisterDialog();
                registerDialog.ShowDialog();
            }
        }
        public MainWindow() { InitializeComponent(); }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BookCatalog bookCat = new BookCatalog();

            dsBookCat = bookCat.GetBookInfo();
            DataContext = dsBookCat.Tables["Category"];
            dsTopBooks = bookCat.getTopBooks();
            bookOrder = new BookOrder();
            userData = UserData.getInstance();
            testing = new BookRec();
            Sttngs = new UserSettings();
        }
        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            OrderItemDialog orderItemDialog = new OrderItemDialog();
            DataRowView selectedRow;
            if (ProductsDataGrid.SelectedItems.Count > 0)
            {

                selectedRow = (DataRowView)this.ProductsDataGrid.SelectedItems[0];

                orderItemDialog.isbnTextBox.Text = selectedRow.Row.ItemArray[0].ToString();
                orderItemDialog.titleTextBox.Text = selectedRow.Row.ItemArray[2].ToString();
                orderItemDialog.priceTextBox.Text = selectedRow.Row.ItemArray[4].ToString();
                orderItemDialog.Owner = this;
                orderItemDialog.ShowDialog();
                if (orderItemDialog.DialogResult == true)
                {
                    string isbn = orderItemDialog.isbnTextBox.Text;
                    string title = orderItemDialog.titleTextBox.Text;
                    double unitPrice = double.Parse(orderItemDialog.priceTextBox.Text);
                    int quantity = int.Parse(orderItemDialog.quantityTextBox.Text);
                    bookOrder.AddItem(new OrderItem(isbn, title, unitPrice, quantity));
                    updateRec();
                }
            }
        }
        private void chechoutButton_Click(object sender, RoutedEventArgs e)
        {

            if (!UserData.getInstance().LoggedIn)
            {
                MessageBox.Show("Please log before proceeding.");
            }
            else
            {
                MessageBox.Show("Please review your order in cart before proceeding. Thank you.");
                {
                    if (cartView == null || !cartView.IsLoaded)
                    {
                        cartView = new Cart();
                        cartView.Owner = this;
                        cartView.orderListView.ItemsSource = bookOrder.OrderItemList;
                        cartView.thisOrder = bookOrder;
                    }
                    cartView.Show();
                }
            }
        }

        private void addRating_Click(object sender, RoutedEventArgs e)
        {
            Rating rateBookDialog = new Rating();
            ratingInfo = new RatingInfo();
            DataRowView selectedRow;
            selectedRow = (DataRowView)this.ProductsDataGrid.SelectedItems[0];
            rateBookDialog.bookTitleBlock.Text = selectedRow.Row[2].ToString();
            rateBookDialog.overallRating.Text = selectedRow.Row[8].ToString();
            rateBookDialog.Owner = this;
            rateBookDialog.ShowDialog();
            if (rateBookDialog.DialogResult == true)
            {
                string bookComment = "";
                string bookTitle = rateBookDialog.bookTitleBlock.Text;
                int userRating = Convert.ToInt32(rateBookDialog.userRating.SelectionBoxItem.ToString());
                if (rateBookDialog.DialogResult == true)
                {
                    if (userData.UserID >= 1)
                    {
                        ratingInfo.SetUserRating(bookTitle, userData.UserID, userRating, bookComment);
                    }
                }
            }
        }

        private void viewCart_Click(object sender, RoutedEventArgs e)
        {

            {
                if (cartView == null || !cartView.IsLoaded)
                {
                    cartView = new Cart();
                    cartView.Owner = this;
                    cartView.orderListView.ItemsSource = bookOrder.OrderItemList;
                    cartView.thisOrder = bookOrder;
                }
                cartView.Show();
            }
        }

        private void previewButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView selectedRow;
                selectedRow = (DataRowView)this.ProductsDataGrid.SelectedItems[0];

                if (selectedRow != null)
                {
                    PreviewBookDialog previewBookDialog = new PreviewBookDialog(selectedRow.Row.ItemArray[2].ToString());
                    previewBookDialog.ShowDialog();
                }
            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.Message);
            }
        }
    }
}
