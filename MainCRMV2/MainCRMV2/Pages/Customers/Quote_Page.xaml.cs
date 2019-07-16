using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MainCRMV2.Pages.Customers
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Quote_Page : ContentPage
    {
        private List<DataPair> entryDict,entryDictQ;
        string address;
        private int customer;
        private int stage;
        public Quote_Page(int customerIn,int stageIn)
        {
            customer = customerIn;
            stage = stageIn;
            InitializeComponent();
            searchCustomers();
            populateFileList();
        }
        public void searchCustomers()
        {
            TaskCallback call2 = populatePage;
            DatabaseFunctions.SendToPhp(false, "SELECT cusindex.Name,cusfields.IDKey AS FID,cusindex.IDKey,cusfields.Value,cusfields.Index FROM cusfields INNER JOIN cusindex ON cusfields.cusID=cusindex.IDKey WHERE cusfields.CusID='" + customer + "' AND cusfields.Index <> 'QUOTEFIELD';", call2);
            TaskCallback call3 = populateQuoteList;
            DatabaseFunctions.SendToPhp(false,"SELECT cusfields.IDKey,cusfields.Value,cusfields.AdvValue FROM cusfields WHERE cusfields.CusID='"+customer+"' AND cusfields.Index='QUOTEFIELD';",call3);
        }
        public void onClickAdvance(object sender, EventArgs e)
        {
            //App.MDP.Detail.Navigation.PopToRootAsync();
            App.MDP.Detail.Navigation.PushAsync(new Advance_Page(customer));
        }
        public void populatePage(string result)
        {
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            entryDict = new List<DataPair>();
            if (dictionary.Count > 0)
            {
                for (int i = 0; i < dictionary["Index"].Count; i++)
                {
                    if (dictionary["Index"][i].Contains("hone"))
                    {
                        phoneLabel.Text = dictionary["Value"][i];
                    }
                    else
                    {
                        DataPair dataPair = new DataPair(int.Parse(dictionary["FID"][i]), dictionary["Index"][i], dictionary["Value"][i]);
                        dataPair.Value.Text = dictionary["Value"][i];
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
                        TSection.Add(viewCell);
                        entryDict.Add(dataPair);
                    }
                }
            }
        }
        public void populateFileList()
        {
            string[] customerFileList = DatabaseFunctions.getCustomerFileList(nameLabel.Text);
            foreach (string text in customerFileList)
            {
                if ((text != "." || text != "..") && customerFileList.Length > 1)
                {
                    DataButton dataButton = new DataButton(nameLabel.Text + "/" + text)
                    {
                        Text = text,
                        FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        HorizontalOptions = LayoutOptions.CenterAndExpand
                    };
                    dataButton.Clicked += onFileButton;
                    ViewCell item = new ViewCell();
                    StackLayout stackLayout = new StackLayout();
                    stackLayout.Orientation = StackOrientation.Horizontal;
                    stackLayout.Children.Add(dataButton);
                    TSection.Add(item);
                }
            }
        }
        public void populateQuoteList(string result)
        {
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            entryDictQ = new List<DataPair>();
            if (dictionary.Count > 0)
            {
                for (int i = 0; i < dictionary["Value"].Count; i++)
                {
                    DataPair dataPair = new DataPair(0, dictionary["Value"][i], dictionary["AdvValue"][i]);
                    dataPair.Value.Text = dictionary["Value"][i];
                    dataPair.Value.Placeholder = "Item";
                    dataPair.Value.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
                    dataPair.Value.VerticalOptions = LayoutOptions.CenterAndExpand;
                    dataPair.Value.HorizontalOptions = LayoutOptions.StartAndExpand;
                    dataPair.Index.Text = dictionary["AdvValue"][i];
                    dataPair.Index.Placeholder = "Amount";
                    dataPair.Index.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
                    dataPair.Index.VerticalOptions = LayoutOptions.CenterAndExpand;
                    dataPair.Index.HorizontalOptions = LayoutOptions.EndAndExpand;
                    StackLayout stackLayout = new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal
                    };
                    stackLayout.Children.Add(dataPair.Index);
                    stackLayout.Children.Add(dataPair.Value);
                    quoteStack.Children.Add(stackLayout);
                    entryDictQ.Add(dataPair);
                }
            }
        }
        public void onClicked(object sender, EventArgs e)
        {
            foreach (DataPair dataPair in entryDict)
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
                else if (!dataPair.Index.Text.Equals(dataPair.Index.GetInit()))
                {
                    DatabaseFunctions.SendToPhp(string.Concat(new object[] { "UPDATE cusfields SET Value = '", dataPair.Value.Text, "',Index='", dataPair.Index.Text, "' WHERE (IDKey= '", dataPair.Index.GetInt(), "');" }));
                }
            }
            string sql = "DELETE FROM cusfields WHERE CusID='" + customer + "' AND cusfields.Index='QUOTEFIELD'";
            DatabaseFunctions.SendToPhp(sql);
            foreach (DataPair dp in entryDictQ)
            {
                if (dp.Value.Text != "" && dp.Index.Text != "")
                {
                    string sql2 = "INSERT INTO cusfields(cusfields.Value,cusfields.Index,CusID,cusfields.AdvValue) VALUES ('" + dp.Value.Text + "','QUOTEFIELD','" + customer + "','" + dp.Index.Text + "')";
                    DatabaseFunctions.SendToPhp(sql2);
                }
            }
            App.MDP.Detail.Navigation.PopToRootAsync();
            App.MDP.Detail.Navigation.PushAsync(new Quote_Page(customer,stage));
        }
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
            TSection.Add(viewCell);
            entryDict.Add(dataPair);
            Button x = (Button)sender;
            if (x!=row)
            {
                if (x == sig)
                {
                    dataPair.Index.Text = "Signature";
                    dataPair.Value.Text = "True";
                }
                if (x == fie)
                {
                    dataPair.Index.Text = "Deposit Received";
                    dataPair.Value.Text = "True";
                }
                if (x == met)
                {
                    dataPair.Index.Text = "Payment Method";
                }
            }
        }
        public void onClickAddFieldsQ(object sender, EventArgs e)
        {
            DataPair dataPair = new DataPair(0, "", "");
            dataPair.setNew();
            dataPair.Value.Text = "";
            dataPair.Value.Placeholder = "Item";
            dataPair.Value.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
            dataPair.Value.VerticalOptions = LayoutOptions.CenterAndExpand;
            dataPair.Value.HorizontalOptions = LayoutOptions.StartAndExpand;
            dataPair.Index.Text = "";
            dataPair.Index.Placeholder = "Amount";
            dataPair.Index.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
            dataPair.Index.VerticalOptions = LayoutOptions.CenterAndExpand;
            dataPair.Index.HorizontalOptions = LayoutOptions.EndAndExpand;
            StackLayout stackLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal
            };
            stackLayout.Children.Add(dataPair.Index);
            stackLayout.Children.Add(dataPair.Value);
            quoteStack.Children.Add(stackLayout);
            entryDictQ.Add(dataPair);
        }
        public void onFileButton(object sender, EventArgs e)
        {

        }
        public void onClickCDR(object sender, EventArgs e)
        {
            App.MDP.Detail.Navigation.PushAsync(new CDR_Page(false, customer + ""));
        }
    }
}