using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ScrumStore
{
    static class Ultity
    {

        static public bool checkForNonDigit(string str)
        {
            Regex regex = new Regex(@"^[0-9]+$");
            return !regex.IsMatch(str);

        }
    }
}
