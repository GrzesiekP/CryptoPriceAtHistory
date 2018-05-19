using System;
using CryptoPriceAtHistory.BusinessLogic;
using CryptoPriceAtHistory.Enums;
using NUnit.Framework;

namespace CryptoPriceAtHistory.Tests
{
    [TestFixture]
    public class PoloniexApiTests
    {
        private static readonly DateTime _date = new DateTime(2018, 04, 15, 15, 32, 55);
        private readonly string _btcString = "BTC";
        private readonly string _usdtString = "USDT";


        [Test, MaxTime(10000)]
        public void ShouldReturnPriceAtDate()
        {
            var price = PoloniexApi.GetBitcoinPrice(_date);

            Assert.AreNotEqual(0, price);
        }

        [Test, MaxTime(30000)]
        [TestCase("ETH", "BTC", ExpectedResult = true)]
        [TestCase("XMR", "BTC", ExpectedResult = true)]
        [TestCase("BTC", "USDT", ExpectedResult = true)]
        [TestCase("INVALID", "INVALIDALSO", ExpectedResult = false)]
        [TestCase("", "INVALIDALSO", ExpectedResult = false)]
        [TestCase(null, "INVALIDALSO", ExpectedResult = false)]
        [TestCase(" ", "INVALIDALSO", ExpectedResult = false)]
        [TestCase("INVALIDALSO", "", ExpectedResult = false)]
        [TestCase("INVALIDALSO", null, ExpectedResult = false)]
        [TestCase("INVALIDALSO", " ", ExpectedResult = false)]
        public bool ShouldRecognizeIfPairExist(string priceOf, string valuedIn)
        {
            var exist = PoloniexApi.PairExist(priceOf, valuedIn);

            return exist;
        }

        [Test, MaxTime(30000)]
        [TestCase("ETH", "BTC", ExpectedResult = null)]
        [TestCase("XMR", "BTC", ExpectedResult = null)]
        [TestCase("ASDFG", "WERT", ExpectedResult = ErrorType.INVALID_CURRENCY_PAIR)]
        [TestCase("INVALID", "INVALIDALSO", ExpectedResult = ErrorType.INVALID_CURRENCY_PAIR)]
        [TestCase("", "INVALIDALSO", ExpectedResult = ErrorType.INVALID_CURRENCY_PAIR)]
        [TestCase(null, "INVALIDALSO", ExpectedResult = ErrorType.INVALID_CURRENCY_PAIR)]
        [TestCase(" ", "INVALIDALSO", ExpectedResult = ErrorType.INVALID_CURRENCY_PAIR)]
        [TestCase("INVALIDALSO", "", ExpectedResult = ErrorType.INVALID_CURRENCY_PAIR)]
        [TestCase("INVALIDALSO", null, ExpectedResult = ErrorType.INVALID_CURRENCY_PAIR)]
        [TestCase("INVALIDALSO", " ", ExpectedResult = ErrorType.INVALID_CURRENCY_PAIR)]
        public ErrorType? ShouldReturnInvalidPairError(string priceOf, string valuedIn)
        {
            var error = PoloniexApi.GetPrice(_date, priceOf, valuedIn).Error;

            return error;
        }

        [Test, MaxTime(30000)]
        [TestCase("01/20/2018", ExpectedResult = null)]
        [TestCase("01/20/2030", ExpectedResult = ErrorType.INVALID_DATE)]
        [TestCase("01/20/1990", ExpectedResult = ErrorType.INVALID_DATE)]
        public ErrorType? ShouldReturnInvalidDateError(DateTime dateTime)
        {
            var response = PoloniexApi.GetPrice(dateTime, _btcString, _usdtString);

            return response.Error;
        }
    }
}
