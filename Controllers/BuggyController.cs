using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BuggyController : BaseApiController
    {
        [HttpGet("not-found")]
        public IActionResult GetNotFound()
        {
            return NotFound();   
        }

        [HttpGet("bad-request")]
        public IActionResult GetBadRequest()
        {
            return BadRequest("This is a bad request");   
        }

        [HttpGet("unauthorised")]
        public IActionResult GetUnauthorised()
        {
            return Unauthorized();   
        }

        [HttpGet("validation-error")]
        public IActionResult GetValidationError()
        {
            ModelState.AddModelError("Problem1","This is the first error");
            ModelState.AddModelError("Problem2","This is the second error");

            return ValidationProblem();
        }

        [HttpGet("server-er ror")]
        public IActionResult GetServerError()
        {
            throw new Exception("This is a server error");
        }
    }
}