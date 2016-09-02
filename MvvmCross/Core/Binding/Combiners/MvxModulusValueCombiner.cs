// MvxModulusValueCombiner.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Combiners
{
    public class MvxModulusValueCombiner
        : MvxNumericOnlyValueCombiner
    {
		protected override bool CombineDecimalAndDecimal(decimal input1, decimal input2, out object value)
		{
			value = input1 % input2;
			return true;
		}

		protected override bool CombineDecimalAndDouble(decimal input1, double input2, out object value)
		{
			value = (double)input1 % input2;
			return true;
		}

		protected override bool CombineDecimalAndLong(decimal input1, long input2, out object value)
		{
			value = input1 % input2;
			return true;
		}

		protected override bool CombineDecimalAndNull(decimal input1, out object value)
		{
			value = 0m;
			return true;
		}

		protected override bool CombineDoubleAndDecimal(double input1, decimal input2, out object value)
		{
			value = input1 % (double)input2;
			return true;
		}

		protected override bool CombineDoubleAndDouble(double input1, double input2, out object value)
        {
            value = input1 % input2;
            return true;
        }

        protected override bool CombineDoubleAndLong(double input1, long input2, out object value)
        {
            value = input1 % input2;
            return true;
        }

        protected override bool CombineDoubleAndNull(double input1, out object value)
        {
            // divided by zero... hmmmm
            value = 0.0;
            return true;
        }

		protected override bool CombineLongAndDecimal(long input1, decimal input2, out object value)
		{
			value = input1 % input2;
			return true;
		}

		protected override bool CombineLongAndDouble(long input1, double input2, out object value)
        {
            value = input1 % input2;
            return true;
        }

        protected override bool CombineLongAndLong(long input1, long input2, out object value)
        {
            value = input1 % input2;
            return true;
        }

        protected override bool CombineLongAndNull(long input1, out object value)
        {
            // divided by zero... hmmmm
            value = 0;
            return true;
        }

		protected override bool CombineNullAndDecimal(decimal input2, out object value)
		{
			value = 0 % input2;
			return true;
		}

		protected override bool CombineNullAndDouble(double input2, out object value)
        {
            value = 0 % input2;
            return true;
        }

        protected override bool CombineNullAndLong(long input2, out object value)
        {
            value = 0 % input2;
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