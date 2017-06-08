// MvxMailToUrlBuilder.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Text;

namespace MvvmCross.Plugins.Email
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