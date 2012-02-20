using System.Linq;
using System.Text;

using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Cirrious.MvvmCross.Binding.Android;
using Cirrious.MvvmCross.Binding.Binders;
using Cirrious.MvvmCross.Binding.Interfaces.Binders;
using Cirrious.MvvmCross.Binding.Interfaces.Bindings.Target.Construction;
using Cirrious.MvvmCross.ExtensionMethods;

namespace CustomerManagement.Droid
{
    public class CustomerManagementBindingSetup
        : MvxAndroidBindingSetup
    {
        public CustomerManagementBindingSetup()
        {

        }

        protected override void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            base.FillTargetFactories(registry);
        }

        protected override void FillValueConverters(IMvxValueConverterRegistry registry)
        {
            base.FillValueConverters(registry);

            //var staticFiller = new MvxStaticBasedValueConverterRegistryFiller(registry);
            //staticFiller.AddStaticFieldConverters(typeof(Converters));
        }
    }
}