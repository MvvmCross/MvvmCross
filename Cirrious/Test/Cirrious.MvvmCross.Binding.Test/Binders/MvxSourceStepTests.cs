// MvxSourceStepTests.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Cirrious.CrossCore.Converters;
using Cirrious.MvvmCross.Binding.Binders;
using Cirrious.MvvmCross.Binding.Bindings.Source;
using Cirrious.MvvmCross.Binding.Bindings.Source.Construction;
using Cirrious.MvvmCross.Binding.Bindings.SourceSteps;
using Cirrious.MvvmCross.Binding.Combiners;
using Cirrious.MvvmCross.Binding.Parse.PropertyPath;
using Cirrious.MvvmCross.Test.Core;
using Moq;
using NUnit.Framework;

namespace Cirrious.MvvmCross.Binding.Test.Binders
{
    [TestFixture]
    public class MvxSourceStepTests : MvxIoCSupportingTest
    {
        public class BaseSource : INotifyPropertyChanged
        {
            public int SubscriptionCount;

            private event PropertyChangedEventHandler _PropertyChanged;
            public event PropertyChangedEventHandler PropertyChanged
            {
                add 
                {
                    _PropertyChanged += value;
                    SubscriptionCount++;
                }
                remove
                {
                    _PropertyChanged -= value;
                    SubscriptionCount--;
                }
            }

            protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
            {
                var handler = _PropertyChanged;
                if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public class MySource : BaseSource
        {
            private string _property1;
            public string Property1
            {
                get { return _property1; }
                set { _property1 = value; RaisePropertyChanged(); }
            }

            private string _property2;
            public string Property2
            {
                get { return _property2; }
                set { _property2 = value; RaisePropertyChanged(); }
            }

            private int _intProperty1;
            public int IntProperty1
            {
                get { return _intProperty1; }
                set { _intProperty1 = value; RaisePropertyChanged(); }
            }

            private int _intProperty2;
            public int IntProperty2
            {
                get { return _intProperty2; }
                set { _intProperty2 = value; RaisePropertyChanged(); }
            }

            private double _doubleProperty1;
            public double DoubleProperty1
            {
                get { return _doubleProperty1; }
                set { _doubleProperty1 = value; RaisePropertyChanged(); }
            }

            private double _doubleProperty2;
            public double DoubleProperty2
            {
                get { return _doubleProperty2; }
                set { _doubleProperty2 = value; RaisePropertyChanged(); }
            }

            private ObservableCollection<string> _collection = new ObservableCollection<string>();
            public ObservableCollection<string> Collection
            {
                get { return _collection; }
                set { _collection = value; RaisePropertyChanged(); }
            }

            private MySubSource _subSource;
            public MySubSource SubSource
            {
                get { return _subSource; }
                set { _subSource = value; RaisePropertyChanged(); }
            }            
        }

        public class MySubSource : BaseSource
        {
            private string _property1;
            public string SubProperty1
            {
                get { return _property1; }
                set { _property1 = value; RaisePropertyChanged(); }
            }

            private string _property2;
            public string SubProperty2
            {
                get { return _property2; }
                set { _property2 = value; RaisePropertyChanged(); }
            }
        }

        public class IntPlus1ValueConverter : MvxValueConverter<int, int>
        {
            protected override int Convert(int value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                return value + 1;
            }

            protected override int ConvertBack(int value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                return value - 1;
            }
        }

        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            SetInvariantCulture();
        }

        [Test]
        public void TestSimpleStringBinding()
        {
            var realSourceStepFactory = SetupSourceStepFactory();

            var sourceStepDescription = new MvxPathSourceStepDescription()
                {
                    SourcePropertyPath = "Property1"
                };

            var source = new MySource()
                {
                    Property1 = "Test 42"
                };

            var sourceStep = realSourceStepFactory.Create(sourceStepDescription);

            sourceStep.DataContext = source;

            Assert.AreEqual(typeof (string), sourceStep.SourceType);

            object value = sourceStep.GetValue();
            Assert.AreEqual("Test 42", value);

            sourceStep.SetValue("Life line");

            Assert.AreEqual("Life line", source.Property1);
        }

