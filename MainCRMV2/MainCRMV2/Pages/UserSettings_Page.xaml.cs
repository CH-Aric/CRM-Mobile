using System;
using System.Collections.Generic;
using MainCRMV2.Pages.Popup_Pages;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
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
            UsernameEntry.Text = dictionary["Username"][0];
            PasswordEntry.Text = dictionary["Password"][0];
            AgentID.Text = dictionary["AgentNum"][0];
            AgentIDK.Text = dictionary["IDKey"][0];
        }
        public void onClicked(object sender, EventArgs e)
        {
            DatabaseFunctions.SendToPhp( "UPDATE agents SET Username='"+ this.UsernameEntry.Text+"' ,Password='"+ this.PasswordEntry.Text+ "' WHERE IDKey='"+ ClientData.AgentIDK+"'");
            DatabaseFunctions.client.writeUserDataToFile(this.UsernameEntry.Text, this.PasswordEntry.Text);
        }
        public void onClickedPermissions(object sender, EventArgs e)
        {
            setPermissions();
        }
        public async void setPermissions()
        {
            var Callstatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Phone);
            if (Callstatus != PermissionStatus.Granted)
            {
                Dictionary<Permission, PermissionStatus> Callx = await CrossPermissions.Current.RequestPermissionsAsync(new Permission[] { Permission.Phone });
            }
            var Locstatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
            if (Locstatus != PermissionStatus.Granted)
            {
                Dictionary<Permission, PermissionStatus> x = await CrossPermissions.Current.RequestPermissionsAsync(new Permission[] { Permission.Location });
            }
            var Calstatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Calendar);
            if (Calstatus != PermissionStatus.Granted)
            {
                Dictionary<Permission, PermissionStatus> Calx = await CrossPermissions.Current.RequestPermissionsAsync(new Permission[] { Permission.Calendar });
            }
            
            var Medstatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.MediaLibrary);
            if (Medstatus != PermissionStatus.Granted)
            {
                Dictionary<Permission, PermissionStatus> Medx = await CrossPermissions.Current.RequestPermissionsAsync(new Permission[] { Permission.Phone });
            }
            var Sensstatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Sensors);
            if (Sensstatus != PermissionStatus.Granted)
            {
                Dictionary<Permission, PermissionStatus> Sensx = await CrossPermissions.Current.RequestPermissionsAsync(new Permission[] { Permission.Phone });
            }
        }
    }
}
