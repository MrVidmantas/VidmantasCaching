namespace Vidmantas.Caching.Attributes
{
    #region Usings

    using System;

    #endregion

    /// <summary>
    /// Signals that a particular method can be removed from the cache when BustCacheAsync is called.
    /// </summary>
    public sealed class CacheRemovableAttribute : Attribute
    {
        #region Constructors

        public CacheRemovableAttribute(bool withCacheKeyModifier = false)
        {
            WithCacheKeyModifier = withCacheKeyModifier;
        }

        #endregion

        #region Properties

        /// <summary>
        /// This property is used to determine whether a cache key modifier should be used when busting the cache.
        /// <para>The default is false which means - DO NOT pass any modifier to RemoveCacheAsync. This is primarily used to bust lists of things that have a complex object as the cache key modifier.</para>
        /// <para>Setting to true indicates that we DO pass a modifier to cache remove. This is most commonly used for cache entries that have a simple cache key modifier like an item id.</para>
        /// </summary>
        public bool WithCacheKeyModifier { get; set; }

        #endregion
    }
}
