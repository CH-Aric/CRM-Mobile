using System;
using System.CodeDom.Compiler;
using System.Reflection;
using Rg.Plugins.Popup.Animations;
using Rg.Plugins.Popup.Converters.TypeConverters;
using Rg.Plugins.Popup.Enums;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Internals;

namespace MainCRMV2.Pages.Popup_Pages
{
    // Token: 0x02000032 RID: 50
    public partial class Notification_Popup : PopupPage
    {
        private TaskCallback call;
        public Notification_Popup(string labelText, string confirmText, TaskCallback c)
        {
            InitializeComponent();
            this.label.Text = labelText;
            this.button.Text = confirmText;
            base.Animation = new ScaleAnimation();
            this.BackgroundColor = Color.WhiteSmoke;
            this.call = c;
        }

        // Token: 0x060000E0 RID: 224 RVA: 0x0000965E File Offset: 0x0000785E
        protected override bool OnBackButtonPressed()
        {
            this.call("Cancel");
            return base.OnBackButtonPressed();
        }

        // Token: 0x060000E1 RID: 225 RVA: 0x00009676 File Offset: 0x00007876
        public void onClicked(object sender, EventArgs e)
        {
            this.call("Done");
        }

        /*// Token: 0x060000E2 RID: 226 RVA: 0x00009688 File Offset: 0x00007888
        [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private void InitializeComponent()
        {
            if (ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
            {
                AssemblyName = typeof(Notification_Popup).GetTypeInfo().Assembly.GetName(),
                ResourcePath = "Pages/Popup_Pages/Notification_Popup.xaml"
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
            ScaleAnimation scaleAnimation = new ScaleAnimation();
            Label label = new Label();
            Button button = new Button();
            StackLayout stackLayout = new StackLayout();
            NameScope nameScope = new NameScope();
            NameScope.SetNameScope(this, nameScope);
            ((INameScope)nameScope).RegisterName("label", label);
            if (label.StyleId == null)
            {
                label.StyleId = "label";
            }
            ((INameScope)nameScope).RegisterName("button", button);
            if (button.StyleId == null)
            {
                button.StyleId = "button";
            }
            this.label = label;
            this.button = button;
            scaleAnimation.PositionIn = MoveAnimationOptions.Center;
            scaleAnimation.PositionOut = MoveAnimationOptions.Center;
            scaleAnimation.ScaleIn = 1.2;
            scaleAnimation.ScaleOut = 0.8;
            scaleAnimation.DurationIn = (uint)new UintTypeConverter().ConvertFromInvariantString("400");
            scaleAnimation.DurationOut = (uint)new UintTypeConverter().ConvertFromInvariantString("300");
            scaleAnimation.EasingIn = new EasingTypeConverter().ConvertFromInvariantString("SinOut");
            scaleAnimation.EasingOut = new EasingTypeConverter().ConvertFromInvariantString("SinIn");
            scaleAnimation.HasBackgroundAnimation = true;
            this.SetValue(PopupPage.AnimationProperty, scaleAnimation);
            stackLayout.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
            stackLayout.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
            stackLayout.SetValue(Xamarin.Forms.Layout.PaddingProperty, new Thickness(20.0, 20.0, 20.0, 20.0));
            label.SetValue(Label.TextProperty, "What do you want to say?");
            stackLayout.Children.Add(label);
            button.SetValue(Button.TextProperty, "Submit");
            button.Clicked += this.onClicked;
            stackLayout.Children.Add(button);
            this.SetValue(ContentPage.ContentProperty, stackLayout);
        }

        // Token: 0x060000E3 RID: 227 RVA: 0x000098C9 File Offset: 0x00007AC9
        private void __InitComponentRuntime()
        {
            this.LoadFromXaml(typeof(Notification_Popup));
            this.label = this.FindByName("label");
            this.button = this.FindByName("button");
        }

        // Token: 0x040000A4 RID: 164
        

        // Token: 0x040000A5 RID: 165
        [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private Label label;

        // Token: 0x040000A6 RID: 166
        [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private Button button;*/
    }
}
