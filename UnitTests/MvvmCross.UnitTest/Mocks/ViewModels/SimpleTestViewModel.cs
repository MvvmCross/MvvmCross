// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Core.ViewModels;

namespace MvvmCross.UnitTest.Mocks.ViewModels
{
    public class SimpleTestViewModel : MvxViewModel
    {
        public virtual void Init()
        {
        }

        public virtual void Init(string hello)
        {
        }
    }
}
