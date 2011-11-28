using System;
using System.IO;
using System.Text;

namespace Phone7.Fx.Extensions
{
    public static class StreamReaderExtensions
    {
        /// <summary>
        /// Read the stream from the beginning to the end
        /// </summary>
        /// <param name="rdr">The RDR.</param>
        /// <returns></returns>
        public static string ReadToEndEx(this StreamReader rdr)
        {
            StringBuilder sb = new StringBuilder();
            while (!rdr.EndOfStream)
            {
                sb.Append(Convert.ToChar(rdr.Read()));
            }
            return sb.ToString();
        }
    }
}