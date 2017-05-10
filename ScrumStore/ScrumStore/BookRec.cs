using System;
using System.Collections.Generic;
using System.Linq;

namespace ScrumStore
{
    public class BookRec
    {

        int[,] adjMatrix;
        List<string> Books;



        public BookRec()
        {
            DALBookCatalog dalBook = new DALBookCatalog();
            DALOrder dalOrder = new DALOrder();
            List<string> relations;
            string temp;
            List<string> group = new List<string>();

            Books = dalBook.getListofBooks();

            adjMatrix = new int[Books.Count, Books.Count];

            for (int i = 0; i < adjMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < adjMatrix.GetLength(0); j++)
                {
                    adjMatrix[i, j] = 0;
                }
            }

            relations = dalOrder.getListofRelation();
            for (int i = 0; i < relations.Count; i += 2)
            {
                group.Clear();
                temp = relations.ElementAt(i);
                group.Add(relations.ElementAt(i + 1));
                while (i < relations.Count - 2 && temp.Equals(relations.ElementAt(i + 2), StringComparison.Ordinal))
                {
                    group.Add(relations.ElementAt(i + 3));
                    i += 2;
                }

                for (int x = 0; x < group.Count; x++)
                {
                    for (int y = 0; y < group.Count; y++)
                    {
                        if (x == y)
                            continue;
                        adjMatrix[Books.IndexOf(group.ElementAt(x)),
                            Books.IndexOf(group.ElementAt(y))] += 1;
                    }
                }
            }
        }

        /// <summary>
        /// Tries to find a recommendation book for user based on current selection and pass history
        /// </summary>
        /// <param name="currentOrderInCart"></param>
        /// <param name="userID"></param>
        /// <returns>a list of three string 1 is the book the rec is based of off, 2 the book recommended and 3 the optional phrasing how the recommendation was found
        /// null null, phrase is the option when no recommendations are found</returns>
        public List<string> findRec(BookOrder currentOrderInCart, int userID)
        {
            List<string> recommendations = new List<string>();
            List<string> currentBooks = new List<string>();
            List<string> historyOfBooks = new List<string>();
            string bookRecID = "", bookRecFromID = "";
            int relationWeight = 0;

            foreach (var item in currentOrderInCart.OrderItemList)
                currentBooks.Add(item.BookID);

            if (userID > 0)
            {
                DALOrder dalOrder = new DALOrder();
                historyOfBooks = new List<string>();
                historyOfBooks = dalOrder.getHistoryOfPurchasedBooks(userID);
            }

            foreach (var book in currentBooks)
            {
                var bookIndex = Books.IndexOf(book);
                for (int i = 0; i < adjMatrix.GetLength(0); i++)
                {
                    if (adjMatrix[bookIndex, i] > relationWeight && !historyOfBooks.Contains(book) && !historyOfBooks.Contains(Books.ElementAt(i)))
                    {
                        bookRecFromID = book;
                        bookRecID = Books.ElementAt(i);
                        relationWeight = adjMatrix[bookIndex, i];
                    }
                }
            }

            if (historyOfBooks.Count != 0)
            {
                foreach (var book in historyOfBooks)
                {
                    var bookIndex = Books.IndexOf(book);
                    for (int i = 0; i < adjMatrix.GetLength(0); i++)
                    {
                        if (adjMatrix[bookIndex, i] > relationWeight && !currentBooks.Contains(book) && !historyOfBooks.Contains(Books.ElementAt(i)))
                        {
                            bookRecFromID = book;
                            bookRecID = Books.ElementAt(i);
                            relationWeight = adjMatrix[bookIndex, i];
                        }
                    }
                }
            }

            if (relationWeight > 0)
            {
                recommendations.Add(bookRecFromID);
                recommendations.Add(bookRecID);
                if (currentBooks.Contains(bookRecFromID))
                    recommendations.Add("Based on your current order of ");
                else
                    recommendations.Add("Based on your past order of ");
            }
            else
            {
                recommendations.Add(null);
                recommendations.Add(null);
                recommendations.Add("No recommendations found, sorry.");
            }



            return recommendations;
        }

        public string getRecommendationString(BookOrder order, int user)
        {
            string ans = "";

            List<string> li = findRec(order, user);
            if (li.ElementAt(0) == null)
                ans = li.ElementAt(2);
            else
            {
                DALBookCatalog dalOrder = new DALBookCatalog();
                ans = li.ElementAt(2) + dalOrder.getBookTitleFromId(li.ElementAt(0)) + " you may be interested in " + dalOrder.getBookTitleFromId(li.ElementAt(1));
            }

            return ans;
        }

    }



}
