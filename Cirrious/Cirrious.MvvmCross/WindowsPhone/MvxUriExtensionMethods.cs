#region Copyright
// <copyright file="MvxUriExtensionMethods.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;

namespace Cirrious.MvvmCross.WindowsPhone
{
    public static class MvxUriExtensionMethods
    {
        // based on http://densom.blogspot.com/2009/08/how-to-parse-query-string-without-using.html
        // assumed free to use in the public domain (it's a blog post)
        public static Dictionary<string,string> ParseQueryString(this Uri uri)
        {
            var toReturn = new Dictionary<string, string>();

            var simplePath = uri.ToString();
            var question = simplePath.IndexOf('?');
            if (question < 0)
                return toReturn;
            if (question >= (simplePath.Length - 1))
                return toReturn;

            var parameters = simplePath.Substring(question + 1);

            foreach (var vp in Regex.Split(parameters, "&"))
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
            return toReturn;
        }
    }
}