using Android.App;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Shared.Attributes;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MvvmCross.Droid.Shared.Presenter
{
    public class FragmentHostRegistrationSettings
    {
        private readonly IEnumerable<Assembly> _assembliesToLookup;
        private readonly IMvxViewModelTypeFinder _viewModelTypeFinder;

        private readonly Dictionary<Type, SortedSet<MvxFragmentAttribute>> _fragmentTypeToMvxFragmentAttributeMap;
        private Dictionary<Type, Type> _viewModelToFragmentMap;

        private bool isInitialized;

        public FragmentHostRegistrationSettings(IEnumerable<Assembly> assembliesToLookup)
        {
            _assembliesToLookup = assembliesToLookup;
            _viewModelTypeFinder = Mvx.Resolve<IMvxViewModelTypeFinder>();
            _fragmentTypeToMvxFragmentAttributeMap = new Dictionary<Type, SortedSet<MvxFragmentAttribute>>();
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

                foreach (var typeWithMvxFragmentAttribute in typesWithMvxFragmentAttribute)
                {
                    if (!_fragmentTypeToMvxFragmentAttributeMap.ContainsKey(typeWithMvxFragmentAttribute))
                        _fragmentTypeToMvxFragmentAttributeMap.Add(typeWithMvxFragmentAttribute, new SortedSet<MvxFragmentAttribute>());

                    foreach (var mvxAttribute in typeWithMvxFragmentAttribute.GetMvxFragmentAttributes())
                        _fragmentTypeToMvxFragmentAttributeMap[typeWithMvxFragmentAttribute].Add(mvxAttribute);
                }

                _viewModelToFragmentMap =
                    typesWithMvxFragmentAttribute.ToDictionary(GetAssociatedViewModelType, fragmentType => fragmentType);
            }
        }

        private Type GetAssociatedViewModelType(Type fromFragmentType)
        {
            Type viewModelType = _viewModelTypeFinder.FindTypeOrNull(fromFragmentType);

            return viewModelType ?? fromFragmentType.GetMvxFragmentAttributes().First().ViewModelType;
        }

        public bool IsTypeRegisteredAsFragment(Type viewModelType)
        {
            InitializeIfNeeded();

            return _viewModelToFragmentMap.ContainsKey(viewModelType);
        }

        public bool IsActualHostValid(Type forViewModelType)
        {
            InitializeIfNeeded();

            var activityViewModelType = GetCurrentActivityViewModelType();

            return
                GetMvxFragmentAssociatedAttributes(forViewModelType)
                    .Any(x => x.ParentActivityViewModelType == activityViewModelType);
        }

        private Type GetCurrentActivityViewModelType()
        {
            Activity currentActivity = Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity;
            Type currentActivityType = currentActivity.GetType();

            var activityViewModelType = _viewModelTypeFinder.FindTypeOrNull(currentActivityType);

            if (activityViewModelType == null)
                throw new InvalidOperationException($"Sorry but looks like your Activity ({currentActivityType.ToString()}) does not inherit from MvvmCross Activity - Viewmodel Type is null!");

            return activityViewModelType;
        }

        public Type GetFragmentHostViewModelType(Type forViewModelType)
        {
            InitializeIfNeeded();

            return GetMvxFragmentAssociatedAttributes(forViewModelType).First().ParentActivityViewModelType;
        }

        public Type GetFragmentTypeAssociatedWith(Type viewModelType)
        {
            InitializeIfNeeded();

            return _viewModelToFragmentMap[viewModelType];
        }

        private SortedSet<MvxFragmentAttribute> GetMvxFragmentAssociatedAttributes(Type withFragmentViewModelType)
            => _fragmentTypeToMvxFragmentAttributeMap[withFragmentViewModelType];

        public MvxFragmentAttribute GetMvxFragmentAttributeAssociatedWithCurrentHost(Type fragmentViewModelType)
        {
            InitializeIfNeeded();

            var currentActivityViewModelType = GetCurrentActivityViewModelType();

            return
                GetMvxFragmentAssociatedAttributes(fragmentViewModelType)
                    .Single(x => x.ParentActivityViewModelType == currentActivityViewModelType);
        }
    }
}