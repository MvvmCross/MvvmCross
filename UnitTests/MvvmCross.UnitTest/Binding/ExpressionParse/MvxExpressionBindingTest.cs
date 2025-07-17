// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Globalization;
using Microsoft.Extensions.Logging;
using MvvmCross.Binding;
using MvvmCross.Binding.Binders;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Bindings;
using MvvmCross.Binding.Bindings.SourceSteps;
using MvvmCross.Binding.ExpressionParse;
using MvvmCross.Converters;
using NSubstitute;
using Xunit;

namespace MvvmCross.UnitTest.Binding.ExpressionParse
{
    [Collection("MvxTest")]
    public class MvxExpressionBindingTest
    {
        private readonly NavigationTestFixture _fixture;

        public MvxExpressionBindingTest(NavigationTestFixture fixture)
        {
            _fixture = fixture;
        }

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

        [Fact]
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

            DoTest(test, expectedDesc);
        }

        [Fact]
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

            DoTest(test, expectedDesc);
        }

        [Fact]
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

            DoTest(test, expectedDesc);
        }

        [Fact]
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

            DoTest(test, expectedDesc);
        }

        [Fact]
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

            DoTest(test, expectedDesc);
        }

        [Fact]
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

            DoTest(test, expectedDesc);
        }

        [Fact]
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

            DoTest(test, expectedDesc);
        }

        [Fact]
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

            DoTest(test, expectedDesc);
        }

        [Fact]
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

            DoTest(test, expectedDesc);
        }

        private static readonly int Zero = 0;

        [Fact]
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

        [Fact]
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

        [Fact]
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

            DoTest(test, expectedDesc);
        }

        [Fact]
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

            DoTest(test, mock => mock, expectedDesc);
        }

        private void DoTest(
            Action<MockBindingContext> action,
            MvxBindingDescription expectedDescription)
        {
            DoTest(action, (context) => context.Target, expectedDescription);
        }

        private void DoTest(
            Action<MockBindingContext> action,
            Func<MockBindingContext, object> findTargetObjectFunc,
            MvxBindingDescription expectedDescription)
        {
            _fixture.ClearAll();

            var dataContext = new TestDataContext();

            var bindingContext = Substitute.For<IMvxBindingContext>();
            bindingContext.RegisterBinding(Arg.Any<object>(), Arg.Any<IMvxUpdateableBinding>());
            bindingContext.DataContext.Returns(dataContext);

            var callbacksSeen = new List<Callback>();

            var binder = Substitute.For<IMvxBinder>();
            binder
                .When(b => b.Bind(Arg.Any<object>(), Arg.Any<object>(), Arg.Any<IEnumerable<MvxBindingDescription>>()))
                .Do(x =>
                {
                    if (x.ArgAt<IEnumerable<MvxBindingDescription>>(2).Count() != 1)
                        throw new Exception("Unexpected description count");

                    callbacksSeen.Add(new Callback
                    {
                        Source = x.ArgAt<object>(0),
                        Target = x.ArgAt<object>(1),
                        BindingDescription = x.ArgAt<IEnumerable<MvxBindingDescription>>(2).First()
                    });
                });
            _fixture.Ioc.RegisterSingleton(binder);

            var loggerFactory = Substitute.For<ILoggerFactory>();
            _fixture.Ioc.RegisterSingleton<IMvxPropertyExpressionParser>(new MvxPropertyExpressionParser(loggerFactory));

            var converterLookup = new MvxValueConverterRegistry();
            converterLookup.AddOrOverwrite("SampleConv", new SampleValueConverter());
            _fixture.Ioc.RegisterSingleton<IMvxValueConverterLookup>(converterLookup);

            var testTarget = new MockBindingContext
            {
                Target = new TestTarget(),
                BindingContext = bindingContext
            };

            action(testTarget);

            Assert.Single(callbacksSeen);
            var callback = callbacksSeen[0];
            var expectedTarget = findTargetObjectFunc(testTarget);
            Assert.Equal(expectedTarget, callback.Target);
            Assert.Equal(dataContext, callback.Source);

            var desc = callback.BindingDescription;
            Assert.True(expectedDescription.Source is MvxPathSourceStepDescription);
            var path = desc.Source as MvxPathSourceStepDescription;
            Assert.True(desc.Source is MvxPathSourceStepDescription);
            var expectedPath = expectedDescription.Source as MvxPathSourceStepDescription;
            Assert.Equal(expectedPath.ConverterParameter, path.ConverterParameter);
            Assert.Equal(expectedPath.FallbackValue, path.FallbackValue);
            Assert.Equal(expectedPath.SourcePropertyPath, path.SourcePropertyPath);
            Assert.Equal(expectedDescription.Mode, desc.Mode);
            Assert.Equal(expectedDescription.TargetName, desc.TargetName);
            if (expectedPath.Converter == null)
                Assert.Null(path.Converter);
            else
                Assert.Equal(expectedPath.Converter.GetType(), path.Converter.GetType());
        }
    }
}
