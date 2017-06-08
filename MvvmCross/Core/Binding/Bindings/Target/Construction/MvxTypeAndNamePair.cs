// MvxTypeAndNamePair.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;

namespace MvvmCross.Binding.Bindings.Target.Construction
{
    public class MvxTypeAndNamePair
    {
        public MvxTypeAndNamePair()
        {
        }

        public MvxTypeAndNamePair(Type type, string name)
        {
            Type = type;
            Name = name;
        }

        public Type Type { get; set; }
        public string Name { get; set; }
    }
}