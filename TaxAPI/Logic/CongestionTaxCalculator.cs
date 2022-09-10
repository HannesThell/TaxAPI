
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
    }
}

