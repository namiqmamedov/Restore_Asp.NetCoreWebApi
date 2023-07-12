using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BuggyController : BaseApiController
    {
        [HttpGet]
        public IActionResult GetNotFound()
        {
            return NotFound();   
        }
                [HttpGet]
        public IActionResult GetBadRequest()
        {
            return NotFound();   
        }
        //         [HttpGet]
        // public IActionResult GetNotFound()
        // {
        //     return NotFound();   
        // }
        //         [HttpGet]
        // public IActionResult GetNotFound()
        // {
        //     return NotFound();   
        // }
        //         [HttpGet]
        // public IActionResult GetNotFound()
        // {
        //     return NotFound();   
        // }
    }
}