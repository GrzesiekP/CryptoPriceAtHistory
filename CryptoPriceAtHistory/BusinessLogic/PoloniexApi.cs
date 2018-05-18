using Newtonsoft.Json.Linq;
using System;

namespace CryptoPriceAtHistory.BusinessLogic
{
    public static class PoloniexApi
    {
        private static string getBtcUrl(long timestamp) => $"https://poloniex.com/public?command=returnChartData&currencyPair=USDT_BTC&start={timestamp - 300}&end={timestamp}&period=300";
        private static string getCryptoUrl(long timestamp, string priceOfCurrency, string inCurrency) =>
            $"https://poloniex.com/public?command=returnChartData&currencyPair={inCurrency}_{priceOfCurrency}&start={timestamp - 300}&end={timestamp}&period=300";

        public static double GetBitcoinPrice(DateTime dateTime)
        {
            if (dateTime > DateTime.Now)
                return 0;

            try
            {
                var timestampOfDate = ((DateTimeOffset) dateTime).ToUnixTimeSeconds();
                return GetBitcoinPrice(timestampOfDate);
            }
            catch (ArgumentOutOfRangeException e)
            {
                // log exception
                return 0;
            }
        }

        private static double GetBitcoinPrice(long timestamp)
        {
            var fullUrl = getBtcUrl(timestamp);
            var jsonResponseArray = WebHelper.GetJsonFromUrl(fullUrl);
            var price = (double)JArray.Parse(jsonResponseArray)[0]["weightedAverage"];

            return price;
        }

        public static double GetPrice(DateTime dateTime, string priceOfCurrency, string inCurrency)
        {
            if (dateTime > DateTime.Now)
                return 0;

            try
            {
                var timestampOfDate = ((DateTimeOffset)dateTime).ToUnixTimeSeconds();
                return GetPrice(timestampOfDate, priceOfCurrency, inCurrency);
            }
            catch (ArgumentOutOfRangeException e)
            {
                // log exception
                return 0;
            }
        }

        private static double GetPrice(long timestamp, string priceOfCurrency, string inCurrency)
        {
            var fullUrl = getCryptoUrl(timestamp, priceOfCurrency, inCurrency);
            var jsonResponseArray = WebHelper.GetJsonFromUrl(fullUrl);
            var price = (double)JArray.Parse(jsonResponseArray)[0]["weightedAverage"];

            return price;
        }

        public static bool PairExist(string priceOfCurrency, string inCurrency)
        {
            var testDate = new DateTime(2018, 04, 15, 15, 32, 55);
            var timestampOfDate = ((DateTimeOffset)testDate).ToUnixTimeSeconds();
            var pairUrl = getCryptoUrl(timestampOfDate, priceOfCurrency, inCurrency);

            var response = WebHelper.GetJsonFromUrl(pairUrl);

            if (response.Contains("Invalid currency pair."))
                return false;

            return !string.IsNullOrEmpty(response) && (double) JArray.Parse(response)[0]["weightedAverage"] != 0;
        }
    }
}
