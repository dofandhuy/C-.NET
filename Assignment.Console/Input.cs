using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleApp 
{
    public class Input
    {
        public Boolean isEmailFormatValid(String email)
        {
            if (string.IsNullOrEmpty(email) || !Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                return false;
            }
            return true;
        }

        public Boolean isDateRangeValid(DateOnly start, DateOnly end)
        {
            if (start >= end)
            {
                return false;
            }
            return true;
        }

        public Boolean isDateAfterToday(DateOnly date)
        {
            if (date.ToDateTime(new TimeOnly(0, 0)) > DateTime.Today)
            {
                return true;
            }
            return false;
        }

        public Boolean isValidPhoneNumberFormat(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber) ||
                phoneNumber.Length != 10 ||
                !phoneNumber.All(char.IsDigit) ||
                !phoneNumber.StartsWith("0"))
            {
                return false;
            }
            return true;
        }

        public bool isDate18YearsAgo(DateOnly date)
        {
            if (date.ToDateTime(new TimeOnly(0, 0)).AddYears(18) > DateTime.Today)
            {
                return false;
            }
            return true;
        }

        public string generateRandomString(int index)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, index)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
