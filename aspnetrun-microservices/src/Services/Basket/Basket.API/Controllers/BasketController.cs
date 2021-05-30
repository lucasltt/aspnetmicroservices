using Basket.API.Entities;
using Basket.API.Repositories;
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

        public BasketController(IBasketRepository repositoy)
        {
            _repository = repositoy;
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

            return Ok(await _repository.CreateOrUpdateBasketAsync(basket));

        }
    }
}
