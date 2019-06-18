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
        List<DataEntry> CountEntries;
        List<DataViewCell> cells;
        private Dictionary<string, List<string>> dict;
        public Items_Page()
        {
            InitializeComponent();
            loadItems();
        }
        public void loadItems()
        {
            string sql = "SELECT SUM(stock.Quantity) Total,items.Description,items.Price,items.IDKey FROM stock LEFT JOIN items ON items.IDKey=stock.ItemID GROUP BY items.IDKey;";
            TaskCallback call = populateTable;
            DatabaseFunctions.SendToPhp(false, sql, call);
        }
        public void populateTable(string result)
        {
            string[] input = FormatFunctions.SplitToPairs(result);
            dict = FormatFunctions.createValuePairs(input);
            cells = new List<DataViewCell>();
            if (dict.Count > 0)
            {
                for (int i = 0; i < dict["Description"].Count; i++)
                {
                    DataViewCell c = new DataViewCell(int.Parse(dict["IDKey"][i]));
                    cells.Add(c);
                    Label e = new Label() { HorizontalOptions = LayoutOptions.FillAndExpand};
                    e.Text = dict["Description"][i];
                    Label e2 = new Label() { HorizontalOptions=LayoutOptions.FillAndExpand};
                    e2.Text = dict["Price"][i];
                    Label e3 = new Label() { HorizontalOptions=LayoutOptions.FillAndExpand};
                    e3.Text = dict["Total"][i];
                    StackLayout s = new StackLayout() { Orientation=StackOrientation.Horizontal};
                    c.View = s;
                    s.Children.Add(e);
                    s.Children.Add(e3);
                    s.Children.Add(e2);
                    TSection.Add(c);
                }
            }
        }
        public void populateTableWithAudit(string result)
        {
            string[] input = FormatFunctions.SplitToPairs(result);
            dict = FormatFunctions.createValuePairs(input);
            if (dict.Count > 0)
            {
                PurgeCells();
                for (int i = 0; i < dict["Description"].Count; i++)
                {
                    DataViewCell c = new DataViewCell(int.Parse(dict["IDKey"][i]));
                    cells.Add(c);
                    Label e = new Label() { HorizontalOptions = LayoutOptions.FillAndExpand };
                    e.Text = dict["Description"][i];
                    Entry e2 = new Entry() { HorizontalOptions = LayoutOptions.FillAndExpand };
                    e2.Placeholder = dict["Price"][i];
                    Label e3 = new Label() { HorizontalOptions = LayoutOptions.FillAndExpand };
                    e3.Text = dict["Total"][i];
                    StackLayout s = new StackLayout() { Orientation = StackOrientation.Horizontal };
                    c.View = s;
                    s.Children.Add(e);
                    s.Children.Add(e3);
                    s.Children.Add(e2);
                    TSection.Add(c);
                }
            }
        }
        public void onClickedViewAudit(object sender, EventArgs e)
        {
            string sql = "SELECT SUM(audits.Quantity) total,items.Description,items.Price FROM audits LEFT JOIN items ON items.IDKey=stock.ItemID GROUP BY items.IDKey;";
            TaskCallback call = populateTableWithAudit;
            DatabaseFunctions.SendToPhp(false,sql,call);
        }
        public void PurgeCells()
        {
            foreach (DataViewCell item in cells)
            {
                this.TSection.Remove(item);
            }
            cells = new List<DataViewCell>();
        }
    }
}