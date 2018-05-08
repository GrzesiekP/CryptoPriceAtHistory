using Newtonsoft.Json.Linq;
using System;

namespace CryptoPriceAtHistory.BusinessLogic
{
    public static class PoloniexApi
    {
        private static string getUrl(long timestamp) => $"https://poloniex.com/public?command=returnChartData&currencyPair=USDT_BTC&start={timestamp - 300}&end={timestamp}&period=300";

        public static double GetBitcoinPrice(DateTime dateTime)
        {
            if (dateTime > DateTime.Now)
                return 0;

            var timestampOfDate = ((DateTimeOffset)dateTime).ToUnixTimeSeconds();
            return GetBitcoinPrice(timestampOfDate);
        }

        private static double GetBitcoinPrice(long timestamp)
        {
            var fullUrl = getUrl(timestamp);
            var jsonResponseArray = WebHelper.GetJsonFromUrl(fullUrl);
            var price = (double)JArray.Parse(jsonResponseArray)[0]["weightedAverage"];

            return price;
        }
    }
}
