using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MainCRMV2
{
    public class ClientData
    {
        public static int StoredKey;
        public static int AgentID;
        public static int AgentIDK;
        private string Username = "x";
        private string Password = "x";
        private bool Responded;
        private Dictionary<string, List<string>> dict;
        public void loadUserDatafromFile()
        {
            if (Application.Current.Properties.ContainsKey("UN"))
            {
                this.Username = (Application.Current.Properties["UN"] as string);
                this.Password = (Application.Current.Properties["PW"] as string);
            }
        }
        public void writeUserDataToFile(string u, string p)
        {
            Application.Current.Properties["UN"] = u;
            Application.Current.Properties["PW"] = p;
        }
        public void wipeUserDataFromFile()
        {
            this.writeUserDataToFile("", "");
        }
        public async Task<bool> attemptNewLogin(string u, string p, bool s)
        {
            this.Responded = false;
            TaskCallback call = new TaskCallback(this.response);
            string statement = string.Concat(new string[]
            {
                "SELECT IDKey,AgentNum FROM agents WHERE Username='",
                u,
                "'AND Password='",
                p,
                "';"
            });
            DatabaseFunctions.SendToPhp(false, statement, call);
            while (!this.Responded)
            {
                await Task.Delay(50);
            }
            bool result;
            if (this.dict.Count > 1)
            {
                if (s)
                {
                    this.writeUserDataToFile(u, p);
                }
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }
        public async Task<bool> attemptSavedLoginAsync()
        {
            Responded = false;
            loadUserDatafromFile();
            TaskCallback call = response;
            string statement = "SELECT IDKey,AgentNum FROM agents WHERE Username='" + Username + "'AND Password='" + Password + "';";
            DatabaseFunctions.SendToPhp(false, statement, call);
            while (!Responded)
            {
                await Task.Delay(50);
            }
            bool result;
            if (dict.Count > 1)
            {
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }
        public void response(string result)
        {
            string[] input = FormatFunctions.SplitToPairs(result);
            dict = FormatFunctions.createValuePairs(input);
            Responded = true;
            if (this.dict.Count > 1)
            {
                ClientData.AgentIDK = int.Parse(this.dict["IDKey"][0]);
            }
        }
    }
}
