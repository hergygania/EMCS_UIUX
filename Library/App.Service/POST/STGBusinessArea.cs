using App.Data.Caching;

namespace App.Service.POST
{
    public static class StgBusinessArea
    {
        public const string CacheName = "App.POST.STGBusinessArea";

        public readonly static ICacheManager CacheManager = new MemoryCacheManager();


    }
}
