using System;
using System.Threading.Tasks;
using Plugin.Media;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ParkIT.Views.BarcodingViews;
using ParkIT.Views.Popups;
using System.IO;
using System.Data.SqlClient;
using Rg.Plugins.Popup.Services;
using System.Data;

namespace ParkIT.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CreateSignPage : ContentPage
	{
        private string m_SignString = string.Empty;
        private Plugin.Media.Abstractions.MediaFile m_Photo =null;
        PermitPage m_PermitPage = null;
        PaymentPage m_PaymentPage = null;
        ProhibitedPage m_ProhibitedPage = null;
        DisabledPage m_DisabledPage = null;

        public CreateSignPage ()
		{
			InitializeComponent ();
            parkingPermitButton.Source = ImageSource.FromResource("ParkIT.Views.Images.Icons.PermitIcon.png");
            paymentButton.Source = ImageSource.FromResource("ParkIT.Views.Images.Icons.shekelIcon.jpg");
            prohibitedButton.Source = ImageSource.FromResource("ParkIT.Views.Images.Icons.ProhibitedIcon.png");
            AllTimeProhibitedButton.Source = ImageSource.FromResource("ParkIT.Views.Images.Icons.ProhibitedIcon.png");
            DisabledButton.Source = ImageSource.FromResource("ParkIT.Views.Images.Icons.DisabledIcon.png");
            AllTimeDisabledButton.Source = ImageSource.FromResource("ParkIT.Views.Images.Icons.DisabledIcon.png");

            m_PermitPage = new PermitPage(this);
            m_PaymentPage = new PaymentPage(this);
            m_ProhibitedPage = new ProhibitedPage(this);
            m_DisabledPage = new DisabledPage(this);
        }

        private async void ParkingPermitButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(m_PermitPage);
        }

        private async void PaymentButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(m_PaymentPage); 
        }

        private async void ProhibitedButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(m_ProhibitedPage);
        }

        private async void DisabledButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(m_DisabledPage);
        }

        private async void ButtonCreateNewSign_Clicked(object sender, EventArgs e)
        {
            m_SignString += getSortedStr(m_PermitPage.PermitStr);
            m_SignString += getSortedStr(m_PaymentPage.PaymentsStr);
            m_SignString += getSortedStr(m_ProhibitedPage.ProhibitedStr);
            m_SignString += getSortedStr(m_DisabledPage.DisabledStr);

            if (!m_SignString.Equals(string.Empty))
               // uploadSignFromPhone();
              await nextSteps();
            else
                await DisplayAlert("לא נבחרו נתונים", "יש לבחור לפחות אחת מהאפשרויות", "חזור");
        }

        private void uploadSignFromPhone()
        {
            Services.DB_Service.DataBaseService db = new Services.DB_Service.DataBaseService();
            db.PickImageuploadSign(m_SignString);
        }

        private string getSortedStr(string unsortedStr)
        {
            if (unsortedStr.Equals(string.Empty))
                return string.Empty;

            string[] strArr = unsortedStr.Split('}');
            Array.Sort(strArr);
            string res = string.Empty;

            foreach (string str in strArr)
                if(!str.Equals(string.Empty))
                 res += str + "}";

            return res;
        }

        private async Task nextSteps()
        {
            if (!App.DataBase.IsSignExists(m_SignString))
            {
                Task uploadSignTask = new Task(new Action(uploadSign));
                await takePhoto();
                if (m_Photo != null && !m_SignString.Equals(string.Empty))
                {
                    uploadSignTask.Start();
                }
            }
            await Navigation.PushAsync(new UserAddress(m_SignString));
        }

        private void uploadSign()
        {
            FileStream fileStream = new FileStream(m_Photo.Path, FileMode.Open, FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            byte[] imageBytes = binaryReader.ReadBytes((int)fileStream.Length);

            App.DataBase.UploadSign(imageBytes, m_SignString);
        }

        private async Task takePhoto()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("מצלמה לא זמינה", "וודא שקיימת הרשאה למצלמה", "חזור");
                m_Photo = null;
            }
            else
            {
                m_Photo = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Small, //******
                    Directory = "Sample",
                    Name = "test.jpg"
                });

            }

        }

        public void AddPermitIcon()
        {
            Image image = new Image() { BackgroundColor = Color.Transparent, HeightRequest = 30, WidthRequest = 30 };
            image.Source = ImageSource.FromResource("ParkIT.Views.Images.Icons.PermitIcon.png");
            permitStackLayout.Children.Add(image);
        }

        public void AddPayentIcon()
        {
            Image image = new Image() { BackgroundColor = Color.Transparent, HeightRequest = 30, WidthRequest = 30 };
            image.Source = ImageSource.FromResource("ParkIT.Views.Images.Icons.shekelIcon.jpg");
            paymentStackLayout.Children.Add(image);
        }

        public void AddProhibitedIcon()
        {
            Image image = new Image() { BackgroundColor = Color.Transparent, HeightRequest = 30, WidthRequest = 30 };
            image.Source = ImageSource.FromResource("ParkIT.Views.Images.Icons.ProhibitedIcon.png");
            prohibitedStackLayout.Children.Add(image);
        }

        private async void AllTimeDisabledButton_Clicked(object sender, EventArgs e)
        {
            m_SignString = "{^}";
            await Navigation.PushAsync(new UserAddress(m_SignString));
        }

        private async void AllTimeProhibitedButton_Clicked(object sender, EventArgs e)
        {
            m_SignString = "{!}";
            await Navigation.PushAsync(new UserAddress(m_SignString));
        }
    }
}