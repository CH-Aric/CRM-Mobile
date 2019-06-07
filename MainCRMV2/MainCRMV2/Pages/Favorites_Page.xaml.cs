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

        // Token: 0x0400005A RID: 90
        private List<DataSwitch> Group;

        // Token: 0x0400005B RID: 91
        private Picker GroupSelector;

        // Token: 0x0400005C RID: 92
        private Entry GroupEntry;

        // Token: 0x0400005D RID: 93
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

        // Token: 0x06000097 RID: 151 RVA: 0x00006364 File Offset: 0x00004564
        public void getAgents()
        {
            string statement = "SELECT FName,IDKey FROM agents;";
            TaskCallback call = new TaskCallback(this.populateList);
            DatabaseFunctions.SendToPhp(false, statement, call);
        }

        // Token: 0x06000098 RID: 152 RVA: 0x0000638C File Offset: 0x0000458C
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

        // Token: 0x06000099 RID: 153 RVA: 0x000064E4 File Offset: 0x000046E4
        public void getFavorites()
        {
            string statement = "SELECT TargetID FROM chatfavorite WHERE AgentID='" + ClientData.AgentIDK + "'";
            TaskCallback call = new TaskCallback(this.populateFavorites);
            DatabaseFunctions.SendToPhp(false, statement, call);
        }

        // Token: 0x0600009A RID: 154 RVA: 0x00006520 File Offset: 0x00004720
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

        // Token: 0x0600009B RID: 155 RVA: 0x00006584 File Offset: 0x00004784
        public void getGroups()
        {
            string statement = "SELECT GroupName,GroupID FROM groupmembers WHERE Admin='1' AND MemberID='" + ClientData.AgentIDK + "';";
            TaskCallback call = new TaskCallback(this.populateGroups);
            DatabaseFunctions.SendToPhp(false, statement, call);
        }

        // Token: 0x0600009C RID: 156 RVA: 0x000065C0 File Offset: 0x000047C0
        public void populateGroups(string result)
        {
            string[] input = FormatFunctions.SplitToPairs(result);
            this.pickerIndex = FormatFunctions.createValuePairs(input);
            this.GroupSelector.ItemsSource = this.pickerIndex["GroupName"];
            this.GroupSelector.SelectedItem = 1;
        }

        // Token: 0x0600009D RID: 157 RVA: 0x0000660C File Offset: 0x0000480C
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

        // Token: 0x0600009E RID: 158 RVA: 0x0000686C File Offset: 0x00004A6C
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

        // Token: 0x0600009F RID: 159 RVA: 0x00006970 File Offset: 0x00004B70
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

        // Token: 0x060000A0 RID: 160 RVA: 0x00006A40 File Offset: 0x00004C40
        public void onClickCreateGroup(object sender, EventArgs e)
        {
            string statement = "SELECT GroupID FROM groupmembers ORDER BY GroupID DESC LIMIT 1";
            TaskCallback call = new TaskCallback(this.createGroup);
            DatabaseFunctions.SendToPhp(false, statement, call);
        }

        // Token: 0x060000A1 RID: 161 RVA: 0x00006A68 File Offset: 0x00004C68
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

        // Token: 0x060000A2 RID: 162 RVA: 0x00006AEC File Offset: 0x00004CEC
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

        // Token: 0x060000A3 RID: 163 RVA: 0x00006B54 File Offset: 0x00004D54
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

        /*// Token: 0x060000A4 RID: 164 RVA: 0x00006CB4 File Offset: 0x00004EB4
        [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private void InitializeComponent()
        {
            if (ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
            {
                AssemblyName = typeof(Favorites_Page).GetTypeInfo().Assembly.GetName(),
                ResourcePath = "Pages/Favorites_Page.xaml"
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
            Label label2 = new Label();
            Label label3 = new Label();
            StackLayout stackLayout = new StackLayout();
            ViewCell viewCell = new ViewCell();
            TableSection tableSection = new TableSection();
            TableRoot tableRoot = new TableRoot();
            TableView tableView = new TableView();
            NameScope nameScope = new NameScope();
            NameScope.SetNameScope(this, nameScope);
            ((INameScope)nameScope).RegisterName("TSection", tableSection);
            this.TSection = tableSection;
            tableSection.SetValue(TableSectionBase.TitleProperty, "Favorite's Management");
            stackLayout.SetValue(StackLayout.OrientationProperty, StackOrientation.Horizontal);
            label.SetValue(Label.TextProperty, "Agent");
            label.SetValue(View.HorizontalOptionsProperty, LayoutOptions.StartAndExpand);
            stackLayout.Children.Add(label);
            label2.SetValue(Label.TextProperty, "Fav");
            label2.SetValue(View.HorizontalOptionsProperty, LayoutOptions.CenterAndExpand);
            stackLayout.Children.Add(label2);
            label3.SetValue(Label.TextProperty, "Group");
            label3.SetValue(View.HorizontalOptionsProperty, LayoutOptions.EndAndExpand);
            stackLayout.Children.Add(label3);
            viewCell.View = stackLayout;
            tableSection.Add(viewCell);
            tableRoot.Add(tableSection);
            tableView.Root = tableRoot;
            this.SetValue(ContentPage.ContentProperty, tableView);
        }

        // Token: 0x060000A5 RID: 165 RVA: 0x00006E64 File Offset: 0x00005064
        private void __InitComponentRuntime()
        {
            this.LoadFromXaml(typeof(Favorites_Page));
            this.TSection = this.FindByName("TSection");
        }

        // Token: 0x04000059 RID: 89
        

        // Token: 0x0400005E RID: 94
        [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private TableSection TSection;
        */
    }
}
