using System.Diagnostics;
using System.Text;

namespace Cirrious.MvvmCross.Plugins.Email.Wpf
{
    public class MvxComposeEmail : IMvxComposeEmailTask
    {
        public void ComposeEmail(string to, string cc, string subject, string body, bool isHtml)
        {
            StringBuilder builder = new StringBuilder("mailto:");

            builder.AppendFormat("{0}", to);

            bool firstArgument = true;

            if (!string.IsNullOrWhiteSpace(cc))
            {
                builder.AppendFormat("{0}cc={1}", firstArgument ? "?" : "&", cc);
                firstArgument = false;
            }

            if (!string.IsNullOrWhiteSpace(subject))
            {
                builder.AppendFormat("{0}subject={1}", firstArgument ? "?" : "&", subject);
                firstArgument = false;
            }

            if (!string.IsNullOrWhiteSpace(body))
            {
                builder.AppendFormat("{0}body={1}", firstArgument ? "?" : "&", body);
                firstArgument = false;
            }

            Process.Start(builder.ToString());
        }
    }
}
