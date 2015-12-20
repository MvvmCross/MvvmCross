// MvxTibetBindingTest.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Test.Parse.Binding.Swiss
{
    using System.Collections.Generic;

    using MvvmCross.Binding.Parse.Binding;
    using MvvmCross.Binding.Parse.Binding.Tibet;
    using MvvmCross.Platform.Platform;

    using NUnit.Framework;

    [TestFixture]
    public class MvxTibetBindingTest
        : MvxBaseSwissBindingTest<MvxTibetBindingParser>
    {
        [Test]
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
            MvxTrace.Trace(MvxTraceLevel.Diagnostic, "Testing: {0}", text);
            this.PerformTest(text, expected);
        }

        [Test]
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
                                                Literal = 12,
                                            },
                                    },
                            }
                    }
                };
            MvxTrace.Trace(MvxTraceLevel.Diagnostic, "Testing: {0}", text);
            this.PerformTest(text, expected);
        }

        [Test]
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
            MvxTrace.Trace(MvxTraceLevel.Diagnostic, "Testing: {0}", text);
            this.PerformTest(text, expected);
        }

        [Test]
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
            MvxTrace.Trace(MvxTraceLevel.Diagnostic, "Testing: {0}", text);
            this.PerformTest(text, expected);
        }

        [Test]
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
            MvxTrace.Trace(MvxTraceLevel.Diagnostic, "Testing: {0}", text);
            this.PerformTest(text, expected);
        }

        [Test]
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
            MvxTrace.Trace(MvxTraceLevel.Diagnostic, "Testing: {0}", text);
            this.PerformTest(text, expected);
        }

        [Test]
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
            this.PerformTest(text, expected);
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
            this.PerformTest(text, expected);
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
            this.PerformTest(text, expected);
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
                MvxTrace.Trace(MvxTraceLevel.Diagnostic, "Testing: {0}", text);
                this.PerformTest(text, expected);
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
            this.PerformTest(text, expected);
        }

        [Test]
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
            MvxTrace.Trace(MvxTraceLevel.Diagnostic, "Testing: {0}", text);
            this.PerformTest(text, expected);
        }

        [Test]
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
            MvxTrace.Trace(MvxTraceLevel.Diagnostic, "Testing: {0}", text);
            this.PerformTest(text, expected);
        }

        [Test]
        public void TestFunctionalValueConverterWithNullInTheName()
        {
            this.TestFunctionalValueConverterWithKEYWORDInTheName("Null");
            this.TestFunctionalValueConverterWithKEYWORDInTheName("null");
            this.TestFunctionalValueConverterWithKEYWORDInTheName("NULL");
        }

        [Test]
        public void TestFunctionalValueConverterWithTrueInTheName()
        {
            this.TestFunctionalValueConverterWithKEYWORDInTheName("True");
            this.TestFunctionalValueConverterWithKEYWORDInTheName("true");
            this.TestFunctionalValueConverterWithKEYWORDInTheName("TRUE");
        }

        [Test]
        public void TestFunctionalValueConverterWithFalseInTheName()
        {
            this.TestFunctionalValueConverterWithKEYWORDInTheName("False");
            this.TestFunctionalValueConverterWithKEYWORDInTheName("false");
            this.TestFunctionalValueConverterWithKEYWORDInTheName("FALSE");
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
            MvxTrace.Trace(MvxTraceLevel.Diagnostic, "Testing: {0}", text);
            this.PerformTest(text, expected);
        }

        [Test]
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
            MvxTrace.Trace(MvxTraceLevel.Diagnostic, "Testing: {0}", text);
            this.PerformTest(text, expected);
        }
    }
}