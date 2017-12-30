using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using System;
using System.Reflection;
using Xamarin.Forms;

namespace MvvmCross.Forms.ViewModels
{
    public class MvxFormsViewModelViewTypeFinder : MvxViewModelViewTypeFinder
    {
        public MvxFormsViewModelViewTypeFinder(IMvxViewModelByNameLookup viewModelByNameLookup, IMvxNameMapping viewToViewModelNameMapping) : base(viewModelByNameLookup, viewToViewModelNameMapping)
        {
        }

        protected override bool CheckCandidateTypeIsAView(Type candidateType)
        {
            if (candidateType == null)
                return false;

            if (candidateType.GetTypeInfo().IsAbstract)
                return false;

            if (!typeof(VisualElement).IsAssignableFrom(candidateType))
                return false;

            return true;
        }
    }
}
