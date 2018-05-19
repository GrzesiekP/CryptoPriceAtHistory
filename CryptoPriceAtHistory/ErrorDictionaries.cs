using System.Collections.Generic;
using CryptoPriceAtHistory.Enums;

namespace CryptoPriceAtHistory
{
    public static class ErrorDictionaries
    {
        public static Dictionary<ErrorType?, string> PoloniexErrors() => new Dictionary<ErrorType?, string>
            {
                {ErrorType.INVALID_CURRENCY_PAIR, "Invalid currency pair."},
                {ErrorType.INVALID_DATE, "Invalid date."}
            };
    }
}
