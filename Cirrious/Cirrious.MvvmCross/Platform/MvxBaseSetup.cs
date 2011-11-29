using Cirrious.MvvmCross.Conventions;
using Cirrious.MvvmCross.IoC;

namespace Cirrious.MvvmCross.Platform
{
    public abstract class MvxBaseSetup
    {
        public void Initialize()
        {
            InitializeIoC();
            InitializeConventions();
            InitializeApp();
            InitializeViewsContainer();
            InitializeViews();
        }

        protected virtual void InitializeIoC()
        {
            // initialise the IoC service registry
            // this also pulls in all platform services too
            MvxOpenNetCfServiceProviderSetup.Initialize();
        }

        protected virtual void InitializeConventions()
        {
            // initialize conventions
            MvxDefaultConventionSetup.Initialize();
        }

        protected virtual void InitializeApp()
        {            
            // would always expect this to be overridden!
        }

        protected virtual void InitializeViewsContainer()
        {
            // would always expect this to be overridden!
        }

        protected virtual void InitializeViews()
        {
            // would always expect this to be overridden!
        }
    }
}