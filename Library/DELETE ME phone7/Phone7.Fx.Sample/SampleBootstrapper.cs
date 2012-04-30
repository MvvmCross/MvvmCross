using Phone7.Fx.Ioc;
using Phone7.Fx.Sample.Services;
using Phone7.Fx.Sample.Services.Contracts;

namespace Phone7.Fx.Sample
{
    public class SampleBootstrapper:PhoneBootstrapper
    {
        public SampleBootstrapper()
        {
            int i = 0;
        }

        protected override void Configure()
        {
            Container.Current.RegisterType<IHelloService, HelloService>();
        }
    }
}