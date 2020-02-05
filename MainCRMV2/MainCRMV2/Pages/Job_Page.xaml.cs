using System;
using System.Collections.Generic;
using MainCRMV2.Pages.Customers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MainCRMV2.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Job_Page : ContentPage
    {
        int JobID;
        List<DataPair> dp;
        public Job_Page(int jobID)
        {
            InitializeComponent();
            JobID = jobID;
            string sql = "SELECT * FROM jobfields WHERE JobID='"+JobID+"'";
            TaskCallback call = populateGrid;
            DatabaseFunctions.SendToPhp(false, sql, call);
        }
        public void populateGrid(string result)
        {//TODO port to new Mobile friendly version from desktop
            dp = new List<DataPair>();     
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            if (dictionary.Count > 0)
            {
                for (int i = 0; i < dictionary["IDKey"].Count; i++)
                {
                    DataPair d = new DataPair(int.Parse(dictionary["IDKey"][i]),dictionary["Index"][i],dictionary["Value"][i]);
                    List<View> list = new List<View>() { d.Index, d.Value };
                    GridFiller.rapidFillPremadeObjects(list, MainGrid, new bool[] { true, true });
                    dp.Add(d);
                }
            }
        }
        public void onClickSave(object sender, EventArgs e)
        {
            List<string> batch = new List<string>();
            foreach (DataPair dataPair in this.dp)
            {
                if (dataPair.isNew)
                {
                    DatabaseFunctions.SendToPhp(string.Concat(new object[]
                    {
                        "INSERT INTO jobfields (jobfields.Value,jobfields.Index,JobID) VALUES('",
                        dataPair.Value.Text,
                        "','",
                        dataPair.Index.Text,
                        "','",
                        this.JobID,
                        "')"
                    }));
                    dataPair.isNew = false;
                }
                else if (dataPair.Index.Text != dataPair.Index.GetInit() || dataPair.Value.Text != dataPair.Value.GetInit())
                {
                    DatabaseFunctions.SendToPhp(string.Concat(new object[] { "UPDATE jobfields SET Value = '", dataPair.Value.Text, "',Index='", dataPair.Index.Text, "' WHERE (IDKey= '", dataPair.Index.GetInt(), "');" }));
                }
            }
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
            GridFiller.rapidFillPremadeObjects(list, MainGrid, new bool[] { true, true });
            dp.Add(dataPair);
        }
        public void onClickAdvance(object sender, EventArgs e)
        {
            App.MDP.Detail.Navigation.PopToRootAsync();
            App.MDP.Detail.Navigation.PushAsync(new Advance_Page(JobID));
        }
    }
}