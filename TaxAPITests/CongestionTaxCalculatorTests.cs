using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using TaxAPI.Logic;
using TaxAPI.Services;

namespace TaxAPITests
{
    public class Tests
    {
        private CongestionTaxCalculator taxCalculator;
        private Mock<ITaxationService> taxationServiceMock;
        private DateTime firstDate = new(2013, 01, 15, 06, 00, 00);
        private DateTime secondDate = new(2013, 01, 15, 06, 30, 00);
        private DateTime thirdDate = new(2013, 01, 15, 07, 00, 00);
        private DateTime fourthDate = new(2013, 01, 15, 08, 15, 00);//"2013-01-15 08:15:00";
        private DateTime fifthDate = new(2013, 01, 15, 08, 30, 00);//"2013-01-15 08:30:00";
        private DateTime sixthDate = new(2013, 01, 15, 15, 00, 00);//"2013-01-15 15:00:00";
        private DateTime seventhDate = new(2013, 01, 15, 16, 30, 00);//"2013-01-15 16:30:00";
        private DateTime eightDate = new(2013, 01, 15, 18, 50, 00);//"2013-01-15 18:50:00";
        private DateTime ninthDate = new(2013, 01, 15, 20, 50, 00);//"2013-01-15 20:50:00";

        [SetUp]
        public void Setup()
        {
            taxationServiceMock = new Mock<ITaxationService>();
            taxationServiceMock.Setup(x => x.GetTollFee(It.IsAny<DateTime>(), It.IsAny<string>())).Returns(10);

            taxCalculator = new CongestionTaxCalculator(taxationServiceMock.Object);
        }

        [Test]
        public void GetTaxForOneDay_Returns30_GivenASetOfDates()
        {
            //Assign

            var dates = new List<DateTime>
            {
                secondDate,
                fourthDate,
                seventhDate,

            };

            //Act
            var taxResult = taxCalculator.GetTaxForOneDay("Car", dates);
            //Assert
            Assert.AreEqual(30, taxResult);
        }


        [Test]
        public void GetTaxForOneDay_Returns10_Given3DatesWithin60Minutes()
        {
            //Assign

            var dates = new List<DateTime>
            {
                firstDate,
                secondDate,
                thirdDate,

            };

            //Act
            var taxResult = taxCalculator.GetTaxForOneDay("Car", dates);
            //Assert
            Assert.AreEqual(10, taxResult);
        }

        [Test]
        public void GetTaxForOneDay_Returns60_AsItIsMaxAmount()
        {
            //Assign

            var dates = new List<DateTime>
            {
                firstDate,
                secondDate,
                thirdDate,
                fourthDate,
                fifthDate,
                sixthDate,
                seventhDate,
                eightDate,
                ninthDate

            };

            //Act
            var taxResult = taxCalculator.GetTaxForOneDay("Car", dates);
            //Assert
            Assert.AreEqual(60, taxResult);
        }

        [Test]
        public void GetTax_Returns20_GivenSameTimeButDifferentDays()
        {
            DateTime dayOne = new(2013, 01, 15, 06, 00, 00);
            DateTime dayTwo = new(2013, 01, 16, 06, 00, 00);

            var dates = new List<DateTime>
            {
                dayOne,
                dayTwo
            };

            //Act
            var taxResult = taxCalculator.GetTaxForOneDay("CAR", dates);
            //Assert
            Assert.AreEqual(20, taxResult);
        }

    }
}

