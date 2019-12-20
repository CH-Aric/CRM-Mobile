using MainCRMV2.Pages.Inventory;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MainCRMV2.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Toolbox_Page : ContentPage
    {
        public Toolbox_Page(string Title)
        {
            base.Title = Title;
            InitializeComponent();
            //DatabaseFunctions.SendToDebug("Creating Toolbox!");
            displayDailyTasks();
        }

        public void onClickAccess(object sender,EventArgs e)
        {
            Button b = (Button)sender;
            if (b.Text.Equals("Pricing Guide"))
            {
                App.MDP.Detail.Navigation.PushAsync(new Price_Page());
            }
            else if (b.Text.Equals("Coupon Checker"))
            {
                App.MDP.Detail.Navigation.PushAsync(new CouponChecker_Page());
            }
            else if (b.Text.Equals("Chat"))
            {
                App.MDP.Detail.Navigation.PushAsync(new Chat_Page());
            }
            else if (b.Text.Equals("Manage Favourites"))
            {
                App.MDP.Detail.Navigation.PushAsync(new Favorites_Page());
            }
            else if (b.Text.Equals("Inventory"))
            {
                App.MDP.Detail.Navigation.PushAsync(new Items_Page());
            }
            else if (b.Text.Equals("Pricing Guide"))
            {
                App.MDP.Detail.Navigation.PushAsync(new Price_Page());
            }
            else if (b.Text.Equals("Perform an Audit"))
            {
                App.MDP.Detail.Navigation.PushAsync(new Toolbox_Page("Soon, I shall be a thing!"));
            }
            else if (b.Text.Equals("Punch Admin"))
            {
                if (ClientData.hasSecurityKey("Admin"))
                {
                    App.MDP.Detail.Navigation.PushAsync(new PunchAdmin());
                }
                else
                {
                    App.MDP.Detail.Navigation.PushAsync(new Toolbox_Page("Access Denied, Admin Access required! For Now..."));
                }
            }
            else if (b.Text.Equals("Punch Clock"))
            {
                App.MDP.Detail.Navigation.PushAsync(new Punch_Page());
            }
            //DatabaseFunctions.SendToDebug("Access Clicked");
        }
        public void displayDailyTasks()
        {
            //DatabaseFunctions.SendToDebug("Beginning display of daily tasks");
            if (ClientData.hasRole("1"))
            {
                TaskCallback call = populateList;
                DatabaseFunctions.SendToPhp(false,"SELECT * FROM Tasks WHERE AgentID='"+ClientData.AgentIDK+"' OR GroupID IN(SELECT GroupID FROM crm2.groupmembers WHERE MemberID='"+ClientData.AgentIDK+ "');", call);
            }
            else
            {
                tasksList.IsVisible = false;
            }
            //DatabaseFunctions.SendToDebug("Completed display of daily tasks");
        }
        public void populateList(string result)
        {
            //DatabaseFunctions.SendToDebug("Beginning populating daily tasks");
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            if (dictionary.Count > 1)
            {
                for (int i = 0; i < dictionary["Name"].Count; i++)
                {
                    string text = dictionary["Name"][i] ?? "";
                    SecurityButton dataButton = new SecurityButton(int.Parse(dictionary["IDKey"][i]), new string[] { "Employee" })
                    {
                        Text = text,
                        FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        HorizontalOptions = LayoutOptions.FillAndExpand
                    };
                    dataButton.Clicked += this.onClicked;
                    List<View> list = new List<View>();
                    list.Add(dataButton);
                    GridFiller.rapidFillPremadeObjects(list, tasksList, new bool[] { true, true });
                }
            }
            //DatabaseFunctions.SendToDebug("Finsihed populating daily tasks");
        }
        public void onClicked(object sender, EventArgs e)
        {
            SecurityButton dataButton = (SecurityButton)sender;
            App.MDP.Detail.Navigation.PushAsync(new TaskEdit_Page(dataButton.Integer));
            App.MDP.IsPresented = false;
        }
    }
}