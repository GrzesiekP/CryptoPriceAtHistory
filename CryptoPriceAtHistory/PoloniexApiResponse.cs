using System;
using CryptoPriceAtHistory.Enums;
using Newtonsoft.Json.Linq;

namespace CryptoPriceAtHistory
{
    public class PoloniexApiResponse : BaseResponse
    {
        private const string WEIGHTED_AVERAGE_KEY = "weightedAverage";
        private const string ERROR_KEY = "error";
        private const string DATE_KEY = "date";

        private readonly string _rawContent;
        public double Price { get; set; }

        public PoloniexApiResponse(string rawJson) : base(false)
        {
            _rawContent = rawJson;
            CheckError();
            if (!IsSuccess)
            {
                if (Error == ErrorType.UNKNOWN_ERROR)
                    ParseErrorMessage();
            }
            else
            {
                ParsePrice();
            }
        }

        public PoloniexApiResponse(string rawJson, string errorMessage) : base(false)
        {
            _rawContent = rawJson;
            Error = ErrorType.UNKNOWN_ERROR;
            ErrorMessage = errorMessage;
        }

        private void CheckError()
        {
            IsSuccess = false;
            if (_rawContent.Contains(
                ErrorDictionaries.PoloniexErrors()[ErrorType.INVALID_CURRENCY_PAIR]
                ))
            {
                Error = ErrorType.INVALID_CURRENCY_PAIR;
                ParseErrorMessage();
            }
            else if (FetchedDateIsInvalid(_rawContent))
            {
                Error = ErrorType.INVALID_DATE;
                ErrorMessage = ErrorDictionaries.PoloniexErrors()[Error];
            }
            else if (_rawContent.Contains(ERROR_KEY))
            {
                Error = ErrorType.UNKNOWN_ERROR;
            }
            else
            {
                IsSuccess = true;
            }
        }

        private void ParseErrorMessage()
        {
            try
            {
                var jObject = JObject.Parse(_rawContent);
                ErrorMessage = (string)jObject[ERROR_KEY];
            }
            catch (Exception e)
            {
                Error = ErrorType.UNKNOWN_ERROR;
                ErrorMessage = e.Message;
            }
        }

        private void ParsePrice()
        {
            try
            {
                Price = (double) JArray.Parse(_rawContent)[0][WEIGHTED_AVERAGE_KEY];
            }
            catch (Exception e)
            {
                Error = ErrorType.UNKNOWN_ERROR;
                ErrorMessage = e.Message;
            }
        }

        private bool FetchedDateIsInvalid(string rawJson)
        {
            try
            {
                var date = (int) JArray.Parse(rawJson)[0][DATE_KEY];
                return date == 0;
            }
            catch (Exception e)
            {
                return true;
            }
        }
    }
}
