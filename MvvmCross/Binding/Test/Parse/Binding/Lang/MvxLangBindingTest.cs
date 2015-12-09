// MvxLangBindingTest.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.Binding.Parse.Binding;
using Cirrious.MvvmCross.Binding.Parse.Binding.Lang;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Cirrious.MvvmCross.Binding.Test.Parse.Binding.Lang
{
    [TestFixture]
    public class MvxLangBindingTest
        : MvxBindingTest
    {
        [Test]
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
            Assert.IsTrue(parsed, "Failed to parse " + testPair.Key);
            Assert.AreEqual(1, result.Count);
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

            Assert.AreEqual(keyAndDescription.Key, resultKeyAndDescription.Key);
            AssertAreEquivalent(expectedDescription, resultKeyAndDescription.Value);
        }

        private Dictionary<string, MvxSerializableBindingSpecification> _toTest = new Dictionary<string, MvxSerializableBindingSpecification>()
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
    }
}