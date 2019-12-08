using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ParkITAspWeb.Logic.DataObjects
{
    class ParkingPermit
    {
        string parkingPermitAreaNum;
        private bool isParkingPermit = false;

        public ParkingPermit(string parkingPermitAreaNum)
        {
            this.parkingPermitAreaNum = parkingPermitAreaNum;
            this.isParkingPermit = true;
        }

        public bool GetIfParkingPermit()
        {
            return this.isParkingPermit;
        }

        public string GetParkingPermitAreaNum()
        {
            return this.parkingPermitAreaNum;
        }
    }
}
