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
    public partial class Install_Page : ContentPage
    {
        private List<DataPair> entryDict, entryDictQ;
        string address;
        private int customer;
        private int stage;
        List<string> prices;
        List<string> salesmen;
        public Install_Page(int customerIn, int stageIn)
        {
            customer = customerIn;
            stage = stageIn;
            InitializeComponent();
            searchCustomers();
            //populateFileList();
            fillPriceGuideComboBox();
        }
        public void searchCustomers()
        {
            TaskCallback call2 = populatePage;
            DatabaseFunctions.SendToPhp(false, "SELECT cusindex.Name,cusfields.IDKey AS FID,cusindex.IDKey,cusfields.Value,cusfields.Index FROM cusfields INNER JOIN cusindex ON cusfields.cusID=cusindex.IDKey WHERE cusfields.CusID='" + customer + "' AND cusfields.Index <> 'QUOTEFIELD';", call2);
            TaskCallback call3 = populateQuoteList;
            DatabaseFunctions.SendToPhp(false, "SELECT cusfields.IDKey,cusfields.Value,cusfields.AdvValue FROM cusfields WHERE cusfields.CusID='" + customer + "' AND cusfields.Index='INVOICEFIELD';", call3);
        }
        public void onClickAdvance(object sender, EventArgs e)
        {

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
                        phoneLabel.Text = FormatFunctions.PrettyDate(dictionary["Value"][i]);
                    }
                    else if (dictionary["Index"][i].Contains("alesMan"))
                    {
                        SalemanCombo.SelectedIndex = DatabaseFunctions.findIndexInList(salesmen, dictionary["Value"][i]);//TODO UPDATE with proper index checking
                    }
                    else if (dictionary["Index"][i].Contains("uoteTotal"))
                    {
                        QuoteTotal.Text = FormatFunctions.PrettyDate(dictionary["Value"][i]);
                    }
                    else if (dictionary["Index"][i].Contains("ontactDate"))
                    {
                        contactLabel.Text = FormatFunctions.PrettyDate(dictionary["Value"][i]);
                    }
                    else
                    {
                        DataPair dataPair = new DataPair(int.Parse(dictionary["FID"][i]), dictionary["Index"][i], dictionary["Value"][i]);
                        dataPair.Value.Text = FormatFunctions.PrettyDate(dictionary["Value"][i]);
                        dataPair.Value.Placeholder = "Value here";
                        dataPair.Index.Text = FormatFunctions.PrettyDate(dictionary["Index"][i]); ;
                        dataPair.Index.Placeholder = "Index here";
                        List<View> list = new List<View>() { dataPair.Index, dataPair.Value };
                        int[] j = new int[] { 2, 4 };
                        GridFiller.rapidFillSpacedPremadeObjects(list, bottomStack, j, new bool[] { true, true });
                        entryDict.Add(dataPair);
                        if (dictionary["Index"][i].Contains("dress"))
                        {
                            address = FormatFunctions.PrettyDate(dictionary["Value"][i]);
                        }
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
                    SecurityButton dataButton = new SecurityButton(nameLabel.Text + "/" + text, new string[] { "Sales" })
                    {
                        Text = text
                    };
                    dataButton.Clicked += onFileButton;
                    List<View> list = new List<View>() { dataButton };
                    GridFiller.rapidFillPremadeObjects(list, fileGrid, new bool[] { true, true });
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
                    dataPair.Index.Text = dictionary["AdvValue"][i];
                    dataPair.Index.Placeholder = "Amount";
                    List<View> list = new List<View>() { dataPair.Index, dataPair.Value };
                    int[] j = new int[] { 2, 4 };
                    GridFiller.rapidFillSpacedPremadeObjects(list, quoteStack, j, new bool[] { true, true });
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
                        FormatFunctions.CleanDateNew(dataPair.Value.Text),
                        "','",
                        FormatFunctions.CleanDateNew(dataPair.Index.Text),
                        "','",
                        this.customer,
                        "')"
                    }));
                    dataPair.isNew = false;
                }
                else if (!dataPair.Index.Text.Equals(dataPair.Index.GetInit()) || !dataPair.Value.Text.Equals(dataPair.Value.GetInit()))
                {
                    DatabaseFunctions.SendToPhp(string.Concat(new object[] { "UPDATE cusfields SET Value = '", FormatFunctions.CleanDateNew(dataPair.Value.Text), "',Index='", FormatFunctions.CleanDateNew(dataPair.Index.Text), "' WHERE (IDKey= '", dataPair.Index.GetInt(), "');" }));
                }
            }
            string sql = "DELETE FROM cusfields WHERE CusID='" + customer + "' AND cusfields.Index='INVOICEFIELD'";
            DatabaseFunctions.SendToPhp(sql);
            foreach (DataPair dp in entryDictQ)
            {
                if (dp.Value.Text != "" && dp.Index.Text != "")
                {
                    string sql2 = "INSERT INTO cusfields(cusfields.Value,cusfields.Index,CusID,cusfields.AdvValue) VALUES ('" + FormatFunctions.CleanDateNew(dp.Value.Text) + "','INVOICEFIELD','" + customer + "','" + FormatFunctions.CleanDateNew(dp.Index.Text) + "')";
                    DatabaseFunctions.SendToPhp(sql2);
                }
            }

            List<string> batch = new List<string>();
            string sql5 = "UPDATE cusfields SET cusfields.value='" + FormatFunctions.CleanDateNew(DateTime.Now.ToString("yyyy/M/d h:mm:ss")) + "' WHERE cusfields.Index LIKE '%odified On%' AND CusID= '" + customer + "'";//'Modified On','" + FormatFunctions.CleanDateNew(DateTime.Now.ToString("yyyy/M/d h:mm:ss")) + "'
            batch.Add(sql5);
            string sql3 = "UPDATE cusfields SET cusfields.value='" + FormatFunctions.CleanDateNew(contactLabel.Text) + "' WHERE cusfields.Index LIKE '%ookin%' AND CusID= '" + customer + "'";
            batch.Add(sql3);
            string sql4 = "UPDATE cusfields SET cusfields.value='" + FormatFunctions.CleanDateNew(phoneLabel.Text) + "' WHERE cusfields.Index LIKE '%hone%' AND CusID= '" + customer + "'";
            batch.Add(sql4);
            string sql6 = "UPDATE cusfields SET cusfields.value='" + salesmen[SalemanCombo.SelectedIndex] + "' WHERE cusfields.Index LIKE '%alesman%' AND CusID= '" + customer + "'";
            batch.Add(sql6);
            string sql7 = "UPDATE cusfields SET cusfields.value='" + FormatFunctions.CleanDateNew(DateTime.Now.ToString("yyyy/M/d h:mm:ss")) + "' WHERE cusfields.Index LIKE '%odified On%' AND CusID= '" + customer + "'";//'Modified On','" + FormatFunctions.CleanDateNew(DateTime.Now.ToString("yyyy/M/d h:mm:ss")) + "'
            batch.Add(sql7);
            DatabaseFunctions.SendBatchToPHP(batch);
            App.MDP.Detail.Navigation.PushAsync(new Install_Page(customer, stage));
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
            int[] i = new int[] { 2, 4 };
            GridFiller.rapidFillSpacedPremadeObjects(list, bottomStack, i, new bool[] { true, true });
            entryDictQ.Add(dataPair);
            entryDict.Add(dataPair);
            Button x = (Button)sender;
            if (x != row)
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
            dataPair.Index.Text = "";
            dataPair.Index.Placeholder = "Item";
            dataPair.Value.Text = "";
            dataPair.Value.Placeholder = "Amount";
            List<View> list = new List<View>() { dataPair.Index, dataPair.Value };
            int[] i = new int[] { 3, 3 };
            GridFiller.rapidFillSpacedPremadeObjects(list, quoteStack, i, new bool[] { true, true });
            entryDictQ.Add(dataPair);
        }
        public void onClickAddPrefilledFieldsQ(object sender, EventArgs e)
        {
            DataPair dataPair = new DataPair(0, "", "");
            dataPair.setNew();
            dataPair.Index.Text = PriceGuidecombo.SelectedItem.ToString();
            dataPair.Index.Placeholder = "Item";
            dataPair.Value.Text = prices[PriceGuidecombo.SelectedIndex];
            dataPair.Value.Placeholder = "Amount";
            List<View> list = new List<View>() { dataPair.Index, dataPair.Value };
            int[] i = new int[] { 3, 3 };
            GridFiller.rapidFillSpacedPremadeObjects(list, quoteStack, i, new bool[] { true, true });
            entryDictQ.Add(dataPair);
        }
        public void fillPriceGuideComboBox()
        {
            string sql = "SELECT PriceSale, concat(Brand, '/ ',ItemType,'/ ',Desc1,'/ ',Desc2) as v FROM crm2.pricesheet;";//Make this readable in some way
            TaskCallback call2 = populateCombo;
            DatabaseFunctions.SendToPhp(false, sql, call2);
            string sql2 = "SELECT agents.FName,agents.IDKey FROM agents INNER JOIN agentroles ON agents.IDKey=agentroles.AgentID AND AgentRole='1'";
            TaskCallback call3 = populateSalesCombo;
            DatabaseFunctions.SendToPhp(false, sql2, call3);
        }
        public void populateCombo(string result)
        {
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            PriceGuidecombo.ItemsSource = dictionary["v"];
            prices = dictionary["PriceSale"];
        }
        public void populateSalesCombo(string result)
        {
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            SalemanCombo.ItemsSource = dictionary["FName"];
            salesmen = dictionary["IDKey"];
        }
        public void onFileButton(object sender, EventArgs e)
        {
            SecurityButton sb = (SecurityButton)sender;
            App.MDP.Detail.Navigation.PushAsync(new FileDisplay(sb.Text, customer));
        }
        public void onClickCDR(object sender, EventArgs e)
        {
            App.MDP.Detail.Navigation.PushAsync(new CDR_Page(false, customer + ""));
        }
    }
}