/* **********************************************************************************
 * For use by students taking 60-422 (Fall, 2014) to work on assignments and project.
 * Permission required material. Contact: xyuan@uwindsor.ca 
 * **********************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;

namespace ScrumStore
{
    public class DALOrder
    {

        /// <summary>
        /// takes in isbn and number of to that book to remove
        /// Auther CJ
        /// </summary>
        /// <param name="isbn"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public bool updateOrderDB(string isbn, int amount)
        {
            SqlConnection conn = new SqlConnection(Properties.Settings.Default.scrumConnection);
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "Update BookData Set Inventory = @n where ISBN = @isbn";
                cmd.Parameters.AddWithValue("@n", (checkOrder(isbn) - amount).ToString());
                cmd.Parameters.AddWithValue("@isbn", isbn);
                conn.Open();
                cmd.ExecuteNonQuery();
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
        /// <summary>
        /// retrieve the current orderID that was just created.
        /// Auther CJ
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public int retrieveOrderID(string userID)
        {

            SqlConnection conn = new SqlConnection(Properties.Settings.Default.scrumConnection);
            try
            {

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "select Max (OrderID) from Orders where UserID = @id";
                cmd.Parameters.AddWithValue("@id", userID);
                conn.Open();
                return (int)cmd.ExecuteScalar();

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return -1;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        /// <summary>
        /// completes the order table for the current order being process
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isbn"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public bool processBooks(string id, string isbn, string amount)
        {

            SqlConnection conn = new SqlConnection(Properties.Settings.Default.scrumConnection);
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "insert into OrderItem (OrderID, ISBN, Quantity) Values (@id, @isbn, @n)";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@isbn", isbn);
                cmd.Parameters.AddWithValue("@n", amount);
                conn.Open();
                cmd.ExecuteNonQuery();
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

        /// <summary>
        /// starts the current order Process
        /// /// Auther CJ
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public bool processOrder(string userID)
        {

            SqlConnection conn = new SqlConnection(Properties.Settings.Default.scrumConnection);
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "Insert into Orders (UserID) Values (@id)";
                cmd.Parameters.AddWithValue("@id", userID);
                conn.Open();
                cmd.ExecuteNonQuery();
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
        /// <summary>
        /// used for checking the book amount and returns the amount in inventory
        /// /// Auther CJ
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public int checkOrder(string str)
        {
            SqlConnection conn = new SqlConnection(Properties.Settings.Default.scrumConnection);
            try
            {

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "Select Inventory from BookData where ISBN = @isbn";
                cmd.Parameters.AddWithValue("@isbn", str);
                conn.Open();
                int books = (int)cmd.ExecuteScalar();
                return books;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return -1;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        public List<string> getHistoryOfPurchasedBooks(int userID)
        {
            var listOfBooks = new List<string>();

            SqlConnection conn = new SqlConnection(Properties.Settings.Default.scrumConnection);

            try
            {
                conn.Open();
                string sql = "SELECT OrderItem.ISBN FROM Orders INNER JOIN OrderItem ON Orders.OrderID = OrderItem.OrderID WHERE Orders.UserID = ";
                sql += userID.ToString();
                using (var command = new SqlCommand(sql, conn))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string temp = reader[0].ToString();
                            listOfBooks.Add(temp);
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



            return listOfBooks;
        }
        public List<string> getListofRelation()
        {
            var listofRecs = new List<string>();
            SqlConnection conn = new SqlConnection(Properties.Settings.Default.scrumConnection);

            try
            {
                conn.Open();
                string sql = "Select Orders.UserID, OrderItem.ISBN FROM Orders INNER JOIN OrderItem  ON Orders.OrderID = OrderItem.OrderID order by Orders.UserID";
                using (var command = new SqlCommand(sql, conn))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string temp = reader[0].ToString();
                            listofRecs.Add(temp);
                            temp = reader[1].ToString();
                            listofRecs.Add(temp);
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString());
            }
            finally
            {
                conn.Close();
            }
            return listofRecs;
        }


    }
}
