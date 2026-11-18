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

namespace MvvmCross.UnitTest.Binding.Binders
{
    [ClassDataSource<NavigationTestFixture>(Shared = SharedType.Keyed, Key = "MvxTest")]
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

        [Test]
        public async Task TestCombinerPropertiesMissingBinding()
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

            await Assert.That(sourceStep.SourceType).IsEqualTo(typeof(double));

            var value = sourceStep.GetValue();
            await Assert.That(value).IsEqualTo($"{doubleProperty}It was missing");

            var changes = new List<object>();
            sourceStep.Changed += (sender, args) => { changes.Add(sourceStep.GetValue()); };

            source.DoubleProperty1 = doubleProperty = 11.11;

            await Assert.That(changes).HasSingleItem();
            await Assert.That(changes[0]).IsEqualTo($"{doubleProperty}It was missing");

            value = sourceStep.GetValue();
            await Assert.That(value).IsEqualTo($"{doubleProperty}It was missing");

            source.DoubleProperty1 = doubleProperty = 12.11;

            await Assert.That(changes.Count).IsEqualTo(2);
            await Assert.That(changes[1]).IsEqualTo($"{doubleProperty}It was missing");

            value = sourceStep.GetValue();
            await Assert.That(value).IsEqualTo($"{doubleProperty}It was missing");

            source.SubSource = new MySubSource { SubProperty1 = "Hello" };

            await Assert.That(changes.Count).IsEqualTo(3);
            await Assert.That(changes[2]).IsEqualTo($"{doubleProperty}Hello");

