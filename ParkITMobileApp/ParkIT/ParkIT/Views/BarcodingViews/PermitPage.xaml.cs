using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ParkIT.Views.Popups;
using Rg.Plugins.Popup.Services;

namespace ParkIT.Views.BarcodingViews
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PermitPage : ContentPage
	{
        string[] m_Days = new string[7];
        string m_Area = string.Empty;
       // string m_ImportantNote = string.Empty;
        private string m_PermitStr = string.Empty;
        CreateSignPage m_CreateSignPage = null;

        public string PermitStr { get => m_PermitStr; set => m_PermitStr = value; }

        public PermitPage (CreateSignPage i_CreateSignPage)
		{
			InitializeComponent ();
           
               permitPicker.Items.Add("-");
               permitPicker.Items.Add("1");
               permitPicker.Items.Add("2");
               permitPicker.Items.Add("4");
               permitPicker.Items.Add("9");
               permitPicker.Items.Add("10");
               permitPicker.Items.Add("30");
               permitPicker.Items.Add("50");

            m_CreateSignPage = i_CreateSignPage;

            for (int i = 0; i < 7; i++)
            {
                m_Days[i] = string.Empty;
            }
        }
        private void A_Button_Clicked(object sender, EventArgs e)
        {
            if (a_Button.BackgroundColor == Color.Transparent)
                a_Button.BackgroundColor = Color.FromHex("#3a66a0");
            else
                a_Button.BackgroundColor = Color.Transparent;

            if (m_Days[0].Equals(string.Empty))
                m_Days[0] = "A";
            else
                m_Days[0] = string.Empty;

        }

        private void B_Button_Clicked(object sender, EventArgs e)
        {
            if (b_Button.BackgroundColor == Color.Transparent)
                b_Button.BackgroundColor = Color.FromHex("#3a66a0");
            else
                b_Button.BackgroundColor = Color.Transparent;

            if (m_Days[1].Equals(string.Empty))
                m_Days[1] = "B";
            else
                m_Days[1] = string.Empty;
        }

        private void C_Button_Clicked(object sender, EventArgs e)
        {
            if (c_Button.BackgroundColor == Color.Transparent)
                c_Button.BackgroundColor = Color.FromHex("#3a66a0");
            else
                c_Button.BackgroundColor = Color.Transparent;

            if (m_Days[2].Equals(string.Empty))
                m_Days[2] = "C";
            else
                m_Days[2] = string.Empty;
        }

        private void D_Button_Clicked(object sender, EventArgs e)
        {
            if (d_Button.BackgroundColor == Color.Transparent)
                d_Button.BackgroundColor = Color.FromHex("#3a66a0");
            else
                d_Button.BackgroundColor = Color.Transparent;

            if (m_Days[3].Equals(string.Empty))
                m_Days[3] = "D";
            else
                m_Days[3] = string.Empty;
        }

        private void E_Button_Clicked(object sender, EventArgs e)
        {
            if (e_Button.BackgroundColor == Color.Transparent)
                e_Button.BackgroundColor = Color.FromHex("#3a66a0");
            else
                e_Button.BackgroundColor = Color.Transparent;

            if (m_Days[4].Equals(string.Empty))
                m_Days[4] = "E";
            else
                m_Days[4] = string.Empty;
        }

        private void F_Button_Clicked(object sender, EventArgs e)
        {
            if (f_Button.BackgroundColor == Color.Transparent)
                f_Button.BackgroundColor = Color.FromHex("#3a66a0");
            else
                f_Button.BackgroundColor = Color.Transparent;

            if (m_Days[5].Equals(string.Empty))
                m_Days[5] = "F";
            else
                m_Days[5] = string.Empty;
        }

        private void G_Button_Clicked(object sender, EventArgs e)
        {
            if (g_Button.BackgroundColor == Color.Transparent)
                g_Button.BackgroundColor = Color.FromHex("#3a66a0");
            else
                g_Button.BackgroundColor = Color.Transparent;

            if (m_Days[6].Equals(string.Empty))
                m_Days[6] = "G";
            else
                m_Days[6] = string.Empty;
        }

        private async void SetPermitButton_Clicked(object sender, EventArgs e)
        {
            if ((m_Days[0] + m_Days[1] + m_Days[2] + m_Days[3] + m_Days[4] + m_Days[5] + m_Days[6]).Equals(string.Empty) || m_Area.Equals(string.Empty))
            {
                await DisplayAlert("לא נבחרו נתונים", "יש לבחור אזור, ימים ,שעות ואז ללחוץ הוספה", "חזור");
            }
            else
            {
                m_PermitStr += getSignString("@"+m_Area);
                cleanForm();
                m_CreateSignPage.AddPermitIcon();
                await Navigation.PopAsync();
            }
        }

        private void PermitPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_Area = permitPicker.Items[permitPicker.SelectedIndex].ToString();
        }

        private string getSignString(string sign)
        {
            if (startTimePicker.Time >= endTimePicker.Time)  //with day after
            {
                string firstPart = string.Empty;
                string secondPart = string.Empty;

                firstPart += String.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}-{9}{10}{11}"
                                          , "{", m_Days[0], m_Days[1], m_Days[2], m_Days[3], m_Days[4], m_Days[5], m_Days[6],
                                           startTimePicker.Time.ToString(@"hh\:mm"), "23:59", sign, "}");

                char[] dayAfterStr = new char[7];
                string daysAfter = string.Empty;

                for (int i = 0; i < 7; i++)
                {
                    dayAfterStr[i] = (char)('A' + i);
                }

                for (int j = 0; j < 7; j++)
                {
                    if (!m_Days[j].Equals(string.Empty))
                    {
                        daysAfter += dayAfterStr[(j + 1) % 7];
                    }
                }
                secondPart += String.Format("{0}{1}{2}-{3}{4}{5}"
                            , "{", daysAfter,
                             "00:00", endTimePicker.Time.ToString(@"hh\:mm"), sign, "}");

                return firstPart + secondPart;
            }
            else
            {
                return String.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}-{9}{10}{11}"
               , "{", m_Days[0], m_Days[1], m_Days[2], m_Days[3], m_Days[4], m_Days[5], m_Days[6],
               startTimePicker.Time.ToString(@"hh\:mm"), endTimePicker.Time.ToString(@"hh\:mm"), sign, "}");
            }
        }

        private void cleanForm()
        {
            m_Days[0] = string.Empty;
            m_Days[1] = string.Empty;
            m_Days[2] = string.Empty;
            m_Days[3] = string.Empty;
            m_Days[4] = string.Empty;
            m_Days[5] = string.Empty;
            m_Days[6] = string.Empty;

            a_Button.BackgroundColor = Color.Transparent;
            b_Button.BackgroundColor = Color.Transparent;
            c_Button.BackgroundColor = Color.Transparent;
            d_Button.BackgroundColor = Color.Transparent;
            e_Button.BackgroundColor = Color.Transparent;
            f_Button.BackgroundColor = Color.Transparent;
            g_Button.BackgroundColor = Color.Transparent;
        }
    }
}


