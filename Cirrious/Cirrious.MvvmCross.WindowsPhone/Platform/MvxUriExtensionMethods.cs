// MvxUriExtensionMethods.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;

namespace Cirrious.MvvmCross.WindowsPhone.Platform
{
    public static class MvxUriExtensionMethods
    {
        // based on http://densom.blogspot.com/2009/08/how-to-parse-query-string-without-using.html
        // assumed free to use in the public domain (it's a blog post)
        public static Dictionary<string, string> ParseQueryString(this Uri uri)
        {
            var toReturn = new Dictionary<string, string>();

            var simplePath = uri.ToString();

            string parameters;
            if (!TryGetParameterString(simplePath, out parameters))
                return toReturn;

            foreach (var vp in Regex.Split(parameters, "&"))
            {
                AddPairFrom(toReturn, vp);
            }
            return toReturn;
        }

        private static bool TryGetParameterString(string simplePath, out string parameters)
        {
            var question = simplePath.IndexOf('?');
            if (question < 0)
            {
                parameters = null;
                return false;
            }
            if (question >= (simplePath.Length - 1))
            {
                parameters = null;
                return false;
            }

            parameters = simplePath.Substring(question + 1);
            return true;
        }

        private static void AddPairFrom(Dictionary<string, string> toReturn, string vp)
        {
            string[] singlePair = Regex.Split(vp, "=");
            if (singlePair.Length == 2)
            {
                toReturn[singlePair[0]] = HttpUtility.UrlDecode(singlePair[1]);
            }
            else
            {
                toReturn[singlePair[0]] = string.Empty;
            }
        }
    }
}