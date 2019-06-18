using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.FacebookClient;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MainCRMV2.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Job_Page : ContentPage
    {
        public Job_Page()
        {
            InitializeComponent();
        }
        public void postEvent()
        {
            //CrossFacebookClient.Current.QueryDataAsync("coolheat/events",);
        }
    }
}