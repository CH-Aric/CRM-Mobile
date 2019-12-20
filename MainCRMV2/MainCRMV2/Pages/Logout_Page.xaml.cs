using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MainCRMV2.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Logout_Page : ContentPage
    {
        public Logout_Page()
        {
            InitializeComponent();
        }
        public void Clicked(object sender, EventArgs e)
        {
            DatabaseFunctions.client.wipeUserDataFromFile();
            Application.Current.MainPage = new NavigationPage();
            Application.Current.MainPage.Navigation.PushAsync(new Login());
        }
    }
}
