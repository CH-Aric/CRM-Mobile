﻿using System;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MainCRMV2.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();
        }
        private void Button_Clicked(object sender, EventArgs e)
        {
        }
        public void LoginClick(object sender, EventArgs e)
        {
            TaskAwaiter<bool> taskAwaiter = DatabaseFunctions.client.attemptNewLogin(this.userN.Text, this.pass.Text, this.stay.IsToggled).GetAwaiter();
            while (!taskAwaiter.IsCompleted)
            {

            }
            if (taskAwaiter.GetResult())
            {
                string WelcomeMsg="Welcome to the CoolHeat CRM";
                App.MDP = new MasterDetailPage
                {
                    Master = new home(),
                    Detail = new NavigationPage(new Toolbox_Page(WelcomeMsg))
                };
                Application.Current.MainPage = App.MDP;
            }
            else
            {
                Message.Text = "Username or Password incorrect, Try again, or contact Support.";
            }
        }
    }
}
