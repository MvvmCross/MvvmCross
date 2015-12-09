// MvxCompositeAutoViewTextLoader.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.AutoView.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cirrious.MvvmCross.AutoView.Builders
{
    public class MvxCompositeAutoViewTextLoader : IMvxAutoViewTextLoader
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