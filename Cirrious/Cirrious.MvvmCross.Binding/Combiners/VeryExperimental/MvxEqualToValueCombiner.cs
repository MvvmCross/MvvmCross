namespace Cirrious.MvvmCross.Binding.Combiners
{
    public class MvxEqualToValueCombiner
        : MvxPairwiseValueCombiner
    {
        protected override bool CombineDoubleAndDouble(double input1, double input2, out object value)
        {
            value = input1 == input2;
            return true;
        }

        protected override bool CombineDoubleAndLong(double input1, long input2, out object value)
        {
            value = input1 == input2;
            return true;
        }

        protected override bool CombineDoubleAndNull(double input1, out object value)
        {
            value = input1 == 0;
            return true;
        }

        protected override bool CombineDoubleAndObject(double double1, object object1, out object value)
        {
            value = false;
            return true;
        }

        protected override bool CombineLongAndDouble(long input1, double input2, out object value)
        {
            value = input1 == input2;
            return true;
        }

        protected override bool CombineLongAndLong(long input1, long input2, out object value)
        {
            value = input1 == input2;
            return true;
        }

        protected override bool CombineLongAndNull(long input1, out object value)
        {
            value = input1 == 0;
            return true;
        }

        protected override bool CombineLongAndObject(long int1, object object1, out object value)
        {
            value = false;
            return true;
        }

        protected override bool CombineNullAndDouble(double input2, out object value)
        {
            value = 0 == input2;
            return true;
        }

        protected override bool CombineNullAndLong(long input2, out object value)
        {
            value = 0 == input2;
            return true;
        }

        protected override bool CombineNullAndObject(object object1, out object value)
        {
            value = object1.Equals(null);
            return true;
        }

        protected override bool CombineObjectAndDouble(object input1, double input2, out object value)
        {
            value = false;
            return true;
        }

        protected override bool CombineObjectAndLong(object input1, long input2, out object value)
        {
            value = false;
            return true;
        }

        protected override bool CombineObjectAndNull(object input1, out object value)
        {
            value = input1.Equals(null);
            return true;
        }

        protected override bool CombineObjectAndObject(object object1, object object2, out object value)
        {
            value = object1.Equals(object2);
            return true;
        }

        protected override bool CombineTwoNulls(out object value)
        {
            value = true;
            return true;
        }
    }
}