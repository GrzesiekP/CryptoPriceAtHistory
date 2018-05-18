using System;
using CryptoPriceAtHistory.BusinessLogic;
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
    }
}
