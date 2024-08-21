using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Portfolio.API.Errors;
using Portfolio.Repository.Data;

namespace Portfolio.API.Controllers
{
   
    public class BuggyController : APIBaseController
    {
        private readonly DataDbContext _context;

        public BuggyController(DataDbContext context)
        {
            _context = context;
        }


        [HttpGet("NotFound")]
        public ActionResult GetNotFoundRequest()
        {
            var Design = _context.Designs.Find(1000);
            if (Design is null)
            {
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "Resource Not Found"));
            }

            return Ok(Design);
        }


        [HttpGet("Unauthorized")]
        public ActionResult GetUnauthorized()
        {
            return Unauthorized(new ApiResponse(401));
        }

        [HttpGet("badrequest")]
        public ActionResult GetBadRequest()
        {
            return BadRequest(new ApiResponse(400));
        }













        [HttpGet("badrequest/{id}")]
        public ActionResult GetBadRequest(int? id)
        {
            return Ok();
        }


        [HttpGet("servererror")]
        public ActionResult GetServerError()
        {
            var Design = _context.Designs.Find(1000);
            var result = Design.ToString();
            return Ok(result);

        }



    }
}
