using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using ParkIT.Views;
using ParkIT.Services.Map_Service;
using ParkIT.Views.Popups;
using Rg.Plugins.Popup.Services;
using Xamarin.Essentials;

namespace ParkIT.ViewModels
{
    class MenuPageViewModel
    {

        public ICommand GoHomeCommand { get; set; }
        public ICommand GoWhereIsMyCarCommand { get; set; }
        public ICommand GoCreateSignCommand { get; set; }
        public ICommand GoUpdateUserDetails { get; set; }

        public MenuPageViewModel()
        {
            GoHomeCommand = new Command(GoHome);
            GoWhereIsMyCarCommand = new Command(GoWhereIsMyCar);
            GoCreateSignCommand = new Command(GoCreateSign);
            GoUpdateUserDetails = new Command(GoUpdateDetails);
        }

        void GoHome(object obj)
        {
            App.NavigationPage.Navigation.PopToRootAsync();
            App.MenuIsPresented = false;
        }

       async void GoWhereIsMyCar(object obj)
        {
          Location LastLocation = null;
          Maps map = null;

          if (!(Preferences.Get("Latitude", "0").Equals("0") || Preferences.Get("Longitude", "0").Equals("0")))
          {
              double latitude =Double.Parse(Preferences.Get("Latitude", "0"));
              double longitude =Double.Parse(Preferences.Get("Longitude", "0"));
              LastLocation = new Location(latitude, longitude);

                if (LastLocation != null)
                {
                    map = new Maps();
                    await map.NavigateWalking(LastLocation);
                }
            }
         else
            {
               await PopupNavigation.Instance.PushAsync(new LocationUninitialized());
            }
        }

        void GoCreateSign(object obj)
        {
            App.NavigationPage.Navigation.PushAsync(new CreateSignPage());
            App.MenuIsPresented = false;
        }

        void GoUpdateDetails(object obj)
        {
            App.NavigationPage.Navigation.PushAsync(new UpdateUserDetailsPage());
            App.MenuIsPresented = false;
        }
    }
}
