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
    public partial class Request_Page : ContentPage
    {

        private List<DataPair> entryDict;
        string address;
        private int customer;
        public Request_Page(int customerIn)
        {
            customer = customerIn;
            InitializeComponent();
            searchCustomers();
        }
        public void searchCustomers()
        {
            TaskCallback call2 =populatePage;
            DatabaseFunctions.SendToPhp(false, "SELECT cusindex.Name,cusfields.IDKey AS FID,cusindex.IDKey,cusfields.value,cusfields.Index FROM cusfields INNER JOIN cusindex ON cusfields.cusID=cusindex.IDKey WHERE cusfields.CusID='" + customer + "';", call2);
        }
        public void onClickAdvance(object sender,EventArgs e)
        {

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
                        phoneLabel.Text = dictionary["value"][i];
                    }
                    else if (dictionary["Index"][i].Contains("ook"))
                    {
                        nameLabel.Text = "Booked For: " + dictionary["value"][i];
                    }
                    else if (dictionary["Index"][i].Contains("OTES"))
                    {
                        address = dictionary["value"][i];
                        noteLabel.Text += dictionary["value"][i];
                    }
                    else
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
                        TSection.Add(viewCell);
                        entryDict.Add(dataPair);
                    }
                }
            }
            populateFileList();
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
                    DatabaseFunctions.SendToPhp(string.Concat(new object[]{"UPDATE cusfields SET Value = '", dataPair.Value.Text,  "',Index='", dataPair.Index.Text, "' WHERE (IDKey= '", dataPair.Index.GetInt(), "');"}));
                }
            }
            string sql = "UPDATE cusfields SET cusfields.value='"+noteLabel.Text+"' WHERE cusfields.Index='Notes:'";
            DatabaseFunctions.SendToPhp(sql);
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