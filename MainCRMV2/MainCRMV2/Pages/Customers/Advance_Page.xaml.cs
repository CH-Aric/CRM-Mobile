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
        List<DataButton> Buttons;
        public Advance_Page(int customerIn)
        {
            InitializeComponent();
            Buttons = new List<DataButton>();
            customer = customerIn;
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
            ViewCell vc = new ViewCell();
            
            StackLayout sl = new StackLayout() { Orientation=StackOrientation.Vertical};
            int Stage = int.Parse(dictionary["Stage"][0]);
            if (Stage<2)
            {
                DataButton l = new DataButton(2) { HorizontalOptions=LayoutOptions.StartAndExpand, Text="Booking"};
                l.Clicked += onClicked;
                sl.Children.Add(l);
                Buttons.Add(l);
            }
            if (Stage < 3)
            {
                DataButton l = new DataButton(3) { HorizontalOptions = LayoutOptions.StartAndExpand, Text = "Quote" };
                l.Clicked += onClicked;
                sl.Children.Add(l);
                Buttons.Add(l);
            }
            if (Stage < 4)
            {
                DataButton l = new DataButton(4) { HorizontalOptions = LayoutOptions.StartAndExpand, Text = "Sale" };
                l.Clicked += onClicked;
                sl.Children.Add(l);
                Buttons.Add(l);
            }
            if (Stage < 5)
            {
                DataButton l = new DataButton(5) { HorizontalOptions = LayoutOptions.StartAndExpand, Text = "Install" };
                l.Clicked += onClicked;
                sl.Children.Add(l);
                Buttons.Add(l);
            }
            if (Stage < 6)
            {
                DataButton l = new DataButton(6) { HorizontalOptions = LayoutOptions.StartAndExpand, Text = "Installing" };
                l.Clicked += onClicked;
                sl.Children.Add(l);
                Buttons.Add(l);
            }
            if (Stage < 7)
            {
                DataButton l = new DataButton(7) { HorizontalOptions = LayoutOptions.StartAndExpand, Text = "Quality Assurance" };
                l.Clicked += onClicked;
                sl.Children.Add(l);
                Buttons.Add(l);
            }
            if (Stage < 8)
            {
                DataButton l = new DataButton(8) { HorizontalOptions = LayoutOptions.StartAndExpand, Text = "Clients" };
                l.Clicked += onClicked;
                sl.Children.Add(l);
                Buttons.Add(l);
            }
            if (Stage < 9)
            {
                DataButton l = new DataButton(9) { HorizontalOptions = LayoutOptions.StartAndExpand, Text = "Archive"};
                l.Clicked += onClicked;
                sl.Children.Add(l);
                Buttons.Add(l);
            }
            vc.View = sl;
            TSection.Add(vc);
        }
        public void onClicked(object sender, EventArgs e)
        {
            DataButton db = (DataButton)sender;
            string sql = "UPDATE cusindex SET Stage='"+db.GetInt()+"' WHERE IDKey='"+customer+"'";
            DatabaseFunctions.SendToPhp(sql);
            App.MDP.Detail.Navigation.PopToRootAsync();
        }
    }
}