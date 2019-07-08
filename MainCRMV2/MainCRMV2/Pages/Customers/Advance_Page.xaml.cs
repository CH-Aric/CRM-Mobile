using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MainCRMV2.Pages.Customers
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Advance_Page : ContentPage
    {
        int customer;
        List<DataSwitch> checks;
        public Advance_Page(int customerIn)
        {
            customer = customerIn;
            InitializeComponent();
            assessStage();
        }
        public void assessStage()
        {
            string sql = "SELECT Stage FROM cusindex WHERE IDKey='"+customer+"'";
            TaskCallback call = populateAssessment;
            DatabaseFunctions.SendToPhp(false,sql,call);
        }
        public void populateAssessment(string result)
        {
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            checks = new List<DataSwitch>();
            ViewCell vc = new ViewCell();
            TSection.Insert(TSection.IndexOf(footer), vc);
            StackLayout sl = new StackLayout() { Orientation=StackOrientation.Vertical};
            int Stage = int.Parse(dictionary["Stage"][0]);
            if (Stage<2)
            {
                StackLayout s = new StackLayout() { Orientation=StackOrientation.Horizontal};
                Label l = new Label() { HorizontalOptions=LayoutOptions.StartAndExpand, Text="Booking"};
                DataSwitch rb=new DataSwitch(2);
                s.Children.Add(l);
                sl.Children.Add(sl);
                checks.Add(rb);
            }
            if (Stage < 3)
            {
                StackLayout s = new StackLayout() { Orientation = StackOrientation.Horizontal };
                Label l = new Label() { HorizontalOptions = LayoutOptions.StartAndExpand, Text = "Quote" };
                DataSwitch rb = new DataSwitch(3);
                s.Children.Add(l);
                sl.Children.Add(sl);
                checks.Add(rb);
            }
            if (Stage < 4)
            {
                StackLayout s = new StackLayout() { Orientation = StackOrientation.Horizontal };
                Label l = new Label() { HorizontalOptions = LayoutOptions.StartAndExpand, Text = "Sale" };
                DataSwitch rb = new DataSwitch(3);
                s.Children.Add(l);
                sl.Children.Add(sl);
                checks.Add(rb);
            }
            if (Stage < 5)
            {
                StackLayout s = new StackLayout() { Orientation = StackOrientation.Horizontal };
                Label l = new Label() { HorizontalOptions = LayoutOptions.StartAndExpand, Text = "Install" };
                DataSwitch rb = new DataSwitch(3);
                s.Children.Add(l);
                sl.Children.Add(sl);
                checks.Add(rb);
            }
            if (Stage < 6)
            {
                StackLayout s = new StackLayout() { Orientation = StackOrientation.Horizontal };
                Label l = new Label() { HorizontalOptions = LayoutOptions.StartAndExpand, Text = "Installing" };
                DataSwitch rb = new DataSwitch(3);
                s.Children.Add(l);
                sl.Children.Add(sl);
                checks.Add(rb);
            }
            if (Stage < 7)
            {
                StackLayout s = new StackLayout() { Orientation = StackOrientation.Horizontal };
                Label l = new Label() { HorizontalOptions = LayoutOptions.StartAndExpand, Text = "Quality Assurance" };
                DataSwitch rb = new DataSwitch(3);
                s.Children.Add(l);
                sl.Children.Add(sl);
                checks.Add(rb);
            }
            if (Stage < 8)
            {
                StackLayout s = new StackLayout() { Orientation = StackOrientation.Horizontal };
                Label l = new Label() { HorizontalOptions = LayoutOptions.StartAndExpand, Text = "Quality Assurance" };
                DataSwitch rb = new DataSwitch(3);
                s.Children.Add(l);
                sl.Children.Add(sl);
                checks.Add(rb);
            }
            if (Stage < 9)
            {
                StackLayout s = new StackLayout() { Orientation = StackOrientation.Horizontal };
                Label l = new Label() { HorizontalOptions = LayoutOptions.StartAndExpand, Text = "Archive" };
                DataSwitch rb = new DataSwitch(3);
                s.Children.Add(l);
                sl.Children.Add(sl);
                checks.Add(rb);
            }
        }
    }
}