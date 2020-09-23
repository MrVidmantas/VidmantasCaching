namespace Vidmantas.Caching.Core
{
    #region Usings

    using Interfaces;

    #endregion

    public class CacheKey : ICacheKey
    {
        #region Properties

        public string ParentKey { get; set; }

        public string ValueKey { get; set; }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return $"{ParentKey}{(string.IsNullOrEmpty(ValueKey) ? string.Empty : $":{ValueKey}")}";
        }

        #endregion
    }
}
