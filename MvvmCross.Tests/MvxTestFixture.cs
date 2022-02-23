// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;

namespace MvvmCross.Tests
{
    public class MvxTestFixture : MvxIoCSupportingTest, IDisposable
    {
        public MvxTestFixture()
        {
            Setup();
        }

        ~MvxTestFixture()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Reset();
            }
        }
    }
}
