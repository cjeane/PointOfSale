using System.Data.SqlClient;
using System.Data;

namespace ScrumStore
{
    public class DALUserSettings
    {
        private string connStr = Properties.Settings.Default.scrumConnection;

        //  SET PASSWORD
        public int setPassword(int usrID, string pw)
        {
            string CmdString = string.Empty;
            int ret;

            using (SqlConnection con = new SqlConnection(connStr))
            {
                CmdString = "UPDATE UserData SET Password = @pass WHERE UserID = @uid";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                cmd.Parameters.AddWithValue("@pass", pw);
                cmd.Parameters.AddWithValue("@uid", usrID);

                con.Open();
                ret = cmd.ExecuteNonQuery();
            }
            return ret;
        }

        //  SET FULL NAME
        public int setName(int usrID, string name)
        {
            string CmdString = string.Empty;
            int ret;

            using (SqlConnection con = new SqlConnection(connStr))
            {
                CmdString = "UPDATE UserData SET FullName = @name WHERE UserID = @uid";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@uid", usrID);

                con.Open();
                ret = cmd.ExecuteNonQuery();
            }
            return ret;
        }

        //  SET ADDRESS
        public int setAddress(int usrID, string addr)
        {
            string CmdString = string.Empty;
            int ret;

            using (SqlConnection con = new SqlConnection(connStr))
            {
                CmdString = "UPDATE UserData SET Address = @addr WHERE UserID = @uid";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                cmd.Parameters.AddWithValue("@addr", addr);
                cmd.Parameters.AddWithValue("@uid", usrID);

                con.Open();
                ret = cmd.ExecuteNonQuery();
            }

            return ret;
        }

        //  SET POSTAL CODE
        public int setPostal(int usrID, string post)
        {
            string CmdString = string.Empty;
            int ret;

            using (SqlConnection con = new SqlConnection(connStr))
            {
                CmdString = "UPDATE UserData SET PostalCode = @post WHERE UserID = @uid";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                cmd.Parameters.AddWithValue("@post", post);
                cmd.Parameters.AddWithValue("@uid", usrID);

                con.Open();
                ret = cmd.ExecuteNonQuery();
            }
            return ret;
        }

        //  SET CITY
        public int setCity(int usrID, string city)
        {
            string CmdString = string.Empty;
            int ret;

            using (SqlConnection con = new SqlConnection(connStr))
            {
                CmdString = "UPDATE UserData SET City = @city WHERE UserID = @uid";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                cmd.Parameters.AddWithValue("@city", city);
                cmd.Parameters.AddWithValue("@uid", usrID);

                con.Open();
                ret = cmd.ExecuteNonQuery();
            }
            return ret;
        }

        //  SET PROVINCE
        public int setProvince(int usrID, string prov)
        {
            string CmdString = string.Empty;
            int ret;

            using (SqlConnection con = new SqlConnection(connStr))
            {
                CmdString = "UPDATE UserData SET Province = @prov WHERE UserID = @uid";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                cmd.Parameters.AddWithValue("@prov", prov);
                cmd.Parameters.AddWithValue("@uid", usrID);

                con.Open();
                ret = cmd.ExecuteNonQuery();
            }
            return ret;
        }

        //  SET EMAIL
        public int setEmail(int usrID, string em)
        {
            string CmdString = string.Empty;
            int ret;

            using (SqlConnection con = new SqlConnection(connStr))
            {
                CmdString = "UPDATE UserData SET EmailAddress = @email WHERE UserID = @uid";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                cmd.Parameters.AddWithValue("@email", em);
                cmd.Parameters.AddWithValue("@uid", usrID);

                con.Open();
                ret = cmd.ExecuteNonQuery();
            }
            return ret;
        }

        //  SET PHONE NUMBER
        public int setPhone(int usrID, string phone)
        {
            string CmdString = string.Empty;
            int ret;

            using (SqlConnection con = new SqlConnection(connStr))
            {
                CmdString = "UPDATE UserData SET ContactNumber = @cn WHERE UserID = @uid";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                cmd.Parameters.AddWithValue("@cn", phone);
                cmd.Parameters.AddWithValue("@uid", usrID);

                con.Open();
                ret = cmd.ExecuteNonQuery();
            }
            return ret;
        }

        /* --- OLD CODE ---
                public int setContact(string email, string phone, int usrID)
                {
                    var conn = new SqlConnection(Properties.Settings.Default.scrumConnection);
                    try
                    {
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = conn;
                        cmd.CommandText = "UPDATE UserData SET EmailAddress = @em, ContactNumber = @ph WHERE UserID = @uid";

                        cmd.Parameters.AddWithValue("@em", email);
                        cmd.Parameters.AddWithValue("@ph", phone);
                        cmd.Parameters.AddWithValue("@uid", usrID);

                        conn.Open();
                        return cmd.ExecuteNonQuery();
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
        ------ */

        /********************************************* ORDER HISTORY SECTION **************************************************/

