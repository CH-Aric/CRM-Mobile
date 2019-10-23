using MainCRMV2.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace MainCRMV2
{
    public partial class home : ContentPage
    {
        public home()
        {

            InitializeComponent();
            //DatabaseFunctions.SendToDebug("Created Home, beginning creation of MDP");
            MDPMain();
            //DatabaseFunctions.SendToDebug("Finished creating MDPMain");
        }
        public void MDPMain()
        {
            baseMDP.Padding = Padding = new Thickness(0.0, 10.0, 0.0, 0.0);//Controls Menu Button Layout
            List<View> v = new List<View>(){new MainLink("Chat") ,new MainLink("Tasks") ,new MainLink("Customers") ,new MainLink("CDR"),new MainLink("Account"), new MainLink("Coupon Checker") ,new MainLink("Inventory") ,new MainLink("Calls Received") ,new MainLink("Price Guide"),new MainLink("Logout") };
            //DatabaseFunctions.SendToDebug("Pupulating the MDP with premade buttons");
            foreach ( View n in v)
            {
                baseMDP.Children.Add(n);
            }
            base.Title = "Master";
            base.BackgroundColor = Color.Gray.WithLuminosity(0.9);
        }
    }
}
