using System;
using System.Data;
using System.Diagnostics;
using System.Windows;

namespace ScrumStore
{
    /// <summary>
    /// Interaction logic for AdminView.xaml
    /// </summary>

    public partial class AdminView : Window
    {

        Search search = new Search();
        EditBook bookView;

        String CurrentSearch = "";
        String CurrentCat = "";
        String CurrentRating = "";

        public AdminView()
        {
            InitializeComponent();
            DataContext = search.getBooksSearchAdmin("", "", "");
            CurrentCat = "ALL";
            CurrentRating = "0";
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {

            if (ProductsDataGrid.SelectedItems.Count > 0)
            {
                DataRowView selectedRow = (DataRowView)ProductsDataGrid.SelectedItems[0];
                string isbnNum = selectedRow.Row.ItemArray[0].ToString();
                if (isbnNum == null || isbnNum == "")
                {
                    MessageBox.Show("Sorry, but something went wrong. Book may have been deleted.");
                    return;
                }
                try
                {
                    bookView = new EditBook(isbnNum);
                    bookView.Show();
                    bookView.Owner = this;
                    bookView.resetData();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    bookView = null;
                }
            }
            else
            {
                MessageBox.Show("Please select an item from the table to edit.");
            }
        }

        public void updateCategories()
        {
            String f, g, k, backupCat, backupSup;
            k = categoriesComboBox.Text;
            if (typeCategory.Visibility == Visibility.Visible)
                f = NewCategory.Text;
            else
                f = CategorySelect.Text;
            if (typeSupplier.Visibility == Visibility.Visible)
                g = NewSupplier.Text;
            else
                g = SupplierSelect.Text;
            backupCat = CategorySelect.Text;
            backupSup = SupplierSelect.Text;

            DataContext = search.getBooksSearchAdmin(CurrentCat, CurrentSearch, CurrentRating);
            categoriesComboBox.Text = CurrentCat;
            selectCategory.Visibility = Visibility.Visible;
            typeCategory.Visibility = Visibility.Collapsed;
            selectSupplier.Visibility = Visibility.Visible;
            typeSupplier.Visibility = Visibility.Collapsed;
            categoriesComboBox.Text = k;
            CategorySelect.Text = f;
            SupplierSelect.Text = g;
            if (CategorySelect.Text == "" || CategorySelect.Text == null)
                CategorySelect.Text = backupCat;
            if (SupplierSelect.Text == "" || SupplierSelect.Text == null)
                SupplierSelect.Text = backupSup;
            NewCategory.Text = "";
            CategoryDescription.Text = "";
            NewSupplier.Text = "";
        }
        public void clearData()
        {
            ISBN.Text = "";
            TitleBox.Text = "";
            Author.Text = "";
            CategorySelect.SelectedIndex = 0;
            NewCategory.Text = "";
            CategoryDescription.Text = "";
            SupplierSelect.SelectedIndex = 0;
            NewSupplier.Text = "";
            selectCategory.Visibility = Visibility.Visible;
            typeCategory.Visibility = Visibility.Collapsed;
            selectSupplier.Visibility = Visibility.Visible;
            typeSupplier.Visibility = Visibility.Collapsed;
            Price.Text = "";
            Year.Text = "";
            Edition.Text = "";
            Publisher.Text = "";
            Inventory.Text = "";
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            string t = categoriesComboBox.Text, f = CategorySelect.Text;
            DataContext = search.getBooksSearchAdmin(categoriesComboBox.Text, keywords.Text, rating.Text);
            CurrentRating = rating.Text;
            CurrentCat = categoriesComboBox.Text;
            CurrentSearch = keywords.Text;
            categoriesComboBox.Text = t;
            CategorySelect.Text = f;
        }
        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            string f = CategorySelect.Text;
            keywords.Text = "";
            rating.Text = "0";
            DataContext = search.getBooksSearchAdmin("", "", "");
            CurrentSearch = "";
            CurrentCat = "ALL";
            CurrentRating = "0";
            CategorySelect.Text = f;
        }
        private void newCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            selectCategory.Visibility = Visibility.Visible;
            typeCategory.Visibility = Visibility.Collapsed;
        }
        private void categoryButton_Click(object sender, RoutedEventArgs e)
        {
            selectCategory.Visibility = Visibility.Collapsed;
            typeCategory.Visibility = Visibility.Visible;
        }
        private void newSupplierButton_Click(object sender, RoutedEventArgs e)
        {
            selectSupplier.Visibility = Visibility.Visible;
            typeSupplier.Visibility = Visibility.Collapsed;
        }
        private void supplierButton_Click(object sender, RoutedEventArgs e)
        {
            selectSupplier.Visibility = Visibility.Collapsed;
            typeSupplier.Visibility = Visibility.Visible;
        }
        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            EditBookData edit = new EditBookData();
            String CatId = "";
            String SupId = "";
            String message = "";

            if (typeCategory.Visibility == Visibility.Visible)
            {
                CatId = edit.saveCategory(NewCategory.Text, CategoryDescription.Text, CatId);
                message = "Category/Supplier was saved. ";
            }
            else
                CatId = edit.getCategory(CategorySelect.Text, CatId);
            if (typeSupplier.Visibility == Visibility.Visible)
            {
                SupId = edit.saveSupplier(NewSupplier.Text, SupId);
                message = "Category/Supplier was saved. ";
            }
            else
                SupId = edit.getSupplier(SupplierSelect.Text, SupId);

            if (!edit.createBook(CatId, TitleBox.Text, Author.Text, Price.Text, SupId, Year.Text, Edition.Text, Publisher.Text, Inventory.Text, ISBN.Text))
            {
                MessageBox.Show(message + "Your book was not saved. Please check data.");
                updateCategories();
            }
            else
            {
                MessageBox.Show(message + "Your book was saved!");
                updateCategories();
                clearData();
            }
        }
    }
}