        /// <summary>
        ///     GET ORDER HISTORY
        /// </summary>
        /// <returns></returns>
        public DataSet getOrderHistory(int usr)
        {
            string CmdString = string.Empty;
            DataSet dsOrders;

            using (SqlConnection con = new SqlConnection(connStr))
            {
                // Add a table "Orders" to dataset
                CmdString = "SELECT OrderID, OrderDate FROM Orders WHERE UserID = @usr";
                SqlCommand cmdSelOrdID = new SqlCommand(CmdString, con);
                cmdSelOrdID.Parameters.AddWithValue("@usr", usr);
                SqlDataAdapter sdaOrder = new SqlDataAdapter(cmdSelOrdID);
                dsOrders = new DataSet("myOrderDS");
                sdaOrder.Fill(dsOrders, "Orders");

                // Add a table "OrderItem" to dataset
                CmdString = "SELECT OrderItem.OrderID, BookData.Title, OrderItem.ISBN, "
                   + "BookData.Price, OrderItem.Quantity, BookData.Price * OrderItem.Quantity AS SubTotal "
                   + "FROM OrderItem "
                   + "JOIN Orders ON OrderItem.OrderID=Orders.OrderID "
                   + "JOIN BookData ON OrderItem.ISBN=BookData.ISBN "
                   + "WHERE UserID = @usr";
                SqlCommand cmdOrderItem = new SqlCommand(CmdString, con);
                cmdOrderItem.Parameters.AddWithValue("@usr", usr);
                SqlDataAdapter sdaOrderBook = new SqlDataAdapter(cmdOrderItem);
                sdaOrderBook.Fill(dsOrders, "OrderItem");

                // Create a relation between Orders and OrderItem and add to dataset
                DataRelation drOrder_OrderItem = new DataRelation
                    ("drOrder_OrderItem", dsOrders.Tables["Orders"].Columns["OrderID"],
                      dsOrders.Tables["OrderItem"].Columns["OrderID"], false);
                dsOrders.Relations.Add(drOrder_OrderItem);


                /*/-------- CURRENTLY DOESNT WORK
                CmdString = "SELECT OrderItem.OrderID, SUM (BookData.Price * OrderItem.Quantity) AS Total "
                  + "FROM OrderItem "
                  + "JOIN Orders ON OrderItem.OrderID=Orders.OrderID "
                  + "JOIN BookData ON OrderItem.ISBN=BookData.ISBN "
                  + "WHERE UserID = @usr "
                  + "GROUP BY OrderItem.OrderID";
                SqlCommand cmdOT = new SqlCommand(CmdString, con);
                cmdOT.Parameters.AddWithValue("@usr", usr);
                SqlDataAdapter sdaOT = new SqlDataAdapter(cmdOT);
                sdaOT.Fill(dsOrders, "OrderTotal");

                DataRelation drBig_Order = new DataRelation ("drOrder_OrderTotal", 
                    new DataColumn[] { dsOrders.Tables["Orders"].Columns["OrderID"], dsOrders.Tables["OrderItem"].Columns["OrderID"] },
                    new DataColumn[] { dsOrders.Tables["Orders"].Columns["OrderID"], dsOrders.Tables["OrderTotal"].Columns["OrderID"] });
                dsOrders.Relations.Add(drBig_Order);*/
            }

            return dsOrders;
        }

        // RECEIPT OF THE USER'S ORDER FOR EMAIL PURPOSE
        public DataSet getOrderReceipt(int usrID, int orderID)
        {
            string CmdString = string.Empty;
            DataSet order;

            using (SqlConnection con = new SqlConnection(connStr))
            {
                CmdString = "SELECT BD.Title, OI.ISBN, "
                    + "BD.Price, OI.Quantity, BD.Price* OI.Quantity AS SubTotal "
                    + "FROM OrderItem OI "
                    + "JOIN Orders O ON OI.OrderID = O.OrderID "
                    + "JOIN BookData BD ON OI.ISBN = BD.ISBN "
                    + "WHERE O.UserID = @uid AND O.OrderID = @oid";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                cmd.Parameters.AddWithValue("@uid", usrID);
                cmd.Parameters.AddWithValue("@oid", orderID);

                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                order = new DataSet("dsOrderReceipt");
                sda.Fill(order, "OrderReceipt");
            }
            return order;
        }


        /********************************************* BACKGROUND COLOR SECTION **************************************************/

        public string getUserBG(int usrID)
        {
            string CmdString = string.Empty;
            string retS;

            using (SqlConnection con = new SqlConnection(connStr))
            {
                CmdString = "SELECT BgColor FROM UserAppSettings WHERE UserID = @uid";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                cmd.Parameters.AddWithValue("@uid", usrID);

                con.Open();
                retS = cmd.ExecuteScalar().ToString();
            }
            return retS;
        }

        public int setUserBG(int usrID, string bg)
        {
            string CmdString = string.Empty;
            int ret;

            using (SqlConnection con = new SqlConnection(connStr))
            {
                CmdString = "UPDATE UserAppSettings SET BgColor = @bg WHERE UserID = @uid";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                cmd.Parameters.AddWithValue("@bg", bg);
                cmd.Parameters.AddWithValue("@uid", usrID);

                con.Open();
                ret = cmd.ExecuteNonQuery();
            }
            return ret;
        }

        public int newUserAppSetting(int usrID, string bg)
        {
            string CmdString = string.Empty;
            int ret;

            using (SqlConnection con = new SqlConnection(connStr))
            {
                CmdString = "INSERT INTO UserAppSettings VALUES (@uid, @bg)";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                cmd.Parameters.AddWithValue("@bg", bg);
                cmd.Parameters.AddWithValue("@uid", usrID);

                con.Open();
                ret = cmd.ExecuteNonQuery();
            }
            return ret;
        }

        public int hasAppSettings(int usrID)
        {
            string CmdString = string.Empty;
            int ret;

            using (SqlConnection con = new SqlConnection(connStr))
            {
                CmdString = "SELECT UserID FROM UserAppSettings WHERE UserID = @uid";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                cmd.Parameters.AddWithValue("@uid", usrID);

                con.Open();
                var res = cmd.ExecuteScalar();
                if (res != null)
                    ret = (int)res;
                else
                    ret = -1;
            }
            return ret;
        }
    }
}
