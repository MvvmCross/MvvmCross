// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Binding.Combiners
{
    public abstract class MvxNumericOnlyValueCombiner
        : MvxObjectAsStringPairwiseValueCombiner
    {
        protected override bool CombineStringAndDouble(string input1, double input2, out object value)
        {
            value = null;
            return false;
        }

        protected override bool CombineStringAndLong(string input1, long input2, out object value)
        {
            value = null;
            return false;
        }

        protected override bool CombineStringAndDecimal(string input1, decimal input2, out object value)
        {
            value = null;
            return false;
        }

        protected override bool CombineStringAndNull(string input1, out object value)
        {
            value = null;
            return false;
        }

        protected sealed override bool CombineStringAndString(string input1, string input2, out object value)
        {
            value = null;
            return false;
        }

        protected sealed override bool CombineLongAndString(long input1, string input2, out object value)
        {
            value = null;
            return false;
        }

        protected sealed override bool CombineDoubleAndString(double input1, string input2, out object value)
        {
            value = null;
            return false;
        }

        protected sealed override bool CombineDecimalAndString(decimal input1, string input2, out object value)
        {
            value = null;
            return false;
        }

        protected sealed override bool CombineNullAndString(string input2, out object value)
        {
            value = null;
            return false;
        }
    }
}
