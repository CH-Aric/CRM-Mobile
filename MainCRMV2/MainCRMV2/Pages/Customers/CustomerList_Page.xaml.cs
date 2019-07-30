using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace MainCRMV2.Pages.Customers
{
    public partial class CustomerList_Page : ContentPage
    {
        private List<ViewCell> views;
        int stageSearch = 0;
        public CustomerList_Page()
        {
            this.InitializeComponent();
            views = new List<ViewCell>();
        }
        public void loadFromDatabase()
        {
            TaskCallback call = populateList;
            string sql = "SELECT cusindex.Name,cusindex.IDKey,cusfields.Value,cusfields.Index,cusindex.Stage FROM cusindex INNER JOIN cusfields ON cusindex.IDKey=cusfields.CusID WHERE (cusfields.Index LIKE '%Address%' OR cusfields.Index LIKE '%Phone%')";
            sql += appendPickerResult();
            DatabaseFunctions.SendToPhp(false, sql, call);
        }
        public void populateList(string result)
        {
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            Dictionary<string, SecurityButton> dictionary2 = new Dictionary<string, SecurityButton>();
            this.views = new List<ViewCell>();
            if (dictionary.Count > 0)
            {
                for (int i = 0; i < dictionary["Name"].Count; i++)
                {
                    if (!dictionary2.ContainsKey(dictionary["IDKey"][i]))
                    {
                        string text = dictionary["Name"][i] + ": " + dictionary["Value"][i];
                        SecurityButton dataButton = new SecurityButton(int.Parse(dictionary["IDKey"][i]), new string[] { "Employee" })
                        {
                            Text = text,
                            FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                            VerticalOptions = LayoutOptions.CenterAndExpand,
                            HorizontalOptions = LayoutOptions.FillAndExpand
                        };
                        dataButton.Clicked += this.onClicked;
                        dataButton.Integer2 = int.Parse(dictionary["Stage"][i]);
                        ViewCell viewCell = new ViewCell();
                        StackLayout stackLayout = new StackLayout
                        {
                            Orientation = StackOrientation.Horizontal,
                            HorizontalOptions = LayoutOptions.FillAndExpand
                        };
                        stackLayout.Children.Add(dataButton);
                        viewCell.View = stackLayout;
                        this.TSection.Add(viewCell);
                        this.views.Add(viewCell);
                        dictionary2.Add(dictionary["IDKey"][i], dataButton);
                    }
                    else
                    {
                        SecurityButton dataButton2 = dictionary2[dictionary["IDKey"][i]];
                        dataButton2.Text = dataButton2.Text + ", " + dictionary["Value"][i];
                    }
                }
            }
        }
        public void onClicked(object sender, EventArgs e)
        {
            SecurityButton dataButton = (SecurityButton)sender;
            App.MDP.IsPresented = false;
            if (dataButton.GetInt2() == 0)
            {
                App.MDP.Detail.Navigation.PushAsync(new CustomerDetail_Page(dataButton.Integer));
            }
            else if (dataButton.GetInt2() == 1)
            {
                App.MDP.Detail.Navigation.PushAsync(new Request_Page(dataButton.Integer));
            }
            else if (dataButton.GetInt2() == 2)
            {
                App.MDP.Detail.Navigation.PushAsync(new Booking_Page(dataButton.Integer));
            }
            else if (dataButton.GetInt2() > 2 && dataButton.GetInt2() < 9)
            {
                App.MDP.Detail.Navigation.PushAsync(new Quote_Page(dataButton.Integer, stageSearch));
            }
        }
        public void onClickedSearch(object sender, EventArgs e)
        {
            string text = "%" + SearchEntry.Text + "%";
            string statement = "SELECT DISTINCT cusindex.IDKey FROM cusindex INNER JOIN cusfields ON cusindex.IDKey=cusfields.CusID WHERE (cusfields.Value LIKE '" + text + "' OR cusindex.Name LIKE '" + text + "')";
            statement += appendPickerResult();
            TaskCallback call = new TaskCallback(this.PerformSearch);
            DatabaseFunctions.SendToPhp(false, statement, call);
        }
        public void onClickedCreate(object sender, EventArgs e)
        {
            string sql = "INSERT INTO cusindex (Stage) VALUES ('" + (NewPicker.SelectedIndex + 1) + "')";
            DatabaseFunctions.SendToPhp(sql);
            System.Threading.Thread.Sleep(500);
            string sql2 = "SELECT IDKey,Stage FROM cusindex ORDER BY IDKey Desc LIMIT 1";
            TaskCallback call = OpenPage;
            DatabaseFunctions.SendToPhp(false, sql2, call);
        }
        public void OpenPage(string result)
        {
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            SecurityButton x = new SecurityButton(int.Parse(dictionary["IDKey"][0]), new string[] { "Employee" });
            x.Integer2 = int.Parse(dictionary["Stage"][0]);
            EventArgs y = new EventArgs();
            onClicked(x, y);
        }
        public void PerformSearch(string result)
        {
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            TaskCallback call = new TaskCallback(this.populateList);
            if (dictionary.Count > 0)
            {
                string text = "SELECT cusindex.Name,cusindex.IDKey,cusfields.Value,cusfields.Index,cusindex.Stage FROM cusindex INNER JOIN cusfields ON cusindex.IDKey=cusfields.CusID WHERE (cusfields.Index LIKE '%Address%' OR cusfields.Index LIKE '%Phone%') AND (";
                foreach (string str in dictionary["IDKey"])
                {
                    text = text + " cusindex.IDKey='" + str + "' OR";
                }
                text += " cusindex.IDKey='0');";
                this.PurgeCells();
                DatabaseFunctions.SendToPhp(false, text, call);
            }
        }
        public void PurgeCells()
        {
            foreach (ViewCell item in this.views)
            {
                this.TSection.Remove(item);
            }
        }
        public string appendPickerResult()
        {
            string sql = "";
            if ((string)picker.SelectedItem == "All")
            {
                sql += "ORDER BY cusindex.IDKey DESC";
            }
            else if ((string)picker.SelectedItem == "Leads")
            {
                sql += " AND cusindex.Stage='1' ORDER BY cusindex.IDKey DESC";
            }
            else if ((string)picker.SelectedItem == "Booked")
            {
                sql += " AND cusindex.Stage='2' ORDER BY cusindex.IDKey DESC";
            }
            else if ((string)picker.SelectedItem == "Quoted")
            {
                sql += " AND cusindex.Stage='3' ORDER BY cusindex.IDKey DESC";
            }
            else if ((string)picker.SelectedItem == "Follow Up With")
            {
                sql += " AND cusindex.Stage='3' ORDER BY cusindex.IDKey DESC";
            }
            else if ((string)picker.SelectedItem == "Sold")
            {
                sql += " AND cusindex.Stage='4' ORDER BY cusindex.IDKey DESC";
            }
            else if ((string)picker.SelectedItem == "Installs")
            {
                sql += " AND cusindex.Stage='5' ORDER BY cusindex.IDKey DESC";
            }
            else if ((string)picker.SelectedItem == "Installing")
            {
                sql += " AND cusindex.Stage='6' ORDER BY cusindex.IDKey DESC";
            }
            else if ((string)picker.SelectedItem == "QA")
            {
                sql += " AND cusindex.Stage='7' ORDER BY cusindex.IDKey DESC";
            }
            else if ((string)picker.SelectedItem == "Clients")
            {
                sql += " AND cusindex.Stage='8' ORDER BY cusindex.IDKey DESC";
            }
            else if ((string)picker.SelectedItem == "Archived")
            {
                sql += " AND cusindex.Stage='9' ORDER BY cusindex.IDKey DESC";
            }
            return sql;
        }
    }
}