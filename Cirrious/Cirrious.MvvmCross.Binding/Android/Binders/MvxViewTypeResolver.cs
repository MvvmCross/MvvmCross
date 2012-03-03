#region Copyright
// <copyright file="MvxViewTypeResolver.cs" company="Cirrious">
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
using System.Linq;
using System.Text;
using Android.Views;
using Cirrious.MvvmCross.Binding.Android.Interfaces.Binders;

namespace Cirrious.MvvmCross.Binding.Android.Binders
{
    public class MvxViewTypeResolver : IMvxViewTypeResolver
    {
        private Dictionary<string, Type> _cache = new Dictionary<string, Type>();

        #region IMvxViewTypeResolver Members

        public virtual Type Resolve(string tagName)
        {
            var longLowerCaseName = GetLookupName(tagName);

            Type toReturn;
            if (_cache.TryGetValue(longLowerCaseName, out toReturn))
                return toReturn;

            var viewType = typeof(View);
#warning AppDomain.CurrentDomain.GetAssemblies is only the loaded assemblies - so we might miss controls if not already loaded
            var query = from assembly in AppDomain.CurrentDomain.GetAssemblies()
                        from type in assembly.GetTypes()
                        where viewType.IsAssignableFrom(type)
                        where (type.FullName ?? "-").ToLowerInvariant() == longLowerCaseName
                        select type;

            toReturn = query.FirstOrDefault();
            _cache[longLowerCaseName] = toReturn;

            return toReturn;
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