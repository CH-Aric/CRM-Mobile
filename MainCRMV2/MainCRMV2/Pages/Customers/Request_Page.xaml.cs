using MainCRMV2.Pages.Popup_Pages;
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
            DatabaseFunctions.SendToPhp(false, "SELECT cusindex.Name,cusfields.IDKey AS FID,cusindex.IDKey,cusfields.Value,cusfields.Index FROM cusfields INNER JOIN cusindex ON cusfields.cusID=cusindex.IDKey WHERE cusfields.CusID='" + customer + "';", call2);
        }
        public void onClickAdvance(object sender,EventArgs e)
        {
            App.MDP.Detail.Navigation.PopToRootAsync();
            App.MDP.Detail.Navigation.PushAsync(new Advance_Page(customer));
        }
        public void populatePage(string result)
        {
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            entryDict = new List<DataPair>();
            if (dictionary.Count > 0)
            {
                nameLabel.Text = dictionary["Name"][0];
                for (int i = 0; i < dictionary["Index"].Count; i++)
                {
                    if (dictionary["Index"][i].Contains("hone"))
                    {
                        phoneLabel.Text = dictionary["Value"][i];
                    }
                    else if (dictionary["Index"][i].Contains("ook")&& dictionary["Value"][i]!="")
                    {
                        datePicker.Date = DateTime.Parse(FormatFunctions.PrettyDate(dictionary["Value"][i]));
                    }
                    else if (dictionary["Index"][i].Contains("otes:"))
                    {
                        address = dictionary["Value"][i];
                        noteLabel.Text += dictionary["Value"][i];
                    }
                    else
                    {
                        DataPair dataPair = new DataPair(int.Parse(dictionary["FID"][i]), dictionary["Value"][i], dictionary["Index"][i]);
                        dataPair.Value.Text = dictionary["Value"][i];
                        dataPair.Value.Placeholder = "Value here";
                        dataPair.Index.Text = dictionary["Index"][i];
                        dataPair.Index.Placeholder = "Index here";
                        List<View> list = new List<View>() { dataPair.Index, dataPair.Value};
                        GridFiller.rapidFillPremadeObjectsStandardHeight(list, bodyGrid, new bool[] { true, true },50);
                        entryDict.Add(dataPair);
                    }
                }
            }
            //populateFileList();
        }
        //TODO Add Populate File list here
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
            dataPair.Index.Text = "";
            dataPair.Index.Placeholder = "Value here";
            List<View> list = new List<View>() { dataPair.Index, dataPair.Value };
            GridFiller.rapidFillPremadeObjects(list, bodyGrid, new bool[] { true, true });
            entryDict.Add(dataPair);
        }
        public void onFileButton(object sender, EventArgs e)
        {
            SecurityButton sb = (SecurityButton)sender;
            App.MDP.Detail.Navigation.PushAsync(new FileDisplay(sb.Text,customer));
        }
        public void onClickCDR(object sender, EventArgs e)
        {
            App.MDP.Detail.Navigation.PushAsync(new CDR_Page(false, customer + ""));
        }
        public void onClickedFiles(object sender, EventArgs e)
        {
            App.MDP.Detail.Navigation.PushAsync(new FileUpload(customer));
        }
    }
}