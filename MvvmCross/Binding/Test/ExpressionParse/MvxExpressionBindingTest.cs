// MvxExpressionBindingTest.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Test.ExpressionParse
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using Moq;

    using MvvmCross.Binding.Binders;
    using MvvmCross.Binding.BindingContext;
    using MvvmCross.Binding.Bindings;
    using MvvmCross.Binding.Bindings.SourceSteps;
    using MvvmCross.Binding.ExpressionParse;
    using MvvmCross.Platform.Converters;
    using MvvmCross.Test.Core;

    using NUnit.Framework;

    [TestFixture]
    public class MvxExpressionBindingTest : MvxIoCSupportingTest
    {
        public class Child
        {
            public string Value { get; set; }
        }

        public class Parent
        {
            public Child MyChild { get; set; }
        }

        public class GrandParent
        {
            public Parent MyChild { get; set; }
        }

        public class CollectionClass
        {
            public List<Child> MyList { get; set; }
            public Dictionary<string, Child> MyLookup { get; set; }
            public GrandParent GrandParent { get; set; }
        }

        public class TestDataContext
        {
            public CollectionClass MyCollection { get; set; }
        }

        public class TestUnderscoreDataContext
        {
            public CollectionClass MyCollection { get; set; }
        }

        public class SampleValueConverter : IMvxValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        public class TestTarget
        {
            public string Text { get; set; }
        }

        public class Callback
        {
            public object Target { get; set; }
            public object Source { get; set; }
            public MvxBindingDescription BindingDescription { get; set; }
        }

        public class MockBindingContext : IMvxBindingContextOwner
        {
            public string SimpleValue { get; set; }
            public TestTarget Target { get; set; }
            public IMvxBindingContext BindingContext { get; set; }
        }

        [Test]
        public void TestLongExpression()
        {
            var expectedDesc = new MvxBindingDescription
            {
                Source = new MvxPathSourceStepDescription()
                {
                    SourcePropertyPath = "MyCollection.GrandParent.MyChild.MyChild.Value",
                },
                TargetName = "Text"
            };
            Action<MockBindingContext> test = mock =>
                mock
                    .CreateBinding(mock.Target)
                    .For(te => te.Text)
                    .To<TestDataContext>(source => source.MyCollection.GrandParent.MyChild.MyChild.Value)
                    .Apply();

            this.DoTest(test, expectedDesc);
        }

        [Test]
        public void TestLongExpressionWithUnderscores()
        {
            var expectedDesc = new MvxBindingDescription
            {
                Source = new MvxPathSourceStepDescription()
                {
                    SourcePropertyPath = "MyCollection.GrandParent.MyChild.MyChild.Value",
                },
                TargetName = "Text"
            };
            Action<MockBindingContext> test = mock =>
                mock
                    .CreateBinding(mock.Target)
                    .For(te => te.Text)
                    .To<TestUnderscoreDataContext>(source => source.MyCollection.GrandParent.MyChild.MyChild.Value)
                    .Apply();

            this.DoTest(test, expectedDesc);
        }

        [Test]
        public void TestExtraParameters()
        {
            var expectedDesc = new MvxBindingDescription
            {
                Source = new MvxPathSourceStepDescription()
                {
                    SourcePropertyPath = "MyCollection.GrandParent.MyChild.MyChild.Value",
                    ConverterParameter = "My Converter Parameter",
                    Converter = new SampleValueConverter(),
                    FallbackValue = 12.3445,
                },
                TargetName = "Text",
                Mode = MvxBindingMode.TwoWay
            };

            Action<MockBindingContext> test = mock =>
                mock
                    .CreateBinding(mock.Target)
                    .For(te => te.Text)
                    .To<TestDataContext>(source => source.MyCollection.GrandParent.MyChild.MyChild.Value)
                    .WithConversion(new SampleValueConverter(), "My Converter Parameter")
                    .WithFallback(12.3445)
                    .TwoWay()
                    .Apply();

            this.DoTest(test, expectedDesc);
        }

        [Test]
        public void TestExtraParametersNamedConverter()
        {
            var expectedDesc = new MvxBindingDescription
            {
                Source = new MvxPathSourceStepDescription()
                {
                    SourcePropertyPath = "MyCollection.GrandParent.MyChild.MyChild.Value",
                    ConverterParameter = "My Converter Parameter",
                    Converter = new SampleValueConverter(),
                    FallbackValue = 12.3445,
                },
                TargetName = "Text",
                Mode = MvxBindingMode.TwoWay
            };

            Action<MockBindingContext> test = mock =>
                mock
                    .CreateBinding(mock.Target)
                    .For(te => te.Text)
                    .To<TestDataContext>(source => source.MyCollection.GrandParent.MyChild.MyChild.Value)
                    .WithConversion("SampleConv", "My Converter Parameter")
                    .WithFallback(12.3445)
                    .TwoWay()
                    .Apply();

            this.DoTest(test, expectedDesc);
        }

        [Test]
        public void TestExtraParametersUnknownConverter()
        {
            var expectedDesc = new MvxBindingDescription
            {
                Source = new MvxPathSourceStepDescription()
                {
                    SourcePropertyPath = "MyCollection.GrandParent.MyChild.MyChild.Value",
                    ConverterParameter = "My Converter Parameter",
                    Converter = null,
                    FallbackValue = 12.3445,
                },
                TargetName = "Text",
                Mode = MvxBindingMode.TwoWay
            };

            Action<MockBindingContext> test = mock =>
                mock
                    .CreateBinding(mock.Target)
                    .For(te => te.Text)
                    .To<TestDataContext>(source => source.MyCollection.GrandParent.MyChild.MyChild.Value)
                    .WithConversion("NoWayConv", "My Converter Parameter")
                    .WithFallback(12.3445)
                    .TwoWay()
                    .Apply();

            this.DoTest(test, expectedDesc);
        }

        [Test]
        public void TestExtraParametersStringConverter()
        {
            var expectedDesc = new MvxBindingDescription
            {
                TargetName = "Text",
                Source = new MvxPathSourceStepDescription()
                {
                    SourcePropertyPath = "MyCollection.GrandParent.MyChild.MyChild.Value",
                    ConverterParameter = "My Converter Parameter",
                    Converter = new SampleValueConverter(),
                    FallbackValue = 12.3445,
                },
                Mode = MvxBindingMode.TwoWay
            };

            Action<MockBindingContext> test = mock =>
                mock
                    .CreateBinding(mock.Target)
                    .For(te => te.Text)
                    .To<TestDataContext>(source => source.MyCollection.GrandParent.MyChild.MyChild.Value)
                    .WithConversion(new SampleValueConverter(), "My Converter Parameter")
                    .WithFallback(12.3445)
                    .TwoWay()
                    .Apply();

            this.DoTest(test, expectedDesc);
        }

        [Test]
        public void TestNumberIndexedExpression()
        {
            var expectedDesc = new MvxBindingDescription
            {
                Source = new MvxPathSourceStepDescription()
                {
                    SourcePropertyPath = "MyCollection.MyList[0].Value",
                },
                TargetName = "Text"
            };
            Action<MockBindingContext> test = mock =>
                mock
                    .CreateBinding(mock.Target)
                    .For(te => te.Text)
                    .To<TestDataContext>(source => source.MyCollection.MyList[0].Value)
                    .Apply();

            this.DoTest(test, expectedDesc);
        }

        [Test]
        public void TestStringIndexedExpression()
        {
            var expectedDesc = new MvxBindingDescription
            {
                Source = new MvxPathSourceStepDescription()
                {
                    SourcePropertyPath = "MyCollection.MyLookup[\"Fred\"].Value",
                },
                TargetName = "Text"
            };
            Action<MockBindingContext> test = mock =>
                mock
                    .CreateBinding(mock.Target)
                    .For(te => te.Text)
                    .To<TestDataContext>(source => source.MyCollection.MyLookup["Fred"].Value)
                    .Apply();

            this.DoTest(test, expectedDesc);
        }

        [Test]
        public void TestNumberVariableIndexedExpression()
        {
            var expectedDesc = new MvxBindingDescription
            {
                Source = new MvxPathSourceStepDescription()
                {
                    SourcePropertyPath = "MyCollection.MyList[0].Value",
                },
                TargetName = "Text"
            };
            var index = 0;
            Action<MockBindingContext> test = mock =>
                mock
                    .CreateBinding(mock.Target)
                    .For(te => te.Text)
                    .To<TestDataContext>(source => source.MyCollection.MyList[index].Value)
                    .Apply();

            this.DoTest(test, expectedDesc);
        }

        private static readonly int Zero = 0;

        [Test]
        public void TestNumberStaticVariableIndexedExpression()
        {
            var expectedDesc = new MvxBindingDescription
            {
                Source = new MvxPathSourceStepDescription()
                {
                    SourcePropertyPath = "MyCollection.MyList[0].Value",
                },
                TargetName = "Text"
            };
            Action<MockBindingContext> test = mock =>
                mock
                .CreateBinding(mock.Target)
                .For(te => te.Text)
                .To<TestDataContext>(source => source.MyCollection.MyList[Zero].Value)
                .Apply();

            DoTest(test, expectedDesc);
        }

        [Test]
        public void TestNumberPropertyIndexedExpression()
        {
            var expectedDesc = new MvxBindingDescription
            {
                Source = new MvxPathSourceStepDescription()
                {
                    SourcePropertyPath = "MyCollection.MyList[0].Value",
                },
                TargetName = "Text"
            };
            var obj = new { Index = 0 };
            Action<MockBindingContext> test = mock =>
                mock
                .CreateBinding(mock.Target)
                .For(te => te.Text)
                .To<TestDataContext>(source => source.MyCollection.MyList[obj.Index].Value)
                .Apply();

            DoTest(test, expectedDesc);
        }

        [Test]
        public void TestStringVariableIndexedExpression()
        {
            var expectedDesc = new MvxBindingDescription
            {
                Source = new MvxPathSourceStepDescription()
                {
                    SourcePropertyPath = "MyCollection.MyLookup[\"Fred\"].Value",
                },
                TargetName = "Text"
            };
            var index = "Fred";
            Action<MockBindingContext> test = mock =>
                mock
                    .CreateBinding(mock.Target)
                    .For(te => te.Text)
                    .To<TestDataContext>(source => source.MyCollection.MyLookup[index].Value)
                    .Apply();

            this.DoTest(test, expectedDesc);
        }

        [Test]
        public void TestDirectToObjectExpression()
        {
            var expectedDesc = new MvxBindingDescription
            {
                Source = new MvxPathSourceStepDescription()
                {
                    SourcePropertyPath = "MyCollection.MyLookup[\"Fred\"].Value",
                },
                TargetName = "SimpleValue"
            };
            Action<MockBindingContext> test = mock =>
                mock
                    .CreateBinding()
                    .For(te => te.SimpleValue)
                    .To<TestDataContext>(source => source.MyCollection.MyLookup["Fred"].Value)
                    .Apply();

            this.DoTest(test, mock => mock, expectedDesc);
        }

        private void DoTest(
            Action<MockBindingContext> action,
            MvxBindingDescription expectedDescription)
        {
            this.DoTest(action, (context) => context.Target, expectedDescription);
        }

        private void DoTest(
            Action<MockBindingContext> action,
            Func<MockBindingContext, object> findTargetObjectFunc,
            MvxBindingDescription expectedDescription)
        {
            ClearAll();
            MvxBindingSingletonCache.Initialize();

            var dataContext = new TestDataContext();

            var bindingContext = new Mock<IMvxBindingContext>();
            bindingContext.Setup(x => x.RegisterBinding(It.IsAny<object>(), It.IsAny<IMvxUpdateableBinding>()));
            bindingContext.SetupGet(x => x.DataContext).Returns(dataContext);

            var callbacksSeen = new List<Callback>();

            var binder = new Mock<IMvxBinder>();
            binder.Setup(
                b => b.Bind(It.IsAny<object>(), It.IsAny<object>(), It.IsAny<IEnumerable<MvxBindingDescription>>()))
                  .Callback((object source, object target, IEnumerable<MvxBindingDescription> descriptions) =>
                      {
                          if (descriptions.Count() != 1)
                              throw new Exception("Unexpected description count");

                          callbacksSeen.Add(new Callback
                          {
                              Source = source,
                              Target = target,
                              BindingDescription = descriptions.First()
                          });
                      });
            Ioc.RegisterSingleton(binder.Object);

            Ioc.RegisterSingleton<IMvxPropertyExpressionParser>(new MvxPropertyExpressionParser());

            var converterLookup = new MvxValueConverterRegistry();
            converterLookup.AddOrOverwrite("SampleConv", new SampleValueConverter());
            Ioc.RegisterSingleton<IMvxValueConverterLookup>(converterLookup);

            var testTarget = new MockBindingContext
            {
                Target = new TestTarget(),
                BindingContext = bindingContext.Object
            };

            action(testTarget);

            Assert.AreEqual(1, callbacksSeen.Count);
            var callback = callbacksSeen[0];
            var expectedTarget = findTargetObjectFunc(testTarget);
            Assert.AreEqual(expectedTarget, callback.Target);
            Assert.AreEqual(dataContext, callback.Source);

            var desc = callback.BindingDescription;
            Assert.IsTrue(expectedDescription.Source is MvxPathSourceStepDescription);
            var path = desc.Source as MvxPathSourceStepDescription;
            Assert.IsTrue(desc.Source is MvxPathSourceStepDescription);
            var expectedPath = expectedDescription.Source as MvxPathSourceStepDescription;
            Assert.AreEqual(expectedPath.ConverterParameter, path.ConverterParameter);
            Assert.AreEqual(expectedPath.FallbackValue, path.FallbackValue);
            Assert.AreEqual(expectedPath.SourcePropertyPath, path.SourcePropertyPath);
            Assert.AreEqual(expectedDescription.Mode, desc.Mode);
            Assert.AreEqual(expectedDescription.TargetName, desc.TargetName);
            if (expectedPath.Converter == null)
                Assert.IsNull(path.Converter);
            else
                Assert.AreEqual(expectedPath.Converter.GetType(), path.Converter.GetType());
        }
    }
}