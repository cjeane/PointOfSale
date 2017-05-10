using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace ScrumStore
{
    public class Search
    {

        SqlConnection conn;
        DataSet dsBooks;
        DataSet AdminBooks;
        DataSet Category;
        DataSet BookInfo;

        public Search()
        {
            conn = new SqlConnection(Properties.Settings.Default.scrumConnection);
        }

        public DataSet getBooks()
        {
            try
            {
                conn.Open();
                SqlCommand updateRating = new SqlCommand("UpdateRating", conn);
                updateRating.ExecuteNonQuery();

                String strSQL = "Select CategoryID, Name, Description from Category";
                SqlCommand cmdSelCategory = new SqlCommand(strSQL, conn);
                SqlDataAdapter daCategory = new SqlDataAdapter(cmdSelCategory);
                dsBooks = new DataSet("Books");
                daCategory.Fill(dsBooks, "Category");
                String strSQL2 = "Select ISBN, CategoryID, Title, Author, Price, Year, Edition, Publisher, Rating FROM BookData";
                SqlCommand cmdSelBook = new SqlCommand(strSQL2, conn);
                SqlDataAdapter daBook = new SqlDataAdapter(cmdSelBook);
                daBook.Fill(dsBooks, "Books");
                DataRelation drCat_Book = new DataRelation("drCat_Book",
                    dsBooks.Tables["Category"].Columns["CategoryID"],
                    dsBooks.Tables["Books"].Columns["CategoryID"], false);
                dsBooks.Relations.Add(drCat_Book);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return dsBooks;
        }

        public DataSet getBooksSearchAdmin(string cat, string keywords, string rating)
        {
            try
            {

                conn.Open();
                SqlCommand updateRating = new SqlCommand("UpdateRating", conn);
                updateRating.ExecuteNonQuery();

                AdminBooks = new DataSet("Books");

                String strSQL = "Select CategoryID, Name, Description from Category";
                SqlCommand cmdSelCategory = new SqlCommand(strSQL, conn);
                SqlDataAdapter daCategory = new SqlDataAdapter(cmdSelCategory);
                daCategory.Fill(AdminBooks, "Category");
                DataRow row = AdminBooks.Tables["Category"].NewRow();
                row["Name"] = "ALL";
                row["CategoryId"] = -1;
                AdminBooks.Tables["Category"].Rows.InsertAt(row, 0);

                strSQL = "Select CategoryID, Name, Description from Category as Cat";
                cmdSelCategory = new SqlCommand(strSQL, conn);
                daCategory = new SqlDataAdapter(cmdSelCategory);
                daCategory.Fill(AdminBooks, "Cat");

                strSQL = "Select SupplierId, Name from Supplier";
                cmdSelCategory = new SqlCommand(strSQL, conn);
                daCategory = new SqlDataAdapter(cmdSelCategory);
                daCategory.Fill(AdminBooks, "Supplier");

                String RatingADD = "";
                if (rating == "" || Int32.Parse(rating) <= 0)
                    RatingADD = "ISBN IS NOT NULL ";
                else
                    RatingADD = "Rating >= " + rating + " ";

                String strSQL2 = "Select distinct ISBN, BookData.CategoryID, Title, Author, Price, Year, Edition, Publisher, Rating from BookData";
                /** adding SQL for catefory search field **/
                if (cat != "" && cat != null && cat != "ALL")
                    strSQL2 += ", Category where Category.CategoryID = BookData.CategoryID and Name = '" + cat + "' and " + RatingADD;
                else
                    strSQL2 += " where " + RatingADD;

                strSQL2 += keywordSplit(keywords);

                SqlCommand cmdSelBook = new SqlCommand(strSQL2, conn);
                SqlDataAdapter daBook = new SqlDataAdapter(cmdSelBook);
                daBook.Fill(AdminBooks, "Books");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return AdminBooks;
        }

        public DataSet getBooksSearch(string cat, string keywords, string rating)
        {
            try
            {
                conn.Open();
                SqlCommand updateRating = new SqlCommand("UpdateRating", conn);
                updateRating.ExecuteNonQuery();

                String strSQL = "Select CategoryID, Name, Description from Category";
                if (cat != null)
                    strSQL += " where Name = '" + cat + "'";
                SqlCommand cmdSelCategory = new SqlCommand(strSQL, conn);
                SqlDataAdapter daCategory = new SqlDataAdapter(cmdSelCategory);
                dsBooks = new DataSet("Books");
                daCategory.Fill(dsBooks, "Category");
                String RatingADD = "";
                if (rating == "" || Int32.Parse(rating) <= 0)
                    RatingADD = "ISBN IS NOT NULL ";
                else
                    RatingADD = "Rating >= " + rating + " ";
                String strSQL2 = "Select distinct ISBN, CategoryID, Title, Author, Price, Year, Edition, Publisher, Rating from BookData where " + RatingADD;

                strSQL2 += keywordSplit(keywords);

                SqlCommand cmdSelBook = new SqlCommand(strSQL2, conn);
                SqlDataAdapter daBook = new SqlDataAdapter(cmdSelBook);
                daBook.Fill(dsBooks, "Books");
                DataRelation drCat_Book = new DataRelation("drCat_Book",
                    dsBooks.Tables["Category"].Columns["CategoryID"],
                    dsBooks.Tables["Books"].Columns["CategoryID"], false);
                dsBooks.Relations.Add(drCat_Book);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return dsBooks;
        }

        public DataSet getCategoriesAndSuppliers()
        {
            try
            {
                Category = new DataSet("Books");
                String strSQL = "Select CategoryID, Name, Description from Category";
                SqlCommand cmdSelCategory = new SqlCommand(strSQL, conn);
                SqlDataAdapter daCategory = new SqlDataAdapter(cmdSelCategory);
                daCategory.Fill(Category, "Category");

                strSQL = "Select SupplierId, Name from Supplier";
                cmdSelCategory = new SqlCommand(strSQL, conn);
                daCategory = new SqlDataAdapter(cmdSelCategory);
                daCategory.Fill(Category, "Supplier");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return Category;
        }


        public DataSet getBookInfo(string ISBN)
        {
            try
            {
                BookInfo = new DataSet("Books");

                String strSQL = "Select ISBN, Title, Author, Price, Year, Edition, Publisher, Inventory, Category.CategoryId as catId, Supplier.SupplierId as supId, Category.Name as cat, Supplier.Name as sup from BookData, Category, Supplier "
                    + "where ISBN = '" + ISBN + "' and Category.CategoryID = BookData.CategoryID and Supplier.SupplierId = BookData.SupplierId";

                SqlCommand cmdSelBook = new SqlCommand(strSQL, conn);
                SqlDataAdapter daBook = new SqlDataAdapter(cmdSelBook);
                daBook.Fill(BookInfo, "Books");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return BookInfo;
        }

        private string keywordSplit(String keywords)
        {
            string[] words = keywords.Split(' ');
            string compare = "";
            if (words != null && words.Length > 0)
            {
                bool first = true;
                foreach (String key in words)
                {
                    if (key == "" || key == null)
                        continue;
                    if (first)
                    {
                        compare += "LOWER(Title) LIKE LOWER('%" + key + "%') OR LOWER(Author) LIKE LOWER('%" + key + "%')";
                        first = false;
                    }
                    else
                        compare += " OR LOWER(Title) LIKE LOWER('%" + key + "%') OR LOWER(Author) LIKE LOWER('%" + key + "%')";
                }

            }
            if (compare != "")
                return "and (" + compare + ") ";
            else return "";
        }
    }
}
