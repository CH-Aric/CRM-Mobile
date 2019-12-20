using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MainCRMV2.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CouponChecker_Page : ContentPage
    {
        public CouponChecker_Page()
        {
            InitializeComponent();
        }
        public void onClickCheck(object sender,EventArgs e)
        {
            string today = ""+DateTime.Today;
            string sql = "SELECT * FROM coupons WHERE Code='" + CouponEntry.Text + "' AND ((StartDate<'"+today+ "'AND EndDate>'" + today + "') OR EndDate IS NULL);";
            TaskCallback call = populate;
            DatabaseFunctions.SendToPhp(false,sql,call);
        }
        public void populate(string result)
        {
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            if (dictionary.Count > 0)
            {
                detailDisplay.Text=dictionary["Description"][0];
            }
            else
            {
                detailDisplay.Text = "Please check the coupon Code and Expiry and try again: "+CouponEntry.Text+" Does not appear to be valid!";
            }
        }
    }
}