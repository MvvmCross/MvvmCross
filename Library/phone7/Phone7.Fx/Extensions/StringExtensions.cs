namespace Phone7.Fx.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Encodes the string to Base64.
        /// </summary>
        /// <param name="toEncode">To encode.</param>
        /// <returns></returns>
        public static string EncodeTo64(this string toEncode)
        {
            byte[] toEncodeAsBytes = System.Text.Encoding.UTF8.GetBytes(toEncode);
            string returnValue = System.Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;
        }

        /// <summary>
        /// Decodes the string from Base64.
        /// </summary>
        /// <param name="encodedData">The encoded data.</param>
        /// <returns></returns>
        public static string DecodeFrom64(this string encodedData)
        {
            byte[] encodedDataAsBytes = System.Convert.FromBase64String(encodedData);
            string returnValue = System.Text.Encoding.UTF8.GetString(encodedDataAsBytes, 0, encodedDataAsBytes.Length);
            return returnValue;
        }
    }
}