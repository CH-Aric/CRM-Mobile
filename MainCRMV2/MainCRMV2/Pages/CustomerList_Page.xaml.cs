using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace MainCRMV2.Pages
{
    public partial class CustomerList_Page : ContentPage
    {
        private List<ViewCell> views;
        public CustomerList_Page()
        {
            this.InitializeComponent();
            this.loadFromDatabase();
        }
        public void loadFromDatabase()
        {
            TaskCallback call = new TaskCallback(this.populateList);
            DatabaseFunctions.SendToPhp(false, "SELECT cusindex.Name,cusindex.IDKey,cusfields.Value,cusfields.Index FROM cusindex INNER JOIN cusfields ON cusindex.IDKey=cusfields.CusID WHERE cusfields.Index LIKE '%Address%' OR cusfields.Index LIKE '%Phone%';", call);
        }
        public void populateList(string result)
        {
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            Dictionary<string, DataButton> dictionary2 = new Dictionary<string, DataButton>();
            this.views = new List<ViewCell>();
            if (dictionary.Count > 0)
            {
                for (int i = 0; i < dictionary["Name"].Count; i++)
                {
                    if (!dictionary2.ContainsKey(dictionary["IDKey"][i]))
                    {
                        string text = dictionary["Name"][i] + ": " + dictionary["Value"][i];
                        DataButton dataButton = new DataButton(int.Parse(dictionary["IDKey"][i]))
                        {
                            Text = text,
                            FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                            VerticalOptions = LayoutOptions.CenterAndExpand,
                            HorizontalOptions = LayoutOptions.CenterAndExpand
                        };
                        dataButton.Clicked += this.onClicked;
                        ViewCell viewCell = new ViewCell();
                        StackLayout stackLayout = new StackLayout
                        {
                            Orientation = StackOrientation.Horizontal,
                            HorizontalOptions = LayoutOptions.Start
                        };
                        stackLayout.Children.Add(dataButton);
                        viewCell.View = stackLayout;
                        this.TSection.Add(viewCell);
                        this.views.Add(viewCell);
                        dictionary2.Add(dictionary["IDKey"][i], dataButton);
                    }
                    else
                    {
                        DataButton dataButton2 = dictionary2[dictionary["IDKey"][i]];
                        dataButton2.Text = dataButton2.Text + ", " + dictionary["Value"][i];
                    }
                }
            }
        }
        public void onClicked(object sender, EventArgs e)
        {
            DataButton dataButton = (DataButton)sender;
            App.MDP.Detail.Navigation.PushAsync(new CustomerDetail_Page(dataButton.Integer));
            App.MDP.IsPresented = false;
        }
        public void onClickedSearch(object sender, EventArgs e)
        {
            string text = "%" + SearchEntry.Text + "%";
            string statement = string.Concat(new string[]
            {
                "SELECT DISTINCT cusindex.IDKey FROM cusindex INNER JOIN cusfields ON cusindex.IDKey=cusfields.CusID WHERE cusfields.Value LIKE '%",
                text,
                "%' OR cusindex.Name LIKE '%",
                text,
                "%';"
            });
            TaskCallback call = new TaskCallback(this.PerformSearch);
            DatabaseFunctions.SendToPhp(false, statement, call);
        }
        public void PerformSearch(string result)
        {
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            TaskCallback call = new TaskCallback(this.populateList);
            string text = "SELECT cusindex.Name,cusindex.IDKey,cusfields.Value,cusfields.Index FROM cusindex INNER JOIN cusfields ON cusindex.IDKey=cusfields.CusID WHERE (cusfields.Index LIKE '%Address%' OR cusfields.Index LIKE '%Phone%') AND (";
            foreach (string str in dictionary["IDKey"])
            {
                text = text + " cusindex.IDKey='" + str + "' OR";
            }
            text += " cusindex.IDKey='0');";
            this.PurgeCells();
            DatabaseFunctions.SendToPhp(false, text, call);
        }
        public void PurgeCells()
        {
            foreach (ViewCell item in this.views)
            {
                this.TSection.Remove(item);
            }
        }
    }
}
