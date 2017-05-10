using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

//Author: Randy Daigle

namespace ScrumStore
{
    public class RatingInfo
    {
        public int RatingID { get; set; }
        public string BookISBN { get; set; }
        public int UserID { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }

        public void SetUserRating(string bookNumber, int userID, int userRating, string bookComment)
        {
            using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.scrumConnection))
            {
                try
                {
                    if (bookNumber != null || bookNumber != "")
                    {
                        if (userID > 0)
                        {
                            if (userRating >= 1 && userRating <= 5)
                            {
                                conn.Open();
                                string query = "INSERT INTO Rating(BookISBN, UserID, Rating, Comment) VALUES(@bookISBN, @userID, @rating, @comment)";
                                SqlCommand cmd = new SqlCommand(query, conn);
                                cmd.CommandType = CommandType.Text;
                                cmd.Parameters.AddWithValue("@bookISBN", bookNumber);
                                cmd.Parameters.AddWithValue("@userID", userID);
                                cmd.Parameters.AddWithValue("@rating", userRating);
                                cmd.Parameters.AddWithValue("@comment", bookComment);
                                cmd.ExecuteNonQuery();

                                SqlCommand updateRating = new SqlCommand("UpdateRating", conn);
                                updateRating.ExecuteNonQuery();
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
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
            }
        }
    }
}
