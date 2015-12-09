// MvxTypeAndNamePair.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Bindings.Target.Construction
{
    using System;

    public class MvxTypeAndNamePair
    {
        public MvxTypeAndNamePair()
        {
        }

        public MvxTypeAndNamePair(Type type, string name)
        {
            this.Type = type;
            this.Name = name;
        }

        public Type Type { get; set; }
        public string Name { get; set; }
    }
}