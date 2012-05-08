using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Platform;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Platform.Diagnostics;

namespace Cirrious.MvvmCross.Plugins.Json
{
    public class ModuleLoader
        : IMvxServiceProducer<IMvxTextSerializer>
        , IMvxServiceProducer<IMvxJsonConverter>
    {
        public static readonly ModuleLoader Instance = new ModuleLoader();

        private bool _loaded;

        public void EnsureLoaded(bool useJsonAsDefaultTextSerializer=true)
        {
            if (_loaded)
            {
                MvxTrace.Trace(MvxTraceLevel.Warning, "Warning - you should really only initialize Json serialization once otherwise there is a risk of different clients requesting different useJsonAsDefaultTextSerializer options");
                return;
            }

            this.RegisterServiceType<IMvxJsonConverter, MvxJsonConverter>();
            if (useJsonAsDefaultTextSerializer)
            {
                this.RegisterServiceType<IMvxTextSerializer, MvxJsonConverter>();
            }

            _loaded = true;
        }
    }
}
