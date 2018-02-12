﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Test;
using Xunit;

namespace MvvmCross.Plugins.Visibility.UnitTest
{
    [CollectionDefinition("Visibility")]
    public class VisibilityCollection : ICollectionFixture<MvxTestFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
