using CryptoPriceAtHistory.BusinessLogic;
using NUnit.Framework;
using System;

namespace CryptoPriceAtHistory.Tests
{
    [TestFixture]
    public class PoloniexApiTests
    {
        private readonly DateTime _date = new DateTime(2018, 04, 15, 15, 32, 55);

        [Test, MaxTime(5000)]
        public void ShouldReturnPriceAtDate()
        {
            var price = PoloniexApi.GetBitcoinPrice(_date);

            Assert.AreNotEqual(0, price);
        }

        [Test, MaxTime(5000)]
        [TestCase("ETH", "BTC", ExpectedResult = true)]
        [TestCase("XMR", "BTC", ExpectedResult = true)]
        [TestCase("BTC", "USDT", ExpectedResult = true)]
        [TestCase("INVALID", "INVALIDALSO", ExpectedResult = false)]
        public bool ShouldRecognizeIfPairExist(string priceOf, string valuedIn)
        {
            var exist = PoloniexApi.PairExist(priceOf, valuedIn);

            return exist;
        }
    }
}
