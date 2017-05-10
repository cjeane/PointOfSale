using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
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
using System.Windows.Shapes;

namespace ScrumStore
{
    /// <summary>
    /// Interaction logic for EditBook.xaml
    /// </summary>
    public partial class EditBook : Window
    {
        DataSet row;
        String ISBN;

        public EditBook(String isbn)
        {
            InitializeComponent();
            Search search = new Search();
            DataContext = search.getCategoriesAndSuppliers();
            row = search.getBookInfo(isbn);
            ISBN = isbn;
        }

        public void resetData()
        {
            DataTable selectedRow = row.Tables["Books"];

            try
            {
                TitleBox.Text = getEntry(selectedRow, "Title");
                Author.Text = getEntry(selectedRow, "Author");
                CategorySelect.Text = getEntry(selectedRow, "cat");
                Price.Text = getEntry(selectedRow, "Price");
                Year.Text = getEntry(selectedRow, "Year");
                Edition.Text = getEntry(selectedRow, "Edition");
                SupplierSelect.Text = getEntry(selectedRow, "sup");
                Publisher.Text = getEntry(selectedRow, "Publisher");
                Inventory.Text = getEntry(selectedRow, "Inventory");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                MessageBox.Show("Book may have been deleted.");
                Close();
            }
        }
        private String getEntry(DataTable selectedRow, String entry)
        {
            if (selectedRow.Rows[0][entry] != null)
                return selectedRow.Rows[0][entry].ToString();
            else
                return "";
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

        public void resetScreen()
        {
            Search search = new Search();
            DataContext = search.getCategoriesAndSuppliers();

            if (NewCategory.Text != "")
            {
                selectCategory.Visibility = Visibility.Visible;
                typeCategory.Visibility = Visibility.Collapsed;
                CategorySelect.Text = NewCategory.Text;
                NewCategory.Text = "";
                CategoryDescription.Text = "";
            }
            if (NewSupplier.Text != "")
            {
                selectSupplier.Visibility = Visibility.Visible;
                typeSupplier.Visibility = Visibility.Collapsed;
                SupplierSelect.Text = NewSupplier.Text;
                NewSupplier.Text = "";
            }
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            EditBookData edit = new EditBookData();
            string CatId = getEntry(row.Tables["Books"], "catId");
            string SupId = getEntry(row.Tables["Books"], "supId");

            if (typeCategory.Visibility == Visibility.Visible)
                CatId = edit.saveCategory(NewCategory.Text, CategoryDescription.Text, CatId);
            else
                CatId = edit.getCategory(CategorySelect.Text, CatId);
            if (typeSupplier.Visibility == Visibility.Visible)
                SupId = edit.saveSupplier(NewSupplier.Text, SupId);
            else
                SupId = edit.getSupplier(SupplierSelect.Text, SupId);

            if (!edit.editBook(CatId, TitleBox.Text, Author.Text, Price.Text, SupId, Year.Text, Edition.Text, Publisher.Text, Inventory.Text, ISBN))
            {
                ((AdminView)this.Owner).updateCategories();
                resetScreen();
                MessageBox.Show("An error occured trying to save that data, please make sure your information is valid.");
                return;
            }
            ((AdminView)this.Owner).updateCategories();
            Close();
        }

    }
}