        [Test]
        public void TestSimpleIntBinding()
        {
            var realSourceStepFactory = SetupSourceStepFactory();

            var sourceStepDescription = new MvxPathSourceStepDescription()
            {
                SourcePropertyPath = "IntProperty1"
            };

            var source = new MySource()
            {
                IntProperty1 = 42
            };

            var sourceStep = realSourceStepFactory.Create(sourceStepDescription);

            sourceStep.DataContext = source;

            Assert.AreEqual(typeof(int), sourceStep.SourceType);

            object value = sourceStep.GetValue();
            Assert.AreEqual(42, value);

            sourceStep.SetValue(72);

            Assert.AreEqual(72, source.IntProperty1);

            sourceStep.SetValue("101");
            Assert.AreEqual(101, source.IntProperty1);
        }

        [Test]
        public void TestSimpleDoubleBinding()
        {
            var realSourceStepFactory = SetupSourceStepFactory();

            var sourceStepDescription = new MvxPathSourceStepDescription()
            {
                SourcePropertyPath = "DoubleProperty1"
            };

            var source = new MySource()
            {
                DoubleProperty1 = 42.21
            };

            var sourceStep = realSourceStepFactory.Create(sourceStepDescription);

            sourceStep.DataContext = source;

            Assert.AreEqual(typeof(double), sourceStep.SourceType);

            object value = sourceStep.GetValue();
            Assert.AreEqual(42.21, value);

            sourceStep.SetValue(13.72);

            Assert.AreEqual(13.72, source.DoubleProperty1);
        }

        [Test]
        public void TestSimpleCollectionBinding()
        {
            var realSourceStepFactory = SetupSourceStepFactory();

            var sourceStepDescription = new MvxPathSourceStepDescription()
            {
                SourcePropertyPath = "Collection[0]",
                FallbackValue = "Pah"
            };

            var source = new MySource()
            {
            };

            var sourceStep = realSourceStepFactory.Create(sourceStepDescription);

            sourceStep.DataContext = source;

            Assert.AreEqual(typeof(string), sourceStep.SourceType);

            object value = sourceStep.GetValue();
            Assert.AreEqual("Pah", value);

            source.Collection.Add("Hi there");
            value = sourceStep.GetValue();
            Assert.AreEqual("Hi there", value);

            sourceStep.SetValue("New value");
            Assert.AreEqual("New value", source.Collection[0]);
        }

        [Test]
        public void TestSimpleSubPropertyBinding()
        {
            var realSourceStepFactory = SetupSourceStepFactory();

            var sourceStepDescription = new MvxPathSourceStepDescription()
            {
                SourcePropertyPath = "SubSource.SubProperty2"
            };

            var source = new MySource()
            {
                SubSource = new MySubSource() {  SubProperty2 = "Hello World" }
            };

            var sourceStep = realSourceStepFactory.Create(sourceStepDescription);

            sourceStep.DataContext = source;

            Assert.AreEqual(typeof(string), sourceStep.SourceType);

            object value = sourceStep.GetValue();
            Assert.AreEqual("Hello World", value);

            source.SubSource.SubProperty2 = "Hello Mum";

            Assert.AreEqual(typeof(string), sourceStep.SourceType);

            value = sourceStep.GetValue();
            Assert.AreEqual("Hello Mum", value);
            source.SubSource.SubProperty2 = "Hello Mum";

            source.SubSource = null;

            Assert.AreEqual(typeof(object), sourceStep.SourceType);

            value = sourceStep.GetValue();
            Assert.AreEqual(MvxBindingConstant.UnsetValue, value);
        }

        [Test]
        public void TestSimpleChangePropagationBinding()
        {
            var realSourceStepFactory = SetupSourceStepFactory();

            var sourceStepDescription = new MvxPathSourceStepDescription()
            {
                SourcePropertyPath = "Property1"
            };

            var source = new MySource()
            {
                Property1 = "Test 42"
            };

            var sourceStep = realSourceStepFactory.Create(sourceStepDescription);

            sourceStep.DataContext = source;

            Assert.AreEqual(typeof(string), sourceStep.SourceType);

            object value = sourceStep.GetValue();
            Assert.AreEqual("Test 42", value);

            var changes = new List<object>();
            sourceStep.Changed += (sender, args) =>
                {
                    changes.Add(sourceStep.GetValue());
                };

            source.Property1 = "Changed to 17";

            Assert.AreEqual(1, changes.Count);
            Assert.AreEqual("Changed to 17", changes[0]);

            value = sourceStep.GetValue();
            Assert.AreEqual("Changed to 17", value);

            sourceStep.DataContext = new MySource();

            value = sourceStep.GetValue();
            Assert.AreEqual(null, value);

            source.Property1 = "Changed again 19";

            Assert.AreEqual(1, changes.Count);

            sourceStep.DataContext = source;

            Assert.AreEqual(1, changes.Count);

            value = sourceStep.GetValue();
            Assert.AreEqual("Changed again 19", value);

            source.Property1 = "Changed again again 19";

            Assert.AreEqual(2, changes.Count);
            Assert.AreEqual("Changed again again 19", changes[1]);

            value = sourceStep.GetValue();
            Assert.AreEqual("Changed again again 19", value);
        }

