using System;
using System.Collections.Generic;
using System.Text;
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
    }
}
