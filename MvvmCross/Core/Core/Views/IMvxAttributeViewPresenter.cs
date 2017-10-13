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
        MvxBasePresentationAttribute GetPresentationAttribute(Type viewModelType);
        MvxBasePresentationAttribute CreatePresentationAttribute(Type viewModelType, Type viewType);
        MvxBasePresentationAttribute GetOverridePresentationAttribute(Type viewModelType, Type viewType);
    }
}
