using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
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
using System.Text.RegularExpressions;
using System.Data;

namespace ScrumStore
{
    /// <summary>
    /// Interaction logic for PreviewBook.xaml
    /// </summary>
    public partial class PreviewBookDialog : Window
    {
        DataSet comments;

        String imagesDir = Directory.GetCurrentDirectory() + "\\..\\..\\..\\Images\\";
        String placeholderImagePath = Directory.GetCurrentDirectory() + "\\..\\..\\..\\Images\\placeholderBook.png";

        public PreviewBookDialog(String bookTitle)
        {
            InitializeComponent();

            PreviewBook PB = new PreviewBook();

            if (PreviewBook.bookExists(bookTitle))
            {

                Title = bookTitle;
                priceLabel.Content += "$" + PreviewBook.getPrice(bookTitle);
                ratingLabel.Content += PreviewBook.getRating(bookTitle) + "/5";

                comments = PB.getTopBookFeedback(bookTitle);
                DataContext = comments;
            }
            else
            {

                Title = "Book Not Found";
                priceLabel.Content += "N/A";
                ratingLabel.Content += "N/A";
            }

            loadImage(imagesDir + Regex.Replace(bookTitle, "[:]", "") + ".jpg");
        }
        public void loadImage(String imagePath)
        {

            BitmapImage b = new BitmapImage();

            if (PreviewBook.imageExists(imagePath))
            {

                b = new BitmapImage();
                b.BeginInit();
                b.UriSource = new Uri(imagePath);
                b.EndInit();
            }
            else
            {

                b = new BitmapImage();
                b.BeginInit();
                b.UriSource = new Uri(placeholderImagePath);
                b.EndInit();
            }

            image.Source = b;
        }

        /*
        // preview button function in the main window
        private void previewButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView selectedRow;
                selectedRow = (DataRowView)this.ProductsDataGrid.SelectedItems[0];

                if (selectedRow != null)
                {

                    // open preview
                    PreviewBookDialog previewBookDialog = new PreviewBookDialog(selectedRow.Row.ItemArray[2].ToString());
                    previewBookDialog.ShowDialog();

                }
            }
            catch { }
        }
        */
    }
}
