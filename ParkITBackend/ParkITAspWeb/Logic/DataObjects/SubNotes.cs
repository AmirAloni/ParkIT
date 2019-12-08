using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ParkITAspWeb.Logic.DataObjects
{
    public class SubNotesTS
    {
        private String subNotesString = null;
        private bool isPaid = false; //'$'
        private bool cantPark = false; //'!'
        private bool disable = false; //'^,carNum'
        private string disableCarNum = string.Empty; //'^,carNum'
        private ParkingPermit parkingPermit = null; //'@,area'
        private String canParkForMore = null;
        DateTime today;
        private bool overlapBetweenCurrTime = false;
        private String importantNote = null;

        public const int HOUR_SIZE = 2;

        public bool Disable { get => disable; set => disable = value; }
        public string ImportantNote { get => importantNote; set => importantNote = value; }
        public bool IsPaid { get => isPaid; set => isPaid = value; }
        public bool CantPark { get => cantPark; set => cantPark = value; }
        public string CanParkForMore { get => canParkForMore; set => canParkForMore = value; }
        public bool OverlapBetweenCurrTime { get => overlapBetweenCurrTime; set => overlapBetweenCurrTime = value; }
        public string DisableCarNum { get => disableCarNum; set => disableCarNum = value; }

        internal ParkingPermit GetParkingPermitObject()
        {
            return this.parkingPermit;
        }

        /// like in case "permit to park just for 3 hours
        //& & between 2 charcters

        public SubNotesTS(string str, string dateTime)
        {
            subNotesString = str;
            today = getDateTime(dateTime);
            setLogicValuesForSubNotes(str);
        }

        public void setImportantNoteString(char[] charArrOfCurrSubNotes, int index)
        {
            int startingPointIndexOfImportantNote = 0;
            int sizeImportantNote = calcImportantNoteSize(charArrOfCurrSubNotes, index,
                ref startingPointIndexOfImportantNote);
            char[] charArrOfImportantNote = new char[sizeImportantNote];

            for (int i = 0; i < sizeImportantNote; i++)
            {
                charArrOfImportantNote[i] = charArrOfCurrSubNotes[startingPointIndexOfImportantNote];
                startingPointIndexOfImportantNote++;
            }
            this.ImportantNote = charArrOfImportantNote.ToString();
        }

        public int calcImportantNoteSize(char[] str, int index, ref int startingPointIndexOfImportantNote)
        {
            int importantNoteSize = 0;
            int strSize = str.Length;

            while (str[index] != '&')
            {
                importantNoteSize++;
                index++;
            }
            startingPointIndexOfImportantNote = index;

            return importantNoteSize;
        }

        public bool checkOverlap(char[] charArrOfCurrSubNotes)
        {
            char currDaySymbol = getDaySymbol();
            int[] hoursToAndFrom = new int[HOUR_SIZE] { 0, 0 };
            DateTime startTime = getDateTime("00:00"); 
            DateTime endTime = getDateTime("00:00");
            int size = charArrOfCurrSubNotes.Length;
            bool hourCheck = false;

            for (int i = 0; i < size; i++)
            {
                if (currDaySymbol == charArrOfCurrSubNotes[i])
                {
                    while (checkIfCharIsADay(charArrOfCurrSubNotes[i]))
                    {
                        i++;
                    }
                    string signString = new string(charArrOfCurrSubNotes);
                    startTime = getDateTime(signString.Substring(i, 5));
                    i = i + 6;
                    endTime = getDateTime(signString.Substring(i, 5));

                    int endTimeHour = endTime.Hour;

                    if (today > startTime && today < endTime) 
                    {
                        int hours = endTime.Subtract(today).Hours;
                        int minutes = endTime.Subtract(today).Minutes;
                        canParkForMore = string.Format("{0},{1}", hours, minutes);
                        return true;
                    }

                    hourCheck = true;
                }
                if (hourCheck)
                    break;
            }
            return false;       
        }

        private DateTime getDateTime(string str)
        {
           return DateTime.Parse(str).AddHours(3);
        }

        public char getDaySymbol()
        {
            char checkDay;

            switch (today.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    checkDay = 'A';
                    break;
                case DayOfWeek.Monday:
                    checkDay = 'B';
                    break;
                case DayOfWeek.Tuesday:
                    checkDay = 'C';
                    break;
                case DayOfWeek.Wednesday:
                    checkDay = 'D';
                    break;
                case DayOfWeek.Thursday:
                    checkDay = 'E';
                    break;
                case DayOfWeek.Friday:
                    checkDay = 'F';
                    break;
                default:// Saturday
                    checkDay = 'G';
                    break;
            }
            return checkDay;
        }

        public bool checkIfCharIsADay(char currDay)
        {
            bool isDay = false;
            if (currDay == 'A' || currDay == 'B' ||
                currDay == 'C' || currDay == 'D' ||
                currDay == 'E' || currDay == 'F' ||
                currDay == 'G')
            {
                isDay = true;
            }
            return isDay;
        }

        public void setLogicValuesForSubNotes(String StringToCheck)
        {
            int sizeOfStringToCheck = StringToCheck.Length;
            char[] charArrOfCurrSubNotes = string.Join(string.Empty, subNotesString).ToCharArray();

            if (checkOverlap(charArrOfCurrSubNotes))
            {
                overlapBetweenCurrTime = true;
            }
            setSymbolAction(charArrOfCurrSubNotes);
        }

        void setSymbolAction(char[] charArrOfCurrSubNotes)
        {
            int size = charArrOfCurrSubNotes.Length;
            char symbol;
            for (int i = 0; i < size; i++)
            {
                symbol = charArrOfCurrSubNotes[i];
                switch (symbol)
                {
                    case '@':
                        string area = subNotesString.Split('@')[1];
                        parkingPermit = new ParkingPermit(area);
                        break;
                    case '$':
                        this.IsPaid = true;
                        break;
                    case '!':
                        this.CantPark = true;
                        break;
                    case '^':
                        Disable = true;
                        DisableCarNum = subNotesString.Split('^')[1];
                        break;
                    case '&':
                        if (this.ImportantNote != null)
                        {
                            this.setImportantNoteString(charArrOfCurrSubNotes, i++);
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }
}