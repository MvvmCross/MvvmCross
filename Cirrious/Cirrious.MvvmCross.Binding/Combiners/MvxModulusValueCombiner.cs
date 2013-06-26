// MvxModulusValueCombiner.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace Cirrious.MvvmCross.Binding.Combiners
{
    public class MvxModulusValueCombiner
        : MvxNumericOnlyValueCombiner
    {
        protected override bool CombineDoubleAndDouble(double input1, double input2, out object value)
        {
            value = input1%input2;
            return true;
        }

        protected override bool CombineDoubleAndInt(double input1, int input2, out object value)
        {
            value = input1%input2;
            return true;
        }

        protected override bool CombineDoubleAndNull(double input1, out object value)
        {
            // divided by zero... hmmmm
            value = 0.0;
            return true;
        }

        protected override bool CombineIntAndDouble(int input1, double input2, out object value)
        {
            value = input1%input2;
            return true;
        }

        protected override bool CombineIntAndInt(int input1, int input2, out object value)
        {
            value = input1%input2;
            return true;
        }

        protected override bool CombineIntAndNull(int input1, out object value)
        {
            // divided by zero... hmmmm
            value = 0;
            return true;
        }

        protected override bool CombineNullAndDouble(double input2, out object value)
        {
            value = 0%input2;
            return true;
        }

        protected override bool CombineNullAndInt(int input2, out object value)
        {
            value = 0%input2;
            return true;
        }

        protected override bool CombineTwoNulls(out object value)
        {
            // zero divided by zero... hmmmm
            value = null;
            return true;
        }
    }
}