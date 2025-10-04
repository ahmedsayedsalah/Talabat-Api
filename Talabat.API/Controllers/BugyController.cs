using Talabat.API.Errors;
using Talabat.DataAccess.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Talabat.API.Controllers
{
    public class BugyController : ApiBaseController
    {
        private readonly AppDbContext dbContext;

        public BugyController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet("notfound")]
        public ActionResult GetNotFoundRequest()
        {
            var product = dbContext.Products.Find(200);

            if (product is null) return NotFound(new ApiResponse(404));

            return Ok(product);
        }
        [HttpGet("servererror")]
        public ActionResult GetServerErrorRequest()
        {
            var product = dbContext.Products.Find(200);

            var productToReturn = product.ToString();

            return Ok(productToReturn);
        }
        [HttpGet("badrequest")]
        public ActionResult GetBadRequest()
        {

            return BadRequest(new ApiResponse(400));
        }
        [HttpGet("badrequest/{id}")]
        public ActionResult GetBadRequest(int id) // Validation Error
        {

            return Ok();
        }
    }
}
