// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

#nullable enable
using System.Globalization;
using System.Reflection;
using Microsoft.Extensions.Logging;
using MvvmCross.Logging;

namespace MvvmCross.Core
{
    public class MvxStringToTypeParser
        : IMvxStringToTypeParser, IMvxFillableStringToTypeParser
    {
        public interface IParser
        {
            object? ReadValue(string input, string fieldOrParameterName);
        }

        public interface IExtraParser
        {
            bool Parses(Type t);

            object? ReadValue(Type t, string input, string fieldOrParameterName);
        }

        private sealed class EnumParser : IExtraParser
        {
            public bool Parses(Type t)
            {
                return t.GetTypeInfo().IsEnum;
            }

            public object? ReadValue(Type t, string input, string fieldOrParameterName)
            {
                object? enumValue = null;
                try
                {
                    enumValue = Enum.Parse(t, input, true);
                }
                catch (Exception)
                {
                    MvxLogHost.Default?.Log(LogLevel.Error,
                        "Failed to parse enum parameter {FieldOrParameterName} from string {Input}",
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
                        MvxLogHost.Default?.Log(LogLevel.Error,
                            "Failed to create default enum value for {FieldOrParameterName} - will return null",
                            fieldOrParameterName);
                    }
                }
                return enumValue;
            }
        }

        private sealed class StringParser : IParser
        {
            public object? ReadValue(string input, string fieldOrParameterName)
            {
                return input;
            }
        }

        public abstract class ValueParser : IParser
        {
            protected abstract bool TryParse(string input, out object result);

            public object? ReadValue(string input, string fieldOrParameterName)
            {
                if (!TryParse(input, out var result))
                {
                    MvxLogHost.Default?.Log(LogLevel.Error,
                        "Failed to parse {Type} parameter {FieldOrParameterName} from string {Input}",
                        GetType().Name, fieldOrParameterName, input);
                }
                return result;
            }
        }

        private sealed class NumberParser<T> : ValueParser where T : struct
        {
            // See also https://stackoverflow.com/questions/2961656/generic-tryparse/6553694
            // Piers Myers(https://stackoverflow.com/users/275751/piers-myers)'s question and
            // Charlie Brown(https://stackoverflow.com/users/825578/charlie-brown)'s answer
            public delegate bool TryParseHandler(string input, NumberStyles style, IFormatProvider provider, out T result);

            private readonly TryParseHandler _tryParseHandler;

            public NumberParser(TryParseHandler handler) => _tryParseHandler = handler;

            protected override bool TryParse(string input, out object result)
            {
                var toReturn = _tryParseHandler(input, NumberStyles.Any, CultureInfo.InvariantCulture, out T value);
                result = value;
                return toReturn;
            }
        }

        private sealed class CharParser : ValueParser
        {
            protected override bool TryParse(string input, out object result)
            {
                var toReturn = char.TryParse(input, out var value);
                result = value;
                return toReturn;
            }
        }

        private sealed class BoolParser : ValueParser
        {
            protected override bool TryParse(string input, out object result)
            {
                var toReturn = bool.TryParse(input, out var value);
                result = value;
                return toReturn;
            }
        }

        private sealed class GuidParser : ValueParser
        {
            protected override bool TryParse(string input, out object result)
            {
                var toReturn = Guid.TryParse(input, out var value);
                result = value;
                return toReturn;
            }
        }

        private sealed class DateTimeParser : ValueParser
        {
            protected override bool TryParse(string input, out object result)
            {
                var toReturn = DateTime.TryParse(input, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind,
                    out var value);
                result = value;
                return toReturn;
            }
        }

        public IDictionary<Type, IParser> TypeParsers { get; }

        public IList<IExtraParser> ExtraParsers { get; }

        public MvxStringToTypeParser()
        {
            TypeParsers = new Dictionary<Type, IParser>
            {
                { typeof(char), new CharParser() },
                { typeof(string), new StringParser() },
                { typeof(sbyte), new NumberParser<sbyte>(sbyte.TryParse) },
                { typeof(short), new NumberParser<short>(short.TryParse) },
                { typeof(int), new NumberParser<int>(int.TryParse) },
                { typeof(long), new NumberParser<long>(long.TryParse) },
                { typeof(byte), new NumberParser<byte>(byte.TryParse) },
                { typeof(ushort), new NumberParser<ushort>(ushort.TryParse) },
                { typeof(uint), new NumberParser<uint>(uint.TryParse) },
                { typeof(ulong), new NumberParser<ulong>(ulong.TryParse) },
                { typeof(double), new NumberParser<double>(double.TryParse) },
                { typeof(float), new NumberParser<float>(float.TryParse) },
                { typeof(decimal), new NumberParser<decimal>(decimal.TryParse) },
                { typeof(bool), new BoolParser() },
                { typeof(Guid), new GuidParser() },
                { typeof(DateTime), new DateTimeParser() }
            };

            ExtraParsers = new List<IExtraParser>
            {
                new EnumParser()
            };
        }

        public bool TypeSupported(Type targetType)
        {
            if (TypeParsers.ContainsKey(targetType))
                return true;

            return ExtraParsers.Any(x => x.Parses(targetType));
        }

        public object? ReadValue(string rawValue, Type targetType, string fieldOrParameterName)
        {
            if (TypeParsers.TryGetValue(targetType, out var parser))
            {
                return parser.ReadValue(rawValue, fieldOrParameterName);
            }

            var extra = ExtraParsers.FirstOrDefault(x => x.Parses(targetType));
            if (extra != null)
            {
                return extra.ReadValue(targetType, rawValue, fieldOrParameterName);
            }

            MvxLogHost.Default?.Log(LogLevel.Error,
                "Parameter {ParameterName} is invalid targetType {TypeName}",
                fieldOrParameterName, targetType.Name);
            return null;
        }
    }
}
