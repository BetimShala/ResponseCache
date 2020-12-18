# ResponseCache
<img src="https://api.nuget.org/v3-flatcontainer/responsecache/1.0.4/icon" width="100" height="100" alt="ResponseCache">

[![NuGet](https://img.shields.io/badge/nuget-v1.0.5-blue)](https://www.nuget.org/packages/ResponseCache/)

### What is ResponseCache?
ResponseCache is a lightweight library that is used to cache API responses by simply putting **ResponseCache** attribute to the endpoint you want to cache.

### Setup

#### NuGet install:
```
Install-Package ResponseCache
```

#### Memory Cache configuration
```csharp
services.AddMemoryCache();
services.AddInMemoryResponseCache();
```

#### Redis Cache configuration
First we bind cache settings with configuration values from appsettings.json then we add the service

```csharp

var cacheSettings = new CacheSettings();
Configuration.GetSection(nameof(CacheSettings)).Bind(cacheSettings);
services.AddSingleton(cacheSettings);

services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect(cacheSettings.ConnectionString));
services.AddDistributedRedisCache(options => options.Configuration = cacheSettings.ConnectionString);

services.AddDistributedResponseCache();
```
Add this to appsettings.json
```json
  "CacheSettings": {
    "ConnectionString": "localhost:6379"
  },
```

### Usage
``` [ResponseCache(TTL)] ``` attribute where ``` TTL ``` is an integer representing cache living time in seconds.

#### Example
e.g. Cache response list of products for 1 minute
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
