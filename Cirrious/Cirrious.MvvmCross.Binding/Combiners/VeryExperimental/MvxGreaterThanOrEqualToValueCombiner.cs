namespace Cirrious.MvvmCross.Binding.Combiners
{
    public class MvxGreaterThanOrEqualToValueCombiner : MvxObjectAsStringPairwiseValueCombiner
    {
        protected override bool CombineDoubleAndDouble(double input1, double input2, out object value)
        {
            value = input1.CompareTo(input2) >= 0;
            return true;
        }

        protected override bool CombineDoubleAndInt(double input1, int input2, out object value)
        {
            value = input1.CompareTo(input2) >= 0;
            return true;
        }

        protected override bool CombineDoubleAndNull(double input1, out object value)
        {
            value = input1.CompareTo(0) >= 0;
            return true;
        }

        protected override bool CombineIntAndDouble(int input1, double input2, out object value)
        {
            value = ((double)input1).CompareTo(input2) >= 0;
            return true;
        }

        protected override bool CombineIntAndInt(int input1, int input2, out object value)
        {
            value = input1.CompareTo(input2) >= 0;
            return true;
        }

        protected override bool CombineIntAndNull(int input1, out object value)
        {
            value = input1.CompareTo(0) >= 0;
            return true;
        }

        protected override bool CombineNullAndDouble(double input2, out object value)
        {
            value = (0.0).CompareTo(input2) >= 0;
            return true;
        }

        protected override bool CombineNullAndInt(int input2, out object value)
        {
            value = (0).CompareTo(input2) >= 0;
            return true;
        }

        protected override bool CombineTwoNulls(out object value)
        {
            value = true;
            return true;
        }

        protected override bool CombineStringAndString(string input1, string input2, out object value)
        {
            value = input1.CompareTo(input2) >= 0;
            return true;
        }

        protected override bool CombineIntAndString(int input1, string input2, out object value)
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

        protected override bool CombineStringAndInt(string input1, int input2, out object value)
        {
            value = false;
            return true;
        }

        protected override bool CombineStringAndNull(string input1, out object value)
        {
            value = true;
            return true;
        }
    }
}