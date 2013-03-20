// MvxSwissBindingTest.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Binding.Parse.Binding;
using Cirrious.MvvmCross.Binding.Parse.Binding.Swiss;
using Cirrious.MvvmCross.Binding.Test.Parse.Binding.Lang;
using Cirrious.MvvmCross.Test.Core;
using NUnit.Framework;

namespace Cirrious.MvvmCross.Binding.Test.Parse.Binding.Swiss
{
    [TestFixture]
    public class MvxSwissBindingTest
        : MvxBindingTest
    {
        [Test]
        public void TestSimpleBinding()
        {
            foreach (var parameterSet in GenerateAllTestParameters())
            {
                PerformParseTest(parameterSet);
            }
        }

        [Test]
        public void TestTupleBinding()
        {
            foreach (var parameterSet1 in GenerateSampledTestParameters(101, 20))
                foreach (var parameterSet2 in GenerateSampledTestParameters(23, 20))
                {
                    PerformParseTest(parameterSet1, parameterSet2);
                }
        }

        [Test]
        public void TestLongTupleBinding()
        {
            foreach (var parameterSet1 in GenerateSampledTestParameters(79, 5))
                foreach (var parameterSet2 in GenerateSampledTestParameters(23, 5))
                    foreach (var parameterSet3 in GenerateSampledTestParameters(111, 5))
                        foreach (var parameterSet4 in GenerateSampledTestParameters(103, 5))
                            foreach (var parameterSet5 in GenerateSampledTestParameters(71, 5))
                            {
                                PerformParseTest(parameterSet1, parameterSet2, parameterSet3, parameterSet4,
                                                 parameterSet5);
                            }
        }

        private readonly List<MvxBindingMode> _bindingModes = new List<MvxBindingMode>
            {
                MvxBindingMode.Default,
                MvxBindingMode.OneWay,
                MvxBindingMode.OneWayToSource,
                MvxBindingMode.TwoWay,
                MvxBindingMode.OneTime,
            };

        private readonly List<string> _targetNames = new List<string>
            {
                "TargetName",
                "_",
                "T",
                "_s",
                "s",
            };

        private readonly List<string> _sourcePaths = new List<string>
            {
                "Source",
                ".",
                "",
                "Life.Foo.Bar",
                "Source['Life']",
                "Source[\"Life\"]",
                "Source[0]",
                "Source[1]",
                "Source[100]",
                "Life.Foo[1].Bar[2]",
                "Life[1].Foo[2].Bar",
            };

        private readonly List<string> _converters = new List<string>
            {
                null,
                "IntConvert",
                "TheName",
            };

        private readonly Dictionary<string, object> _values = new Dictionary<string, object>
            {
                {string.Empty, null},
                {"'One'", "One"},
                {"true", true},
                {"123", 123L},
                {"1.23", 1.23},
            };


        private IEnumerable<PerformSimpleTestParams> GenerateSampledTestParameters(int everyN, int maxToReturn)
        {
            var count = 0;
            var stillToReturn = maxToReturn;
            foreach (var testParameters in GenerateAllTestParameters())
            {
                count++;
                if (count % everyN != 1)
                    continue;

                yield return testParameters;
                stillToReturn--;
                if (stillToReturn == 0)
                    break;
            }
        }

        private IEnumerable<PerformSimpleTestParams> GenerateAllTestParameters()
        {
            foreach (var useInlinePath in new[] { true, false })
                foreach (var testBindingMode in new[] { true, false })
                    foreach (var bindingMode in _bindingModes)
                        foreach (var targetName in _targetNames)
                            foreach (var sourcePath in _sourcePaths)
                                foreach (var converter in _converters)
                                    foreach (var converterParameterValue in _values)
                                        foreach (var fallbackValue in _values)
                                            yield return new PerformSimpleTestParams(
                                                sourcePath,
                                                targetName,
                                                useInlinePath,
                                                bindingMode,
                                                testBindingMode,
                                                converter,
                                                converterParameterValue,
                                                fallbackValue
                                                );
        }

        protected string CreateText(PerformSimpleTestParams testParams)
        {
            var optionalParameters = BuildOptionalParameters(testParams);
            var text = string.Format("{0} {1}",
                                     testParams.Target,
                                     optionalParameters
                );
            return text;
        }

        private string BuildOptionalParameters(PerformSimpleTestParams testParams)
        {
            var toReturn = new StringBuilder();
            bool firstOptionAdded = false;

            if (!string.IsNullOrEmpty(testParams.Source))
            {
                if (firstOptionAdded)
                    toReturn.Append(@",");
                firstOptionAdded = true;
                if (testParams.UseInlinePath)
                {
                    toReturn.Append(testParams.Source);
                }
                else
                {
                    toReturn.AppendFormat("Path={0}", testParams.Source);
                }
            }

            if (testParams.Converter != null)
            {
                if (firstOptionAdded)
                    toReturn.Append(@",");
                firstOptionAdded = true;
                toReturn.AppendFormat("Converter={0}", testParams.Converter);
            }

            if (testParams.TestBindingMode)
            {
                if (firstOptionAdded)
                    toReturn.Append(@",");
                firstOptionAdded = true;
                toReturn.AppendFormat("Mode={0}", testParams.BindingMode);
            }

            if (testParams.ConverterParameterValue.Key != string.Empty)
            {
                if (firstOptionAdded)
                    toReturn.Append(@",");
                firstOptionAdded = true;
                toReturn.AppendFormat("ConverterParameter={0}", testParams.ConverterParameterValue.Key);
            }

            if (testParams.FallbackValue.Key != string.Empty)
            {
                if (firstOptionAdded)
                    toReturn.Append(@",");
                firstOptionAdded = true;
                toReturn.AppendFormat("FallbackValue={0}", testParams.FallbackValue.Key);
            }

            return toReturn.ToString();
        }

        public class PerformSimpleTestParams
        {
            private readonly string _source;
            private readonly string _target;
            private readonly bool _useInlinePath;
            private readonly MvxBindingMode _bindingMode;
            private readonly bool _testBindingMode;
            private readonly string _converter;
            private readonly KeyValuePair<string, object> _converterParameterValue;
            private readonly KeyValuePair<string, object> _fallbackValue;

            public PerformSimpleTestParams(
                string sourcePath,
                string targetName,
                bool useInlinePath,
                MvxBindingMode bindingMode,
                bool testBindingMode,
                string converter,
                KeyValuePair<string, object> converterParameterValue,
                KeyValuePair<string, object> fallbackValue)
            {
                _source = sourcePath;
                _useInlinePath = useInlinePath;
                _target = targetName;
                _bindingMode = bindingMode;
                _testBindingMode = testBindingMode;
                _converter = converter;
                _converterParameterValue = converterParameterValue;
                _fallbackValue = fallbackValue;
            }

            public string Source
            {
                get { return _source; }
            }

            public string Target
            {
                get { return _target; }
            }

            public bool UseInlinePath
            {
                get { return _useInlinePath; }
            }

            public MvxBindingMode BindingMode
            {
                get { return _bindingMode; }
            }

            public bool TestBindingMode
            {
                get { return _testBindingMode; }
            }

            public string Converter
            {
                get { return _converter; }
            }

            public KeyValuePair<string, object> ConverterParameterValue
            {
                get { return _converterParameterValue; }
            }

            public KeyValuePair<string, object> FallbackValue
            {
                get { return _fallbackValue; }
            }
        }

        private void PerformParseTest(params PerformSimpleTestParams[] testParamsArray)
        {
            var text = CreateText(testParamsArray);

            var expectedLookup = new MvxSerializableBindingSpecification();
            foreach (var testParams in testParamsArray)
            {
                expectedLookup[testParams.Target] = CreateExpectedDesciption(testParams);
            }

            MvxTrace.Trace(MvxTraceLevel.Diagnostic, "Testing: {0}", text);
            PerformTest(text, expectedLookup);
        }

        private MvxSerializableBindingDescription CreateExpectedDesciption(PerformSimpleTestParams testParams)
        {
            return new MvxSerializableBindingDescription
            {
                Converter = testParams.Converter,
                ConverterParameter = testParams.ConverterParameterValue.Value,
                FallbackValue = testParams.FallbackValue.Value,
                Mode = testParams.TestBindingMode ? testParams.BindingMode : MvxBindingMode.Default,
                Path = string.IsNullOrEmpty(testParams.Source) ? null : testParams.Source
            };
        }


        private string CreateText(IEnumerable<PerformSimpleTestParams> testParams)
        {
            return string.Join(";", testParams.Select(CreateText));
        }

        private void PerformTest(string text, MvxSerializableBindingSpecification expectedLookup)
        {
            var theParser = new MvxSwissBindingParser();
            MvxSerializableBindingSpecification specification;
            Assert.IsTrue(theParser.TryParseBindingSpecification(text, out specification));
            AssertAreEquivalent(expectedLookup, specification);
        }
    }
}