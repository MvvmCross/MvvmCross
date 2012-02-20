using System;
using System.Reflection;
using Cirrious.MvvmCross.Android.Interfaces;
using Cirrious.MvvmCross.Android.Platform;
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

        public int[] BindableListViewStylableGroupId { get; private set; }
        public int BindableListItemTemplateId { get; private set; }

    }
}