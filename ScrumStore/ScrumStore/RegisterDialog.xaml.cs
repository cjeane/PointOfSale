using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ScrumStore
{
    /// <summary>
    /// Interaction logic for RegisterDialog.xaml
    /// </summary>
    public partial class RegisterDialog : Window
    {

        private SolidColorBrush validColor = new SolidColorBrush(Colors.LightGreen);
        private SolidColorBrush invalidColor = new SolidColorBrush(Colors.Red);

        public RegisterDialog()
        {
            InitializeComponent();
        }

        private void registerBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Register.registerUser(username.Text,
                                      password.Password,
                                      passwordVerify.Password,
                                      email.Text,
                                      fullName.Text,
                                      address.Text,
                                      city.Text,
                                      province.Text,
                                      postalCode.Text,
                                      phone.Text))
            {
                MessageBox.Show("SUCCESSFUL REGISTRATION\nYOU CAN NOW LOGIN");
                this.Close();
            }
            else
            {
                MessageBox.Show("FAILED REGISTRATION\nINVALID FIELDS ARE LABELLED IN RED");
            }
        }

        private void username_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Register.isValidUsername(username.Text))
            {
                usernameLabel.Foreground = validColor;
            }
            else
            {
                usernameLabel.Foreground = invalidColor;
            }
        }

        private void password_PasswordChanged(object sender, RoutedEventArgs e)
        {

            if (Register.isValidPassword(password.Password, passwordVerify.Password))
            {
                passwordLabel.Foreground = validColor;
            }
            else
            {
                passwordLabel.Foreground = invalidColor;
            }
        }

        private void passwordVerify_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (Register.isValidPassword(password.Password, passwordVerify.Password))
            {
                passwordLabel.Foreground = validColor;
            }
            else
            {
                passwordLabel.Foreground = invalidColor;
            }
        }

        private void email_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Register.isValidEmail(email.Text))
            {
                emailLabel.Foreground = validColor;
            }
            else
            {
                emailLabel.Foreground = invalidColor;
            }
        }

        private void fullName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Register.isNotEmpty(fullName.Text))
            {
                fullNameLabel.Foreground = validColor;
            }
            else
            {
                fullNameLabel.Foreground = invalidColor;
            }
        }

        private void address_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Register.isNotEmpty(address.Text))
            {
                addressLabel.Foreground = validColor;
            }
            else
            {
                addressLabel.Foreground = invalidColor;
            }
        }

        private void city_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Register.isNotEmpty(city.Text))
            {
                cityLabel.Foreground = validColor;
            }
            else
            {
                cityLabel.Foreground = invalidColor;
            }
        }

        private void province_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Register.isNotEmpty(province.Text))
            {
                provinceLabel.Foreground = validColor;
            }
            else
            {
                provinceLabel.Foreground = invalidColor;
            }
        }

        private void postalCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Register.isValidPostalCode(postalCode.Text))
            {
                postalCodeLabel.Foreground = validColor;
            }
            else
            {
                postalCodeLabel.Foreground = invalidColor;
            }
        }

        private void phone_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Register.isValidPhone(phone.Text))
            {
                phoneLabel.Foreground = validColor;
            }
            else
            {
                phoneLabel.Foreground = invalidColor;
            }
        }

    }
}
