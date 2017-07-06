using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Java.Lang;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Droid.Views.Attributes;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid.Platform;
using MvvmCross.Platform.Platform;

namespace MvvmCross.Droid.Views
{
    public class MvxAndroidViewPresenter : MvxViewPresenter, IMvxAndroidViewPresenter
    {
        protected virtual Activity CurrentActivity => Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity;
        protected virtual FragmentManager CurrentFragmentManager => CurrentActivity.FragmentManager;
        protected IEnumerable<Assembly> _androidViewAssemblies;

        private Dictionary<Type, Action<Type, MvxBasePresentationAttribute, MvxViewModelRequest>> _attributeTypesToShowMethodDictionary;
        protected Dictionary<Type, Action<Type, MvxBasePresentationAttribute, MvxViewModelRequest>> AttributeTypesToShowMethodDictionary
        {
            get
            {
                if (_attributeTypesToShowMethodDictionary == null)
                {
                    _attributeTypesToShowMethodDictionary = new Dictionary<Type, Action<Type, MvxBasePresentationAttribute, MvxViewModelRequest>>();
                    RegisterAttributeTypes();
                }
                return _attributeTypesToShowMethodDictionary;
            }
        }

        private Dictionary<Type, IList<MvxBasePresentationAttribute>> _viewModelToPresentationAttributeMap;
        protected Dictionary<Type, IList<MvxBasePresentationAttribute>> ViewModelToPresentationAttributeMap
        {
            get
            {
                if (_viewModelToPresentationAttributeMap == null)
                {
                    _viewModelToPresentationAttributeMap = new Dictionary<Type, IList<MvxBasePresentationAttribute>>();
                    RegisterAttributes();
                }
                return _viewModelToPresentationAttributeMap;
            }
        }

        protected readonly IMvxViewModelTypeFinder _viewModelTypeFinder;

        protected Lazy<IMvxNavigationSerializer> _lazyNavigationSerializerFactory;
        protected IMvxNavigationSerializer Serializer;
        public const string ViewModelRequestBundleKey = "__mvxViewModelRequest";

        public MvxAndroidViewPresenter(IEnumerable<Assembly> androidViewAssemblies)
        {
            _androidViewAssemblies = androidViewAssemblies;
            _viewModelTypeFinder = Mvx.Resolve<IMvxViewModelTypeFinder>();

            //Serializer = Mvx.Resolve<IMvxNavigationSerializer>();
        }

        private void RegisterAttributes()
        {
            var typesWithBasePresentationAttribute = _androidViewAssemblies
                         .SelectMany(x => x.DefinedTypes)
                         .Select(x => x.AsType())
                         .Where(x => x.HasBasePresentationAttribute())
                         .ToList();

            foreach (var typeWithAttribute in typesWithBasePresentationAttribute)
            {
                var viewModelType = GetAssociatedViewModelType(typeWithAttribute);

                if (!ViewModelToPresentationAttributeMap.ContainsKey(viewModelType))
                    ViewModelToPresentationAttributeMap.Add(viewModelType, new List<MvxBasePresentationAttribute>());

                foreach (var attribute in typeWithAttribute.GetBasePresentationAttributes())
                {
                    //TODO: Can we set the viewType from somewhere else?
                    attribute.ViewType = typeWithAttribute;
                    ViewModelToPresentationAttributeMap[viewModelType].Add(attribute);
                }
            }
        }

        private Type GetAssociatedViewModelType(Type fromFragmentType)
        {
            Type viewModelType = _viewModelTypeFinder.FindTypeOrNull(fromFragmentType);
            return viewModelType ?? fromFragmentType.GetBasePresentationAttributes().First().ViewModelType;
        }

        protected virtual void RegisterAttributeTypes()
        {
            _attributeTypesToShowMethodDictionary.Add(
               typeof(MvxActivityAttribute),
               (view, attribute, request) => ShowActivity(view, (MvxActivityAttribute)attribute, request));

            _attributeTypesToShowMethodDictionary.Add(
               typeof(MvxFragmentAttribute),
               (view, attribute, request) => ShowFragment(view, (MvxFragmentAttribute)attribute, request));

            _attributeTypesToShowMethodDictionary.Add(
               typeof(MvxDialogAttribute),
               (view, attribute, request) => ShowDialogFragment(view, (MvxDialogAttribute)attribute, request));
        }

        public override void Show(MvxViewModelRequest request)
        {
            var attribute = GetAttributeForViewModel(request.ViewModelType);
            var view = attribute.ViewType;
            var attributeType = attribute.GetType();

            if (AttributeTypesToShowMethodDictionary.TryGetValue(attributeType,
                out Action<Type, MvxBasePresentationAttribute, MvxViewModelRequest> showAction))
            {
                showAction.Invoke(view, attribute, request);
                return;
            }

            throw new KeyNotFoundException($"The type {attributeType.Name} is not configured in the presenter dictionary");
        }

        private MvxBasePresentationAttribute GetAttributeForViewModel(Type viewModelType)
        {
            IList<MvxBasePresentationAttribute> attributes;
            if (ViewModelToPresentationAttributeMap.TryGetValue(viewModelType, out attributes))
            {
                var attribute = attributes.FirstOrDefault();
                if (attribute.ViewType?.GetInterfaces().OfType<IMvxOverridePresentationAttribute>().FirstOrDefault() is IMvxOverridePresentationAttribute view)
                {
                    var presentationAttribute = view.PresentationAttribute();

                    if (presentationAttribute != null)
                        return presentationAttribute;
                }
                return attribute;
            }

            //TODO: Find view Type if attribute is unknown

            //TODO: Check if class implements IDialogInterface
            //TODO: Check if class is a Fragment


            //TODO: Check if it is a Dialog
            //TODO: Check if it is a Fragment

            return new MvxActivityAttribute() { ViewModelType = viewModelType };
        }

