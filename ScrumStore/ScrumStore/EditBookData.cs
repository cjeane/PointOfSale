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
    class EditBookData
    {

        SqlConnection conn;
        public EditBookData()
        {
            conn = new SqlConnection(Properties.Settings.Default.scrumConnection);
        }

        public void updateTable()
        {
            try
            {
                conn.Open();
                SqlCommand updateRating = new SqlCommand("UpdateRating", conn);
                updateRating.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        public String saveCategory(String NewCategory, String CategoryDescription, String error)
        {
            updateTable();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "INSERT INTO Category(Name, Description) VALUES(@name, @description)";
                cmd.Parameters.AddWithValue("@name", NewCategory);
                cmd.Parameters.AddWithValue("@description", CategoryDescription);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                DataSet Category = new DataSet("Cat");
                String strSQL = "Select CategoryID from Category where Name = '" + NewCategory + "'";
                conn.Open();
                SqlCommand cmdSelCategory = new SqlCommand(strSQL, conn);
                SqlDataAdapter daCategory = new SqlDataAdapter(cmdSelCategory);
                conn.Close();
                daCategory.Fill(Category, "Cat");
                return Category.Tables["Cat"].Rows[0]["CategoryId"].ToString();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return error;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        public string getCategory(String CategorySelect, string error)
        {

            updateTable();
            try
            {
                DataSet Category = new DataSet("Cat");
                String strSQL = "Select CategoryID from Category where Name = '" + CategorySelect + "'";
                conn.Open();
                SqlCommand cmdSelCategory = new SqlCommand(strSQL, conn);
                SqlDataAdapter daCategory = new SqlDataAdapter(cmdSelCategory);
                conn.Close();
                daCategory.Fill(Category, "Cat");
                return Category.Tables["Cat"].Rows[0]["CategoryId"].ToString();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return error;
            }
        }

        public String saveSupplier(String NewSupplier, String error)
        {

            updateTable();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "INSERT INTO Supplier(Name) VALUES(@name)";
                cmd.Parameters.AddWithValue("@name", NewSupplier);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                DataSet Supplier = new DataSet("Sup");
                String strSQL = "Select SupplierId from Supplier where Name = '" + NewSupplier + "'";
                conn.Open();
                SqlCommand cmdSelCategory = new SqlCommand(strSQL, conn);
                SqlDataAdapter daCategory = new SqlDataAdapter(cmdSelCategory);
                conn.Close();
                daCategory.Fill(Supplier, "Sup");
                return Supplier.Tables["Sup"].Rows[0]["SupplierId"].ToString();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return error;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        public String getSupplier(String SupplierSelect, String error)
        {

            updateTable();
            try
            {
                DataSet Supplier = new DataSet("Sup");
                String strSQL = "Select SupplierId from Supplier where Name = '" + SupplierSelect + "'";
                conn.Open();
                SqlCommand cmdSelCategory = new SqlCommand(strSQL, conn);
                SqlDataAdapter daCategory = new SqlDataAdapter(cmdSelCategory);
                conn.Close();
                daCategory.Fill(Supplier, "Sup");
                return Supplier.Tables["Sup"].Rows[0]["SupplierId"].ToString();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return error;
            }
        }

        public Boolean editBook(String CatId, String Title, String Author, String Price, String SupId, String Year, String Edition, String Publisher, String Inventory, String ISBN)
        {
            String sql = "UPDATE BookData "
                            + "SET CategoryId = @categoryId,"
                            + " Title = @title,"
                            + " Author = @author,"
                            + " Price = @price,"
                            + " SupplierId = @supplierId,"
                            + " Year = @year,"
                            + " Edition = @edition,"
                            + " Publisher = @publisher,"
                            + " Inventory = @inventory"
                            + " where ISBN = @isbn";
            return saveBook(sql, CatId, Title, Author, Price, SupId, Year, Edition, Publisher, Inventory, ISBN, "");
        }

        public Boolean createBook(String CatId, String Title, String Author, String Price, String SupId, String Year, String Edition, String Publisher, String Inventory, String ISBN)
        {
            String sql = "INSERT INTO BookData(ISBN, CategoryId, Title, Author, Price, SupplierId, Year, Edition, Publisher, Rating, Inventory) "
                + " VALUES(@isbn, @categoryId, @title, @author, @price, @supplierId, @year, @edition, @publisher, @rating, @inventory)";
            return saveBook(sql, CatId, Title, Author, Price, SupId, Year, Edition, Publisher, Inventory, ISBN, "0");
        }

        private Boolean saveBook(String sql, String CatId, String Title, String Author, String Price, String SupId, String Year, String Edition, String Publisher, String Inventory, String ISBN, String Rating)
        {
            updateTable();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@categoryId", CatId);
                cmd.Parameters.AddWithValue("@title", Title);
                cmd.Parameters.AddWithValue("@author", Author);
                cmd.Parameters.AddWithValue("@price", Price);
                cmd.Parameters.AddWithValue("@supplierId", SupId);
                cmd.Parameters.AddWithValue("@year", Year);
                cmd.Parameters.AddWithValue("@edition", Edition);
                cmd.Parameters.AddWithValue("@publisher", Publisher);
                if (Rating != "" && Rating != null)
                    cmd.Parameters.AddWithValue("@rating", Rating);
                cmd.Parameters.AddWithValue("@inventory", Inventory);
                cmd.Parameters.AddWithValue("@isbn", ISBN);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return false;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }
    }
}
