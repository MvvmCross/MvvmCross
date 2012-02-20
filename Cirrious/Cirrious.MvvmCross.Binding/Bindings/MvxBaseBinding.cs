using Cirrious.MvvmCross.Binding.Interfaces;

namespace Cirrious.MvvmCross.Binding.Bindings
{
    public abstract class MvxBaseBinding  : IMvxBinding
    {
        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            // nothing to do in this base class
        }
    }
}