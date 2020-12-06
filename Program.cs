using System;
using System.Collections.Generic;

namespace TollFeeCalculator
{
    public class Program
    {
        static void Main(string[] args)
        {
            Run(Environment.CurrentDirectory + "../../../../testData.txt");
        }

        static public void Run(string filePath) {
          
            Console.Write(MessageBasedOnInPutFile(filePath));
        }

        private static string MessageBasedOnInPutFile(string filePath)
        {
            string message = string.Empty;
            var loggedTollPassages = GetPassagesFromFile(filePath, ref message);
            if (message.Length == 0)
            {
                message = "The total fee for the inputfile is " + TotalFeeCost(loggedTollPassages);
            }
            return message;
        }


        private static DateTime[] GetPassagesFromFile(string filePath, ref string errorMessage)
        {
            string indata = string.Empty;
            try
            {
                indata = System.IO.File.ReadAllText(filePath);
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                return null;
            }
            string[] dateStrings = indata.Split(", ");
            DateTime[] loggedTollPassages = new DateTime[dateStrings.Length];
            for (int i = 0; i < loggedTollPassages.Length; i++)
            {
                try
                {
                    loggedTollPassages[i] = DateTime.Parse(dateStrings[i]);
                }
                catch (Exception e)
                {
                    errorMessage = e.Message;
                }
            }
            return loggedTollPassages;
        }


        //private static DateTime[] GetPassagesFromFile(string filePath, ref string errorMessage)
        //{
        //    string indata = string.Empty;
        //    try
        //    {
        //        indata = System.IO.File.ReadAllText(filePath);
        //    }
        //    catch (Exception e)
        //    {
        //        errorMessage = e.Message;
        //    }
        //    string[] dateStrings = indata.Split(", ");
        //    DateTime[] loggedTollPassages = new DateTime[dateStrings.Length];
        //    for (int i = 0; i < loggedTollPassages.Length; i++)
        //    {
        //        if (DateTime.TryParse(dateStrings[i], out DateTime result))
        //        {
        //            loggedTollPassages[i] = result;
        //        }
        //    }
        //    return loggedTollPassages;
        //}

        //private static bool ReadFile(string filePath, out DateTime[] loggedTollPassages, ref string errorMessage)
        //{
        //    bool fileIsInCorrectFormat = true;
        //    string indata = string.Empty;
        //    try
        //    {
        //        indata = System.IO.File.ReadAllText(filePath);
        //    }
        //    catch (Exception e)
        //    {
        //        errorMessage = e.Message;
        //    }
        //    string[] dateStrings = indata.Split(", ");
        //        loggedTollPassages = new DateTime[dateStrings.Length];
        //        for (int i = 0; i < loggedTollPassages.Length; i++)
        //        {
        //            if (DateTime.TryParse(dateStrings[i], out DateTime result))
        //            {
        //                loggedTollPassages[i] = result;
        //            }
        //            else fileIsInCorrectFormat = false;
        //        }
        //    return fileIsInCorrectFormat;
        //}

        static public int TotalFeeCost(DateTime[] loggedTollPassages) {
            List<DateTime[]> passagesSortedByDays = SortPassagesByDay(loggedTollPassages);
            int totalFee = 0;
            const int MAXFEE = 60;
            DateTime hourIntervalStart = loggedTollPassages[0]; //Starting interval
            totalFee += TollFeePass(hourIntervalStart);
            for (int i = 1; i < loggedTollPassages.Length - 1; i++)
            {
                var currentPassage = loggedTollPassages[i];
                var currentFee = TollFeePass(currentPassage);
                var diffInMinutes = currentPassage.Subtract(hourIntervalStart).TotalMinutes;
                if (diffInMinutes < 60)
                {
                    totalFee += AddTheHighestFee(currentFee, TollFeePass(hourIntervalStart));
                }
                else
                {
                    totalFee += currentFee;
                    hourIntervalStart = currentPassage;
                }
            }
            //foreach (var currentPassageTime in loggedTollPassages)
            //{
            //    //var diffInMinutes = currentPassageTime.Subtract(hourIntervalStart).TotalMinutes;
            //    long diffInMinutes = (currentPassageTime - hourIntervalStart).Minutes;
            //    if (diffInMinutes < 60) {
            //        totalFee += AddTheHighestFee(TollFeePass(currentPassageTime),TollFeePass(hourIntervalStart));
            //    }
            //    else {
            //        totalFee += TollFeePass(currentPassageTime);
            //        hourIntervalStart = currentPassageTime;
            //        //if (TollFeePass(currentPassageTime) < TollFeePass(previousPassageTime))
            //    }
            //}

            return Math.Min(totalFee, MAXFEE);
        }

        private static List<DateTime[]> SortPassagesByDay(DateTime[] loggedTollPassages)
        {
            List<DateTime[]> passsagesDividedByDate = new List<DateTime[]>();
            passsagesDividedByDate.Add(loggedTollPassages.g)
            loggedTollPassages.
        }

        static public int TollFeePass(DateTime d)
        {
            if (Free(d)) return 0;
            int hour = d.Hour;
            int minute = d.Minute;
            if (hour == 6 && minute >= 0 && minute <= 29) return 8;
            else if (hour == 6 && minute >= 30 && minute <= 59) return 13;
            else if (hour == 7 && minute >= 0 && minute <= 59) return 18;
            else if (hour == 8 && minute >= 0 && minute <= 29) return 13;
            else if (hour == 8 && minute >= 30 && minute <= 59) return 8;
            else if (hour >= 9 && hour <= 14) return 8;
            else if (hour == 15 && minute >= 0 && minute <= 29) return 13;
            else if (hour == 15 && minute >= 0 || hour == 16 && minute <= 59) return 18;
            else if (hour == 17 && minute >= 0 && minute <= 59) return 13;
            else if (hour == 18 && minute >= 0 && minute <= 29) return 8;
            else return 0;
        }

        static public bool Free(DateTime date) {
        return (int)date.DayOfWeek == 0 || (int)date.DayOfWeek == 6 || date.Month == 7;
        }

        static int AddTheHighestFee(int currentFee, int previousFee)
        {
            int fee = 0;
            if(currentFee > previousFee)
            {
                fee = currentFee - previousFee;
            }
            return fee;
        }
    }
}