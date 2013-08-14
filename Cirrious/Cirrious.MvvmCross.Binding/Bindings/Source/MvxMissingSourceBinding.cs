// MvxMissingSourceBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;

namespace Cirrious.MvvmCross.Binding.Bindings.Source
{
    public class MvxMissingSourceBinding : MvxSourceBinding
    {
        public MvxMissingSourceBinding(object source) : base(source)
        {
        }

        public override void SetValue(object value)
        {
            // nothing we can do here - binding is missing
        }

        public override Type SourceType
        {
            get { return typeof(object); }
        }

        public override bool TryGetValue(out object value)
        {
            // binding is missing so return false
            value = null;
            return false;
        }
    }
}