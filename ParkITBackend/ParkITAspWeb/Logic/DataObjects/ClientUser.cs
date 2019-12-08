using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ParkITAspWeb.Logic.DataObjects
{
    public class ClientUser
    {
        private bool isDisabled = false;
        private string disableCarNumber = string.Empty;
        ParkingPermit parkingPermit = null;

        public bool IsDisabled { get => isDisabled; set => isDisabled = value; }
        public string DisableCarNumber { get => disableCarNumber; set => disableCarNumber = value; }

        public ClientUser(string disable, string disableCarNum, string i_ParkingPermit, string parkingPermitAreaNum) //if not parkingPermitAreaNum - char parkingPermitAreaNum = 0 else the correct number
        {
            if (disable.Equals("Disable"))
            {
                isDisabled = true;
                DisableCarNumber = disableCarNum;
            }
            if (i_ParkingPermit.Equals("ParkingPermit"))
            {
                parkingPermit = new ParkingPermit(parkingPermitAreaNum);
            }
        }

        internal ParkingPermit GetParkingPermitObject()
        {
            return parkingPermit;
        }
    }
}
