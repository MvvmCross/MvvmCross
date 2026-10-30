// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Base;

namespace MvvmCross.UnitTest.Base
{

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
        public async Task TestPropertyExpression()
        {
            var t = new TestClass();
            var result = t.GetFooExpression();
            await Assert.That(result).IsEqualTo("Foo");
        }

        [Test]
        public async Task TestUnaryPropertyExpression()
        {
            var t = new TestClass();
            var result = t.GetPropertyNameFromExpression(() => t.Foo);
            await Assert.That(result).IsEqualTo("Foo");
        }
    }
}
