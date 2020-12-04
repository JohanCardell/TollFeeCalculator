using System;

namespace TollFeeCalculator
{
    public class Program
    {
        static void Main(string[] args)
        {
            Run(Environment.CurrentDirectory + "../../../../testData.txt");
        }

        static public void Run(String inputFile) {
          
            WriteToConsole(MessageBasedOnInPutFile(inputFile));
        }

        private static DateTime[] MessageBasedOnInPutFile(string inputFile)
        {
            string message;
            if(ConvertedString(inputFile) != null)
            {
                message = "The total fee for the inputfile is " + TotalFeeCost(loggedTollPassages));
            }
            ConvertedString(inputFile);
            string indata = System.IO.File.ReadAllText(inputFile);
            string[] dateStrings = indata.Split(", ");
            DateTime[] loggedTollPassages = new DateTime[dateStrings.Length];
            for (int i = 0; i < loggedTollPassages.Length; i++)
            {
                loggedTollPassages[i] = DateTime.Parse(dateStrings[i]);
            }
            return loggedTollPassages;
        }

        private static void WriteToConsole(DateTime[] loggedTollPassages)
        {
            Console.Write"The total fee for the inputfile is " +  TotalFeeCost(loggedTollPassages));
        }

        static public int TotalFeeCost(DateTime[] loggedTollPassages) {
            int totalFee = 0;
            const int MAXFEE = 60;
            DateTime hourIntervalStart = new DateTime(); //Starting interval
            foreach (var currentPassageTime in loggedTollPassages)
            {
                var diffInMinutes = currentPassageTime.Subtract(hourIntervalStart).TotalMinutes;
                if(diffInMinutes < 60) {
                    totalFee += AddTheHighestFee(TollFeePass(currentPassageTime),TollFeePass(hourIntervalStart));
                }
                else {
                    totalFee += TollFeePass(currentPassageTime);
                    hourIntervalStart = currentPassageTime;
                    //if (TollFeePass(currentPassageTime) < TollFeePass(previousPassageTime))
                }
            }
            return Math.Min(totalFee, MAXFEE);
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
            else if (hour >= 9 && hour <= 14 && minute >= 0 && minute <= 59) return 8;
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