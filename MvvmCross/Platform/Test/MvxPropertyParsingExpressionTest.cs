// MvxPropertyNameFromExpressionTests.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Platform.Test
{
    using MvvmCross.Platform.Core;

    using NUnit.Framework;

    [TestFixture]
    public class MvxPropertyNameFromExpressionTests
    {
        public class TestClass
        {
            public string Foo { get; set; }

            public string GetFooExpression()
            {
                return this.GetPropertyNameFromExpression(() => this.Foo);
            }
        }

        [Test]
        public void TestPropertyExpression()
        {
            var t = new TestClass();
            var result = t.GetFooExpression();
            Assert.AreEqual("Foo", result);
        }

        [Test]
        public void TestUnaryPropertyExpression()
        {
            var t = new TestClass();
            var result = t.GetPropertyNameFromExpression(() => t.Foo);
            Assert.AreEqual("Foo", result);
        }
    }
}