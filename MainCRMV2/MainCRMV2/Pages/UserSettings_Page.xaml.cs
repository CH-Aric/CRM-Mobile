using System;
using System.Collections.Generic;
using MainCRMV2.Pages.Popup_Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MainCRMV2.Pages
{
    // Token: 0x0200002E RID: 46
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserSettings_Page : ContentPage
    {
        // Token: 0x060000CF RID: 207 RVA: 0x00008860 File Offset: 0x00006A60
        public UserSettings_Page()
        {
            InitializeComponent();
            string statement = "SELECT * FROM agents WHERE IDKey='" + ClientData.AgentIDK + "';";
            TaskCallback call = new TaskCallback(this.populateEntries);
            DatabaseFunctions.SendToPhp(false, statement, call);
        }

        // Token: 0x060000D0 RID: 208 RVA: 0x000088A8 File Offset: 0x00006AA8
        public void populateEntries(string result)
        {
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            this.UsernameEntry.Text = dictionary["Username"][0];
            this.PasswordEntry.Text = dictionary["Password"][0];
            this.AgentID.Text = dictionary["AgentNum"][0];
            this.AgentIDK.Text = dictionary["IDKey"][0];
        }

        // Token: 0x060000D1 RID: 209 RVA: 0x0000896C File Offset: 0x00006B6C
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

        // Token: 0x060000D2 RID: 210 RVA: 0x000089A8 File Offset: 0x00006BA8
        public async void voidCall(string x)
        {
            await PopupNavigation.Instance.PopAllAsync(true);
        }

        /*// Token: 0x060000D3 RID: 211 RVA: 0x000089DC File Offset: 0x00006BDC
        [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private void InitializeComponent()
        {
            if (ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
            {
                AssemblyName = typeof(UserSettings_Page).GetTypeInfo().Assembly.GetName(),
                ResourcePath = "Pages/UserSettings_Page.xaml"
            }))
            {
                this.__InitComponentRuntime();
                return;
            }
            if (XamlLoader.XamlFileProvider != null && XamlLoader.XamlFileProvider(base.GetType()) != null)
            {
                this.__InitComponentRuntime();
                return;
            }
            Label label = new Label();
            Button button = new Button();
            Label label2 = new Label();
            StackLayout stackLayout = new StackLayout();
            ViewCell viewCell = new ViewCell();
            Label label3 = new Label();
            Label label4 = new Label();
            Label label5 = new Label();
            StackLayout stackLayout2 = new StackLayout();
            ViewCell viewCell2 = new ViewCell();
            Label label6 = new Label();
            Entry entry = new Entry();
            StackLayout stackLayout3 = new StackLayout();
            ViewCell viewCell3 = new ViewCell();
            Label label7 = new Label();
            Entry entry2 = new Entry();
            StackLayout stackLayout4 = new StackLayout();
            ViewCell viewCell4 = new ViewCell();
            Label label8 = new Label();
            Entry entry3 = new Entry();
            StackLayout stackLayout5 = new StackLayout();
            ViewCell viewCell5 = new ViewCell();
            TableSection tableSection = new TableSection();
            TableRoot tableRoot = new TableRoot();
            TableView tableView = new TableView();
            NameScope nameScope = new NameScope();
            NameScope.SetNameScope(this, nameScope);
            ((INameScope)nameScope).RegisterName("TSection", tableSection);
            ((INameScope)nameScope).RegisterName("AgentIDK", label3);
            if (label3.StyleId == null)
            {
                label3.StyleId = "AgentIDK";
            }
            ((INameScope)nameScope).RegisterName("AgentID", label4);
            if (label4.StyleId == null)
            {
                label4.StyleId = "AgentID";
            }
            ((INameScope)nameScope).RegisterName("Active", label5);
            if (label5.StyleId == null)
            {
                label5.StyleId = "Active";
            }
            ((INameScope)nameScope).RegisterName("UsernameEntry", entry);
            if (entry.StyleId == null)
            {
                entry.StyleId = "UsernameEntry";
            }
            ((INameScope)nameScope).RegisterName("PasswordEntry", entry2);
            if (entry2.StyleId == null)
            {
                entry2.StyleId = "PasswordEntry";
            }
            ((INameScope)nameScope).RegisterName("CrewEntry", entry3);
            if (entry3.StyleId == null)
            {
                entry3.StyleId = "CrewEntry";
            }
            this.TSection = tableSection;
            this.AgentIDK = label3;
            this.AgentID = label4;
            this.Active = label5;
            this.UsernameEntry = entry;
            this.PasswordEntry = entry2;
            this.CrewEntry = entry3;
            tableSection.SetValue(TableSectionBase.TitleProperty, "Settings");
            stackLayout.SetValue(StackLayout.OrientationProperty, StackOrientation.Horizontal);
            label.SetValue(Label.TextProperty, "Index");
            stackLayout.Children.Add(label);
            button.SetValue(Button.TextProperty, "Save");
            button.Clicked += this.onClicked;
            stackLayout.Children.Add(button);
            label2.SetValue(Label.TextProperty, "Value");
            label2.SetValue(View.HorizontalOptionsProperty, LayoutOptions.EndAndExpand);
            stackLayout.Children.Add(label2);
            viewCell.View = stackLayout;
            tableSection.Add(viewCell);
            stackLayout2.SetValue(StackLayout.OrientationProperty, StackOrientation.Horizontal);
            label3.SetValue(Label.TextProperty, "AgentIDK");
            label3.SetValue(View.HorizontalOptionsProperty, LayoutOptions.StartAndExpand);
            stackLayout2.Children.Add(label3);
            label4.SetValue(Label.TextProperty, "AgentID");
            label4.SetValue(View.HorizontalOptionsProperty, LayoutOptions.CenterAndExpand);
            stackLayout2.Children.Add(label4);
            label5.SetValue(Label.TextProperty, "Active");
            label5.SetValue(View.HorizontalOptionsProperty, LayoutOptions.EndAndExpand);
            stackLayout2.Children.Add(label5);
            viewCell2.View = stackLayout2;
            tableSection.Add(viewCell2);
            stackLayout3.SetValue(StackLayout.OrientationProperty, StackOrientation.Horizontal);
            label6.SetValue(Label.TextProperty, "Username");
            label6.SetValue(View.HorizontalOptionsProperty, LayoutOptions.StartAndExpand);
            stackLayout3.Children.Add(label6);
            entry.SetValue(Entry.PlaceholderProperty, "Name");
            entry.SetValue(View.HorizontalOptionsProperty, LayoutOptions.EndAndExpand);
            stackLayout3.Children.Add(entry);
            viewCell3.View = stackLayout3;
            tableSection.Add(viewCell3);
            stackLayout4.SetValue(StackLayout.OrientationProperty, StackOrientation.Horizontal);
            label7.SetValue(Label.TextProperty, "Password");
            label7.SetValue(View.HorizontalOptionsProperty, LayoutOptions.StartAndExpand);
            stackLayout4.Children.Add(label7);
            entry2.SetValue(Entry.PlaceholderProperty, "Password");
            entry2.SetValue(View.HorizontalOptionsProperty, LayoutOptions.EndAndExpand);
            stackLayout4.Children.Add(entry2);
            viewCell4.View = stackLayout4;
            tableSection.Add(viewCell4);
            stackLayout5.SetValue(StackLayout.OrientationProperty, StackOrientation.Horizontal);
            label8.SetValue(Label.TextProperty, "Crew");
            label8.SetValue(View.HorizontalOptionsProperty, LayoutOptions.StartAndExpand);
            stackLayout5.Children.Add(label8);
            entry3.SetValue(Entry.PlaceholderProperty, "Name");
            entry3.SetValue(View.HorizontalOptionsProperty, LayoutOptions.EndAndExpand);
            stackLayout5.Children.Add(entry3);
            viewCell5.View = stackLayout5;
            tableSection.Add(viewCell5);
            tableRoot.Add(tableSection);
            tableView.Root = tableRoot;
            this.SetValue(ContentPage.ContentProperty, tableView);
        }

        // Token: 0x060000D4 RID: 212 RVA: 0x00008F64 File Offset: 0x00007164
        private void __InitComponentRuntime()
        {
            this.LoadFromXaml(typeof(UserSettings_Page));
            this.TSection = this.FindByName("TSection");
            this.AgentIDK = this.FindByName("AgentIDK");
            this.AgentID = this.FindByName("AgentID");
            this.Active = this.FindByName("Active");
            this.UsernameEntry = this.FindByName("UsernameEntry");
            this.PasswordEntry = this.FindByName("PasswordEntry");
            this.CrewEntry = this.FindByName("CrewEntry");
        }

        // Token: 0x04000091 RID: 145
        [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private TableSection TSection;

        // Token: 0x04000092 RID: 146
        [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private Label AgentIDK;

        // Token: 0x04000093 RID: 147
        [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private Label AgentID;

        // Token: 0x04000094 RID: 148
        [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private Label Active;

        // Token: 0x04000095 RID: 149
        [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private Entry UsernameEntry;

        // Token: 0x04000096 RID: 150
        [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private Entry PasswordEntry;

        // Token: 0x04000097 RID: 151
        [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private Entry CrewEntry;*/
    }
}