        [Test]
        public void TestIndedexedChangePropagationBinding()
        {
            var realSourceStepFactory = SetupSourceStepFactory();

            var sourceStepDescription = new MvxPathSourceStepDescription()
            {
                SourcePropertyPath = "Collection[0]"
            };

            var source = new MySource()
            {
            };
            source.Collection.Add("Initial");

            var sourceStep = realSourceStepFactory.Create(sourceStepDescription);

            sourceStep.DataContext = source;

            Assert.AreEqual(typeof(string), sourceStep.SourceType);

            object value = sourceStep.GetValue();
            Assert.AreEqual("Initial", value);

            var changes = new List<object>();
            sourceStep.Changed += (sender, args) =>
            {
                changes.Add(sourceStep.GetValue());
            };

            source.Collection[0] = "Changed to 17";

            Assert.AreEqual(1, changes.Count);
            Assert.AreEqual("Changed to 17", changes[0]);

            value = sourceStep.GetValue();
            Assert.AreEqual("Changed to 17", value);

            sourceStep.DataContext = new MySource();

            value = sourceStep.GetValue();
            Assert.AreEqual(MvxBindingConstant.UnsetValue, value);

            source.Collection[0] = "Changed again 19";

            Assert.AreEqual(1, changes.Count);

            sourceStep.DataContext = source;

            Assert.AreEqual(1, changes.Count);

            value = sourceStep.GetValue();
            Assert.AreEqual("Changed again 19", value);

            source.Collection[0] = "Changed again again 19";

            Assert.AreEqual(2, changes.Count);
            Assert.AreEqual("Changed again again 19", changes[1]);

            value = sourceStep.GetValue();
            Assert.AreEqual("Changed again again 19", value);
        }

        [Test]
        public void TestSimpleSubObjectChangePropagationBinding()
        {
            var realSourceStepFactory = SetupSourceStepFactory();

            var sourceStepDescription = new MvxPathSourceStepDescription()
            {
                SourcePropertyPath = "SubSource.SubProperty1"
            };

            var source = new MySource()
            {
                SubSource = new MySubSource() { SubProperty1 = "Test 42" }
            };

            var sourceStep = realSourceStepFactory.Create(sourceStepDescription);

            sourceStep.DataContext = source;

            Assert.AreEqual(typeof(string), sourceStep.SourceType);

            object value = sourceStep.GetValue();
            Assert.AreEqual("Test 42", value);

            var changes = new List<object>();
            sourceStep.Changed += (sender, args) =>
            {
                changes.Add(sourceStep.GetValue());
            };

            source.SubSource.SubProperty1 = "Changed to 17";

            Assert.AreEqual(1, changes.Count);
            Assert.AreEqual("Changed to 17", changes[0]);

            value = sourceStep.GetValue();
            Assert.AreEqual("Changed to 17", value);

            var oldSubSource = source.SubSource;
            source.SubSource = new MySubSource() { SubProperty1 = "New Sub object"};

            Assert.AreEqual(2, changes.Count);
            Assert.AreEqual("New Sub object", changes[1]);

            value = sourceStep.GetValue();
            Assert.AreEqual("New Sub object", value);

            oldSubSource.SubProperty1 = "Should not fire";

            Assert.AreEqual(2, changes.Count);

            source.SubSource.SubProperty1 = "Should fire";

            Assert.AreEqual(3, changes.Count);
            Assert.AreEqual("Should fire", changes[2]);
        }

