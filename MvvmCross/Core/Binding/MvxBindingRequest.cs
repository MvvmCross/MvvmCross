// MvxBindingRequest.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding
{
    using MvvmCross.Binding.Bindings;

    public class MvxBindingRequest
    {
        public MvxBindingRequest()
        {
        }

        public MvxBindingRequest(object source, object target, MvxBindingDescription description)
        {
            this.Target = target;
            this.Source = source;
            this.Description = description;
        }

        public object Target { get; set; }
        public object Source { get; set; }
        public MvxBindingDescription Description { get; set; }
    }
}