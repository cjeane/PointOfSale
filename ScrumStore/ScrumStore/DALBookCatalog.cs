using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrumStore
{
    class DALBookCatalog
    {
        SqlConnection conn;
        DataSet dsBooks;
        DataTable dt = new DataTable();
        public DALBookCatalog()
        {
            conn = new SqlConnection(Properties.Settings.Default.scrumConnection);
        }

        public DataSet GetTopBooks()
        {

            try
            {

                String strSQL = "select top 5 title , author, price, year, rating from bookData order by rating desc";
                strSQL = "Select ISBN, CategoryID, Title, Author, Price, Year, Edition, Publisher, Rating from BookData";

                conn.Open();
                SqlCommand cmd = new SqlCommand(strSQL, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                dsBooks = new DataSet("Books");
                da.Fill(dsBooks, "Books"); // data table is null, why


            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return dsBooks;
        }

        //Author: Randy Daigle

        public DataSet GetBookInfo()
        {
            try
            {
                String strSQL = "Select CategoryID, Name, Description from Category";
                SqlCommand cmdSelCategory = new SqlCommand(strSQL, conn);
                SqlDataAdapter daCategory = new SqlDataAdapter(cmdSelCategory);
                dsBooks = new DataSet("Books");
                daCategory.Fill(dsBooks, "Category");
                String strSQL2 = "Select ISBN, CategoryID, Title, Author, Price, Year, Edition, Publisher, Rating from BookData";
                SqlCommand cmdSelBook = new SqlCommand(strSQL2, conn);
                SqlDataAdapter daBook = new SqlDataAdapter(cmdSelBook);
                daBook.Fill(dsBooks, "Books");

                String strSQL3 = "Select top 5 ISBN, CategoryID, Title, Author, Price, Year, Edition, Publisher, Rating from BookData order by rating desc";
                SqlCommand cmdSelBook2 = new SqlCommand(strSQL3, conn);
                SqlDataAdapter daBook2 = new SqlDataAdapter(cmdSelBook2);
                daBook2.Fill(dsBooks, "Books2");

                DataRelation drCat_Book = new DataRelation("drCat_Book",
                    dsBooks.Tables["Category"].Columns["CategoryID"],
                    dsBooks.Tables["Books"].Columns["CategoryID"], false);
                dsBooks.Relations.Add(drCat_Book);

                DataRelation drCat_Book_rated = new DataRelation("drCat_Book_rated",
                    dsBooks.Tables["Category"].Columns["CategoryID"],
                    dsBooks.Tables["Books2"].Columns["CategoryID"], false);
                dsBooks.Relations.Add(drCat_Book_rated);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return dsBooks;
        }

        public string getBookTitleFromId(string id)
        {

            string temp = "";
            var conn = new SqlConnection(Properties.Settings.Default.scrumConnection);
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "Select Title from BookData where "
                    + "ISBN = @id";
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();
                temp = cmd.ExecuteScalar().ToString();
                return temp;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());

            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
            return temp;
        }

        public List<string> getListofBooks()
        {
            var listofBooks = new List<string>();
            SqlConnection conn = new SqlConnection(Properties.Settings.Default.scrumConnection);

            try
            {
                conn.Open();
                string sql = "SELECT ISBN FROM BookData order by ISBN";
                using (var command = new SqlCommand(sql, conn))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string temp = reader[0].ToString();
                            listofBooks.Add(temp);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return listofBooks;
        }
    }
}
