using System;
using System.Collections.Generic;
using MainCRMV2.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MainCRMV2.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Tasks_Page : ContentPage
    {
        private List<ViewCell> views;
        private Dictionary<string, List<string>> dict;
        private Dictionary<string, DataButton> buttonDict;
        private List<DataSwitch> switchDict;
        private Dictionary<string, List<string>> agents;
        private Dictionary<string, List<string>> groups;
        private bool gMode;
        public Tasks_Page()
        {
            InitializeComponent();
            this.loadFromDatabase();
        }
        private async void Clicked_Task(object sender, EventArgs e)
        {
            await Application.Current.MainPage.Navigation.PopAsync();
        }
        public void loadFromDatabase()
        {
            TaskCallback call = new TaskCallback(this.populateList);
            TaskCallback call2 = new TaskCallback(this.populateAgents);
            TaskCallback call3 = new TaskCallback(this.populateGroups);
            DatabaseFunctions.SendToPhp(false, string.Concat(new object[]
            {
                "SELECT * FROM Tasks WHERE AgentID='",
                ClientData.AgentIDK,
                "' OR GroupID IN(SELECT GroupID FROM crm2.groupmembers WHERE MemberID='",
                ClientData.AgentIDK,
                "');"
            }), call);
            DatabaseFunctions.SendToPhp(false, "SELECT FName,IDKey FROM agents WHERE Active='1';", call2);
            DatabaseFunctions.SendToPhp(false, "SELECT GroupName,GroupID FROM groupmembers WHERE MemberID='" + ClientData.AgentIDK + "';", call3);
        }
        public void populateList(string result)
        {
            string[] input = FormatFunctions.SplitToPairs(result);
            this.dict = FormatFunctions.createValuePairs(input);
            this.views = new List<ViewCell>();
            if (this.dict.Count > 1)
            {
                this.buttonDict = new Dictionary<string, DataButton>();
                this.switchDict = new List<DataSwitch>();
                for (int i = 0; i < this.dict["Name"].Count; i++)
                {
                    if (!this.buttonDict.ContainsKey(this.dict["IDKey"][i]))
                    {
                        string text = this.dict["Name"][i] ?? "";
                        DataButton dataButton = new DataButton(int.Parse(this.dict["IDKey"][i]))
                        {
                            Text = text,
                            FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                            VerticalOptions = LayoutOptions.CenterAndExpand,
                            HorizontalOptions = LayoutOptions.EndAndExpand
                        };
                        DataSwitch item = new DataSwitch(int.Parse(this.dict["IDKey"][i]))
                        {
                            VerticalOptions = LayoutOptions.CenterAndExpand,
                            HorizontalOptions = LayoutOptions.StartAndExpand
                        };
                        dataButton.Clicked += this.onClicked;
                        this.buttonDict.Add(this.dict["IDKey"][i], dataButton);
                        this.switchDict.Add(item);
                        ViewCell viewCell = new ViewCell();
                        StackLayout stackLayout = new StackLayout
                        {
                            Orientation = StackOrientation.Horizontal
                        };
                        stackLayout.Children.Add(item);
                        stackLayout.Children.Add(dataButton);
                        viewCell.View = stackLayout;
                        this.views.Add(viewCell);
                        TSection.Add(viewCell);
                    }
                    else
                    {
                        DataButton dataButton2 = this.buttonDict[this.dict["IDKey"][i]];
                        dataButton2.Text = dataButton2.Text + ", " + this.dict["Value"][i];
                    }
                }
            }
        }
        public void populateAgents(string result)
        {
            string[] input = FormatFunctions.SplitToPairs(result);
            agents = FormatFunctions.createValuePairs(input);
            agentPicker.ItemsSource = this.agents["FName"];
        }
        public void populateGroups(string result)
        {
            string[] input = FormatFunctions.SplitToPairs(result);
            this.groups = FormatFunctions.createValuePairs(input);
        }
        public void onClicked(object sender, EventArgs e)
        {
            DataButton dataButton = (DataButton)sender;
            App.MDP.Detail.Navigation.PushAsync(new TaskEdit_Page(dataButton.Integer));
            App.MDP.IsPresented = false;
        }
        public void onClickedAssign(object sender, EventArgs e)
        {
            string text;
            if (!this.gMode)
            {
                int num = int.Parse(DatabaseFunctions.lookupInDictionary((string)this.agentPicker.SelectedItem, "FName", "IDKey", this.agents));
                text = "UPDATE tasks SET AgentID='" + num + "' , GroupID='0' WHERE";
                foreach (DataSwitch dataSwitch in this.switchDict)
                {
                    if (dataSwitch.hasChanged())
                    {
                        text = string.Concat(new object[]
                        {
                            text,
                            " IDKey='",
                            dataSwitch.GetInt(),
                            "' OR"
                        });
                    }
                }
                text += " IDKey='-1'";
            }
            else
            {
                int num = int.Parse(DatabaseFunctions.lookupInDictionary((string)this.agentPicker.SelectedItem, "GroupName", "GroupID", this.groups));
                text = "UPDATE tasks SET GroupID='" + num + "', AgentID='0' WHERE";
                foreach (DataSwitch dataSwitch2 in this.switchDict)
                {
                    if (dataSwitch2.hasChanged())
                    {
                        text = string.Concat(new object[]
                        {
                            text,
                            " IDKey='",
                            dataSwitch2.GetInt(),
                            "' OR"
                        });
                    }
                }
                text += " IDKey='-1'";
            }
            DatabaseFunctions.SendToPhp(text);
        }
        public void onClickedCreate(object sender, EventArgs e)
        {
            App.MDP.Detail.Navigation.PushAsync(new TaskCreate_Page());
            App.MDP.IsPresented = false;
        }
        public void onClickedSearch(object sender, EventArgs e)
        {
            this.PurgeCells();
            string text = "%" + SearchEntry.Text + "%";
            string statement = string.Concat(new object[]
            {
                "SELECT DISTINCT tasks.IDKey,tasks.Name FROM tasks INNER JOIN taskfields ON tasks.IDKey=taskfields.TaskID  WHERE tasks.AgentID='",
                ClientData.AgentIDK,
                "' AND ( tasks.Name LIKE '",
                text,
                "' OR taskfields.Value LIKE '",
                text,
                "');"
            });
            TaskCallback call = new TaskCallback(this.populateList);
            DatabaseFunctions.SendToPhp(false, statement, call);
        }
        public void PurgeCells()
        {
            foreach (ViewCell item in this.views)
            {
                this.TSection.Remove(item);
            }
        }
        public void onToggledGroup(object sender, EventArgs e)
        {
            this.gMode = !this.gMode;
            if (this.gMode)
            {
                this.agentPicker.Title = "Select Group";
                this.agentPicker.ItemsSource = this.groups["GroupName"];
                return;
            }
            this.agentPicker.Title = "Select Agent";
            this.agentPicker.ItemsSource = this.agents["FName"];
        }
        /*
        // Token: 0x06000068 RID: 104 RVA: 0x00003FD8 File Offset: 0x000021D8
        [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private void InitializeComponent()
        {
            if (ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
            {
                AssemblyName = typeof(Tasks_Page).GetTypeInfo().Assembly.GetName(),
                ResourcePath = "Pages/Tasks_Page.xaml"
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
            Switch @switch = new Switch();
            Picker picker = new Picker();
            Button button = new Button();
            Button button2 = new Button();
            StackLayout stackLayout = new StackLayout();
            ViewCell viewCell = new ViewCell();
            Entry entry = new Entry();
            Button button3 = new Button();
            StackLayout stackLayout2 = new StackLayout();
            ViewCell viewCell2 = new ViewCell();
            TableSection tableSection = new TableSection();
            TableRoot tableRoot = new TableRoot();
            TableView tableView = new TableView();
            NameScope nameScope = new NameScope();
            NameScope.SetNameScope(this, nameScope);
            ((INameScope)nameScope).RegisterName("TSection", tableSection);
            ((INameScope)nameScope).RegisterName("G", @switch);
            if (@switch.StyleId == null)
            {
                @switch.StyleId = "G";
            }
            ((INameScope)nameScope).RegisterName("agentPicker", picker);
            if (picker.StyleId == null)
            {
                picker.StyleId = "agentPicker";
            }
            ((INameScope)nameScope).RegisterName("SearchEntry", entry);
            if (entry.StyleId == null)
            {
                entry.StyleId = "SearchEntry";
            }
            this.TSection = tableSection;
            this.G = @switch;
            this.agentPicker = picker;
            this.SearchEntry = entry;
            tableSection.SetValue(TableSectionBase.TitleProperty, "Task List");
            stackLayout.SetValue(StackLayout.OrientationProperty, StackOrientation.Horizontal);
            @switch.Toggled += new EventHandler<ToggledEventArgs>(this.onToggledGroup);
            stackLayout.Children.Add(@switch);
            picker.SetValue(Picker.TitleProperty, "Select Agent");
            picker.SetValue(View.HorizontalOptionsProperty, LayoutOptions.FillAndExpand);
            stackLayout.Children.Add(picker);
            button.SetValue(Button.TextProperty, "Transfer");
            button.Clicked += this.onClickedAssign;
            stackLayout.Children.Add(button);
            button2.SetValue(Button.TextProperty, "Create");
            button2.Clicked += this.onClickedCreate;
            stackLayout.Children.Add(button2);
            viewCell.View = stackLayout;
            tableSection.Add(viewCell);
            stackLayout2.SetValue(StackLayout.OrientationProperty, StackOrientation.Horizontal);
            entry.SetValue(Entry.PlaceholderProperty, "Search");
            entry.SetValue(View.HorizontalOptionsProperty, LayoutOptions.FillAndExpand);
            stackLayout2.Children.Add(entry);
            button3.SetValue(Button.TextProperty, "Search");
            button3.Clicked += this.onClickedSearch;
            stackLayout2.Children.Add(button3);
            viewCell2.View = stackLayout2;
            tableSection.Add(viewCell2);
            tableRoot.Add(tableSection);
            tableView.Root = tableRoot;
            this.SetValue(ContentPage.ContentProperty, tableView);
        }

        // Token: 0x06000069 RID: 105 RVA: 0x000042D4 File Offset: 0x000024D4
        private void __InitComponentRuntime()
        {
            this.LoadFromXaml(typeof(Tasks_Page));
            this.TSection = this.FindByName("TSection");
            this.G = this.FindByName("G");
            this.agentPicker = this.FindByName("agentPicker");
            this.SearchEntry = this.FindByName("SearchEntry");
        }

        // Token: 0x04000036 RID: 54
        

        // Token: 0x0400003D RID: 61
        [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private TableSection TSection;

        // Token: 0x0400003E RID: 62
        [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private Switch G;

        // Token: 0x0400003F RID: 63
        [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private Picker agentPicker;

        // Token: 0x04000040 RID: 64
        [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private Entry SearchEntry;*/
    }
}
