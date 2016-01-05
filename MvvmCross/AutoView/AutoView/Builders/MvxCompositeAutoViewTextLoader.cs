// MvxCompositeAutoViewTextLoader.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.AutoView.Builders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using MvvmCross.AutoView.Interfaces;

    public class MvxCompositeAutoViewTextLoader : IMvxAutoViewTextLoader
    {
        private readonly List<IMvxAutoViewTextLoader> _subLoaders = new List<IMvxAutoViewTextLoader>();

        protected void Add(IMvxAutoViewTextLoader subLoader)
        {
            this._subLoaders.Add(subLoader);
        }

        public bool HasDefinition(Type viewModelType, string key)
        {
            return this._subLoaders.Any(l => l.HasDefinition(viewModelType, key));
        }

        public string GetDefinition(Type viewModelType, string key)
        {
            foreach (var subLoader in this._subLoaders)
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