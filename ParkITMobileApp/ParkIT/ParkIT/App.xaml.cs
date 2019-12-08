using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ParkIT.Views;
using Xamarin.Essentials;
using Plugin.Media;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;
using ParkIT.Services.DB_Service;
using ParkIT.Services.AspService;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace ParkIT
{
    public partial class App : Application
    {
        public static DataBaseService DataBase = new DataBaseService();
        public static AspService AspService = new AspService();


        public static NavigationPage NavigationPage { get; private set; }
        private static RootPage RootPage;
        public static bool MenuIsPresented
        {
            get
            {
                return RootPage.IsPresented;
            }
            set
            {
                RootPage.IsPresented = value;
            }
        }


        public App()
        {
            InitializeComponent();
            var menuPage = new MenuPage();
            NavigationPage = new NavigationPage(new HomePage()) {BarBackgroundColor = Color.FromHex("#0ba5ff")};
            RootPage = new RootPage();
            RootPage.Master = menuPage;
            RootPage.Detail = NavigationPage;
            MainPage =  RootPage;

        }
        
        protected async override void OnStart()
        {
            //  ExperimentalFeatures.Enable(ExperimentalFeatures.EmailAttachments);
           await CrossMedia.Current.Initialize();
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
