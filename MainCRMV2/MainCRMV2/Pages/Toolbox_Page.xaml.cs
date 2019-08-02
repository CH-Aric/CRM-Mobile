using MainCRMV2.Pages.Inventory;
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
    public partial class Toolbox_Page : ContentPage
    {
        public Toolbox_Page(string Title)
        {
            base.Title = Title;
            InitializeComponent();
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
                App.MDP.Detail.Navigation.PushAsync(new Toolbox_Page("Not Implemented! Pick again!"));
            }
            else if (b.Text.Equals("Place a Call"))
            {
                App.MDP.Detail.Navigation.PushAsync(new Call_Page());
            }
        }
    }
}