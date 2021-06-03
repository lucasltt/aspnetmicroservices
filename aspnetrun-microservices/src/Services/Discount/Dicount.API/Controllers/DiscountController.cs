using Dicount.API.Entities;
using Dicount.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Dicount.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountRepository _repository;

        public DiscountController(IDiscountRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{productName}", Name = "GetDiscount")]
        [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Coupon>> GetDiscountAsync(string productName)
        {

            Coupon coupon = await _repository.GetDiscountAsync(productName);
            return Ok(coupon);
          
        }


        [HttpPost]
        [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Coupon>> CreateDiscountAsync([FromBody] Coupon coupon)
        {
            bool responseResult = await _repository.CreateDiscountAsync(coupon);
            return CreatedAtRoute("GetDiscount", new { productName = coupon.ProductName }, coupon);

        }

        [HttpDelete("{productName}", Name = "DeleteDiscount")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<bool>> DeleteDiscountAsync(string productName)
        {
            return Ok(await _repository.DeleteDiscountAsync(productName));
        }

        [HttpPut]
        [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Coupon>> UpdateDiscountAsync(Coupon coupon)
        {
            bool responseResult = await _repository.UpdateDiscountAsync(coupon);
            return CreatedAtRoute("GetDiscount", new { productName = coupon.ProductName }, coupon);
        }
    }
}
