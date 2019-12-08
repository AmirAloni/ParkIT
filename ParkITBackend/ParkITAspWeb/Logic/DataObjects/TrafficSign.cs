using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ParkITAspWeb.Logic.DataObjects
{
    public class TrafficSign
    {
        private int numberOfSubNotes = 0;
        private string uniqueString = null;
        private string[] strPerSubNotes = null;
        private SubNotesTS[] subNotes = null;
        private bool isDisabled = false;

        public TrafficSign(string uniqueString ,string stringDateTime)
        {
            char[] charArrUniqueString = string.Join(string.Empty, uniqueString).ToCharArray();
            this.numberOfSubNotes = calcNotesSize(charArrUniqueString);
            this.strPerSubNotes = new String[this.numberOfSubNotes];
            this.subNotes = new SubNotesTS[this.numberOfSubNotes];
            this.uniqueString = uniqueString;
            this.getStrSubNotes(uniqueString);

            for (int i = 0; i < this.numberOfSubNotes; i++)
            {
                subNotes[i] = new SubNotesTS(strPerSubNotes[i], stringDateTime);
            }
        }

        public bool getIfDisabledAppearOnTheTrafficSign()
        {
            return this.isDisabled;
        }

        public void getStrSubNotes(String uniqueString)
        {
            int sizeOfUniqueString = uniqueString.Length;
            char[] charArrUniqueString = string.Join(string.Empty, uniqueString).ToCharArray();
            int currCharArrSubNotesSize = 0;
            int startingPointIndexPerSubNotes = 1;

            for (int j = 0; j < this.numberOfSubNotes; j++)
            {
                currCharArrSubNotesSize = 0;
                currCharArrSubNotesSize = calcSubNotesSize(charArrUniqueString, startingPointIndexPerSubNotes);
                char[] currSubNotesCharArr = calcSubNotes(currCharArrSubNotesSize, charArrUniqueString, ref startingPointIndexPerSubNotes);
                this.strPerSubNotes[j] = new string(currSubNotesCharArr);
                currSubNotesCharArr = null;
            }

        }

        public int calcNotesSize(char[] charArrUniqueString)
        {
            int notesSize = 0;
            int size = charArrUniqueString.Length;

            for (int i = 0; i < size; i++)
            {
                if (charArrUniqueString[i] == '{')
                {
                    notesSize++;
                }
            }

            return notesSize;
        }

        public int calcSubNotesSize(char[] charArrUniqueString, int startingPointIndexPerSubNotes)
        {
            int resSize = 0;
            int size = charArrUniqueString.Length;
            bool getTheCorrectSize = false;

            for (int i = startingPointIndexPerSubNotes; i < size; i++)
            {
                if (charArrUniqueString[i] == '}')
                {
                    getTheCorrectSize = true;
                }

                if (getTheCorrectSize)
                    break;
                resSize++;
            }

            return resSize;
        }

        public char[] calcSubNotes(int strSubNotesSize, char[] charArrUniqueString, ref int startingPointIndexPerSubNotes)
        {
            char[] currSubNotesCharArr = new Char[strSubNotesSize];
            int sizeOfCharArrCurrSubNotes = currSubNotesCharArr.Length;

            for (int i = 0; i < sizeOfCharArrCurrSubNotes; i++)
            {
                currSubNotesCharArr[i] = charArrUniqueString[startingPointIndexPerSubNotes];
                startingPointIndexPerSubNotes++;
            }
            startingPointIndexPerSubNotes += 2;

            return currSubNotesCharArr;
        }

        char getSymbolFromIndex(int index)
        {
            char symbol;

            switch (index)
            {
                case 0:
                    symbol = '+';
                    break;
                case 1:
                    symbol = '@';
                    break;
                case 2:
                    symbol = '+';
                    break;
                default:
                    symbol = 'X';
                    break;
            }

            return symbol;
        }


        public SubNotesTS getSuitableSubNotes()
        {
            SubNotesTS neededSubNotes = null;

            for (int i = 0; i < numberOfSubNotes; i++)
            {
                if (this.subNotes[i].OverlapBetweenCurrTime)
                    neededSubNotes = this.subNotes[i];
            }

            return neededSubNotes;
        }
    }
}
