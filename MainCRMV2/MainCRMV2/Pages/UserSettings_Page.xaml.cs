using System;
using System.Collections.Generic;
using MainCRMV2.Pages.Popup_Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MainCRMV2.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserSettings_Page : ContentPage
    {
        public UserSettings_Page()
        {
            InitializeComponent();
            string statement = "SELECT * FROM agents WHERE IDKey='" + ClientData.AgentIDK + "';";
            TaskCallback call = new TaskCallback(this.populateEntries);
            DatabaseFunctions.SendToPhp(false, statement, call);
        }
        public void populateEntries(string result)
        {
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            this.UsernameEntry.Text = dictionary["Username"][0];
            this.PasswordEntry.Text = dictionary["Password"][0];
            this.AgentID.Text = dictionary["AgentNum"][0];
            this.AgentIDK.Text = dictionary["IDKey"][0];
        }
        public async void onClicked(object sender, EventArgs e)
        {
            DatabaseFunctions.SendToPhp(string.Concat(new object[]
            {
                "UPDATE agents SET Username='",
                this.UsernameEntry.Text,
                "' ,Password='",
                this.PasswordEntry.Text,
                "' WHERE IDKey='",
                ClientData.AgentIDK,
                "'"
            }));
            TaskCallback c = new TaskCallback(this.voidCall);
            DatabaseFunctions.client.writeUserDataToFile(this.UsernameEntry.Text, this.PasswordEntry.Text);
            await PopupNavigation.Instance.PushAsync(new Notification_Popup("Your settings have been saved", "OK", c), true);
        }
        public async void voidCall(string x)
        {
            await PopupNavigation.Instance.PopAllAsync(true);
        }
    }
}
