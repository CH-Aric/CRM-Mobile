using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MainCRMV2.Pages
{
    public partial class Chat_Page : ContentPage
    {
        private Dictionary<string, List<string>> pickerIndex;
        public Chat_Page()
        {
            InitializeComponent();
            getChatMessages();
            pickerIndex = new Dictionary<string, List<string>>();
            pickerIndex.Add("Name", new List<string>
            {
                "All"
            });
            pickerIndex.Add("g", new List<string>
            {
                "1"
            });
            pickerIndex.Add("IDKey", new List<string>
            {
                "0"
            });
            getFavoriteAgents();
            getFavoriteGroups();
        }

        // Token: 0x06000079 RID: 121 RVA: 0x00004C70 File Offset: 0x00002E70
        public void getChatMessages()
        {
            string statement = string.Concat(new object[]
            {
                "SELECT chat.Message,chat.Timestamp,agents.FName FROM chat INNER JOIN agents ON chat.AgentID=agents.IDKey WHERE chat.TargetID = '",
                ClientData.AgentIDK,
                "' OR chat.AgentID = '",
                ClientData.AgentIDK,
                "' OR 'Chat.Global' = '1' OR('chat.Global' = '2' AND chat.TargetID IN(SELECT IDKey FROM groupmembers WHERE MemberID = '",
                ClientData.AgentIDK,
                "')); "
            });
            TaskCallback call = new TaskCallback(this.populateChat);
            DatabaseFunctions.SendToPhp(false, statement, call);
        }

        // Token: 0x0600007A RID: 122 RVA: 0x00004CE8 File Offset: 0x00002EE8
        public void getFavoriteAgents()
        {
            string statement = "SELECT agents.Fname AS Name,agents.IDKey, '0' AS g FROM agents INNER JOIN chatfavorite ON agents.IDKey=chatfavorite.TargetID WHERE chatfavorite.AgentID='" + ClientData.AgentIDK + "';";
            TaskCallback call = new TaskCallback(this.populatePicker);
            DatabaseFunctions.SendToPhp(false, statement, call);
        }

        // Token: 0x0600007B RID: 123 RVA: 0x00004D24 File Offset: 0x00002F24
        public void getFavoriteGroups()
        {
            string statement = "SELECT IDKey,GroupName AS Name, '2' AS g FROM groupmembers WHERE MemberID='" + ClientData.AgentIDK + "';";
            TaskCallback call = new TaskCallback(this.populatePicker);
            DatabaseFunctions.SendToPhp(false, statement, call);
        }

        // Token: 0x0600007C RID: 124 RVA: 0x00004D60 File Offset: 0x00002F60
        public void populateChat(string result)
        {
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            if (dictionary.Count > 0)
            {
                for (int i = 0; i < dictionary["Message"].Count; i++)
                {
                    Label item = new Label
                    {
                        Text = string.Concat(new string[]
                        {
                            dictionary["Timestamp"][i],
                            ":",
                            dictionary["FName"][i],
                            ":",
                            dictionary["Message"][i]
                        }),
                        FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        HorizontalOptions = LayoutOptions.StartAndExpand
                    };
                    ChatStack.Children.Add(item);
                }
            }
        }

        // Token: 0x0600007D RID: 125 RVA: 0x00004E48 File Offset: 0x00003048
        public void populatePicker(string result)
        {
            string[] input = FormatFunctions.SplitToPairs(result);
            if (pickerIndex == null)
            {
                pickerIndex = FormatFunctions.createValuePairs(input);
            }
            else
            {
                Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(input);
                foreach (string key in dictionary.Keys)
                {
                    pickerIndex[key] = pickerIndex[key].Union(dictionary[key]).ToList<string>();
                }
            }
            Target.ItemsSource = pickerIndex["Name"];
        }

        // Token: 0x0600007E RID: 126 RVA: 0x00004EFC File Offset: 0x000030FC
        public void OnClickSendMsg(object sender, EventArgs e)
        {
            int num = int.Parse(DatabaseFunctions.lookupInDictionary((string)Target.SelectedItem, "Name", "IDKey", this.pickerIndex));
            int num2 = int.Parse(DatabaseFunctions.lookupInDictionary((string)Target.SelectedItem, "Name", "g", this.pickerIndex));
            string text = DateTime.Now.ToString("yyyy'-'MM'-'dd HH':'mm':'ss");
            DatabaseFunctions.SendToPhp(string.Concat(new object[]
            {
                "INSERT INTO chat (Message,AgentID,TargetID,Global,Timestamp) VALUES ('",
                this.Message.Text,
                "','",
                ClientData.AgentIDK,
                "','",
                num,
                "','",
                num2,
                "','",
                text,
                "')"
            }));
            this.getChatMessages();
        }

        // Token: 0x0600007F RID: 127 RVA: 0x00004FEC File Offset: 0x000031EC
        public void OnClickMan(object sender, EventArgs e)
        {
            App.MDP.Detail.Navigation.PushAsync(new Favorites_Page());
        }

        /*// Token: 0x06000080 RID: 128 RVA: 0x00005008 File Offset: 0x00003208
        [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private void InitializeComponent()
        {
            if (ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
            {
                AssemblyName = typeof(Chat_Page).GetTypeInfo().Assembly.GetName(),
                ResourcePath = "Pages/Chat_Page.xaml"
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
            StackLayout stackLayout = new StackLayout();
            ScrollView scrollView = new ScrollView();
            ViewCell viewCell = new ViewCell();
            Entry entry = new Entry();
            Picker picker = new Picker();
            Button button = new Button();
            StackLayout stackLayout2 = new StackLayout();
            ViewCell viewCell2 = new ViewCell();
            Button button2 = new Button();
            StackLayout stackLayout3 = new StackLayout();
            ViewCell viewCell3 = new ViewCell();
            TableSection tableSection = new TableSection();
            TableRoot tableRoot = new TableRoot();
            TableView tableView = new TableView();
            NameScope nameScope = new NameScope();
            NameScope.SetNameScope(this, nameScope);
            ((INameScope)nameScope).RegisterName("TSection", tableSection);
            ((INameScope)nameScope).RegisterName("ChatStack", stackLayout);
            if (stackLayout.StyleId == null)
            {
                stackLayout.StyleId = "ChatStack";
            }
            ((INameScope)nameScope).RegisterName("UIPrimary", stackLayout2);
            if (stackLayout2.StyleId == null)
            {
                stackLayout2.StyleId = "UIPrimary";
            }
            ((INameScope)nameScope).RegisterName("Message", entry);
            if (entry.StyleId == null)
            {
                entry.StyleId = "Message";
            }
            ((INameScope)nameScope).RegisterName("Target", picker);
            if (picker.StyleId == null)
            {
                picker.StyleId = "Target";
            }
            ((INameScope)nameScope).RegisterName("UISecondary", stackLayout3);
            if (stackLayout3.StyleId == null)
            {
                stackLayout3.StyleId = "UISecondary";
            }
            this.TSection = tableSection;
            this.ChatStack = stackLayout;
            this.UIPrimary = stackLayout2;
            this.Message = entry;
            this.Target = picker;
            this.UISecondary = stackLayout3;
            tableView.SetValue(TableView.HasUnevenRowsProperty, true);
            scrollView.SetValue(View.VerticalOptionsProperty, LayoutOptions.FillAndExpand);
            scrollView.SetValue(View.HorizontalOptionsProperty, LayoutOptions.FillAndExpand);
            stackLayout.SetValue(StackLayout.OrientationProperty, StackOrientation.Vertical);
            stackLayout.SetValue(View.VerticalOptionsProperty, LayoutOptions.FillAndExpand);
            stackLayout.SetValue(View.HorizontalOptionsProperty, LayoutOptions.FillAndExpand);
            scrollView.Content = stackLayout;
            viewCell.View = scrollView;
            tableSection.Add(viewCell);
            stackLayout2.SetValue(StackLayout.OrientationProperty, StackOrientation.Horizontal);
            entry.SetValue(Entry.PlaceholderProperty, "Message");
            stackLayout2.Children.Add(entry);
            picker.SetValue(Picker.TitleProperty, "Target");
            stackLayout2.Children.Add(picker);
            button.SetValue(Button.TextProperty, "Send");
            button.Clicked += this.OnClickSendMsg;
            stackLayout2.Children.Add(button);
            viewCell2.View = stackLayout2;
            tableSection.Add(viewCell2);
            stackLayout3.SetValue(StackLayout.OrientationProperty, StackOrientation.Horizontal);
            button2.SetValue(Button.TextProperty, "Manage Groups and Favourites");
            button2.Clicked += this.OnClickMan;
            stackLayout3.Children.Add(button2);
            viewCell3.View = stackLayout3;
            tableSection.Add(viewCell3);
            tableRoot.Add(tableSection);
            tableView.Root = tableRoot;
            this.SetValue(ContentPage.ContentProperty, tableView);
        }

        // Token: 0x06000081 RID: 129 RVA: 0x00005368 File Offset: 0x00003568
        private void __InitComponentRuntime()
        {
            this.LoadFromXaml(typeof(Chat_Page));
            this.TSection = this.FindByName("TSection");
            this.ChatStack = this.FindByName("ChatStack");
            this.UIPrimary = this.FindByName("UIPrimary");
            this.Message = this.FindByName("Message");
            this.Target = this.FindByName("Target");
            this.UISecondary = this.FindByName("UISecondary");
        }*/

    }
}
