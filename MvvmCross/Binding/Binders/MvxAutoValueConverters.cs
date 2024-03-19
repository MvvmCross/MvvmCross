// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

using MvvmCross.Converters;

namespace MvvmCross.Binding.Binders;

public class MvxAutoValueConverters
    : IMvxAutoValueConverters
{
    private record struct Key(Type ViewModelType, Type ViewType);

    private readonly Dictionary<Key, IMvxValueConverter> _lookup = new();

    public IMvxValueConverter? Find(Type viewModelType, Type viewType)
    {
        _lookup.TryGetValue(new Key(viewModelType, viewType), out var result);
        return result;
    }

    public void Register(Type viewModelType, Type viewType, IMvxValueConverter converter)
    {
        _lookup[new Key(viewModelType, viewType)] = converter;
    }
}
