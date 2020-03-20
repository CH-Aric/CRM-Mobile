using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using PostToPhp;
using Xamarin.Essentials;

namespace MainCRMV2
{
    public static class DatabaseFunctions
    {
        public static HttpClient clientH = new HttpClient();
        public static async void SendToPhpB(bool PBX, string statement,TaskCallback call)
        {
            try
            {
                data d = new data();
                d.df_text1 = statement;
                string requestUriString;
                if (inOffice())
                {
                    if (PBX)
                    {
                        requestUriString = "http://192.168.0.69/accessPBX.php";
                    }
                    else
                    {
                        requestUriString = "http://192.168.0.69/access.php";
                    }
                }
                else
                {
                    if (PBX)
                    {
                        requestUriString = "http://174.114.166.80/accessPBX.php";
                    }
                    else
                    {
                        requestUriString = "http://174.114.166.80/access.php";
                    }
                }
                string text = JsonClass.JSONSerialize<DatabaseFunctions.data>(d);
                StringContent content=new StringContent(text,Encoding.UTF8);
                var result = await clientH.PostAsync(requestUriString, content).ConfigureAwait(false);
                var t = await result.Content.ReadAsStringAsync();
                call(t);
            }
            catch (HttpRequestException e)
            {

            }
        }
        public static bool inOffice()
        {
            IPAddress[] addresses = Dns.GetHostAddresses(Dns.GetHostName());
            string k = new WebClient().DownloadString("http://icanhazip.com/");
            k = Regex.Replace(k, "[^0-9]", "");
            if (k == @"17411416680")
            {
                return true;
            }
            foreach (IPAddress i in addresses)
            {
                string j = i.ToString();
                if (j == @"174.114.166.80")
                {
                    return true;
                }
            }
            return false;
        }
        public static void SendToPhp(bool PBX, string statement, TaskCallback call)
        {
            try
            {
                data d = new data();
                d.df_text1 = statement;
                string requestUriString;
                if (inOffice())
                {
                    if (PBX)
                    {
                        requestUriString = "http://192.168.0.69/accessPBX.php";
                    }
                    else
                    {
                        requestUriString = "http://192.168.0.69/access.php";
                    }
                }
                else
                {
                    if (PBX)
                    {
                        requestUriString = "http://174.114.166.80/accessPBX.php";
                    }
                    else
                    {
                        requestUriString = "http://174.114.166.80/access.php";
                    }
                }
                string text = JsonClass.JSONSerialize<DatabaseFunctions.data>(d);
                byte[] bytes = Encoding.UTF8.GetBytes(text);
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUriString);
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                httpWebRequest.ContentLength = (long)bytes.Length;
                httpWebRequest.Timeout = 50000;
                httpWebRequest.ReadWriteTimeout = 50000;
                httpWebRequest.GetRequestStream().Write(bytes, 0, bytes.Length);
                httpWebRequest.KeepAlive = false;
                using (Stream responseStream = ((HttpWebResponse)httpWebRequest.GetResponse()).GetResponseStream())
                {
                    StreamReader streamReader = new StreamReader(responseStream);
                    String textOut = streamReader.ReadToEnd();
                    bool status = httpWebRequest.HaveResponse;
                    call(textOut);
                    httpWebRequest.Abort();
                    streamReader.Dispose();
                    responseStream.Dispose();
                }
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
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("http://174.114.166.80/access.php");
                httpWebRequest.Method = "POST";
                string s = text;
                byte[] bytes = Encoding.UTF8.GetBytes(s);
                httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                httpWebRequest.ContentLength = (long)bytes.Length;
                httpWebRequest.GetRequestStream().Write(bytes, 0, bytes.Length);
                httpWebRequest.Abort();
            }
            catch (WebException ex)
            {
                string str = ex.ToString();
                Console.WriteLine("--->" + str);
            }
        }
        public static void SendToDebug(string statement)
        {
            try
            {
                string text = JsonClass.JSONSerialize<DatabaseFunctions.data>(new DatabaseFunctions.data
                {
                    df_text1 = statement
                });
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("http://174.114.166.80/debug.php");
                httpWebRequest.Method = "POST";
                string s = text;
                byte[] bytes = Encoding.UTF8.GetBytes(s);
                httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                httpWebRequest.ContentLength = (long)bytes.Length;
                httpWebRequest.GetRequestStream().Write(bytes, 0, bytes.Length);
                httpWebRequest.Abort();
            }
            catch (WebException ex)
            {
                string str = ex.ToString();
                Console.WriteLine("--->" + str);
            }
        }
        public static void SendBatchToPHP(List<string> statementList)
        {
            try
            {
                string[] statements = statementList.ToArray();
                string text = JsonClass.JSONSerialize<DatabaseFunctions.dataArray>(new DatabaseFunctions.dataArray
                {
                    BatchLength = statements.Length,
                    statements = statements
                });
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("http://174.114.166.80/accessBatched.php");
                httpWebRequest.Method = "POST";
                string s = text;
                byte[] bytes = Encoding.UTF8.GetBytes(s);
                httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                httpWebRequest.ContentLength = (long)bytes.Length;
                httpWebRequest.GetRequestStream().Write(bytes, 0, bytes.Length);
                httpWebRequest.Abort();
            }
            catch (WebException ex)
            {
                string str = ex.ToString();
                Console.WriteLine("--->" + str);
            }
        }
        public static void SendBatchToPHP(bool PBX, List<string> statement, List<TaskCallback> call)
        {
            for (int j = 0; j < statement.Count; j++)
            {
                try
                {
                    data d = new data();
                    d.df_text1 = statement[j];
                    string requestUriString;
                    if (PBX)
                    {
                        requestUriString = "http://174.114.166.80/accessPBX.php";
                    }
                    else
                    {
                        requestUriString = "http://174.114.166.80/access.php";
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
                    call[j](streamReader.ReadToEnd());
                    httpWebRequest.Abort();
                    streamReader.Close();
                    responseStream.Close();
                }
                catch (WebException ex)
                {
                    string str = ex.ToString();
                    Console.WriteLine("--->" + str);
                }
            }
        }
        public static string[] getCustomerFileList(string name)
        {
            string s = JsonClass.JSONSerialize<DatabaseFunctions.data>(new DatabaseFunctions.data
            {
                df_text1 = name
            });
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("http://174.114.166.80/getCusFolders.php");
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
            httpWebRequest.Abort();
            streamReader.Close();
            responseStream.Close();
            response.Close();
            requestStream.Close();
            return result;
        }
        public static string getCallFile(string Date, string filename, TaskCallback call)
        {
            string[] array = FormatFunctions.CleanDate(Date);
            string df_text = string.Concat(new string[]
            {
                @"CDR\",
                array[0],
                @"\",
                array[1],
                @"\",
                array[2],
                @"\",
                filename
            });
            string s = JsonClass.JSONSerialize<DatabaseFunctions.data>(new DatabaseFunctions.data
            {
                df_text1 = df_text
            });
            byte[] bytes = Encoding.UTF8.GetBytes(s);
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("http://174.114.166.80/getCusFile.php");
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
            httpWebRequest.Abort();
            return "CHStreamFile" + filename;
        }
        public static async void getFile(string filename)
        {
            string df_text = @"\Root\Files\Customer Files\CoolHeat Comfort Customer List\Residential Customers\" + filename;
            string s = JsonClass.JSONSerialize<DatabaseFunctions.data>(new DatabaseFunctions.data
            {
                df_text1 = df_text
            });
            byte[] bytes = Encoding.UTF8.GetBytes(s);
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("http://174.114.166.80/getCusFile.php");
            httpWebRequest.Method = "POST";
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";
            httpWebRequest.ContentLength = (long)bytes.Length;
            httpWebRequest.GetRequestStream().Write(bytes, 0, bytes.Length);
            Stream responseStream = ((HttpWebResponse)httpWebRequest.GetResponse()).GetResponseStream();
            int num = 1024;
            byte[] buffer = new byte[num];
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            FileStream fileStream = File.Create(path + "CHStreamFile" + filename);
            int count;
            while ((count = responseStream.Read(buffer, 0, num)) != 0)
            {
                fileStream.Write(buffer, 0, count);
            }
            await Browser.OpenAsync(filename, BrowserLaunchMode.SystemPreferred);
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
        public static DataDoubleSwitch findDDataSwitchinList(List<DataDoubleSwitch> switches, int intToFind, int intToFind2)
        {
            foreach (DataDoubleSwitch dataSwitch in switches)
            {
                if (dataSwitch.GetInt() == intToFind && dataSwitch.getSecondInt() == intToFind2)
                {
                    return dataSwitch;
                }
            }
            return null;
        }
        public static List<string> findUnique(List<string> input)
        {
            List<string> results = new List<string>();
            foreach (string s in input)
            {
                if (!results.Contains(s))
                {
                    results.Add(s);
                }
            }
            return results;
        }
        public static int findIndexInList(List<string> list, string index)
        {
            int count = 0;
            foreach (string x in list)
            {
                if (x == index)
                {
                    return count;
                }
                count++;
            }
            return 0;
        }
        public static ClientData client = new ClientData();
        public class data
        {
            public string df_text1 { get; set; }
        }
        public class dataArray
        {
            public int BatchLength { get; set; }
            public string[] statements { get; set; }
        }
    }
}
