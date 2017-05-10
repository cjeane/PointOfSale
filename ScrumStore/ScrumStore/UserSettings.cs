using System;
using System.Text.RegularExpressions;

namespace ScrumStore
{
    public class UserSettings
    {
        private DALUserSettings Sttng = new DALUserSettings();
        private UserData Session = UserData.getInstance();
        public string msgStr { set; get; }
        public string passUpd { set; get; }

        public UserSettings()
        {
            passUpd = "";
            msgStr = "";
        }

        public int bulkSave(string name, string address, string postal, string city,
                             string province, string email, string phone, int usrID)
        {
            // not very thought out regexs
            string namePattern = "[a-zA-Z]*?\\s?\\,?\\s?[a-zA-Z]+";
            string addrPattern = "\\d+\\s[a-zA-Z]+(\\s[a-zA-Z]+)?";
            string postPattern = "[a-zA-Z]\\d[a-zA-Z]\\s?\\d[a-zA-Z]\\d";
            string cityPattern = "[a-zA-Z]+";
            string provPattern = "[a-zA-Z]+";
            string emailPattern = "[\\w]+@[\\w]+\\.[\\w]{2,}";
            string phonePattern = "[0-9]{7,}|(\\(\\d{3}\\)\\s\\d{3}-\\d{4})|(\\d{3}-\\d{3}-\\d{4})";

            bool nameChk = Regex.IsMatch(name, namePattern);
            bool addrChk = Regex.IsMatch(address, addrPattern);
            bool postChk = Regex.IsMatch(postal, postPattern);
            bool cityChk = Regex.IsMatch(city, cityPattern);
            bool provChk = Regex.IsMatch(province, provPattern);
            bool emailChk = Regex.IsMatch(email, emailPattern);
            bool phoneChk = Regex.IsMatch(phone, phonePattern);

            int count = 0;

            if (nameChk && name != Session.FullName && Sttng.setName(usrID, name) == 1)
            {
                msgStr += "Name updated\n";
                count++;
            }
            if (addrChk && address != Session.Address && Sttng.setAddress(usrID, address) == 1)
            {
                msgStr += "Address updated\n";
                count++;
            }
            if (postChk && postal != Session.Postal && Sttng.setPostal(usrID, postal) == 1)
            {
                msgStr += "Postal Code updated\n";
                count++;
            }
            if (cityChk && city != Session.City && Sttng.setCity(usrID, city) == 1)
            {
                msgStr += "City updated\n";
                count++;
            }
            if (provChk && province != Session.Province && Sttng.setProvince(usrID, province) == 1)
            {
                msgStr += "Province updated\n";
                count++;
            }
            if (emailChk && email != Session.Email && Sttng.setEmail(usrID, email) == 1)
            {
                msgStr += "Email updated\n";
                count++;
            }
            if (phoneChk && phone != Session.Phone && Sttng.setPhone(usrID, phone) == 1)
            {
                msgStr += "Contact Number updated\n";
                count++;
            }

            return count;
        }

        public int savePass(string oldPw, string newPw, string verPw, int usrID)
        {
            if (oldPw.Equals(Session.Password) && newPw.Equals(verPw)
                && oldPw != "" && newPw != "" && verPw != ""
               )
            {
                if (Sttng.setPassword(usrID, newPw) == 1)
                {
                    passUpd = "Your password has been changed\n";
                    return 1;
                }
                else
                {
                    passUpd = "Error updating password. Try again or contact System Admin\n";
                    return -1;
                }
            }
            else
            {
                if (oldPw == "" || newPw == "" || verPw == "")
                {
                    passUpd = "Please fill in all Password fields\n";
                    return -2;
                }
                else if (!oldPw.Equals(Session.Password))
                {
                    passUpd = "Old password does not match records\n";
                    return -3;
                }
                else if (!newPw.Equals(verPw))
                {
                    passUpd = "Please verify your new password again\n";
                    return -4;
                }
                else
                {
                    passUpd = "Error updating password. Try again or contact System Admin\n";
                    return -1;
                }
            }
        }

        public bool saveAppearance(string color, int usrID)
        {
            if (Sttng.hasAppSettings(usrID) == usrID)
            {
                return Sttng.setUserBG(usrID, color) == 1;
            }
            else
            {
                try
                {
                    return Sttng.newUserAppSetting(usrID, color) == 1;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
        }

        /*** OLD CODE ***
               public bool savePhone(string phone, int usrID)
               {
                   string emailPattern = "[\\w]+@[\\w]+\\.[\\w]{2,}";
                   string phonePattern = "(\\(\\d{3}\\)\\s\\d{3}-\\d{4})|(\\d{3}-\\d{3}-\\d{4})";

                   bool emChk = Regex.IsMatch(email, emailPattern);
                   bool phChk = Regex.IsMatch(phone, phonePattern);

                   if (phChk)
                   {
                       if (usrSttng.setPhone(usrID, phone) == 1)
                       {
                           msgStr += "Phone saved\n";
                           return true;
                       }
                       else
                       {
                           msgStr += "Error updating Phone. Try again or contact System Admin\n";
                           return false;
                       }
                   }
                   return false;
               }

               public bool saveEmail(string email, int usrID)
               {
                   string emailPattern = "[\\w]+@[\\w]+\\.[\\w]{2,}";
                   bool emChk = Regex.IsMatch(email, emailPattern);

                   if (emChk)
                   {
                       if (usrSttng.setEmail(usrID, email) == 1)
                       {
                           msgStr += "Email Saved";
                           return true;
                       }
                       else
                       {
                           msgStr += "Error updating Email. Try again or contact System Admin\n";
                           return false;
                       }
                   }
                   return false;
               }*/
    }
}
