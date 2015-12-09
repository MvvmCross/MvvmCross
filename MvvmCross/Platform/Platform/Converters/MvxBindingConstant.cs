// MvxBindingConstant.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Platform.Converters
{
    public class MvxBindingConstant
    {
        public static readonly MvxBindingConstant DoNothing = new MvxBindingConstant("DoNothing");
        public static readonly MvxBindingConstant UnsetValue = new MvxBindingConstant("UnsetValue");

        private readonly string _debug;

        private MvxBindingConstant(string debug)
        {
            this._debug = debug;
        }

        public override string ToString()
        {
            return "Binding:" + this._debug;
        }
    }
}