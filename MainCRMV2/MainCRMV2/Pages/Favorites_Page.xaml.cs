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
    public partial class Favorites_Page : ContentPage
    {
        private List<DataSwitch> Favorites;
        private List<DataSwitch> Group;
        private Picker GroupSelector;
        private Entry GroupEntry;
        private IDictionary<string, List<string>> pickerIndex;
        public Favorites_Page()
        {
            InitializeComponent();
            this.Favorites = new List<DataSwitch>();
            this.Group = new List<DataSwitch>();
            this.getAgents();
            this.getFavorites();
            this.renderLowerUI();
            this.getGroups();
        }
        public void getAgents()
        {
            string statement = "SELECT FName,IDKey FROM agents;";
            TaskCallback call = new TaskCallback(this.populateList);
            DatabaseFunctions.SendToPhp(false, statement, call);
        }
        public void populateList(string result)
        {
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            for (int i = 0; i < dictionary["FName"].Count; i++)
            {
                string text = dictionary["FName"][i] ?? "";
                Label item = new Label
                {
                    Text = (text ?? ""),
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.StartAndExpand
                };
                DataSwitch item2 = new DataSwitch(int.Parse(dictionary["IDKey"][i]))
                {
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.EndAndExpand
                };
                DataSwitch item3 = new DataSwitch(int.Parse(dictionary["IDKey"][i]))
                {
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.CenterAndExpand
                };
                ViewCell viewCell = new ViewCell();
                StackLayout stackLayout = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal
                };
                stackLayout.Children.Add(item);
                stackLayout.Children.Add(item2);
                stackLayout.Children.Add(item3);
                viewCell.View = stackLayout;
                this.TSection.Add(viewCell);
                this.Favorites.Add(item2);
                this.Group.Add(item3);
            }
        }
        public void getFavorites()
        {
            string statement = "SELECT TargetID FROM chatfavorite WHERE AgentID='" + ClientData.AgentIDK + "'";
            TaskCallback call = new TaskCallback(this.populateFavorites);
            DatabaseFunctions.SendToPhp(false, statement, call);
        }
        public void populateFavorites(string result)
        {
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            if (dictionary.Count > 0)
            {
                for (int i = 0; i < dictionary["TargetID"].Count; i++)
                {
                    DatabaseFunctions.findDataSwitchinList(this.Favorites, int.Parse(dictionary["TargetID"][i])).setStartState(true);
                }
            }
        }
        public void getGroups()
        {
            string statement = "SELECT GroupName,GroupID FROM groupmembers WHERE Admin='1' AND MemberID='" + ClientData.AgentIDK + "';";
            TaskCallback call = new TaskCallback(this.populateGroups);
            DatabaseFunctions.SendToPhp(false, statement, call);
        }
        public void populateGroups(string result)
        {
            string[] input = FormatFunctions.SplitToPairs(result);
            this.pickerIndex = FormatFunctions.createValuePairs(input);
            this.GroupSelector.ItemsSource = this.pickerIndex["GroupName"];
            this.GroupSelector.SelectedItem = 1;
        }
        public void renderLowerUI()
        {
            ViewCell viewCell = new ViewCell();
            ViewCell viewCell2 = new ViewCell();
            ViewCell viewCell3 = new ViewCell();
            StackLayout stackLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal
            };
            StackLayout stackLayout2 = new StackLayout
            {
                Orientation = StackOrientation.Horizontal
            };
            StackLayout stackLayout3 = new StackLayout
            {
                Orientation = StackOrientation.Horizontal
            };
            viewCell.View = stackLayout;
            viewCell2.View = stackLayout2;
            viewCell3.View = stackLayout3;
            this.GroupSelector = new Picker
            {
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            Button button = new Button
            {
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                Text = "Add"
            };
            button.Clicked += this.onClickAddToGroup;
            Button button2 = new Button
            {
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                Text = "Remove"
            };
            button2.Clicked += this.onClickRemoveFromGroup;
            Button button3 = new Button
            {
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                Text = "Create"
            };
            button3.Clicked += this.onClickCreateGroup;
            Button button4 = new Button
            {
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                Text = "Leave"
            };
            button4.Clicked += this.onClickDeleteGroup;
            Button button5 = new Button
            {
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                Text = "Save Changes To Favorites"
            };
            button5.Clicked += this.onClickSaveFavorite;
            this.GroupEntry = new Entry
            {
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                Placeholder = "Group Name",
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            stackLayout.Children.Add(this.GroupSelector);
            stackLayout.Children.Add(button);
            stackLayout.Children.Add(button2);
            stackLayout2.Children.Add(this.GroupEntry);
            stackLayout2.Children.Add(button3);
            stackLayout2.Children.Add(button4);
            stackLayout3.Children.Add(button5);
            this.TSection.Add(viewCell3);
            this.TSection.Add(viewCell);
            this.TSection.Add(viewCell2);
        }
        public void onClickAddToGroup(object sender, EventArgs e)
        {
            string text = "INSERT INTO groupmembers (GroupName,GroupID,MemberID) VALUES ";
            int num = 0;
            foreach (DataSwitch dataSwitch in this.Group)
            {
                if (dataSwitch.IsToggled)
                {
                    num++;
                    text = string.Concat(new object[]
                    {
                        text,
                        "('",
                        this.GroupSelector.SelectedItem,
                        "','",
                        this.pickerIndex["GroupID"][this.GroupSelector.SelectedIndex],
                        "','",
                        dataSwitch.GetInt(),
                        "'), "
                    });
                }
            }
            if (num > 0)
            {
                text = text.TrimEnd(new char[]
                {
                    ' '
                });
                text = text.TrimEnd(new char[]
                {
                    ','
                });
                DatabaseFunctions.SendToPhp(text);
            }
        }
        public void onClickRemoveFromGroup(object sender, EventArgs e)
        {
            string text = "DELETE FROM groupmembers WHERE ";
            int num = 0;
            foreach (DataSwitch dataSwitch in this.Group)
            {
                if (dataSwitch.IsToggled)
                {
                    num++;
                    text = string.Concat(new object[]
                    {
                        text,
                        "(GroupID='",
                        this.pickerIndex["GroupID"][this.GroupSelector.SelectedIndex],
                        "' AND MemberID='",
                        dataSwitch.GetInt(),
                        "') OR "
                    });
                }
            }
            if (num > 0)
            {
                text += "GroupID='-1'";
                DatabaseFunctions.SendToPhp(text);
            }
        }
        public void onClickCreateGroup(object sender, EventArgs e)
        {
            string statement = "SELECT GroupID FROM groupmembers ORDER BY GroupID DESC LIMIT 1";
            TaskCallback call = new TaskCallback(this.createGroup);
            DatabaseFunctions.SendToPhp(false, statement, call);
        }
        public void createGroup(string result)
        {
            int num = int.Parse(FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result))["GroupID"][0]) + 1;
            DatabaseFunctions.SendToPhp(string.Concat(new object[]
            {
                "INSERT INTO groupmembers (MemberID,Admin,GroupName,GroupID) VALUES ('",
                ClientData.AgentIDK,
                "','1','",
                this.GroupEntry.Text,
                "','",
                num,
                "');"
            }));
        }
        public void onClickDeleteGroup(object sender, EventArgs e)
        {
            DatabaseFunctions.SendToPhp(string.Concat(new object[]
            {
                "DELETE FROM groupmembers WHERE (GroupID='",
                this.pickerIndex["GroupID"][this.GroupSelector.SelectedIndex],
                "' AND MemberID='",
                ClientData.AgentIDK,
                "')"
            }));
        }
        public void onClickSaveFavorite(object sender, EventArgs e)
        {
            string text = "INSERT INTO chatfavorite (TargetID,AgentID) VALUES ";
            string text2 = "DELETE FROM chatfavorite WHERE ";
            int num = 0;
            int num2 = 0;
            foreach (DataSwitch dataSwitch in this.Favorites)
            {
                if (dataSwitch.IsToggled && dataSwitch.hasChanged())
                {
                    num++;
                    text = string.Concat(new object[]
                    {
                        text,
                        "('",
                        dataSwitch.GetInt(),
                        "','",
                        ClientData.AgentIDK,
                        "'), "
                    });
                }
                else if (!dataSwitch.IsToggled && dataSwitch.hasChanged())
                {
                    num2++;
                    text2 = string.Concat(new object[]
                    {
                        text2,
                        "(TargetID='",
                        dataSwitch.GetInt(),
                        "' AND AgentID='",
                        ClientData.AgentIDK,
                        "') OR "
                    });
                }
            }
            if (num > 0)
            {
                text = text.TrimEnd(new char[]
                {
                    ' '
                });
                text = text.TrimEnd(new char[]
                {
                    ','
                });
                DatabaseFunctions.SendToPhp(text);
            }
            if (num2 > 0)
            {
                text2 += " AgentID=-1";
                DatabaseFunctions.SendToPhp(text2);
            }
        }
    }
}
