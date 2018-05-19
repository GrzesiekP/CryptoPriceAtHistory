using System;
using CryptoPriceAtHistory.BusinessLogic;
using CryptoPriceAtHistory.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CryptoPriceAtHistory.Controllers
{
    [Route("api/price")]
    public class PriceController : Controller
    {
        [HttpGet("{datetime}")]
        public IActionResult GetPrice(DateTime dateTime)
        {
            var price = PoloniexApi.GetBitcoinPrice(dateTime);

            if (price == 0)
                return BadRequest();

            return Ok(price);
        }

        [HttpGet("atDate")]
        public IActionResult GetCryptoPrice(DateTime dateTime, string priceOfCurrency, string inCurrency)
        {
            var response = PoloniexApi.GetPrice(dateTime, priceOfCurrency, inCurrency);

            // ReSharper disable once InvertIf
            if (!response.IsSuccess)
            {
                if (response.Error == ErrorType.UNKNOWN_ERROR)
                    // log message
                    return StatusCode(StatusCodes.Status500InternalServerError);

                return BadRequest(response.ErrorMessage);
            }

            return Ok(response.Price);
        }
    }
}
