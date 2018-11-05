using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Pioneer.Logs.Samples.AspNetCore.Controllers
{

    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        [Route("api/values")]
        public ActionResult<IEnumerable<string>> Get()
        {
            throw new Exception("Tester");
        }
    }
}
