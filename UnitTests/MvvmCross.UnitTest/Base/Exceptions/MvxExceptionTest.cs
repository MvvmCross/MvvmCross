// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Exceptions;

namespace MvvmCross.UnitTest.Base.Exceptions
{
    public class MvxExceptionTest
    {
        [Test]
        public async Task MvxWrap_Is_MvxException()
        {
            var ex = new Exception("hello");
            var wrapped = ex.MvxWrap();

            await Assert.That(wrapped).IsTypeOf<MvxException>();
        }

        [Test]
        public void MvxWrap_Is_Thrown_As_MvxException()
        {
            var ex = new Exception("hello");
            var wrapped = ex.MvxWrap();

            Assert.Throws<MvxException>(() => Throw(wrapped));

            void Throw(Exception e)
            {
                throw e;
            }
        }

        [Test]
        public async Task MvxWrap_Has_InnerException()
        {
            var ex = new Exception("hello");
            var wrapped = ex.MvxWrap();

            await Assert.That(wrapped.InnerException).IsNotNull();
            await Assert.That(wrapped.InnerException).IsEqualTo(ex);
        }

        [Test]
        public async Task MvxWrap_Wraps_Only_One_Level()
        {
            var ex = new Exception("hello");
            var wrapped = ex.MvxWrap();

            for (var i = 0; i < 10; i++)
                wrapped = wrapped.MvxWrap();

            await Assert.That(wrapped.InnerException).IsEqualTo(ex);
        }
    }
}
