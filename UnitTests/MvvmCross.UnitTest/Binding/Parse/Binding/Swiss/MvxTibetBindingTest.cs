// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using MvvmCross.Base.Logging;
using MvvmCross.Binding.Parse.Binding;
using MvvmCross.Binding.Parse.Binding.Tibet;
using MvvmCross.Test;
using Xunit;

namespace MvvmCross.UnitTest.Binding.Parse.Binding.Swiss
{
    [Collection("MvxTest")]
    public class MvxTibetBindingTest
        : MvxBaseSwissBindingTest<MvxTibetBindingParser>
    {
        public MvxTibetBindingTest(MvxTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public void TestFunctionalValueConverterBinding()
        {
            var text = "Target ConvertThis(Foo)";
            var expected = new MvxSerializableBindingSpecification()
                {
                    {
                        "Target",
                        new MvxSerializableBindingDescription()
                            {
                                Function = "ConvertThis",
                                Sources = new MvxSerializableBindingDescription[]
                                    {
                                        new MvxSerializableBindingDescription()
                                            {
                                                Path = "Foo",
                                            },
                                    }
                            }
                    }
                };
            MvxTestLog.Instance.Trace("Testing: {0}", text);
            PerformTest(text, expected);
        }

        [Fact]
        public void TestFunctionalValueConverterWithParameterBinding()
        {
            var text = "Target ConvertThis(Foo, 12)";
            var expected = new MvxSerializableBindingSpecification()
                {
                    {
                        "Target",
                        new MvxSerializableBindingDescription()
                            {
                                Function = "ConvertThis",
                                Sources = new MvxSerializableBindingDescription[]
                                    {
                                        new MvxSerializableBindingDescription()
                                            {
                                                Path = "Foo",
                                            },
                                        new MvxSerializableBindingDescription()
                                            {
                                                Literal = 12L,
                                            },
                                    },
                            }
                    }
                };
            MvxTestLog.Instance.Trace("Testing: {0}", text);
            PerformTest(text, expected);
        }

        [Fact]
        public void TestFunctionalValueConverterWithParameterBinding2()
        {
            var text = "Target ConvertThis(Foo, 12.45)";
            var expected = new MvxSerializableBindingSpecification()
                {
                    {
                        "Target",
                        new MvxSerializableBindingDescription()
                            {
                                Function = "ConvertThis",
                                Sources = new MvxSerializableBindingDescription[]
                                    {
                                        new MvxSerializableBindingDescription()
                                            {
                                                Path = "Foo",
                                            },
                                        new MvxSerializableBindingDescription()
                                            {
                                                Literal = 12.45,
                                            },
                                    },
                            }
                    }
                };
            MvxTestLog.Instance.Trace("Testing: {0}", text);
            PerformTest(text, expected);
        }

        [Fact]
        public void TestFunctionalValueConverterWithParameterBinding3()
        {
            var text = "Target ConvertThis(Foo, true)";
            var expected = new MvxSerializableBindingSpecification()
                {
                    {
                        "Target",
                        new MvxSerializableBindingDescription()
                            {
                                Function = "ConvertThis",
                                Sources = new MvxSerializableBindingDescription[]
                                    {
                                        new MvxSerializableBindingDescription()
                                            {
                                                Path = "Foo",
                                            },
                                        new MvxSerializableBindingDescription()
                                            {
                                                Literal = true,
                                            },
                                    },
                            }
                    }
                };
            MvxTestLog.Instance.Trace("Testing: {0}", text);
            PerformTest(text, expected);
        }

        [Fact]
        public void TestFunctionalValueConverterWithParameterBinding4()
        {
            var text = "Target ConvertThis(Foo, 'Hello World')";
            var expected = new MvxSerializableBindingSpecification()
                {
                    {
                        "Target",
                        new MvxSerializableBindingDescription()
                            {
                                Function = "ConvertThis",
                                Sources = new MvxSerializableBindingDescription[]
                                    {
                                        new MvxSerializableBindingDescription()
                                            {
                                                Path = "Foo",
                                            },
                                        new MvxSerializableBindingDescription()
                                            {
                                                Literal = "Hello World",
                                            },
                                    },
                            }
                    }
                };
            MvxTestLog.Instance.Trace("Testing: {0}", text);
            PerformTest(text, expected);
        }

        [Fact]
        public void TestSimpleCombinerBinding()
        {
            var text = "Target CombineThis(Foo, Foo2)";
            var expected = new MvxSerializableBindingSpecification()
                {
                    {
                        "Target",
                        new MvxSerializableBindingDescription()
                            {
                                Function = "CombineThis",
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
            MvxTestLog.Instance.Trace("Testing: {0}", text);
            PerformTest(text, expected);
        }

        [Fact]
        public void TestAdvancedCombinerBinding()
        {
            var text = "Target CombineThis(First(Foo1, 'param1'), (Foo2, Converter=Second, FallbackValue=23), 'test this', 23)";
            var expected = new MvxSerializableBindingSpecification()
                {
                    {
                        "Target",
                        new MvxSerializableBindingDescription()
                            {
                                Function =  "CombineThis",
                                Sources = new MvxSerializableBindingDescription[]
                                    {
                                        new MvxSerializableBindingDescription()
                                            {
                                                Function = "First",
                                                Sources = new List<MvxSerializableBindingDescription>()
                                                    {
                                                        new MvxSerializableBindingDescription()
                                                            {
                                                                Path = "Foo1"
                                                            },
                                                        new MvxSerializableBindingDescription()
                                                            {
                                                                Literal = "param1"
                                                            }
                                                    }
                                            },
                                        new MvxSerializableBindingDescription()
                                            {
                                                Function =  "Single",
                                                Sources = new List<MvxSerializableBindingDescription>()
                                                    {
                                                        new MvxSerializableBindingDescription()
                                                            {
                                                                Path = "Foo2",
                                                                Converter = "Second",
                                                                FallbackValue = 23L,
                                                            }
                                                    },
                                            },
                                        new MvxSerializableBindingDescription()
                                            {
                                                Literal = "test this",
                                            },
                                        new MvxSerializableBindingDescription()
                                            {
                                                Literal = 23L,
                                            },
                                    }
                            }
                    }
                };
            MvxTestLog.Instance.Trace("Testing: {0}", text);
            PerformTest(text, expected);
        }

        [Fact]
        public void TestOperatorCombinerBinding()
        {
            var text = "Target First(Foo1, 'param1') + (Foo2, Converter=Second, FallbackValue=23) - 'test this'";
            var expected = new MvxSerializableBindingSpecification()
                {
                    {
                        "Target",
                        new MvxSerializableBindingDescription()
                            {
                                Function = "Add",
                                Sources = new MvxSerializableBindingDescription[]
                                    {
                                        new MvxSerializableBindingDescription()
                                            {
                                                Function = "First",
                                                Sources = new MvxSerializableBindingDescription[]
                                                    {
                                                        new MvxSerializableBindingDescription()
                                                            {
                                                                Path = "Foo1"
                                                            },
                                                        new MvxSerializableBindingDescription()
                                                            {
                                                                Literal = "param1"
                                                            }
                                                    },
                                            },
                                        new MvxSerializableBindingDescription()
                                            {
                                                Function = "Subtract",
                                                Sources = new MvxSerializableBindingDescription[]
                                                    {
                                                        new MvxSerializableBindingDescription()
                                                            {
                                                                Function = "Single",
                                                                Sources = new List<MvxSerializableBindingDescription>()
                                                                    {
                                                                        new MvxSerializableBindingDescription()
                                                                            {
                                                                                Path = "Foo2",
                                                                                Converter = "Second",
                                                                                FallbackValue = 23L,
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
            MvxTestLog.Instance.Trace("Testing: {0}", text);
            PerformTest(text, expected);
        }

        [Fact]
        public void TestOperatorWithPathCombinerBinding()
        {
            var text = "Target First(Foo1, 'param1') + (Foo2, Converter=Second, FallbackValue=23) - Life.Like.That";
            var expected = new MvxSerializableBindingSpecification()
                {
                    {
                        "Target",
                        new MvxSerializableBindingDescription()
                            {
                                Function = "Add",
                                Sources = new MvxSerializableBindingDescription[]
                                    {
                                        new MvxSerializableBindingDescription()
                                            {
                                                Function = "First",
                                                Sources = new MvxSerializableBindingDescription[]
                                                    {
                                                        new MvxSerializableBindingDescription()
                                                            {
                                                                Path = "Foo1"
                                                            },
                                                        new MvxSerializableBindingDescription()
                                                            {
                                                                Literal = "param1"
                                                            }
                                                    },
                                            },
                                        new MvxSerializableBindingDescription()
                                            {
                                                Function = "Subtract",
                                                Sources = new MvxSerializableBindingDescription[]
                                                    {
                                                        new MvxSerializableBindingDescription()
                                                            {
                                                                Function = "Single",
                                                                Sources = new List<MvxSerializableBindingDescription>()
                                                                    {
                                                                        new MvxSerializableBindingDescription()
                                                                            {
                                                                                Path = "Foo2",
                                                                                Converter = "Second",
                                                                                FallbackValue = 23L,
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
            MvxTestLog.Instance.Trace("Testing: {0}", text);
            PerformTest(text, expected);
        }

        [Fact]
        public void TestAllOperators()
        {
            var operators = new Dictionary<string, string>()
                {
                    { "+", "Add" },
                    { "-", "Subtract" },
                    { "*", "Multiply" },
                    { "/", "Divide" },
                    { "%", "Modulus" },
                    { ">", "GreaterThan" },
                    { "<", "LessThan" },
                    { ">=", "GreaterThanOrEqualTo" },
                    { "<=", "LessThanOrEqualTo" },
                    { "!=", "NotEqualTo" },
                    { "==", "EqualTo" },
                    { "&&", "And" },
                    { "||", "Or" }
                };

            foreach (var kvp in operators)
            {
                var text = $"Target Foo1 {kvp.Key} Foo2";
                var expected = new MvxSerializableBindingSpecification()
                    {
                        {
                            "Target",
                            new MvxSerializableBindingDescription()
                                {
                                    Function = kvp.Value,
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
                MvxTestLog.Instance.Trace("Testing: {0}", text);
                PerformTest(text, expected);
            }
        }

        [Fact]
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
            MvxTestLog.Instance.Trace("Testing: {0}", text);
            PerformTest(text, expected);
        }

        [Fact]
        public void TestLiteralNullBinding()
        {
            var text = "Target null";
            var expected = new MvxSerializableBindingSpecification()
                {
                    {
                        "Target",
                        new MvxSerializableBindingDescription()
                            {
                                Literal = MvxTibetBindingParser.LiteralNull
                            }
                    }
                };
            MvxTestLog.Instance.Trace("Testing: {0}", text);
            PerformTest(text, expected);
        }

        [Fact]
        public void TestValueConverterNullBinding()
        {
            var text = "Target Conv(null)";
            var expected = new MvxSerializableBindingSpecification()
                {
                    {
                        "Target",
                        new MvxSerializableBindingDescription()
                            {
                            Function = "Conv",
                            Sources = new MvxSerializableBindingDescription[]
                                {
                                    new MvxSerializableBindingDescription()
                                        {
                                            Literal = MvxTibetBindingParser.LiteralNull
                                        },
                                },
                            }
                    }
                };
            MvxTestLog.Instance.Trace("Testing: {0}", text);
            PerformTest(text, expected);
        }

        [Fact]
        public void TestFunctionalValueConverterWithNullInTheName()
        {
            TestFunctionalValueConverterWithKEYWORDInTheName("Null");
            TestFunctionalValueConverterWithKEYWORDInTheName("null");
            TestFunctionalValueConverterWithKEYWORDInTheName("NULL");
        }

        [Fact]
        public void TestFunctionalValueConverterWithTrueInTheName()
        {
            TestFunctionalValueConverterWithKEYWORDInTheName("True");
            TestFunctionalValueConverterWithKEYWORDInTheName("true");
            TestFunctionalValueConverterWithKEYWORDInTheName("TRUE");
        }

        [Fact]
        public void TestFunctionalValueConverterWithFalseInTheName()
        {
            TestFunctionalValueConverterWithKEYWORDInTheName("False");
            TestFunctionalValueConverterWithKEYWORDInTheName("false");
            TestFunctionalValueConverterWithKEYWORDInTheName("FALSE");
        }

        public void TestFunctionalValueConverterWithKEYWORDInTheName(string keyword)
        {
            var text = "Target " + keyword + "This(Foo, 'Hello World')";
            var expected = new MvxSerializableBindingSpecification()
            {
                {
                    "Target",
                    new MvxSerializableBindingDescription()
                    {
                            Function = keyword + "This",
                            Sources = new MvxSerializableBindingDescription[]
                                {
                                    new MvxSerializableBindingDescription()
                                        {
                                            Path = "Foo",
                                        },
                                    new MvxSerializableBindingDescription()
                                        {
                                            Literal = "Hello World",
                                        },
                                },
                    }
                }
            };
            MvxTestLog.Instance.Trace("Testing: {0}", text);
            PerformTest(text, expected);
        }

        [Fact]
        public void TestCommandParameterSpecialBinding()
        {
            var text = "Target CommandParameter(One, Two)";
            var expected = new MvxSerializableBindingSpecification()
                {
                    {
                        "Target",
                        new MvxSerializableBindingDescription()
                            {
                            Function = "CommandParameter",
                            Sources = new MvxSerializableBindingDescription[]
                                {
                                    new MvxSerializableBindingDescription()
                                        {
                                            Path = "One"
                                        },
                                    new MvxSerializableBindingDescription()
                                        {
                                            Path = "Two"
                                        },
                                },
                            }
                    }
                };
            MvxTestLog.Instance.Trace("Testing: {0}", text);
            PerformTest(text, expected);
        }
    }
}
