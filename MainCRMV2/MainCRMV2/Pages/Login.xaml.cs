using System;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Internals;

namespace MainCRMV2.Pages
{
    // Token: 0x02000018 RID: 24
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();
        }
        private void Button_Clicked(object sender, EventArgs e)
        {
        }
        public void LoginClick(object sender, EventArgs e)
        {
            TaskAwaiter<bool> taskAwaiter = DatabaseFunctions.client.attemptNewLogin(this.userN.Text, this.pass.Text, this.stay.IsToggled).GetAwaiter();
            while (!taskAwaiter.IsCompleted)
            {

            }
            if (taskAwaiter.GetResult())
            {
                App.MDP = new MasterDetailPage
                {
                    Master = new Home(),
                    Detail = new NavigationPage(new LinkPage("A"))
                };
                Application.Current.MainPage = App.MDP;
            }
            else
            {
                Message.Text = "Username or Password incorrect, Try again, or contact Support.";
            }
        }

        /*// Token: 0x06000053 RID: 83 RVA: 0x00003364 File Offset: 0x00001564
        [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private void InitializeComponent()
        {
            if (ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
            {
                AssemblyName = typeof(Login).GetTypeInfo().Assembly.GetName(),
                ResourcePath = "Pages/Login.xaml"
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
            Entry entry = new Entry();
            Label label2 = new Label();
            Entry entry2 = new Entry();
            Button button = new Button();
            Label label3 = new Label();
            Switch @switch = new Switch();
            Label label4 = new Label();
            StackLayout stackLayout = new StackLayout();
            NameScope nameScope = new NameScope();
            NameScope.SetNameScope(this, nameScope);
            ((INameScope)nameScope).RegisterName("userN", entry);
            if (entry.StyleId == null)
            {
                entry.StyleId = "userN";
            }
            ((INameScope)nameScope).RegisterName("pass", entry2);
            if (entry2.StyleId == null)
            {
                entry2.StyleId = "pass";
            }
            ((INameScope)nameScope).RegisterName("stay", @switch);
            if (@switch.StyleId == null)
            {
                @switch.StyleId = "stay";
            }
            ((INameScope)nameScope).RegisterName("Message", label4);
            if (label4.StyleId == null)
            {
                label4.StyleId = "Message";
            }
            this.userN = entry;
            this.pass = entry2;
            this.stay = @switch;
            this.Message = label4;
            stackLayout.SetValue(View.VerticalOptionsProperty, LayoutOptions.StartAndExpand);
            label.SetValue(Label.TextProperty, "Username:");
            stackLayout.Children.Add(label);
            entry.SetValue(Entry.PlaceholderProperty, "Username");
            stackLayout.Children.Add(entry);
            label2.SetValue(Label.TextProperty, "Password:");
            stackLayout.Children.Add(label2);
            entry2.SetValue(Entry.IsPasswordProperty, true);
            stackLayout.Children.Add(entry2);
            button.SetValue(Button.TextProperty, "Login");
            button.Clicked += this.LoginClick;
            stackLayout.Children.Add(button);
            label3.SetValue(Label.TextProperty, "Stay Logged In:");
            stackLayout.Children.Add(label3);
            @switch.SetValue(Switch.IsToggledProperty, true);
            stackLayout.Children.Add(@switch);
            label4.SetValue(Label.TextProperty, "messages go here");
            stackLayout.Children.Add(label4);
            this.SetValue(ContentPage.ContentProperty, stackLayout);
        }

        // Token: 0x06000054 RID: 84 RVA: 0x000035F4 File Offset: 0x000017F4
        private void __InitComponentRuntime()
        {
            this.LoadFromXaml(typeof(Login));
            this.userN = this.FindByName("userN");
            this.pass = this.FindByName("pass");
            this.stay = this.FindByName("stay");
            this.Message = this.FindByName("Message");
        }

        // Token: 0x0400002E RID: 46
        [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private Entry userN;

        // Token: 0x0400002F RID: 47
        [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private Entry pass;

        // Token: 0x04000030 RID: 48
        [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private Switch stay;

        // Token: 0x04000031 RID: 49
        [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private Label Message;*/
    }
}
