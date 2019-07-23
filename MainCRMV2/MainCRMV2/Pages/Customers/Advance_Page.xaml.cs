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
        List<SecurityButton> Buttons;
        public Advance_Page(int customerIn)
        {
            InitializeComponent();
            Buttons = new List<SecurityButton>();
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
                SecurityButton l = new SecurityButton(2,new string[] { "Employee"}) { HorizontalOptions=LayoutOptions.StartAndExpand, Text="Booking"};
                l.Clicked += onClicked;
                sl.Children.Add(l);
                Buttons.Add(l);
            }
            if (Stage < 3)
            {
                SecurityButton l = new SecurityButton(3, new string[] { "Employee" }) { HorizontalOptions = LayoutOptions.StartAndExpand, Text = "Quote" };
                l.Clicked += onClicked;
                sl.Children.Add(l);
                Buttons.Add(l);
            }
            if (Stage < 4)
            {
                SecurityButton l = new SecurityButton(4, new string[] { "Employee" }) { HorizontalOptions = LayoutOptions.StartAndExpand, Text = "Sale" };
                l.Clicked += onClicked;
                sl.Children.Add(l);
                Buttons.Add(l);
            }
            if (Stage < 5)
            {
                SecurityButton l = new SecurityButton(5, new string[] { "Employee" }) { HorizontalOptions = LayoutOptions.StartAndExpand, Text = "Install" };
                l.Clicked += onClicked;
                sl.Children.Add(l);
                Buttons.Add(l);
            }
            if (Stage < 6)
            {
                SecurityButton l = new SecurityButton(6, new string[] { "Employee" }) { HorizontalOptions = LayoutOptions.StartAndExpand, Text = "Installing" };
                l.Clicked += onClicked;
                sl.Children.Add(l);
                Buttons.Add(l);
            }
            if (Stage < 7)
            {
                SecurityButton l = new SecurityButton(7, new string[] { "Employee" }) { HorizontalOptions = LayoutOptions.StartAndExpand, Text = "Quality Assurance" };
                l.Clicked += onClicked;
                sl.Children.Add(l);
                Buttons.Add(l);
            }
            if (Stage < 8)
            {
                SecurityButton l = new SecurityButton(8, new string[] { "Employee" }) { HorizontalOptions = LayoutOptions.StartAndExpand, Text = "Clients" };
                l.Clicked += onClicked;
                sl.Children.Add(l);
                Buttons.Add(l);
            }
            if (Stage < 9)
            {
                SecurityButton l = new SecurityButton(9, new string[] { "Employee" }) { HorizontalOptions = LayoutOptions.StartAndExpand, Text = "Archive"};
                l.Clicked += onClicked;
                sl.Children.Add(l);
                Buttons.Add(l);
            }
            vc.View = sl;
            TSection.Add(vc);
        }
        public void onClicked(object sender, EventArgs e)
        {
            SecurityButton db = (SecurityButton)sender;
            string sql = "UPDATE cusindex SET Stage='"+db.GetInt()+"' WHERE IDKey='"+customer+"'";
            DatabaseFunctions.SendToPhp(sql);
            App.MDP.Detail.Navigation.PopToRootAsync();
        }
    }
}