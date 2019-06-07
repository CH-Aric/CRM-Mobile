using MainCRMV2.Pages;
using System;
using System.CodeDom.Compiler;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Internals;

namespace MainCRMV2
{
    public partial class App : Application
    {
        public static MasterDetailPage MDP;
        public App()
        {
            InitializeComponent();
            base.MainPage = new NavigationPage();
            if (DatabaseFunctions.client.attemptSavedLoginAsync().GetAwaiter().GetResult())
            {
                App.MDP = new MasterDetailPage
                {
                    Master = new Home(),
                    Detail = new NavigationPage(new LinkPage("Welcome to the CoolHeat CRM"))
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
