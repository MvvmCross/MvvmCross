// MvxSwissBindingTest.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Test.Parse.Binding.Swiss
{
    using MvvmCross.Binding.Parse.Binding;
    using MvvmCross.Binding.Parse.Binding.Swiss;
    using MvvmCross.Platform.Platform;

    using NUnit.Framework;

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
            this.PerformTest(text, expected);
        }

        [Test]
        public void TestFunctionalValueConverterWithNullInTheName()
        {
            var text = "Target NullThis(Foo, 'Hello World')";
            var expected = new MvxSerializableBindingSpecification()
            {
                {
                    "Target",
                    new MvxSerializableBindingDescription()
                    {
                            Converter = "NullThis",
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
            this.PerformTest(text, expected);
        }
    }
}