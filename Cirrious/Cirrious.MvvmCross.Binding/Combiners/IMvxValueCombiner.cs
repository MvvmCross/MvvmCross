using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Converters;
using Cirrious.CrossCore.Platform;

namespace Cirrious.MvvmCross.Binding.Binders
{
    public interface IMvxValueCombiner
    {
        Type SourceType(IEnumerable<IMvxSourceStep> steps, object parameter);
        void SetValue(IEnumerable<IMvxSourceStep> steps, object parameter, object value);
        bool TryGetValue(IEnumerable<IMvxSourceStep> steps, object parameter, out object value);
        IEnumerable<Type> SubStepTargetTypes(IEnumerable<IMvxSourceStep> subSteps, object parameter, Type overallTargetType);
    }

    public interface IMvxValueCombinerLookup
    {
        IMvxValueCombiner Find(string combinerName);
    }

    public interface IMvxValueCombinerRegistryFiller
        : IMvxNamedInstanceRegistryFiller<IMvxValueCombiner>
    {
    }

    public interface IMvxValueCombinerRegistry
        : IMvxNamedInstanceRegistry<IMvxValueCombiner>
    {
    }

    public class MvxValueCombinerRegistryFiller
        : MvxNamedInstanceRegistryFiller<IMvxValueCombiner>
        , IMvxValueCombinerRegistryFiller
    {
        protected override string FindName(Type type)
        {
            var name = base.FindName(type);
            name = RemoveTail(name, "ValueCombiner");
            name = RemoveTail(name, "Combiner");
            return name;
        }
    }

    public class MvxValueCombinerRegistry 
        : MvxNamedInstanceRegistry<IMvxValueCombiner>
        , IMvxValueCombinerLookup
        , IMvxValueCombinerRegistry
    {
    }

