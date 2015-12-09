// MvxAutoValueConverters.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Binders
{
    using System;
    using System.Collections.Generic;

    using MvvmCross.Platform.Converters;

    public class MvxAutoValueConverters
        : IMvxAutoValueConverters
    {
        public class Key
        {
            public Key(Type viewModel, Type view)
            {
                this.ViewType = view;
                this.ViewModelType = viewModel;
            }

            public Type ViewModelType { get; private set; }
            public Type ViewType { get; private set; }

            public override bool Equals(object obj)
            {
                var rhs = obj as Key;
                if (rhs == null)
                    return false;

                return this.ViewModelType == rhs.ViewModelType
                       && this.ViewType == rhs.ViewType;
            }

            public override int GetHashCode()
            {
                return this.ViewModelType.GetHashCode() + this.ViewType.GetHashCode();
            }
        }

        private readonly Dictionary<Key, IMvxValueConverter> _lookup = new Dictionary<Key, IMvxValueConverter>();

        public IMvxValueConverter Find(Type viewModelType, Type viewType)
        {
            IMvxValueConverter result;
            this._lookup.TryGetValue(new Key(viewModelType, viewType), out result);
            return result;
        }

        public void Register(Type viewModelType, Type viewType, IMvxValueConverter converter)
        {
            this._lookup[new Key(viewModelType, viewType)] = converter;
        }
    }
}