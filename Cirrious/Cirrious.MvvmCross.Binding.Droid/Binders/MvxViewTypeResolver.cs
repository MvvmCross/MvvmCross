// MvxViewTypeResolver.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Views;
using Cirrious.CrossCore.Platform;

namespace Cirrious.MvvmCross.Binding.Droid.Binders
{
    public class MvxViewTypeResolver : IMvxViewTypeResolver
    {
        private readonly Dictionary<string, Type> _cache = new Dictionary<string, Type>();

        public IDictionary<string, string> ViewNamespaceAbbreviations { get; set; }

        #region IMvxViewTypeResolver Members

        public virtual Type Resolve(string tagName)
        {
            Type toReturn;
            if (_cache.TryGetValue(tagName, out toReturn))
                return toReturn;

            var unabbreviatedTagName = UnabbreviateTagName(tagName);

            var longLowerCaseName = GetLookupName(unabbreviatedTagName);
            var viewType = typeof (View);

            // Note - AppDomain.CurrentDomain.GetAssemblies only shows the loaded assemblies
            // so we might miss controls if not already loaded
            var query = from assembly in AppDomain.CurrentDomain.GetAssemblies()
                        from type in assembly.GetTypes()
                        where viewType.IsAssignableFrom(type)
                        where (type.FullName ?? "-").ToLowerInvariant() == longLowerCaseName
                        select type;

            toReturn = query.FirstOrDefault();
            _cache[tagName] = toReturn;

            return toReturn;
        }

        private string UnabbreviateTagName(string tagName)
        {
            var filteredTagName = tagName;
            if (ViewNamespaceAbbreviations != null)
            {
                var split = tagName.Split(new[] {'.'}, 2, StringSplitOptions.RemoveEmptyEntries);
                if (split.Length == 2)
                {
                    var abbreviate = split[0];
                    string fullName;
                    if (ViewNamespaceAbbreviations.TryGetValue(abbreviate, out fullName))
                    {
                        filteredTagName = fullName + "." + split[1];
                    }
                    else
                    {
                        MvxBindingTrace.Trace(MvxTraceLevel.Diagnostic, "Abbreviation not found {0}", abbreviate);
                    }
                }
            }
            return filteredTagName;
        }

        #endregion

        protected string GetLookupName(string tagName)
        {
            var nameBuilder = new StringBuilder();

            switch (tagName)
            {
                case "View":
                case "ViewGroup":
                    nameBuilder.Append("android.view.");
                    break;

                default:
                    if (!IsFullyQualified(tagName))
                        nameBuilder.Append("android.widget.");
                    break;
            }

            nameBuilder.Append(tagName);
            return nameBuilder.ToString().ToLowerInvariant();
        }

        private static bool IsFullyQualified(string tagName)
        {
            return tagName.Contains(".");
        }
    }
}