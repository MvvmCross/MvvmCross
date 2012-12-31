#region Copyright

// <copyright file="MvxCompositeAutoViewTextLoader.cs" company="Cirrious">
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
using Cirrious.MvvmCross.AutoView.Interfaces;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;

namespace Cirrious.MvvmCross.AutoView.Builders
{
    public class MvxCompositeAutoViewTextLoader : IMvxServiceConsumer, IMvxAutoViewTextLoader
    {
        private readonly List<IMvxAutoViewTextLoader> _subLoaders = new List<IMvxAutoViewTextLoader>();

        protected void Add(IMvxAutoViewTextLoader subLoader)
        {
            _subLoaders.Add(subLoader);
        }

        public bool HasDefinition(Type viewModelType, string key)
        {
            return _subLoaders.Any(l => l.HasDefinition(viewModelType, key));
        }

        public string GetDefinition(Type viewModelType, string key)
        {
            foreach (var subLoader in _subLoaders)
            {
                var result = subLoader.GetDefinition(viewModelType, key);
                if (!string.IsNullOrEmpty(result))
                {
                    return result;
                }
            }

            return null;
        }
    }
}