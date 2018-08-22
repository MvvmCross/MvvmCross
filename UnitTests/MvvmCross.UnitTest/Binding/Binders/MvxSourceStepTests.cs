// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using MvvmCross.Binding.Binders;
using MvvmCross.Binding.Bindings.Source.Construction;
using MvvmCross.Binding.Bindings.SourceSteps;
using MvvmCross.Binding.Combiners;
using MvvmCross.Binding.Parse.PropertyPath;
using MvvmCross.Converters;
using MvvmCross.Tests;
using Xunit;

namespace MvvmCross.UnitTest.Binding.Binders
{
    [Collection("MvxTest")]
    public class MvxSourceStepTests
    {
        private readonly NavigationTestFixture _fixture;

        public MvxSourceStepTests(NavigationTestFixture fixture)
        {
            _fixture = fixture;
        }

        public class BaseSource : INotifyPropertyChanged
        {
            public int SubscriptionCount { get; private set; }

            public event PropertyChangedEventHandler PropertyChanged
            {
                add
                {
                    InternalPropertyChanged += value;
                    SubscriptionCount++;
                }
                remove
                {
                    InternalPropertyChanged -= value;
                    SubscriptionCount--;
                }
            }

            private event PropertyChangedEventHandler InternalPropertyChanged;

            protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
            {
                InternalPropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public class MySource : BaseSource
        {
            private ObservableCollection<string> _collection = new ObservableCollection<string>();

            private double _doubleProperty1;

            private double _doubleProperty2;

            private int _intProperty1;

            private int _intProperty2;
            private string _property1;

            private string _property2;

            private MySubSource _subSource;

            public string Property1
            {
                get => _property1;
                set
                {
                    _property1 = value;
                    RaisePropertyChanged();
                }
            }

            public string Property2
            {
                get => _property2;
                set
                {
                    _property2 = value;
                    RaisePropertyChanged();
                }
            }

            public int IntProperty1
            {
                get => _intProperty1;
                set
                {
                    _intProperty1 = value;
                    RaisePropertyChanged();
                }
            }

            public int IntProperty2
            {
                get => _intProperty2;
                set
                {
                    _intProperty2 = value;
                    RaisePropertyChanged();
                }
            }

            public double DoubleProperty1
            {
                get => _doubleProperty1;
                set
                {
                    _doubleProperty1 = value;
                    RaisePropertyChanged();
                }
            }

            public double DoubleProperty2
            {
                get => _doubleProperty2;
                set
                {
                    _doubleProperty2 = value;
                    RaisePropertyChanged();
                }
            }

            public ObservableCollection<string> Collection
            {
                get => _collection;
                set
                {
                    _collection = value;
                    RaisePropertyChanged();
                }
            }

            public MySubSource SubSource
            {
                get => _subSource;
                set
                {
                    _subSource = value;
                    RaisePropertyChanged();
                }
            }
        }

        public class MySubSource : BaseSource
        {
            private string _property1;

            private string _property2;

            public string SubProperty1
            {
                get => _property1;
                set
                {
                    _property1 = value;
                    RaisePropertyChanged();
                }
            }

            public string SubProperty2
            {
                get => _property2;
                set
                {
                    _property2 = value;
                    RaisePropertyChanged();
                }
            }
        }

        public class IntPlus1ValueConverter : MvxValueConverter<int, int>
        {
            protected override int Convert(int value, Type targetType, object parameter, CultureInfo culture)
            {
                return value + 1;
            }

            protected override int ConvertBack(int value, Type targetType, object parameter, CultureInfo culture)
            {
                return value - 1;
            }
        }

        private IMvxSourceStep SetupSimpleBindingTest(BaseSource source, string sourceProperty)
        {
            var realSourceStepFactory = SetupSourceStepFactory();
            var sourceStepDescription = new MvxPathSourceStepDescription
            {
                SourcePropertyPath = sourceProperty
            };

            var sourceStep = realSourceStepFactory.Create(sourceStepDescription);
            sourceStep.DataContext = source;

            return sourceStep;
        }

        private MvxSourceStepFactory SetupSourceStepFactory()
        {
            _fixture.ClearAll();

            var autoValueConverters = new MvxAutoValueConverters();
            _fixture.Ioc.RegisterSingleton<IMvxAutoValueConverters>(autoValueConverters);

            var sourcePropertyParser = new MvxSourcePropertyPathParser();
            _fixture.Ioc.RegisterSingleton<IMvxSourcePropertyPathParser>(sourcePropertyParser);

            var realSourceBindingFactory = new MvxSourceBindingFactory();
            _fixture.Ioc.RegisterSingleton<IMvxSourceBindingFactory>(realSourceBindingFactory);

            var sourceStepFactory = new MvxSourceStepFactory();
            sourceStepFactory.AddOrOverwrite(typeof(MvxPathSourceStepDescription), new MvxPathSourceStepFactory());
            sourceStepFactory.AddOrOverwrite(typeof(MvxLiteralSourceStepDescription),
                new MvxLiteralSourceStepFactory());
            sourceStepFactory.AddOrOverwrite(typeof(MvxCombinerSourceStepDescription),
                new MvxCombinerSourceStepFactory());
            _fixture.Ioc.RegisterSingleton<IMvxSourceStepFactory>(sourceStepFactory);

            var propertySource = new MvxPropertySourceBindingFactoryExtension();
            realSourceBindingFactory.Extensions.Add(propertySource);

            return sourceStepFactory;
        }

        [Fact]
        public void TestCombinerPropertiesMissingBinding()
        {
            var realSourceStepFactory = SetupSourceStepFactory();

            var sourceStepDescription = new MvxCombinerSourceStepDescription
            {
                Combiner = new MvxAddValueCombiner(),
                InnerSteps = new List<MvxSourceStepDescription>
                {
                    new MvxPathSourceStepDescription
                    {
                        SourcePropertyPath = "DoubleProperty1"
                    },
                    new MvxPathSourceStepDescription
                    {
                        SourcePropertyPath = "SubSource.SubProperty1",
                        FallbackValue = "It was missing"
                    }
                }
            };

            var doubleProperty = 12.34;

            var source = new MySource
            {
                DoubleProperty1 = doubleProperty
            };

            var sourceStep = realSourceStepFactory.Create(sourceStepDescription);

            sourceStep.DataContext = source;

            Assert.Equal(typeof(double), sourceStep.SourceType);

            var value = sourceStep.GetValue();
            Assert.Equal($"{doubleProperty}It was missing", value);

            var changes = new List<object>();
            sourceStep.Changed += (sender, args) => { changes.Add(sourceStep.GetValue()); };

            source.DoubleProperty1 = doubleProperty = 11.11;

            Assert.Single(changes);
            Assert.Equal($"{doubleProperty}It was missing", changes[0]);

            value = sourceStep.GetValue();
            Assert.Equal($"{doubleProperty}It was missing", value);

            source.DoubleProperty1 = doubleProperty = 12.11;

            Assert.Equal(2, changes.Count);
            Assert.Equal($"{doubleProperty}It was missing", changes[1]);

            value = sourceStep.GetValue();
            Assert.Equal($"{doubleProperty}It was missing", value);

            source.SubSource = new MySubSource { SubProperty1 = "Hello" };

            Assert.Equal(3, changes.Count);
            Assert.Equal($"{doubleProperty}Hello", changes[2]);

            value = sourceStep.GetValue();
            Assert.Equal($"{doubleProperty}Hello", value);
        }

        [Fact]
        public void TestCombinerPropertiesMissingBinding_Part2()
        {
            var realSourceStepFactory = SetupSourceStepFactory();

            var sourceStepDescription = new MvxCombinerSourceStepDescription
            {
                Combiner = new MvxAddValueCombiner(),
                InnerSteps = new List<MvxSourceStepDescription>
                {
                    new MvxPathSourceStepDescription
                    {
                        SourcePropertyPath = "SubSource.SubProperty1",
                        FallbackValue = "It was missing"
                    },
                    new MvxPathSourceStepDescription
                    {
                        SourcePropertyPath = "DoubleProperty1"
                    }
                }
            };

            var doubleProperty = 12.34;

            var source = new MySource
            {
                DoubleProperty1 = 12.34
            };

            var sourceStep = realSourceStepFactory.Create(sourceStepDescription);

            sourceStep.DataContext = source;

            Assert.Equal(typeof(object), sourceStep.SourceType);

            var value = sourceStep.GetValue();
            Assert.Equal($"It was missing{doubleProperty}", value);

            var changes = new List<object>();
            sourceStep.Changed += (sender, args) => { changes.Add(sourceStep.GetValue()); };

            source.DoubleProperty1 = doubleProperty = 11.11;

            Assert.Single(changes);
            Assert.Equal($"It was missing{doubleProperty}", changes[0]);

            value = sourceStep.GetValue();
            Assert.Equal($"It was missing{doubleProperty}", value);

            source.DoubleProperty1 = doubleProperty = 12.11;

            Assert.Equal(2, changes.Count);
            Assert.Equal($"It was missing{doubleProperty}", changes[1]);

            value = sourceStep.GetValue();
            Assert.Equal($"It was missing{doubleProperty}", value);

            source.SubSource = new MySubSource { SubProperty1 = "Hello" };

            Assert.Equal(3, changes.Count);
            Assert.Equal($"Hello{doubleProperty}", changes[2]);

            value = sourceStep.GetValue();
            Assert.Equal($"Hello{doubleProperty}", value);
        }

        [Fact]
        public void TestCombinerPropertiesPresentBinding()
        {
            var realSourceStepFactory = SetupSourceStepFactory();

            var sourceStepDescription = new MvxCombinerSourceStepDescription
            {
                Combiner = new MvxAddValueCombiner(),
                InnerSteps = new List<MvxSourceStepDescription>
                {
                    new MvxPathSourceStepDescription
                    {
                        SourcePropertyPath = "DoubleProperty1"
                    },
                    new MvxPathSourceStepDescription
                    {
                        SourcePropertyPath = "DoubleProperty2"
                    }
                }
            };

            var source = new MySource
            {
                DoubleProperty1 = 12.34,
                DoubleProperty2 = 23.45
            };

            var sourceStep = realSourceStepFactory.Create(sourceStepDescription);

            sourceStep.DataContext = source;

            Assert.Equal(typeof(double), sourceStep.SourceType);

            var value = sourceStep.GetValue();
            Assert.Equal(35.79, value);

            var changes = new List<object>();
            sourceStep.Changed += (sender, args) => { changes.Add(sourceStep.GetValue()); };

            source.DoubleProperty1 = 11.11;

            Assert.Single(changes);
            Assert.Equal(34.56, changes[0]);

            value = sourceStep.GetValue();
            Assert.Equal(34.56, value);

            source.DoubleProperty2 = 12.11;

            Assert.Equal(2, changes.Count);
            Assert.Equal(23.22, changes[1]);

            value = sourceStep.GetValue();
            Assert.Equal(23.22, value);
        }

        [Fact]
        public void TestIndedexedChangePropagationBinding()
        {
            var realSourceStepFactory = SetupSourceStepFactory();

            var sourceStepDescription = new MvxPathSourceStepDescription
            {
                SourcePropertyPath = "Collection[0]"
            };

            var source = new MySource();
            source.Collection.Add("Initial");

            var sourceStep = realSourceStepFactory.Create(sourceStepDescription);

            sourceStep.DataContext = source;

            Assert.Equal(typeof(string), sourceStep.SourceType);

            var value = sourceStep.GetValue();
            Assert.Equal("Initial", value);

            var changes = new List<object>();
            sourceStep.Changed += (sender, args) => { changes.Add(sourceStep.GetValue()); };

            source.Collection[0] = "Changed to 17";

            Assert.Single(changes);
            Assert.Equal("Changed to 17", changes[0]);

            value = sourceStep.GetValue();
            Assert.Equal("Changed to 17", value);

            sourceStep.DataContext = new MySource();

            value = sourceStep.GetValue();
            Assert.Equal(MvxBindingConstant.UnsetValue, value);

            source.Collection[0] = "Changed again 19";

            Assert.Single(changes);

            sourceStep.DataContext = source;

            Assert.Single(changes);

            value = sourceStep.GetValue();
            Assert.Equal("Changed again 19", value);

            source.Collection[0] = "Changed again again 19";

            Assert.Equal(2, changes.Count);
            Assert.Equal("Changed again again 19", changes[1]);

            value = sourceStep.GetValue();
            Assert.Equal("Changed again again 19", value);
        }

        [Fact]
        public void TestLiteralDoubleBinding()
        {
            var realSourceStepFactory = SetupSourceStepFactory();

            var sourceStepDescription = new MvxLiteralSourceStepDescription
            {
                Literal = 13.72
            };

            var source = new MySource
            {
                IntProperty1 = 42
            };

            var sourceStep = realSourceStepFactory.Create(sourceStepDescription);

            sourceStep.DataContext = source;

            Assert.Equal(typeof(double), sourceStep.SourceType);

            var value = sourceStep.GetValue();
            Assert.Equal(13.72, value);
        }

        [Fact]
        public void TestLiteralStringBinding()
        {
            var realSourceStepFactory = SetupSourceStepFactory();

            var sourceStepDescription = new MvxLiteralSourceStepDescription
            {
                Literal = "Love it"
            };

            var source = new MySource
            {
                IntProperty1 = 42
            };

            var sourceStep = realSourceStepFactory.Create(sourceStepDescription);

            sourceStep.DataContext = source;

            Assert.Equal(typeof(string), sourceStep.SourceType);

            var value = sourceStep.GetValue();
            Assert.Equal("Love it", value);
        }

        [Fact]
        public void TestSimpleChangePropagationBinding()
        {
            var source = new MySource
            {
                Property1 = "Test 42"
            };

            var sourceStep = SetupSimpleBindingTest(source, "Property1");

            Assert.Equal(typeof(string), sourceStep.SourceType);

            var value = sourceStep.GetValue();
            Assert.Equal("Test 42", value);

            var changes = new List<object>();
            sourceStep.Changed += (sender, args) => { changes.Add(sourceStep.GetValue()); };

            source.Property1 = "Changed to 17";

            Assert.Single(changes);
            Assert.Equal("Changed to 17", changes[0]);

            value = sourceStep.GetValue();
            Assert.Equal("Changed to 17", value);

            sourceStep.DataContext = new MySource();

            value = sourceStep.GetValue();
            Assert.Null(value);

            source.Property1 = "Changed again 19";

            Assert.Single(changes);

            sourceStep.DataContext = source;

            Assert.Single(changes);

            value = sourceStep.GetValue();
            Assert.Equal("Changed again 19", value);

            source.Property1 = "Changed again again 19";

            Assert.Equal(2, changes.Count);
            Assert.Equal("Changed again again 19", changes[1]);

            value = sourceStep.GetValue();
            Assert.Equal("Changed again again 19", value);
        }

        [Fact]
        public void TestSimpleCollectionBinding()
        {
            var realSourceStepFactory = SetupSourceStepFactory();

            var sourceStepDescription = new MvxPathSourceStepDescription
            {
                SourcePropertyPath = "Collection[0]",
                FallbackValue = "Pah"
            };

            var source = new MySource();

            var sourceStep = realSourceStepFactory.Create(sourceStepDescription);

            sourceStep.DataContext = source;

            Assert.Equal(typeof(string), sourceStep.SourceType);

            var value = sourceStep.GetValue();
            Assert.Equal("Pah", value);

            source.Collection.Add("Hi there");
            value = sourceStep.GetValue();
            Assert.Equal("Hi there", value);

            sourceStep.SetValue("New value");
            Assert.Equal("New value", source.Collection[0]);
        }

        [Fact]
        public void TestSimpleDoubleBinding()
        {
            var source = new MySource
            {
                DoubleProperty1 = 42.21
            };

            var sourceStep = SetupSimpleBindingTest(source, "DoubleProperty1");

            Assert.Equal(typeof(double), sourceStep.SourceType);

            var value = sourceStep.GetValue();
            Assert.Equal(42.21, value);

            sourceStep.SetValue(13.72);

            Assert.Equal(13.72, source.DoubleProperty1);
        }

        [Fact]
        public void TestSimpleIntBinding()
        {
            var realSourceStepFactory = SetupSourceStepFactory();

            var sourceStepDescription = new MvxPathSourceStepDescription
            {
                SourcePropertyPath = "IntProperty1"
            };

            var source = new MySource
            {
                IntProperty1 = 42
            };

            var sourceStep = realSourceStepFactory.Create(sourceStepDescription);

            sourceStep.DataContext = source;

            Assert.Equal(typeof(int), sourceStep.SourceType);

            var value = sourceStep.GetValue();
            Assert.Equal(42, value);

            sourceStep.SetValue(72);

            Assert.Equal(72, source.IntProperty1);

            sourceStep.SetValue("101");
            Assert.Equal(101, source.IntProperty1);
        }

        [Fact]
        public void TestSimpleIntWithValueConversionBinding()
        {
            var realSourceStepFactory = SetupSourceStepFactory();

            var sourceStepDescription = new MvxPathSourceStepDescription
            {
                SourcePropertyPath = "IntProperty1",
                Converter = new IntPlus1ValueConverter()
            };

            var source = new MySource
            {
                IntProperty1 = 42
            };

            var sourceStep = realSourceStepFactory.Create(sourceStepDescription);

            sourceStep.DataContext = source;

            Assert.Equal(typeof(int), sourceStep.SourceType);

            var value = sourceStep.GetValue();
            Assert.Equal(43, value);

            var changed = -99;
            sourceStep.Changed += (sender, args) => changed = (int) sourceStep.GetValue();

            source.IntProperty1 = 71;
            Assert.Equal(72, changed);

            value = sourceStep.GetValue();
            Assert.Equal(72, value);

            sourceStep.SetValue(101);
            Assert.Equal(100, source.IntProperty1);
        }

        [Fact]
        public void TestSimpleStringBinding()
        {
            var source = new MySource
            {
                Property1 = "Test 42"
            };

            var sourceStep = SetupSimpleBindingTest(source, "Property1");

            Assert.Equal(typeof(string), sourceStep.SourceType);

            var value = sourceStep.GetValue();
            Assert.Equal("Test 42", value);

            sourceStep.SetValue("Life line");

            Assert.Equal("Life line", source.Property1);
        }

        [Fact]
        public void TestSimpleSubObjectChangePropagationBinding()
        {
            var realSourceStepFactory = SetupSourceStepFactory();

            var sourceStepDescription = new MvxPathSourceStepDescription
            {
                SourcePropertyPath = "SubSource.SubProperty1"
            };

            var source = new MySource
            {
                SubSource = new MySubSource { SubProperty1 = "Test 42" }
            };

            var sourceStep = realSourceStepFactory.Create(sourceStepDescription);

            sourceStep.DataContext = source;

            Assert.Equal(typeof(string), sourceStep.SourceType);

            var value = sourceStep.GetValue();
            Assert.Equal("Test 42", value);

            var changes = new List<object>();
            sourceStep.Changed += (sender, args) => { changes.Add(sourceStep.GetValue()); };

            source.SubSource.SubProperty1 = "Changed to 17";

            Assert.Single(changes);
            Assert.Equal("Changed to 17", changes[0]);

            value = sourceStep.GetValue();
            Assert.Equal("Changed to 17", value);

            var oldSubSource = source.SubSource;
            source.SubSource = new MySubSource { SubProperty1 = "New Sub object" };

            Assert.Equal(2, changes.Count);
            Assert.Equal("New Sub object", changes[1]);

            value = sourceStep.GetValue();
            Assert.Equal("New Sub object", value);

            oldSubSource.SubProperty1 = "Should not fire";

            Assert.Equal(2, changes.Count);

            source.SubSource.SubProperty1 = "Should fire";

            Assert.Equal(3, changes.Count);
            Assert.Equal("Should fire", changes[2]);
        }

        [Fact]
        public void TestSimpleSubPropertyBinding()
        {
            var realSourceStepFactory = SetupSourceStepFactory();

            var sourceStepDescription = new MvxPathSourceStepDescription
            {
                SourcePropertyPath = "SubSource.SubProperty2"
            };

            var source = new MySource
            {
                SubSource = new MySubSource { SubProperty2 = "Hello World" }
            };

            var sourceStep = realSourceStepFactory.Create(sourceStepDescription);

            sourceStep.DataContext = source;

            Assert.Equal(typeof(string), sourceStep.SourceType);

            var value = sourceStep.GetValue();
            Assert.Equal("Hello World", value);

            source.SubSource.SubProperty2 = "Hello Mum";

            Assert.Equal(typeof(string), sourceStep.SourceType);

            value = sourceStep.GetValue();
            Assert.Equal("Hello Mum", value);
            source.SubSource.SubProperty2 = "Hello Mum";

            source.SubSource = null;

            Assert.Equal(typeof(object), sourceStep.SourceType);

            value = sourceStep.GetValue();
            Assert.Equal(MvxBindingConstant.UnsetValue, value);
        }
    }
}
