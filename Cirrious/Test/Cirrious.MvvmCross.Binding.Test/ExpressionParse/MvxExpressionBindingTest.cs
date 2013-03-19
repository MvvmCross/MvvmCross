// MvxExpressionBindingTest.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Cirrious.CrossCore.Interfaces.Converters;
using Cirrious.MvvmCross.Binding.Binders;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Binding.Interfaces.Binders;
using Cirrious.MvvmCross.Binding.Interfaces.BindingContext;
using Cirrious.MvvmCross.Test.Core;
using Moq;
using NUnit.Framework;

namespace Cirrious.MvvmCross.Binding.Test.ExpressionParse
{
    [TestFixture]
    public class MvxExpressionBindingTest : BaseIoCSupportingTest
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
                    SourcePropertyPath = "MyCollection.GrandParent.MyChild.MyChild.Value",
                    TargetName = "Text"
                };
            Action<MockBindingContext> test = mock =>
                                              mock.Bind(mock.Target, te => te.Text,
                                                        (TestDataContext source) =>
                                                        source.MyCollection.GrandParent.MyChild.MyChild.Value);

            DoTest(test, expectedDesc);
        }

        [Test]
        public void TestExtraParameters()
        {
            var expectedDesc = new MvxBindingDescription
                {
                    SourcePropertyPath = "MyCollection.GrandParent.MyChild.MyChild.Value",
                    TargetName = "Text",
                    ConverterParameter = "My Converter Parameter",
                    Converter = new SampleValueConverter(),
                    FallbackValue = 12.3445,
                    Mode = MvxBindingMode.TwoWay
                };

            Action<MockBindingContext> test = mock =>
                                              mock.Bind(
                                                  mock.Target,
                                                  te => te.Text,
                                                  (TestDataContext source) =>
                                                  source.MyCollection.GrandParent.MyChild.MyChild.Value,
                                                  converter: new SampleValueConverter(),
                                                  converterParameter: "My Converter Parameter",
                                                  fallbackValue: 12.3445,
                                                  mode: MvxBindingMode.TwoWay);

            DoTest(test, expectedDesc);
        }

        [Test]
        public void TestExtraParametersNamedConverter()
        {
            var expectedDesc = new MvxBindingDescription
                {
                    SourcePropertyPath = "MyCollection.GrandParent.MyChild.MyChild.Value",
                    TargetName = "Text",
                    ConverterParameter = "My Converter Parameter",
                    Converter = new SampleValueConverter(),
                    FallbackValue = 12.3445,
                    Mode = MvxBindingMode.TwoWay
                };

            Action<MockBindingContext> test = mock =>
                                              mock.Bind(
                                                  mock.Target,
                                                  te => te.Text,
                                                  (TestDataContext source) =>
                                                  source.MyCollection.GrandParent.MyChild.MyChild.Value,
                                                  converterName: "SampleConv",
                                                  converterParameter: "My Converter Parameter",
                                                  fallbackValue: 12.3445,
                                                  mode: MvxBindingMode.TwoWay);

            DoTest(test, expectedDesc);
        }

        [Test]
        public void TestExtraParametersUnknownConverter()
        {
            var expectedDesc = new MvxBindingDescription
                {
                    SourcePropertyPath = "MyCollection.GrandParent.MyChild.MyChild.Value",
                    TargetName = "Text",
                    ConverterParameter = "My Converter Parameter",
                    Converter = null,
                    FallbackValue = 12.3445,
                    Mode = MvxBindingMode.TwoWay
                };

            Action<MockBindingContext> test = mock =>
                                              mock.Bind(
                                                  mock.Target,
                                                  te => te.Text,
                                                  (TestDataContext source) =>
                                                  source.MyCollection.GrandParent.MyChild.MyChild.Value,
                                                  converterName: "NoWayConv",
                                                  converterParameter: "My Converter Parameter",
                                                  fallbackValue: 12.3445,
                                                  mode: MvxBindingMode.TwoWay);

            DoTest(test, expectedDesc);
        }

        [Test]
        public void TestExtraParametersStringConverter()
        {
            var expectedDesc = new MvxBindingDescription
                {
                    SourcePropertyPath = "MyCollection.GrandParent.MyChild.MyChild.Value",
                    TargetName = "Text",
                    ConverterParameter = "My Converter Parameter",
                    Converter = new SampleValueConverter(),
                    FallbackValue = 12.3445,
                    Mode = MvxBindingMode.TwoWay
                };

            Action<MockBindingContext> test = mock =>
                                              mock.Bind(
                                                  mock.Target,
                                                  te => te.Text,
                                                  (TestDataContext source) =>
                                                  source.MyCollection.GrandParent.MyChild.MyChild.Value,
                                                  converter: new SampleValueConverter(),
                                                  converterParameter: "My Converter Parameter",
                                                  fallbackValue: 12.3445,
                                                  mode: MvxBindingMode.TwoWay);

            DoTest(test, expectedDesc);
        }

        [Test]
        public void TestNumberIndexedExpression()
        {
            var expectedDesc = new MvxBindingDescription
                {
                    SourcePropertyPath = "MyCollection.MyList[0].Value",
                    TargetName = "Text"
                };
            Action<MockBindingContext> test = mock =>
                                              mock.Bind(mock.Target, te => te.Text,
                                                        (TestDataContext source) => source.MyCollection.MyList[0].Value);

            DoTest(test, expectedDesc);
        }

        [Test]
        public void TestStringIndexedExpression()
        {
            var expectedDesc = new MvxBindingDescription
                {
                    SourcePropertyPath = "MyCollection.MyLookup[\"Fred\"].Value",
                    TargetName = "Text"
                };
            Action<MockBindingContext> test = mock =>
                                              mock.Bind(mock.Target, te => te.Text,
                                                        (TestDataContext source) =>
                                                        source.MyCollection.MyLookup["Fred"].Value);

            DoTest(test, expectedDesc);
        }

        [Test]
        public void TestDirectToObjectExpression()
        {
            var expectedDesc = new MvxBindingDescription
            {
                SourcePropertyPath = "MyCollection.MyLookup[\"Fred\"].Value",
                TargetName = "SimpleValue"
            };
            Action<MockBindingContext> test = mock =>
                                              mock.Bind(te => te.SimpleValue,
                                                        (TestDataContext source) =>
                                                        source.MyCollection.MyLookup["Fred"].Value);

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
            var dataContext = new TestDataContext();

            var bindingContext = new Mock<IMvxBindingContext>();
            bindingContext.Setup(x => x.RegisterBinding(It.IsAny<IMvxUpdateableBinding>()));
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
            Assert.AreEqual(expectedDescription.ConverterParameter, desc.ConverterParameter);
            Assert.AreEqual(expectedDescription.FallbackValue, desc.FallbackValue);
            Assert.AreEqual(expectedDescription.Mode, desc.Mode);
            Assert.AreEqual(expectedDescription.SourcePropertyPath, desc.SourcePropertyPath);
            Assert.AreEqual(expectedDescription.TargetName, desc.TargetName);
            if (expectedDescription.Converter == null)
                Assert.IsNull(desc.Converter);
            else
                Assert.AreEqual(expectedDescription.Converter.GetType(), desc.Converter.GetType());
        }
    }
}