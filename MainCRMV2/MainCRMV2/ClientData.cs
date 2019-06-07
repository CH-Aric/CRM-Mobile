using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MainCRMV2
{
    // Token: 0x02000004 RID: 4
    public class ClientData
    {
        // Token: 0x0600000B RID: 11 RVA: 0x000021E8 File Offset: 0x000003E8
        public void loadUserDatafromFile()
        {
            if (Application.Current.Properties.ContainsKey("UN"))
            {
                this.Username = (Application.Current.Properties["UN"] as string);
                this.Password = (Application.Current.Properties["PW"] as string);
            }
        }

        // Token: 0x0600000C RID: 12 RVA: 0x00002249 File Offset: 0x00000449
        public void writeUserDataToFile(string u, string p)
        {
            Application.Current.Properties["UN"] = u;
            Application.Current.Properties["PW"] = p;
        }

        // Token: 0x0600000D RID: 13 RVA: 0x00002275 File Offset: 0x00000475
        public void wipeUserDataFromFile()
        {
            this.writeUserDataToFile("", "");
        }

        // Token: 0x0600000E RID: 14 RVA: 0x00002288 File Offset: 0x00000488
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

        // Token: 0x0600000F RID: 15 RVA: 0x000022E8 File Offset: 0x000004E8
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

        // Token: 0x06000010 RID: 16 RVA: 0x00002330 File Offset: 0x00000530
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

        // Token: 0x04000002 RID: 2
        public static int StoredKey;

        // Token: 0x04000003 RID: 3
        public static int AgentID;

        // Token: 0x04000004 RID: 4
        public static int AgentIDK;

        // Token: 0x04000005 RID: 5
        private string Username = "x";

        // Token: 0x04000006 RID: 6
        private string Password = "x";

        // Token: 0x04000007 RID: 7
        private bool Responded;

        // Token: 0x04000008 RID: 8
        private Dictionary<string, List<string>> dict;
    }
}
