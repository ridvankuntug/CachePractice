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
        public string GetCache(int switchKey)
        {
            if (switchKey == 1)
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
            else if(switchKey == 2)
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
            else if(switchKey == 3)
            {
                // Look for cache key.
                if (_cache.TryGetValue("timeNow", out DateTime time))
                {
                    return " Cache: " + _cache.Get("timeNow") + "\n Şimdiki zaman: " + DateTime.Now;
                }
                else
                {
                    // Key not in cache, so get data.
                    time = DateTime.Now;

                    // Set cache options.
                    MemoryCacheEntryOptions CacheOptions = new MemoryCacheEntryOptions();
                    //Absolute Expiration
                    CacheOptions.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30);
                    //Sliding Expiration
                    CacheOptions.SlidingExpiration = TimeSpan.FromSeconds(10);
                    _cache.Set("timeNow", time, CacheOptions);

                    return "Cache boş, şimdiki zaman: " + time + " Cache Absolute ve Sliding olarak dolduruldu.";
                }
            }
            else
            {
                return "1 ile 3 arasında bir değer seçin";
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
