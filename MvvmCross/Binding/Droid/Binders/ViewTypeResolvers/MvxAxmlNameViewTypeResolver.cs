// MvxAxmlNameViewTypeResolver.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Droid.Binders.ViewTypeResolvers
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Android.Views;

    using MvvmCross.Platform.IoC;
    using MvvmCross.Platform.Platform;

    public class MvxAxmlNameViewTypeResolver : MvxLongLowerCaseViewTypeResolver, IMvxAxmlNameViewTypeResolver
    {
        public MvxAxmlNameViewTypeResolver(IMvxTypeCache<View> typeCache)
            : base(typeCache)
        {
            this.ViewNamespaceAbbreviations = new Dictionary<string, string>();
        }

        public IDictionary<string, string> ViewNamespaceAbbreviations { get; private set; }

        public override Type Resolve(string tagName)
        {
            var unabbreviatedTagName = this.UnabbreviateTagName(tagName);
            var longLowerCaseName = this.GetLookupName(unabbreviatedTagName);
            return this.ResolveLowerCaseTypeName(longLowerCaseName);
        }

        private string UnabbreviateTagName(string tagName)
        {
            var filteredTagName = tagName;
            if (this.ViewNamespaceAbbreviations != null)
            {
                var split = tagName.Split(new[] { '.' }, 2, StringSplitOptions.RemoveEmptyEntries);
                if (split.Length == 2)
                {
                    var abbreviate = split[0];
                    string fullName;
                    if (this.ViewNamespaceAbbreviations.TryGetValue(abbreviate, out fullName))
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
    }
}