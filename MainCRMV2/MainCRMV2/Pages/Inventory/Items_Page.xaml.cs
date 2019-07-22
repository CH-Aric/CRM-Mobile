using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MainCRMV2.Pages.Inventory
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Items_Page : ContentPage
    {
        public int AuditID=0;
        private Dictionary<string, List<string>> dict;
        public Items_Page()
        {
            InitializeComponent();
            loadItems();
        }
        public void loadItems()
        {
            string sql = "SELECT SUM(stock.Quantity) Total,items.Description,items.Price,items.IDKey,items.Category,items.Units FROM stock LEFT JOIN items ON items.IDKey=stock.ItemID GROUP BY items.IDKey;";
            TaskCallback call = populateTable;
            DatabaseFunctions.SendToPhp(false, sql, call);
            string sql2 = "SELECT Name,IDKey FROM auditlist";
            TaskCallback call2 = popupatePicker;
            DatabaseFunctions.SendToPhp(false, sql2, call2);
        }
        public void popupatePicker(string result)
        {
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            AuditPicker.ItemsSource = dictionary["Name"];
        }
        public void populateTable(string result)
        {
            string[] input = FormatFunctions.SplitToPairs(result);
            dict = FormatFunctions.createValuePairs(input);
            if (dict.Count > 0)
            {
                for (int i = 0; i < dict["Description"].Count; i++)
                {
                    string[] s = new string[] { dict["Description"][i], dict["Price"][i], dict["Total"][i] };
                    GridFiller.rapidFill(s,BodyGrid);
                }
            }
        }
        public void populateTableWithAudit(string result)
        {
            string[] input = FormatFunctions.SplitToPairs(result);
            dict = FormatFunctions.createValuePairs(input);
            if (dict.Count > 0)
            {
                string[] total = dict["Total"].ToArray();
                int[] idk = Array.ConvertAll<string, int>(dict["IDKey"].ToArray(),int.Parse);
                GridFiller.rapidAppendData(total, idk, BodyGrid);
            }
        }
        public void onClickedViewAudit(object sender, EventArgs e)
        {
            string sql = "SELECT SUM(audits.Quantity) Total,items.Description,items.Price,items.IDKey FROM audits LEFT JOIN items ON items.IDKey=audits.ItemID  INNER JOIN auditlist ON audits.AuditID=auditlist.IDKey WHERE auditlist.Name='"+AuditPicker.SelectedItem+"' GROUP BY audits.IDKey;";
            TaskCallback call = populateTableWithAudit;
            DatabaseFunctions.SendToPhp(false,sql,call);
        }
    }
}