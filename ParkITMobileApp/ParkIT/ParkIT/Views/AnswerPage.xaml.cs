using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ParkIT.Views.Popups;
using Rg.Plugins.Popup.Services;

namespace ParkIT.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AnswerPage : ContentPage
	{
        string m_SignString = string.Empty;
        string m_Explanation = string.Empty;

        public AnswerPage (string[] i_Answer , string i_SignString)
		{
            m_SignString = string.Empty;
            m_Explanation = string.Empty;

            InitializeComponent ();
            m_SignString = i_SignString;

            loadSignImage();

            if (i_Answer[0].Equals("Yes"))
            {
                lableMessage.Text = "החניה מותרת - ParkIT";
                lableMessage.TextColor = Color.FromHex("#4A876B");

                if (i_Answer[3].Equals("Paid"))
                {
                    lableExplanation.Text = "חניה בתשלום ";
                }

                if (!i_Answer[2].Equals(string.Empty))
                {
                    string[] hm = new string[2];
                    hm[1] = i_Answer[1];
                    hm[0] = i_Answer[2];
                    string timeLeft = string.Empty;

                    if (int.Parse(hm[1])<=3)
                        timeLeft = String.Format("נותרו {0} שעות ו-{1} דקות", hm[1], hm[0]);

                    lableExplanation.Text += timeLeft;
                }

            }
            else
            {
                lableMessage.Text = "החניה אסורה";
                lableMessage.TextColor = Color.FromHex("#DC143C");
            }

           
        }

        private void loadSignImage()
        {
            byte[] imageBytes = null;
            imageBytes = App.DataBase.DownloadImage(m_SignString);
            imageSign.Source = ImageSource.FromStream(() => new MemoryStream(imageBytes));
        }

        private async void ButtonReportSign_Clicked(object sender, EventArgs e)
         {
             await Navigation.PushAsync(new ReportProblemPage(m_SignString));
         }

        private async void ButtonSaveMyParking_Clicked(object sender, EventArgs e)
        {
            try
            {
                Location LastLocation = null;
                LastLocation = await Geolocation.GetLastKnownLocationAsync();
                if (LastLocation != null)
                {
                    Preferences.Set("Latitude", LastLocation.Latitude);
                    Preferences.Set("Longitude", LastLocation.Longitude);
                    await PopupNavigation.Instance.PushAsync(new LocationSavedPopup("מיקום החניה נשמר"));
                }
                else
                {
                    await PopupNavigation.Instance.PushAsync(new LocationSavedPopup("חניה לא נשמרה, יתכן ששירותי המיקום לא פעילים"));
                }
            }
            catch (Exception ex)
            {
                // Unable to get location
            }
        }
    }
    }
