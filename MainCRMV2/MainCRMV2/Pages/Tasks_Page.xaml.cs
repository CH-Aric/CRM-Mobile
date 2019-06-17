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
                        buttonDict.Add(this.dict["IDKey"][i], dataButton);
                        switchDict.Add(item);
                        ViewCell viewCell = new ViewCell();
                        StackLayout stackLayout = new StackLayout { Orientation = StackOrientation.Horizontal };
                        stackLayout.Children.Add(item);
                        stackLayout.Children.Add(dataButton);
                        viewCell.View = stackLayout;
                        views.Add(viewCell);
                        TSection.Add(viewCell);
                    }
                    else
                    {
                        DataButton dataButton2 = buttonDict[dict["IDKey"][i]];
                        dataButton2.Text = dataButton2.Text + ", " + dict["Value"][i];
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
            groups = FormatFunctions.createValuePairs(input);
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
                int num = int.Parse(DatabaseFunctions.lookupInDictionary((string)agentPicker.SelectedItem, "FName", "IDKey", agents));
                text = "UPDATE tasks SET AgentID='" + num + "' , GroupID='0' WHERE";
                foreach (DataSwitch dataSwitch in switchDict)
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
                int num = int.Parse(DatabaseFunctions.lookupInDictionary((string)agentPicker.SelectedItem, "GroupName", "GroupID", groups));
                text = "UPDATE tasks SET GroupID='" + num + "', AgentID='0' WHERE";
                foreach (DataSwitch dataSwitch2 in switchDict)
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
            TaskCallback call = new TaskCallback(populateList);
            DatabaseFunctions.SendToPhp(false, statement, call);
        }
        public void PurgeCells()
        {
            foreach (ViewCell item in views)
            {
                this.TSection.Remove(item);
            }
        }
        public void onToggledGroup(object sender, EventArgs e)
        {
            gMode = !gMode;
            if (gMode)
            {
               agentPicker.Title = "Select Group";
               agentPicker.ItemsSource = groups["GroupName"];
                return;
            }
            agentPicker.Title = "Select Agent";
            agentPicker.ItemsSource = agents["FName"];
        }
    }
}
