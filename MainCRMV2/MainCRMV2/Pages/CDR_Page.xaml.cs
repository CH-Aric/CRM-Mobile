using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Internals;

namespace MainCRMV2.Pages
{
    public partial class CDR_Page : ContentPage
    {
        // Token: 0x0600006C RID: 108 RVA: 0x00004402 File Offset: 0x00002602
        private List<ViewCell> views;
        public CDR_Page(bool Searchmode, string Searchfor)
        {

            views = new List<ViewCell>();
            this.InitializeComponent();
            if (!Searchmode)
            {
                SearchEntry.Text = Searchfor;
                SearchEntry.IsEnabled = false;
                SearchButton.IsEnabled = false;
            }
        }

        // Token: 0x0600006D RID: 109 RVA: 0x00004444 File Offset: 0x00002644
        public void PerformSearch()
        {
            string statement = "SELECT DISTINCT CusID FROM cusfields WHERE Value LIKE '%" + this.SearchEntry.Text + "%'";
            TaskCallback call = new TaskCallback(this.performSearch2);
            DatabaseFunctions.SendToPhp(false, statement, call);
        }

        // Token: 0x0600006E RID: 110 RVA: 0x00004484 File Offset: 0x00002684
        public void performSearch2(string result)
        {
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            string text = "SELECT DISTINCT cusfields.Value FROM cusfields WHERE cusfields.Index LIKE '%Phone%' AND (cusfields.Value='-14' ";
            foreach (string str in dictionary["CusID"])
            {
                text = text + "OR CusID = '" + str + "' ";
            }
            text += ");";
            TaskCallback call = new TaskCallback(this.performSearch3);
            DatabaseFunctions.SendToPhp(false, text, call);
        }

        // Token: 0x0600006F RID: 111 RVA: 0x00004518 File Offset: 0x00002718
        public void performSearch3(string result)
        {
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            string text = "SELECT uniqueid,cnum,cnam,disposition,duration,did,recordingfile,src,calldate FROM asteriskcdrdb.cdr WHERE ";
            foreach (string phone in dictionary["Value"])
            {
                string text2 = FormatFunctions.CleanPhone(phone);
                text = string.Concat(new string[]
                {
                    text,
                    "dst LIKE'%",
                    text2,
                    "%' OR src LIKE'%",
                    text2,
                    "%' OR clid LIKE'%",
                    text2,
                    "%' OR cnam LIKE'%",
                    text2,
                    "%' OR cnum LIKE'%",
                    text2,
                    "%' OR outbound_cnam LIKE'%",
                    text2,
                    "%' OR outbound_cnum LIKE'%",
                    text2,
                    "%' OR "
                });
            }
            text = Regex.Replace(text, "[-()]", "");
            text += "src='-14' LIMIT 500;";
            TaskCallback call = new TaskCallback(this.populateResults);
            DatabaseFunctions.SendToPhp(true, text, call);
        }

        // Token: 0x06000070 RID: 112 RVA: 0x0000462C File Offset: 0x0000282C
        public void populateResults(string result)
        {
            this.PurgeCells();
            this.views = new List<ViewCell>();
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            if (dictionary.Count > 1)
            {
                for (int i = 0; i < dictionary["uniqueid"].Count; i++)
                {
                    string text = dictionary["calldate"][i] + " : " + dictionary["cnam"][i];
                    DataButton dataButton = new DataButton(int.Parse(Regex.Replace(dictionary["uniqueid"][i], "^[^.]+.", "")))
                    {
                        Text = text,
                        FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        HorizontalOptions = LayoutOptions.EndAndExpand
                    };
                    dataButton.Clicked += this.onClicked;
                    dataButton.String = dictionary["calldate"][i];
                    dataButton.String2 = dictionary["recordingfile"][i];
                    ViewCell viewCell = new ViewCell();
                    StackLayout stackLayout = new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal
                    };
                    stackLayout.Children.Add(dataButton);
                    viewCell.View = stackLayout;
                    this.views.Add(viewCell);
                    this.TSection.Add(viewCell);
                }
            }
        }

        // Token: 0x06000071 RID: 113 RVA: 0x00002149 File Offset: 0x00000349
        public void openFile(string result)
        {
        }

        // Token: 0x06000072 RID: 114 RVA: 0x00004794 File Offset: 0x00002994
        public void onClicked(object sender, EventArgs e)
        {
            DataButton dataButton = (DataButton)sender;
            TaskCallback call = new TaskCallback(this.openFile);
            DatabaseFunctions.getFile(dataButton.String, dataButton.String2, call);
        }

