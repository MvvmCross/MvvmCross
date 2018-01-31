// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using System.Text;

namespace MvvmCross.Plugins.ResourceLoader
{
    public abstract class MvxResourceProvider
    {
        protected static string MakeLookupKey(string namespaceKey, string typeKey)
        {
            return $"{namespaceKey}|{typeKey}";
        }

        protected static string MakeLookupKey(string namespaceKey, string typeKey, string name)
        {
            return $"{namespaceKey}|{typeKey}|{name}";
        }

        protected static string GenerateResourceNameFromPath(string path)
        {
            char[] invalidChars = { ' ', '\u00A0', '.', ',', ';',
                                    '|', '~', '@', '#', '%',
                                    '^', '&', '*', '+', '-',
                                    '/', '\\', '<', '>', '?',
                                    '[', ']', '(', ')', '{',
                                    '}', '"', '\'', ':', '!' };
            StringBuilder builder = new StringBuilder();

            if (string.IsNullOrEmpty(path))
            {
                builder.Append("_");
            }
            else
            {
                string[] pathParts = path.Replace('/', '.').Split('.');

                foreach (string part in pathParts)
                {
                    if (part.Length > 0)
                    {
                        if (part[0] >= '0' && part[0] <= '9')
                        {
                            builder.Append('_');
                        }

                        builder.Append(string.Join("_", part.Split(invalidChars)));
                        builder.Append('.');
                    }
                }
            }

            builder.Remove(builder.Length - 1, 1);
            return builder.ToString();
        }
    }
}