// MvxSourceBindingFactory.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.Linq;
using Cirrious.MvvmCross.Binding.Interfaces.Bindings.Source;
using Cirrious.MvvmCross.Binding.Interfaces.Bindings.Source.Construction;

namespace Cirrious.MvvmCross.Binding.Bindings.Source.Construction
{
    public class MvxSourceBindingFactory : IMvxSourceBindingFactory
    {
        private static readonly char[] FieldSeparator = new[] {'.'};

        #region IMvxSourceBindingFactory Members

        public IMvxSourceBinding CreateBinding(object source, string combinedPropertyName)
        {
            if (combinedPropertyName == null)
                combinedPropertyName = "";

            return CreateBinding(source,
                                 combinedPropertyName.Split(FieldSeparator, StringSplitOptions.RemoveEmptyEntries));
        }

        public IMvxSourceBinding CreateBinding(object source, IEnumerable<string> childPropertyNames)
        {
            var childPropertyNameList = childPropertyNames.ToList();

            switch (childPropertyNameList.Count)
            {
                case 0:
                    return new MvxDirectToSourceBinding(source);
                case 1:
                    return new MvxPropertyInfoSourceBinding(source,
                                                            childPropertyNameList.DefaultIfEmpty(string.Empty)
                                                                                 .FirstOrDefault());
                default:
                    return new MvxChainedSourceBinding(source, childPropertyNameList.First(),
                                                       childPropertyNameList.Skip(1));
            }
        }

        #endregion
    }
}