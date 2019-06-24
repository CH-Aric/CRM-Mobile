using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            PurgeCells();
            this.views = new List<ViewCell>();
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result),true);
            if (dictionary.Count > 1)
            {
                for (int i = 0; i < dictionary["eventtime"].Count; i++)
                {

                    Label a = new Label()
                    {
                        Text = dictionary["cid_num"][i],
                        HorizontalOptions = LayoutOptions.FillAndExpand
                    };
                    Label b = new Label()
                    {
                        Text = dictionary["eventtime"][i],
                        HorizontalOptions = LayoutOptions.FillAndExpand
                    };
                    Label c = new Label()
                    {
                        HorizontalOptions = LayoutOptions.FillAndExpand
                    };
                    if (dictionary["eventtype"][i] == "ANSWER")
                    {
                        c.Text = "Yes";
                    }
                    else
                    {
                        c.Text = "No";
                    }
                    ViewCell viewCell = new ViewCell();
                    StackLayout stackLayout = new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal
                    };
                    stackLayout.Children.Add(a);
                    stackLayout.Children.Add(b);
                    stackLayout.Children.Add(c);
                    viewCell.View = stackLayout;
                    this.views.Add(viewCell);
                    this.TSection.Add(viewCell);
                }
            }
        }
        public void PurgeCells()
        {
            if (this.views.Count > 0)
            {
                foreach (ViewCell item in this.views)
                {
                    this.TSection.Remove(item);
                }
            }
        }
    }
}
