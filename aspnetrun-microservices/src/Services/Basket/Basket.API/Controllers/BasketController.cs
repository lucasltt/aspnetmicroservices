using Basket.API.Entities;
using Basket.API.GrpcServices;
using Basket.API.Repositories;
using Discount.Grpc.Protos;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _repository;
        private readonly DiscountGrpcService _discountGrpcService;

        public BasketController(IBasketRepository repositoy, DiscountGrpcService discountGrpcService)
        {
            _repository = repositoy;
            _discountGrpcService = discountGrpcService;
        }

        [HttpDelete("{username}", Name = "DeleteBasketAsync")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteBasketAsync(string userName)
        {
            await _repository.DeleteBasketAsync(userName);
            return Ok();
        }

        [HttpGet("{username}", Name = "GetBasketAsync")]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> GetBasketAsync(string userName)
        {
            ShoppingCart basket = await _repository.GetBasketAsync(userName);
            return Ok(basket ?? new ShoppingCart(userName));

        }

        [HttpPost]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> CreateOrUpdateBasketAsync([FromBody] ShoppingCart basket)
        {
            //Communicate with Discount.Grps and calculate the latest prices of the products into the shopping cart
            
            foreach(ShoppingCartItem item in basket.Items)
            {
                CouponModel coupon = await _discountGrpcService.GetDiscountAsync(item.ProductName);
                item.Price -= coupon.Amount;
            }
            return Ok(await _repository.CreateOrUpdateBasketAsync(basket));

        }
    }
}
