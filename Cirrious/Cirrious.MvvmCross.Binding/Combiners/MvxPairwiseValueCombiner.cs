// MvxPairwiseValueCombiner.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore;
using Cirrious.CrossCore.Converters;
using Cirrious.MvvmCross.Binding.Bindings.SourceSteps;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cirrious.MvvmCross.Binding.Combiners
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

        private Type GetLookupTypeFor(object value)
        {
            if (value == null)
                return null;
            if (value is long)
                return typeof(long);
            if (value is double)
                return typeof(double);
            return typeof(object);
        }

        public class TypeTuple
        {
            public TypeTuple(Type type1, Type type2)
            {
                Type2 = type2;
                Type1 = type1;
            }

            public Type Type1 { get; private set; }
            public Type Type2 { get; private set; }

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
                return (Type1 == null ? 0 : Type1.GetHashCode())
                       + (Type2 == null ? 0 : Type2.GetHashCode());
            }
        }

        private delegate bool CombinerFunc(out object value);

        private delegate bool CombinerFunc<T1>(T1 input1, out object value);

        private delegate bool CombinerFunc<T1, T2>(T1 input1, T2 input2, out object value);

        private readonly Dictionary<TypeTuple, CombinerFunc<object, object>> _combinerActions;

        protected MvxPairwiseValueCombiner()
        {
            _combinerActions = new Dictionary<TypeTuple, CombinerFunc<object, object>>();
            AddSingle<object, object>(CombineObjectAndObject);
            AddSingle<double, object>(CombineDoubleAndObject);
            AddSingle<object, double>(CombineObjectAndDouble);
            AddSingle<double, double>(CombineDoubleAndDouble);
            AddSingle<long, object>(CombineLongAndObject);
            AddSingle<object, long>(CombineObjectAndLong);
            AddSingle<long, double>(CombineLongAndDouble);
            AddSingle<double, long>(CombineDoubleAndLong);
            AddSingle<long, long>(CombineLongAndLong);
            AddSingle<object>(CombineObjectAndNull, CombineNullAndObject);
            AddSingle<double>(CombineDoubleAndNull, CombineNullAndDouble);
            AddSingle<long>(CombineLongAndNull, CombineNullAndLong);
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

        protected abstract bool CombineObjectAndNull(object input1, out object value);

        protected abstract bool CombineDoubleAndObject(double input1, object input2, out object value);

        protected abstract bool CombineDoubleAndDouble(double input1, double input2, out object value);

        protected abstract bool CombineDoubleAndLong(double input1, long input2, out object value);

        protected abstract bool CombineDoubleAndNull(double input1, out object value);

        protected abstract bool CombineLongAndObject(long input1, object input2, out object value);

        protected abstract bool CombineLongAndDouble(long input1, double input2, out object value);

        protected abstract bool CombineLongAndLong(long input1, long input2, out object value);

        protected abstract bool CombineLongAndNull(long input1, out object value);

        protected abstract bool CombineNullAndObject(object object1, out object value);

        protected abstract bool CombineNullAndDouble(double input2, out object value);

        protected abstract bool CombineNullAndLong(long input2, out object value);

        protected abstract bool CombineTwoNulls(out object value);
    }
}