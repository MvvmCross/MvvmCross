// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Platform.Core;
using NUnit.Framework;

namespace MvvmCross.Platform.Test
{
    [TestFixture]
    public class MvxPropertyNameFromExpressionTests
    {
        public class TestClass
        {
            public string Foo { get; set; }

            public string GetFooExpression()
            {
                return this.GetPropertyNameFromExpression(() => Foo);
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