// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using MvvmCross.Binding.Bindings.Source.Chained;
using MvvmCross.Binding.Bindings.Source.Leaf;
using MvvmCross.Binding.Parse.PropertyPath.PropertyTokens;
using MvvmCross.Exceptions;

namespace MvvmCross.Binding.Bindings.Source.Construction;

/// <summary>
/// Uses a global cache of calls in Reflection namespace
/// </summary>
public class MvxPropertySourceBindingFactoryExtension
    : IMvxSourceBindingFactoryExtension
{
    private readonly ConcurrentDictionary<int, PropertyInfo> _propertyInfoCache = new();

    [RequiresUnreferencedCode("This method uses reflection which may not be preserved during trimming")]
    public bool TryCreateBinding(
        object? source,
        IMvxPropertyToken propertyToken,
        List<IMvxPropertyToken> remainingTokens,
        out IMvxSourceBinding? result)
    {
        if (source == null)
        {
            result = null;
            return false;
        }

        result = remainingTokens.Count == 0
            ? CreateLeafBinding(source, propertyToken)
            : CreateChainedBinding(source, propertyToken, remainingTokens);

        return result != null;
    }

    protected virtual MvxChainedSourceBinding? CreateChainedBinding(
        object source,
        IMvxPropertyToken propertyToken,
        List<IMvxPropertyToken> remainingTokens)
    {
        switch (propertyToken)
        {
            case MvxIndexerPropertyToken indexPropertyToken:
                {
                    var itemPropertyInfo = FindPropertyInfo(source);
                    if (itemPropertyInfo == null)
                        return null;

                    return new MvxIndexerChainedSourceBinding(source, itemPropertyInfo, indexPropertyToken,
                        remainingTokens);
                }
            case MvxPropertyNamePropertyToken propertyNameToken:
                {
                    var propertyInfo = FindPropertyInfo(source, propertyNameToken.PropertyName);

                    if (propertyInfo == null)
                        return null;

                    return new MvxSimpleChainedSourceBinding(source, propertyInfo,
                        remainingTokens);
                }
            default:
                throw new MvxException("Unexpected property chaining - seen token type {0}",
                    propertyToken.GetType().FullName);
        }
    }

    [RequiresUnreferencedCode("This method uses reflection which may not be preserved during trimming")]
    protected virtual IMvxSourceBinding? CreateLeafBinding(object source, IMvxPropertyToken propertyToken)
    {
        if (propertyToken is MvxIndexerPropertyToken indexPropertyToken)
        {
            var itemPropertyInfo = FindPropertyInfo(source);
            if (itemPropertyInfo == null)
                return null;
            return new MvxIndexerLeafPropertyInfoSourceBinding(source, itemPropertyInfo, indexPropertyToken);
        }

        if (propertyToken is MvxPropertyNamePropertyToken propertyNameToken)
        {
            var propertyInfo = FindPropertyInfo(source, propertyNameToken.PropertyName);
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

    protected PropertyInfo? FindPropertyInfo(object source, string propertyName = "Item")
    {
        var sourceType = source.GetType();
        var key = (sourceType.FullName + "." + propertyName).GetHashCode();

        if (_propertyInfoCache.TryGetValue(key, out PropertyInfo? pi))
            return pi;

        // Get lowest property
        while (sourceType != null)
        {
            // Use BindingFlags.DeclaredOnly to avoid AmbiguousMatchException
            pi = sourceType.GetProperty(propertyName,
                BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);

            if (pi != null)
            {
                break;
            }
            sourceType = sourceType.BaseType;
        }

        if (pi != null)
            _propertyInfoCache.TryAdd(key, pi);

        return pi;
    }
}
