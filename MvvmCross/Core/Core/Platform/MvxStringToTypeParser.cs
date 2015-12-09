// MvxStringToTypeParser.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Core.Platform
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using MvvmCross.Platform.Platform;

    public class MvxStringToTypeParser
        : IMvxStringToTypeParser
          , IMvxFillableStringToTypeParser
    {
        public interface IParser
        {
            object ReadValue(string input, string fieldOrParameterName);
        }

        public interface IExtraParser
        {
            bool Parses(Type t);

            object ReadValue(Type t, string input, string fieldOrParameterName);
        }

        public class EnumParser : IExtraParser
        {
            public bool Parses(Type t)
            {
                return t.GetTypeInfo().IsEnum;
            }

            public object ReadValue(Type t, string input, string fieldOrParameterName)
            {
                object enumValue = null;
                try
                {
                    enumValue = Enum.Parse(t, input, true);
                }
                catch (Exception)
                {
                    MvxTrace.Error("Failed to parse enum parameter {0} from string {1}",
                                   fieldOrParameterName,
                                   input);
                }
                if (enumValue == null)
                {
                    try
                    {
                        // we set enumValue to 0 here - just have to hope that's the default
                        enumValue = Enum.ToObject(t, 0);
                    }
                    catch (Exception)
                    {
                        MvxTrace.Error("Failed to create default enum value for {0} - will return null",
                                       fieldOrParameterName);
                    }
                }
                return enumValue;
            }
        }

        public class StringParser : IParser
        {
            public object ReadValue(string input, string fieldOrParameterName)
            {
                return input;
            }
        }

        public abstract class ValueParser : IParser
        {
            protected abstract bool TryParse(string input, out object result);

            public object ReadValue(string input, string fieldOrParameterName)
            {
                object result;
                if (!this.TryParse(input, out result))
                {
                    MvxTrace.Error("Failed to parse {0} parameter {1} from string {2}",
                                   this.GetType().Name, fieldOrParameterName, input);
                }
                return result;
            }
        }

        public class BoolParser : ValueParser
        {
            protected override bool TryParse(string input, out object result)
            {
                bool value;
                var toReturn = bool.TryParse(input, out value);
                result = value;
                return toReturn;
            }
        }

        public class ShortParser : ValueParser
        {
            protected override bool TryParse(string input, out object result)
            {
                short value;
                var toReturn = short.TryParse(input, out value);
                result = value;
                return toReturn;
            }
        }

        public class IntParser : ValueParser
        {
            protected override bool TryParse(string input, out object result)
            {
                int value;
                var toReturn = int.TryParse(input, out value);
                result = value;
                return toReturn;
            }
        }

        public class LongParser : ValueParser
        {
            protected override bool TryParse(string input, out object result)
            {
                long value;
                var toReturn = long.TryParse(input, out value);
                result = value;
                return toReturn;
            }
        }

        public class UshortParser : ValueParser
        {
            protected override bool TryParse(string input, out object result)
            {
                ushort value;
                var toReturn = ushort.TryParse(input, out value);
                result = value;
                return toReturn;
            }
        }

        public class UintParser : ValueParser
        {
            protected override bool TryParse(string input, out object result)
            {
                uint value;
                var toReturn = uint.TryParse(input, out value);
                result = value;
                return toReturn;
            }
        }

        public class UlongParser : ValueParser
        {
            protected override bool TryParse(string input, out object result)
            {
                ulong value;
                var toReturn = ulong.TryParse(input, out value);
                result = value;
                return toReturn;
            }
        }

        public class FloatParser : ValueParser
        {
            protected override bool TryParse(string input, out object result)
            {
                float value;
                var toReturn = float.TryParse(input, out value);
                result = value;
                return toReturn;
            }
        }

        public class DoubleParser : ValueParser
        {
            protected override bool TryParse(string input, out object result)
            {
                double value;
                var toReturn = double.TryParse(input, out value);
                result = value;
                return toReturn;
            }
        }

#if !UNITY3D

        public class GuidParser : ValueParser
        {
            protected override bool TryParse(string input, out object result)
            {
                Guid value;
                var toReturn = Guid.TryParse(input, out value);
                result = value;
                return toReturn;
            }
        }

#else
        // UNITY3D does not support Guid.TryParse
        // See https://github.com/slodge/MvvmCross/issues/215
        public class GuidParser : ValueParser
        {
            protected override bool TryParse(string input, out object result)
            {
                try
                {
                    result = new Guid(input);
                    return true;
                }
                catch (Exception)
                {
                    result = null;
                    return false;
                }
            }
        }
#endif

        public class DateTimeParser : ValueParser
        {
            protected override bool TryParse(string input, out object result)
            {
                DateTime value;
                var toReturn = DateTime.TryParse(input, out value);
                result = value;
                return toReturn;
            }
        }

        public IDictionary<Type, IParser> TypeParsers { get; private set; }
        public IList<IExtraParser> ExtraParsers { get; private set; }

        public MvxStringToTypeParser()
        {
            this.TypeParsers = new Dictionary<Type, IParser>
                {
                    {typeof (string), new StringParser()},
                    {typeof (short), new ShortParser()},
                    {typeof (int), new IntParser()},
                    {typeof (long), new LongParser()},
                    {typeof (ushort), new UshortParser()},
                    {typeof (uint), new UintParser()},
                    {typeof (ulong), new UlongParser()},
                    {typeof (double), new DoubleParser()},
                    {typeof (float), new FloatParser()},
                    {typeof (bool), new BoolParser()},
                    {typeof (Guid), new GuidParser()},
                    {typeof (DateTime), new DateTimeParser()},
                };
            this.ExtraParsers = new List<IExtraParser>
                {
                    new EnumParser()
                };
        }

        public bool TypeSupported(Type targetType)
        {
            if (this.TypeParsers.ContainsKey(targetType))
                return true;

            return this.ExtraParsers.Any(x => x.Parses(targetType));
        }

        public object ReadValue(string rawValue, Type targetType, string fieldOrParameterName)
        {
            IParser parser;
            if (this.TypeParsers.TryGetValue(targetType, out parser))
            {
                return parser.ReadValue(rawValue, fieldOrParameterName);
            }

            var extra = this.ExtraParsers.FirstOrDefault(x => x.Parses(targetType));
            if (extra != null)
            {
                return extra.ReadValue(targetType, rawValue, fieldOrParameterName);
            }

            MvxTrace.Error("Parameter {0} is invalid targetType {1}", fieldOrParameterName,
                           targetType.Name);
            return null;
        }
    }
}