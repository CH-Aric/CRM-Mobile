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
        public static int GridMargin = 1;
        public static Color[] rotatingColors=new Color[2] { Color.White,Color.LightGray};
        public static Color[] rotatingConfirmationColors = new Color[2] { Color.LimeGreen, Color.LightGreen };
        public static Color[] rotatingNegativeColors = new Color[2] { Color.LightPink, Color.MistyRose };
        public static Color textColor = Color.Black;
        public static int LastColor = 0;
        private string Username = "x";
        private string Password = "x";
        private bool Responded;
        private Dictionary<string, List<string>> dict;
        private static List<string> SecurityKeys;
        private static List<string> Roles;
        public static Color getGridColor()
        {
            LastColor++;
            if (LastColor > rotatingColors.Length-1)
            {
                LastColor = 0;
            }
            return rotatingColors[LastColor];
        }
        public static Color getGridColorCC(bool CC)
        {
            LastColor++;
            if (LastColor > rotatingColors.Length - 1)
            {
                LastColor = 0;
            }
            if (CC)
            {
                return rotatingConfirmationColors[LastColor];
            }
            return rotatingNegativeColors[LastColor];
        }
        public void loadUserDatafromFile()
        {
            if (Application.Current.Properties.ContainsKey("UN"))
            {
               Username = (Application.Current.Properties["UN"] as string);
               Password = (Application.Current.Properties["PW"] as string);
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
            TaskCallback call = new TaskCallback(response);
            string statement = string.Concat(new string[]
            {
                "SELECT IDKey,AgentNum FROM agents WHERE Username='",
                u,
                "'AND Password='",
                p,
                "';"
            });
            DatabaseFunctions.SendToPhp(false, statement, call);
            while (!Responded)
            {
                await Task.Delay(50);
            }
            bool result;
            if (dict.Count > 1)
            {
                if (s)
                {
                    writeUserDataToFile(u, p);
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
            if (dict.Count > 1)
            {
                ClientData.AgentIDK = int.Parse(dict["IDKey"][0]);
                string sql = "SELECT PermissionGranted FROM agentpermissions WHERE AgentID='" + AgentIDK+"'";
                TaskCallback call = loadSecurityKeys;
                DatabaseFunctions.SendToPhp(false,sql,call);
                string sql2 = "SELECT AgentRole FROM agentroles WHERE AgentID='" + AgentIDK + "'";
                TaskCallback call2 = loadRoles;
                DatabaseFunctions.SendToPhp(false, sql2, call2);
            }
        }
        public void loadSecurityKeys(string result)
        {
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            SecurityKeys = new List<string>();
            if (dictionary.Count > 0)
            {
                SecurityKeys = dictionary["PermissionGranted"];
            }
        }
        public static bool hasSecurityKey(string Key)
        {
            if (SecurityKeys.Contains("Admin"))
            {
                return true;
            }
            return SecurityKeys.Contains(Key);
        }
        public void loadRoles(string result)
        {
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            Roles = new List<string>();
            if (dictionary.Count > 0)
            {
                Roles = dictionary["AgentRole"];
            }
        }
        public static bool hasRole(string Key)//Role 0=Salesman, 1=Installer
        {
            return Roles.Contains(Key);
        }
    }
}