        // Token: 0x06000073 RID: 115 RVA: 0x000047C8 File Offset: 0x000029C8
        public void onClickedSearch(object sender, EventArgs e)
        {
            this.PerformSearch();
        }

        // Token: 0x06000074 RID: 116 RVA: 0x000047D0 File Offset: 0x000029D0
        public void onClickedExplicitySearch(object sender, EventArgs e)
        {
            string text = "SELECT uniqueid,cnum,cnam,disposition,duration,did,recordingfile,src,calldate FROM asteriskcdrdb.cdr WHERE ";
            string text2 = this.SearchEntry.Text;
            text = string.Concat(new string[]
            {
                text,
                "dst LIKE'%",
                text2,
                "%' OR src LIKE'%",
                text2,
                "%' OR clid LIKE'%",
                text2,
                "%' OR cnam LIKE'%",
                text2,
                "%' OR cnum LIKE'%",
                text2,
                "%' OR outbound_cnam LIKE'%",
                text2,
                "%' OR outbound_cnum LIKE'%",
                text2,
                "%' OR "
            });
            text = Regex.Replace(text, "[-()]", "");
            text += "src='-14' LIMIT 500;";
            TaskCallback call = new TaskCallback(this.populateResults);
            DatabaseFunctions.SendToPhp(true, text, call);
        }

        // Token: 0x06000075 RID: 117 RVA: 0x00004898 File Offset: 0x00002A98
        public void PurgeCells()
        {
            if (this.views.Count > 0)
            {
                foreach (ViewCell item in this.views)
                {
                    this.TSection.Remove(item);
                }
            }
        }
        /*
        // Token: 0x06000076 RID: 118 RVA: 0x00004900 File Offset: 0x00002B00
        [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private void InitializeComponent()
        {
            if (ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
            {
                AssemblyName = typeof(CDR_Page).GetTypeInfo().Assembly.GetName(),
                ResourcePath = "Pages/CDR_Page.xaml"
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
            Button button2 = new Button();
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
            ((INameScope)nameScope).RegisterName("SearchButton", button);
            if (button.StyleId == null)
            {
                button.StyleId = "SearchButton";
            }
            ((INameScope)nameScope).RegisterName("ExSearchButton", button2);
            if (button2.StyleId == null)
            {
                button2.StyleId = "ExSearchButton";
            }
            this.TSection = tableSection;
            this.SearchEntry = entry;
            this.SearchButton = button;
            this.ExSearchButton = button2;
            tableSection.SetValue(TableSectionBase.TitleProperty, "Call Detail Record");
            stackLayout.SetValue(StackLayout.OrientationProperty, StackOrientation.Horizontal);
            label.SetValue(Label.TextProperty, "Index");
            stackLayout.Children.Add(label);
            entry.SetValue(Entry.PlaceholderProperty, "Search");
            stackLayout.Children.Add(entry);
            button.SetValue(Button.TextProperty, "Search");
            button.Clicked += this.onClickedSearch;
            stackLayout.Children.Add(button);
            button2.SetValue(Button.TextProperty, "D-Search");
            button2.Clicked += this.onClickedExplicitySearch;
            stackLayout.Children.Add(button2);
            label2.SetValue(Label.TextProperty, "Value");
            label2.SetValue(View.HorizontalOptionsProperty, LayoutOptions.EndAndExpand);
            stackLayout.Children.Add(label2);
            viewCell.View = stackLayout;
            tableSection.Add(viewCell);
            tableRoot.Add(tableSection);
            tableView.Root = tableRoot;
            this.SetValue(ContentPage.ContentProperty, tableView);
        }

        // Token: 0x06000077 RID: 119 RVA: 0x00004B74 File Offset: 0x00002D74
        private void __InitComponentRuntime()
        {
            this.LoadFromXaml(typeof(CDR_Page));
            this.TSection = this.FindByName("TSection");
            this.SearchEntry = this.FindByName("SearchEntry");
            this.SearchButton = this.FindByName("SearchButton");
            this.ExSearchButton = this.FindByName("ExSearchButton");
        }

        // Token: 0x04000044 RID: 68
        

        // Token: 0x04000045 RID: 69
        [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private TableSection TSection;

        // Token: 0x04000046 RID: 70
        [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private Entry SearchEntry;

        // Token: 0x04000047 RID: 71
        [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private Button SearchButton;

        // Token: 0x04000048 RID: 72
        [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private Button ExSearchButton;*/
    }
}
