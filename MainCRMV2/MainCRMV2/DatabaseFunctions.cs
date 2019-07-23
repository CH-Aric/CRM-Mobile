using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using PostToPhp;

namespace MainCRMV2
{
    public static class DatabaseFunctions
    {
        public static void SendToPhp(bool PBX, string statement, TaskCallback call)
        {
            try
            {
                data d = new data();
                d.df_text1 = statement;
                string requestUriString;
                if (PBX)
                {
                    requestUriString = "http://coolheatcrm.duckdns.org/CRM-2/accessPBX.php";
                }
                else
                {
                    requestUriString = "http://coolheatcrm.duckdns.org/CRM-2/access.php";
                }
                string text = JsonClass.JSONSerialize<DatabaseFunctions.data>(d);
                byte[] bytes = Encoding.UTF8.GetBytes(text);
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUriString);
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                httpWebRequest.ContentLength = (long)bytes.Length;
                httpWebRequest.GetRequestStream().Write(bytes, 0, bytes.Length);
                Stream responseStream = ((HttpWebResponse)httpWebRequest.GetResponse()).GetResponseStream();
                StreamReader streamReader = new StreamReader(responseStream);
                call(streamReader.ReadToEnd());
                streamReader.Close();
                responseStream.Close();
            }
            catch (WebException ex)
            {
                string str = ex.ToString();
                Console.WriteLine("--->" + str);
            }
        }
        public static void SendToPhp(string statement)
        {
            try
            {
                string text = JsonClass.JSONSerialize<DatabaseFunctions.data>(new DatabaseFunctions.data
                {
                    df_text1 = statement
                });
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("http://coolheatcrm.duckdns.org/CRM-2/access.php");
                httpWebRequest.Method = "POST";
                string s = text;
                byte[] bytes = Encoding.UTF8.GetBytes(s);
                httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                httpWebRequest.ContentLength = (long)bytes.Length;
                httpWebRequest.GetRequestStream().Write(bytes, 0, bytes.Length);
            }
            catch (WebException ex)
            {
                string str = ex.ToString();
                Console.WriteLine("--->" + str);
            }
        }
        public static string[] getCustomerFileList(string name)
        {
            string s = JsonClass.JSONSerialize<DatabaseFunctions.data>(new DatabaseFunctions.data
            {
                df_text1 = name
            });
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("http://coolheatcrm.duckdns.org/CRM-2/getCusFolders.php");
            httpWebRequest.Method = "POST";
            byte[] bytes = Encoding.UTF8.GetBytes(s);
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";
            httpWebRequest.ContentLength = (long)bytes.Length;
            Stream requestStream = httpWebRequest.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);
            WebResponse response = httpWebRequest.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader streamReader = new StreamReader(responseStream);
            string[] result = FormatFunctions.SplitToPairs(streamReader.ReadToEnd());
            streamReader.Close();
            responseStream.Close();
            response.Close();
            requestStream.Close();
            return result;
        }
        public static string getFile(string Date, string filename, TaskCallback call)
        {
            string[] array = FormatFunctions.CleanDate(Date);
            string df_text = string.Concat(new string[]
            {
                array[0],
                "/",
                array[1],
                "/",
                array[2],
                "/",
                filename
            });
            string s = JsonClass.JSONSerialize<DatabaseFunctions.data>(new DatabaseFunctions.data
            {
                df_text1 = df_text
            });
            byte[] bytes = Encoding.UTF8.GetBytes(s);
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("http://coolheatcrm.duckdns.org/crm-2/getCusFile.php");
            httpWebRequest.Method = "POST";
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";
            httpWebRequest.ContentLength = (long)bytes.Length;
            httpWebRequest.GetRequestStream().Write(bytes, 0, bytes.Length);
            Stream responseStream = ((HttpWebResponse)httpWebRequest.GetResponse()).GetResponseStream();
            int num = 1024;
            byte[] buffer = new byte[num];
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            FileStream fileStream = File.Create(path+"CHStreamFile" + filename);
            int count;
            while ((count = responseStream.Read(buffer, 0, num)) != 0)
            {
                fileStream.Write(buffer, 0, count);
            }
            return "CHStreamFile" + filename;
        }
        public static string lookupInDictionary(string Index, string ToFinditIn, string ToReturn, Dictionary<string, List<string>> DictionaryToUse)
        {
            for (int i = 0; i < DictionaryToUse[ToFinditIn].Count; i++)
            {
                if (DictionaryToUse[ToFinditIn][i] == Index)
                {
                    return DictionaryToUse[ToReturn][i];
                }
            }
            return "";
        }
        public static DataSwitch findDataSwitchinList(List<DataSwitch> switches, int IntToFind)
        {
            foreach (DataSwitch dataSwitch in switches)
            {
                if (dataSwitch.GetInt() == IntToFind)
                {
                    return dataSwitch;
                }
            }
            return null;
        }
        public static ClientData client = new ClientData();
        public class data
        {
            public string df_text1 { get; set; }
        }
        public class dataArray
        {
            public string[] df_text1 { get; set; }
        }
    }
}
