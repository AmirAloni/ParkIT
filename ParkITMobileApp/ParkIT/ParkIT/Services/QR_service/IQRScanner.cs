using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ParkIT.QR_service
{
    public interface IQRScanner
    {
       Task<string> ScanAsync();
    }
}
