// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Windows.Foundation;

namespace MvvmCross.Platforms.Uap
{
    public static class MvxPseudoAsyncExtensions
    {
        public static void Await(this IAsyncAction operation)
        {
            try
            {
                var task = operation.AsTask();
                task.Wait();
            }
            catch (AggregateException exception)
            {
                // TODO - this possibly oversimplifies the problem report
                throw exception.InnerException;
            }
        }

        public static TResult Await<TResult>(this IAsyncOperation<TResult> operation)
        {
            try
            {
                var task = operation.AsTask();
                task.Wait();
                return task.Result;
            }
            catch (AggregateException exception)
            {
                // TODO - this possibly oversimplifies the problem report
                throw exception.InnerException;
            }
        }
    }
}
