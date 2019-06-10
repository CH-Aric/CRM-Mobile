using MainCRMV2.Pages;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MainCRMV2
{
    public partial class Home : ContentPage
    {
        public Home()
        {
            
            this.MDPMain();
        }
        public void MDPMain()
        {
            base.Content = new StackLayout
            {
                Padding = new Thickness(0.0, 20.0, 0.0, 0.0),
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
                    new MainLink("Logout")
                }
                }
            };
            base.Title = "Master";
            base.BackgroundColor = Color.Gray.WithLuminosity(0.9);
        }

        private async void Clicked_Task(object sender, EventArgs e)
        {
            await Application.Current.MainPage.Navigation.PushAsync(new Tasks_Page());
        }
    }
}
