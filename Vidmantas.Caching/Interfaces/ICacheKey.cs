namespace Vidmantas.Caching.Interfaces
{
    public interface ICacheKey
    {
        #region Properties

        /// <summary>
        /// Gets or sets the parent key.
        /// </summary>
        /// <value>
        /// The parent key.
        /// </value>
        string ParentKey { get; set; }

        /// <summary>
        /// Gets or sets the value key.
        /// </summary>
        /// <value>
        /// The value key.
        /// </value>
        string ValueKey { get; set; }

        /// <summary>
        /// Gets or sets the value key modifier.
        /// </summary>
        /// <value>
        /// The value key modifier.
        /// </value>
        object ValueKeyModifier { get; set; }

        #endregion
    }
}
