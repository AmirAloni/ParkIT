using System;  
using System.Collections.Generic;  
using System.Linq;  
using System.Text;  
using System.Threading.Tasks;  
using Android.App;  
using Android.Content;  
using Android.OS;  
using Android.Runtime;  
using Android.Views;  
using Android.Widget;   
using ZXing.Mobile;
using ParkIT.QR_service;

using Xamarin.Forms;  
  
[assembly: Dependency(typeof(ParkIT.Droid.QRScanningService.QRScanningService))]  
  
namespace ParkIT.Droid.QRScanningService
{
    class QRScanningService : IQRScanner
    {
        public async Task<string> ScanAsync()
        {
            var optionsDefault = new MobileBarcodeScanningOptions();
            var optionsCustom = new MobileBarcodeScanningOptions();

            var scanner = new MobileBarcodeScanner()
            {
                TopText = "Scan the sign QR code",
                CancelButtonText = "Cancel"
            };

            var scanResult = await scanner.Scan(optionsCustom);
            return scanResult.Text;
        }
    }
}