            value = sourceStep.GetValue();
            await Assert.That(value).IsEqualTo($"{doubleProperty}Hello");
        }

        [Test]
        public async Task TestCombinerPropertiesMissingBinding_Part2()
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

            await Assert.That(sourceStep.SourceType).IsEqualTo(typeof(object));

            var value = sourceStep.GetValue();
            await Assert.That(value).IsEqualTo($"It was missing{doubleProperty}");

            var changes = new List<object>();
            sourceStep.Changed += (sender, args) => { changes.Add(sourceStep.GetValue()); };

            source.DoubleProperty1 = doubleProperty = 11.11;

            await Assert.That(changes).HasSingleItem();
            await Assert.That(changes[0]).IsEqualTo($"It was missing{doubleProperty}");

            value = sourceStep.GetValue();
            await Assert.That(value).IsEqualTo($"It was missing{doubleProperty}");

            source.DoubleProperty1 = doubleProperty = 12.11;

            await Assert.That(changes.Count).IsEqualTo(2);
            await Assert.That(changes[1]).IsEqualTo($"It was missing{doubleProperty}");

            value = sourceStep.GetValue();
            await Assert.That(value).IsEqualTo($"It was missing{doubleProperty}");

            source.SubSource = new MySubSource { SubProperty1 = "Hello" };

            await Assert.That(changes.Count).IsEqualTo(3);
            await Assert.That(changes[2]).IsEqualTo($"Hello{doubleProperty}");

            value = sourceStep.GetValue();
            await Assert.That(value).IsEqualTo($"Hello{doubleProperty}");
        }

        [Test]
        public async Task TestCombinerPropertiesPresentBinding()
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

            await Assert.That(sourceStep.SourceType).IsEqualTo(typeof(double));

            var value = sourceStep.GetValue();
            await Assert.That(value).IsEqualTo(35.79);

            var changes = new List<object>();
            sourceStep.Changed += (sender, args) => { changes.Add(sourceStep.GetValue()); };

            source.DoubleProperty1 = 11.11;

            await Assert.That(changes).HasSingleItem();
            await Assert.That(changes[0]).IsEqualTo(34.56);

            value = sourceStep.GetValue();
            await Assert.That(value).IsEqualTo(34.56);

            source.DoubleProperty2 = 12.11;

            await Assert.That(changes.Count).IsEqualTo(2);
            await Assert.That(changes[1]).IsEqualTo(23.22);

            value = sourceStep.GetValue();
            await Assert.That(value).IsEqualTo(23.22);
        }

        [Test]
        public async Task TestIndedexedChangePropagationBinding()
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

            await Assert.That(sourceStep.SourceType).IsEqualTo(typeof(string));

            var value = sourceStep.GetValue();
            await Assert.That(value).IsEqualTo("Initial");

            var changes = new List<object>();
            sourceStep.Changed += (sender, args) => { changes.Add(sourceStep.GetValue()); };

            source.Collection[0] = "Changed to 17";

            await Assert.That(changes).HasSingleItem();
            await Assert.That(changes[0]).IsEqualTo("Changed to 17");

            value = sourceStep.GetValue();
            await Assert.That(value).IsEqualTo("Changed to 17");

            sourceStep.DataContext = new MySource();

            value = sourceStep.GetValue();
            await Assert.That(value).IsEqualTo(MvxBindingConstant.UnsetValue);

            source.Collection[0] = "Changed again 19";

            await Assert.That(changes).HasSingleItem();

            sourceStep.DataContext = source;

            await Assert.That(changes).HasSingleItem();

            value = sourceStep.GetValue();
            await Assert.That(value).IsEqualTo("Changed again 19");

            source.Collection[0] = "Changed again again 19";

            await Assert.That(changes.Count).IsEqualTo(2);
            await Assert.That(changes[1]).IsEqualTo("Changed again again 19");

            value = sourceStep.GetValue();
            await Assert.That(value).IsEqualTo("Changed again again 19");
        }

        [Test]
        public async Task TestLiteralDoubleBinding()
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

            await Assert.That(sourceStep.SourceType).IsEqualTo(typeof(double));

            var value = sourceStep.GetValue();
            await Assert.That(value).IsEqualTo(13.72);
        }

        [Test]
        public async Task TestLiteralStringBinding()
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

            await Assert.That(sourceStep.SourceType).IsEqualTo(typeof(string));

            var value = sourceStep.GetValue();
            await Assert.That(value).IsEqualTo("Love it");
        }

        [Test]
        public async Task TestSimpleChangePropagationBinding()
        {
            var source = new MySource
            {
                Property1 = "Test 42"
            };

            var sourceStep = SetupSimpleBindingTest(source, "Property1");

            await Assert.That(sourceStep.SourceType).IsEqualTo(typeof(string));

            var value = sourceStep.GetValue();
            await Assert.That(value).IsEqualTo("Test 42");

            var changes = new List<object>();
            sourceStep.Changed += (sender, args) => { changes.Add(sourceStep.GetValue()); };

            source.Property1 = "Changed to 17";

            await Assert.That(changes).HasSingleItem();
            await Assert.That(changes[0]).IsEqualTo("Changed to 17");

            value = sourceStep.GetValue();
            await Assert.That(value).IsEqualTo("Changed to 17");

            sourceStep.DataContext = new MySource();

            value = sourceStep.GetValue();
            await Assert.That(value).IsNull();

            source.Property1 = "Changed again 19";

            await Assert.That(changes).HasSingleItem();

            sourceStep.DataContext = source;

            await Assert.That(changes).HasSingleItem();

            value = sourceStep.GetValue();
            await Assert.That(value).IsEqualTo("Changed again 19");

            source.Property1 = "Changed again again 19";

            await Assert.That(changes.Count).IsEqualTo(2);
            await Assert.That(changes[1]).IsEqualTo("Changed again again 19");

            value = sourceStep.GetValue();
            await Assert.That(value).IsEqualTo("Changed again again 19");
        }

        [Test]
        public async Task TestSimpleCollectionBinding()
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

            await Assert.That(sourceStep.SourceType).IsEqualTo(typeof(string));

            var value = sourceStep.GetValue();
            await Assert.That(value).IsEqualTo("Pah");

            source.Collection.Add("Hi there");
            value = sourceStep.GetValue();
            await Assert.That(value).IsEqualTo("Hi there");

            sourceStep.SetValue("New value");
            await Assert.That(source.Collection[0]).IsEqualTo("New value");
        }

        [Test]
        public async Task TestSimpleDoubleBinding()
        {
            var source = new MySource
            {
                DoubleProperty1 = 42.21
            };

            var sourceStep = SetupSimpleBindingTest(source, "DoubleProperty1");

            await Assert.That(sourceStep.SourceType).IsEqualTo(typeof(double));

            var value = sourceStep.GetValue();
            await Assert.That(value).IsEqualTo(42.21);

            sourceStep.SetValue(13.72);

            await Assert.That(source.DoubleProperty1).IsEqualTo(13.72);
        }

        [Test]
        public async Task TestSimpleIntBinding()
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

            await Assert.That(sourceStep.SourceType).IsEqualTo(typeof(int));

            var value = sourceStep.GetValue();
            await Assert.That(value).IsEqualTo(42);

            sourceStep.SetValue(72);

            await Assert.That(source.IntProperty1).IsEqualTo(72);

            sourceStep.SetValue("101");
            await Assert.That(source.IntProperty1).IsEqualTo(101);
        }

        [Test]
        public async Task TestSimpleIntWithValueConversionBinding()
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

            await Assert.That(sourceStep.SourceType).IsEqualTo(typeof(int));

            var value = sourceStep.GetValue();
            await Assert.That(value).IsEqualTo(43);

            var changed = -99;
            sourceStep.Changed += (sender, args) => changed = (int)sourceStep.GetValue();

            source.IntProperty1 = 71;
            await Assert.That(changed).IsEqualTo(72);

            value = sourceStep.GetValue();
            await Assert.That(value).IsEqualTo(72);

            sourceStep.SetValue(101);
            await Assert.That(source.IntProperty1).IsEqualTo(100);
        }

        [Test]
        public async Task TestSimpleStringBinding()
        {
            var source = new MySource
            {
                Property1 = "Test 42"
            };

            var sourceStep = SetupSimpleBindingTest(source, "Property1");

            await Assert.That(sourceStep.SourceType).IsEqualTo(typeof(string));

            var value = sourceStep.GetValue();
            await Assert.That(value).IsEqualTo("Test 42");

            sourceStep.SetValue("Life line");

            await Assert.That(source.Property1).IsEqualTo("Life line");
        }

        [Test]
        public async Task TestSimpleSubObjectChangePropagationBinding()
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

            await Assert.That(sourceStep.SourceType).IsEqualTo(typeof(string));

            var value = sourceStep.GetValue();
            await Assert.That(value).IsEqualTo("Test 42");

            var changes = new List<object>();
            sourceStep.Changed += (sender, args) => { changes.Add(sourceStep.GetValue()); };

            source.SubSource.SubProperty1 = "Changed to 17";

            await Assert.That(changes).HasSingleItem();
            await Assert.That(changes[0]).IsEqualTo("Changed to 17");

            value = sourceStep.GetValue();
            await Assert.That(value).IsEqualTo("Changed to 17");

            var oldSubSource = source.SubSource;
            source.SubSource = new MySubSource { SubProperty1 = "New Sub object" };

            await Assert.That(changes.Count).IsEqualTo(2);
            await Assert.That(changes[1]).IsEqualTo("New Sub object");

            value = sourceStep.GetValue();
            await Assert.That(value).IsEqualTo("New Sub object");

            oldSubSource.SubProperty1 = "Should not fire";

            await Assert.That(changes.Count).IsEqualTo(2);

            source.SubSource.SubProperty1 = "Should fire";

            await Assert.That(changes.Count).IsEqualTo(3);
            await Assert.That(changes[2]).IsEqualTo("Should fire");
        }

        [Test]
        public async Task TestSimpleSubPropertyBinding()
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

            await Assert.That(sourceStep.SourceType).IsEqualTo(typeof(string));

            var value = sourceStep.GetValue();
            await Assert.That(value).IsEqualTo("Hello World");

            source.SubSource.SubProperty2 = "Hello Mum";

            await Assert.That(sourceStep.SourceType).IsEqualTo(typeof(string));

            value = sourceStep.GetValue();
            await Assert.That(value).IsEqualTo("Hello Mum");
            source.SubSource.SubProperty2 = "Hello Mum";

            source.SubSource = null;

            await Assert.That(sourceStep.SourceType).IsEqualTo(typeof(object));

            value = sourceStep.GetValue();
            await Assert.That(value).IsEqualTo(MvxBindingConstant.UnsetValue);
        }
    }
}
