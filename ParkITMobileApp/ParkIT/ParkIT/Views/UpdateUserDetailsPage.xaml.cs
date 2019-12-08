using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace ParkIT.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class UpdateUserDetailsPage : ContentPage
	{
		public UpdateUserDetailsPage ()
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
            permitPicker.SelectedIndex = 0;

            disabledPicker.Items.Add("לא");
            disabledPicker.Items.Add("כן");

            permitPicker.Title = "בחר תו חניה";
            disabledPicker.Title = "האם יש תו נכה";

            permitPicker.SelectedIndex = 0;
            disabledPicker.SelectedIndex = 0;
        }

        private async void ButtonUpdateDetails_Clicked(object sender, EventArgs e)
        {
            Preferences.Set("Permit", permitPicker.Items[permitPicker.SelectedIndex]);
            Preferences.Set("Disabled", disabledPicker.Items[disabledPicker.SelectedIndex]);
            Preferences.Set("CarNumber", carNumEntry.Text);
            await Navigation.PopAsync();
        }
    }
}