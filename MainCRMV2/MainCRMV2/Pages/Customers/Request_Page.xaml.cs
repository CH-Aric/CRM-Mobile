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
                    if (dictionary["Index"][i].Contains("ook")&& dictionary["Value"][i]!="")
                    {
                        datePicker.Date = DateTime.Parse(FormatFunctions.PrettyDate(dictionary["Value"][i]));
                    }
                    else if (dictionary["Index"][i].Contains("otes"))
                    {
                        noteLabel.Text += dictionary["Value"][i];
                    }
                    else if (dictionary["Index"][i].Contains("dress"))
                    {
                        address = dictionary["Value"][i];
                        addressLabel.Text = dictionary["Value"][i];
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
                        if (dictionary["Index"][i].Contains("hone"))
                        {
                            phoneLabel.Text = dictionary["Value"][i];
                        }
                    }
                }
            }
            //populateFileList();
        }
        //TODO Add Populate File list here
        public void onClicked(object sender, EventArgs e)
        {
            List<string> batch = new List<string>();
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
            string sql = "UPDATE cusfields SET cusfields.value='" + FormatFunctions.CleanDateNew(noteLabel.Text) + "' WHERE cusfields.Index LIKE'%otes%' AND CusID= '" + customer + "'";
            batch.Add(sql);
            string sql2 = "UPDATE cusindex SET Name='" + FormatFunctions.CleanDateNew(nameLabel.Text) + "' WHERE IDKey= '" + customer + "'";
            batch.Add(sql2);
            string sql3 = "UPDATE cusfields SET cusfields.value='" + FormatFunctions.CleanDateNew(addressLabel.Text) + "' WHERE cusfields.Index LIKE '%dress%' AND CusID= '" + customer + "'";
            batch.Add(sql3);
            string sql4 = "UPDATE cusfields SET cusfields.value='" + FormatFunctions.CleanDateNew(phoneLabel.Text) + "' WHERE cusfields.Index LIKE '%hone%' AND CusID= '" + customer + "'";
            batch.Add(sql4);
            string sql5 = "UPDATE cusfields SET cusfields.value='" + FormatFunctions.CleanDateNew(DateTime.Now.ToString("yyyy/M/d h:mm:ss")) + "' WHERE cusfields.Index LIKE '%odified On%' AND CusID= '" + customer + "'";//'Modified On','" + FormatFunctions.CleanDateNew(DateTime.Now.ToString("yyyy/M/d h:mm:ss")) + "'
            batch.Add(sql5);
            DatabaseFunctions.SendBatchToPHP(batch);
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