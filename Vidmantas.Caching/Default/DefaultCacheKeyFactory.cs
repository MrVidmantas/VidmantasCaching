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
        #region Fields

        private readonly ICacheObjectSerializer _objectSerializer;

        #endregion

        #region Constructors

        public DefaultCacheKeyFactory(ICacheObjectSerializer objectSerializer)
        {
            _objectSerializer = objectSerializer;
        }

        #endregion

        #region Interface Implementations

        public async Task<ICacheKey> CreateCacheKeyAsync(string parentKey, string valueKey, object valueKeyModifier)
        {
            var processedParentKey = ProcessParentKey(parentKey);
            var processedValueKeyModifier = await ProcessValueKeyModifierAsync(valueKeyModifier);
            var combinedValueKey = GetCombinedValueKey(valueKey, processedValueKeyModifier);

            return new CacheKey
            {
                ParentKey = processedParentKey,
                ValueKey = combinedValueKey
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
