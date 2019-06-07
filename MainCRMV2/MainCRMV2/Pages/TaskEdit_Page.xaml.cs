using System;
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

        // Token: 0x0400008F RID: 143
        private int task;
        public TaskEdit_Page(int i)
        {
            this.task = i;
            InitializeComponent();
            TaskCallback call = new TaskCallback(this.populateFields);
            DatabaseFunctions.SendToPhp(false, "SELECT * FROM taskfields WHERE TaskID='" + i + "';", call);
        }

        // Token: 0x060000CA RID: 202 RVA: 0x00008234 File Offset: 0x00006434
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

        // Token: 0x060000CB RID: 203 RVA: 0x000083EC File Offset: 0x000065EC
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

        // Token: 0x060000CC RID: 204 RVA: 0x00008534 File Offset: 0x00006734
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

        /*// Token: 0x060000CD RID: 205 RVA: 0x00008664 File Offset: 0x00006864
        [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private void InitializeComponent()
        {
            if (ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
            {
                AssemblyName = typeof(TaskEdit_Page).GetTypeInfo().Assembly.GetName(),
                ResourcePath = "Pages/TaskEdit_Page.xaml"
            }))
            {
                this.__InitComponentRuntime();
                return;
            }
            if (XamlLoader.XamlFileProvider != null && XamlLoader.XamlFileProvider(base.GetType()) != null)
            {
                this.__InitComponentRuntime();
                return;
            }
            Label label = new Label();
            Button button = new Button();
            Button button2 = new Button();
            Label label2 = new Label();
            StackLayout stackLayout = new StackLayout();
            ViewCell viewCell = new ViewCell();
            TableSection tableSection = new TableSection();
            TableRoot tableRoot = new TableRoot();
            TableView tableView = new TableView();
            NameScope nameScope = new NameScope();
            NameScope.SetNameScope(this, nameScope);
            ((INameScope)nameScope).RegisterName("TSection", tableSection);
            this.TSection = tableSection;
            tableSection.SetValue(TableSectionBase.TitleProperty, "Task Fields");
            stackLayout.SetValue(StackLayout.OrientationProperty, StackOrientation.Horizontal);
            label.SetValue(Label.TextProperty, "Index");
            stackLayout.Children.Add(label);
            button.SetValue(Button.TextProperty, "Add Field");
            button.Clicked += this.onClickAddFields;
            stackLayout.Children.Add(button);
            button2.SetValue(Button.TextProperty, "Save");
            button2.Clicked += this.onClicked;
            stackLayout.Children.Add(button2);
            label2.SetValue(Label.TextProperty, "Value");
            label2.SetValue(View.HorizontalOptionsProperty, LayoutOptions.EndAndExpand);
            stackLayout.Children.Add(label2);
            viewCell.View = stackLayout;
            tableSection.Add(viewCell);
            tableRoot.Add(tableSection);
            tableView.Root = tableRoot;
            this.SetValue(ContentPage.ContentProperty, tableView);
        }

        // Token: 0x060000CE RID: 206 RVA: 0x00008839 File Offset: 0x00006A39
        private void __InitComponentRuntime()
        {
            this.LoadFromXaml(typeof(TaskEdit_Page));
            this.TSection = this.FindByName("TSection");
        }

        // Token: 0x0400008E RID: 142
       

        // Token: 0x04000090 RID: 144
        [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private TableSection TSection;*/
    }
}
