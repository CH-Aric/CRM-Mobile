using MainCRMV2.Pages.Popup_Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace MainCRMV2.Pages
{
    public partial class CDR_Page : ContentPage
    {
        private List<ViewCell> views;
        public CDR_Page(bool Searchmode, string Searchfor)
        {

            views = new List<ViewCell>();
            this.InitializeComponent();
            if (!Searchmode)
            {
                SearchEntry.Text = Searchfor;
                SearchEntry.IsEnabled = false;
                SearchButton.IsEnabled = false;
                string text = "SELECT DISTINCT cusfields.Value FROM cusfields WHERE cusfields.Index LIKE '%Phone%' AND ( CusID = '" + Searchfor + "');";
                TaskCallback call = new TaskCallback(this.performSearch3);
                DatabaseFunctions.SendToPhp(false, text, call);
            }
        }
        public void PerformSearch()
        {
            string statement = "SELECT DISTINCT CusID FROM cusfields WHERE Value LIKE '%" + this.SearchEntry.Text + "%'";
            TaskCallback call = new TaskCallback(this.performSearch2);
            DatabaseFunctions.SendToPhp(false, statement, call);
        }
        public void performSearch2(string result)
        {
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            string text = "SELECT DISTINCT cusfields.Value FROM cusfields WHERE cusfields.Index LIKE '%Phone%' AND (cusfields.Value='-14' ";
            foreach (string str in dictionary["CusID"])
            {
                text = text + "OR CusID = '" + str + "' ";
            }
            text += ");";
            TaskCallback call = new TaskCallback(this.performSearch3);
            DatabaseFunctions.SendToPhp(false, text, call);
        }
        public void performSearch3(string result)
        {
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            string text = "SELECT uniqueid,cnum,cnam,disposition,duration,did,recordingfile,src,calldate FROM asteriskcdrdb.cdr WHERE ";
            foreach (string phone in dictionary["Value"])
            {
                string text2 = FormatFunctions.CleanPhone(phone);
                text = text+"dst LIKE'%" +text2 +"%' OR src LIKE'%" +text2 +"%' OR clid LIKE'%" +text2 +"%' OR cnam LIKE'%"+text2+"%' OR cnum LIKE'%" +text2 +"%' OR outbound_cnam LIKE'%"+text2 +"%' OR outbound_cnum LIKE'%" +text2 +"%' OR ";
            }
            text = Regex.Replace(text, "[-()]", "");
            text += "src='-14' ORDER BY calldate DESC LIMIT 500;";
            TaskCallback call = new TaskCallback(this.populateResults);
            DatabaseFunctions.SendToPhp(true, text, call);
        }
        public void populateResults(string result)
        {
            this.PurgeCells();
            this.views = new List<ViewCell>();
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result),true);
            if (dictionary.Count > 1)
            {
                for (int i = 0; i < dictionary["uniqueid"].Count; i++)
                {
                    string text = dictionary["calldate"][i] + " : " + dictionary["cnam"][i];
                    SecurityButton dataButton = new SecurityButton(int.Parse(Regex.Replace(dictionary["uniqueid"][i], "^[^.]+.", "")),new string[]{ "Manager"})
                    {
                        Text = text,
                        FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        HorizontalOptions = LayoutOptions.EndAndExpand
                    };
                    dataButton.Clicked += onClicked;
                    dataButton.String = dictionary["calldate"][i];
                    dataButton.String2 = dictionary["recordingfile"][i];
                    ViewCell viewCell = new ViewCell();
                    StackLayout stackLayout = new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal
                    };
                    stackLayout.Children.Add(dataButton);
                    viewCell.View = stackLayout;
                    this.views.Add(viewCell);
                    this.TSection.Add(viewCell);
                }
            }
        }
        public void openFile(string result)
        {

        }
        public async void onClicked(object sender, EventArgs e)
        {
            SecurityButton dataButton = (SecurityButton)sender;
            TaskCallback call = new TaskCallback(this.openFile);
            string loadedfile=DatabaseFunctions.getFile(dataButton.String, dataButton.String2, call);
            await PopupNavigation.Instance.PushAsync(new Audio_Popup(loadedfile), true);
        }
        public void onClickedSearch(object sender, EventArgs e)
        {
            this.PerformSearch();
        }
        public void onClickedExplicitySearch(object sender, EventArgs e)
        {
            string text = "SELECT uniqueid,cnum,cnam,disposition,duration,did,recordingfile,src,calldate FROM asteriskcdrdb.cdr WHERE ";
            string text2 = this.SearchEntry.Text;
            text = text+"dst LIKE'%"+text2+"%' OR src LIKE'%"+text2+"%' OR clid LIKE'%"+text2+"%' OR cnam LIKE'%"+text2+"%' OR cnum LIKE'%"+text2+"%' OR outbound_cnam LIKE'%"+text2+"%' OR outbound_cnum LIKE'%"+text2+"%' LIMIT 500;";
            text = Regex.Replace(text, "[-()]", "");
            TaskCallback call = new TaskCallback(this.populateResults);
            DatabaseFunctions.SendToPhp(true, text, call);
        }
        public void PurgeCells()
        {
            if (this.views.Count > 0)
            {
                foreach (ViewCell item in this.views)
                {
                    this.TSection.Remove(item);
                }
            }
        }
    }
}
