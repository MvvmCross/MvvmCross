using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Android.App;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.FullFragging.Attributes;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid.Platform;

namespace MvvmCross.Droid.FullFragging.Presenter
{
    public class FragmentHostRegistrationSettings
    {
        private readonly IEnumerable<Assembly> _assembliesToLookup;
        private readonly IMvxViewModelTypeFinder _viewModelTypeFinder;

        private Dictionary<Type, MvxFragmentAttribute> _fragmentTypeToMvxFragmentAttributeMap;
        private Dictionary<Type, Type> _viewModelToFragmentMap;

        private bool isInitialized;

        public FragmentHostRegistrationSettings(IEnumerable<Assembly> assembliesToLookup)
        {
            _assembliesToLookup = assembliesToLookup;
            _viewModelTypeFinder = Mvx.Resolve<IMvxViewModelTypeFinder>();
        }

        private void InitializeIfNeeded()
        {
            lock (this)
            {
                if (isInitialized)
                    return;

                isInitialized = true;

                var typesWithMvxFragmentAttribute =
                    _assembliesToLookup
                        .SelectMany(x => x.DefinedTypes)
                        .Select(x => x.AsType())
                        .Where(x => x.HasMvxFragmentAttribute())
                        .ToList();

                _fragmentTypeToMvxFragmentAttributeMap = typesWithMvxFragmentAttribute.ToDictionary(key => key, key => key.GetMvxFragmentAttribute());
                _viewModelToFragmentMap =
                    typesWithMvxFragmentAttribute.ToDictionary(GetAssociatedViewModelType, fragmentType => fragmentType);
            }
        }

        private Type GetAssociatedViewModelType(Type fromFragmentType)
        {
            Type viewModelType = _viewModelTypeFinder.FindTypeOrNull(fromFragmentType);

            return viewModelType ?? fromFragmentType.GetMvxFragmentAttribute().ViewModelType;
        }

        public bool IsTypeRegisteredAsFragment(Type viewModelType)
        {
            InitializeIfNeeded();

            return _viewModelToFragmentMap.ContainsKey(viewModelType);
        }

        public bool IsActualHostValid(Type forViewModelType)
        {
            InitializeIfNeeded();

            Activity currentActivity = Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity;
            Type currentActivityType = currentActivity.GetType();

            var activityViewModelType = _viewModelTypeFinder.FindTypeOrNull(currentActivityType);

            if (activityViewModelType == null)
                throw new InvalidOperationException($"Sorry but looks like your Activity ({currentActivityType.ToString()}) does not inherit from MvvmCross Activity - Viewmodel Type is null!");

            return GetMvxFragmentAttributeAssociated(forViewModelType).ParentActivityViewModelType == activityViewModelType;
        }

        public Type GetFragmentHostViewModelType(Type forViewModelType)
        {
            InitializeIfNeeded();

            return GetMvxFragmentAttributeAssociated(forViewModelType).ParentActivityViewModelType;
        }

        public Type GetFragmentTypeAssociatedWith(Type viewModelType)
        {
            return _viewModelToFragmentMap[viewModelType];
        }

        public MvxFragmentAttribute GetMvxFragmentAttributeAssociated(Type withFragmentForViewModelType)
        {
            var fragmentType = GetFragmentTypeAssociatedWith(withFragmentForViewModelType);
            return _fragmentTypeToMvxFragmentAttributeMap[fragmentType];
        }
    }
}