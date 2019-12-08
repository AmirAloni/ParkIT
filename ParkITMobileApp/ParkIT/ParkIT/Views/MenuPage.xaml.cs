using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ParkIT.ViewModels;



namespace ParkIT.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MenuPage : ContentPage
	{
        public MenuPage()
        {
            BindingContext = new MenuPageViewModel();
            Title = "Menu";
            InitializeComponent();
            whereIsMyCarImage.Source = ImageSource.FromResource("ParkIT.Views.Images.Icons.LocationIcon.png");
            createSignImage.Source = ImageSource.FromResource("ParkIT.Views.Images.Icons.createSignIcon.png");
            updateUserDetailsImage.Source = ImageSource.FromResource("ParkIT.Views.Images.Icons.UpdateSettingsIcon.png");
        }

    }
}