using MainCRMV2.Pages;
using System;
using Xamarin.Forms;

namespace MainCRMV2
{
    public partial class App : Application
    {
        public static MasterDetailPage MDP;
        public static string WelcomeMsg="Welcome to the CoolHeat CRM";
        public App()
        {
            InitializeComponent();
            base.MainPage = new NavigationPage();
            if (DatabaseFunctions.client.attemptSavedLoginAsync().GetAwaiter().GetResult())
            {
                App.MDP = new MasterDetailPage
                {
                    Master = new home(),
                    Detail = new NavigationPage(new Toolbox_Page(WelcomeMsg))
                };
                Application.Current.MainPage = App.MDP;
                return;
            }
            base.MainPage.Navigation.PushAsync(new Login());
        }
        private void Clicked_Task(object sender, EventArgs e)
        {
            base.MainPage = new Tasks_Page();
        }
    }
}