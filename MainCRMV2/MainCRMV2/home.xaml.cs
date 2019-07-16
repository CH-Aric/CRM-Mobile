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
            MDPMain();
        }
        public void MDPMain()
        {
            base.Content = new StackLayout
            {
                Padding = new Thickness(0.0, 10.0, 0.0, 0.0),//Controls Menu Button Layout
                Children = {
                {
                    new MainLink("Chat")
                },
                {
                    new MainLink("Tasks")
                },
                {
                    new MainLink("Customers")
                },
                {
                    new MainLink("CDR")
                },
                {
                    new MainLink("Account")
                },
                {
                    new MainLink("Coupon Checker")
                },
                {
                    new MainLink("Inventory")
                },
                 {
                    new MainLink("Calls Received")
                },
                 {
                    new MainLink("Price Guide")
                },
                {
                    new MainLink("Logout")
                }
                }
            };
            base.Title = "Master";
            base.BackgroundColor = Color.Gray.WithLuminosity(0.9);
        }
        public void RenderHomeScreen()
        {

        }
        private async void Clicked_Task(object sender, EventArgs e)
        {
            await Application.Current.MainPage.Navigation.PushAsync(new Tasks_Page());
        }
    }
}
