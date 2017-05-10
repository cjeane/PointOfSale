using System.Windows;
using System.Windows.Media;

namespace ScrumStore
{
    /// <summary>
    /// Interaction logic for UserSettingsDialog.xaml
    /// </summary>
    public partial class UserSettingsDialog : Window
    {
        private UserData usr;
        private UserSettings sttngs;
        private SolidColorBrush defBG = new SolidColorBrush(Colors.LightGray);    // DEFAULT
        private SolidColorBrush selBG = (SolidColorBrush)Application.Current.Resources["WindowBrush"]; // DEFAULT
        private string colAsHex = Colors.LightGray.ToString();  // DEFAULT

        public UserSettingsDialog()
        {
            InitializeComponent();
            usr = UserData.getInstance();
            sttngs = new UserSettings();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (usr.LoggedIn == true)
            {
                var usrID = usr.UserID;

                // User info populates text boxes which can be edited
                this.fullname.Text = usr.FullName;
                this.addr.Text = usr.Address;
                this.postal.Text = usr.Postal;
                this.city.Text = usr.City;
                this.province.Text = usr.Province;
                this.email.Text = usr.Email;
                this.phone.Text = usr.Phone;
            }
        }

        private void savebutton_Click(object sender, RoutedEventArgs e)
        {
            string colStr = "";
            // start trying to save the different components (password, profile, bgcolor)
            int pwStat = 0;
            if (this.passOld.Password != "" || this.pass.Password != "" || this.passVerify.Password != "")
                pwStat = sttngs.savePass(this.passOld.Password, this.pass.Password, this.passVerify.Password, usr.UserID);

            int numSaved = sttngs.bulkSave(this.fullname.Text, this.addr.Text, this.postal.Text, this.city.Text,
                                            this.province.Text, this.email.Text, this.phone.Text, usr.UserID);

            if (selBG.Color != defBG.Color)
            {
                Application.Current.Resources["WindowBrush"] = selBG;
                if (sttngs.saveAppearance(colAsHex, usr.UserID) == false)
                    colStr += "Appearance failed to save\n";
                else
                    colStr += "Appearance saved as " + selBG.Color.ToString();
            }

            // display result-of-actions dialog
            if (pwStat < 0 || numSaved > 0 || colStr != "")
            {
                MessageBox.Show(sttngs.passUpd + sttngs.msgStr + colStr/* + numSaved*/);
            }
            else
            {
                MessageBox.Show("No changes made"/*sttngs.passUpd + sttngs.msgStr + numSaved*/);
                this.Close();
            }

            // cleanup
            sttngs.passUpd = "";
            sttngs.msgStr = "";
            colStr = "";

            if (numSaved > 0)
                usr.updateData();
        }
        private void cancelbutton_Click(object sender, RoutedEventArgs e)
        {
            this.Owner.Focus();
            this.Close();
        }
        private void historybutton_Click(object sender, RoutedEventArgs e)
        {
            OrderHistory ohdiag = new OrderHistory();
            ohdiag.Owner = this;
            this.Hide();
            ohdiag.Show();
        }
        private void updatePassbutton_Click(object sender, RoutedEventArgs e)
        {
            var flag = sttngs.savePass(this.passOld.Password, this.pass.Password, this.passVerify.Password, usr.UserID);
            if (flag == 1 || flag == -1)
            {
                MessageBox.Show(sttngs.passUpd);
                passOld.Password = "";
                pass.Password = "";
                passVerify.Password = "";
            }
            else
            {
                MessageBox.Show(sttngs.passUpd);
                if (flag == -2 || flag == -3)
                    passOld.Focus();
                if (flag == -4)
                {
                    passVerify.Password = "";
                    passVerify.Focus();
                }
            }
            sttngs.passUpd = "";
        }

        private void colorBtn_Click(object sender, RoutedEventArgs e)
        {
            // Requires System.Windows.Forms assembly reference
            var cdlg = new System.Windows.Forms.ColorDialog();

            if (cdlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // Requires System.Drawing assembly reference
                var col = Color.FromArgb(cdlg.Color.A, cdlg.Color.R, cdlg.Color.G, cdlg.Color.B);
                var brush = new SolidColorBrush(col);

                colAsHex = brush.Color.ToString();

                this.Background = brush;
                selBG = brush;
            }
        }

        private void defaultBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Background = defBG;
            selBG = defBG;
        }

        // TESTING EMAIL FUNCTION
        /*
        private void testBtn_Click(object sender, RoutedEventArgs e)
        {
            Receipt r = new Receipt();
            if (r.sendReceiptEmail(5) == true)
                MessageBox.Show("The receipt has been sent!");
            else
                MessageBox.Show("Debug required");
        }*/
    }
}
