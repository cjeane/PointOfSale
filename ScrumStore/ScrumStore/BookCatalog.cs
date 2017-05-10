using System.Data;

namespace ScrumStore
{
    public class BookCatalog
    {
        public static DataTable topBookTable { get; set; }
        public DataSet GetBookInfo()
        {
            DALBookCatalog bookCatalog = new DALBookCatalog();
            return bookCatalog.GetBookInfo();
        }

        public DataSet getTopBooks()
        {
            DALBookCatalog bookCatalog = new DALBookCatalog();
            //topBookTable = bookCatalog.GetTopBooks();
            // GetTopBooks() return null lists -> try to populate the table manually to test if dataTable data is displayed in dataGrid first, 
            //then work to get the dataTable data from database for actual 
            //return topBookTable; 
            return bookCatalog.GetTopBooks();
        }
    }
}