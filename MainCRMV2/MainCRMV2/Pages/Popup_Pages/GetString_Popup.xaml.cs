using System;
using Rg.Plugins.Popup.Animations;
using Rg.Plugins.Popup.Converters.TypeConverters;
using Rg.Plugins.Popup.Enums;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MainCRMV2.Pages.Popup_Pages
{
    public partial class GetString_Popup : PopupPage
    {
        private TaskCallback call;
        public GetString_Popup(string labelText, string confirmText, string cancelText, TaskCallback c)
        {
            InitializeComponent();
            this.label.Text = labelText;
            this.button.Text = confirmText;
            this.cancelButton.Text = cancelText;
            base.Animation = new ScaleAnimation();
            this.BackgroundColor = Color.WhiteSmoke;
            this.call = c;
        }
        protected override bool OnBackButtonPressed()
        {
            this.call("Cancel");
            return base.OnBackButtonPressed();
        }
        public void onClicked(object sender, EventArgs e)
        {
            this.call(this.TextInput.Text);
        }
        public void onClickedCancel(object sender, EventArgs e)
        {
            this.call("Cancel");
        }

        /*// Token: 0x060000DD RID: 221 RVA: 0x000092D8 File Offset: 0x000074D8
        [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private void InitializeComponent()
        {
            if (ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
            {
                AssemblyName = typeof(GetString_Popup).GetTypeInfo().Assembly.GetName(),
                ResourcePath = "Pages/Popup_Pages/GetString_Popup.xaml"
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
            Entry entry = new Entry();
            Button button = new Button();
            Button button2 = new Button();
            StackLayout stackLayout = new StackLayout();
            NameScope nameScope = new NameScope();
            NameScope.SetNameScope(this, nameScope);
            ((INameScope)nameScope).RegisterName("label", label);
            if (label.StyleId == null)
            {
                label.StyleId = "label";
            }
            ((INameScope)nameScope).RegisterName("TextInput", entry);
            if (entry.StyleId == null)
            {
                entry.StyleId = "TextInput";
            }
            ((INameScope)nameScope).RegisterName("button", button);
            if (button.StyleId == null)
            {
                button.StyleId = "button";
            }
            ((INameScope)nameScope).RegisterName("cancelButton", button2);
            if (button2.StyleId == null)
            {
                button2.StyleId = "cancelButton";
            }
            this.label = label;
            this.TextInput = entry;
            this.button = button;
            this.cancelButton = button2;
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
            stackLayout.Children.Add(entry);
            button.SetValue(Button.TextProperty, "Submit");
            button.Clicked += this.onClicked;
            stackLayout.Children.Add(button);
            button2.SetValue(Button.TextProperty, "Cancel");
            button2.Clicked += this.onClickedCancel;
            stackLayout.Children.Add(button2);
            this.SetValue(ContentPage.ContentProperty, stackLayout);
        }

        // Token: 0x060000DE RID: 222 RVA: 0x000095C4 File Offset: 0x000077C4
        private void __InitComponentRuntime()
        {
            this.LoadFromXaml(typeof(GetString_Popup));
            this.label = this.FindByName("label");
            this.TextInput = this.FindByName("TextInput");
            this.button = this.FindByName("button");
            this.cancelButton = this.FindByName("cancelButton");
        }

        // Token: 0x0400009F RID: 159
        

        // Token: 0x040000A0 RID: 160
        [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private Label label;

        // Token: 0x040000A1 RID: 161
        [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private Entry TextInput;

        // Token: 0x040000A2 RID: 162
        [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private Button button;

        // Token: 0x040000A3 RID: 163
        [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private Button cancelButton;*/
    }
}
