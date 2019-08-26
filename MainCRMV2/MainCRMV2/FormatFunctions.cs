using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MainCRMV2
{
    public static class FormatFunctions
    {
        private static Regex digitsOnly = new Regex("[^\\d]");
        static bool Responded = false;
        static Dictionary<string, List<string>> dict;
        public static string CleanPhone(string phone)
        {
            return FormatFunctions.digitsOnly.Replace(phone, "");
        }
        public static string PrettyPhone(string phone)
        {
            if (phone.ToCharArray().Length < 10)
            {
                return phone;
            }
            return string.Format("({0}) {1}-{2}", phone.Substring(0, 3), phone.Substring(3, 3), phone.Substring(6));
        }
        public static string[] CleanDate(string datein)
        {
            return datein.Split(' ')[0].Split('-');
        }
        public static string CleanDateNew(string Date)
        {
            string s1 = Date.Replace(",", "@");
            string s2 = s1.Replace(":", "!");
            return s2;
        }
        public static string PrettyDate(string Date)
        {
            string s1 = Date.Replace("@", ",");
            string s2 = s1.Replace("!", ":");
            return s2;
        }
        public static string stripper(string intake)
        {
            string intake2 = intake;
            intake2 = intake2.Replace("\"", "");
            intake2 = intake2.Replace("\\", "");
            intake2 = intake2.Replace("[", "");
            intake2 = intake2.Replace("]", "");
            intake2 = intake2.Replace("{", "");
            intake2 = intake2.Replace("}", "");
            intake2 = intake2.Replace("[", "");
            intake2 = intake2.Replace("NewRow:", "");
            return intake2;
        }
        public static string[] SplitToPairs(string input)
        {
            input = stripper(input);
            return input.Split(',');
        }
        public static Dictionary<string, List<string>> createValuePairs(string[] input)
        {
            Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();
            for (int i = 0; i < input.Length; i++)
            {
                string[] array = input[i].Split(':');
                if (array.Length < 2)
                {
                    return dictionary;
                }
                if (dictionary.ContainsKey(array[0]))
                {
                    dictionary[array[0]].Add(array[1]);
                }
                else
                {
                    dictionary.Add(array[0], new List<string>());
                    dictionary[array[0]].Add(array[1]);
                }
            }
            return dictionary;
        }
        public static Dictionary<string, List<string>> createValuePairs(string[] input, bool TimeOverload)
        {
            Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();
            for (int i = 0; i < input.Length; i++)
            {
                string[] array = input[i].Split(':');
                if (array.Length < 2)
                {
                    return dictionary;
                }
                if (dictionary.ContainsKey(array[0]))
                {
                    if (array.Length == 4)
                    {
                        dictionary[array[0]].Add(array[1] + ":" + array[2] + ":" + array[3]);
                    }
                    else
                    {
                        dictionary[array[0]].Add(array[1]);
                    }
                }
                else
                {
                    dictionary.Add(array[0], new List<string>());
                    if (array.Length == 4)
                    {
                        dictionary[array[0]].Add(array[1] + ":" + array[2] + ":" + array[3]);
                    }
                    else
                    {
                        dictionary[array[0]].Add(array[1]);
                    }
                }
            }
            return dictionary;
        }
        public async static Task<string> smartsearch(string term, string table)
        {
            Responded = false;
            TaskCallback call = boolSetter;
            string sql = "SELECT smartStatement FROM smartsearch WHERE table='" + table + "' AND term='" + term + "';";
            DatabaseFunctions.SendToPhp(false, sql, call);
            while (!Responded)
            {
                await Task.Delay(50);
            }
            string toReturn = "";
            if (dict.Count > 0)
            {
                foreach (string s in dict["smartStatement"])
                {
                    toReturn += " OR " + s;
                }
            }
            return toReturn;
        }
        public static void boolSetter(string result)
        {
            string[] input = FormatFunctions.SplitToPairs(result);
            dict = FormatFunctions.createValuePairs(input);
            Responded = true;
        }
        public static List<View> scrubOutUnwanted(List<View> listToScrub, View example)
        {
            List<View> returnList = new List<View>();
            foreach (View x in listToScrub)
            {
                if (!x.GetType().Equals(example.GetType()))
                {
                    returnList.Add(x);
                }
            }
            return returnList;
        }
        public static List<View> scrubOutUnlessWanted(List<View> listToScrub, View example, List<Label> scrubList)
        {
            List<View> returnList = new List<View>();
            foreach (View x in listToScrub)
            {
                if (x.GetType().Equals(example.GetType()))
                {
                    returnList.Add(x);
                }
                else
                {
                    scrubList.Add((Label)x);
                }
            }
            return returnList;
        }
    }
}