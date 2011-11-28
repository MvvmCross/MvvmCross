using Cirrious.MvvmCross.Exceptions;

namespace Cirrious.MvvmCross.Core
{
    public abstract class MvxSingleton<TInterface> where TInterface : class
    {
        public static TInterface Instance { get; private set; }

        protected MvxSingleton()
        {
            if (Instance != null)
                throw new MvxException("You cannot create more than one instance of MvxSingleton");

            Instance = this as TInterface;
        }
    }
}