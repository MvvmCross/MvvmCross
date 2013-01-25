// MvxAndroidBindingResource.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.MvvmCross.Droid.Interfaces;
using Cirrious.MvvmCross.Exceptions;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;

namespace Cirrious.MvvmCross.Binding.Droid
{
    public class MvxAndroidBindingResource
        : IMvxServiceConsumer
    {
        public static readonly MvxAndroidBindingResource Instance = new MvxAndroidBindingResource();

        private MvxAndroidBindingResource()
        {
			var setup = this.GetService<IMvxAndroidGlobals>();
            var resourceTypeName = setup.ExecutableNamespace + ".Resource";
            Type resourceType = setup.ExecutableAssembly.GetType(resourceTypeName);
            if (resourceType == null)
                throw new MvxException("Unable to find resource type - " + resourceTypeName);
            try
            {
                BindingTagUnique = (int) resourceType.GetNestedType("Id").GetField("MvxBindingTagUnique").GetValue(null);

                BindingStylableGroupId =
                    (int[]) resourceType.GetNestedType("Styleable").GetField("MvxBinding").GetValue(null);
                BindingBindId =
                    (int) resourceType.GetNestedType("Styleable").GetField("MvxBinding_MvxBind").GetValue(null);

                HttpImageViewStylableGroupId =
                    (int[]) resourceType.GetNestedType("Styleable").GetField("MvxHttpImageView").GetValue(null);
                HttpSourceBindId =
                    (int)
                    resourceType.GetNestedType("Styleable").GetField("MvxHttpImageView_MvxHttpSource").GetValue(null);

                BindableListViewStylableGroupId =
                    (int[]) resourceType.GetNestedType("Styleable").GetField("MvxBindableListView").GetValue(null);
                BindableListItemTemplateId =
                    (int)
                    resourceType.GetNestedType("Styleable")
                                .GetField("MvxBindableListView_MvxItemTemplate")
                                .GetValue(null);
                BindableDropDownListItemTemplateId =
                    (int)
                    resourceType.GetNestedType("Styleable")
                                .GetField("MvxBindableListView_MvxDropDownItemTemplate")
                                .GetValue(null);
            }
            catch (Exception exception)
            {
                throw exception.MvxWrap(
                    "Error finding resource ids for MvxBinding - please make sure ResourcesToCopy are linked into the executable");
            }
        }

        public int BindingTagUnique { get; private set; }

        public int[] BindingStylableGroupId { get; private set; }
        public int BindingBindId { get; private set; }

        public int[] HttpImageViewStylableGroupId { get; private set; }
        public int HttpSourceBindId { get; private set; }

        public int[] BindableListViewStylableGroupId { get; private set; }
        public int BindableListItemTemplateId { get; private set; }
        public int BindableDropDownListItemTemplateId { get; private set; }
    }
}