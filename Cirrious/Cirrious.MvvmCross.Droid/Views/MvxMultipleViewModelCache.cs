using System;
using System.Collections.Generic;
using Cirrious.MvvmCross.ViewModels;

namespace Cirrious.MvvmCross.Droid.Views
{
    public class MvxMultipleViewModelCache
        : IMvxMultipleViewModelCache
    {
        private Dictionary<Type, IMvxViewModel> _currentViewModels;

        private Dictionary<Type, IMvxViewModel> CurrentViewModels
        {
            get { return _currentViewModels ?? (_currentViewModels = new Dictionary<Type, IMvxViewModel>()); }
        }

        public void Cache(IMvxViewModel toCache)
        {
            CurrentViewModels.Add(toCache.GetType(), toCache);
        }

        public IMvxViewModel GetAndClear(Type viewModelType)
        {
            IMvxViewModel vm;
            CurrentViewModels.TryGetValue(viewModelType, out vm);

            if (vm != null)
                CurrentViewModels.Remove(viewModelType);

            return vm;
        }

        public T GetAndClear<T>() where T : IMvxViewModel
        {
            return (T) GetAndClear(typeof (T));
        }
    }
}