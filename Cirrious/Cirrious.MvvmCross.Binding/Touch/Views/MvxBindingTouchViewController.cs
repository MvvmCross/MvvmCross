using System.Collections.Generic;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Touch.Views;
using Cirrious.MvvmCross.Views;
using MonoTouch.Foundation;

namespace Cirrious.MvvmCross.Binding.Touch.Views
{
    public class MvxBindingTouchViewController<TViewModel>
        : MvxTouchViewController<TViewModel>
          , IMvxServiceConsumer<IMvxBinder>
        where TViewModel : class, IMvxViewModel
    {
        private readonly List<IMvxUpdateableBinding> _bindings;

        protected MvxBindingTouchViewController(MvxShowViewModelRequest request) 
            : base(request)
        {
            _bindings = new List<IMvxUpdateableBinding>();
        }

        protected MvxBindingTouchViewController(MvxShowViewModelRequest request, string nibName, NSBundle bundle) 
            : base(request, nibName, bundle)
        {
            _bindings = new List<IMvxUpdateableBinding>();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ClearBindings();
            }

            base.Dispose(disposing);
        }

        public override void ViewDidUnload()
        {
            ClearBindings();
            base.ViewDidUnload();
        }

        protected void ClearBindings()
        {
            _bindings.ForEach(x => x.Dispose());
            _bindings.Clear();
        }

        protected void AddBinding(IMvxUpdateableBinding binding)
        {
            _bindings.Add(binding);
        }

        protected void AddBindings(IEnumerable<IMvxUpdateableBinding> bindings)
        {
            _bindings.AddRange(bindings);
        }

        protected void AddBindings(object target, string bindingText)
        {
            AddBindings(ViewModel, target, bindingText);
        }

        protected void AddBindings(object source, object target, string bindingText)
        {
            var binder = this.GetService<IMvxBinder>();
            AddBindings(binder.Bind(source, target, bindingText));
        }

        protected void AddBindings(object target, IEnumerable<MvxBindingDescription> bindingDescriptions)
        {
            AddBindings(ViewModel, target, bindingDescriptions);
        }

        protected void AddBindings(object source, object target, IEnumerable<MvxBindingDescription> bindingDescriptions)
        {
            var binder = this.GetService<IMvxBinder>();
            AddBindings(binder.Bind(source, target, bindingDescriptions));
        }

        protected void AddBindings(string targetPropertyName, IEnumerable<MvxBindingDescription> bindingDescriptions)
        {
            AddBindings(ViewModel, targetPropertyName, bindingDescriptions);
        }

        protected void AddBindings(object source, string targetPropertyName, IEnumerable<MvxBindingDescription> bindingDescriptions)
        {
            object target;
            if (!TryGetPropertyValue(targetPropertyName, out target))
                return;

            AddBindings(source, target, bindingDescriptions);
        }

        protected void AddBindings(object source, string targetPropertyName, string bindingText)
        {
            object target;
            if (!TryGetPropertyValue(targetPropertyName, out target))
                return;

            AddBindings(source, target, bindingText);
        }

        protected void AddBindings(string targetPropertyName, string bindingText)
        {
            AddBindings(ViewModel, targetPropertyName, bindingText);
        }

        protected void AddBindings(IDictionary<string, string> bindingMap)
        {
            AddBindings(ViewModel, bindingMap);
        }

        protected void AddBindings(object source, IDictionary<string, string> bindingMap)
        {
            foreach (var kvp in bindingMap)
            {
                AddBindings(source, kvp.Key, kvp.Value);
            }
        }

        protected void AddBindings(object bindingObject)
        {
            AddBindings(ViewModel, bindingObject);
        }

        protected void AddBindings(object source, object bindingObject)
        {
            var bindingMap = bindingObject.ToSimplePropertyDictionary();
            AddBindings(source, bindingMap);
        }

        protected void AddBindings(IDictionary<object, string> bindingMap)
        {
            AddBindings(ViewModel, bindingMap);
        }

        protected void AddBindings(object source, IDictionary<object, string> bindingMap)
        {
            foreach (var kvp in bindingMap)
            {
                var candidatePropertyName = kvp.Key as string;
                if (candidatePropertyName == null)
                    AddBindings(source, kvp.Key, kvp.Value);
                else
                    AddBindings(source, candidatePropertyName, kvp.Value);
            }
        }

        protected void AddBindings(IDictionary<object, IEnumerable<MvxBindingDescription>> bindingMap)
        {
            AddBindings(ViewModel, bindingMap);
        }

        protected void AddBindings(object source, IDictionary<object, IEnumerable<MvxBindingDescription>> bindingMap)
        {
            foreach (var kvp in bindingMap)
            {
                var candidatePropertyName = kvp.Key as string;
                if (candidatePropertyName == null)
                    AddBindings(source, kvp.Key, kvp.Value);
                else
                    AddBindings(source, candidatePropertyName, kvp.Value);
            }
        }

        private bool TryGetPropertyValue(string targetPropertyName, out object value)
        {
            var propertyInfo = this.GetType().GetProperty(targetPropertyName);
            if (propertyInfo == null)
            {
                MvxBindingTrace.Trace(MvxBindingTraceLevel.Warning, "Unable to find property for binding - property {0}",
                                      targetPropertyName);
                value = null;
                return false;
            }
            value = propertyInfo.GetValue(this, null);
            if (value == null)
            {
                MvxBindingTrace.Trace(MvxBindingTraceLevel.Warning, "property for binding is null - property {0}",
                                      targetPropertyName);
                return false;
            }
            
            return true;
        }
    }
}