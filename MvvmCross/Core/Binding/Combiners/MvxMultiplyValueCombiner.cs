// MvxMultiplyValueCombiner.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Combiners
{
    public class MvxMultiplyValueCombiner
        : MvxNumericOnlyValueCombiner
    {
        protected override bool CombineDoubleAndDouble(double input1, double input2, out object value)
        {
            value = input1 * input2;
            return true;
        }

        protected override bool CombineDoubleAndLong(double input1, long input2, out object value)
        {
            value = input1 * input2;
            return true;
        }

        protected override bool CombineDoubleAndNull(double input1, out object value)
        {
            value = null;
            return true;
        }

        protected override bool CombineLongAndDouble(long input1, double input2, out object value)
        {
            value = input1 * input2;
            return true;
        }

        protected override bool CombineLongAndLong(long input1, long input2, out object value)
        {
            value = input1 * input2;
            return true;
        }

        protected override bool CombineLongAndNull(long input1, out object value)
        {
            value = null;
            return true;
        }

        protected override bool CombineNullAndDouble(double input2, out object value)
        {
            value = null;
            return true;
        }

        protected override bool CombineNullAndLong(long input2, out object value)
        {
            value = null;
            return true;
        }

        protected override bool CombineTwoNulls(out object value)
        {
            value = null;
            return true;
        }
    }
}