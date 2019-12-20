using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MainCRMV2.Pages.Customers
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Tombstone_Page : ContentPage
    {
        public int CusID;
        public Tombstone_Page(int cusID)
        {
            InitializeComponent();
            CusID = cusID;
            string sql = "SELECT cusindex.Name,cusfields.IDKey AS FID,cusindex.IDKey,cusfields.Value,cusfields.Index FROM cusfields INNER JOIN cusindex ON cusfields.cusID=cusindex.IDKey WHERE cusfields.CusID='" + CusID + "';";
            TaskCallback call = TombstonePrinter;
            DatabaseFunctions.SendToPhp(false, sql, call);
        }
        public void TombstonePrinter(string result)
        {
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            Name.Text = FormatFunctions.PrettyDate(dictionary["Name"][0]);
            for(int i = 0; i < dictionary["Name"].Count; i++)
            {
                if (dictionary["Index"][i].Contains("Address"))
                {
                    Address.Text = FormatFunctions.PrettyDate(dictionary["Value"][i]);
                }
                else if (dictionary["Index"][i].Contains("Phone"))
                {
                    Phone.Text = FormatFunctions.PrettyPhone(dictionary["Value"][i]);
                }
                else if (dictionary["Index"][i].Contains("Email"))
                {
                    Email.Text = FormatFunctions.PrettyDate(dictionary["Value"][i]);
                }
                else if (dictionary["Index"][i].Contains("Region"))
                {
                    Region.Text = FormatFunctions.PrettyDate(dictionary["Value"][i]);
                }
                else if (dictionary["Index"][i].Contains("First Contact"))
                {
                    FirstContact.Text = FormatFunctions.PrettyDate("First Contact: " +dictionary["Value"][i]);
                }
                else if (dictionary["Index"][i].Contains("Last Contact"))
                {
                    LastContact.Date = DateTime.Parse(FormatFunctions.PrettyDate(dictionary["Value"][i]));
                }
                else if (dictionary["Index"][i].Contains("Source"))
                {
                    Source.Text = FormatFunctions.PrettyDate(dictionary["Value"][i]);
                }
            }
        }
        public void onClickAddJob(object sender,EventArgs e)
        {

        }
        public void onClickSave(object sender, EventArgs e)
        {
            string sql  = "UPDATE cusfields SET Value='"+ FormatFunctions.CleanDateNew(Address.Text)+"' WHERE Index LIKE '%Address%' AND CusID='"+CusID+"'";
            string sql2 = "UPDATE cusfields SET Value='" + FormatFunctions.CleanDateNew(Phone.Text) + "' WHERE Index LIKE '%Phone%' AND CusID='" + CusID + "'";
            string sql3 = "UPDATE cusfields SET Value='" + FormatFunctions.CleanDateNew(Email.Text) + "' WHERE Index LIKE '%Email%' AND CusID='" + CusID + "'";
            string sql4 = "UPDATE cusfields SET Value='" + FormatFunctions.CleanDateNew(Region.Text) + "' WHERE Index LIKE '%Region%' AND CusID='" + CusID + "'";
            string sql5 = "UPDATE cusfields SET Value='" + FormatFunctions.CleanDateNew(LastContact.Date.ToString()) + "' WHERE Index LIKE '%Address%' AND CusID='" + CusID + "'";
            string sql6 = "UPDATE cusindex SET Name='"+Name.Text+"' WHERE IDKey='"+CusID+"'";
            string sql7 = "UPDATE cusfields SET Value='"+ FormatFunctions.CleanDateNew(Source.Text) + "' WHERE Index LIKE '%Source%' AND CusID='"+CusID+"'";
            List<string> batch = new List<string>() { sql,sql2,sql3,sql4,sql5,sql6};
            DatabaseFunctions.SendBatchToPHP(batch);
        }
    }
}