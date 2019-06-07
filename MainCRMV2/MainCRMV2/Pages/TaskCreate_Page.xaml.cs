using System;
using System.Collections.Generic;
using MainCRMV2.Pages.Popup_Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MainCRMV2.Pages
{
    // Token: 0x02000023 RID: 35
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class TaskCreate_Page : ContentPage
    {
        private List<DataEntry> Names;
        private List<DataEntry> Values;
        private Dictionary<string, List<string>> templates;
        private string saveAs = "";
        private string descAs = "";
        public TaskCreate_Page()
        {
            InitializeComponent();
            this.loadFromDatabase();
            this.Values = new List<DataEntry>();
            this.Names = new List<DataEntry>();
        }
        public void loadFromDatabase()
        {
            TaskCallback call = new TaskCallback(this.populateTemplates);
            string statement = "SELECT Name,IDKey FROM tasktemplates";
            DatabaseFunctions.SendToPhp(false, statement, call);
        }
        public void populateTemplates(string result)
        {
            string[] input = FormatFunctions.SplitToPairs(result);
            this.templates = FormatFunctions.createValuePairs(input);
            this.templatePicker.ItemsSource = this.templates["Name"];
        }
        public void populateFields(string result)
        {
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            for (int i = 0; i < dictionary["Name"].Count; i++)
            {
                DataEntry item = new DataEntry(int.Parse(dictionary["IDKey"][i]), dictionary["Value"][i])
                {
                    Text = dictionary["Value"][i],
                    FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.StartAndExpand
                };
                DataEntry item2 = new DataEntry(int.Parse(dictionary["IDKey"][i]), dictionary["Index"][i])
                {
                    Text = dictionary["Name"][i],
                    FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.EndAndExpand
                };
                ViewCell viewCell = new ViewCell();
                StackLayout stackLayout = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal
                };
                stackLayout.Children.Add(item2);
                stackLayout.Children.Add(item);
                viewCell.View = stackLayout;
                this.TSection.Add(viewCell);
                this.Values.Add(item);
                this.Names.Add(item2);
            }
        }
        public async void onClickedSaveTemplate(object sender, EventArgs e)
        {
            TaskCallback c = new TaskCallback(this.populateTemplateName);
            await PopupNavigation.Instance.PushAsync(new GetString_Popup("Name for Template:", "Save", "Back", c), true);
        }
        public async void populateTemplateName(string result)
        {
            this.saveAs = result;
            TaskCallback callD = new TaskCallback(this.populateTemplateDesc);
            await PopupNavigation.Instance.PopAllAsync(true);
            await PopupNavigation.Instance.PushAsync(new GetString_Popup("Description for Template:", "Save", "Back", callD), true);
        }
        public async void populateTemplateDesc(string result)
        {
            this.descAs = result;
            await PopupNavigation.Instance.PopAllAsync(true);
            TaskCallback c = new TaskCallback(this.populateLastIDK);
            await PopupNavigation.Instance.PushAsync(new Notification_Popup("Your template has been saved", "OK", c), true);
        }
        public async void populateLastIDK(string result)
        {
            DatabaseFunctions.SendToPhp(string.Concat(new string[]
            {
                "INSERT INTO tasktemplates (Name,Description) VALUES ('",
                this.saveAs,
                "','",
                this.descAs,
                "');"
            }));
            string statement = "SELECT IDKey FROM tasktemplates ORDER BY IDKey DESC LIMIT 1;";
            TaskCallback call = new TaskCallback(this.saveTemplateFields);
            DatabaseFunctions.SendToPhp(false, statement, call);
        }
        public async void saveTemplateFields(string result)
        {
            string text = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result))["IDKey"][0];
            string text2 = "INSERT INTO taskfields (Index,Value,TemplateID) VALUES ";
            for (int i = 0; i < this.Names.Count; i++)
            {
                if (!string.IsNullOrEmpty(this.Names[i].Text))
                {
                    text2 = string.Concat(new string[]
                    {
                        text2,
                        "('",
                        this.Names[i].Text,
                        "', '",
                        this.Values[i].Text,
                        "', '",
                        text,
                        "'),"
                    });
                }
            }
            text2 = text2 + "('', '', '" + text + "')";
            DatabaseFunctions.SendToPhp(text2);
            TaskCallback c = new TaskCallback(this.voidCall);
            await PopupNavigation.Instance.PushAsync(new Notification_Popup("Your template has been saved", "OK", c), true);
        }
        public async void voidCall(string result)
        {
            await PopupNavigation.Instance.PopAllAsync(true);
        }
        public void onClickedLoadTemplate(object sender, EventArgs e)
        {
            int num = int.Parse(DatabaseFunctions.lookupInDictionary((string)this.templatePicker.SelectedItem, "Name", "IDKey", this.templates));
            string statement = "SELECT Value,Index,IDKey FROM taskfields WHERE TemplateID='" + num + "';";
            TaskCallback call = new TaskCallback(this.populateFields);
            DatabaseFunctions.SendToPhp(false, statement, call);
        }
        public async void onClickedCreate(object sender, EventArgs e)
        {
            TaskCallback c = new TaskCallback(this.populateTaskName);
            await PopupNavigation.Instance.PushAsync(new GetString_Popup("Name for Task:", "Ok", "Back", c), true);
        }
        public async void populateTaskName(string result)
        {
            await PopupNavigation.Instance.PopAllAsync(true);
            if (result != "Cancel")
            {
                this.saveAs = result;
                DatabaseFunctions.SendToPhp(string.Concat(new object[]
                {
                    "INSERT INTO tasks (Name,AgentID) VALUES ('",
                    this.saveAs,
                    "','",
                    ClientData.AgentIDK,
                    "');"
                }));
                DatabaseFunctions.SendToPhp(false, "SELECT IDKey FROM tasks ORDER BY IDKey DESC LIMIT 1;", new TaskCallback(this.saveTaskFields));
            }
        }
        public async void saveTaskFields(string result)
        {
            string text = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result))["IDKey"][0];
            string text2 = "INSERT INTO taskfields (Index,Value,TaskID) VALUES ";
            for (int i = 0; i < this.Names.Count; i++)
            {
                if (!string.IsNullOrEmpty(this.Names[i].Text))
                {
                    text2 = string.Concat(new string[]
                    {
                        text2,
                        "('",
                        this.Names[i].Text,
                        "', '",
                        this.Values[i].Text,
                        "', '",
                        text,
                        "'),"
                    });
                }
            }
            text2 = text2 + "('', '', '" + text + "')";
            DatabaseFunctions.SendToPhp(text2);
            TaskCallback c = new TaskCallback(this.voidCall);
            await PopupNavigation.Instance.PushAsync(new Notification_Popup("Your Task has been created", "OK", c), true);
        }
        public void onClickAddFields(object sender, EventArgs e)
        {
            DataEntry item = new DataEntry(0, "")
            {
                Text = "",
                Placeholder = "Value here",
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.StartAndExpand
            };
            DataEntry item2 = new DataEntry(0, "")
            {
                Text = "",
                Placeholder = "Name here",
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.EndAndExpand
            };
            ViewCell viewCell = new ViewCell();
            StackLayout stackLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal
            };
            stackLayout.Children.Add(item2);
            stackLayout.Children.Add(item);
            viewCell.View = stackLayout;
            this.TSection.Add(viewCell);
            this.Values.Add(item);
            this.Names.Add(item2);
        }

        /*// Token: 0x060000B5 RID: 181 RVA: 0x00007444 File Offset: 0x00005644
        [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private void InitializeComponent()
        {
            if (ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
            {
                AssemblyName = typeof(taskCreate_Page).GetTypeInfo().Assembly.GetName(),
                ResourcePath = "Pages/TaskCreate_Page.xaml"
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
            Picker picker = new Picker();
            Button button = new Button();
            Button button2 = new Button();
            StackLayout stackLayout = new StackLayout();
            ViewCell viewCell = new ViewCell();
            Button button3 = new Button();
            Button button4 = new Button();
            StackLayout stackLayout2 = new StackLayout();
            ViewCell viewCell2 = new ViewCell();
            TableSection tableSection = new TableSection();
            TableRoot tableRoot = new TableRoot();
            TableView tableView = new TableView();
            NameScope nameScope = new NameScope();
            NameScope.SetNameScope(this, nameScope);
            ((INameScope)nameScope).RegisterName("TSection", tableSection);
            ((INameScope)nameScope).RegisterName("templatePicker", picker);
            if (picker.StyleId == null)
            {
                picker.StyleId = "templatePicker";
            }
            this.TSection = tableSection;
            this.templatePicker = picker;
            tableSection.SetValue(TableSectionBase.TitleProperty, "Creating Task");
            stackLayout.SetValue(StackLayout.OrientationProperty, StackOrientation.Horizontal);
            picker.SetValue(Picker.TitleProperty, "Select Template");
            stackLayout.Children.Add(picker);
            button.SetValue(Button.TextProperty, "Load template");
            button.Clicked += this.onClickedLoadTemplate;
            stackLayout.Children.Add(button);
            button2.SetValue(Button.TextProperty, "Save Template");
            button2.Clicked += this.onClickedSaveTemplate;
            stackLayout.Children.Add(button2);
            viewCell.View = stackLayout;
            tableSection.Add(viewCell);
            stackLayout2.SetValue(StackLayout.OrientationProperty, StackOrientation.Horizontal);
            button3.SetValue(Button.TextProperty, "Create Task");
            button3.Clicked += this.onClickedCreate;
            stackLayout2.Children.Add(button3);
            button4.SetValue(Button.TextProperty, "Add Fields");
            button4.Clicked += this.onClickAddFields;
            stackLayout2.Children.Add(button4);
            viewCell2.View = stackLayout2;
            tableSection.Add(viewCell2);
            tableRoot.Add(tableSection);
            tableView.Root = tableRoot;
            this.SetValue(ContentPage.ContentProperty, tableView);
        }

        // Token: 0x060000B6 RID: 182 RVA: 0x000076A9 File Offset: 0x000058A9
        private void __InitComponentRuntime()
        {
            this.LoadFromXaml(typeof(taskCreate_Page));
            this.TSection = this.FindByName("TSection");
            this.templatePicker = this.FindByName("templatePicker");
        }

        // Token: 0x0400005F RID: 95
        

        // Token: 0x04000062 RID: 98
        private string saveAs = "";

        // Token: 0x04000063 RID: 99
        private string descAs = "";

        // Token: 0x04000064 RID: 100
        [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private TableSection TSection;

        // Token: 0x04000065 RID: 101
        [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private Picker templatePicker;*/
    }
}