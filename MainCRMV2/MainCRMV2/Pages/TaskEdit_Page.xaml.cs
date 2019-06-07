﻿using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Internals;

namespace MainCRMV2.Pages
{
    public partial class TaskEdit_Page : ContentPage
    {
        private List<DataPair> entryDict;
        private int task;
        public TaskEdit_Page(int i)
        {
            this.task = i;
            InitializeComponent();
            TaskCallback call = new TaskCallback(this.populateFields);
            DatabaseFunctions.SendToPhp(false, "SELECT * FROM taskfields WHERE TaskID='" + i + "';", call);
        }
        public void populateFields(string result)
        {
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            this.entryDict = new List<DataPair>();
            if (dictionary.Count > 0)
            {
                for (int i = 0; i < dictionary["Index"].Count; i++)
                {
                    DataPair dataPair = new DataPair(int.Parse(dictionary["FID"][i]), dictionary["value"][i], dictionary["Index"][i]);
                    dataPair.Value.Text = dictionary["value"][i];
                    dataPair.Value.Placeholder = "Value here";
                    dataPair.Value.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
                    dataPair.Value.VerticalOptions = LayoutOptions.CenterAndExpand;
                    dataPair.Value.HorizontalOptions = LayoutOptions.StartAndExpand;
                    dataPair.Index.Text = dictionary["Index"][i];
                    dataPair.Index.Placeholder = "Index here";
                    dataPair.Index.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
                    dataPair.Index.VerticalOptions = LayoutOptions.CenterAndExpand;
                    dataPair.Index.HorizontalOptions = LayoutOptions.EndAndExpand;
                    ViewCell viewCell = new ViewCell();
                    StackLayout stackLayout = new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal
                    };
                    stackLayout.Children.Add(dataPair.Index);
                    stackLayout.Children.Add(dataPair.Value);
                    viewCell.View = stackLayout;
                    this.TSection.Add(viewCell);
                    this.entryDict.Add(dataPair);
                }
            }
        }
        public void onClicked(object sender, EventArgs e)
        {
            foreach (DataPair dataPair in this.entryDict)
            {
                if (dataPair.isNew)
                {
                    DatabaseFunctions.SendToPhp(string.Concat(new object[]
                    {
                        "INSERT INTO taskfields (cusfields.Value,cusfields.Index,CusID) VALUES('",
                        dataPair.Value.Text,
                        "','",
                        dataPair.Index.Text,
                        "','",
                        this.task,
                        "')"
                    }));
                    dataPair.isNew = false;
                }
                else if (dataPair.Index.Text != dataPair.Index.GetInit())
                {
                    DatabaseFunctions.SendToPhp(string.Concat(new object[]
                    {
                        "UPDATE taskfields SET Value = '",
                        dataPair.Value.Text,
                        "',Index='",
                        dataPair.Index.Text,
                        "' WHERE (IDKey= '",
                        dataPair.Index.GetInt(),
                        "');"
                    }));
                }
            }
        }
        public void onClickAddFields(object sender, EventArgs e)
        {
            DataPair dataPair = new DataPair(0, "", "");
            dataPair.setNew();
            dataPair.Value.Text = "";
            dataPair.Value.Placeholder = "Index here";
            dataPair.Value.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
            dataPair.Value.VerticalOptions = LayoutOptions.CenterAndExpand;
            dataPair.Value.HorizontalOptions = LayoutOptions.StartAndExpand;
            dataPair.Index.Text = "";
            dataPair.Index.Placeholder = "Value here";
            dataPair.Index.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
            dataPair.Index.VerticalOptions = LayoutOptions.CenterAndExpand;
            dataPair.Index.HorizontalOptions = LayoutOptions.EndAndExpand;
            ViewCell viewCell = new ViewCell();
            StackLayout stackLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal
            };
            stackLayout.Children.Add(dataPair.Index);
            stackLayout.Children.Add(dataPair.Value);
            viewCell.View = stackLayout;
            this.TSection.Add(viewCell);
            this.entryDict.Add(dataPair);
        }
    }
}
