using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using System.Data.SqlClient;


namespace ParkIT.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ReportProblemPage : ContentPage
	{
        string m_SignString = string.Empty;
        Location m_Location = null;


        public ReportProblemPage (string i_SignString)
		{
			InitializeComponent ();
            m_SignString = i_SignString;
		}

        private void ButtonDifferentSign_Clicked(object sender, EventArgs e)
        {
           report("Different Sign");
        }

        private void ButtonOther_Clicked(object sender, EventArgs e)
        {
           report("General");
        }

        private async void report(string i_Messege)
        {
            try
            {
                m_Location = await Geolocation.GetLastKnownLocationAsync();
                if (m_Location != null)
                {
                    uploadReport(i_Messege);
                    await DisplayAlert("תודה רבה", "נטפל בבעיה בהקדם.", "המשך");
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("מיקום לא ידוע", "יש לוודא ששירותי המיקום פתוחים.", "המשך");
                }
            }
            catch(Exception ex) {

                await DisplayAlert("קרתה תקלה","", "המשך");
            }

        }

        private void uploadReport(string i_Messege)
        {
            SqlConnection connection = new SqlConnection("Data Source = parkitsqlserver.database.windows.net; Initial Catalog = ParkITDB; Persist Security Info = True; User ID = amiral; Password = Aa130492");
            SqlCommand command = new SqlCommand(string.Format("INSERT INTO reports (sign_string,altitude,longitude,messege) VALUES ('{0}','{1}','{2}','{3}')", m_SignString,m_Location.Latitude,m_Location.Longitude,i_Messege), connection);
            command.Connection.Open();
            command.ExecuteNonQuery();
            command.Connection.Close();
        }




    }
}