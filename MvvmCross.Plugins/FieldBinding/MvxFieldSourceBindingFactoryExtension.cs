// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Source;
using MvvmCross.Binding.Bindings.Source.Construction;
using MvvmCross.Binding.Parse.PropertyPath.PropertyTokens;

namespace MvvmCross.Plugin.FieldBinding
{
    [Preserve(AllMembers = true)]
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
                MvxBindingLog.Warning("INotifyChange is null for {0}", propertyNameToken.PropertyName);
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
