// MvxSwissBindingTest.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Binding.Parse.Binding;
using Cirrious.MvvmCross.Binding.Parse.Binding.Swiss;
using NUnit.Framework;

namespace Cirrious.MvvmCross.Binding.Test.Parse.Binding.Swiss
{
    [TestFixture]
    public class MvxTibetBindingTest
        : MvxBaseSwissBindingTest<MvxTibetBindingParser>
    {
        [Test]
        public void TestSimpleCombinerBinding()
        {
            var text = "Target $CombineThis(Foo, Foo2)";
            var expected = new MvxSerializableBindingSpecification()
                {
                    {
                        "Target",
                        new MvxSerializableBindingDescription()
                            {
                                Combiner = "CombineThis",
                                Sources = new MvxSerializableBindingDescription[]
                                    {
                                        new MvxSerializableBindingDescription()
                                            {
                                                Path = "Foo",
                                            },
                                        new MvxSerializableBindingDescription()
                                            {
                                                Path = "Foo2",
                                            },
                                    }
                            }
                    }
                };
            MvxTrace.Trace(MvxTraceLevel.Diagnostic, "Testing: {0}", text);
            PerformTest(text, expected);
        }

        [Test]
        public void TestAdvancedCombinerBinding()
        {
            var text = "Target $CombineThis(First(Foo1, 'param1'), (Foo2, Converter=Second, FallbackValue=23), 'test this', 23)";
            var expected = new MvxSerializableBindingSpecification()
                {
                    {
                        "Target",
                        new MvxSerializableBindingDescription()
                            {
                                Combiner = "CombineThis",
                                Sources = new MvxSerializableBindingDescription[]
                                    {
                                        new MvxSerializableBindingDescription()
                                            {
                                                Converter = "First",
                                                ConverterParameter = "param1",                                                
                                                Combiner = "Single",
                                                Sources = new List<MvxSerializableBindingDescription>()
                                                    {
                                                        new MvxSerializableBindingDescription()
                                                            {
                                                                Path = "Foo1"
                                                            }
                                                    }
                                            },
                                        new MvxSerializableBindingDescription()
                                            {
                                                Combiner = "Single",
                                                Sources = new List<MvxSerializableBindingDescription>()
                                                    {
                                                        new MvxSerializableBindingDescription()
                                                            {
                                                        Path = "Foo2",
                                                        Converter = "Second",
                                                        FallbackValue = 23,                                                
                                                            }
                                                    },
                                            },
                                        new MvxSerializableBindingDescription()
                                            {
                                                Literal = "test this",
                                            },
                                        new MvxSerializableBindingDescription()
                                            {
                                                Literal = 23,
                                            },
                                    }
                            }
                    }
                };
            MvxTrace.Trace(MvxTraceLevel.Diagnostic, "Testing: {0}", text);
            PerformTest(text, expected);
        }

        [Test]
        public void TestOperatorCombinerBinding()
        {
            var text = "Target First(Foo1, 'param1') + (Foo2, Converter=Second, FallbackValue=23) - 'test this'";
            var expected = new MvxSerializableBindingSpecification()
                {
                    {
                        "Target",
                        new MvxSerializableBindingDescription()
                            {
                                Combiner = "Add",
                                Sources = new MvxSerializableBindingDescription[]
                                    {
                                        new MvxSerializableBindingDescription()
                                        {
                                            Combiner = "Single",
                                            Converter = "First",
                                            ConverterParameter = "param1",                                                
                                            Sources = new MvxSerializableBindingDescription[]
                                                {
                                                    new MvxSerializableBindingDescription()
                                                    {
                                                        Path = "Foo1"
                                                    }
                                                },
                                        },
                                        new MvxSerializableBindingDescription()
                                            {
                                                Combiner = "Subtract",
                                                Sources = new MvxSerializableBindingDescription[]
                                                    {
                                                        new MvxSerializableBindingDescription()
                                                        {
                                                            Combiner = "Single",
                                                            Sources = new List<MvxSerializableBindingDescription>()
                                                                {
                                                                    new MvxSerializableBindingDescription()
                                                                        {
                                                                                Path = "Foo2",
                                                                                Converter = "Second",
                                                                                FallbackValue = 23,                                                
                                                                        }
                                                                }
                                                        },
                                                        new MvxSerializableBindingDescription()
                                                            {
                                                                Literal = "test this",
                                                            },
                                                        },
                                                    }
                                            },
                            }
                    }
                };
            MvxTrace.Trace(MvxTraceLevel.Diagnostic, "Testing: {0}", text);
            PerformTest(text, expected);
        }

        [Test]
        public void TestOperatorWithPathCombinerBinding()
        {
            var text = "Target First(Foo1, 'param1') + (Foo2, Converter=Second, FallbackValue=23) - Life.Like.That";
            var expected = new MvxSerializableBindingSpecification()
                {
                    {
                        "Target",
                        new MvxSerializableBindingDescription()
                            {
                                Combiner = "Add",
                                Sources = new MvxSerializableBindingDescription[]
                                    {
                                        new MvxSerializableBindingDescription()
                                        {
                                            Combiner = "Single",
                                            Converter = "First",
                                            ConverterParameter = "param1",                                                
                                            Sources = new MvxSerializableBindingDescription[]
                                                {
                                                    new MvxSerializableBindingDescription()
                                                    {
                                                        Path = "Foo1"
                                                    }
                                                },
                                        },
                                        new MvxSerializableBindingDescription()
                                            {
                                                Combiner = "Subtract",
                                                Sources = new MvxSerializableBindingDescription[]
                                                    {
                                                        new MvxSerializableBindingDescription()
                                                        {
                                                            Combiner = "Single",
                                                            Sources = new List<MvxSerializableBindingDescription>()
                                                                {
                                                                    new MvxSerializableBindingDescription()
                                                                        {
                                                                                Path = "Foo2",
                                                                                Converter = "Second",
                                                                                FallbackValue = 23,                                                
                                                                        }
                                                                }
                                                        },
                                                        new MvxSerializableBindingDescription()
                                                            {
                                                                Path = "Life.Like.That",
                                                            },
                                                        },
                                                    }
                                            },
                            }
                    }
                };
            MvxTrace.Trace(MvxTraceLevel.Diagnostic, "Testing: {0}", text);
            PerformTest(text, expected);
        }


        [Test]
        public void TestAllOperators()
        {
            var operators = new Dictionary<string, string>()
                {
                    {"+", "Add"},
                    {"-", "Subtract"},
                    {"*", "Multiply"},
                    {"/", "Divide"},
                    {"%", "Modulus"},
                    {">", "GreaterThan"},
                    {"<", "LessThan"},
                    {">=", "GreaterThanOrEqualTo"},
                    {"<=", "LessThanOrEqualTo"},
                    {"!=", "NotEqualTo"},
                    {"==", "EqualTo"},
                    {"&&", "And"},
                    {"||", "Or"},
                };

            foreach (var kvp in operators)
            {
                var text = string.Format("Target Foo1 {0} Foo2", kvp.Key);
                var expected = new MvxSerializableBindingSpecification()
                {
                    {
                        "Target",
                        new MvxSerializableBindingDescription()
                            {
                                Combiner = kvp.Value,
                                Sources = new MvxSerializableBindingDescription[]
                                    {
                                        new MvxSerializableBindingDescription()
                                        {
                                            Path = "Foo1"
                                        },
                                        new MvxSerializableBindingDescription()
                                        {
                                            Path = "Foo2",
                                        },
                                    }
                            }
                    }
                };
                MvxTrace.Trace(MvxTraceLevel.Diagnostic, "Testing: {0}", text);
                PerformTest(text, expected);
            }
        }

        [Test]
        public void TestLiteralBinding()
        {
            var text = "Target 'James'";
            var expected = new MvxSerializableBindingSpecification()
                {
                    {
                        "Target",
                        new MvxSerializableBindingDescription()
                            {
                                Literal = "James"
                            }
                    }
                };
            MvxTrace.Trace(MvxTraceLevel.Diagnostic, "Testing: {0}", text);
            PerformTest(text, expected);
        }
    }

    [TestFixture]
    public class MvxSwissBindingTest
        : MvxBaseSwissBindingTest<MvxSwissBindingParser>
    {
    }
}