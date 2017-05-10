using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

// Author: Angelico Salazar

namespace ScrumStore
{
    public class Register
    {

        static SqlConnection conn = new SqlConnection(Properties.Settings.Default.scrumConnection);

        public static Boolean registerUser(String username,
                                        String password,
                                        String passwordVerify,
                                        String email,
                                        String fullName,
                                        String address,
                                        String city,
                                        String province,
                                        String postalCode,
                                        String phone)
        {

            Boolean isValid = isValidUsername(username) &&
                              isValidPassword(password, passwordVerify) &&
                              isValidEmail(email) &&
                              isNotEmpty(fullName) &&
                              isNotEmpty(address) &&
                              isNotEmpty(city) &&
                              isNotEmpty(province) &&
                              isValidPostalCode(postalCode) &&
                              isValidPhone(phone);

            if (isValid)
            {

                String sql = "INSERT INTO UserData (" +
                    "[UserName] , [Password] , [Manager] , [FullName] , [Address]," +
                    "[PostalCode] , [EmailAddress] , [ContactNumber] , [Province] , [City] )" +
                    "VALUES (" +
                    "@UserName , @Password , @Manager , @FullName , @Address," +
                    "@PostalCode , @EmailAddress , @ContactNumber , @Province , @City )";

                conn.Open();

                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.CommandType = CommandType.Text;

                cmd.Parameters.AddWithValue("@UserName", username);
                cmd.Parameters.AddWithValue("@Password", password);
                cmd.Parameters.AddWithValue("@Manager", 0);
                cmd.Parameters.AddWithValue("@FullName", fullName);
                cmd.Parameters.AddWithValue("@Address", address);

                cmd.Parameters.AddWithValue("@PostalCode", postalCode);
                cmd.Parameters.AddWithValue("@EmailAddress", email);
                cmd.Parameters.AddWithValue("@ContactNumber", phone);
                cmd.Parameters.AddWithValue("@Province", province);
                cmd.Parameters.AddWithValue("@City", city);

                cmd.ExecuteNonQuery();

                conn.Close();

                return true;
            }
            else
            {

                return false;
            }
        }


        // static validation functions

        public static Boolean isNotEmpty(String field)
        {

            return !(String.IsNullOrEmpty(field) || String.IsNullOrWhiteSpace(field));
        }

        public static Boolean isValidUsername(String username)
        {

            String sql = "Select * from UserData where UserName = '" + username + "'";

            SqlCommand cmd = new SqlCommand(sql, conn);

            SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            da.Fill(dt);

            Boolean doesNotExist = !(dt.Rows.Count > 0);

            return isNotEmpty(username) && doesNotExist;
        }

        public static Boolean isValidPassword(String password, String passwordVerify)
        {

            // took this from UserData.cs for consistency

            return password.Equals(passwordVerify) && password.Length == 6 && char.IsLetter(password[0]) && Regex.IsMatch(password, @"^[a-zA-Z0-9]+$");
        }

        public static Boolean isValidEmail(String email)
        {

            return Regex.IsMatch(email, @"^([\w]+.)*[\w]+@[\w]+\.[\w]+$");
        }


        public static Boolean isValidPostalCode(String postalCode)
        {
            // based on the definition that postal code is 3 or more alphanumeric digits
            return Regex.IsMatch(postalCode, "[A-Z]\\d[A-Z]\\d[A-Z]\\d");
        }

        public static Boolean isValidPhone(String phone)
        {

            return Regex.IsMatch(phone, "[0-9]{7,}|(\\(\\d{3}\\)\\s\\d{3}-\\d{4})|(\\d{3}-\\d{3}-\\d{4})");
        }

    }
}
