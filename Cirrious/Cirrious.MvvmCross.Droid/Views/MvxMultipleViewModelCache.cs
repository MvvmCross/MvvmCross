using System;
using System.Collections.Concurrent;
using Cirrious.MvvmCross.ViewModels;

namespace Cirrious.MvvmCross.Droid.Views
{
    public class MvxMultipleViewModelCache
        : IMvxMultipleViewModelCache
    {
        private ConcurrentDictionary<Type, IMvxViewModel> _currentViewModels;

        private ConcurrentDictionary<Type, IMvxViewModel> CurrentViewModels
        {
            get { return _currentViewModels ?? (_currentViewModels = new ConcurrentDictionary<Type, IMvxViewModel>()); }
        }

        public void Cache(IMvxViewModel toCache)
        {
            if (toCache == null) return;

            var type = toCache.GetType();

            if (!CurrentViewModels.ContainsKey(type))
                CurrentViewModels.TryAdd(type, toCache);
        }

        public IMvxViewModel GetAndClear(Type viewModelType)
        {
            if (viewModelType == null) return null;

            IMvxViewModel vm;
            CurrentViewModels.TryRemove(viewModelType, out vm);

            return vm;
        }

        public T GetAndClear<T>() where T : IMvxViewModel
        {
            return (T) GetAndClear(typeof (T));
        }
    }
}