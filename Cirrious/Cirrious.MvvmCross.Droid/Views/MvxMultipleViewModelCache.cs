using System;
using System.Collections.Generic;
using System.Linq;
using Cirrious.MvvmCross.ViewModels;

namespace Cirrious.MvvmCross.Droid.Views
{
    public class MvxMultipleViewModelCache
        : IMvxMultipleViewModelCache
    {
        private HashSet<IMvxViewModel> _currentViewModels;

        public void Cache(IMvxViewModel toCache)
        {
            if (_currentViewModels == null)
                _currentViewModels = new HashSet<IMvxViewModel>();

            _currentViewModels.Add(toCache);
        }

        public IMvxViewModel GetAndClear(Type viewModelType)
        {
            if (_currentViewModels == null)
                return null;

            var item = _currentViewModels.FirstOrDefault(vm => vm.GetType() == viewModelType);

            if (_currentViewModels.Contains(item))
                _currentViewModels.Remove(item);

            return item;
        }


        public T GetAndClear<T>() where T : IMvxViewModel
        {
            if (_currentViewModels == null)
                return default(T);

            var item = _currentViewModels.FirstOrDefault(vm => vm.GetType() == typeof(T));

            if (_currentViewModels.Contains(item))
                _currentViewModels.Remove(item);

            return (T)item;
        }
    }
}