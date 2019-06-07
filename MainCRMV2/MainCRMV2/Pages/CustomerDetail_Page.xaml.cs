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
    public partial class CustomerDetail_Page : ContentPage
    {
        private List<DataPair> entryDict;

        private int customer;

        public CustomerDetail_Page(int i)
        {
            customer = i;
            InitializeComponent();
            TaskCallback call = new TaskCallback(this.populatePage);
            DatabaseFunctions.SendToPhp(false, "SELECT cusindex.Name,cusfields.IDKey AS FID,cusindex.IDKey,cusfields.value,cusfields.Index FROM cusfields INNER JOIN cusindex ON cusfields.cusID=cusindex.IDKey WHERE cusfields.CusID='" + i + "';", call);
        }

        // Token: 0x06000086 RID: 134 RVA: 0x0000561C File Offset: 0x0000381C
        public void populatePage(string result)
        {
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            entryDict = new List<DataPair>();
            NameDisplay.Text = dictionary["Name"][0];
            if (dictionary.Count > 0)
            {
                for (int i = 0; i < dictionary["Index"].Count; i++)
                {
                    DataPair dataPair = new DataPair(int.Parse(dictionary["FID"][i]), dictionary["value"][i], dictionary["Index"][i]);
                    dataPair.Value.Text = dictionary["value"][i];
                    dataPair.Value.Placeholder = "Value here";
                    dataPair.Value.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
                    dataPair.Value.VerticalOptions = LayoutOptions.CenterAndExpand;
                    dataPair.Value.HorizontalOptions = LayoutOptions.StartAndExpand;
                    dataPair.Index.Text = dictionary["Index"][i];
                    dataPair.Index.Placeholder = "Index here";
                    dataPair.Index.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
                    dataPair.Index.VerticalOptions = LayoutOptions.CenterAndExpand;
                    dataPair.Index.HorizontalOptions = LayoutOptions.EndAndExpand;
                    ViewCell viewCell = new ViewCell();
                    StackLayout stackLayout = new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal
                    };
                    stackLayout.Children.Add(dataPair.Index);
                    stackLayout.Children.Add(dataPair.Value);
                    viewCell.View = stackLayout;
                    this.TSection.Add(viewCell);
                    this.entryDict.Add(dataPair);
                }
            }
            this.populateFileList();
        }

        // Token: 0x06000087 RID: 135 RVA: 0x000057F4 File Offset: 0x000039F4
        public void populateFileList()
        {
            string[] customerFileList = DatabaseFunctions.getCustomerFileList(this.NameDisplay.Text);
            foreach (string text in customerFileList)
            {
                if ((text != "." || text != "..") && customerFileList.Length > 1)
                {
                    DataButton dataButton = new DataButton(this.NameDisplay.Text + "/" + text)
                    {
                        Text = text,
                        FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        HorizontalOptions = LayoutOptions.CenterAndExpand
                    };
                    dataButton.Clicked += this.onFileButton;
                    ViewCell item = new ViewCell();
                    StackLayout stackLayout = new StackLayout();
                    stackLayout.Orientation = StackOrientation.Horizontal;
                    stackLayout.Children.Add(dataButton);
                    this.TSection.Add(item);
                }
            }
        }

        // Token: 0x06000088 RID: 136 RVA: 0x000058E4 File Offset: 0x00003AE4
        public void onClicked(object sender, EventArgs e)
        {
            foreach (DataPair dataPair in this.entryDict)
            {
                if (dataPair.isNew)
                {
                    DatabaseFunctions.SendToPhp(string.Concat(new object[]
                    {
                        "INSERT INTO cusfields (cusfields.Value,cusfields.Index,CusID) VALUES('",
                        dataPair.Value.Text,
                        "','",
                        dataPair.Index.Text,
                        "','",
                        this.customer,
                        "')"
                    }));
                    dataPair.isNew = false;
                }
                else if (dataPair.Index.Text != dataPair.Index.GetInit())
                {
                    DatabaseFunctions.SendToPhp(string.Concat(new object[]
                    {
                        "UPDATE cusfields SET Value = '",
                        dataPair.Value.Text,
                        "',Index='",
                        dataPair.Index.Text,
                        "' WHERE (IDKey= '",
                        dataPair.Index.GetInt(),
                        "');"
                    }));
                }
            }
        }

        // Token: 0x06000089 RID: 137 RVA: 0x00005A2C File Offset: 0x00003C2C
        public void onClickAddFields(object sender, EventArgs e)
        {
            DataPair dataPair = new DataPair(0, "", "");
            dataPair.setNew();
            dataPair.Value.Text = "";
            dataPair.Value.Placeholder = "Index here";
            dataPair.Value.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
            dataPair.Value.VerticalOptions = LayoutOptions.CenterAndExpand;
            dataPair.Value.HorizontalOptions = LayoutOptions.StartAndExpand;
            dataPair.Index.Text = "";
            dataPair.Index.Placeholder = "Value here";
            dataPair.Index.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
            dataPair.Index.VerticalOptions = LayoutOptions.CenterAndExpand;
            dataPair.Index.HorizontalOptions = LayoutOptions.EndAndExpand;
            ViewCell viewCell = new ViewCell();
            StackLayout stackLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal
            };
            stackLayout.Children.Add(dataPair.Index);
            stackLayout.Children.Add(dataPair.Value);
            viewCell.View = stackLayout;
            this.TSection.Add(viewCell);
            this.entryDict.Add(dataPair);
        }

        // Token: 0x0600008A RID: 138 RVA: 0x00002149 File Offset: 0x00000349
        public void onFileButton(object sender, EventArgs e)
        {
        }

        /*// Token: 0x0600008B RID: 139 RVA: 0x00005B5C File Offset: 0x00003D5C
        [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private void InitializeComponent()
        {
            if (ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
            {
                AssemblyName = typeof(CustomerDetail_Page).GetTypeInfo().Assembly.GetName(),
                ResourcePath = "Pages/CustomerDetail_Page.xaml"
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
            Button button2 = new Button();
            StackLayout stackLayout = new StackLayout();
            ViewCell viewCell = new ViewCell();
            TableSection tableSection = new TableSection();
            TableRoot tableRoot = new TableRoot();
            TableView tableView = new TableView();
            NameScope nameScope = new NameScope();
            NameScope.SetNameScope(this, nameScope);
            ((INameScope)nameScope).RegisterName("TSection", tableSection);
            ((INameScope)nameScope).RegisterName("NameDisplay", label);
            if (label.StyleId == null)
            {
                label.StyleId = "NameDisplay";
            }
            this.TSection = tableSection;
            this.NameDisplay = label;
            tableSection.SetValue(TableSectionBase.TitleProperty, "Customer Details");
            stackLayout.SetValue(StackLayout.OrientationProperty, StackOrientation.Horizontal);
            label.SetValue(Label.TextProperty, "");
            label.SetValue(View.VerticalOptionsProperty, LayoutOptions.StartAndExpand);
            label.SetValue(View.HorizontalOptionsProperty, LayoutOptions.CenterAndExpand);
            stackLayout.Children.Add(label);
            button.SetValue(Button.TextProperty, "Add Field");
            button.Clicked += this.onClickAddFields;
            stackLayout.Children.Add(button);
            button2.SetValue(Button.TextProperty, "Save");
            button2.Clicked += this.onClicked;
            stackLayout.Children.Add(button2);
            viewCell.View = stackLayout;
            tableSection.Add(viewCell);
            tableRoot.Add(tableSection);
            tableView.Root = tableRoot;
            this.SetValue(ContentPage.ContentProperty, tableView);
        }

        // Token: 0x0600008C RID: 140 RVA: 0x00005D45 File Offset: 0x00003F45
       

        // Token: 0x04000052 RID: 82
        

        // Token: 0x04000054 RID: 84
        [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private TableSection TSection;

        // Token: 0x04000055 RID: 85
        [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private Label NameDisplay;*/
    }
}
