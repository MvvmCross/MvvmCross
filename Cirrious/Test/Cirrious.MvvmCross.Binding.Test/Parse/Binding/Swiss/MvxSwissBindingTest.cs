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
using Cirrious.MvvmCross.Binding.Parse.Binding.Tibet;
using NUnit.Framework;

namespace Cirrious.MvvmCross.Binding.Test.Parse.Binding.Swiss
{
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
            PerformTest(text, expected);
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
            PerformTest(text, expected);
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
            PerformTest(text, expected);
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
            PerformTest(text, expected);
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
            PerformTest(text, expected);
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
            PerformTest(text, expected);
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
                            Converter = "ConvertThis",
                            Function = "Single",
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
            PerformTest(text, expected);
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
                            Converter = "ConvertThis",
                            Function = "Single",
                            Sources = new MvxSerializableBindingDescription[]
                                {
                                    new MvxSerializableBindingDescription()
                                        {
                                            Path = "Foo",
                                        }, 
                                },
                            ConverterParameter = 12
                    }
                }
            };
            MvxTrace.Trace(MvxTraceLevel.Diagnostic, "Testing: {0}", text);
            PerformTest(text, expected);
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
                            Converter = "ConvertThis",
                            Function = "Single",
                            Sources = new MvxSerializableBindingDescription[]
                                {
                                    new MvxSerializableBindingDescription()
                                        {
                                            Path = "Foo",
                                        }, 
                                },
                            ConverterParameter = 12.45
                    }
                }
            };
            MvxTrace.Trace(MvxTraceLevel.Diagnostic, "Testing: {0}", text);
            PerformTest(text, expected);
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
                            Converter = "ConvertThis",
                            Function = "Single",
                            Sources = new MvxSerializableBindingDescription[]
                                {
                                    new MvxSerializableBindingDescription()
                                        {
                                            Path = "Foo",
                                        }, 
                                },
                            ConverterParameter = true
                    }
                }
            };
            MvxTrace.Trace(MvxTraceLevel.Diagnostic, "Testing: {0}", text);
            PerformTest(text, expected);
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
                            Converter = "ConvertThis",
                            Function = "Single",
                            Sources = new MvxSerializableBindingDescription[]
                                {
                                    new MvxSerializableBindingDescription()
                                        {
                                            Path = "Foo",
                                        }, 
                                },
                            ConverterParameter = "Hello World"
                    }
                }
            };
            MvxTrace.Trace(MvxTraceLevel.Diagnostic, "Testing: {0}", text);
            PerformTest(text, expected);
        }
    }
}