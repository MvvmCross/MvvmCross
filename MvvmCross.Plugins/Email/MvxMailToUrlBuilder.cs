// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Text;

namespace MvvmCross.Plugin.Email
{
    [Preserve(AllMembers = true)]
    public class MvxMailToUrlBuilder
    {
        public string Build(string to, string cc, string subject, string body)
        {
            var builder = new StringBuilder();
            builder.Append("mailto:" + to);

            var sep = "?";
            AddParam(builder, "cc", cc, ref sep);
            AddParam(builder, "subject", subject, ref sep);
            AddParam(builder, "body", body, ref sep);

            var url = builder.ToString();
            return url;
        }

        private static void AddParam(StringBuilder builder, string param, string value, ref string separator)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            builder.Append(separator);
            separator = "&";
            builder.Append(param);
            builder.Append("=");
            builder.Append(Uri.EscapeDataString(value));
        }
    }
}