        protected virtual void ShowActivity(
            Type view,
            MvxActivityAttribute attribute,
            MvxViewModelRequest request)
        {
            var intent = CreateIntentForRequest(request);
            if (attribute.Extras != null)
                intent.PutExtras(attribute.Extras);
            ShowIntent(intent);
        }

        protected virtual Intent CreateIntentForRequest(MvxViewModelRequest request)
        {
            IMvxAndroidViewModelRequestTranslator requestTranslator = Mvx.Resolve<IMvxAndroidViewModelRequestTranslator>();

            if (request is MvxViewModelInstanceRequest)
            {
                var instanceRequest = requestTranslator.GetIntentWithKeyFor(((MvxViewModelInstanceRequest)request).ViewModelInstance);
                return instanceRequest.Item1;
            }
            return requestTranslator.GetIntentFor(request);
        }

        protected virtual void ShowIntent(Intent intent)
        {
            var activity = CurrentActivity;
            if (activity == null)
            {
                MvxTrace.Warning("Cannot Resolve current top activity");
                return;
            }
            activity.StartActivity(intent);
        }

        protected void ShowHostActivity(MvxFragmentAttribute attribute)
        {
            var currentHostViewModelType = GetCurrentActivityViewModelType();
            if (attribute.ParentActivityViewModelType != currentHostViewModelType)
            {
                //TODO: Check if Activity base extends FragmentActivity
                var hostViewModelRequest = MvxViewModelRequest.GetDefaultRequest(currentHostViewModelType);
                Show(hostViewModelRequest);
            }
        }

        protected Type GetCurrentActivityViewModelType()
        {
            Activity currentActivity = Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity;
            Type currentActivityType = currentActivity.GetType();

            var activityViewModelType = _viewModelTypeFinder.FindTypeOrNull(currentActivityType);
            return activityViewModelType;
        }

        protected virtual void ShowFragment(
            Type view,
            MvxFragmentAttribute attribute,
            MvxViewModelRequest request)
        {
            ShowHostActivity(attribute);

            if (CurrentActivity.FindViewById(attribute.FragmentContentId) == null)
                throw new NullReferenceException("FrameLayout to show Fragment not found");

            var fragmentName = FragmentJavaName(attribute.ViewType);
            var fragment = CreateFragment(fragmentName);

            var ft = CurrentActivity.FragmentManager.BeginTransaction();

            if(attribute.SharedElements != null)
            {
                foreach (var item in attribute.SharedElements)
                {
                    ft.AddSharedElement(item.Value, item.Key);
                }
            }
            if(!attribute.CustomAnimations.Equals((int.MinValue,int.MinValue,int.MinValue,int.MinValue)))
            {
                var customAnimations = attribute.CustomAnimations;
                ft.SetCustomAnimations(customAnimations.enter, customAnimations.exit, customAnimations.popEnter, customAnimations.popExit);
            }
            if(attribute.TransitionStyle != int.MinValue)
                ft.SetTransitionStyle(attribute.TransitionStyle);
            
            ft.Replace(attribute.FragmentContentId, (Fragment)fragment, fragmentName);
            ft.CommitNowAllowingStateLoss();
        }

        protected virtual IMvxFragmentView CreateFragment(string fragmentName)
        {
            var fragment = Fragment.Instantiate(CurrentActivity, fragmentName);
            return (IMvxFragmentView)fragment;
        }

        protected virtual void ShowDialogFragment(
            Type view,
            MvxDialogAttribute attribute,
            MvxViewModelRequest request)
        {
            var fragmentName = FragmentJavaName(attribute.ViewType);
            var dialog = (DialogFragment)CreateFragment(fragmentName);
            dialog.Cancelable = attribute.Cancelable;
            dialog.Show(CurrentFragmentManager, fragmentName);
        }

        protected virtual string FragmentJavaName(Type fragmentType)
        {
            return Class.FromType(fragmentType).Name;
        }

        public override void ChangePresentation(MvxPresentationHint hint)
        {
            if (HandlePresentationChange(hint)) return;

            var presentationHint = hint as MvxClosePresentationHint;
            if (presentationHint != null)
            {
                Close(presentationHint.ViewModelToClose);
                return;
            }

            MvxTrace.Warning("Hint ignored {0}", hint.GetType().Name);
        }

        public override void Close(IMvxViewModel viewModel)
        {
            var attribute = GetAttributeForViewModel(viewModel.GetType());

            if (attribute is MvxActivityAttribute)
            {
                //TODO: Check if a Dialog is shown

                if (CurrentFragmentManager.BackStackEntryCount > 0)
                    CurrentFragmentManager.PopBackStackImmediate(null, PopBackStackFlags.Inclusive);

                var activity = CurrentActivity;

                var currentView = activity as IMvxView;

                if (currentView == null)
                {
                    Mvx.Warning("Ignoring close for viewmodel - rootframe has no current page");
                    return;
                }

                if (currentView.ViewModel != viewModel)
                {
                    Mvx.Warning("Ignoring close for viewmodel - rootframe's current page is not the view for the requested viewmodel");
                    return;
                }

                activity.Finish();
            }
            else if (attribute is MvxFragmentAttribute fragment)
            {
                CurrentFragmentManager.PopBackStackImmediate();
            }
            else if (attribute is MvxDialogAttribute dialog)
            {
                CurrentFragmentManager.PopBackStackImmediate();
            }
        }
    }
}
