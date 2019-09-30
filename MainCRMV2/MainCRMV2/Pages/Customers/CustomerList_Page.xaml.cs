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
            string sql = "SELECT cusindex.Name,cusindex.IDKey,cusfields.Value,cusfields.Index,cusindex.Stage FROM cusindex INNER JOIN cusfields ON cusindex.IDKey=cusfields.CusID WHERE (cusfields.Index LIKE '%Phone%')";
            sql += appendPickerResult();
            DatabaseFunctions.SendToPhp(false, sql, call);
        }
        public void populateList(string result)
        {
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            Dictionary<string, SecurityButton> dictionary2 = new Dictionary<string, SecurityButton>();
            if (dictionary.Count > 0)
            {
                for (int i = 0; i < dictionary["Name"].Count; i++)
                {
                    if (!dictionary2.ContainsKey(dictionary["IDKey"][i]))
                    {
                        string text = dictionary["Name"][i] + " ," + dictionary["Value"][i];
                        SecurityButton dataButton = new SecurityButton(int.Parse(dictionary["IDKey"][i]), new string[] { "Employee" })
                        {
                            Text = text,
                            FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                            VerticalOptions = LayoutOptions.CenterAndExpand,
                            HorizontalOptions = LayoutOptions.FillAndExpand
                        };
                        dataButton.Clicked += this.onClicked;
                        dataButton.Integer2 = int.Parse(dictionary["Stage"][i]);
                        List<View> list = new List<View>() { dataButton };
                        bool[] box = new bool[] { false };
                        GridFiller.rapidFillPremadeObjectsStandardHeight(list, dataGrid, box, 50);
                        dictionary2.Add(dictionary["IDKey"][i], dataButton);
                    }
                    else
                    {
                        SecurityButton dataButton2 = dictionary2[dictionary["IDKey"][i]];
                        dataButton2.Text = dataButton2.Text + " ," + dictionary["Value"][i];
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
            else if (dataButton.GetInt2() == 3)
            {
                App.MDP.Detail.Navigation.PushAsync(new Quote_Page(dataButton.Integer, stageSearch));
            }
            else if (dataButton.GetInt2() > 3 && dataButton.GetInt2() < 9)
            {
                App.MDP.Detail.Navigation.PushAsync(new Install_Page(dataButton.Integer, stageSearch));
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
        public void onCreateStageSpecificData(string cusID)
        {
            List<string> batch = new List<string>();
            string noteSQL = "INSERT INTO cusfields (cusfields.Index,cusfields.Value,CusID) VALUES ('Notes:','','" + cusID + "'),('Created On','" + FormatFunctions.CleanDateNew(DateTime.Now.ToString("yyyy/M/d h:mm:ss")) + "','" + cusID + "'),('Modified On','" + FormatFunctions.CleanDateNew(DateTime.Now.ToString("yyyy/M/d h:mm:ss")) + "','" + cusID + "')";//'"+ClientData.AgentIDK+"','"+FormatFunctions.CleanDateNew(DateTime.Now.ToString("yyyy/M/d h:mm:ss"))+"'
            batch.Add(noteSQL);
            if (NewPicker.SelectedIndex == 0)//For creating leads
            {
                //BookingDate, Notes
                string sql = "INSERT INTO cusfields (cusfields.Index,cusfields.Value,CusID) VALUES ('BookingDate','','" + cusID + "'),('PhoneNumber','','" + cusID + "')";
                batch.Add(sql);
            }
            else if (NewPicker.SelectedIndex == 1)//Booked!
            {
                string sql = "INSERT INTO cusfields (cusfields.Index,cusfields.Value,CusID) VALUES ('BookingDate','','" + cusID + "'),('PhoneNumber','','" + cusID + "'),('Salesman','','" + cusID + "')";
                batch.Add(sql);
            }
            else if (NewPicker.SelectedIndex == 2)//Quoted!
            {
                string sql = "INSERT INTO cusfields (cusfields.Index,cusfields.Value,CusID) VALUES ('BookingDate','','" + cusID + "'),('PhoneNumber','','" + cusID + "'),('Salesman','','" + cusID + "'),('QuoteTotal','','" + cusID + "'),('Signature','False','" + cusID + "'),('Deposit Received','False','" + cusID + "'),('Payment Method','Fill Me Out!','" + cusID + "')";
                batch.Add(sql);
            }
            else if (NewPicker.SelectedIndex == 3)//Followup on Quote
            {
                string sql = "INSERT INTO cusfields (cusfields.Index,cusfields.Value,CusID) VALUES ('BookingDate','','" + cusID + "'),('PhoneNumber','','" + cusID + "'),('Salesman','','" + cusID + "'),('QuoteTotal','','" + cusID + "'),('Signature','False','" + cusID + "'),('Deposit Received','False','" + cusID + "'),('Payment Method','Fill Me Out!','" + cusID + "'),('LastContact','mm/dd','" + cusID + "')";
                batch.Add(sql);
            }
            else if (NewPicker.SelectedIndex == 4)//Sold!
            {
                string sql = "INSERT INTO cusfields (cusfields.Index,cusfields.Value,CusID) VALUES ('BookingDate','','" + cusID + "'),('PhoneNumber','','" + cusID + "'),('Salesman','','" + cusID + "'),('QuoteTotal','','" + cusID + "'),('Signature','True','" + cusID + "'),('Deposit Received','True','" + cusID + "'),('Payment Method','Fill Me Out!','" + cusID + "')";
                batch.Add(sql);
            }
            else if (NewPicker.SelectedIndex == 5)//Install!
            {
                string sql = "INSERT INTO cusfields (cusfields.Index,cusfields.Value,CusID) VALUES ('PhoneNumber','','" + cusID + "'),('Salesman','','" + cusID + "'),('QuoteTotal','','" + cusID + "'),('Signature','True','" + cusID + "'),('Deposit Received','True','" + cusID + "'),('Payment Method','Fill Me Out!','" + cusID + "'),('InstallDate','','" + cusID + "')";
                batch.Add(sql);
            }
            DatabaseFunctions.SendBatchToPHP(batch);
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
            GridFiller.PurgeAllGrid(dataGrid);
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