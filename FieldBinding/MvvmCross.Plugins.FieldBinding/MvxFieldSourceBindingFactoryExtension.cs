// MvxFieldSourceBindingFactoryExtension.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore;
using Cirrious.MvvmCross.Binding;
using Cirrious.MvvmCross.Binding.Bindings.Source;
using Cirrious.MvvmCross.Binding.Bindings.Source.Construction;
using Cirrious.MvvmCross.Binding.Parse.PropertyPath.PropertyTokens;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MvvmCross.Plugins.FieldBinding
{
    public class MvxFieldSourceBindingFactoryExtension
        : IMvxSourceBindingFactoryExtension
    {
        public bool TryCreateBinding(object source, MvxPropertyToken currentToken,
                                     List<MvxPropertyToken> remainingTokens, out IMvxSourceBinding result)
        {
            if (source == null)
            {
                result = null;
                return false;
            }

            var propertyNameToken = currentToken as MvxPropertyNamePropertyToken;
            if (propertyNameToken == null)
            {
                result = null;
                return false;
            }

            var fieldInfo = FindFieldInfo(source, propertyNameToken.PropertyName);

            if (fieldInfo == null)
            {
                result = null;
                return false;
            }

            if (typeof(INotifyChange).IsAssignableFrom(fieldInfo.FieldType))
            {
                return TryCreateNotifyChangeBinding(source, remainingTokens, out result, fieldInfo, propertyNameToken);
            }

            return TryCreateFieldInfoBinding(source, remainingTokens, out result, fieldInfo);
        }

        protected bool TryCreateFieldInfoBinding(object source, List<MvxPropertyToken> remainingTokens,
                                                 out IMvxSourceBinding result, FieldInfo fieldInfo)
        {
            if (remainingTokens.Any())
            {
                result = new MvxChainedFieldSourceBinding(source, fieldInfo, remainingTokens);
            }
            else
            {
                result = new MvxLeafFieldSourceBinding(source, fieldInfo);
            }
            return true;
        }

        protected bool TryCreateNotifyChangeBinding(object source, List<MvxPropertyToken> remainingTokens,
                                                    out IMvxSourceBinding result,
                                                    FieldInfo fieldInfo, MvxPropertyNamePropertyToken propertyNameToken)
        {
            var fieldValue = fieldInfo.GetValue(source) as INotifyChange;
            if (fieldValue == null)
            {
                MvxBindingTrace.Warning("INotifyChange is null for {0}", propertyNameToken.PropertyName);
                result = null;
                return false;
            }

            if (remainingTokens.Any())
            {
                result = new MvxChainedNotifyChangeFieldSourceBinding(source, fieldValue, remainingTokens);
            }
            else
            {
                result = new MvxLeafNotifyChangeFieldSourceBinding(source, fieldValue);
            }
            return true;
        }

        protected FieldInfo FindFieldInfo(object source, string name)
        {
            var fieldInfo = source.GetType()
                                  .GetField(name,
                                            BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
            return fieldInfo;
        }
    }
}