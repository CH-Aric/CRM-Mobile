using Plugin.Geolocator;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Maps;
using System;
using System.Collections.Generic;

namespace MainCRMV2.Pages.Customers
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Booking_Page : ContentPage
    {
        Location place;
        int customerID;
        private List<DataPair> entryDict;
        public Booking_Page(int cusID)
        {
            customerID = cusID;
            InitializeComponent();
            searchCustomerData();
        }
        public void onClickAdvance(object sender, EventArgs e)
        {
            App.MDP.Detail.Navigation.PopToRootAsync();
            App.MDP.Detail.Navigation.PushAsync(new Advance_Page(customerID));
        }
        public async void renderBookingMap(string Address)
        {
            var geoloc = await Geocoding.GetLocationsAsync(Address);
            place = geoloc.FirstOrDefault();
            Xamarin.Forms.Maps.Map map=new Xamarin.Forms.Maps.Map(MapSpan.FromCenterAndRadius(new Position(place.Latitude,place.Longitude),Distance.FromKilometers(1)))
            {
                IsShowingUser = true,
                HeightRequest = 100,
                WidthRequest = 960,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            HeadData.Children.Add(map,0,3);
            Grid.SetColumnSpan(map, 2);
        }
        public async void onClickNavigate(object sender,EventArgs e)
        {
            var options = new MapLaunchOptions { NavigationMode = NavigationMode.Driving };
            await Xamarin.Essentials.Map.OpenAsync(place, options);
        }
        public void searchCustomerData()
        {
            string sql = "SELECT cusfields.Index,cusfields.Value,cusindex.Name FROM crm2.cusfields INNER JOIN crm2.cusindex ON cusfields.CusID=cusindex.IDKey WHERE (cusfields.Index LIKE '%phone%' OR cusfields.Index LIKE '%address%' OR cusfields.Index LIKE '%book%') AND cusfields.CusID='"+customerID+"';";
            TaskCallback call = populateCustomerData;
            DatabaseFunctions.SendToPhp(false, sql, call);
        }
        public void populateCustomerData(string result)
        {
            string address = "2591 Ottawa Regional Road 174";
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            if (dictionary.Count > 0)
            {
                nameLabel.Text = dictionary["Name"][0];
                for (int i = 0; i < dictionary["Index"].Count; i++)
                {
                    if (dictionary["Index"][i].Contains("hone"))
                    {
                        phoneLabel.Text = dictionary["Value"][i];
                    }
                    else if (dictionary["Index"][i].Contains("ook"))
                    {
                        bookLabel.Text = "Booked For: "+dictionary["Value"][i];
                    }
                    else if (dictionary["Index"][i].Contains("ress"))
                    {
                        address = dictionary["Value"][i];
                        navButton.Text += dictionary["Value"][i];
                        address = dictionary["Value"][i];
                    }
                }
            }
            renderBookingMap(address);
        }
        public void onClicked(object sender, EventArgs e)
        {
            List<string> batch = new List<string>();
            foreach (DataPair dataPair in this.entryDict)
            {
                if (dataPair.isNew)
                {
                    string s = "INSERT INTO cusfields (cusfields.Value,cusfields.Index,CusID) VALUES('" + FormatFunctions.CleanDateNew(dataPair.Value.Text) + "','" + FormatFunctions.CleanDateNew(dataPair.Index.Text) + "','" + this.customerID + "')";
                    batch.Add(s);
                    dataPair.isNew = false;
                }
                else if (!dataPair.Index.Text.Equals(dataPair.Index.GetInit()))
                {
                    string s = "UPDATE cusfields SET cusfields.Value = '" + FormatFunctions.CleanDateNew(dataPair.Value.Text) + "',cusfields.Index='" + FormatFunctions.CleanDateNew(dataPair.Index.Text) + "' WHERE (IDKey= '" + dataPair.Index.GetInt() + "');";
                    batch.Add(s);
                }
            }
            string sql2 = "UPDATE cusindex SET Name='" + FormatFunctions.CleanDateNew(nameLabel.Text) + "' WHERE IDKey= '" + customerID + "'";
            batch.Add(sql2);
            //string sql3 = "UPDATE cusfields SET cusfields.value='" + FormatFunctions.CleanDateNew(BookingDate.Text) + "' WHERE cusfields.Index LIKE '%ookin%' AND CusID= '" + customerID + "'";
            //batch.Add(sql3);
            string sql4 = "UPDATE cusfields SET cusfields.value='" + FormatFunctions.CleanDateNew(phoneLabel.Text) + "' WHERE cusfields.Index LIKE '%hone%' AND CusID= '" + customerID + "'";
            batch.Add(sql4);
            string sql5 = "UPDATE cusfields SET cusfields.value='" + FormatFunctions.CleanDateNew(DateTime.Now.ToString("yyyy/M/d h:mm:ss")) + "' WHERE cusfields.Index LIKE '%odified On%' AND CusID= '" + customerID + "'";//'Modified On','" + FormatFunctions.CleanDateNew(DateTime.Now.ToString("yyyy/M/d h:mm:ss")) + "'
            batch.Add(sql5);
            DatabaseFunctions.SendBatchToPHP(batch);
        }
    }
}