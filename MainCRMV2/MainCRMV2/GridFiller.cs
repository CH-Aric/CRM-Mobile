using MainCRMV2.Pages.Popup_Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MainCRMV2
{
    public static class GridFiller
    {
        public static void rapidFill(string[]strings, Grid g){
            int i= 0;
            Color c= ClientData.getGridColor();
            g.RowDefinitions.Add(new RowDefinition { Height = new GridLength(10, GridUnitType.Star) });
            foreach (string s in strings)
            {
                BoxView b = new BoxView() { BackgroundColor = c, Margin = ClientData.GridMargin };
                Label l = new Label() { Text = s, HorizontalOptions = LayoutOptions.FillAndExpand, TextColor = ClientData.textColor };
                g.Children.Add(b, i, g.RowDefinitions.Count - 1);
                g.Children.Add(l, i, g.RowDefinitions.Count - 1);
                i++;
            }
        }
        public static void rapidFillColorized(string[] strings, Grid g,bool CC)
        {
            int i = 0;
            Color c = ClientData.getGridColorCC(CC);
            g.RowDefinitions.Add(new RowDefinition { Height = new GridLength(10, GridUnitType.Star) });
            foreach (string s in strings)
            {
                BoxView b = new BoxView() { BackgroundColor = c, Margin = ClientData.GridMargin };
                Label l = new Label() { Text = s, HorizontalOptions = LayoutOptions.FillAndExpand, TextColor = ClientData.textColor };
                g.Children.Add(b, i, g.RowDefinitions.Count - 1);
                g.Children.Add(l, i, g.RowDefinitions.Count - 1);
                i++;
            }
        }
        public static void rapidAppendData(string[] values,int[] IDs,Grid g)
        {
            int count = 1;
            Color c = ClientData.getGridColor();
            int column = g.ColumnDefinitions.Count;
            g.ColumnDefinitions.Add(new ColumnDefinition { Width=new GridLength(25,GridUnitType.Star)});
            BoxView b1 = new BoxView() { BackgroundColor = c, Margin = ClientData.GridMargin };
            Label l = new Label() { Text = "Audited", TextColor = Color.Black };

            g.Children.Add(b1, column, 0);
            g.Children.Add(l, column, 0);
            foreach (string i in values)
            {
                BoxView b = new BoxView() { BackgroundColor = c, Margin = ClientData.GridMargin };
                Label d = new Label() { Text=values[count-1],TextColor=Color.Black};
                g.Children.Add(b, column, count);
                g.Children.Add(d, column, count);
                count++;
            }
        }
        public static void rapidFillSpaced(string[] strings, Grid g, int[] Spacing)
        {
            int i = 0;
            int r = 0;
            Color c = ClientData.getGridColor();
            g.RowDefinitions.Add(new RowDefinition { Height = new GridLength(10, GridUnitType.Star) });
            foreach (string s in strings)
            {
                BoxView b = new BoxView() { BackgroundColor = c, Margin = ClientData.GridMargin};
                Label l = new Label() { Text = s, HorizontalOptions = LayoutOptions.FillAndExpand, TextColor = ClientData.textColor };
                g.Children.Add(b, i, g.RowDefinitions.Count - 1);
                g.Children.Add(l, i, g.RowDefinitions.Count - 1);
                Grid.SetColumnSpan(b, Spacing[r]);
                Grid.SetColumnSpan(l, Spacing[r]);
                i += Spacing[r];
                r++;
            }
        }
        public static void rapidFillSpacedPremadeObjects(List<View> Objects, Grid g, int[] Spacing, bool[] boxoff)
        {
            int i = 0;
            int r = 0;
            Color c = ClientData.getGridColor();
            g.RowDefinitions.Add(new RowDefinition { Height = new GridLength(10, GridUnitType.Star) });
            foreach (View s in Objects)
            {
                if (boxoff[i])
                {
                    BoxView b = new BoxView() { BackgroundColor = c, Margin = ClientData.GridMargin };
                    g.Children.Add(b, i, g.RowDefinitions.Count - 1);
                    Grid.SetColumnSpan(b, Spacing[r]);
                }                
                g.Children.Add(s, i, g.RowDefinitions.Count - 1);
                Grid.SetColumnSpan(s, Spacing[r]);
                i += Spacing[r];
                r++;
            }
        }
        public static void rapidFillSpacedRowHeightLocked(string[] strings, Grid g, int[] Spacing, int[] rowHeigtWidth)
        {
            int i = 0;
            int r = 0;
            Color c = ClientData.getGridColor();
            g.RowDefinitions.Add(new RowDefinition { Height = new GridLength(rowHeigtWidth[0], GridUnitType.Absolute) });
            foreach (string s in strings)
            {
                BoxView b = new BoxView() { BackgroundColor = c, Margin = ClientData.GridMargin };
                Label l = new Label() { Text = s, HorizontalOptions = LayoutOptions.FillAndExpand, TextColor = ClientData.textColor };
                g.Children.Add(b, i, g.RowDefinitions.Count - 1);
                g.Children.Add(l, i, g.RowDefinitions.Count - 1);
                Grid.SetColumnSpan(b, Spacing[r]);
                Grid.SetColumnSpan(l, Spacing[r]);
                i += Spacing[r];
                r++;
            }
        }
        public static void rapidFillPremadeObjects(List<View> Objects,Grid g,bool[] boxoff)
        {
            int i = 0;
            Color c = ClientData.getGridColor();
            g.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50, GridUnitType.Absolute) });
            foreach (View s in Objects)
            {
                if (boxoff[i])
                {
                    BoxView b = new BoxView() { BackgroundColor = c, Margin = ClientData.GridMargin };
                    g.Children.Add(b, i, g.RowDefinitions.Count - 1);
                }
                g.Children.Add(s, i, g.RowDefinitions.Count - 1);
                i++;
            }
        }
        public static void rapidFillPremadeObjectsStandardHeight(List<View> Objects, Grid g, bool[] boxoff,int Height)
        {
            int i = 0;
            Color c = ClientData.getGridColor();
            g.RowDefinitions.Add(new RowDefinition { Height = new GridLength(Height, GridUnitType.Absolute) });
            foreach (View s in Objects)
            {
                if (boxoff[i])
                {
                    BoxView b = new BoxView() { BackgroundColor = c, Margin = ClientData.GridMargin };
                    g.Children.Add(b, i, g.RowDefinitions.Count - 1);
                }
                g.Children.Add(s, i, g.RowDefinitions.Count - 1);
                i++;
            }
        }
        public static void PurgeGrid(Grid g)
        {
            var children = g.Children.ToList();
            foreach (var child in children.Where(child => Grid.GetRow(child) != 0))
            {
                g.Children.Remove(child);
            }
            while (g.RowDefinitions.Count > 1)
            {
                g.RowDefinitions.RemoveAt(0);
            }
        }
        public static void PurgeAllGrid(Grid g)
        {
            var children = g.Children.OfType<View>().ToArray();
            foreach (View child in children)
            {
                g.Children.Remove(child);
            }
            while (g.RowDefinitions.Count > 0)
            {
                g.RowDefinitions.RemoveAt(0);
            }
        }
        public static void PurgeHeader(Grid g)
        {
            var children = g.Children.OfType<View>().ToArray();
            foreach (View child in children)
            {
                if (Grid.GetRow(child) == 1)
                {
                    g.Children.Remove(child);
                }
            }
            while (g.RowDefinitions.Count > 1)
            {
                g.RowDefinitions.RemoveAt(0);
            }
        }
    }
    public class FileList : Grid
    {
        int customer;
        public FileList(int cusID) : base()
        {
            customer = cusID;
            string sql = "SELECT cusfields.Value,cusindex.Name FROM cusfields INNER JOIN cusindex ON cusfields.CusID=cusindex.IDKey WHERE cusindex.IDKey='"+cusID+"' AND cusfields.Index LIKE '%dress%'";
            TaskCallback call = populateGrid;
            DatabaseFunctions.SendToPhp(false,sql,call);
            GridFiller.rapidFill(new string[] { "Associated Files"},this);
        }
        public void populateGrid(string result)
        {
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            if (dictionary.Count > 0)
            {
                string filepath = dictionary["Value"][0] + " - " + dictionary["Name"][0];
                string[] customerFileList = DatabaseFunctions.getCustomerFileList(filepath);
                filepath.Replace(" ", "%20");
                foreach (string text in customerFileList)
                {
                    if ((text != "." || text != "..") && customerFileList.Length > 1)
                    {
                        SecurityButton dataButton = new SecurityButton(text, new string[] { "Sales" })
                        {
                            Text = text,
                            String2=filepath,
                            FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label))
                        };
                        dataButton.Clicked += onFileButton;
                        List<View> list = new List<View>() { dataButton };
                        GridFiller.rapidFillPremadeObjects(list, this, new bool[] { false });
                    }
                }
            }
        }
        public void onFileButton(object sender, EventArgs e)
        {
            SecurityButton sb = (SecurityButton)sender;
            string escapedFileName = Uri.EscapeUriString(sb.String2 + "/" + sb.Text);
            sendToBrowser("http://coolheatcrm.duckdns.org/sym/" + escapedFileName);
        }
        public async void sendToBrowser(string f)
        {

            await Browser.OpenAsync(f, BrowserLaunchMode.SystemPreferred);
        }
    }
}