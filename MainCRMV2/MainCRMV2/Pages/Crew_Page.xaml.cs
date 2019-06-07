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
    public partial class Crew_Page : ContentPage
    {
        // Token: 0x06000082 RID: 130 RVA: 0x000053EC File Offset: 0x000035EC
        public Crew_Page()
        {
            InitializeComponent();
        }

        /*// Token: 0x06000083 RID: 131 RVA: 0x000053FC File Offset: 0x000035FC
        [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private void InitializeComponent()
        {
            if (ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
            {
                AssemblyName = typeof(Crew_Page).GetTypeInfo().Assembly.GetName(),
                ResourcePath = "Pages/Crew_Page.xaml"
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

        // Token: 0x06000084 RID: 132 RVA: 0x000055AC File Offset: 0x000037AC
        private void __InitComponentRuntime()
        {
            this.LoadFromXaml(typeof(Crew_Page));
            this.TSection = this.FindByName("TSection");
        }

        // Token: 0x04000050 RID: 80
        

        // Token: 0x04000051 RID: 81
        [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private TableSection TSection;*/
    }
}
