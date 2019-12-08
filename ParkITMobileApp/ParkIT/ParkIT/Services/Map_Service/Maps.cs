using System.Threading.Tasks;
using Xamarin.Essentials;

namespace ParkIT.Services.Map_Service
{
    class Maps
    {
        public async Task ShowOnMap(Location i_Location)
        {
            var options = new MapLaunchOptions { NavigationMode = NavigationMode.None };

            await Map.OpenAsync(i_Location, options);
        }

        public async Task NavigateWalking(Location i_Location)
        {
            var options = new MapLaunchOptions { NavigationMode = NavigationMode.Walking };

            await Map.OpenAsync(i_Location, options);
        }

        public async Task NavigateDriving(Location i_Location)
        {
            var options = new MapLaunchOptions { NavigationMode = NavigationMode.Driving };

            await Map.OpenAsync(i_Location, options);
        }
    }
}

