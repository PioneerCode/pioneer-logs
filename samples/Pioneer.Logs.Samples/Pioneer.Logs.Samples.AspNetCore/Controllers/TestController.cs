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
        [PioneerLogsTrackUsage(ActivityName = "TestController Get")]
        public ActionResult<IEnumerable<string>> Get()
        {
            PioneerLogsTub.LogDiagnostic("Test diagnostic log....", 
                HttpContext,
                new Dictionary<string, object> { { "Test", "Parameter" } });
            throw new Exception("Force Exception");
        }
    }
}
