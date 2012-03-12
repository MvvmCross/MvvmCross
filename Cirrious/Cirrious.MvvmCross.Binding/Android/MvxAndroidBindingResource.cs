#region Copyright
// <copyright file="MvxAndroidBindingResource.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;
using Cirrious.MvvmCross.Android.Interfaces;
using Cirrious.MvvmCross.Exceptions;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;

namespace Cirrious.MvvmCross.Binding.Android
{
    public class MvxAndroidBindingResource
        : IMvxServiceConsumer<IMvxAndroidGlobals>
    {
        public readonly static MvxAndroidBindingResource Instance = new MvxAndroidBindingResource();

        private MvxAndroidBindingResource()
        {
            var setup = this.GetService();
            var resourceTypeName = setup.ExecutableNamespace + ".Resource";
            Type resourceType = setup.ExecutableAssembly.GetType(resourceTypeName); 
            if (resourceType == null)
                throw new MvxException("Unable to find resource type - " + resourceTypeName);
            try
            {
                BindingTagUnique = (int)resourceType.GetNestedType("Id").GetField("MvxBindingTagUnique").GetValue(null);

                BindingStylableGroupId = (int[])resourceType.GetNestedType("Styleable").GetField("MvxBinding").GetValue(null);
                BindingBindId = (int)resourceType.GetNestedType("Styleable").GetField("MvxBinding_MvxBind").GetValue(null);

                HttpImageViewStylableGroupId = (int[])resourceType.GetNestedType("Styleable").GetField("MvxHttpImageView").GetValue(null);
                HttpSourceBindId = (int)resourceType.GetNestedType("Styleable").GetField("MvxHttpImageView_MvxHttpSource").GetValue(null);

                BindableListViewStylableGroupId = (int[])resourceType.GetNestedType("Styleable").GetField("MvxBindableListView").GetValue(null);
                BindableListItemTemplateId = (int)resourceType.GetNestedType("Styleable").GetField("MvxBindableListView_MvxItemTemplate").GetValue(null);
            }
            catch (Exception exception)
            {                
                throw exception.MvxWrap("Error finding resource ids for MvxBinding - please make sure ResourcesToCopy are linked into the executable");
            }
        }

        public int BindingTagUnique { get; private set; }

        public int[] BindingStylableGroupId { get; private set; }
        public int BindingBindId { get; private set; }

        public int[] HttpImageViewStylableGroupId { get; private set; }
        public int HttpSourceBindId { get; private set; }

        public int[] BindableListViewStylableGroupId { get; private set; }
        public int BindableListItemTemplateId { get; private set; }

    }
}