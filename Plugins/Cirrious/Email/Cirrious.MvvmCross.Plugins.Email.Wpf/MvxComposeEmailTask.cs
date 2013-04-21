using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cirrious.MvvmCross.Plugins.Email.Wpf
{
    public class MvxComposeEmailTask : IMvxComposeEmailTask
    {
        private void AddParam(StringBuilder builder, string param, string value, ref string separator)
        {
            if(!string.IsNullOrWhiteSpace(value))
            {
                builder.Append(separator);
                separator = "&";
                builder.Append(param);
                builder.Append("=");
                builder.Append(Uri.EscapeDataString(value));
            }
        }

        public void ComposeEmail(string to, string cc, string subject, string body, bool isHtml)
        {
            // hacky attempt, using mailto
            StringBuilder builder = new StringBuilder();
            builder.Append("mailto:" + to);

            string sep = "?";
            AddParam(builder, "cc", cc, ref sep);
            AddParam(builder, "subject", subject, ref sep);
            AddParam(builder, "body", body, ref sep);

            Process.Start(new ProcessStartInfo(builder.ToString()));
        }
    }
}
