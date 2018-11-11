using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Pioneer.Logs.Tubs.AspNetCore;

namespace Pioneer.Logs.Samples.AspNetCore.Controllers
{
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        [Route("api/exception")]
        [PioneerLogsTrackUsageFactory(ActivityName = "Something")]
        public ActionResult<IEnumerable<string>> Get()
        {
            throw new Exception("Force Exception");
        }
    }
}
