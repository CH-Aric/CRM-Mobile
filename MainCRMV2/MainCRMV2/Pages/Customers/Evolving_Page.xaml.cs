using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MainCRMV2.Pages.Customers
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Evolving_Page : TabbedPage
    {
        List<Job_Page> JobSheets;
        int CusID;

        public Evolving_Page(int customer)
        {
            InitializeComponent();
            CusID = customer;
            Tombstone_Page tombPage = new Tombstone_Page(CusID);
            this.Children.Add(tombPage);
            string sql2 = "SELECT * FROM jobindex WHERE CusID='"+CusID+"'";
            TaskCallback call2 = populateSheetNames;
            DatabaseFunctions.SendToPhp(false, sql2, call2);
        }
        public void populateSheetNames(string result)
        {
            JobSheets = new List<Job_Page>();
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            for (int i = 0; i < dictionary["IDKey"].Count; i++)
            {
                Job_Page x = new Job_Page(int.Parse(dictionary["IDKey"][i])) { Title = dictionary["Name"][i] };
                this.Children.Add(x);
                JobSheets.Add(x);
            }
        }
    }
}