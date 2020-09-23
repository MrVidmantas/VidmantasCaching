namespace Vidmantas.Caching.Helpers
{
    #region Usings

    using System.Security.Cryptography;
    using System.Text;

    #endregion

    public static class MD5Helper
    {
        #region Public Methods

        public static string CreateMd5(byte[] inputBytes)
        {
            MD5 md5 = MD5.Create();
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            var sb = new StringBuilder();

            foreach (byte thisByte in hashBytes)
            {
                sb.Append(thisByte.ToString("X2"));
            }

            return sb.ToString();
        }

        #endregion
    }
}
