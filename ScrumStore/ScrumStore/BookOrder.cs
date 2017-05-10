using System;
using System.Collections.ObjectModel;

namespace ScrumStore
{
    public class BookOrder
    {
        ObservableCollection<OrderItem> orderItemList = new ObservableCollection<OrderItem>();

        public ObservableCollection<OrderItem> OrderItemList
        {
            get { return orderItemList; }
        }

        public void AddItem(OrderItem orderItem)
        {
            foreach (var item in orderItemList)
            {
                if (item.BookID == orderItem.BookID)
                {
                    item.Quantity += orderItem.Quantity;
                    item.updateTotal();
                    return;
                }
            }
            orderItemList.Add(orderItem);
        }

        public void RemoveItem(string productID)
        {
            foreach (var item in orderItemList)
            {
                if (item.BookID == productID)
                {
                    orderItemList.Remove(item);
                    item.updateTotal();
                    return;
                }
            }
        }

        public void RemoveItem(string productID, int x)
        {
            foreach (var item in orderItemList)
            {
                if (item.BookID == productID)
                {
                    if (x < item.Quantity && x > 0)
                    {
                        item.Quantity -= x;
                        item.updateTotal();
                    }
                    else if (x >= item.Quantity)
                    {
                        orderItemList.Remove(item);
                        item.updateTotal();
                        return;
                    }
                }
            }
        }

        public void AddItem(string productID, int x)
        {
            foreach (var item in orderItemList)
            {
                if (item.BookID == productID)
                {
                    if (x > 0)
                    {
                        item.Quantity += x;
                        item.updateTotal();
                    }

                }
            }
        }

        public string validateOrder()
        {
            DALOrder dOrder = new DALOrder();
            string temp = "Error regarding books:\n";
            bool flag = false;

            foreach (var item in orderItemList)
            {
                if (item.Quantity > dOrder.checkOrder(item.BookID))
                {
                    temp += item.BookTitle + " only have " + dOrder.checkOrder(item.BookID) + " in stock.\n";
                    flag = true;
                }
            }
            if (!flag)
                return null;
            else
                return temp;
        }


        public double GetOrderTotal()
        {
            if (orderItemList.Count == 0)
            {
                return 0.00;
            }
            else
            {
                double total = 0;
                foreach (var item in orderItemList)
                {
                    total += item.SubTotal;
                }
                return Math.Round(total, 2);
            }
        }

        public double getOrderTax()
        {
            return Math.Round(GetOrderTotal() * 0.13, 2);
        }


        public double getGrandTotal(double discount)
        {
            return Math.Round(GetOrderTotal() + getOrderTax() - discount, 2);
        }

        public bool placeOrder(int userID)
        {
            return true;
        }
        //public int PlaceOrder(int userID)
        //{
        //    string xmlOrder;
        //    xmlOrder = "<Order UserID='" + userID.ToString() + "'>";
        //    foreach (var item in orderItemList)
        //    {
        //        xmlOrder += item.ToString();
        //    }
        //    xmlOrder += "</Order>";
        //    DALOrder dbOrder = new DALOrder();
        //    return dbOrder.Proceed2Order(xmlOrder);
        //}


    }
}