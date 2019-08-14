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

        /// <summary>
        /// Convert byte array to type object.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="byteArray">The byte array.</param>
        /// <returns>Deserialized type object.</returns>
        public Task<T> FromByteArrayAsync<T>(byte[] byteArray) where T : class
        {
            var formatter = new BinaryFormatter();

            using (var stream = new MemoryStream(byteArray))
            {
                return Task.FromResult((T)formatter.Deserialize(stream));
            }
        }

        /// <summary>
        /// Converts object to by array.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>Byte array representing the object.</returns>
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
