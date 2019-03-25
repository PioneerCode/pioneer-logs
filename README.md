# Pioneer Logs

[![Build Status](https://dev.azure.com/pioneer-code/Pioneer%20Logs/_apis/build/status/PioneerCode.pioneer-logs)](https://dev.azure.com/pioneer-code/Pioneer%20Logs/_build/latest?definitionId=3)
[![](https://img.shields.io/nuget/v/Pioneer.Logs.svg)](https://www.nuget.org/packages/Pioneer.Logs/g)
[![](https://img.shields.io/nuget/dt/Pioneer.Logs.svg)](https://www.nuget.org/packages/Pioneer.Logs/)


[Pioneer Logs](https://github.com/PioneerCode/pioneer-logs) is an opinionated abstraction that sits on top of [Serilog](https://github.com/serilog/serilog).  

Typical logging frameworks categorize logs into levels like debug, trace and warning.  In many cases, this concept fails to provide a bigger picture into what you are trying to accomplish with logging and more importantly, what is happening across the bounds of multi-layered application.  Pioneer Logs attempts to solve this problem by providing a [hardened contract](https://github.com/PioneerCode/pioneer-logs/blob/master/src/Pioneer.Logs/Models/PioneerLog.cs) that is logged into 4 meaningful categories:

- Diagnostics
  - What is happening at this specific spot in my code?
- Usage
  - Who is using and when is this path through my code being used?
- Performance
  - How long is it taking my code to finish from point A to point B?
- Error
  - What unexpected exception was thrown? 

Example log...
```json
{
    "Id": "7d6874b5-ddab-4625-8eda-c158fe7f78d0",
    "CreationTimestamp": "2018-12-21T19:55:06.2740723Z",
    "ApplicationName": "Pioneer Logs",
    "ApplicationLayer": "Samples.AspNetCore",
    "ApplicationLocation": "/api/exception",
    "Message": null,
    "Hostname": "DESKTOP-NOMUMK6",
    "UserId": "",
    "Username": "",
    "PerformanceElapsedMilliseconds": null,
    "Category": null,
    "Exception": {
        "Message": "I just manually forced an Exception.  Enjoy!",
        "Data": [],
        "InnerException": null,
        "TargetSite": "Microsoft.AspNetCore.Mvc.ActionResult`1[System.Collections.Generic.IEnumerable`1[System.String]] Get()",
        "StackTrace": "   at Pioneer.Logs.Samples.AspNetCore.Controllers.TestController.Get() in C:\\source\\pioneer-logs\\samples\\Pioneer.Logs.Samples.AspNetCore\\Controllers\\TestController.cs:line 19\r\n   at lambda_method(Closure , Object , Object[] )\r\n   at Microsoft.Extensions.Internal.ObjectMethodExecutor.Execute(Object target, Object[] parameters)\r\n   at 
        .... shortened for space
        ",
        "HelpLink": null,
        "Source": "Pioneer.Logs.Samples.AspNetCore",
        "HResult": -2146233088,
        "$type": "Exception"
    },
    "InnermostExceptionMessage": "I just manually forced an Exception.  Enjoy!",
    "CorrelationId": "0HLJ7FOGSGV53:00000001",
    "AdditionalInfo": {
        "UserAgent": [
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/71.0.3578.98 Safari/537.36"
        ],
        "Languages": [
            "en-US,en;q=0.9" 
        ]
    },
    "$type": "PioneerLog"
}

```


### Tubs

To get started you need to add a "Tub" to your application. A Tub is a predefined set of abstractions that you configure and plug into your application.  

Current tubs....

1) [ASP.NET Core](https://github.com/PioneerCode/pioneer-logs/wiki/ASP.NET-Core)
2) [.NET Core Console Application](https://github.com/PioneerCode/pioneer-logs/wiki/.NET-Core-Console-Application)

