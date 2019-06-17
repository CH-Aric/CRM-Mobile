using MainCRMV2;
using MainCRMV2.Pages;
using MainCRMV2.Pages.Inventory;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MainCRMV2
{
    public class DataButton : Button
    {
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
        public string GetString()
        {
            return this.String;
        }
        public string GetString2()
        {
            return this.String2;
        }
        public int Integer;
        public string String;
        public string String2;
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
        public DataViewCell(int i) :base()
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
    public class MainLink : Button
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
            base.Command = new Command(delegate (object o)
            {
                App.MDP.Detail = new NavigationPage(new LinkPage(name));
                App.MDP.IsPresented = false;
            });
        }
    }
    public class SubLink : Button
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
    public delegate void TaskCallback(string Result);
}
