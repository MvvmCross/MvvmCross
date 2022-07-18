// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

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
