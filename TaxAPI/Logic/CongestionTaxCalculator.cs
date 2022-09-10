
using TaxAPI.Services;

namespace TaxAPI.Logic
{
    public interface ICongestionTaxCalculator
    {
        int GetTaxForOneDay(string vehicleType, List<DateTime> dates);
        int CalculateTax(string vehicleType, List<DateTime> taxDateTimes);
    }
    public class CongestionTaxCalculator : ICongestionTaxCalculator
    {
        private const int MaxTaxPerDay = 60;
        private readonly ITaxationService taxationService;
        public CongestionTaxCalculator(ITaxationService taxationService)
        {
            this.taxationService = taxationService;
        }

        public int CalculateTax(string vehicleType, List<DateTime> taxDateTimes)
        {
            int totalTax = 0;
            var dateGroupings = taxDateTimes.GroupBy(d => new { d.Year, d.Month, d.Day }).Select(x => x);
            foreach (var dateGrouping in dateGroupings)
            {
                totalTax += GetTaxForOneDay(vehicleType, dateGrouping.ToList());
            }
            return totalTax;
        }

        public int GetTaxForOneDay(string vehicleType, List<DateTime> dates)
        {
            dates.Sort((x, y) => DateTime.Compare(x, y));
            DateTime intervalStart = dates.First();
            int totalFee = 0;
            foreach (DateTime date in dates)
            {
                int nextFee = taxationService.GetTollFee(date, vehicleType);
                int tempFee = taxationService.GetTollFee(intervalStart, vehicleType);

                var timeSpanSinceLastToll = date.Subtract(intervalStart);

                if (timeSpanSinceLastToll.TotalMinutes<= 60)
                {
                    if (totalFee > 0) 
                        totalFee -= tempFee;
                    if (nextFee >= tempFee)
                        tempFee = nextFee;
                    totalFee += tempFee;
                }
                else
                {
                    intervalStart = date;
                    totalFee += nextFee;
                }
            }
            if (totalFee > MaxTaxPerDay)
            {
                totalFee = MaxTaxPerDay;
            }
            return totalFee;
        }

        //public int GetTollFee(DateTime date, string vehicleType)
        //{
        //    if (IsTollFreeDate(date) || IsTollFreeVehicle(vehicleType))
        //        return 0;

        //    int hour = date.Hour;
        //    int minute = date.Minute;

        //    if (hour == 6 && minute >= 0 && minute <= 29) return 8;
        //    else if (hour == 6 && minute >= 30 && minute <= 59) return 13;
        //    else if (hour == 7 && minute >= 0 && minute <= 59) return 18;
        //    else if (hour == 8 && minute >= 0 && minute <= 29) return 13;
        //    else if (hour == 8 && minute == 30 || hour <= 14 && minute <= 59) return 8;
        //    else if (hour == 15 && minute >= 0 && minute <= 29) return 13;
        //    else if (hour == 15 && minute >= 0 || hour == 16 && minute <= 59) return 18;
        //    else if (hour == 17 && minute >= 0 && minute <= 59) return 13;
        //    else if (hour == 18 && minute >= 0 && minute <= 29) return 8;
        //    else return 0;
        //}

        //public bool IsTollFreeDate(DateTime date)
        //{
        //    int year = date.Year;
        //    int month = date.Month;
        //    int day = date.Day;

        //    if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
        //        return true;

        //    if (year == 2013)
        //    {
        //        if (month == 1 && day == 1 ||
        //            month == 3 && (day == 28 || day == 29) ||
        //            month == 4 && (day == 1 || day == 30) ||
        //            month == 5 && (day == 1 || day == 8 || day == 9) ||
        //            month == 6 && (day == 5 || day == 6 || day == 21) ||
        //            month == 7 ||
        //            month == 11 && day == 1 ||
        //            month == 12 && (day == 24 || day == 25 || day == 26 || day == 31))
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        //private bool IsTollFreeVehicle(string vehicleType)
        //{
        //    if (string.IsNullOrEmpty(vehicleType))
        //        return false;
        //    return Enum.TryParse<TollFreeVehicles>(vehicleType.ToLower(), out var tollFreeVehicle);
        //}


        //private enum TollFreeVehicles
        //{
        //    motorcycle = 0,
        //    tractor = 1,
        //    emergency = 2,
        //    diplomat = 3,
        //    foreign = 4,
        //    military = 5
        //}
    }
}

//if (minutes <= 60)
//{
//    if (nextFee <= tempFee)
//        continue;
//    tempFee = nextFee;
//    intervalStart = date;
//}
//else
//{
//    totalFee += tempFee;
//    totalFee += nextFee;
//    tempFee = 0;
//}


//if (minutes <= 60)
//{
//    if (totalFee > 0) totalFee -= tempFee;
//    if (nextFee >= tempFee) tempFee = nextFee;
//    totalFee += tempFee;
//}
//else
//{
//    totalFee += nextFee;
//}


