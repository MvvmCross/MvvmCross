// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Concurrent;
using MvvmCross.ViewModels;

namespace MvvmCross.Platforms.Android.Views
{
    public class MvxMultipleViewModelCache
        : IMvxMultipleViewModelCache
    {
        private readonly Lazy<ConcurrentDictionary<CachedViewModelType, IMvxViewModel>> _lazyCurrentViewModels;

        public MvxMultipleViewModelCache()
        {
            _lazyCurrentViewModels =
                new Lazy<ConcurrentDictionary<CachedViewModelType, IMvxViewModel>>(
                    () => new ConcurrentDictionary<CachedViewModelType, IMvxViewModel>());
        }

        private ConcurrentDictionary<CachedViewModelType, IMvxViewModel> CurrentViewModels => _lazyCurrentViewModels.Value;

        public void Cache(IMvxViewModel toCache, string viewModelTag = "singleInstanceCache")
        {
            if (toCache == null) return;

            var type = toCache.GetType();

            var cachedViewModelType = new CachedViewModelType(type, viewModelTag);
            if (!CurrentViewModels.ContainsKey(cachedViewModelType))
                CurrentViewModels.TryAdd(cachedViewModelType, toCache);
        }

        public IMvxViewModel GetAndClear(Type viewModelType, string viewModelTag = "singleInstanceCache")
        {
            if (viewModelType == null) return null;

            IMvxViewModel vm;
            var cachedViewModelType = new CachedViewModelType(viewModelType, viewModelTag);
            CurrentViewModels.TryRemove(cachedViewModelType, out vm);

            return vm;
        }

        public T GetAndClear<T>(string viewModelTag = "singleInstanceCache") where T : IMvxViewModel
        {
            return (T)GetAndClear(typeof(T), viewModelTag);
        }

        private class CachedViewModelType
        {
            public Type ViewModelType { get; }
            public string ViewModelTag { get; }

            public CachedViewModelType(Type viewModelType, string viewModelTag)
            {
                ViewModelType = viewModelType;
                ViewModelTag = viewModelTag ?? string.Empty;
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    int hashCode = 17;
                    hashCode = (hashCode * 23) + ViewModelType.GetHashCode();
                    hashCode = (hashCode * 23) + ViewModelTag.GetHashCode();
                    return hashCode;
                }
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(obj, this))
                    return true;

                var other = obj as CachedViewModelType;

                return other != null &&
                       other.ViewModelTag.Equals(ViewModelTag) &&
                       other.ViewModelType == ViewModelType;
            }
        }
    }
}
