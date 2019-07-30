using MainCRMV2.Pages.Customers;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Internals;

namespace MainCRMV2.Pages.Customers
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
                    List<View> list = new List<View>() { dataPair.Index, dataPair.Value };
                    int[] space=new int[]{ 2,2};
                    bool[] box = new bool[] { true,true};
                    GridFiller.rapidFillSpacedPremadeObjects(list, mainGrid,space, box);
                    this.entryDict.Add(dataPair);
                }
            }
            this.populateFileList();
        }
        public void populateFileList()
        {
            string[] customerFileList = DatabaseFunctions.getCustomerFileList(this.NameDisplay.Text);
            foreach (string text in customerFileList)
            {
                if ((text != "." || text != "..") && customerFileList.Length > 1)
                {
                    SecurityButton dataButton = new SecurityButton(this.NameDisplay.Text + "/" + text,new string[]{ "Employee"})
                    {
                        Text = text,
                        FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        HorizontalOptions = LayoutOptions.CenterAndExpand
                    };
                    dataButton.Clicked += this.onFileButton;
                    GridFiller.rapidFillPremadeObjects(new List<View>() { dataButton }, mainGrid,new bool[]{ false});
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
            GridFiller.rapidFillPremadeObjects(new List<View>() { dataPair.Index,dataPair.Value }, mainGrid, new bool[] { true,true});
            this.entryDict.Add(dataPair);
        }
        public void onFileButton(object sender, EventArgs e)
        {

        }
        public void onClickCDR(object sender,EventArgs e)
        {
            App.MDP.Detail.Navigation.PushAsync(new CDR_Page(false,customer+""));
        }
        public void onBooking(object sender,EventArgs e)
        {
            App.MDP.Detail.Navigation.PushAsync(new Booking_Page(customer));

        }
    }
}
