using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using PostToPhp;

namespace MainCRMV2
{
    // Token: 0x02000008 RID: 8
    public static class DatabaseFunctions
    {
        // Token: 0x0600001A RID: 26 RVA: 0x00002644 File Offset: 0x00000844
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
                /*
                HttpWebRequest myrequest = (HttpWebRequest)WebRequest.Create(requestUriString);
                myrequest.Method = "POST";
                string s = text;
                byte[] bytes = Encoding.UTF8.GetBytes(s);
                myrequest.ContentType = "application/x-www-form-urlencoded";
                myrequest.ContentLength = bytes.Length;
                WebResponse response = myrequest.GetResponse();
                Stream responseStream = response.GetResponseStream();
               */
            }
            catch (WebException ex)
            {
                string str = ex.ToString();
                Console.WriteLine("--->" + str);
            }
        }

        // Token: 0x0600001B RID: 27 RVA: 0x00002728 File Offset: 0x00000928
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

        // Token: 0x0600001C RID: 28 RVA: 0x000027C0 File Offset: 0x000009C0
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

        // Token: 0x0600001D RID: 29 RVA: 0x00002860 File Offset: 0x00000A60
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
            FileStream fileStream = File.Create("CHStreamFile" + filename);
            int count;
            while ((count = responseStream.Read(buffer, 0, num)) != 0)
            {
                fileStream.Write(buffer, 0, count);
            }
            return "CHStreamFile" + filename;
        }

        // Token: 0x0600001E RID: 30 RVA: 0x0000296C File Offset: 0x00000B6C
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

        // Token: 0x0600001F RID: 31 RVA: 0x000029B8 File Offset: 0x00000BB8
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

        // Token: 0x04000014 RID: 20
        public static ClientData client = new ClientData();

        // Token: 0x02000009 RID: 9
        public class data
        {
            // Token: 0x17000001 RID: 1
            // (get) Token: 0x06000021 RID: 33 RVA: 0x00002A1C File Offset: 0x00000C1C
            // (set) Token: 0x06000022 RID: 34 RVA: 0x00002A24 File Offset: 0x00000C24
            public string df_text1 { get; set; }
        }

        // Token: 0x0200000A RID: 10
        public class dataArray
        {
            // Token: 0x17000002 RID: 2
            // (get) Token: 0x06000024 RID: 36 RVA: 0x00002A2D File Offset: 0x00000C2D
            // (set) Token: 0x06000025 RID: 37 RVA: 0x00002A35 File Offset: 0x00000C35
            public string[] df_text1 { get; set; }
        }
    }
}
