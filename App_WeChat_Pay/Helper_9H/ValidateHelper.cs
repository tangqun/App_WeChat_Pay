using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Helper_9H
{
    public static class ValidateHelper
    {
        public static bool Guid(this string guid)
        {
            Regex regex = new Regex("^[A-Za-z0-9]{8}(-[A-Za-z0-9]{4}){3}-[A-Za-z0-9]{12}$");
            return regex.IsMatch(guid);
        }

        public static bool StringLength(this string str, int minLength, int maxLength)
        {
            if (str.Length >= minLength && str.Length <= maxLength)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool Rang(this int i, int minValue, int maxValue)
        {
            if (i >= minValue && i <= maxValue)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
