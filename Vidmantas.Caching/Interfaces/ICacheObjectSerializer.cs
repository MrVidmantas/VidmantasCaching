namespace Vidmantas.Caching.Interfaces
{
    #region Usings

    using System.Threading.Tasks;

    #endregion

    public interface ICacheObjectSerializer
    {
        #region Public Methods

        Task<T> FromByteArrayAsync<T>(byte[] byteArray) where T : class;

        Task<byte[]> ToByteArrayAsync(object obj);

        #endregion
    }
}
