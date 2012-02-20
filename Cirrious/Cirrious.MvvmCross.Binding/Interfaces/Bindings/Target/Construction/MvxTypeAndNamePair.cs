using System;

namespace Cirrious.MvvmCross.Binding.Interfaces.Bindings.Target.Construction
{
    public class MvxTypeAndNamePair
    {
        public Type Type { get; set; }
        public string Name { get; set; }

        public MvxTypeAndNamePair()
        {
        }

        public MvxTypeAndNamePair(Type type, string name)
        {
            Type = type;
            Name = name;
        }
    }
}