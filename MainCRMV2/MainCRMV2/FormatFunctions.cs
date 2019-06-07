using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MainCRMV2
{
    // Token: 0x0200000B RID: 11
    public static class FormatFunctions
    {
        // Token: 0x06000027 RID: 39 RVA: 0x00002A3E File Offset: 0x00000C3E
        public static string CleanPhone(string phone)
        {
            return FormatFunctions.digitsOnly.Replace(phone, "");
        }

        // Token: 0x06000028 RID: 40 RVA: 0x00002A50 File Offset: 0x00000C50
        public static string[] CleanDate(string datein)
        {
            return datein.Split(new char[]
            {
                ' '
            })[0].Split(new char[]
            {
                '-'
            });
        }

        // Token: 0x06000029 RID: 41 RVA: 0x00002A78 File Offset: 0x00000C78
        public static string[] SplitToPairs(string input)
        {
            input = input.Replace("\"", "");
            input = input.Replace("[", "");
            input = input.Replace("]", "");
            input = input.Replace("{", "");
            input = input.Replace("}", "");
            input = input.Replace("[", "");
            input = input.Replace("NewRow:", "");
            return input.Split(new char[]
            {
                ','
            });
        }

        // Token: 0x0600002A RID: 42 RVA: 0x00002B14 File Offset: 0x00000D14
        public static Dictionary<string, List<string>> createValuePairs(string[] input)
        {
            Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();
            for (int i = 0; i < input.Length; i++)
            {
                string[] array = input[i].Split(new char[]
                {
                    ':'
                });
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

        // Token: 0x04000017 RID: 23
        private static Regex digitsOnly = new Regex("[^\\d]");
    }
}
