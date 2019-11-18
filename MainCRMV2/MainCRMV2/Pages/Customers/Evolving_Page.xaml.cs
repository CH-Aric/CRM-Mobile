using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MainCRMV2.Pages.Customers
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Evolving_Page : TabbedPage
    {
        List<ContentPage> JobSheets;
        int CusID;
        Grid tombGrid;
        List<DataPair> tombPairs;
        Entry NameEntry;
        public Evolving_Page(int customer)
        {
            InitializeComponent();
            CusID = customer;
            string sql = "SELECT cusindex.Name,cusfields.IDKey AS FID,cusindex.IDKey,cusfields.Value,cusfields.Index FROM cusfields INNER JOIN cusindex ON cusfields.cusID=cusindex.IDKey WHERE cusfields.CusID='" + CusID + "';";
            TaskCallback call = TombstonePrinter;
            DatabaseFunctions.SendToPhp(false, sql, call);
            string sql2 = "SELECT * FROM jobindex";
            TaskCallback call2 = populateSheetNames;
            DatabaseFunctions.SendToPhp(false, sql2, call2);
        }
        public void populateSheetNames(string result)
        {
            PurgeSheets();
            JobSheets = new List<ContentPage>();
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            for (int i = 0; i < dictionary["IDKey"].Count; i++)
            {
                ContentPage x = new ContentPage() { Title = dictionary["Name"][i] };
                this.Children.Add(x);
                JobSheets.Add(x);
                if (int.Parse(dictionary["Stage"][i]) > 0 && int.Parse(dictionary["Stage"][i]) < 10)
                {
                    populateInstallSheet(int.Parse(dictionary["Stage"][i]), dictionary["Name"][i], x);
                }
                else if (int.Parse(dictionary["Stage"][i]) > 9 && int.Parse(dictionary["Stage"][i]) < 13)
                {
                    populateServiceSheet(int.Parse(dictionary["Stage"][i]), dictionary["Name"][i], x);
                }
                else if (int.Parse(dictionary["Stage"][i]) > 12 && int.Parse(dictionary["Stage"][i]) < 16)
                {
                    populateMaintenanceSheet(int.Parse(dictionary["Stage"][i]), dictionary["Name"][i], x);
                }
            }
        }
        public void populateInstallSheet(int Stage, string Name, ContentPage workingPage)
        {
            StackLayout SL = new StackLayout();
            SL.Children.Add(tombGrid);
            workingPage.Content=SL;

        }
        public void populateServiceSheet(int Stage, string Name, ContentPage workingPage)
        {
            StackLayout SL = new StackLayout();
            SL.Children.Add(tombGrid);
            workingPage.Content = SL;

        }
        public void populateMaintenanceSheet(int Stage, string Name, ContentPage workingPage)
        {
            StackLayout SL = new StackLayout();
            SL.Children.Add(tombGrid);
            workingPage.Content = SL;
        }
        public void TombstonePrinter(string result)
        {
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            NameEntry = new Entry() { Text=dictionary["Name"][0]};
            Label nameLabel = new Label() { Text="Name:", HorizontalOptions = LayoutOptions.EndAndExpand ,VerticalOptions = LayoutOptions.CenterAndExpand };
            tombGrid = new Grid();
            GridFiller.rapidFillPremadeObjects(new List<View> { NameEntry,nameLabel }, tombGrid, new bool[] { true, true });
            tombPairs = new List<DataPair>();
            ColumnDefinition ten = new ColumnDefinition();
            ten.Width = new GridLength(10,GridUnitType.Star);
            tombGrid.ColumnDefinitions.Add(ten);
            tombGrid.ColumnDefinitions.Add(ten);
            for (int i = 0; i < dictionary["IDKey"].Count/2; i++)
            {
                DataPair newEntry = new DataPair(int.Parse(dictionary["FID"][i]), dictionary["Index"][i], dictionary["Value"][i]);
                GridFiller.rapidFillPremadeObjects(new List<View> { newEntry.Index,newEntry.Value},tombGrid,new bool[]{ true,true});
                tombPairs.Add(newEntry);
            }
        }
        public void PurgeSheets()
        {
            {
                for (int i = this.Children.Count; i > 0; i--)
                {
                    this.Children.RemoveAt(i);
                }
            }
        }
    }
}