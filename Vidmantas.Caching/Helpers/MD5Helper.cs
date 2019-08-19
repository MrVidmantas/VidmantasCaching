namespace Vidmantas.Caching.Helpers
{
    #region Usings

    using System.Security.Cryptography;
    using System.Text;

    #endregion

    public static class MD5Helper
    {
        #region Public Methods

        /// <summary>
        /// Creates the MD5.
        /// </summary>
        /// <param name="inputBytes">The input bytes</param>
        public static string CreateMd5(byte[] inputBytes)
        {
            // Use input string to calculate MD5 hash
            MD5 md5 = MD5.Create();
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // Convert the byte array to hexadecimal string
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
