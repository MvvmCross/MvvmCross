// MvxObjectAsStringPairwiseValueCombiner.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace Cirrious.MvvmCross.Binding.Combiners
{
    public abstract class MvxObjectAsStringPairwiseValueCombiner
        : MvxPairwiseValueCombiner
    {
        protected abstract bool CombineStringAndString(string input1, string input2, out object value);

        protected abstract bool CombineLongAndString(long input1, string input2, out object value);

        protected abstract bool CombineDoubleAndString(double input1, string input2, out object value);

        protected abstract bool CombineNullAndString(string input2, out object value);

        protected abstract bool CombineStringAndDouble(string input1, double input2, out object value);

        protected abstract bool CombineStringAndLong(string input1, long input2, out object value);

        protected abstract bool CombineStringAndNull(string input1, out object value);

        protected sealed override bool CombineObjectAndObject(object object1, object object2, out object value)
        {
            return CombineStringAndString(object1.ToString(), object2.ToString(), out value);
        }

        protected sealed override bool CombineLongAndObject(long int1, object object1, out object value)
        {
            return CombineLongAndString(int1, object1.ToString(), out value);
        }

        protected sealed override bool CombineDoubleAndObject(double double1, object object1, out object value)
        {
            return CombineDoubleAndString(double1, object1.ToString(), out value);
        }

        protected sealed override bool CombineNullAndObject(object object1, out object value)
        {
            return CombineNullAndString(object1.ToString(), out value);
        }

        protected sealed override bool CombineObjectAndDouble(object input1, double input2, out object value)
        {
            return CombineStringAndDouble(input1.ToString(), input2, out value);
        }

        protected sealed override bool CombineObjectAndLong(object input1, long input2, out object value)
        {
            return CombineStringAndLong(input1.ToString(), input2, out value);
        }

        protected sealed override bool CombineObjectAndNull(object input1, out object value)
        {
            return CombineStringAndNull(input1.ToString(), out value);
        }
    }
}