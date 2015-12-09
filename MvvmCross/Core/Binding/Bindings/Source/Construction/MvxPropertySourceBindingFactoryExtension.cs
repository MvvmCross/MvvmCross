// MvxPropertySourceBindingFactoryExtension.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Bindings.Source.Construction
{
    using System.Collections.Generic;
    using System.Reflection;

    using MvvmCross.Binding.Bindings.Source.Chained;
    using MvvmCross.Binding.Bindings.Source.Leaf;
    using MvvmCross.Binding.Parse.PropertyPath.PropertyTokens;
    using MvvmCross.Platform;
    using MvvmCross.Platform.Exceptions;

    public class MvxPropertySourceBindingFactoryExtension
        : IMvxSourceBindingFactoryExtension
    {
        public bool TryCreateBinding(object source, MvxPropertyToken currentToken, List<MvxPropertyToken> remainingTokens, out IMvxSourceBinding result)
        {
            if (source == null)
            {
                result = null;
                return false;
            }

            result = remainingTokens.Count == 0 ? this.CreateLeafBinding(source, currentToken) : this.CreateChainedBinding(source, currentToken, remainingTokens);
            return result != null;
        }

        protected virtual MvxChainedSourceBinding CreateChainedBinding(object source, MvxPropertyToken propertyToken,
                                                                       List<MvxPropertyToken> remainingTokens)
        {
            var indexPropertyToken = propertyToken as MvxIndexerPropertyToken;
            if (indexPropertyToken != null)
            {
                var itemPropertyInfo = this.FindItemPropertyInfo(source);
                if (itemPropertyInfo == null)
                    return null;

                return new MvxIndexerChainedSourceBinding(source, itemPropertyInfo, indexPropertyToken,
                                                          remainingTokens);
            }

            var propertyNameToken = propertyToken as MvxPropertyNamePropertyToken;
            if (propertyNameToken != null)
            {
                var propertyInfo = this.FindPropertyInfo(source, propertyNameToken.PropertyName);

                if (propertyInfo == null)
                    return null;

                return new MvxSimpleChainedSourceBinding(source, propertyInfo,
                                                         remainingTokens);
            }

            throw new MvxException("Unexpected property chaining - seen token type {0}",
                                   propertyToken.GetType().FullName);
        }

        protected virtual IMvxSourceBinding CreateLeafBinding(object source, MvxPropertyToken propertyToken)
        {
            var indexPropertyToken = propertyToken as MvxIndexerPropertyToken;
            if (indexPropertyToken != null)
            {
                var itemPropertyInfo = this.FindItemPropertyInfo(source);
                if (itemPropertyInfo == null)
                    return null;
                return new MvxIndexerLeafPropertyInfoSourceBinding(source, itemPropertyInfo, indexPropertyToken);
            }

            var propertyNameToken = propertyToken as MvxPropertyNamePropertyToken;
            if (propertyNameToken != null)
            {
                var propertyInfo = this.FindPropertyInfo(source, propertyNameToken.PropertyName);
                if (propertyInfo == null)
                    return null;
                return new MvxSimpleLeafPropertyInfoSourceBinding(source, propertyInfo);
            }

            if (propertyToken is MvxEmptyPropertyToken)
            {
                return new MvxDirectToSourceBinding(source);
            }

            throw new MvxException("Unexpected property source - seen token type {0}", propertyToken.GetType().FullName);
        }

        protected PropertyInfo FindItemPropertyInfo(object source)
        {
            return this.FindPropertyInfo(source, "Item");
        }

        protected PropertyInfo FindPropertyInfo(object source, string propertyName)
        {
            var propertyInfo = source.GetType()
                                         .GetProperty(propertyName);
            return propertyInfo;
        }
    }
}