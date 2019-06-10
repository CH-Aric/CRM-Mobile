using System;
using System.CodeDom.Compiler;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Internals;

namespace MainCRMV2.Pages.Popup_Pages
{
    // Token: 0x02000033 RID: 51
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page1 : ViewCell
    {
        // Token: 0x060000E4 RID: 228 RVA: 0x000098FE File Offset: 0x00007AFE
        public Page1()
        {
            this.InitializeComponent();

        }

        /*// Token: 0x060000E5 RID: 229 RVA: 0x0000990C File Offset: 0x00007B0C
        [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private void InitializeComponent()
        {
            if (ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
            {
                AssemblyName = typeof(Page1).GetTypeInfo().Assembly.GetName(),
                ResourcePath = "Pages/Popup_Pages/Page1.xaml"
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
            StackLayout stackLayout = new StackLayout();
            NameScope value = new NameScope();
            NameScope.SetNameScope(this, value);
            label.SetValue(Label.TextProperty, "Welcome to Xamarin.Forms!");
            label.SetValue(View.VerticalOptionsProperty, LayoutOptions.CenterAndExpand);
            label.SetValue(View.HorizontalOptionsProperty, LayoutOptions.CenterAndExpand);
            stackLayout.Children.Add(label);
            this.SetValue(ContentPage.ContentProperty, stackLayout);
        }

        // Token: 0x060000E6 RID: 230 RVA: 0x000099E6 File Offset: 0x00007BE6
        private void __InitComponentRuntime()
        {
            this.LoadFromXaml(typeof(Page1));
        }*/
    }
}
