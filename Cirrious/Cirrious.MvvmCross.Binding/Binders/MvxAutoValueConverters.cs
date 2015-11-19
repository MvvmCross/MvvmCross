// MvxAutoValueConverters.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Converters;
using System;
using System.Collections.Generic;

namespace Cirrious.MvvmCross.Binding.Binders
{
    public class MvxAutoValueConverters
        : IMvxAutoValueConverters
    {
        public class Key
        {
            public Key(Type viewModel, Type view)
            {
                ViewType = view;
                ViewModelType = viewModel;
            }

            public Type ViewModelType { get; private set; }
            public Type ViewType { get; private set; }

            public override bool Equals(object obj)
            {
                if (obj == null)
                    return false;

                var rhs = obj as Key;
                if (rhs == null)
                    return false;

                return ViewModelType == rhs.ViewModelType
                       && ViewType == rhs.ViewType;
            }

            public override int GetHashCode()
            {
                return ViewModelType.GetHashCode() + ViewType.GetHashCode();
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
}