        [Test]
        public void TestSimpleIntWithValueConversionBinding()
        {
            var realSourceStepFactory = SetupSourceStepFactory();

            var sourceStepDescription = new MvxPathSourceStepDescription()
            {
                SourcePropertyPath = "IntProperty1",
                Converter = new IntPlus1ValueConverter()
            };

            var source = new MySource()
            {
                IntProperty1 = 42
            };

            var sourceStep = realSourceStepFactory.Create(sourceStepDescription);

            sourceStep.DataContext = source;

            Assert.AreEqual(typeof(int), sourceStep.SourceType);

            object value = sourceStep.GetValue();
            Assert.AreEqual(43, value);

            int changed = -99;
            sourceStep.Changed += (sender, args) => changed = (int)sourceStep.GetValue();

            source.IntProperty1 = 71;
            Assert.AreEqual(72, changed);

            value = sourceStep.GetValue();
            Assert.AreEqual(72, value);

            sourceStep.SetValue(101);
            Assert.AreEqual(100, source.IntProperty1);
        }

        [Test]
        public void TestLiteralStringBinding()
        {
            var realSourceStepFactory = SetupSourceStepFactory();

            var sourceStepDescription = new MvxLiteralSourceStepDescription()
            {
                Literal = "Love it"
            };

            var source = new MySource()
            {
                IntProperty1 = 42
            };

            var sourceStep = realSourceStepFactory.Create(sourceStepDescription);

            sourceStep.DataContext = source;

            Assert.AreEqual(typeof(string), sourceStep.SourceType);

            object value = sourceStep.GetValue();
            Assert.AreEqual("Love it", value);
        }

        [Test]
        public void TestLiteralDoubleBinding()
        {
            var realSourceStepFactory = SetupSourceStepFactory();

            var sourceStepDescription = new MvxLiteralSourceStepDescription()
            {
                Literal = 13.72
            };

            var source = new MySource()
            {
                IntProperty1 = 42
            };

            var sourceStep = realSourceStepFactory.Create(sourceStepDescription);

            sourceStep.DataContext = source;

            Assert.AreEqual(typeof(double), sourceStep.SourceType);

            object value = sourceStep.GetValue();
            Assert.AreEqual(13.72, value);
        }

        [Test]
        public void TestCombinerPropertiesPresentBinding()
        {
            var realSourceStepFactory = SetupSourceStepFactory();

            var sourceStepDescription = new MvxCombinerSourceStepDescription()
            {
                Combiner = new MvxAddValueCombiner(),
                InnerSteps = new List<MvxSourceStepDescription>()
                    {
                        new MvxPathSourceStepDescription()
                            {
                                SourcePropertyPath = "DoubleProperty1",
                            },
                        new MvxPathSourceStepDescription()
                            {
                                SourcePropertyPath = "DoubleProperty2",
                            },
                    }
            };

            var source = new MySource()
            {
                DoubleProperty1 = 12.34,
                DoubleProperty2 = 23.45
            };

            var sourceStep = realSourceStepFactory.Create(sourceStepDescription);

            sourceStep.DataContext = source;

            Assert.AreEqual(typeof(double), sourceStep.SourceType);

            object value = sourceStep.GetValue();
            Assert.AreEqual(35.79, value);

            var changes = new List<object>();
            sourceStep.Changed += (sender, args) =>
                {
                    changes.Add(sourceStep.GetValue());
                };

            source.DoubleProperty1 = 11.11;

            Assert.AreEqual(1, changes.Count);
            Assert.AreEqual(34.56, changes[0]);

            value = sourceStep.GetValue();
            Assert.AreEqual(34.56, value);

            source.DoubleProperty2 = 12.11;

            Assert.AreEqual(2, changes.Count);
            Assert.AreEqual(23.22, changes[1]);

            value = sourceStep.GetValue();
            Assert.AreEqual(23.22, value);
        }

