// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Binding.Parse.Binding;
using MvvmCross.Binding.Parse.Binding.Swiss;
using MvvmCross.Tests;

namespace MvvmCross.UnitTest.Binding.Parse.Binding.Swiss
{
    [ClassDataSource<NavigationTestFixture>(Shared = SharedType.Keyed, Key = "MvxTest")]
    [InheritsTests]
    public class MvxSwissBindingTest
        : MvxBaseSwissBindingTest<MvxSwissBindingParser>
    {
        public MvxSwissBindingTest(MvxTestFixture fixture)
            : base(fixture)
        {
        }

        [Test]
        public async Task TestFunctionalValueConverterBinding()
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
            await PerformTest(text, expected);
        }

        [Test]
        public async Task TestFunctionalValueConverterWithParameterBinding()
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
                            ConverterParameter = 12L
                    }
                }
            };
            await PerformTest(text, expected);
        }

        [Test]
        public async Task TestFunctionalValueConverterWithParameterBinding2()
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
            await PerformTest(text, expected);
        }

        [Test]
        public async Task TestFunctionalValueConverterWithParameterBinding3()
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
            await PerformTest(text, expected);
        }

        [Test]
        public async Task TestFunctionalValueConverterWithParameterBinding4()
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
            await PerformTest(text, expected);
        }

        [Test]
        public async Task TestFunctionalValueConverterWithNullInTheName()
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
            await PerformTest(text, expected);
        }
    }
}
