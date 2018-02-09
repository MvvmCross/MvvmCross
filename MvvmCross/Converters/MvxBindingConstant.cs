// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Converters
{
    public class MvxBindingConstant
    {
        public static readonly MvxBindingConstant DoNothing = new MvxBindingConstant("DoNothing");
        public static readonly MvxBindingConstant UnsetValue = new MvxBindingConstant("UnsetValue");

        private readonly string _debug;

        private MvxBindingConstant(string debug)
        {
            _debug = debug;
        }

        public override string ToString()
        {
            return "Binding:" + _debug;
        }
    }
}
