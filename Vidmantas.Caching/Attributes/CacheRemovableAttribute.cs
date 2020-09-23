namespace Vidmantas.Caching.Attributes
{
    #region Usings

    using System;

    #endregion

    public sealed class CacheRemovableAttribute : Attribute
    {
        #region Constructors

        public CacheRemovableAttribute(bool withCacheKeyModifier = false)
        {
            WithCacheKeyModifier = withCacheKeyModifier;
        }

        #endregion

        #region Properties

        public bool WithCacheKeyModifier { get; set; }

        #endregion
    }
}
