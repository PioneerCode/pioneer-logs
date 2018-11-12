# Pioneer Logs

[![Build Status](https://dev.azure.com/pioneer-code/Pioneer%20Logs/_apis/build/status/PioneerCode.pioneer-logs)](https://dev.azure.com/pioneer-code/Pioneer%20Logs/_build/latest?definitionId=3)

[Pioneer Logs](https://github.com/PioneerCode/pioneer-logs) is a an opinionated abstraction over [Serilog](https://github.com/serilog/serilog) that provides an out of the box solution for error, usage, performance, and diagnostic logging. Platform and Framework specific implementation is managed through a concept known as "Tubs", a predefined set of abstractions that you configure and plug into your application.  


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
A list of Sinks and how to configure them is available at [Serilog Sinks](https://github.com/serilog/serilog/wiki/Provided-Sinks).

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

### Add-Hoc Usage Tracking

You can supply a `PioneerLogsTrackUsage` attribute to any of your methods that you would like to track usage on.

```csharp
[HttpGet]
[Route("api/exception")]
[PioneerLogsTrackUsage(Message = "Exception Get")]
public ActionResult<IEnumerable<string>> Get()
{
    throw new Exception("Force Exception");
}
```


### Global Performance tracking

You can apply a the `PioneerLogsPerformanceFilter` filter global to track performance.

```csharp
public void ConfigureServices(IServiceCollection services)
{
    ...
    services.AddMvc(options => 
            options.Filters.Add(new PioneerLogsPerformanceFilter()))
        .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
    ...
}
```