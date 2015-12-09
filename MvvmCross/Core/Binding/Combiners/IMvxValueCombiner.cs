// IMvxValueCombiner.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Combiners
{
    using System;
    using System.Collections.Generic;

    using MvvmCross.Binding.Bindings.SourceSteps;

    public interface IMvxValueCombiner
    {
        Type SourceType(IEnumerable<IMvxSourceStep> steps);

        void SetValue(IEnumerable<IMvxSourceStep> steps, object value);

        bool TryGetValue(IEnumerable<IMvxSourceStep> steps, out object value);

        IEnumerable<Type> SubStepTargetTypes(IEnumerable<IMvxSourceStep> subSteps, Type overallTargetType);
    }

    /*
     * still to go are...
     *    EqualTo/NotEqualTo
     *    GreaterThan/LessThan/GreaterThanOrEqualTo/LessThanOrEqualTo/etc
     *    And/Or
     *
     * I would also like to support Enum's somehow...
     *

    public abstract class MvxPairwiseComparisonValueCombiner
        : MvxPairwiseValueCombiner
    {
        protected override bool CombineObjectAndNull(object input1, out object value)
        {
            value = false;
            return true;
        }

        protected override sealed bool CombineObjectAndDouble(object input1, double input2, out object value)
        {
            value = false;
            return true;
        }

        protected override sealed bool CombineObjectAndLong(object input1, int input2, out object value)
        {
            value = false;
            return true;
        }

        protected override sealed bool CombineDoubleAndObject(double double1, object object1, out object value)
        {
            value = false;
            return true;
        }

        protected override sealed bool CombineDoubleAndLong(double input1, int input2, out object value)
        {
            value = false;
            return true;
        }

        protected override sealed bool CombineDoubleAndNull(double input1, out object value)
        {
            value = false;
            return true;
        }

        protected override sealed bool CombineLongAndObject(int int1, object object1, out object value)
        {
            value = false;
            return true;
        }

        protected override sealed bool CombineLongAndDouble(int input1, double input2, out object value)
        {
            value = false;
            return true;
        }

        protected override sealed bool CombineNullAndObject(object object1, out object value)
        {
            value = false;
            return true;
        }
    }
    public class MvxEqualToValueCombiner
        : MvxPairwiseComparisonValueCombiner
    {
        protected override bool CombineObjectAndObject(object object1, object object2, out object value)
        {
            value = object1.Equals(object2);
            return true;
        }

        protected override bool CombineDoubleAndDouble(double input1, double input2, out object value)
        {
            value = input1 == input2;
            return true;
        }

        protected override bool CombineLongAndLong(int input1, int input2, out object value)
        {
            value = input1 == input2;
            return true;
        }

        protected override bool CombineTwoNulls(out object value)
        {
            value = true;
            return true;
        }

        protected override sealed bool CombineLongAndNull(int input1, out object value)
        {
            value = false;
            return true;
        }

        protected override sealed bool CombineNullAndDouble(double input2, out object value)
        {
            value = false;
            return true;
        }

        protected override sealed bool CombineNullAndLong(int input2, out object value)
        {
            value = false;
            return true;
        }
    }
    */
}