using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.Services;
using Cirrious.MvvmCross.ViewModel;

namespace Cirrious.MvvmCross.Conventions
{
    public sealed class MvxDefaultConventionSetup 
                        : IMvxServiceProducer<IMvxViewModelLocatorAnalyser>
    {
        public static void Initialize()
        {
            new MvxDefaultConventionSetup().InitializeImpl();
        }

        private MvxDefaultConventionSetup()
        {            
        }

        private void InitializeImpl()
        {
            this.RegisterServiceType<IMvxViewModelLocatorAnalyser, MvxViewModelLocatorAnalyser>();
        }
    }
}