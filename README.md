# Pioneer Logs

[![Build Status](https://dev.azure.com/pioneer-code/Pioneer%20Logs/_apis/build/status/PioneerCode.pioneer-logs)](https://dev.azure.com/pioneer-code/Pioneer%20Logs/_build/latest?definitionId=3)

[PioneerCode/pioneer-logs](https://github.com/PioneerCode/pioneer-logs) is a an opinionated abstraction over [Serilog](https://github.com/serilog/serilog) that provides an out of the box solution for error, usage, performance, and diagnostic logging. Platform and Framework specific implementation is managed through [Tubs], a predefined set of abstractions that you configure and plug into your application.  


## ASP.NET Core

### Global Exception Handling

Add `PioneerLogsConfiguration` object to your `appsettings.json` file.

```json
{
  ...
  "PioneerLogsConfiguration": {
    "ApplicationName": "Pioneer Logs",
    "ApplicationLayer": "Pioneer.Logs.Samples.AspNetCore"
  }
  ...
}
```

### Add Service

Add Pioneer Logs service in your `Startup.cs` file.

```csharp
using Pioneer.Logs.Tubs.AspNetCore;
...

public void ConfigureServices(IServiceCollection services)
{
    ...
    services.AddPioneerLogs(Configuration.GetSection("PioneerLogsConfiguration"));
    ...
}
```

### Add Middleware

#### Default

With this setup, your logs will use the [File](https://github.com/serilog/serilog-sinks-file) Serilog Sink.
 
```csharp
public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{
    ...
    app.UsePioneerLogs();
    ...
}

```

#### Configure Sinks

You can override all log writer Sinks, by providing a configuration option to the Middleware. 
For a list of Sinks and how to configure them is available at [Serilog Sinks](https://github.com/serilog/serilog/wiki/Provided-Sinks); 

```csharp
public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{
    ...
    app.UsePioneerLogs(new PioneerLogsTubConfiguration {
        PerformanceLogger = new LoggerConfiguration()
            .WriteTo.File(path: @"logs\performance-override-name.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger(),
        UsageLogger = new LoggerConfiguration()
            .WriteTo.File(path: @"logs\usage-override-name.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger(),
        ErrorLogger = new LoggerConfiguration()
            .WriteTo.File(path: @"logs\error-override-name.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger(),
        DiagnosticLogger = new LoggerConfiguration()
            .WriteTo.File(path: @"logs\diagnostic-override-name.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger()
    });
    ...
}

```