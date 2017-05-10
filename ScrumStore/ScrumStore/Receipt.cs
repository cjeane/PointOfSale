using System;
using System.Configuration; // Requires assembly reference to System.Configuration
using System.Data;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace ScrumStore
{
    class Receipt
    {
        private DataSet dsReceipt;
        private DALUserSettings Sttngs;
        private UserData Session;
        private string SmtpServerName { get; set; }
        private int Port { get; set; }
        private string SenderEmail { get; set; }
        private string SenderPassword { get; set; }
        private string RecipientEmail { get; set; }
        private string RecipientName { get; set; }

        public Receipt()
        {
            Sttngs = new DALUserSettings();
            Session = UserData.getInstance();
            SmtpServerName = ConfigurationManager.AppSettings["SmtpServer"];
            Port = Int32.Parse(ConfigurationManager.AppSettings["Port"]);
            SenderEmail = ConfigurationManager.AppSettings["SenderEmail"];
            SenderPassword = ConfigurationManager.AppSettings["SenderPassword"];
            RecipientEmail = Session.Email;
            RecipientName = Session.FullName;
        }

        public bool sendReceiptEmail(int orderId)
        {
            bool mailSuccess;

            dsReceipt = Sttngs.getOrderReceipt(Session.UserID, orderId);
            DataTable dtReceipt = dsReceipt.Tables["OrderReceipt"];

            try
            {
                MailMessage mail = new MailMessage(
                        new MailAddress(SenderEmail, "ScrumStore Project"),
                        new MailAddress(RecipientEmail, RecipientName));

                SmtpClient SmtpServer = new SmtpClient(SmtpServerName);

                // Fill email
                mail.Subject = "Order# " + orderId + " - ScrumStore Purchase Receipt";
                mail.IsBodyHtml = true;
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Hello " + RecipientName + ",<br>");
                sb.AppendLine("<p>Here is your receipt for Order# " + orderId + "</p>");
                sb.AppendLine("<p>You've made the following purchases:</p>");
                sb.AppendLine(orderToHTML(dtReceipt));
                sb.AppendLine("<p>Please keep this as a record of your purchase.</p>");
                sb.AppendLine("Thank you,<br>");
                sb.AppendLine("ScrumStore Team");
                mail.Body = sb.ToString();

                // Log into SMTP server to send mail
                SmtpServer.Credentials = new NetworkCredential(SenderEmail, SenderPassword);
                SmtpServer.Port = Port;
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
                mailSuccess = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                mailSuccess = false;
            }
            return mailSuccess;
        }

        private string orderToHTML(DataTable dt)
        {
            StringBuilder rb = new StringBuilder();
            object sum = dt.Compute("Sum(SubTotal)", "");

            //rb.Append("<table border='1px' cellpadding='5' cellspacing='0' ");
            //rb.Append("style='border: solid 1px Silver; font-size: x-small;'>");

            rb.Append("<table cellpadding='10px' ");
            rb.Append("style='border-collapse: collapse; font-family: Arial; font-size: 12px;'>");

            rb.Append("<tr align='left' valign='top'>");
            foreach (DataColumn col in dt.Columns)
            {
                rb.Append("<th align='left' valign='top' ");
                rb.Append("style='background-color: #f5f5f5;'>");
                rb.Append(col.ColumnName);
                rb.Append("</th>");
            }
            rb.Append("</tr>");

            foreach (DataRow row in dt.Rows)
            {
                rb.Append("<tr align='left' valign='top'>");
                foreach (DataColumn col in dt.Columns)
                {
                    rb.Append("<td align='left' valign='top' ");
                    rb.Append("style='border-bottom: 1px solid #ddd; font-family: Monospace; font-size: 11px;'>");
                    rb.Append(row[col.ColumnName].ToString());
                    rb.Append("</td>");
                }
                rb.Append("</tr>");
            }

            var colCount = dt.Columns.Count;
            rb.Append("<tr>");
            for (int i = 1; i <= colCount; i++)
            {
                if (i >= colCount - 1)
                {
                    rb.Append("<th align='right' valign='top' style='background-color: #f5f5f5'>Total</th>");
                    rb.Append("<td align='left' valign='top' "
                        + "style='border-bottom: 1px solid #ddd; font-family: Monospace; font-size: 11px;'>"
                        + sum.ToString() + "</td>");
                    break;
                }
                else
                    rb.Append("<td style='background-color: #f5f5f5'></td>");
            }
            rb.Append("</tr>");
            rb.Append("</table>");

            return rb.ToString();
        }
    }
}
