using Microsoft.Extensions.Caching.Memory;

namespace SensitiveWordsAPI.Helpers
{
    public class MemoryCacheHelper
    {
        private IMemoryCache _memoryCache;
        public MemoryCacheHelper(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        public void SetMemoryCache(string memoryKey, IEnumerable<string> sensitiveWords, int duration)
        {
            _memoryCache.Set(memoryKey, sensitiveWords, TimeSpan.FromMinutes(duration));
        }

        public bool TryGetValue(string memoryKey, out IEnumerable<string> sensitiveWords)
        {
            return _memoryCache.TryGetValue(memoryKey, out sensitiveWords);
        }
    }
}
