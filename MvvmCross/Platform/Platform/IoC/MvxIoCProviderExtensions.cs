namespace MvvmCross.Platform.IoC
{
    public static class MvxIoCProviderExtensions
    {
        public static IMvxIoCProvider CreateChildContainer(this IMvxIoCProvider iocProvider)
        {
            return new MvxIoCContainer(iocProvider);
        }
    }
}
