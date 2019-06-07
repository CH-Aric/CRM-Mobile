using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Internals;

namespace MainCRMV2.Pages
{
    public partial class CustomerList_Page : ContentPage
    {
        private List<ViewCell> views;
        // Token: 0x0600008D RID: 141 RVA: 0x00005D7A File Offset: 0x00003F7A
        public CustomerList_Page()
        {
            this.InitializeComponent();
            this.loadFromDatabase();
        }

        // Token: 0x0600008E RID: 142 RVA: 0x00005D90 File Offset: 0x00003F90
        public void loadFromDatabase()
        {
            TaskCallback call = new TaskCallback(this.populateList);
            DatabaseFunctions.SendToPhp(false, "SELECT cusindex.Name,cusindex.IDKey,cusfields.Value,cusfields.Index FROM cusindex INNER JOIN cusfields ON cusindex.IDKey=cusfields.CusID WHERE cusfields.Index LIKE '%Address%' OR cusfields.Index LIKE '%Phone%';", call);
        }

        // Token: 0x0600008F RID: 143 RVA: 0x00005DB8 File Offset: 0x00003FB8
        public void populateList(string result)
        {
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            Dictionary<string, DataButton> dictionary2 = new Dictionary<string, DataButton>();
            this.views = new List<ViewCell>();
            if (dictionary.Count > 0)
            {
                for (int i = 0; i < dictionary["Name"].Count; i++)
                {
                    if (!dictionary2.ContainsKey(dictionary["IDKey"][i]))
                    {
                        string text = dictionary["Name"][i] + ": " + dictionary["Value"][i];
                        DataButton dataButton = new DataButton(int.Parse(dictionary["IDKey"][i]))
                        {
                            Text = text,
                            FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                            VerticalOptions = LayoutOptions.CenterAndExpand,
                            HorizontalOptions = LayoutOptions.CenterAndExpand
                        };
                        dataButton.Clicked += this.onClicked;
                        ViewCell viewCell = new ViewCell();
                        StackLayout stackLayout = new StackLayout
                        {
                            Orientation = StackOrientation.Horizontal,
                            HorizontalOptions = LayoutOptions.Start
                        };
                        stackLayout.Children.Add(dataButton);
                        viewCell.View = stackLayout;
                        this.TSection.Add(viewCell);
                        this.views.Add(viewCell);
                        dictionary2.Add(dictionary["IDKey"][i], dataButton);
                    }
                    else
                    {
                        DataButton dataButton2 = dictionary2[dictionary["IDKey"][i]];
                        dataButton2.Text = dataButton2.Text + ", " + dictionary["Value"][i];
                    }
                }
            }
        }

        // Token: 0x06000090 RID: 144 RVA: 0x00005F64 File Offset: 0x00004164
        public void onClicked(object sender, EventArgs e)
        {
            DataButton dataButton = (DataButton)sender;
            App.MDP.Detail.Navigation.PushAsync(new CustomerDetail_Page(dataButton.Integer));
            App.MDP.IsPresented = false;
        }

        // Token: 0x06000091 RID: 145 RVA: 0x00005FA4 File Offset: 0x000041A4
        public void onClickedSearch(object sender, EventArgs e)
        {
            string text = "%" + SearchEntry.Text + "%";
            string statement = string.Concat(new string[]
            {
                "SELECT DISTINCT cusindex.IDKey FROM cusindex INNER JOIN cusfields ON cusindex.IDKey=cusfields.CusID WHERE cusfields.Value LIKE '%",
                text,
                "%' OR cusindex.Name LIKE '%",
                text,
                "%';"
            });
            TaskCallback call = new TaskCallback(this.PerformSearch);
            DatabaseFunctions.SendToPhp(false, statement, call);
        }

        // Token: 0x06000092 RID: 146 RVA: 0x00006010 File Offset: 0x00004210
        public void PerformSearch(string result)
        {
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            TaskCallback call = new TaskCallback(this.populateList);
            string text = "SELECT cusindex.Name,cusindex.IDKey,cusfields.Value,cusfields.Index FROM cusindex INNER JOIN cusfields ON cusindex.IDKey=cusfields.CusID WHERE (cusfields.Index LIKE '%Address%' OR cusfields.Index LIKE '%Phone%') AND (";
            foreach (string str in dictionary["IDKey"])
            {
                text = text + " cusindex.IDKey='" + str + "' OR";
            }
            text += " cusindex.IDKey='0');";
            this.PurgeCells();
            DatabaseFunctions.SendToPhp(false, text, call);
        }

        // Token: 0x06000093 RID: 147 RVA: 0x000060AC File Offset: 0x000042AC
        public void PurgeCells()
        {
            foreach (ViewCell item in this.views)
            {
                this.TSection.Remove(item);
            }
        }

        /*// Token: 0x06000094 RID: 148 RVA: 0x00006108 File Offset: 0x00004308
        [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private void InitializeComponent()
        {
            if (ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
            {
                AssemblyName = typeof(CustomerList_Page).GetTypeInfo().Assembly.GetName(),
                ResourcePath = "Pages/CustomerList_Page.xaml"
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
            Entry entry = new Entry();
            Button button = new Button();
            Label label2 = new Label();
            StackLayout stackLayout = new StackLayout();
            ViewCell viewCell = new ViewCell();
            TableSection tableSection = new TableSection();
            TableRoot tableRoot = new TableRoot();
            TableView tableView = new TableView();
            NameScope nameScope = new NameScope();
            NameScope.SetNameScope(this, nameScope);
            ((INameScope)nameScope).RegisterName("TSection", tableSection);
            ((INameScope)nameScope).RegisterName("SearchEntry", entry);
            if (entry.StyleId == null)
            {
                entry.StyleId = "SearchEntry";
            }
            this.TSection = tableSection;
            this.SearchEntry = entry;
            tableSection.SetValue(TableSectionBase.TitleProperty, "Customer List");
            stackLayout.SetValue(StackLayout.OrientationProperty, StackOrientation.Horizontal);
            label.SetValue(Label.TextProperty, "Index");
            stackLayout.Children.Add(label);
            entry.SetValue(Entry.PlaceholderProperty, "Search");
            stackLayout.Children.Add(entry);
            button.SetValue(Button.TextProperty, "Search");
            button.Clicked += this.onClickedSearch;
            stackLayout.Children.Add(button);
            label2.SetValue(Label.TextProperty, "Value");
            label2.SetValue(View.HorizontalOptionsProperty, LayoutOptions.EndAndExpand);
            stackLayout.Children.Add(label2);
            viewCell.View = stackLayout;
            tableSection.Add(viewCell);
            tableRoot.Add(tableSection);
            tableView.Root = tableRoot;
            this.SetValue(ContentPage.ContentProperty, tableView);
        }

        // Token: 0x06000095 RID: 149 RVA: 0x000062F2 File Offset: 0x000044F2
        private void __InitComponentRuntime()
        {
            this.LoadFromXaml(typeof(CustomerList_Page));
            this.TSection = this.FindByName("TSection");
            this.SearchEntry = this.FindByName("SearchEntry");
        }

        // Token: 0x04000056 RID: 86
        

        // Token: 0x04000057 RID: 87
        [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private TableSection TSection;

        // Token: 0x04000058 RID: 88
        [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private Entry SearchEntry;*/
    }
}
