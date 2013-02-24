// MvxDroidBindingResource.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.CrossCore.Droid.Interfaces;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Interfaces.IoC;

namespace Cirrious.MvvmCross.Binding.Droid
{
    public class MvxDroidBindingResource
        : IMvxConsumer
    {
        public static readonly MvxDroidBindingResource Instance = new MvxDroidBindingResource();

        private MvxDroidBindingResource()
        {
            var setup = this.Resolve<IMvxAndroidGlobals>();
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

                ImageViewStylableGroupId =
                    (int[]) resourceType.GetNestedType("Styleable").GetField("MvxImageView").GetValue(null);
                SourceBindId =
                    (int)
                    resourceType.GetNestedType("Styleable").GetField("MvxImageView_MvxSource").GetValue(null);

                ListViewStylableGroupId =
                    (int[]) resourceType.GetNestedType("Styleable").GetField("MvxListView").GetValue(null);
                ListItemTemplateId =
                    (int)
                    resourceType.GetNestedType("Styleable")
                                .GetField("MvxListView_MvxItemTemplate")
                                .GetValue(null);
                DropDownListItemTemplateId =
                    (int)
                    resourceType.GetNestedType("Styleable")
                                .GetField("MvxListView_MvxDropDownItemTemplate")
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

        public int[] ImageViewStylableGroupId { get; private set; }
        public int SourceBindId { get; private set; }

        public int[] ListViewStylableGroupId { get; private set; }
        public int ListItemTemplateId { get; private set; }
        public int DropDownListItemTemplateId { get; private set; }
    }
}