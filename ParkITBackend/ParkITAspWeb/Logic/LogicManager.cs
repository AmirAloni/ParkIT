using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParkITAspWeb.Logic.DataObjects;

namespace ParkITAspWeb.Logic
{
    public class LogicManager
    {

        public string GetMessegeFromBackend(string stringToCheck) // {sign datails},Disable,Parking permit ,num
        {
            string messegeToGet = null;
            string[] strArr = stringToCheck.Split(',');
            string stringForTrafficSign = strArr[0];
            string stringForDisable = strArr[1];
            string stringForDisableCarNum = strArr[2];
            string stringForParkingNote = strArr[3];
            string stringForAreaNumber = strArr[4];
            string stringForDateTime = strArr[5];

            messegeToGet = GetMessegeFromUserByTrafficSign(stringForTrafficSign, stringForDisable, stringForDisableCarNum, stringForParkingNote, stringForAreaNumber, stringForDateTime);
            return messegeToGet;
        }

        public string GetMessegeFromUserByTrafficSign(string uniqueString, string stringForDisable, string stringForDisableCarNum, string stringForParkingNote, string parkingPermitAreaNum, string stringDateTime)
        //char  parkingPermitAreaNum is 0 if not parking permit
        {
            StringBuilder stringToSend = null;
            ClientUser userDetails = new ClientUser(stringForDisable, stringForDisableCarNum, stringForParkingNote, parkingPermitAreaNum);
            TrafficSign trafficSign = new TrafficSign(uniqueString, stringDateTime);
            SubNotesTS suitableSubNotes = null;

            string result = string.Empty;
            result = checkSpetialUniqueString(uniqueString , userDetails);

            if(result.Equals(string.Empty))
            {
                suitableSubNotes = trafficSign.getSuitableSubNotes();
                if (suitableSubNotes != null)
                {
                    stringToSend = messageBuilder(suitableSubNotes, userDetails);
                }
                else
                {
                    stringToSend = canParkMessageBuilder();
                    stringToSend.Append(",,,");
                }
                return stringToSend.ToString();
            }

            return result;
        }

        private string checkSpetialUniqueString(string uniqueString, ClientUser userDetails)
        {
            if (uniqueString.Equals("{^}"))  //check disabled - {^}
            {
                if (userDetails.IsDisabled)
                return "Yes,,,,";
                else
                return "No,,,,";
            }
           if (uniqueString.Equals("{!}")) //check prohibited - {!}
            {
                return "No,,,,";
            }
            return string.Empty;
        }

        public StringBuilder messageBuilder(SubNotesTS suitableSubNotes, ClientUser clientUser)
        {
            bool canPark = true;
            StringBuilder stringToSend = null;

            if (suitableSubNotes.CantPark ||
               (suitableSubNotes.Disable && suitableSubNotes.DisableCarNum.Equals(string.Empty) && (!clientUser.IsDisabled)) ||
               ((suitableSubNotes.Disable && !suitableSubNotes.DisableCarNum.Equals(string.Empty)) && (!clientUser.DisableCarNumber.Equals(suitableSubNotes.DisableCarNum))) ||
               (suitableSubNotes.GetParkingPermitObject() != null && !suitableSubNotes.GetParkingPermitObject().GetParkingPermitAreaNum().Equals(clientUser.GetParkingPermitObject().GetParkingPermitAreaNum())))
            {
                canPark = false;
            }
        
            if (canPark)
            {
                stringToSend = logicSymbolsCheckAfterUserCanPark(suitableSubNotes);
            }
            else
            {
                stringToSend = canNotParkMessageBuilder();
            }

            return stringToSend;
        }

        public StringBuilder logicSymbolsCheckAfterUserCanPark(SubNotesTS suitableSubNotes)
        {
            StringBuilder stringToSend = this.canParkMessageBuilder();
            if (suitableSubNotes.CanParkForMore != null)
            {
                stringToSend.Append(suitableSubNotes.CanParkForMore + ",");
            }
            else
            {
                stringToSend.Append(",");
            }
            if (suitableSubNotes.IsPaid)
            {
                stringToSend.Append("Paid,");
            }
            else
            {
                stringToSend.Append(",");
            }
            if (suitableSubNotes.ImportantNote != null)
            {
                stringToSend.Append(suitableSubNotes.ImportantNote);
            }
            else
            {
                stringToSend.Append(",");
            }
            return stringToSend;
        }
        public StringBuilder canParkMessageBuilder()
        {
            StringBuilder stringToSend = new StringBuilder("Yes,");

            return stringToSend;
        }
        public StringBuilder canNotParkMessageBuilder()
        {
            StringBuilder stringToSend = new StringBuilder("No,");
            stringToSend.Append(",,,");
            return stringToSend;
        }

    }
}
