using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace ScrumStore
{
    public class PreviewBook
    {

        static SqlConnection conn = new SqlConnection(Properties.Settings.Default.scrumConnection);

        public static Boolean imageExists(String image)
        {

            return File.Exists(image);
        }

        public static Boolean bookExists(String book)
        {

            String sql = "Select * from BookData where Title = '" + book + "'";

            SqlCommand cmd = new SqlCommand(sql, conn);

            SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataSet dsBooks = new DataSet("Books");
            DataTable dt = new DataTable();
            da.Fill(dt);

            return dt.Rows.Count > 0;
        }

        public static double getPrice(String book)
        {
            try
            {

                String sql = "Select * from BookData where Title = '" + book + "'";

                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataSet dsBooks = new DataSet("Books");
                DataTable dt = new DataTable();
                da.Fill(dt);

                return Double.Parse(dt.Rows[0]["Price"].ToString());

            }
            catch
            {

                return 0;
            }
        }

        public static float getRating(String book)
        {
            try
            {
                String sql = "SELECT Rating from BookData WHERE Title = '" + book + "'";

                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataSet dsBooks = new DataSet("Books");
                DataTable dt = new DataTable();
                da.Fill(dt);

                return float.Parse(dt.Rows[0]["Rating"].ToString());

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }


        //Author: Randy Daigle

        public DataSet getTopBookFeedback(String book)
        {
            try
            {
                String sql = "SELECT TOP(3) FullName, Comment FROM BookData, Rating, UserData WHERE(ISBN = BookISBN) AND(UserData.UserID = Rating.UserID) AND Title = '" + book + "' AND Comment !=''";
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet dsComments = new DataSet("Comments");
                da.Fill(dsComments, "Comments");
                return dsComments;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
