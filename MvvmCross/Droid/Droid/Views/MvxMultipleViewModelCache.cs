namespace MvvmCross.Droid.Views
{
    using System;
    using System.Collections.Concurrent;

    using MvvmCross.Core.ViewModels;

    public class MvxMultipleViewModelCache
        : IMvxMultipleViewModelCache
    {
        private readonly Lazy<ConcurrentDictionary<CachedViewModelType, IMvxViewModel>> _lazyCurrentViewModels;

        public MvxMultipleViewModelCache()
        {
            this._lazyCurrentViewModels = new Lazy<ConcurrentDictionary<CachedViewModelType, IMvxViewModel>>(() => new ConcurrentDictionary<CachedViewModelType, IMvxViewModel>());
        }

        private ConcurrentDictionary<CachedViewModelType, IMvxViewModel> CurrentViewModels => this._lazyCurrentViewModels.Value;

        public void Cache(IMvxViewModel toCache, string viewModelTag = "singleInstanceCache")
        {
            if (toCache == null) return;

            var type = toCache.GetType();

            var cachedViewModelType = new CachedViewModelType(type, viewModelTag);
            if (!this.CurrentViewModels.ContainsKey(cachedViewModelType))
                this.CurrentViewModels.TryAdd(cachedViewModelType, toCache);
        }

        public IMvxViewModel GetAndClear(Type viewModelType, string viewModelTag = "singleInstanceCache")
        {
            if (viewModelType == null) return null;

            IMvxViewModel vm;
            var cachedViewModelType = new CachedViewModelType(viewModelType, viewModelTag);
            this.CurrentViewModels.TryRemove(cachedViewModelType, out vm);

            return vm;
        }

        public T GetAndClear<T>(string viewModelTag = "singleInstanceCache") where T : IMvxViewModel
        {
            return (T)this.GetAndClear(typeof(T), viewModelTag);
        }

        private class CachedViewModelType
        {
            public Type ViewModelType { get; private set; }
            public string ViewModelTag { get; private set; }

            public CachedViewModelType(Type viewModelType, string viewModelTag)
            {
                this.ViewModelType = viewModelType;
                this.ViewModelTag = viewModelTag ?? string.Empty;
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    int hashCode = 17;
                    hashCode = hashCode * 23 + this.ViewModelType.GetHashCode();
                    hashCode = hashCode * 23 + this.ViewModelTag.GetHashCode();
                    return hashCode;
                }
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(obj, this))
                    return true;

                var other = obj as CachedViewModelType;

                return other != null &&
                       other.ViewModelTag.Equals(this.ViewModelTag) &&
                       other.ViewModelType == this.ViewModelType;
            }
        }
    }
}