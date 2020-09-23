namespace Vidmantas.Caching.Default
{
    #region Usings

    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Threading.Tasks;
    using Interfaces;

    #endregion

    public class DefaultCacheObjectSerializer : ICacheObjectSerializer
    {
        #region Interface Implementations

        public Task<T> FromByteArrayAsync<T>(byte[] byteArray) where T : class
        {
            var formatter = new BinaryFormatter();

            using (var stream = new MemoryStream(byteArray))
            {
                return Task.FromResult((T)formatter.Deserialize(stream));
            }
        }

        public Task<byte[]> ToByteArrayAsync(object obj)
        {
            var formatter = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, obj);
                return Task.FromResult(stream.ToArray());
            }
        }

        #endregion
    }
}
