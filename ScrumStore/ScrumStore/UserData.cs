/* **********************************************************************************
 * For use by students taking 60-422 (Fall, 2014) to work on assignments and project.
 * Permission required material. Contact: xyuan@uwindsor.ca 
 * **********************************************************************************/
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;

namespace ScrumStore
{
    public class UserData
    {
        private static UserData instance = new UserData();  // Changed UserData to a singleton -kevin
        public int UserID { set; get; }
        public string LoginName { set; get; }
        public string Password { get; set; }
        public Boolean LoggedIn { set; get; }
        public string FullName { set; get; }
        public string Address { set; get; }
        public string Postal { set; get; }
        public string Province { set; get; }
        public string City { set; get; }
        public string Email { set; get; }
        public string Phone { set; get; }
        public Boolean Manager { set; get; }

        private UserData() { }
        public static UserData getInstance() { return instance; }


        public void updateData()
        {
            if (LoggedIn)
            {
                var dbUser = new DALUserInfo();
                FullName = dbUser.getName(UserID.ToString());
                Address = dbUser.getAddress(UserID.ToString());
                Postal = dbUser.getPostal(UserID.ToString());
                Province = dbUser.getProvince(UserID.ToString());
                City = dbUser.getCity(UserID.ToString());
                Email = dbUser.getEmail(UserID.ToString());
                Phone = dbUser.getPhone(UserID.ToString());
                Manager = dbUser.getManager(UserID.ToString());
            }
        }
        public Boolean LogIn(string loginName, string password)
        {
            var dbUser = new DALUserInfo();
            if (loginName != null && password != null)
            {
                if (password.Length == 6 && char.IsLetter(password[0]) && Regex.IsMatch(password, @"^[a-zA-Z0-9]+$"))
                {
                    UserID = dbUser.LogIn(loginName, password);
                    if (UserID > 0)
                    {
                        LoginName = loginName;
                        Password = password;
                        LoggedIn = true;
                        FullName = dbUser.getName(loginName, password);
                        Address = dbUser.getAddress(UserID.ToString());
                        Postal = dbUser.getPostal(UserID.ToString());
                        Province = dbUser.getProvince(UserID.ToString());
                        City = dbUser.getCity(UserID.ToString());
                        Email = dbUser.getEmail(UserID.ToString());
                        Phone = dbUser.getPhone(UserID.ToString());
                        Manager = dbUser.getManager(UserID.ToString());
                        return true;
                    }
                    else
                    {
                        LoggedIn = false;
                        return false;
                    }
                }
                else
                    return false;
            }
            else
                return false;
        }

        public Boolean infoSet()
        {
            if (FullName.Length == 0 || Address.Length == 0 || Postal.Length != 6 || Province.Length == 0 || City.Length == 0)
                return false;
            return true;
        }

        public string validateShippingInfo()
        {
            Boolean err = false;
            string listOfProvince = "ontario|quebec|nova scotia|new brunswick|manitoba|british columbia|"
                + "prince Edward Island|Saskatchewan|alberta|newfoundland|newfoundland and labrador"
                + "|^(on)$|^qc$|^ns$|^nb$|^mb$|^bc$|^pe$|^sk$|^ab$|^nl$";
            string postalreg = "[ABCEGHJKLMNPRSTVXY][0-9][ABCEGHJKLMNPRSTVWXYZ][0-9][ABCEGHJKLMNPRSTVWXYZ][0-9]";
            string error = "Please fix the following:\n";

            if (FullName.Length == 0)
            {
                error += "Name is blank!\n";
                err = true;
            }
            if (Address.Length == 0)
            {
                error += "Address is blank!\n";
                err = true;
            }
            if (City.Length == 0)
            {
                error += "City is blank!\n";
                err = true;
            }
            if (Postal.Length == 0)
            {
                error += "Postal code is blank!\n";
                err = true;
            }
            if (Province.Length == 0)
            {
                error += "Province is blank!\n";
                err = true;
            }

            if (!Regex.IsMatch(Province, listOfProvince, RegexOptions.IgnoreCase))
            {
                error += "Province name is incorrect.\n";
                err = true;
            }
            if (!Regex.IsMatch(Postal, postalreg, RegexOptions.IgnoreCase))
            {
                error += "Postal Code must be in the form A1A1A1 and contain no spaces.\n";
                err = true;
            }

            if (err)
                return error;
            return null;
        }
        public void updateShippingInfo()
        {
            var dbUser = new DALUserInfo();
            if (LoggedIn)
            {
                dbUser.updateShipping(FullName, Address, City, Postal, Province, UserID.ToString());
            }
        }

        public void Logout()
        {
            LoggedIn = false;
            UserID = -1;
            LoginName = "";
            Password = "";
            FullName = "";
            Address = "";
            Postal = "";
            Province = "";
            City = "";
            Email = "";
            Phone = "";
        }

        public void loadAppearance()
        {
            DALUserSettings Sttngs = new DALUserSettings();

            if (Sttngs.hasAppSettings(UserID) == UserID)
                Application.Current.Resources["WindowBrush"] = (SolidColorBrush)(new BrushConverter().ConvertFrom(Sttngs.getUserBG(UserID)));
            else
                setDefaultAppearance();
            //Application.Current.Resources["WindowBrush"] = new SolidColorBrush(Colors.Red);
        }

        public void setDefaultAppearance()
        {
            Application.Current.Resources["WindowBrush"] = new SolidColorBrush(Colors.LightGray);
        }
    }
}
