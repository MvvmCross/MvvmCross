// IMvxValueCombiner.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using Cirrious.MvvmCross.Binding.Bindings.SourceSteps;

namespace Cirrious.MvvmCross.Binding.Combiners
{
    public interface IMvxValueCombiner
    {
        Type SourceType(IEnumerable<IMvxSourceStep> steps, object parameter);
        void SetValue(IEnumerable<IMvxSourceStep> steps, object parameter, object value);
        bool TryGetValue(IEnumerable<IMvxSourceStep> steps, object parameter, out object value);

        IEnumerable<Type> SubStepTargetTypes(IEnumerable<IMvxSourceStep> subSteps, object parameter,
                                             Type overallTargetType);
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

        protected override sealed bool CombineObjectAndInt(object input1, int input2, out object value)
        {
            value = false;
            return true;
        }

        protected override sealed bool CombineDoubleAndObject(double double1, object object1, out object value)
        {
            value = false;
            return true;
        }

        protected override sealed bool CombineDoubleAndInt(double input1, int input2, out object value)
        {
            value = false;
            return true;
        }

        protected override sealed bool CombineDoubleAndNull(double input1, out object value)
        {
            value = false;
            return true;
        }

        protected override sealed bool CombineIntAndObject(int int1, object object1, out object value)
        {
            value = false;
            return true;
        }

        protected override sealed bool CombineIntAndDouble(int input1, double input2, out object value)
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

        protected override bool CombineIntAndInt(int input1, int input2, out object value)
        {
            value = input1 == input2;
            return true;
        }

        protected override bool CombineTwoNulls(out object value)
        {
            value = true;
            return true;
        }

        protected override sealed bool CombineIntAndNull(int input1, out object value)
        {
            value = false;
            return true;
        }

        protected override sealed bool CombineNullAndDouble(double input2, out object value)
        {
            value = false;
            return true;
        }

        protected override sealed bool CombineNullAndInt(int input2, out object value)
        {
            value = false;
            return true;
        }
    }
    */
}