// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Text;

namespace MvvmCross.Core.Parse.StringDictionary
{
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

                output.AppendFormat("{0}={1}", Quote(kvp.Key), Quote(kvp.Value));
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