using System;
using System.Text;

namespace Phone7.Fx.Imaging
{
    internal class Helper
    {
        internal static StringBuilder Reverse(StringBuilder stringBuilder)
        {
            if (stringBuilder.Length > 0)
            {
                char[] acPattern = stringBuilder.ToString().ToCharArray();
                Array.Reverse(acPattern);
                stringBuilder = new StringBuilder(acPattern.Length);
                stringBuilder.Append(acPattern);
            }
            return stringBuilder;
        }
    }
}