    public abstract class MvxValueCombiner
        : IMvxValueCombiner
    {
        public virtual Type SourceType(IEnumerable<IMvxSourceStep> steps, object parameter)
        {
            return typeof(object);
        }

        public virtual void SetValue(IEnumerable<IMvxSourceStep> steps, object parameter, object value)
        {
            // do nothing
        }

        public virtual bool TryGetValue(IEnumerable<IMvxSourceStep> steps, object parameter, out object value)
        {
            value = null;
            return false;
        }

        public virtual IEnumerable<Type> SubStepTargetTypes(IEnumerable<IMvxSourceStep> subSteps, object parameter, Type overallTargetType)
        {
            return subSteps.Select(x => overallTargetType);
        }
    }

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
            _combinerActions[new TypeTuple(typeof(T1), null)] = (object x, object y, out object v) => combinerAction((T1)x, out v);
            _combinerActions[new TypeTuple(null, typeof(T1))] = (object x, object y, out object v) => switchedCombinerAction((T1)y, out v);
        }

        private void AddSingle<T1, T2>(CombinerFunc<T1, T2> combinerAction)
        {
            _combinerActions[new TypeTuple(typeof(T1), typeof(T2))] = (object x, object y, out object v) => combinerAction((T1)x, (T2)y, out v);
        }

        public override bool TryGetValue(IEnumerable<IMvxSourceStep> steps, object parameter, out object value)
        {
            var resultPairs = steps.Select(step =>
            {
                object v;
                var r = step.TryGetValue(out v);
                return new ResultPair()
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
                resultPairs[0] = new ResultPair() { IsAvailable = newIsAvailable, Value = newValue };
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

    public abstract class MvxObjectAsStringPairwiseValueCombiner
        : MvxPairwiseValueCombiner
    {
        protected abstract bool CombineStringAndString(string input1, string input2, out object value);
        protected abstract bool CombineIntAndString(int input1, string input2, out object value);
        protected abstract bool CombineDoubleAndString(double input1, string input2, out object value);
        protected abstract bool CombineNullAndString(string input2, out object value);
        protected abstract bool CombineStringAndDouble(string input1, double input2, out object value);
        protected abstract bool CombineStringAndInt(string input1, int input2, out object value);
        protected abstract bool CombineStringAndNull(string input1, out object value);

        protected sealed override bool CombineObjectAndObject(object object1, object object2, out object value)
        {
            return CombineStringAndString(object1.ToString(), object2.ToString(), out value);
        }

        protected sealed override bool CombineIntAndObject(int int1, object object1, out object value)
        {
            return CombineIntAndString(int1, object1.ToString(), out value);
        }

        protected sealed override bool CombineDoubleAndObject(double double1, object object1, out object value)
        {
            return CombineDoubleAndString(double1, object1.ToString(), out value);
        }

        protected sealed override bool CombineNullAndObject(object object1, out object value)
        {
            return CombineNullAndString(object1.ToString(), out value);
        }

        protected sealed override bool CombineObjectAndDouble(object input1, double input2, out object value)
        {
            return CombineStringAndDouble(input1.ToString(), input2, out value);
        }

        protected sealed override bool CombineObjectAndInt(object input1, int input2, out object value)
        {
            return CombineStringAndInt(input1.ToString(), input2, out value);
        }


        protected sealed override bool CombineObjectAndNull(object input1, out object value)
        {
            return CombineStringAndNull(input1.ToString(), out value);
        }
    }

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

    public abstract class MvxNumericOnlyValueCombiner
        : MvxObjectAsStringPairwiseValueCombiner
    {
        protected override bool CombineStringAndDouble(string input1, double input2, out object value)
        {
            value = null;
            return false;
        }

        protected override bool CombineStringAndInt(string input1, int input2, out object value)
        {
            value = null;
            return false;
        }

        protected override bool CombineStringAndNull(string input1, out object value)
        {
            value = null;
            return false;
        }

        protected sealed override bool CombineStringAndString(string input1, string input2, out object value)
        {
            value = null;
            return false;
        }

        protected sealed override bool CombineIntAndString(int input1, string input2, out object value)
        {
            value = null;
            return false;
        }

        protected sealed override bool CombineDoubleAndString(double input1, string input2, out object value)
        {
            value = null;
            return false;
        }

        protected sealed override bool CombineNullAndString(string input2, out object value)
        {
            value = null;
            return false;
        }
    }

    public class MvxSubtractValueCombiner
        : MvxNumericOnlyValueCombiner
    {
        protected override bool CombineDoubleAndDouble(double input1, double input2, out object value)
        {
            value = input1 - input2;
            return true;
        }

        protected override bool CombineDoubleAndInt(double input1, int input2, out object value)
        {
            value = input1 - input2;
            return true;
        }

        protected override bool CombineDoubleAndNull(double input1, out object value)
        {
            value = input1;
            return true;
        }

        protected override bool CombineIntAndDouble(int input1, double input2, out object value)
        {
            value = input1 - input2;
            return true;
        }

        protected override bool CombineIntAndInt(int input1, int input2, out object value)
        {
            value = input1 - input2;
            return true;
        }

        protected override bool CombineIntAndNull(int input1, out object value)
        {
            value = input1;
            return true;
        }

        protected override bool CombineNullAndDouble(double input2, out object value)
        {
            value = -input2;
            return true;
        }

        protected override bool CombineNullAndInt(int input2, out object value)
        {
            value = -input2;
            return true;
        }

        protected override bool CombineTwoNulls(out object value)
        {
            value = null;
            return true;
        }
    }
    public class MvxMultiplyValueCombiner
        : MvxNumericOnlyValueCombiner
    {
        protected override bool CombineDoubleAndDouble(double input1, double input2, out object value)
        {
            value = input1 * input2;
            return true;
        }

        protected override bool CombineDoubleAndInt(double input1, int input2, out object value)
        {
            value = input1 * input2;
            return true;
        }

        protected override bool CombineDoubleAndNull(double input1, out object value)
        {
            value = null;
            return true;
        }

        protected override bool CombineIntAndDouble(int input1, double input2, out object value)
        {
            value = input1 * input2;
            return true;
        }

        protected override bool CombineIntAndInt(int input1, int input2, out object value)
        {
            value = input1 * input2;
            return true;
        }

        protected override bool CombineIntAndNull(int input1, out object value)
        {
            value = null;
            return true;
        }

        protected override bool CombineNullAndDouble(double input2, out object value)
        {
            value = null;
            return true;
        }

        protected override bool CombineNullAndInt(int input2, out object value)
        {
            value = null;
            return true;
        }

        protected override bool CombineTwoNulls(out object value)
        {
            value = null;
            return true;
        }
    }
    public class MvxDivideValueCombiner
        : MvxNumericOnlyValueCombiner
    {
        protected override bool CombineDoubleAndDouble(double input1, double input2, out object value)
        {
            value = input1 / input2;
            return true;
        }

        protected override bool CombineDoubleAndInt(double input1, int input2, out object value)
        {
            value = input1 / input2;
            return true;
        }

        protected override bool CombineDoubleAndNull(double input1, out object value)
        {
            // divided by zero... hmmmm
            value = 0.0;
            return true;
        }

        protected override bool CombineIntAndDouble(int input1, double input2, out object value)
        {
            value = input1 / input2;
            return true;
        }

        protected override bool CombineIntAndInt(int input1, int input2, out object value)
        {
            value = input1 / input2;
            return true;
        }

        protected override bool CombineIntAndNull(int input1, out object value)
        {
            // divided by zero... hmmmm
            value = input1 >= 0 ? double.PositiveInfinity : double.NegativeInfinity;
            return true;
        }

        protected override bool CombineNullAndDouble(double input2, out object value)
        {
            value = 0/input2;
            return true;
        }

        protected override bool CombineNullAndInt(int input2, out object value)
        {
            value = 0 / input2;
            return true;
        }

        protected override bool CombineTwoNulls(out object value)
        {
            // zero divided by zero... hmmmm
            value = null;
            return true;
        }
    }
    public class MvxModulusValueCombiner
        : MvxNumericOnlyValueCombiner
    {
        protected override bool CombineDoubleAndDouble(double input1, double input2, out object value)
        {
            value = input1 % input2;
            return true;
        }

        protected override bool CombineDoubleAndInt(double input1, int input2, out object value)
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

        protected override bool CombineIntAndDouble(int input1, double input2, out object value)
        {
            value = input1 % input2;
            return true;
        }

        protected override bool CombineIntAndInt(int input1, int input2, out object value)
        {
            value = input1 % input2;
            return true;
        }

        protected override bool CombineIntAndNull(int input1, out object value)
        {
            // divided by zero... hmmmm
            value = 0;
            return true;
        }

        protected override bool CombineNullAndDouble(double input2, out object value)
        {
            value = 0 % input2;
            return true;
        }

        protected override bool CombineNullAndInt(int input2, out object value)
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