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
        List<ViewCell> Views;
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
                    ViewCell vc = new ViewCell();
                    StackLayout sl = new StackLayout() { Orientation=StackOrientation.Horizontal};
                    vc.View = sl;
                    Label l1 = new Label() { Text=dictionary["Desc1"][i],HorizontalOptions=LayoutOptions.FillAndExpand};
                    Label l2 = new Label() { Text = dictionary["Desc2"][i], HorizontalOptions = LayoutOptions.FillAndExpand };
                    Label l3 = new Label() { Text = "$" + dictionary["Price"][i], HorizontalOptions = LayoutOptions.FillAndExpand };
                    Label l4 = new Label() { Text = "$" + dictionary["PriceSale"][i], HorizontalOptions = LayoutOptions.FillAndExpand };
                    if (BrandPicker.SelectedIndex == -1)
                    {
                        Label l5 = new Label() { Text = dictionary["Brand"][i], HorizontalOptions = LayoutOptions.FillAndExpand };
                        sl.Children.Add(l5);
                    }
                    if (TypePicker.SelectedIndex == -1)
                    {
                        Label l6 = new Label() { Text = dictionary["ItemType"][i], HorizontalOptions = LayoutOptions.FillAndExpand };
                        sl.Children.Add(l6);
                    }
                    sl.Children.Add(l1);
                    sl.Children.Add(l2);
                    if(BrandPicker.SelectedIndex != -1)//Print the Wholesale price only if Additives is not selected
                    {
                        if (!BrandPicker.SelectedItem.Equals("Additives"))
                        {
                            sl.Children.Add(l3);
                        }
                    }
                    sl.Children.Add(l4);
                    TSection.Add(vc);
                    Views.Add(vc);
                }
                printHeader();
            }
        }
        public void printHeader()
        {
            ViewCell vc = new ViewCell();
            StackLayout sl = new StackLayout() { Orientation = StackOrientation.Horizontal };
            vc.View = sl;
            Label l1 = new Label() { Text = "Spec 1", HorizontalOptions = LayoutOptions.FillAndExpand };
            Label l2 = new Label() { Text = "Spec 2", HorizontalOptions = LayoutOptions.FillAndExpand };
            Label l3 = new Label() { Text = "Wholesale" , HorizontalOptions = LayoutOptions.FillAndExpand };
            Label l4 = new Label() { Text = "Market" , HorizontalOptions = LayoutOptions.FillAndExpand };
            if (BrandPicker.SelectedIndex == -1)
            {
                Label l5 = new Label() { Text ="Brand", HorizontalOptions = LayoutOptions.FillAndExpand };
                sl.Children.Add(l5);
            }
            if (TypePicker.SelectedIndex == -1)
            {
                Label l6 = new Label() { Text = "ItemType", HorizontalOptions = LayoutOptions.FillAndExpand };
                sl.Children.Add(l6);
            }
            sl.Children.Add(l1);
            sl.Children.Add(l2);
            if (BrandPicker.SelectedIndex != -1)//Print the Wholesale price only if Additives is not selected
            {
                if (!BrandPicker.SelectedItem.Equals("Additives"))
                {
                    sl.Children.Add(l3);
                }
            }
            sl.Children.Add(l4);
            TSection.Add(vc);
            Views.Add(vc);
        }
        public void PurgeCells()
        {
            foreach (ViewCell item in Views)
            {
                this.TSection.Remove(item);
            }
        }
    }
}