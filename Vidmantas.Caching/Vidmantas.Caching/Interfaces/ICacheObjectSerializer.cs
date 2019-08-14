namespace Vidmantas.Caching.Interfaces
{
    #region Usings

    using System.Threading.Tasks;

    #endregion

    public interface ICacheObjectSerializer
    {
        #region Public Methods

        /// <summary>
        /// Convert byte array to type object.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="byteArray">The byte array.</param>
        /// <returns>Deserialized type object.</returns>
        Task<T> FromByteArrayAsync<T>(byte[] byteArray) where T : class;

        /// <summary>
        /// Converts object to by array.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>Byte array representing the object.</returns>
        Task<byte[]> ToByteArrayAsync(object obj);

        #endregion
    }
}
