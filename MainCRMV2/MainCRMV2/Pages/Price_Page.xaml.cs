using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MainCRMV2.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Price_Page : ContentPage
    {
        List<ViewCell> Views;//Update this to work with the Grid!
        public Price_Page()
        {
            InitializeComponent();
            string sql = "SELECT DISTINCT(Brand) FROM pricesheet";
            Views = new List<ViewCell>();
            TaskCallback call = populateBrands;
            DatabaseFunctions.SendToPhp(false, sql, call);
        }
        public void onClicked(object sender,EventArgs e)
        {
            string sql = "SELECT * FROM pricesheet WHERE Brand LIKE '%" + BrandPicker.SelectedItem + "%' AND ItemType LIKE '%"+TypePicker.SelectedItem+"%'";
            if (BrandPicker.SelectedIndex == -1)
            {
                sql += " AND Brand !='Additives'";
            }
            TaskCallback call = populateSearch;
            DatabaseFunctions.SendToPhp(false,sql,call);
        }
        public void populateBrands(string result)
        {
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            BrandPicker.ItemsSource = dictionary["Brand"];
        }
        public void populateSearch(string result)
        {
            PurgeCells();
            Views = new List<ViewCell>();
            printHeader();
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            if (dictionary.Count > 0)
            {
                for (int i = 0; i < dictionary["IDKey"].Count; i++)
                {
                    string[] s = new string[] { dictionary["Desc1"][i], dictionary["Desc2"][i], "$" + dictionary["Price"][i], "$" + dictionary["PriceSale"][i] };
                    int count = 4;
                    if (BrandPicker.SelectedIndex == -1 || !BrandPicker.SelectedItem.Equals("Additives"))
                    {
                        count++;
                        s = new string[] { dictionary["Brand"][i] ,s[0],s[1],s[2],s[3]};
                    }
                    if (TypePicker.SelectedIndex == -1)
                    {
                        count++;
                        if (count == 6)
                        {
                            s = new string[] { dictionary["ItemType"][i], s[0], s[1], s[2], s[3], s[4] };
                        }
                        else
                        {
                            s = new string[] { dictionary["ItemType"][i], s[0], s[1], s[2], s[3] };
                        }
                    }
                    int[] Spacing = new int[] { 1,1,1,1,1,1};
                    if (s.Length == 5)
                    {
                        Spacing = new int[] { 1,2,1,1,1};
                    }else if (s.Length == 4)
                    {
                        Spacing = new int[] { 2, 2, 1 ,1};
                    }
                    GridFiller.rapidFillSpaced(s,TSection,Spacing);
                }
            }
        }
        public void printHeader()
        {
            string[] s = new string[] { "Spec 1","Spec 2","Wholesale","Market"};
            if (BrandPicker.SelectedIndex == -1 || !BrandPicker.SelectedItem.Equals("Additives"))
            {
                s = new string[] {"Brand", "Spec 1", "Spec 2", "Wholesale", "Market" };
                if (TypePicker.SelectedIndex == -1)
                {
                    s = new string[] { "ItemType", "Brand", "Spec 1", "Spec 2", "Wholesale", "Market" };
                }
            }
            else if (TypePicker.SelectedIndex == -1)
            {
                s = new string[] { "ItemType", "Spec 1", "Spec 2", "Wholesale", "Market" };
            }
            int[] Spacing = new int[] { 1, 1, 1, 1, 1, 1 };
            if (s.Length == 5)
            {
                Spacing = new int[] { 1, 2, 1, 1, 1 };
            }
            else if (s.Length == 4)
            {
                Spacing = new int[] { 2, 2, 1, 1 };
            }
            GridFiller.rapidFillSpacedRowHeightLocked(s,HeaderGrid,Spacing,new int[] { 50,0});
        }
        public void PurgeCells()
        {
            var children = TSection.Children.ToList();
            foreach (var child in children.Where(child => Grid.GetRow(child) !=0))
            {
                TSection.Children.Remove(child);
            }
            while (TSection.RowDefinitions.Count > 1)
            {
                TSection.RowDefinitions.RemoveAt(0);
            }
            var childrenHead = TSection.Children.ToList();//Strips header for Rewriting
            foreach (var child in childrenHead.Where(child => Grid.GetRow(child) != 0))
            {
                HeaderGrid.Children.Remove(child);
            }
            if (HeaderGrid.RowDefinitions.Count > 1)
            {
                HeaderGrid.RowDefinitions.RemoveAt(1);
            }
        }
        
    }
}