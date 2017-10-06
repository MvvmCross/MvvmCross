using System;
using System.Collections.Generic;
using MvvmCross.Core.ViewModels;

namespace MvvmCross.Core.Views
{
    public interface IMvxAttributeViewPresenter : IMvxViewPresenter
    {
        IMvxViewModelTypeFinder ViewModelTypeFinder { get; }
        IMvxViewsContainer ViewsContainer { get; }
        Dictionary<Type, MvxPresentationAttributeAction> AttributeTypesToActionsDictionary { get; }
        //Dictionary<Type, IList<MvxBasePresentationAttribute>> ViewModelToPresentationAttributeMap { get; }
        void RegisterAttributeTypes();

        //TODO: Maybe move those to helper class
        MvxBasePresentationAttribute GetAttributeForViewModel(Type viewModelType);
        MvxBasePresentationAttribute CreateAttributeForViewModel(Type viewModelType);
        MvxBasePresentationAttribute GetOverridePresentationAttribute(Type viewType);
    }
}
