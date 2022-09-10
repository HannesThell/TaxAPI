﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TaxAPI.Services
{
    public interface ITaxationService
    {
        int GetTollFee(DateTime date, string vehicleType);
        bool IsTollFreeDate(DateTime date);

    }
    public class TaxationService : ITaxationService
    {

        public int GetTollFee(DateTime date, string vehicleType)
        {
            if (IsTollFreeDate(date) || IsTollFreeVehicle(vehicleType))
                return 0;

            int hour = date.Hour;
            int minute = date.Minute;

            if (hour == 6 && minute >= 0 && minute <= 29) return 8;
            else if (hour == 6 && minute >= 30 && minute <= 59) return 13;
            else if (hour == 7 && minute >= 0 && minute <= 59) return 18;
            else if (hour == 8 && minute >= 0 && minute <= 29) return 13;
            else if (hour == 8 && minute == 30 || hour <= 14 && minute <= 59) return 8;
            else if (hour == 15 && minute >= 0 && minute <= 29) return 13;
            else if (hour == 15 && minute >= 0 || hour == 16 && minute <= 59) return 18;
            else if (hour == 17 && minute >= 0 && minute <= 59) return 13;
            else if (hour == 18 && minute >= 0 && minute <= 29) return 8;
            else return 0;
        }

        public bool IsTollFreeDate(DateTime date)
        {
            int year = date.Year;
            int month = date.Month;
            int day = date.Day;

            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                return true;

            if (year == 2013)
            {
                if (month == 1 && day == 1 ||
                    month == 3 && (day == 28 || day == 29) ||
                    month == 4 && (day == 1 || day == 30) ||
                    month == 5 && (day == 1 || day == 8 || day == 9) ||
                    month == 6 && (day == 5 || day == 6 || day == 21) ||
                    month == 7 ||
                    month == 11 && day == 1 ||
                    month == 12 && (day == 24 || day == 25 || day == 26 || day == 31))
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsTollFreeVehicle(string vehicleType)
        {
            if (string.IsNullOrEmpty(vehicleType))
                return false;
            return Enum.TryParse<TollFreeVehicles>(vehicleType.ToUpper(), out var tollFreeVehicle);
        }
        private enum TollFreeVehicles
        {
            MOTORCYCLE = 0,
            TRACTOR = 1,
            EMERGENCY = 2,
            DIPLOMAT = 3,
            FOREIGN = 4,
            MILITARY = 5
        }
    }
}