        [Test]
        public void TestCombinerPropertiesMissingBinding()
        {
            var realSourceStepFactory = SetupSourceStepFactory();

            var sourceStepDescription = new MvxCombinerSourceStepDescription()
            {
                Combiner = new MvxAddValueCombiner(),
                InnerSteps = new List<MvxSourceStepDescription>()
                    {
                        new MvxPathSourceStepDescription()
                            {
                                SourcePropertyPath = "DoubleProperty1",
                            },
                        new MvxPathSourceStepDescription()
                            {
                                SourcePropertyPath = "SubSource.SubProperty1",
                                FallbackValue = "It was missing"
                            },
                    }
            };

            var source = new MySource()
            {
                DoubleProperty1 = 12.34,
            };

            var sourceStep = realSourceStepFactory.Create(sourceStepDescription);

            sourceStep.DataContext = source;

            Assert.AreEqual(typeof(double), sourceStep.SourceType);

            object value = sourceStep.GetValue();
            Assert.AreEqual("12.34It was missing", value);

            var changes = new List<object>();
            sourceStep.Changed += (sender, args) =>
            {
                changes.Add(sourceStep.GetValue());
            };

            source.DoubleProperty1 = 11.11;

            Assert.AreEqual(1, changes.Count);
            Assert.AreEqual("11.11It was missing", changes[0]);

            value = sourceStep.GetValue();
            Assert.AreEqual("11.11It was missing", value);

            source.DoubleProperty1 = 12.11;

            Assert.AreEqual(2, changes.Count);
            Assert.AreEqual("12.11It was missing", changes[1]);

            value = sourceStep.GetValue();
            Assert.AreEqual("12.11It was missing", value);

            source.SubSource = new MySubSource() {SubProperty1 = "Hello"};

            Assert.AreEqual(3, changes.Count);
            Assert.AreEqual("12.11Hello", changes[2]);

            value = sourceStep.GetValue();
            Assert.AreEqual("12.11Hello", value);
        }

        [Test]
        public void TestCombinerPropertiesMissingBinding_Part2()
        {
            var realSourceStepFactory = SetupSourceStepFactory();

            var sourceStepDescription = new MvxCombinerSourceStepDescription()
            {
                Combiner = new MvxAddValueCombiner(),
                InnerSteps = new List<MvxSourceStepDescription>()
                    {
                        new MvxPathSourceStepDescription()
                            {
                                SourcePropertyPath = "SubSource.SubProperty1",
                                FallbackValue = "It was missing"
                            },
                        new MvxPathSourceStepDescription()
                            {
                                SourcePropertyPath = "DoubleProperty1",
                            },
                    }
            };

            var source = new MySource()
            {
                DoubleProperty1 = 12.34,
            };

            var sourceStep = realSourceStepFactory.Create(sourceStepDescription);

            sourceStep.DataContext = source;

            Assert.AreEqual(typeof(object), sourceStep.SourceType);

            object value = sourceStep.GetValue();
            Assert.AreEqual("It was missing12.34", value);

            var changes = new List<object>();
            sourceStep.Changed += (sender, args) =>
            {
                changes.Add(sourceStep.GetValue());
            };

            source.DoubleProperty1 = 11.11;

            Assert.AreEqual(1, changes.Count);
            Assert.AreEqual("It was missing11.11", changes[0]);

            value = sourceStep.GetValue();
            Assert.AreEqual("It was missing11.11", value);

            source.DoubleProperty1 = 12.11;

            Assert.AreEqual(2, changes.Count);
            Assert.AreEqual("It was missing12.11", changes[1]);

            value = sourceStep.GetValue();
            Assert.AreEqual("It was missing12.11", value);

            source.SubSource = new MySubSource() { SubProperty1 = "Hello" };

            Assert.AreEqual(3, changes.Count);
            Assert.AreEqual("Hello12.11", changes[2]);

            value = sourceStep.GetValue();
            Assert.AreEqual("Hello12.11", value);
        }

        private MvxSourceStepFactory SetupSourceStepFactory()
        {
            ClearAll();
            MvxBindingSingletonCache.Initialize();

            var autoValueConverters = new MvxAutoValueConverters();
            Ioc.RegisterSingleton<IMvxAutoValueConverters>(autoValueConverters);

            var sourcePropertyParser = new MvxSourcePropertyPathParser();
            Ioc.RegisterSingleton<IMvxSourcePropertyPathParser>(sourcePropertyParser);

            var realSourceBindingFactory = new MvxSourceBindingFactory();
            Ioc.RegisterSingleton<IMvxSourceBindingFactory>(realSourceBindingFactory);

            var sourceStepFactory = new MvxSourceStepFactory();
            sourceStepFactory.AddOrOverwrite(typeof(MvxPathSourceStepDescription), new MvxPathSourceStepFactory());
            sourceStepFactory.AddOrOverwrite(typeof(MvxLiteralSourceStepDescription), new MvxLiteralSourceStepFactory());
            sourceStepFactory.AddOrOverwrite(typeof(MvxCombinerSourceStepDescription), new MvxCombinerSourceStepFactory());
            Ioc.RegisterSingleton<IMvxSourceStepFactory>(sourceStepFactory);

            var propertySource = new MvxPropertySourceBindingFactoryExtension();
            realSourceBindingFactory.Extensions.Add(propertySource);

            return sourceStepFactory;
        }
    }
}