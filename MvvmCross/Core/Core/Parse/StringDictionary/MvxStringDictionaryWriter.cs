// MvxStringDictionaryWriter.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Core.Parse.StringDictionary
{
    using System.Collections.Generic;
    using System.Text;

    public class MvxStringDictionaryWriter : IMvxStringDictionaryWriter
    {
        public string Write(IDictionary<string, string> dictionary)
        {
            if (dictionary == null
                || dictionary.Count == 0)
            {
                return string.Empty;
            }

            var output = new StringBuilder();
            foreach (var kvp in dictionary)
            {
                if (output.Length > 0)
                    output.Append(";");

                output.AppendFormat("{0}={1}", this.Quote(kvp.Key), this.Quote(kvp.Value));
            }
            return output.ToString();
        }

        private string Quote(string input)
        {
            if (input == null)
                return "null";

            var output = new StringBuilder(input.Length + 32 /* a small extra allowance - normally enough */);
            output.Append('\'');
            foreach (var c in input)
            {
                switch ((int)c)
                {
                    case '\\':
                        output.Append("\\\\");
                        break;

                    case '\'':
                        output.Append("\\\'");
                        break;

                    default:
                        output.Append(c);
                        break;
                }
            }
            output.Append('\'');
            return output.ToString();
        }
    }
}