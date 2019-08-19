namespace Vidmantas.Caching.Default
{
    #region Usings

    using System.IO;
    using System.Threading.Tasks;
    using Core;
    using Helpers;
    using Interfaces;

    #endregion

    public class DefaultCacheKeyFactory : ICacheKeyFactory
    {
        private readonly ICacheObjectSerializer _objectSerializer;

        public DefaultCacheKeyFactory(ICacheObjectSerializer objectSerializer)
        {
            _objectSerializer = objectSerializer;
        }

        #region Interface Implementations

        /// <summary>
        /// Creates the cache key.
        /// </summary>
        /// <param name="parentKey">The parent key.</param>
        /// <param name="valueKey">The value key.</param>
        /// <param name="valueKeyModifier">The value key modifier.</param>
        /// <returns><see cref="Task{ICacheKey}"/></returns>
        public async Task<ICacheKey> CreateCacheKeyAsync(string parentKey, string valueKey, object valueKeyModifier)
        {
            var processedParentKey = ProcessParentKey(parentKey);
            var processedValueKeyModifier = await ProcessValueKeyModifierAsync(valueKeyModifier);
            var combinedValueKey = GetCombinedValueKey(valueKey, processedValueKeyModifier);

            return new CacheKey
            {
                ParentKey = processedParentKey,
                ValueKey = combinedValueKey,
                ValueKeyModifier = valueKeyModifier
            };
        }

        #endregion

        #region Private Methods

        private string GetCombinedValueKey(string valueKey, string processedValueKeyModifier)
        {
            return $"{valueKey}{ (string.IsNullOrEmpty(processedValueKeyModifier) ? string.Empty : $".{processedValueKeyModifier}") }";
        }

        private string ProcessParentKey(string parentKey)
        {
            return !string.IsNullOrEmpty(parentKey) ? Path.GetFileNameWithoutExtension(parentKey) : string.Empty;
        }

        private async Task<string> ProcessValueKeyModifierAsync(object valueKeyModifier)
        {
            return valueKeyModifier is null ? string.Empty : MD5Helper.CreateMd5(await _objectSerializer.ToByteArrayAsync(valueKeyModifier));
        }

        #endregion 
    }
}
