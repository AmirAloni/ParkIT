using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ParkIT.Views.Popups
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LocationSavedPopup
	{
		public LocationSavedPopup (string i_Messege)
		{
			InitializeComponent ();
            labelMessege.Text = i_Messege;

        }
	}
}