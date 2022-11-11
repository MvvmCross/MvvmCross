// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using MvvmCross.Converters;

namespace MvvmCross.Binding.Binders
{
#nullable enable
    public class MvxAutoValueConverters
        : IMvxAutoValueConverters
    {
        private struct Key : IEquatable<Key>
        {
            public Key(Type viewModel, Type view)
            {
                ViewType = view;
                ViewModelType = viewModel;
            }

            public Type ViewModelType { get; }
            public Type ViewType { get; }

            public override int GetHashCode()
            {
                return ViewModelType.GetHashCode() + ViewType.GetHashCode();
            }

            public bool Equals(Key other)
            {
                return ViewModelType == other.ViewModelType
                    && ViewType == other.ViewType;
            }

            public override bool Equals(object obj)
            {
                return obj is Key key && Equals(key);
            }
        }

        private readonly Dictionary<Key, IMvxValueConverter> _lookup = new Dictionary<Key, IMvxValueConverter>();

        public IMvxValueConverter Find(Type viewModelType, Type viewType)
        {
            IMvxValueConverter result;
            _lookup.TryGetValue(new Key(viewModelType, viewType), out result);
            return result;
        }

        public void Register(Type viewModelType, Type viewType, IMvxValueConverter converter)
        {
            _lookup[new Key(viewModelType, viewType)] = converter;
        }
    }
#nullable restore
}
