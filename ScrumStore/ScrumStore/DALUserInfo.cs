/* **********************************************************************************
 * For use by students taking 60-422 (Fall, 2014) to work on assignments and project.
 * Permission required material. Contact: xyuan@uwindsor.ca 
 * **********************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Data;

namespace ScrumStore
{
    public class DALUserInfo
    {
        public int LogIn(string userName, string password)
        {
            var conn = new SqlConnection(Properties.Settings.Default.scrumConnection);

            try
            {

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "Select UserID from UserData where UserName = @UserName and Password = @Password ";
                cmd.Parameters.AddWithValue("@UserName", userName);
                cmd.Parameters.AddWithValue("@Password", password);
                conn.Open();
                int userId = (int)cmd.ExecuteScalar();
                if (userId > 0) return userId;
                else return -1;
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
        public string getName(string userName, string password)
        {
            var conn = new SqlConnection(Properties.Settings.Default.scrumConnection);
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "Select FullName from UserData where "
                    + " UserName = @UserName and Password = @Password ";
                cmd.Parameters.AddWithValue("@UserName", userName);
                cmd.Parameters.AddWithValue("@Password", password);
                conn.Open();

                return cmd.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return "";
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }
        public string getAddress(string user)
        {
            var conn = new SqlConnection(Properties.Settings.Default.scrumConnection);
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "Select Address from UserData where "
                    + " userID = @user";
                cmd.Parameters.AddWithValue("@user", user);
                conn.Open();

                return cmd.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return "";
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        public string getName(string user)
        {
            var conn = new SqlConnection(Properties.Settings.Default.scrumConnection);
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "Select FullName from UserData where "
                    + " userID = @user";
                cmd.Parameters.AddWithValue("@user", user);
                conn.Open();

                return cmd.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return "";
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }



        public string getPostal(string user)
        {
            var conn = new SqlConnection(Properties.Settings.Default.scrumConnection);
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "Select PostalCode from UserData where "
                    + " userID = @user";
                cmd.Parameters.AddWithValue("@user", user);
                conn.Open();

                return cmd.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return "";
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        public string getProvince(string user)
        {
            var conn = new SqlConnection(Properties.Settings.Default.scrumConnection);
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "Select Province from UserData where "
                    + " userID = @user";
                cmd.Parameters.AddWithValue("@user", user);
                conn.Open();

                return cmd.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return "";
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        public string getCity(string user)
        {
            var conn = new SqlConnection(Properties.Settings.Default.scrumConnection);
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "Select City from UserData where "
                    + " userID = @user";
                cmd.Parameters.AddWithValue("@user", user);
                conn.Open();
                return cmd.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return "";
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        public string getEmail(string user)
        {
            var conn = new SqlConnection(Properties.Settings.Default.scrumConnection);
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "Select EmailAddress from UserData where "
                    + " userID = @user";
                cmd.Parameters.AddWithValue("@user", user);
                conn.Open();
                return cmd.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return "";
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        public string getPhone(string user)
        {
            var conn = new SqlConnection(Properties.Settings.Default.scrumConnection);
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "Select ContactNumber from UserData where "
                    + " userID = @user";
                cmd.Parameters.AddWithValue("@user", user);
                conn.Open();
                return cmd.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return "";
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        public bool getManager(string user)
        {
            var conn = new SqlConnection(Properties.Settings.Default.scrumConnection);
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "Select Manager from UserData where "
                    + " userID = @user";
                cmd.Parameters.AddWithValue("@user", user);
                conn.Open();
                string val = cmd.ExecuteScalar().ToString();
                if (val == "" || val == null || val.Equals("False"))
                    return false;
                else
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

        public void updateShipping(string name, string address, string city, string postal, string province, string user)
        {
            var conn = new SqlConnection(Properties.Settings.Default.scrumConnection);
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "UPDATE UserData "
                    + "SET FullName = @name,"
                    + " Address = @address,"
                    + " City = @city,"
                    + " PostalCode = @postal,"
                    + " Province = @province "
                    + " where userID = @user";
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@address", address);
                cmd.Parameters.AddWithValue("@city", city);
                cmd.Parameters.AddWithValue("@postal", postal);
                cmd.Parameters.AddWithValue("@province", province);
                cmd.Parameters.AddWithValue("@user", user);
                conn.Open();
                cmd.ExecuteNonQuery();
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
        }

    }


}
