// MvxPairwiseValueCombiner.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.Linq;
using MvvmCross.Binding.Bindings.SourceSteps;
using MvvmCross.Platform;
using MvvmCross.Platform.Converters;

namespace MvvmCross.Binding.Combiners
{
    public abstract class MvxPairwiseValueCombiner
        : MvxValueCombiner
    {
        public override void SetValue(IEnumerable<IMvxSourceStep> steps, object value)
        {
            Mvx.Trace("The Add Combiner does not support SetValue");
        }

        public override Type SourceType(IEnumerable<IMvxSourceStep> steps)
        {
            return steps.First().SourceType;
        }

        private class ResultPair
        {
            public bool IsAvailable { get; set; }
            public object Value { get; set; }
        }

        private static Type GetLookupTypeFor(object value)
        {
            if (value == null)
                return null;
            if (value is long)
                return typeof(long);
            if (value is double)
                return typeof(double);
            if (value is decimal)
                return typeof(decimal);
            return typeof(object);
        }

        public class TypeTuple
        {
            public TypeTuple(Type type1, Type type2)
            {
                Type2 = type2;
                Type1 = type1;
            }

            public Type Type1 { get; }
            public Type Type2 { get; }

            public override bool Equals(object obj)
            {
                var rhs = obj as TypeTuple;

                if (rhs == null)
                    return false;

                return rhs.Type2 == Type2
                       && rhs.Type1 == Type1;
            }

            public override int GetHashCode()
            {
                return (Type1?.GetHashCode() ?? 0) + (Type2?.GetHashCode() ?? 0);
            }
        }

        private delegate bool CombinerFunc(out object value);

        private delegate bool CombinerFunc<in T1>(T1 input1, out object value);

        private delegate bool CombinerFunc<in T1, in T2>(T1 input1, T2 input2, out object value);

        private readonly Dictionary<TypeTuple, CombinerFunc<object, object>> _combinerActions;

        protected MvxPairwiseValueCombiner()
        {
            _combinerActions = new Dictionary<TypeTuple, CombinerFunc<object, object>>();
            AddSingle<object, object>(CombineObjectAndObject);
            AddSingle<object, double>(CombineObjectAndDouble);
            AddSingle<object, long>(CombineObjectAndLong);
            AddSingle<object, decimal>(CombineObjectAndDecimal);
            AddSingle<double, object>(CombineDoubleAndObject);
            AddSingle<double, double>(CombineDoubleAndDouble);
            AddSingle<double, long>(CombineDoubleAndLong);
            AddSingle<double, decimal>(CombineDoubleAndDecimal);
            AddSingle<long, object>(CombineLongAndObject);
            AddSingle<long, double>(CombineLongAndDouble);
            AddSingle<long, long>(CombineLongAndLong);
            AddSingle<long, decimal>(CombineLongAndDecimal);
            AddSingle<decimal, object>(CombineDecimalAndObject);
            AddSingle<decimal, double>(CombineDecimalAndDouble);
            AddSingle<decimal, long>(CombineDecimalAndLong);
            AddSingle<decimal, decimal>(CombineDecimalAndDecimal);
            AddSingle<object>(CombineObjectAndNull, CombineNullAndObject);
            AddSingle<double>(CombineDoubleAndNull, CombineNullAndDouble);
            AddSingle<long>(CombineLongAndNull, CombineNullAndLong);
            AddSingle<decimal>(CombineDecimalAndNull, CombineNullAndDecimal);
            AddSingle(CombineTwoNulls);
        }

        private void AddSingle(CombinerFunc combinerAction)
        {
            _combinerActions[new TypeTuple(null, null)] = (object x, object y, out object v) => combinerAction(out v);
        }

        private void AddSingle<T1>(CombinerFunc<T1> combinerAction, CombinerFunc<T1> switchedCombinerAction)
        {
            _combinerActions[new TypeTuple(typeof(T1), null)] =
                (object x, object y, out object v) => combinerAction((T1)x, out v);
            _combinerActions[new TypeTuple(null, typeof(T1))] =
                (object x, object y, out object v) => switchedCombinerAction((T1)y, out v);
        }

        private void AddSingle<T1, T2>(CombinerFunc<T1, T2> combinerAction)
        {
            _combinerActions[new TypeTuple(typeof(T1), typeof(T2))] =
                (object x, object y, out object v) => combinerAction((T1)x, (T2)y, out v);
        }

        protected virtual object ForceToSimpleValueTypes(object input)
        {
            if (input is int)
            {
                return (long)(int)input;
            }
            if (input is short)
            {
                return (long)(short)input;
            }
            if (input is float)
            {
                return (double)(float)input;
            }

            return input;
        }

        public override bool TryGetValue(IEnumerable<IMvxSourceStep> steps, out object value)
        {
            var resultPairs = steps.Select(step => step.GetValue()).ToList();

            while (resultPairs.Count > 1)
            {
                var first = resultPairs[0];
                var second = resultPairs[1];

                if (first == MvxBindingConstant.DoNothing
                    || second == MvxBindingConstant.DoNothing)
                {
                    value = MvxBindingConstant.DoNothing;
                    return true;
                }

                if (first == MvxBindingConstant.UnsetValue
                    || second == MvxBindingConstant.UnsetValue)
                {
                    value = MvxBindingConstant.UnsetValue;
                    return true;
                }

                first = ForceToSimpleValueTypes(first);
                second = ForceToSimpleValueTypes(second);

                var firstType = GetLookupTypeFor(first);
                var secondType = GetLookupTypeFor(second);

                CombinerFunc<object, object> combinerFunc;
                if (!_combinerActions.TryGetValue(new TypeTuple(firstType, secondType), out combinerFunc))
                {
                    Mvx.Error("Unknown type pair in Pairwise combiner {0}, {1}", firstType, secondType);
                    value = MvxBindingConstant.UnsetValue;
                    return true;
                }

                object newValue;
                var newIsAvailable = combinerFunc(first, second, out newValue);
                if (!newIsAvailable)
                {
                    value = MvxBindingConstant.UnsetValue;
                    return true;
                }

                resultPairs.RemoveAt(0);
                resultPairs[0] = newValue;
            }

            value = resultPairs[0];
            return true;
        }

        protected abstract bool CombineObjectAndDouble(object input1, double input2, out object value);

        protected abstract bool CombineObjectAndLong(object input1, long input2, out object value);

        protected abstract bool CombineObjectAndObject(object object1, object object2, out object value);

        protected abstract bool CombineObjectAndDecimal(object input1, decimal input2, out object value);

        protected abstract bool CombineObjectAndNull(object input1, out object value);

        protected abstract bool CombineDoubleAndObject(double input1, object input2, out object value);

        protected abstract bool CombineDoubleAndDouble(double input1, double input2, out object value);

        protected abstract bool CombineDoubleAndLong(double input1, long input2, out object value);

        protected abstract bool CombineDoubleAndDecimal(double input1, decimal input2, out object value);

        protected abstract bool CombineDoubleAndNull(double input1, out object value);

        protected abstract bool CombineLongAndObject(long input1, object input2, out object value);

        protected abstract bool CombineLongAndDouble(long input1, double input2, out object value);

        protected abstract bool CombineLongAndLong(long input1, long input2, out object value);

        protected abstract bool CombineLongAndDecimal(long input1, decimal input2, out object value);

        protected abstract bool CombineLongAndNull(long input1, out object value);

        protected abstract bool CombineDecimalAndDouble(decimal input1, double input2, out object value);

        protected abstract bool CombineDecimalAndLong(decimal input1, long input2, out object value);

        protected abstract bool CombineDecimalAndObject(decimal object1, object object2, out object value);

        protected abstract bool CombineDecimalAndDecimal(decimal input1, decimal input2, out object value);

        protected abstract bool CombineDecimalAndNull(decimal input1, out object value);

        protected abstract bool CombineNullAndObject(object object1, out object value);

        protected abstract bool CombineNullAndDouble(double input2, out object value);

        protected abstract bool CombineNullAndLong(long input2, out object value);

        protected abstract bool CombineNullAndDecimal(decimal input2, out object value);

        protected abstract bool CombineTwoNulls(out object value);
    }
}