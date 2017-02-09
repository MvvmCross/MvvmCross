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

        private readonly Dictionary<Type, IList<MvxFragmentAttribute>> _fragmentTypeToMvxFragmentAttributeMap;
        private Dictionary<Type, Type> _viewModelToFragmentTypeMap;

        private bool isInitialized;

        public FragmentHostRegistrationSettings(IEnumerable<Assembly> assembliesToLookup)
        {
            _assembliesToLookup = assembliesToLookup;
            _viewModelTypeFinder = Mvx.Resolve<IMvxViewModelTypeFinder>();
            _fragmentTypeToMvxFragmentAttributeMap = new Dictionary<Type, IList<MvxFragmentAttribute>>();
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
                        _fragmentTypeToMvxFragmentAttributeMap.Add(typeWithMvxFragmentAttribute, new List<MvxFragmentAttribute>());

                    foreach (var mvxAttribute in typeWithMvxFragmentAttribute.GetMvxFragmentAttributes())
                        _fragmentTypeToMvxFragmentAttributeMap[typeWithMvxFragmentAttribute].Add(mvxAttribute);
                }

                _viewModelToFragmentTypeMap =
                    typesWithMvxFragmentAttribute.ToDictionary(GetAssociatedViewModelType, fragmentType => fragmentType);
            }
        }

        private Type GetAssociatedViewModelType(Type fromFragmentType)
        {
            Type viewModelType = _viewModelTypeFinder.FindTypeOrNull(fromFragmentType);

            return viewModelType ?? fromFragmentType.GetMvxFragmentAttributes().First().ViewModelType;
        }

        public virtual bool IsTypeRegisteredAsFragment(Type viewModelType)
        {
            InitializeIfNeeded();

            return _viewModelToFragmentTypeMap.ContainsKey(viewModelType);
        }

        public virtual bool IsActualHostValid(Type forViewModelType)
        {
            InitializeIfNeeded();

            var activityViewModelType = GetCurrentActivityViewModelType();

            // for example: MvxSplashScreen usually does not have associated ViewModel
            // it is for sure not valid host - and it can not be used with Fragment Presenter.
            if (activityViewModelType == typeof(MvxNullViewModel))
                return false;

            return
                GetMvxFragmentAssociatedAttributes(forViewModelType)
                    .Any(x => x.ParentActivityViewModelType == activityViewModelType);
        }

        private Type GetCurrentActivityViewModelType()
        {
            Activity currentActivity = Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity;
            Type currentActivityType = currentActivity.GetType();

            var activityViewModelType = _viewModelTypeFinder.FindTypeOrNull(currentActivityType);
            return activityViewModelType;
        }

        public virtual Type GetFragmentHostViewModelType(Type forViewModelType)
        {
            InitializeIfNeeded();

            var associatedMvxFragmentAttributes = GetMvxFragmentAssociatedAttributes(forViewModelType).ToList();
            return associatedMvxFragmentAttributes.First().ParentActivityViewModelType;
        }

        public virtual Type GetFragmentTypeAssociatedWith(Type viewModelType)
        {
            InitializeIfNeeded();

            return _viewModelToFragmentTypeMap[viewModelType];
        }

        private IList<MvxFragmentAttribute> GetMvxFragmentAssociatedAttributes(Type withFragmentViewModelType)
        {
            var fragmentTypeAssociatedWithViewModel = GetFragmentTypeAssociatedWith(withFragmentViewModelType);
            return _fragmentTypeToMvxFragmentAttributeMap[fragmentTypeAssociatedWithViewModel];
        }

        public virtual MvxFragmentAttribute GetMvxFragmentAttributeAssociatedWithCurrentHost(Type fragmentViewModelType)
        {
            InitializeIfNeeded();

            var currentActivityViewModelType = GetCurrentActivityViewModelType();
            Activity currentActivity = Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity;

            var fragmentAttributes = GetMvxFragmentAssociatedAttributes(fragmentViewModelType)
                .Where(x => x.ParentActivityViewModelType == currentActivityViewModelType);
            MvxFragmentAttribute attribute = fragmentAttributes.FirstOrDefault();

            if (fragmentAttributes.Count() > 1)
            {
                foreach (var item in fragmentAttributes)
                {
                    if (currentActivity.FindViewById(item.FragmentContentId) != null)
                    {
                        attribute = item;
                        break;
                    }
                }
            }

            return attribute;
        }
    }
}