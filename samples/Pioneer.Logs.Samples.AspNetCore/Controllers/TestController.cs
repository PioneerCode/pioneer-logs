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
        [PioneerLogsTrackUsage(Message = "TestController Get")]
        public ActionResult<IEnumerable<string>> Get()
        {
            PioneerLogsTub.LogDiagnostic("Hi, I am about to force an Exception.", 
                HttpContext,
                new Dictionary<string, object> { { "Test", "Parameter" } });
            throw new Exception("I just manually forced an Exception.  Enjoy!");
        }

        [HttpGet]
        [Route("api/Test")]
        [PioneerLogsTrackUsage(Message = "TestController Get")]
        public ActionResult<IEnumerable<string>> GetTest()
        {
            PioneerLogsTub.CorrelationId = Guid.NewGuid().ToString();
            PioneerLogsTub.LogUsage("RunUsageLoggingTask", HttpContext);
            PioneerLogsTub.LogDiagnostic("Some Random Message.", HttpContext);
            return Ok();
        }
    }
}
