using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;

namespace CachePractice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IMemoryCacheController : Controller
    {
        private IMemoryCache _cache;
        public IMemoryCacheController(IMemoryCache cache)
        {
            _cache = cache;
        }

        [HttpGet("{switchKey}")]
        public string GetCache(bool switchKey)
        {
            if (switchKey)
            {
                // Look for cache key.
                if (_cache.TryGetValue("timeNow", out DateTime time))
                {
                    return " Cache: " + time + "\n Şimdiki zaman: " + DateTime.Now;
                }
                else
                {
                    // Key not in cache, so get data.
                    time = DateTime.Now;

                    // Set cache options.
                    MemoryCacheEntryOptions CacheOptions = new MemoryCacheEntryOptions();
                    //Sliding Expiration
                    CacheOptions.SlidingExpiration = TimeSpan.FromSeconds(30);
                    _cache.Set("timeNow", time, CacheOptions);

                    return "Cache boş, şimdiki zaman: " + time + " Cache Sliding olarak dolduruldu.";
                }
            }
            else
            {
                // Look for cache key.
                if (_cache.TryGetValue("timeNow", out DateTime time))
                {
                    return " Cache: " + time + "\n Şimdiki zaman: " + DateTime.Now;
                }
                else
                {
                    // Key not in cache, so get data.
                    time = DateTime.Now;

                    // Set cache options.
                    MemoryCacheEntryOptions CacheOptions = new MemoryCacheEntryOptions();
                    //Absolute Expiration
                    CacheOptions.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30);
                    _cache.Set("timeNow", time, CacheOptions);

                    return "Cache boş, şimdiki zaman: " + time + " Cache Absolute olarak dolduruldu.";
                }
            }
        }

        [HttpDelete]
        public IActionResult DeleteCache()
        {
            if (_cache.TryGetValue("timeNow", out DateTime time))
            {
                _cache.Remove("timeNow");
                return Ok();
            }
            else
            {
                return BadRequest("Cache yok");
            }
        }
    }
}
