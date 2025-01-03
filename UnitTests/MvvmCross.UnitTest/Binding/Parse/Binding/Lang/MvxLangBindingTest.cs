// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Binding;
using MvvmCross.Binding.Parse.Binding;
using MvvmCross.Binding.Parse.Binding.Lang;
using MvvmCross.Tests;
using Xunit;

namespace MvvmCross.UnitTest.Binding.Parse.Binding.Lang
{
    [Collection("MvxTest")]
    public class MvxLangBindingTest
        : MvxBindingTest
    {
        [Fact]
        public void TestAll()
        {
            foreach (var testPair in _toTest)
            {
                DoTest(testPair);
            }
        }

        private void DoTest(KeyValuePair<string, MvxSerializableBindingSpecification> testPair)
        {
            var language = new MvxLanguageBindingParser();
            MvxSerializableBindingSpecification result;
            var parsed = language.TryParseBindingSpecification(testPair.Key, out result);
            Assert.True(parsed, "Failed to parse " + testPair.Key);
            Assert.Single(result);
            var keyAndDescription = testPair.Value.First();
            var resultKeyAndDescription = result.First();
            var expectedDescription = new MvxSerializableBindingDescription()
            {
                Path = keyAndDescription.Value.Path ?? "TextSource",
                Converter = keyAndDescription.Value.Converter ?? "Language",
                ConverterParameter = keyAndDescription.Value.ConverterParameter,
                FallbackValue = keyAndDescription.Value.FallbackValue,
                Mode = MvxBindingMode.OneTime
            };

            Assert.Equal(keyAndDescription.Key, resultKeyAndDescription.Key);
            AssertAreEquivalent(expectedDescription, resultKeyAndDescription.Value);
        }

        private readonly Dictionary<string, MvxSerializableBindingSpecification> _toTest = new Dictionary<string, MvxSerializableBindingSpecification>()
            {
                {
                    "Text Fred",
                    new  MvxSerializableBindingSpecification()
                    {
                        {
                            "Text",
                            new MvxSerializableBindingDescription()
                            {
                                ConverterParameter = "Fred"
                            }
                         }
                    }
                },
                {
                    "Text Key=Fred",
                    new  MvxSerializableBindingSpecification()
                    {
                        {
                            "Text",
                            new MvxSerializableBindingDescription()
                            {
                                ConverterParameter = "Fred"
                            }
                         }
                    }
                },
                {
                    "Text Key='Fred'",
                    new  MvxSerializableBindingSpecification()
                    {
                        {
                            "Text",
                            new MvxSerializableBindingDescription()
                            {
                                ConverterParameter = "Fred"
                            }
                         }
                    }
                },
                {
                    "Text 'Fred.Life Jim'",
                    new  MvxSerializableBindingSpecification()
                    {
                        {
                            "Text",
                            new MvxSerializableBindingDescription()
                            {
                                ConverterParameter = "Fred.Life Jim"
                            }
                         }
                    }
                },
                {
                    "Text Key='Fred.Life Jim'",
                    new  MvxSerializableBindingSpecification()
                    {
                        {
                            "Text",
                            new MvxSerializableBindingDescription()
                            {
                                ConverterParameter = "Fred.Life Jim"
                            }
                         }
                    }
                },
                {
                    "Text Key=Fred.Life",
                    new  MvxSerializableBindingSpecification()
                    {
                        {
                            "Text",
                            new MvxSerializableBindingDescription()
                            {
                                ConverterParameter = "Fred.Life"
                            }
                         }
                    }
                },
                {
                    "Text Fred.Life Jim",
                    new  MvxSerializableBindingSpecification()
                    {
                        {
                            "Text",
                            new MvxSerializableBindingDescription()
                            {
                                ConverterParameter = "Fred.Life Jim"
                            }
                         }
                    }
                },
                {
                    "Text Fred.Life Jim,Converter=MyConv",
                    new  MvxSerializableBindingSpecification()
                    {
                        {
                            "Text",
                            new MvxSerializableBindingDescription()
                            {
                                Converter = "MyConv",
                                ConverterParameter = "Fred.Life Jim"
                            }
                         }
                    }
                },
                {
                    "Text Fred.Life Jim,Converter=MyConv,FallbackValue=Hello World",
                    new  MvxSerializableBindingSpecification()
                    {
                        {
                            "Text",
                            new MvxSerializableBindingDescription()
                            {
                                Converter = "MyConv",
                                ConverterParameter = "Fred.Life Jim",
                                FallbackValue = "Hello World"
                            }
                         }
                    }
                },
                {
                    "SpecialText Fred.Life Jim,Converter=MyConv,FallbackValue=Hello World,Source=SharedTextSource",
                    new  MvxSerializableBindingSpecification()
                    {
                        {
                            "SpecialText",
                            new MvxSerializableBindingDescription()
                            {
                                Converter = "MyConv",
                                ConverterParameter = "Fred.Life Jim",
                                FallbackValue = "Hello World",
                                Path = "SharedTextSource"
                            }
                         }
                    }
                },
            };

        public MvxLangBindingTest(MvxTestFixture fixture)
            : base(fixture)
        {
        }
    }
}
