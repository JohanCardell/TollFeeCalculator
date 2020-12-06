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

        static public void Run(string filePath)
        {
            Console.Write(MessageBasedOnFileContent(filePath));
        }

        private static string MessageBasedOnFileContent(string filePath)
        {
            var errorMessage = string.Empty;
            var passages = GetPassagesFromFile(filePath, ref errorMessage);
            string message;
            if (Error(errorMessage))
            {
                message = errorMessage;
            }
            else
            {
                message = "The total fee for the inputfile is " + TotalFeeCost(passages);
            }
            return message;
        }

        static bool Error(string message)
        {
            return message.Length > 0; 
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
                    return null;
                }
            }
            return loggedTollPassages;
        }

        static public int TotalFeeCost(DateTime[] loggedTollPassages) {
            var totalFee = 0;
            const int MAXFEE = 60;
            DateTime hourIntervalStart = loggedTollPassages[0]; //Starting interval
            var initialFeeToday = TollFeePass(hourIntervalStart);
            var totalFeeToday = initialFeeToday;
            for (int i = 1; i < loggedTollPassages.Length; i++)
            {
                var currentPassage = loggedTollPassages[i];
                var currentFee = TollFeePass(currentPassage);
                if (SameDay(currentPassage, hourIntervalStart))
                {
                    if (WithinAnHour(currentPassage, hourIntervalStart))
                    {
                        totalFeeToday += OnlyUseHighestFee(currentFee, TollFeePass(hourIntervalStart));
                    }
                    else
                    {
                        totalFeeToday += currentFee;
                        hourIntervalStart = currentPassage;
                    }
                }
                else
                {
                    totalFee += Math.Min(totalFeeToday, MAXFEE);
                    totalFeeToday = 0;
                    initialFeeToday = TollFeePass(currentPassage);
                    totalFeeToday += initialFeeToday;
                    hourIntervalStart = currentPassage;
                }
            }
            return totalFee + totalFeeToday;
        }

        static bool SameDay(DateTime current, DateTime previous)
        {
            return current.Date == previous.Date;
        }

        static bool WithinAnHour(DateTime current, DateTime previous)
        {
            return (current.Subtract(previous).TotalMinutes < 60);
        }

        static public int TollFeePass(DateTime passage)
        {
            int hour = passage.Hour;
            int minute = passage.Minute;
            int fee;
            if (Free(passage)) fee = 0;
            else if (hour == 6 && minute >= 0 && minute <= 29) fee = 8;
            else if (hour == 6 && minute >= 30 && minute <= 59) fee = 13;
            else if (hour == 7 && minute >= 0 && minute <= 59) fee = 18;
            else if (hour == 8 && minute >= 0 && minute <= 29) fee = 13;
            else if (hour == 8 && minute >= 30 && minute <= 59) fee = 8;
            else if (hour >= 9 && hour <= 14) fee = 8;
            else if (hour == 15 && minute >= 0 && minute <= 29) fee = 13;
            else if (hour == 15 && minute >= 0 || hour == 16 && minute <= 59) fee = 18;
            else if (hour == 17 && minute >= 0 && minute <= 59) fee = 13;
            else if (hour == 18 && minute >= 0 && minute <= 29) fee = 8;
            else fee = 0;
            return fee;
        }

        static public bool Free(DateTime date)
        {
            return (int)date.DayOfWeek == 0 || (int)date.DayOfWeek == 6 || date.Month == 7;
        }

        private static int OnlyUseHighestFee(int currentFee, int previousFee)
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