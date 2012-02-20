using System.Collections.Generic;
using System.Linq;
using Cirrious.MvvmCross.Binding.Interfaces;

namespace Cirrious.MvvmCross.Binding.Bindings
{
    public class MvxCompositeBinding : MvxBaseBinding
    {
        private readonly List<IMvxBinding> _bindings;

        public MvxCompositeBinding(params IMvxBinding[] args)
        {
            _bindings = args.ToList();
        }

        public void Add(params IMvxBinding[] args)
        {
            _bindings.AddRange(args);
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                _bindings.ForEach(x => x.Dispose());
                _bindings.Clear();
            }
            base.Dispose(isDisposing);
        }
    }
}