/* **********************************************************************************
 * For use by students taking 60-422 (Fall, 2014) to work on assignments and project.
 * Permission required material. Contact: xyuan@uwindsor.ca 
 * **********************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace ScrumStore
{
    public class OrderItem
    {


        public string BookID { get; set; }
        public string BookTitle { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
        public double SubTotal { get; set; }

        public OrderItem(String isbn, String title,
            double unitPrice, int quantity)
        {
            BookID = isbn;
            BookTitle = title;
            UnitPrice = unitPrice;
            Quantity = quantity;
            SubTotal = UnitPrice * Quantity;
        }

        public bool checkPaymentInfo()
        {
            return true;
        }

        public void updateTotal()
        {
            SubTotal = UnitPrice * Quantity;
        }
        public override string ToString()
        {
            string xml = "<OrderItem ISBN='" + BookID + "'";
            xml += " Quantity='" + Quantity + "' />";
            return xml;
        }
    }
}
