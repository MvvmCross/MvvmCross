// MvxCompositeBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Bindings
{
    using System.Collections.Generic;
    using System.Linq;

    public class MvxCompositeBinding : MvxBinding
    {
        private readonly List<IMvxBinding> _bindings;

        public MvxCompositeBinding(params IMvxBinding[] args)
        {
            this._bindings = args.ToList();
        }

        public void Add(params IMvxBinding[] args)
        {
            this._bindings.AddRange(args);
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                foreach (var mvxBinding in this._bindings)
                {
                    mvxBinding.Dispose();
                }
                this._bindings.Clear();
            }
            base.Dispose(isDisposing);
        }
    }
}