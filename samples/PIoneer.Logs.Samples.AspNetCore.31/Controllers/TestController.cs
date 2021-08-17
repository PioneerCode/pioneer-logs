using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Pioneer.Logs.Tubs.AspNetCore;

namespace PIoneer.Logs.Samples.AspNetCore._31.Controllers
{
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        [Route("api/exception")]
        //[PioneerLogsTrackUsage(Message = "TestController Get")]
        public ActionResult<IEnumerable<string>> Get()
        {
            PioneerLogsTub.LogDiagnostic("Hi, I am about to force an Exception.",
                HttpContext,
                new Dictionary<string, object> { { "Test", "Parameter" } });
            throw new Exception("I just manually forced an Exception.  Enjoy!");
        }

        [HttpGet]
        [Route("api/test")]
        [PioneerLogsTrackUsage(Message = "TestController Get")]
        public ActionResult<IEnumerable<string>> GetTest()
        {
            PioneerLogsTub.CorrelationId = Guid.NewGuid().ToString();
            PioneerLogsTub.LogUsage("RunUsageLoggingTask", HttpContext);
            PioneerLogsTub.LogDiagnostic("Some Random Message.", HttpContext);
            //return Ok();
            var errors = new ModelStateDictionary();
            errors.AddModelError("date", "frick");
            return ValidationProblem(errors);
        }
    }
}