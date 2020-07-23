# ResponseCache
<img src="https://api.nuget.org/v3-flatcontainer/responsecache/1.0.3/icon" width="100" height="100" alt="ResponseCache">

[![NuGet](https://img.shields.io/badge/nuget-v1.0.3-blue)](https://www.nuget.org/packages/ResponseCache/)

### What is ResponseCache?
ResponseCache is a library that is used to cache responses by simply putting **ResponseCache** attribute to the endpoint you want to cache.

### Setup

#### NuGet install:
```
Install-Package ResponseCache
```

#### Configure appsettings.json
Add this to appsettings.json
```json
  "CacheSettings": {
    "ConnectionString": "localhost:6379"
  },
```
#### Startup.cs code:
First, bind cache settings with configuration values from appsettings.json then add the service
```csharp
var cacheSettings = new CacheSettings(); //will be used when configuring distributed cache
Configuration.GetSection(nameof(CacheSettings)).Bind(cacheSettings);
services.AddSingleton(cacheSettings);
services.AddResponseCache();
```

#### Redis Cache configuration
```csharp
services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect(cacheSettings.ConnectionString));
services.AddDistributedRedisCache(options => options.Configuration = cacheSettings.ConnectionString);
```

### Usage
``` [ResponseCache(TTL)] ``` attribute where ``` TTL ``` is an integer representing cache living time in seconds.

#### Example
e.g. Cache list of products response for 1 minute
```csharp
[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
    private readonly DbContext _context;

    public ProductsController(DbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [ResponseCache(60)]
    public async Task<IActionResult> List()
    {
        return Ok(await _context.Products.ToListAsync());
    }
}     
```
