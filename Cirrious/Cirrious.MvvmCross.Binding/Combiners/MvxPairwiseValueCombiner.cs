// MvxPairwiseValueCombiner.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.Linq;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Binding.Bindings.SourceSteps;

namespace Cirrious.MvvmCross.Binding.Combiners
{
    public abstract class MvxPairwiseValueCombiner
        : MvxValueCombiner
    {
        public override void SetValue(IEnumerable<IMvxSourceStep> steps, object parameter, object value)
        {
            Mvx.Trace("The Add Combiner does not support SetValue");
        }

        public override Type SourceType(IEnumerable<IMvxSourceStep> steps, object parameter)
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
            if (value is int)
                return typeof (int);
            if (value is double)
                return typeof (double);
            return typeof (object);
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
            AddSingle<int, object>(CombineIntAndObject);
            AddSingle<object, int>(CombineObjectAndInt);
            AddSingle<int, double>(CombineIntAndDouble);
            AddSingle<double, int>(CombineDoubleAndInt);
            AddSingle<int, int>(CombineIntAndInt);
            AddSingle<object>(CombineObjectAndNull, CombineNullAndObject);
            AddSingle<double>(CombineDoubleAndNull, CombineNullAndDouble);
            AddSingle<int>(CombineIntAndNull, CombineNullAndInt);
            AddSingle(CombineTwoNulls);
        }

        private void AddSingle(CombinerFunc combinerAction)
        {
            _combinerActions[new TypeTuple(null, null)] = (object x, object y, out object v) => combinerAction(out v);
        }

        private void AddSingle<T1>(CombinerFunc<T1> combinerAction, CombinerFunc<T1> switchedCombinerAction)
        {
            _combinerActions[new TypeTuple(typeof (T1), null)] =
                (object x, object y, out object v) => combinerAction((T1) x, out v);
            _combinerActions[new TypeTuple(null, typeof (T1))] =
                (object x, object y, out object v) => switchedCombinerAction((T1) y, out v);
        }

        private void AddSingle<T1, T2>(CombinerFunc<T1, T2> combinerAction)
        {
            _combinerActions[new TypeTuple(typeof (T1), typeof (T2))] =
                (object x, object y, out object v) => combinerAction((T1) x, (T2) y, out v);
        }

        public override bool TryGetValue(IEnumerable<IMvxSourceStep> steps, object parameter, out object value)
        {
            var resultPairs = steps.Select(step =>
                {
                    object v;
                    var r = step.TryGetValue(out v);
                    return new ResultPair
                        {
                            IsAvailable = r,
                            Value = v
                        };
                }).ToList();

            while (resultPairs.Count > 1)
            {
                var first = resultPairs[0];
                var second = resultPairs[1];

                if (!first.IsAvailable
                    || !second.IsAvailable)
                {
                    value = null;
                    return false;
                }

                var firstType = GetLookupTypeFor(first.Value);
                var secondType = GetLookupTypeFor(second.Value);

                CombinerFunc<object, object> combinerFunc;
                if (!_combinerActions.TryGetValue(new TypeTuple(firstType, secondType), out combinerFunc))
                {
                    Mvx.Error("Unknown type pair in Pairwise combiner {0}, {1}", firstType, secondType);
                    value = null;
                    return false;
                }

                object newValue;
                var newIsAvailable = combinerFunc(first.Value, second.Value, out newValue);
                resultPairs.RemoveAt(0);
                resultPairs[0] = new ResultPair {IsAvailable = newIsAvailable, Value = newValue};
            }
            value = resultPairs[0].Value;
            return true;
        }

        protected abstract bool CombineObjectAndDouble(object input1, double input2, out object value);
        protected abstract bool CombineObjectAndInt(object input1, int input2, out object value);
        protected abstract bool CombineObjectAndObject(object object1, object object2, out object value);
        protected abstract bool CombineObjectAndNull(object input1, out object value);

        protected abstract bool CombineDoubleAndObject(double double1, object object1, out object value);
        protected abstract bool CombineDoubleAndDouble(double input1, double input2, out object value);
        protected abstract bool CombineDoubleAndInt(double input1, int input2, out object value);
        protected abstract bool CombineDoubleAndNull(double input1, out object value);

        protected abstract bool CombineIntAndObject(int int1, object object1, out object value);
        protected abstract bool CombineIntAndDouble(int input1, double input2, out object value);
        protected abstract bool CombineIntAndInt(int input1, int input2, out object value);
        protected abstract bool CombineIntAndNull(int input1, out object value);

        protected abstract bool CombineNullAndObject(object object1, out object value);
        protected abstract bool CombineNullAndDouble(double input2, out object value);
        protected abstract bool CombineNullAndInt(int input2, out object value);
        protected abstract bool CombineTwoNulls(out object value);
    }
}