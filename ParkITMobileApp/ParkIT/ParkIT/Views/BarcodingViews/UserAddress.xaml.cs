using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Data.SqlClient;
using ParkIT.QR_service;
using System.Net.Mail;
using System.IO;
using System.Text;

namespace ParkIT.Views.BarcodingViews
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class UserAddress : ContentPage
	{
        private string m_SignString = string.Empty;

		public UserAddress (string i_SignString)
		{
			InitializeComponent ();
            m_SignString = i_SignString;
        }

        private void uploadSendRequest()
        {
            SqlConnection connection = new SqlConnection("Data Source = parkitsqlserver.database.windows.net; Initial Catalog = ParkITDB; Persist Security Info = True; User ID = amiral; Password = Aa130492");
            SqlCommand command = new SqlCommand(string.Format("INSERT INTO SendRequests (full_name, full_address, post_number, sign_string) VALUES ('{0}','{1}','{2}','{3}')", fullNameEntry.Text, fullAddressEntry.Text, postEntry.Text, m_SignString), connection);
            command.Connection.Open();
            command.ExecuteNonQuery();
            command.Connection.Close();
        }
      

        private void sendQRByMail()
        {
            try
            {

                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("parkit564@gmail.com");
                mail.To.Add(emailEntry.Text);
                mail.Subject = "ParkIT";
                mail.Body = "תודה על שיתוף הפעולה!";
                Attachment attachment = null;

                QRGenerator QR_Generator = new QRGenerator();
                QR_Generator.setValue(m_SignString);
                var qr = QR_Generator.getQRcode();
                qr.Aspect = Aspect.Fill;
                string s = qr.BarcodeValue.ToString();

                MemoryStream ms = new MemoryStream(Encoding.ASCII.GetBytes(s));
                attachment = new Attachment(ms, "QR.jpg");

                mail.Attachments.Add(attachment);

                SmtpServer.Port = 587;
                SmtpServer.Host = "smtp.gmail.com";
                SmtpServer.EnableSsl = true;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new System.Net.NetworkCredential("parkit564@gmail.com", "Aa130492");

                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
            }
        }







        private async void MailButton_Clicked(object sender, EventArgs e)
        {
            if (fullNameEntry.Text!=null && fullAddressEntry.Text != null && postEntry.Text != null)
            {
                Task task = new Task(new Action(uploadSendRequest));
                task.Start();
                await DisplayAlert("תודה על שיתוף הפעולה!", "ברקוד ישלח לביתך בקרוב", "סיום");
                await Navigation.PopToRootAsync();
            }
            else
            {
                await DisplayAlert("", "יש להזין את הנתונים", "המשך");
            }

        }

        private async void EmailButton_Clicked(object sender, EventArgs e)
        {
            if (emailEntry.Text != null)
            {
                sendQRByMail();
                await DisplayAlert("תודה על שיתוף הפעולה!", "ברקוד ישלח לך במייל", "סיום");
                await Navigation.PopToRootAsync();
            }
            else
            {
                await DisplayAlert("", "יש להזין את כתובת האימייל", "המשך");
            }
        }
    }
}