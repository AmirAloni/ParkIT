using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParkIT.QR_service;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using ParkIT.Services.AspService;


namespace ParkIT.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        private string m_SignStr = string.Empty;

        public HomePage()
        {
            InitializeComponent();
            BackgroundImageSource = ImageSource.FromResource("ParkIT.Views.Images.BlueGradient.jpg");
            buttonScan.Source = ImageSource.FromResource("ParkIT.Views.Images.roundQRcode.png");
            Title = "ParkIT";
        }

        async Task openScanner()
        {
            var scanner = DependencyService.Get<IQRScanner>();
            string result = null;
       
                result = await scanner.ScanAsync();
            if (result != null)
            {
                m_SignStr = result;
            }
        }

        private async void ButtonScan_Clicked(object sender, EventArgs e)
        {

            string[] answerStrArr = new string[4];   //Yes/No,Paid/,time

            try
            {
                await openScanner();
                answerStrArr = getAnswerFromBackend();

                if (answerStrArr != null)
                {
                    await Navigation.PushAsync(new AnswerPage(answerStrArr, m_SignStr));
                }
            }
            catch (Exception ex)
            {
                if(ex.Message.Equals("IllegalQr"))
                    await DisplayAlert("ברקוד לא תקין", "הברקוד שנסרק אינו נמצא במערכת", "חזור");
                else if (ex.Message.Equals("NetworkError"))
                    await DisplayAlert("תקלת חיבור לרשת", "יש לוודא להחיבור לרשת תקין", "חזור");
            }
        }

        private string[] getAnswerFromBackend() 
        {
            string content;
            string disable = getDisable();
            string parkingNote = getParkingNote();

            if(App.DataBase.IsSignExists(m_SignStr))
            {
                App.AspService.SendToServer(m_SignStr, disable, parkingNote);
                content = App.AspService.GetContent();

                if (content != string.Empty)
                {
                    return content.Split(',');
                }
                else
                {
                    throw new Exception("NetworkError");
                }
            }
            else
            {
                throw new Exception("IllegalQr");
            }
        }

        private string getDisable()
        {
            if (Preferences.Get("Disabled", "לא").Equals("כן"))
            {
                return string.Format(",Disable,{0}",Preferences.Get("CarNumber", ""));
            }
            else
            {
                return ",,";
            }
        }

        private string getParkingNote()
        {
            if (!Preferences.Get("Permit", "-").Equals("-"))
            {
                return string.Format(",{0},{1}", "ParkingPermit", Preferences.Get("Permit", "-"));
            }
            else
            {
                return ",,";
            }
        }
    }
}
