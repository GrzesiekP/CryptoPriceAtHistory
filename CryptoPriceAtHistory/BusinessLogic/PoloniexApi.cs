using Newtonsoft.Json.Linq;
using System;
using CryptoPriceAtHistory.Enums;
using static System.String;

namespace CryptoPriceAtHistory.BusinessLogic
{
    public static class PoloniexApi
    {
        private static string GetBtcUrl(long timestamp) => $"https://poloniex.com/public?command=returnChartData&currencyPair=USDT_BTC&start={timestamp - 300}&end={timestamp}&period=300";
        private static string GetCryptoUrl(long timestamp, string priceOfCurrency, string inCurrency) =>
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
            var fullUrl = GetBtcUrl(timestamp);
            var jsonResponseArray = WebHelper.GetJsonFromUrl(fullUrl);
            var price = (double)JArray.Parse(jsonResponseArray)[0]["weightedAverage"];

            return price;
        }

        public static PoloniexApiResponse GetPrice(DateTime dateTime, string priceOfCurrency, string inCurrency)
        {
            try
            {
                var timestampOfDate = ((DateTimeOffset)dateTime).ToUnixTimeSeconds();
                return GetPrice(timestampOfDate, priceOfCurrency, inCurrency);
            }
            catch (ArgumentOutOfRangeException e)
            {
                return new PoloniexApiResponse(null, e.Message);
            }
        }

        private static PoloniexApiResponse GetPrice(long timestamp, string priceOfCurrency, string inCurrency)
        {
            var fullUrl = GetCryptoUrl(timestamp, priceOfCurrency, inCurrency);
            var jsonResponseArray = WebHelper.GetJsonFromUrl(fullUrl);
            
            return new PoloniexApiResponse(jsonResponseArray);
        }

        public static bool PairExist(string priceOfCurrency, string inCurrency)
        {
            if (IsNullOrWhiteSpace(priceOfCurrency) || IsNullOrWhiteSpace(inCurrency))
                return false;

            var testDate = new DateTime(2018, 04, 15, 15, 32, 55);
            var timestampOfDate = ((DateTimeOffset)testDate).ToUnixTimeSeconds();
            var pairUrl = GetCryptoUrl(timestampOfDate, priceOfCurrency, inCurrency);

            var rawResponseJson = WebHelper.GetJsonFromUrl(pairUrl);
            var parsedResponse = new PoloniexApiResponse(rawResponseJson);

            return parsedResponse.Error != ErrorType.INVALID_CURRENCY_PAIR;
        }
    }
}
