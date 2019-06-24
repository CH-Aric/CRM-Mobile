using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace MainCRMV2.Pages.Customers
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Booking_Page : ContentPage
    {
        public Booking_Page()
        {
            InitializeComponent();
            renderBookingMap();
        }
        public async void renderBookingMap()
        {
            var locator = CrossGeolocator.Current;
            var position = await locator.GetPositionAsync();
            var position2=await locator.GetPositionsForAddressAsync("1083 Elmlea");
            var map = new Map(
            MapSpan.FromCenterAndRadius(new Position(position2.Latitude, position2.Longitude),
                                             Distance.FromMiles(1)))
            {
                IsShowingUser = true,
                HeightRequest = 100,
                WidthRequest = 960,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            stack.Children.Add(map);
        }
    }
}