// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using TUnit.Core;

// Fixtures reset process-wide MvxHost/binding state and culture. GC-based lifetime
// tests also require isolation from other tests in the same process.
[assembly: NotInParallel]
