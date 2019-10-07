using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MainCRMV2.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Punch_Page : ContentPage
    {
        public bool PunchedIn = false;
        public bool createPunchOnResult = false;
        public bool statelessPunch = false;
        public Punch_Page()
        {
            InitializeComponent();
            getCurrentPunchState();
            getPunches();
        }
        public void onclick(object sender, EventArgs e)
        {
            createPunchOnResult = true;
            getCurrentPunchState();
        }
        public void onClickStateless(object sender, EventArgs e)
        {
            statelessPunch = true;
            getCurrentPunchState();
        }
        public void getCurrentPunchState()
        {
            string sql = "SELECT State FROM punchclock WHERE AgentID='" + ClientData.AgentIDK + "' AND State!='less' ORDER BY IDKey DESC LIMIT 1";
            TaskCallback call = writeState;
            DatabaseFunctions.SendToPhp(false, sql, call);
        }
        public async void writeState(string result)
        {
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            if (dictionary.Count > 0)
            {
                PunchedIn = bool.Parse(dictionary["State"][0]);
                if (((PunchedIn && !createPunchOnResult) || (!PunchedIn && createPunchOnResult)) && !statelessPunch)
                {

                    ClockState.Text = "Current State: Clocked In";
                }
                else if (!statelessPunch)
                {
                    ClockState.Text = "Current State: Clocked Out";
                }
            }
            if (createPunchOnResult)
            {
                createPunchOnResult = false;
                var request = new GeolocationRequest(GeolocationAccuracy.Medium);
                Location location = await Geolocation.GetLocationAsync(request);
                var placemark = await Geocoding.GetPlacemarksAsync(location.Latitude, location.Longitude);
                var x = placemark?.FirstOrDefault();
                string sql = "INSERT INTO punchclock (AgentID,Timestamp,Coordinates,State,Note) VALUES('" + ClientData.AgentIDK + "','" + FormatFunctions.CleanDateNew(DateTime.Now.ToString("yyyy/M/d HH:mm:ss")) + "','" + x.SubLocality + "< " + x.SubThoroughfare + x.Thoroughfare +"< "+ x.PostalCode + "','" + !PunchedIn + "','" + TextEntry.Text + "')";
                DatabaseFunctions.SendToPhp(sql);
            }
            if (statelessPunch)
            {
                statelessPunch = false;
                var request = new GeolocationRequest(GeolocationAccuracy.Medium);
                var location = await Geolocation.GetLocationAsync(request);
                var placemark = await Geocoding.GetPlacemarksAsync(location.Latitude, location.Longitude);
                var x = placemark?.FirstOrDefault();
                string sql = "INSERT INTO punchclock (AgentID,Timestamp,Coordinates,State,Note) VALUES('" + ClientData.AgentIDK + "','" + FormatFunctions.CleanDateNew(DateTime.Now.ToString("yyyy/M/d HH:mm:ss")) + "','" + x.SubLocality + "< " + x.SubThoroughfare + x.Thoroughfare + "< " + x.PostalCode + "','less','" + TextEntry.Text + "')";
                DatabaseFunctions.SendToPhp(sql);
            }
            getPunches();
        }
        public void getPunches()
        {
            string sql = "SELECT * FROM punchclock WHERE AgentID='" + ClientData.AgentIDK + "' ORDER BY IDKey DESC";
            TaskCallback call = populatePunches;
            DatabaseFunctions.SendToPhp(false, sql, call);
        }
        public void populatePunches(string result)
        {
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            if (dictionary.Count > 0)
            {
                GridFiller.PurgeGrid(logGrid);
                string[] list = new string[3];
                for (int i = 0; i < dictionary["IDKey"].Count; i++)
                {
                    list[0] = dictionary["Note"][i];
                    list[2] = dictionary["State"][i];
                    list[1] = FormatFunctions.PrettyDate(dictionary["TimeStamp"][i]);
                    if (list[2] != "less")
                    {
                        GridFiller.rapidFillColorized(list, logGrid, bool.Parse(list[2]));
                    }
                    else
                    {
                        list[2] = "Location Log";
                        GridFiller.rapidFill(list, logGrid);
                    }
                }
            }
        }
    }
}