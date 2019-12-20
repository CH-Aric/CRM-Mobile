using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MainCRMV2.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Check_Page : ContentPage
    {
        private List<ViewCell> views;
        public Check_Page()
        {
            InitializeComponent();
            views = new List<ViewCell>();
            PerformSearch();
        }

        public void PerformSearch()
        {
            string statement = "SELECT eventtype, eventtime, cid_num, cid_dnid, cid_ani FROM asteriskcdrdb.cel WHERE (eventtype='ANSWER' OR eventtype='CHAN_START') AND (context= 'from-trunk' OR context='macro-dial') and cid_name!='' group by UNIQUEID ORDER BY eventtime DESC limit 10;";
            TaskCallback call = new TaskCallback(populateResults);
            DatabaseFunctions.SendToPhp(true, statement, call);
        }
        public void populateResults(string result)
        {
            this.views = new List<ViewCell>();
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result),true);
            if (dictionary.Count > 1)
            {
                for (int i = 0; i < dictionary["eventtime"].Count; i++)
                {
                    string c;
                    bool CC = false;
                    if (dictionary["eventtype"][i] == "ANSWER")
                    {
                        c = "Yes";
                        CC = true;
                    }
                    else
                    {
                        c = "No";
                    }
                    string[] s = new string[3] { FormatFunctions.PrettyPhone(dictionary["cid_num"][i]), dictionary["eventtime"][i] ,c};
                    GridFiller.rapidFillColorized(s, TSection,CC);
                }
            }
        }
    }
}
