using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MainCRMV2.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PunchAdmin : ContentPage
    {
        public int Mode = 0;//0 for Hours, 1 for Live, 2 for Approve, 3 for Tardy
        List<string> agents;
        public PunchAdmin()
        {
            InitializeComponent();
            string sql = "SELECT agents.FName,agents.IDKey FROM agents WHERE Active='1'";
            TaskCallback call = populateSalesCombo;
            DatabaseFunctions.SendToPhp(false, sql, call);
            Hours.BackgroundColor = Color.Red;
        }
        public void onClickedChangeRender(object sender, EventArgs e)
        {
            DeactivateAll();
            ActivateChosen(sender);
        }
        public void onClickView(object sender, EventArgs e)
        {
            switch(Mode){
                case 0:
                    HoursSearch();
                    break;
                case 1:
                    LiveSearch();
                    break;
                case 2:
                    ApproveSearch();
                    break;
                case 3:
                    TardySearch();
                    break;
            }
        }
        public void DeactivateAll()
        {
            HourStack.IsVisible = false;
            LiveStack.IsVisible = false;
            ApproveStack.IsVisible = false;
            TardStack.IsVisible = false;
            Hours.BackgroundColor = Color.AliceBlue;
            Live.BackgroundColor = Color.AliceBlue;
            Approve.BackgroundColor = Color.AliceBlue;
            Tardy.BackgroundColor = Color.AliceBlue;
        }
        public void ActivateChosen(object sender)
        {
            StyledButton s = (StyledButton)sender;
            s.BackgroundColor = Color.Red;
            if (s.Equals(Hours))
            {
                HourStack.IsVisible = true;
                Mode = 0;
            }
            else if (s.Equals(Live))
            {
                LiveStack.IsVisible = true;
                Mode = 1;
            }
            else if (s.Equals(Approve))
            {
                ApproveStack.IsVisible = true;
                Mode = 2;
            }
            else if (s.Equals(Tardy))
            {
                TardStack.IsVisible = true;
                Mode = 3;
            }
        }
        public void HoursSearch()
        {
            DateTime d = DayPicker.Date;
            string sql = "SELECT * FROM punchclock WHERE AgentID='" + agents[Agent.SelectedIndex] + "' AND (" + FormatFunctions.getRelevantDates(d, "TimeStamp") + ")";
            if ((bool)AppCheck.IsChecked)
            {
                sql += " AND Approved='1'";
            }
            if ((bool)!LocCheck.IsChecked)
            {
                sql += " AND (State='True' OR State='False')";
            }
            sql += " ORDER BY IDKey ASC";
            TaskCallback call = populateStamps;
            DatabaseFunctions.SendToPhp(false, sql, call);
        }
        public void LiveSearch()
        {
            foreach (string s in agents)
            {
                string sql3 = "SELECT agents.FName,punchclock.State,punchclock.Coordinates,punchclock.TimeStamp,punchclock.Note FROM agents INNER JOIN punchclock ON agents.IDKey = punchclock.AgentID WHERE punchclock.AgentID='" + s + "' ORDER BY punchclock.IDKey DESC";
                TaskCallback call2 = populateLiveFeed;
                DatabaseFunctions.SendToPhp(false, sql3, call2);
            }
        }
        public void ApproveSearch()
        {
            TaskCallback call = populatePend;
            DateTime d = DayPicker.Date;
            string sql = "SELECT * FROM punchclock WHERE AgentID='" + agents[Agent.SelectedIndex] + "' AND (" + FormatFunctions.getRelevantDates(d, "TimeStamp") + ")";
            sql += " ORDER BY IDKey DESC";
            DatabaseFunctions.SendToPhp(false, sql, call);
        }
        public void TardySearch()
        {
            TaskCallback call = populateTardiGrid;
            string sql = "SELECT * FROM agentrecords WHERE AgentID='" + agents[Agent.SelectedIndex] + "'";
            DatabaseFunctions.SendToPhp(false, sql, call);
        }
        public void populateSalesCombo(string result)
        {
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            Agent.ItemsSource = dictionary["FName"];
            agents = dictionary["IDKey"];
        }
        public void populateStamps(string result)
        {
            GridFiller.PurgeHeader(HourCalcBody);
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            List<string> clockin = new List<string>();
            List<string> clockout = new List<string>();
            if (dictionary.Count > 0)
            {
                for (int i = 0; i < dictionary["IDKey"].Count; i++)
                {
                    string[] y = new string[2];
                    y[0] = FormatFunctions.PrettyDate(dictionary["TimeStamp"][i]);
                    y[1] = convertState(dictionary["State"][i]);
                    if (dictionary["State"][i] == "True")
                    {
                        GridFiller.rapidFillColorized(y, HourCalcBody, true);
                        clockin.Add(FormatFunctions.PrettyDate(dictionary["TimeStamp"][i]));
                    }
                    else if (dictionary["State"][i] == "False")
                    {
                        GridFiller.rapidFillColorized(y, HourCalcBody, false);
                        clockout.Add(FormatFunctions.PrettyDate(dictionary["TimeStamp"][i]));
                    }
                    else
                    {
                        GridFiller.rapidFill(y, HourCalcBody);
                    }
                }
                double x = calculateHours(clockin, clockout) / 60;
                HourDisplay.Text = "Total Hours: " + x;
            }
        }
        public void populateLiveFeed(string result)
        {
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            if (dictionary.Count > 0)
            {
                string[] s = new string[] { dictionary["FName"][0], FormatFunctions.PrettyDate(dictionary["TimeStamp"][0]), dictionary["Coordinates"][0] };

                if (dictionary["State"][0] == "True")
                {
                    GridFiller.rapidFillColorized(s, LiveBody, true);
                }
                else if (dictionary["State"][0] == "False")
                {
                    GridFiller.rapidFillColorized(s, LiveBody, false);
                }
                else
                {
                    GridFiller.rapidFill(s, LiveBody);
                }
            }
        }
        public void populatePend(string result)
        {
            GridFiller.PurgeHeader(ApproveBody);
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            if (dictionary.Count > 0)
            {
                for (int i = 0; i < dictionary["IDKey"].Count; i++)
                {
                    DataButton b = new DataButton();
                    b.Text = "Approve";
                    b.Integer = int.Parse(dictionary["IDKey"][i]);
                    b.Integer2 = int.Parse(dictionary["Approved"][i]);
                    if (dictionary["Approved"][i] == "1")
                    {
                        b.BackgroundColor = ClientData.rotatingConfirmationColors[0];
                        b.Text = "Disapprove";
                    }
                    else
                    {
                        b.BackgroundColor = ClientData.rotatingNegativeColors[0];
                    }
                    b.Clicked += onClickApprove;
                    DataButton b2 = new DataButton();
                    b2.Text = "Add Note";
                    b2.Integer = int.Parse(dictionary["IDKey"][i]);
                    b2.Clicked += onClickANote;
                    Label T = new Label();
                    T.Text = FormatFunctions.PrettyDate(dictionary["TimeStamp"][i]);
                    Label L = new Label();
                    L.Text = FormatFunctions.PrettyDate(dictionary["Coordinates"][i]);
                    Label I = new Label();
                    I.Text = convertState(dictionary["State"][i]);
                    Label N = new Label();
                    N.Text = FormatFunctions.PrettyDate(dictionary["Note"][i]);
                    List<View> list = new List<View>() { b, b2, T, L, I, N };
                    GridFiller.rapidFillPremadeObjects(list, ApproveBody, new bool[] { false, false, true, true, true, true });
                }
            }
        }
        public string convertState(string intake)
        {
            if (intake == "True")
            {
                return "Punched In";
            }
            else if (intake == "False")
            {
                return "Punched Out";
            }
            return "";
        }
        public double calculateHours(List<string> starts, List<string> ends)
        {
            double output = 0;
            List<DateTime> s = new List<DateTime>();
            List<DateTime> e = new List<DateTime>();
            foreach (string i in starts)
            {
                s.Add(Convert.ToDateTime(i));
            }
            foreach (string i in ends)
            {
                e.Add(Convert.ToDateTime(i));
            }
            for (int i = 0; i < Math.Min(e.Count, s.Count); i++)
            {
                System.TimeSpan diff = e[i].Subtract(s[i]);
                output += diff.TotalMinutes;
            }
            return output;
        }
        public void onClickApprove(object sender, EventArgs e)
        {
            DataButton b = (DataButton)sender;
            b.BackgroundColor = ClientData.getGridColorCC(true);
            string sql;
            if (b.GetInt2() == 1)
            {
                b.BackgroundColor = ClientData.rotatingConfirmationColors[0];
                b.Text = "Approve";
                b.Integer2 = 0;
                sql = "UPDATE punchclock SET Approved='0' WHERE IDKey='" + b.GetInt() + "'";
            }
            else
            {
                b.Text = "Disapprove";
                b.BackgroundColor = ClientData.rotatingNegativeColors[0];
                b.Integer2 = 1;
                sql = "UPDATE punchclock SET Approved='1' WHERE IDKey='" + b.GetInt() + "'";
            }
            DatabaseFunctions.SendToPhp(sql);
        }
        public void onClickANote(object sender, EventArgs e)
        {
            DataButton b = (DataButton)sender;
            string sql = "UPDATE punchclock SET AdminNote='" + AdminNote.Text + "' WHERE IDKey='" + b.GetInt() + "'";
            DatabaseFunctions.SendToPhp(sql);
            b.BackgroundColor = Color.FromRgb(213, 213, 213);
        }
        public void populateTardiGrid(string result)
        {
            GridFiller.PurgeHeader(TardiGrid);
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            int sick = 0;
            int tardy = 0;
            if (dictionary["IDKey"].Count > 0)
            {
                for (int i = 0; i < dictionary["IDKey"].Count; i++)
                {
                    GridFiller.rapidFill(new string[] { dictionary["RecordType"][i], dictionary["Date"][i], dictionary["Note"][i] }, TardiGrid);
                    if (dictionary["RecordType"][i] == "Sick")
                    {
                        sick++;
                    }
                    else if (dictionary["RecordType"][i] == "Tardy")
                    {
                        tardy++;
                    }
                }
            }
        }
    }
}