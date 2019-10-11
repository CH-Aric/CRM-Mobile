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
    public partial class Quote_Page : ContentPage
    {
        private List<DataPair> entryDict;
         private List<FlaggedDataPair> entryDictQ;
        string address;
        private int customer;
        private int stage;
        int currentQuote = 0;
        List<string> prices;
        List<string> salesmen;
        public Quote_Page(int customerIn,int stageIn)
        {
            customer = customerIn;
            stage = stageIn;
            InitializeComponent();
            searchCustomers();
            fillPriceGuideComboBox();
        }
        public void searchCustomers()
        {
            TaskCallback call2 = populatePage;
            DatabaseFunctions.SendToPhp(false, "SELECT cusindex.Name,cusfields.IDKey AS FID,cusindex.IDKey,cusfields.Value,cusfields.Index FROM cusfields INNER JOIN cusindex ON cusfields.cusID=cusindex.IDKey WHERE cusfields.CusID='" + customer + "' AND cusfields.Index <> 'QUOTEFIELD';", call2);
            TaskCallback call3 = populateQuoteList;
            DatabaseFunctions.SendToPhp(false,"SELECT cusfields.IDKey,cusfields.Value,cusfields.AdvValue,TaskID FROM cusfields WHERE cusfields.CusID='"+customer+"' AND cusfields.Index='QUOTEFIELD';",call3);
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
            nameLabel.Text = dictionary["Name"][0];
            if (dictionary.Count > 0)
            {
                for (int i = 0; i < dictionary["Index"].Count; i++)
                {
                    if (dictionary["Index"][i].Contains("dress"))
                    {
                        address = dictionary["Value"][i];
                        contactLabel.Text = dictionary["Value"][i];
                    }
                    else
                    {
                        DataPair dataPair = new DataPair(int.Parse(dictionary["FID"][i]), dictionary["Index"][i], dictionary["Value"][i]);
                        dataPair.Value.Text = dictionary["Value"][i];
                        dataPair.Value.Placeholder = "Value here";
                        dataPair.Value.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
                        dataPair.Index.Text = dictionary["Index"][i];
                        dataPair.Index.Placeholder = "Index here";
                        dataPair.Index.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
                        List<View> list = new List<View>() { dataPair.Index,dataPair.Value};
                        GridFiller.rapidFillPremadeObjects(list,BodyGrid,new bool[]{ true,true});
                        entryDict.Add(dataPair);
                        if (dictionary["Index"][i].Contains("hone"))
                        {
                            phoneLabel.Text = dictionary["Value"][i];
                        }
                    }
                }
            }
            FileList fl = new FileList(customer);
            GridHolder.Children.Add(fl);
        }
        public void populateQuoteList(string result)
        {
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            entryDictQ = new List<FlaggedDataPair>();
            if (dictionary.Count > 0)
            {
                for (int i = 0; i < dictionary["Value"].Count; i++)
                {
                    FlaggedDataPair dataPair = new FlaggedDataPair(0, dictionary["Value"][i], dictionary["AdvValue"][i],int.Parse(dictionary["TaskID"][i]));
                    dataPair.Value.Text = dictionary["Value"][i];
                    dataPair.Value.Placeholder = "Item";
                    dataPair.Index.Text = dictionary["AdvValue"][i];
                    dataPair.Index.Placeholder = "Amount";
                    List<View> list = new List<View>() { dataPair.Index,dataPair.Value};
                    if (dictionary["TaskID"][i] == "0")
                    {
                        GridFiller.rapidFillPremadeObjects(list, QuoteGrid1, new bool[] { true, true });
                    }
                    else if (dictionary["TaskID"][i] == "1")
                    {
                        GridFiller.rapidFillPremadeObjects(list, QuoteGrid2, new bool[] { true, true });
                    }
                    else if (dictionary["TaskID"][i] == "2")
                    {
                        GridFiller.rapidFillPremadeObjects(list, QuoteGrid3, new bool[] { true, true });
                    }
                    else if (dictionary["TaskID"][i] == "3")
                    {
                        GridFiller.rapidFillPremadeObjects(list, QuoteGrid4, new bool[] { true, true });
                    }
                    else if (dictionary["TaskID"][i] == "4")
                    {
                        GridFiller.rapidFillPremadeObjects(list, QuoteGrid5, new bool[] { true, true });
                    }
                    entryDictQ.Add(dataPair);
                }
            }
        }
        public void fillPriceGuideComboBox()
        {
            string sql = "SELECT PriceSale, concat(Brand, '/ ',ItemType,'/ ',Desc1,'/ ',Desc2) as v FROM crm2.pricesheet;";//Make this readable in some way
            TaskCallback call2 = populateCombo;
            DatabaseFunctions.SendToPhp(false, sql, call2);
            string sql2 = "SELECT agents.FName,agents.IDKey FROM agents INNER JOIN agentroles ON agents.IDKey=agentroles.AgentID AND AgentRole='0'";
            TaskCallback call3 = populateSalesCombo;
            DatabaseFunctions.SendToPhp(false, sql2, call3);
        }
        public void populateCombo(string result)
        {
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            PriceGuide.ItemsSource = dictionary["v"];
            prices = dictionary["PriceSale"];
        }
        public void populateSalesCombo(string result)
        {
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            SalemanCombo.ItemsSource = dictionary["FName"];
            salesmen = dictionary["IDKey"];
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
                    DatabaseFunctions.SendToPhp(string.Concat(new object[] { "UPDATE cusfields SET cusfields.Value = '", FormatFunctions.CleanDateNew(dataPair.Value.Text), "',cusfields.Index='", FormatFunctions.CleanDateNew(dataPair.Index.Text), "' WHERE (IDKey= '", dataPair.Index.GetInt(), "');" }));
                }
            }
            string sql = "DELETE FROM cusfields WHERE CusID='" + customer + "' AND cusfields.Index='QUOTEFIELD'";
            DatabaseFunctions.SendToPhp(sql);
            foreach (FlaggedDataPair dp in entryDictQ)
            {
                if (dp.Value.Text != "" || dp.Index.Text != "")
                {
                    string sql2 = "INSERT INTO cusfields(cusfields.Value,cusfields.Index,CusID,cusfields.AdvValue,TaskID) VALUES ('" + FormatFunctions.CleanDateNew(dp.Value.Text) + "','QUOTEFIELD','" + customer + "','" + FormatFunctions.CleanDateNew(dp.Index.Text) + "','" + dp.Flag + "')";
                    DatabaseFunctions.SendToPhp(sql2);
                }
            }
            List<string> batch = new List<string>();
            string sql5 = "UPDATE cusindex SET Name='" + FormatFunctions.CleanDateNew(nameLabel.Text) + "' WHERE IDKey= '" + customer + "'";
            batch.Add(sql5);
            string sql3 = "UPDATE cusfields SET cusfields.value='" + FormatFunctions.CleanDateNew(contactLabel.Text) + "' WHERE cusfields.Index LIKE '%ookin%' AND CusID= '" + customer + "'";
            batch.Add(sql3);
            string sql4 = "UPDATE cusfields SET cusfields.value='" + FormatFunctions.CleanPhone(phoneLabel.Text) + "' WHERE cusfields.Index LIKE '%hone%' AND CusID= '" + customer + "'";
            batch.Add(sql4);
            string sql6 = "UPDATE cusfields SET cusfields.value='" + salesmen[SalemanCombo.SelectedIndex] + "' WHERE cusfields.Index LIKE '%alesman%' AND CusID= '" + customer + "'";
            batch.Add(sql6);
            string sql7 = "UPDATE cusfields SET cusfields.value='" + FormatFunctions.CleanDateNew(DateTime.Now.ToString("yyyy/M/d h:mm:ss")) + "' WHERE cusfields.Index LIKE '%odified On%' AND CusID= '" + customer + "'";//'Modified On','" + FormatFunctions.CleanDateNew(DateTime.Now.ToString("yyyy/M/d h:mm:ss")) + "'
            batch.Add(sql7);
            DatabaseFunctions.SendBatchToPHP(batch);
            App.MDP.Detail.Navigation.PopToRootAsync();
            App.MDP.Detail.Navigation.PushAsync(new Quote_Page(customer,stage));
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
            GridFiller.rapidFillPremadeObjects(list, BodyGrid, new bool[] { true, true });
            entryDict.Add(dataPair);
            entryDict.Add(dataPair);
            Button x = (Button)sender;
        }
        public void onClickAddFieldsQ(object sender, EventArgs e)
        {
            FlaggedDataPair dataPair = new FlaggedDataPair(0, "", "",currentQuote);
            dataPair.setNew();
            dataPair.Value.Text = "";
            dataPair.Value.Placeholder = "Item";
            dataPair.Index.Text = "";
            dataPair.Index.Placeholder = "Amount";
            List<View> list= new List<View>() { dataPair.Index,dataPair.Value};
            if (currentQuote == 0)
            {
                GridFiller.rapidFillPremadeObjects(list,QuoteGrid1,new bool[] { true,true});
            }
            else if (currentQuote == 1)
            {
                GridFiller.rapidFillPremadeObjects(list, QuoteGrid2, new bool[] { true, true });
            }
            else if (currentQuote == 2)
            {
                GridFiller.rapidFillPremadeObjects(list, QuoteGrid3, new bool[] { true, true });
            }
            else if (currentQuote == 3)
            {
                GridFiller.rapidFillPremadeObjects(list, QuoteGrid4, new bool[] { true, true });
            }
            else if (currentQuote == 4)
            {
                GridFiller.rapidFillPremadeObjects(list, QuoteGrid5, new bool[] { true, true });
            }
            entryDictQ.Add(dataPair);
        }
        public void onClickAddPrefilledFieldsQ(object sender, EventArgs e)
        {
            FlaggedDataPair dataPair = new FlaggedDataPair(0, "", "", currentQuote);
            dataPair.setNew();
            dataPair.Index.Text = PriceGuide.SelectedItem.ToString();
            dataPair.Value.Text = prices[PriceGuide.SelectedIndex];
            List<View> list = new List<View>() { dataPair.Index, dataPair.Value };
            if (currentQuote == 0)
            {
                GridFiller.rapidFillPremadeObjects(list, QuoteGrid1, new bool[] { true, true });
            }
            else if (currentQuote == 1)
            {
                GridFiller.rapidFillPremadeObjects(list, QuoteGrid2, new bool[] { true, true });
            }
            else if (currentQuote == 2)
            {
                GridFiller.rapidFillPremadeObjects(list, QuoteGrid3, new bool[] { true, true });
            }
            else if (currentQuote == 3)
            {
                GridFiller.rapidFillPremadeObjects(list, QuoteGrid4, new bool[] { true, true });
            }
            else if (currentQuote == 4)
            {
                GridFiller.rapidFillPremadeObjects(list, QuoteGrid5, new bool[] { true, true });
            }
            entryDictQ.Add(dataPair);
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
        public void onClickedFiles(object sender, EventArgs e)
        {
            App.MDP.Detail.Navigation.PushAsync(new FileUpload(customer));
        }
        public void onClickedChangeQuoteRender(object sender,EventArgs e)
        {
            derenderQutoeGrids();
            if (sender == Quote1)
            {
                QuoteGrid1.IsVisible = true;
                currentQuote = 0;
            }
            else if (sender == Quote2)
            {
                QuoteGrid2.IsVisible = true;
                currentQuote = 1;
            }
            else if (sender == Quote3)
            {
                QuoteGrid3.IsVisible = true;
                currentQuote = 2;
            }
            else if (sender == Quote4)
            {
                QuoteGrid4.IsVisible = true;
                currentQuote = 3;
            }
            else if (sender == Quote5)
            {
                QuoteGrid5.IsVisible = true;
                currentQuote = 4;
            }
        }
        public void derenderQutoeGrids()
        {
            QuoteGrid1.IsVisible = false;
            QuoteGrid2.IsVisible = false;
            QuoteGrid3.IsVisible = false;
            QuoteGrid4.IsVisible = false;
            QuoteGrid5.IsVisible = false;
        }
    }
}