namespace Vidmantas.Caching.Core
{
    #region Usings

    using Interfaces;

    #endregion

    public class CacheKey : ICacheKey
    {
        #region Properties

        /// <summary>
        /// Gets or sets the parent key.
        /// </summary>
        /// <value>
        /// The parent key.
        /// </value>
        public string ParentKey { get; set; }

        /// <summary>
        /// Gets or sets the value key.
        /// </summary>
        /// <value>
        /// The value key.
        /// </value>
        public string ValueKey { get; set; }

        /// <summary>
        /// Gets or sets the value key modifier.
        /// </summary>
        /// <value>
        /// The value key modifier.
        /// </value>
        public object ValueKeyModifier { get; set; }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return $"{ParentKey}:{ValueKey}";
        }

        #endregion
    }
}
