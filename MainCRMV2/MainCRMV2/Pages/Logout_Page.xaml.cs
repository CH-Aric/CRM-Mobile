using Rg.Plugins.Popup.Pages;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MainCRMV2.Pages
{
    // Token: 0x0200001A RID: 26
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Logout_Page : ContentPage
    {
        public Logout_Page()
        {
            InitializeComponent();
        }
        private void Button_Clicked(object sender, EventArgs e)
        {
        }
        public void Clicked(object sender, EventArgs e)
        {
            DatabaseFunctions.client.wipeUserDataFromFile();
            Application.Current.MainPage = new NavigationPage();
            Application.Current.MainPage.Navigation.PushAsync(new Login());
        }

    }
}
