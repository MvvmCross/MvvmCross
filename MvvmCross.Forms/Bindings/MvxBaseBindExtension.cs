// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MvvmCross.Binding;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MvvmCross.Forms.Bindings
{
    public abstract class MvxBaseBindExtension : IMarkupExtension
    {
        public MvxBindingMode Mode { get; set; } = MvxBindingMode.Default;
        public string Converter { get; set; }
        public string ConverterParameter { get; set; }
        public string FallbackValue { get; set; }
        public string PropertyName { get; private set; } = string.Empty;

        protected object BindableObjectRaw;
        protected object BindablePropertyRaw;

        protected BindableObject Bindable;
        protected BindableProperty BindableProp;
        protected IEnumerable<BindableObject> BindableTree;
        public Page SourcePage { protected get; set; }

        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));
            var valueTargetProvider = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
            var rootObjectProvider = serviceProvider.GetService(typeof(IRootObjectProvider)) as IRootObjectProvider;
            Initialize(valueTargetProvider, rootObjectProvider);

            return ProvideValue(serviceProvider);
        }

        public abstract object ProvideValue(IServiceProvider serviceProvider);

        private void Initialize(IProvideValueTarget valueTargetProvider, IRootObjectProvider rootObjectProvider)
        {
            // if XamlCompilation is active, IRootObjectProvider is not available, but SimpleValueTargetProvider is available
            // if XamlCompilation is inactive, IRootObjectProvider is available, but SimpleValueTargetProvider is not available
            object rootObject;

            var propertyInfo = valueTargetProvider.GetType().GetTypeInfo().DeclaredProperties.FirstOrDefault(dp => dp.Name.Contains("ParentObjects"));
            if (propertyInfo == null) throw new ArgumentNullException("ParentObjects");
            var parentObjects = (propertyInfo.GetValue(valueTargetProvider) as IEnumerable<object>).ToList();
            BindableTree = parentObjects.Cast<BindableObject>();

            if (rootObjectProvider == null && valueTargetProvider == null)
                throw new ArgumentException("serviceProvider does not provide an IRootObjectProvider or SimpleValueTargetProvider");
            if (rootObjectProvider == null)
            {
                var parentObject = parentObjects.FirstOrDefault(pO => pO.GetType().GetTypeInfo().IsSubclassOf(typeof(Page)));

                BindableObjectRaw = parentObjects.FirstOrDefault();
                Bindable = BindableObjectRaw as BindableObject;
                rootObject = parentObject ?? throw new ArgumentNullException("parentObject");
            }
            else
            {
                rootObject = rootObjectProvider.RootObject;
                BindableObjectRaw = valueTargetProvider.TargetObject;
                Bindable = BindableObjectRaw as BindableObject;
            }

            BindablePropertyRaw = valueTargetProvider.TargetProperty;
            if (BindablePropertyRaw is BindableProperty bp)
            {
                BindableProp = bp;
                PropertyName = bp.PropertyName;
            }
            else if (BindablePropertyRaw is PropertyInfo pi)
                PropertyName = pi.Name;
            else if(BindablePropertyRaw != null)
                PropertyName = BindablePropertyRaw.ToString();

            if (rootObject is Page providedPage)
                SourcePage = SourcePage ?? providedPage;
        }
    }
}
