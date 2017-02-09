namespace MvvmCross.Binding.Combiners
{
    public class MvxGreaterThanValueCombiner 
        : MvxObjectAsStringPairwiseValueCombiner
    {
        protected override bool CombineDoubleAndDouble(double input1, double input2, out object value)
        {
            value = input1.CompareTo(input2) > 0;
            return true;
        }

        protected override bool CombineDoubleAndLong(double input1, long input2, out object value)
        {
            value = input1.CompareTo(input2) > 0;
            return true;
        }

        protected override bool CombineDoubleAndNull(double input1, out object value)
        {
            value = input1.CompareTo(0) > 0;
            return true;
        }

        protected override bool CombineLongAndDouble(long input1, double input2, out object value)
        {
            value = ((double)input1).CompareTo(input2) > 0;
            return true;
        }

        protected override bool CombineLongAndLong(long input1, long input2, out object value)
        {
            value = input1.CompareTo(input2) > 0;
            return true;
        }

        protected override bool CombineLongAndNull(long input1, out object value)
        {
            value = input1.CompareTo(0) > 0;
            return true;
        }

        protected override bool CombineNullAndDouble(double input2, out object value)
        {
            value = (0.0).CompareTo(input2) > 0;
            return true;
        }

        protected override bool CombineNullAndLong(long input2, out object value)
        {
            value = (0L).CompareTo(input2) > 0;
            return true;
        }

        protected override bool CombineTwoNulls(out object value)
        {
            value = false;
            return true;
        }

        protected override bool CombineStringAndString(string input1, string input2, out object value)
        {
            value = input1.CompareTo(input2) > 0;
            return true;
        }

        protected override bool CombineLongAndString(long input1, string input2, out object value)
        {
            value = false;
            return true;
        }

        protected override bool CombineDoubleAndString(double input1, string input2, out object value)
        {
            value = false;
            return true;
        }

        protected override bool CombineNullAndString(string input2, out object value)
        {
            value = false;
            return true;
        }

        protected override bool CombineStringAndDouble(string input1, double input2, out object value)
        {
            value = false;
            return true;
        }

        protected override bool CombineStringAndLong(string input1, long input2, out object value)
        {
            value = false;
            return true;
        }

        protected override bool CombineStringAndNull(string input1, out object value)
        {
            value = true;
            return true;
        }

        protected override bool CombineDecimalAndString(decimal input1, string input2, out object value)
        {
            value = false;
            return true;
        }

        protected override bool CombineStringAndDecimal(string input1, decimal input2, out object value)
        {
            value = false;
            return true;
        }

        protected override bool CombineDoubleAndDecimal(double input1, decimal input2, out object value)
        {
            value = input1.CompareTo((double)input2) > 0;
            return true;
        }

        protected override bool CombineLongAndDecimal(long input1, decimal input2, out object value)
        {
            value = input1.CompareTo((long)input2) > 0;
            return true;
        }

        protected override bool CombineDecimalAndDouble(decimal input1, double input2, out object value)
        {
            value = ((double)input1).CompareTo(input2) > 0;
            return true;
        }

        protected override bool CombineDecimalAndLong(decimal input1, long input2, out object value)
        {
            value = input1.CompareTo(input2) > 0;
            return true;
        }

        protected override bool CombineDecimalAndDecimal(decimal input1, decimal input2, out object value)
        {
            value = input1.CompareTo(input2) > 0;
            return true;
        }

        protected override bool CombineDecimalAndNull(decimal input1, out object value)
        {
            value = input1.CompareTo(0) > 0;
            return true;
        }

        protected override bool CombineNullAndDecimal(decimal input2, out object value)
        {
            value = 0m.CompareTo(input2) > input2;
            return true;
        }
    }
}