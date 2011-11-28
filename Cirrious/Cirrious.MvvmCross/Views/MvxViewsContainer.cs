using System;
using System.Collections.Generic;
using Cirrious.MvvmCross.Core;
using Cirrious.MvvmCross.Interfaces.ViewModel;
using Cirrious.MvvmCross.Interfaces.Views;

namespace Cirrious.MvvmCross.Views
{
    public abstract class MvxViewsContainer 
        : MvxSingleton<MvxViewsContainer>
        , IMvxViewsContainer
    {
        private class MvxViewBinding
        {
            public Type ViewType { get; set; }
            public MxvViewModelAction ViewModelAction { get; set; }

            public MvxViewBinding(Type viewType, MxvViewModelAction viewModelAction)
            {
                ViewModelAction = viewModelAction;
                ViewType = viewType;
            }
        }

        private readonly Dictionary<string, MvxViewBinding> _bindingMap = new Dictionary<string, MvxViewBinding>();

        public void Add(MxvViewModelAction viewModelAction, Type viewType)
        {
            _bindingMap.Add(viewModelAction.Key, new MvxViewBinding(viewType, viewModelAction));
        }

        public void Add<TViewModel>(string actionName, Type viewType) where TViewModel : IMvxViewModel
        {
            Add(new MxvViewModelAction<TViewModel>(actionName), viewType);
        }

        public void Add<TViewModel>(Type viewType) where TViewModel : IMvxViewModel
        {
            Add<TViewModel>(null, viewType);
        }


        public bool ContainsKey(MxvViewModelAction viewModelAction)
        {
            return _bindingMap.ContainsKey(viewModelAction.Key);
        }

        public Type GetViewType(MxvViewModelAction viewModelAction)
        {
            MvxViewBinding binding;
            if (!_bindingMap.TryGetValue(viewModelAction.Key, out binding))
                throw new KeyNotFoundException("Could not find view for " + viewModelAction.ToString());

            return binding.ViewType;
        }
    }
}