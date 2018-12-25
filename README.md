# Pioneer Logs

[![Build Status](https://dev.azure.com/pioneer-code/Pioneer%20Logs/_apis/build/status/PioneerCode.pioneer-logs)](https://dev.azure.com/pioneer-code/Pioneer%20Logs/_build/latest?definitionId=3)
[![](https://img.shields.io/nuget/v/Pioneer.Logs.svg)](https://www.nuget.org/packages/Pioneer.Logs/g)
[![](https://img.shields.io/nuget/dt/Pioneer.Logs.svg)](https://www.nuget.org/packages/Pioneer.Logs/)


[Pioneer Logs](https://github.com/PioneerCode/pioneer-logs) is a an opinionated abstraction that sits on top of [Serilog](https://github.com/serilog/serilog).  

Typical logging frameworks categorize logs into levels like debug, trace and warning.  In many cases, this concept fails to provide a bigger picture into what you are trying to accomplish with logging and more importantly, what is happening across the bounds of multi-layered application.  Pioneer Logs attempts to solve this problem by providing a hardened contract that is log into 4 meaningful categories:

- Diagnostics
  - What is happening at this specific spot in my code?
- Usage
  - Who is using and when is this path through my code being used?
- Performance
  - How long is it taking my code to finish from point A to point B.
- Error
  - What unexpected exception was thrown? 


### Tubs

Platform specific implementation is managed through a concept known as "Tubs", a predefined set of abstractions that you configure and plug into your application.  

