// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.CodeAnalysis.Core
{
    public static class DiagnosticIds
    {
        public const string UseGenericBaseClassRuleId = "MVX1001";
        public const string CommandWithCanExecuteWithoutCanExecuteChangedRuleId = "MVX1002";
        public const string MvxMessengerSubscriptionDoesntStoreTokenInVariableId = "MVX1003";
        public const string ApplyMustBeCalledWhenUsingFluentBindingSetId = "MVX1004";
        public const string AddArgumentToShowViewModelWhenUsingGenericViewModelId = "MVX1005";
    }
}
