using NUnit.Framework;
using System;
using TaxAPI.Services;

namespace TaxAPITests
{
    public class TaxationServiceTests
    {
        [TestCase("2013-01-14 21:00:00", ExpectedResult = 0)]
        [TestCase("2013-02-07 06:23:27", ExpectedResult = 8)]
        [TestCase("2013-02-07 15:27:00", ExpectedResult = 13)]
        [TestCase("2013-02-08 06:27:00", ExpectedResult = 8)]
        [TestCase("2013-02-08 06:20:27", ExpectedResult = 8)]
        [TestCase("2013-02-08 14:35:00", ExpectedResult = 8)]
        [TestCase("2013-02-08 15:29:00", ExpectedResult = 13)]
        [TestCase("2013-02-08 15:47:00", ExpectedResult = 18)]
        [TestCase("2013-02-08 16:01:00", ExpectedResult = 18)]
        [TestCase("2013-02-08 16:48:00", ExpectedResult = 18)]
        [TestCase("2013-02-08 16:30:00", ExpectedResult = 18)]
        [TestCase("2013-02-08 17:49:00", ExpectedResult = 13)]
        [TestCase("2013-02-08 18:29:00", ExpectedResult = 8)]
        [TestCase("2013-02-08 18:35:00", ExpectedResult = 0)]
        [TestCase("2013-03-26 14:25:00", ExpectedResult = 8)]
        [TestCase("2013-03-28 14:07:27", ExpectedResult = 0, Description = "Day before holiday")]
        public int GetTollFee_ReturnsX_WhenY(string dateTime)
        {
            //Assign
            var date = DateTime.Parse(dateTime);
            var taxationService = new TaxationService();

            //Act
            var taxResult = taxationService.GetTollFee(date, "car");

            //Assert
            return taxResult;
        }
    }
}
