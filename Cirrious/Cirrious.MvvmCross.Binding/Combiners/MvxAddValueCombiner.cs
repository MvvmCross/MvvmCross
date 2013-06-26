// MvxAddValueCombiner.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace Cirrious.MvvmCross.Binding.Combiners
{
    public class MvxAddValueCombiner
        : MvxObjectAsStringPairwiseValueCombiner
    {
        protected override bool CombineStringAndDouble(string input1, double input2, out object value)
        {
            value = input1 + input2.ToString();
            return true;
        }

        protected override bool CombineStringAndInt(string input1, int input2, out object value)
        {
            value = input1 + input2.ToString();
            return true;
        }

        protected override bool CombineStringAndNull(string input1, out object value)
        {
            value = input1;
            return true;
        }

        protected override bool CombineDoubleAndDouble(double input1, double input2, out object value)
        {
            value = input1 + input2;
            return true;
        }

        protected override bool CombineDoubleAndInt(double input1, int input2, out object value)
        {
            value = input1 + input2;
            return true;
        }

        protected override bool CombineDoubleAndNull(double input1, out object value)
        {
            value = input1;
            return true;
        }

        protected override bool CombineIntAndDouble(int input1, double input2, out object value)
        {
            value = input1 + input2;
            return true;
        }

        protected override bool CombineIntAndInt(int input1, int input2, out object value)
        {
            value = input1 + input2;
            return true;
        }

        protected override bool CombineIntAndNull(int input1, out object value)
        {
            value = input1;
            return true;
        }

        protected override bool CombineNullAndDouble(double input2, out object value)
        {
            value = input2;
            return true;
        }

        protected override bool CombineNullAndInt(int input2, out object value)
        {
            value = input2;
            return true;
        }

        protected override bool CombineTwoNulls(out object value)
        {
            value = null;
            return true;
        }

        protected override bool CombineStringAndString(string input1, string input2, out object value)
        {
            value = input1 + input2;
            return true;
        }

        protected override bool CombineIntAndString(int input1, string input2, out object value)
        {
            value = input1 + input2;
            return true;
        }

        protected override bool CombineDoubleAndString(double input1, string input2, out object value)
        {
            value = input1 + input2;
            return true;
        }

        protected override bool CombineNullAndString(string input2, out object value)
        {
            value = input2;
            return true;
        }
    }
}