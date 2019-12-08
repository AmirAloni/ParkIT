using System;
using System.Collections.Generic;
using System.Text;
using ZXing.Net.Mobile.Forms;
using Xamarin.Forms;


namespace ParkIT.QR_service
{
    public class QRGenerator
    {
        ZXingBarcodeImageView m_Barcode;

        public QRGenerator()
        {
            m_Barcode = new ZXingBarcodeImageView
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                AutomationId = "zxingBarcodeImageView",
            };
            m_Barcode.BarcodeFormat = ZXing.BarcodeFormat.QR_CODE;
            m_Barcode.BarcodeOptions.Width = 300;
            m_Barcode.BarcodeOptions.Height = 300;
            m_Barcode.BarcodeOptions.Margin = 10;

        }

        public ZXingBarcodeImageView getQRcode()
        {
            return m_Barcode;
        }


        public void setValue(string i_Value)
        {
            m_Barcode.BarcodeValue = i_Value;
        }

    }

}

        
