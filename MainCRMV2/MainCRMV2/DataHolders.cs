using MainCRMV2;
using MainCRMV2.Pages;
using MainCRMV2.Pages.Customers;
using MainCRMV2.Pages.Inventory;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace MainCRMV2
{
    public class SecurityButton: DataButton
    {
        private string[] Locks;
        public SecurityButton(int i,string[] locks)
        {
            Locks = locks;
            Integer = i;
            SecurityAlpha();
        }
        public SecurityButton(string i, string[] locks)
        {
            Locks = locks;
            String = i;
            SecurityAlpha();
        }
        private bool checkKeys()
        {
            int i = 0;
            foreach(string s in Locks)
            {
                if (ClientData.hasSecurityKey(s))
                {
                    i++;
                }
            }
            if (i == Locks.Length)
            {
                return true;
            }
            return false;
        }
        private void SecurityAlpha()
        {
            if (!checkKeys())
            {
                IsEnabled = false;
                Text = "Unauthorized, Please see your supervisor";
            }
        }
    }
    public class DataButton : StyledButton
    {
        public DataButton()
        {

        }
        public DataButton(int i)
        {
            this.Integer = i;
        }
        public DataButton(string i)
        {
            this.String = i;
        }
        public int GetInt()
        {
            return this.Integer;
        }
        public int GetInt2()
        {
            return this.Integer2;
        }
        public string GetString()
        {
            return this.String;
        }
        public string GetString2()
        {
            return this.String2;
        }
        public int Integer;
        public int Integer2;
        public string String;
        public string String2;
    }
    public class StyledButton : Button
    {
        public StyledButton()
        {
            BackgroundColor = Color.AliceBlue;
            BorderColor = Color.Black;
            BorderWidth = 2;
            TextColor = Color.Black;
        }
    }
    public class DataEntry : Entry
    {
        public DataEntry(int i, string s)
        {
            this.Integer = i;
            this.InitValue = s;
        }
        public int GetInt()
        {
            return this.Integer;
        }
        public string GetInit()
        {
            return this.InitValue;
        }
        public int Integer;
        public string InitValue;
    }
    public class FlaggedDataPair : DataPair
    {
        public int Flag = 0;
        public FlaggedDataPair(int i, string I, string V, int j) : base(i, I, V)
        {
            Flag = j;
        }
    }
    public class DataPair
    {
        public DataPair(int i, string I, string V)
        {
            this.Index = new DataEntry(i, I);
            this.Value = new DataEntry(i, V);
        }
        public void setNew()
        {
            this.isNew = true;
        }
        public DataEntry Index;
        public DataEntry Value;
        public bool isNew;
    }
    public class DataDoubleSwitch : DataSwitch
    {
        public DataDoubleSwitch(int i, int j) : base(i)
        {
            Integer2 = j;
        }
        public int getSecondInt()
        {
            return Integer2;
        }
        public int Integer2;
    }
    public class DataSwitch : Switch
    {
        public DataSwitch(int i)
        {
            this.Integer = i;
        }
        public int GetInt()
        {
            return this.Integer;
        }
        public void setStartState(bool b)
        {
            base.IsToggled = b;
            this.StartState = b;
        }
        public bool hasChanged()
        {
            return base.IsToggled != this.StartState;
        }
        public int Integer;
        public bool StartState;
    }
    public class DataViewCell : ViewCell
    {
        public int Integer;
        public DataViewCell(int i) : base()
        {
            this.Integer = i;
        }

    }
    public class LinkPage : ContentPage
    {
        public LinkPage(string name)
        {
            base.Title = name;
            base.Content = new StackLayout
            {
                Children =
                {
                    new Image
                    {
                        Source = ImageSource.FromResource("MainCRMV2.Resources.background.png"),
                        Aspect = Aspect.AspectFill,
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        HorizontalOptions = LayoutOptions.FillAndExpand
                    }
                }
            };
        }
    }
    public class MainLink : StyledButton
    {
        public MainLink(string name)
        {
            base.Text = name;
            if (base.Text == "Tasks")
            {
                base.Command = new Command(delegate (object o)
                {
                    App.MDP.Detail = new NavigationPage(new Tasks_Page());
                    App.MDP.IsPresented = false;
                });
                return;
            }
            if (base.Text == "Customers")
            {
                base.Command = new Command(delegate (object o)
                {
                    App.MDP.Detail = new NavigationPage(new CustomerList_Page());
                    App.MDP.IsPresented = false;
                });
                return;
            }
            if (base.Text == "CDR")
            {
                base.Command = new Command(delegate (object o)
                {
                    App.MDP.Detail = new NavigationPage(new CDR_Page(true, ""));
                    App.MDP.IsPresented = false;
                });
                return;
            }
            if (base.Text == "Logout")
            {
                base.Command = new Command(delegate (object o)
                {
                    App.MDP.Detail = new NavigationPage(new Logout_Page());
                    App.MDP.IsPresented = false;
                });
                return;
            }
            if (base.Text == "Account")
            {
                base.Command = new Command(delegate (object o)
                {
                    App.MDP.Detail = new NavigationPage(new UserSettings_Page());
                    App.MDP.IsPresented = false;
                });
                return;
            }
            if (base.Text == "Chat")
            {
                base.Command = new Command(delegate (object o)
                {
                    App.MDP.Detail = new NavigationPage(new Chat_Page());
                    App.MDP.IsPresented = false;
                });
                return;
            }
            if (base.Text == "Coupon Checker")
            {
                base.Command = new Command(delegate (object o)
                {
                    App.MDP.Detail = new NavigationPage(new CouponChecker_Page());
                    App.MDP.IsPresented = false;
                });
                return;
            }
            if (base.Text == "Inventory")
            {
                base.Command = new Command(delegate (object o)
                {
                    App.MDP.Detail = new NavigationPage(new Items_Page());
                    App.MDP.IsPresented = false;
                });
                return;
            }
            if (base.Text == "Calls Received")
            {
                base.Command = new Command(delegate (object o)
                {
                    App.MDP.Detail = new NavigationPage(new Check_Page());
                    App.MDP.IsPresented = false;
                });
                return;
            }
            if (base.Text == "Price Guide")
            {
                base.Command = new Command(delegate (object o)
                {
                    App.MDP.Detail = new NavigationPage(new Price_Page());
                    App.MDP.IsPresented = false;
                });
                return;
            }
            base.Command = new Command(delegate (object o)
            {
                App.MDP.Detail = new NavigationPage(new LinkPage(name));
                App.MDP.IsPresented = false;
            }); 
        }
    }
    public class SubLink : StyledButton
    {
        public SubLink(string name)
        {
            base.Text = name;
            base.Command = new Command(delegate (object o)
            {
                App.MDP.Detail.Navigation.PushAsync(new LinkPage(name));
            });
        }
    }
    public class AwesomeHyperLinkLabel : Label
    {

    }
    public delegate void TaskCallback(string Result);
    public class RadioButtonCore :INotifyPropertyChanged{
        public RadioButtonCore()
        {
            MyList = new ObservableCollection<RadioButtonDataHandler>();
            FillData();
        }
        public IList<RadioButtonDataHandler> MyList { get; set; }
        private RadioButtonDataHandler _selectedItem;
        public RadioButtonDataHandler SelectedItem
        {
            get => _selectedItem;
            set { _selectedItem = value; OnPropertyChanged(); }
        }

        void FillData()
        {
            for (int i = 0; i < 6; i++)
            {
                MyList.Add(new RadioButtonDataHandler { Id = i, Name = "Option " + i });
            }
        }

        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string propName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        #endregion
    }
    public class RadioButtonDataHandler
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //Override string and return what you want to be displayed
        public override string ToString() => Name;
    }
}
