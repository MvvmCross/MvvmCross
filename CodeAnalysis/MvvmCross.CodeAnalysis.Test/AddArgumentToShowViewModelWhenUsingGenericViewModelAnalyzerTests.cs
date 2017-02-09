using Microsoft.CodeAnalysis;
using MvvmCross.CodeAnalysis.Analyzers;
using MvvmCross.CodeAnalysis.Core;
using NUnit.Framework;

namespace MvvmCross.CodeAnalysis.Test
{
    [TestFixture]
    public class AddArgumentToShowViewModelWhenUsingGenericViewModelAnalyzerTests : DiagnosticVerifier<AddArgumentToShowViewModelWhenUsingGenericViewModelAnalyzer>
    {
        private const string Test1 = @"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using MvvmCross.Droid.Views;
using MvvmCross.Core.ViewModels;

namespace AndroidApp.Core.ViewModels
{
    public class FirstViewModel : MvxViewModel
    {
        public void NavigateToOtherVM()
        {
            ShowViewModel<SecondViewModel>();
        }
    }
    public class SecondViewModel : MvxViewModel<int> { }
}";

        private const string CorrectCode1 = @"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using MvvmCross.Droid.Views;
using MvvmCross.Core.ViewModels;

namespace AndroidApp.Core.ViewModels
{
    public class FirstViewModel : MvxViewModel
    {
        public void NavigateToOtherVM()
        {
            ShowViewModel<SecondViewModel>();
        }
    }
    public class SecondViewModel : MvxViewModel { }
}";

        private const string CorrectCode2 = @"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using MvvmCross.Droid.Views;
using MvvmCross.Core.ViewModels;

namespace AndroidApp.Core.ViewModels
{
    public class FirstViewModel : MvxViewModel
    {
        public void NavigateToOtherVM()
        {
            ShowViewModel<SecondViewModel>(1);
        }
    }
    public class SecondViewModel : MvxViewModel<int> { }
}";

        private const string Test2 = @"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using MvvmCross.Droid.Views;
using MvvmCross.Core.ViewModels;

namespace AndroidApp.Core.ViewModels
{
    public class FirstViewModel : MvxViewModel
    {
        public void NavigateToOtherVM()
        {
            ShowViewModel<SecondViewModel>(""a"");
        }
    }
    public class SecondViewModel : MvxViewModel<int> { }
}";

        private const string Test3 = @"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using MvvmCross.Droid.Views;
using MvvmCross.Core.ViewModels;

namespace AndroidApp.Core.ViewModels
{
    public class FirstViewModel : MvxViewModel
    {
        public void NavigateToOtherVM()
        {
            ShowViewModel<SecondViewModel>();
        }
    }
    public class BaseViewModel : MvxViewModel<int> { }
    public class SecondViewModel : BaseViewModel { }
}";

        private const string CorrectCode3 = @"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using MvvmCross.Droid.Views;
using MvvmCross.Core.ViewModels;

namespace AndroidApp.Core.ViewModels
{
    public class FirstViewModel : MvxViewModel
    {
        public void NavigateToOtherVM()
        {
            ShowViewModel<SecondViewModel>();
        }
    }
    public class BaseViewModel : MvxViewModel { }
    public class SecondViewModel : BaseViewModel { }
}";

        [Test]
        public void AddArgumentToShowViewModelWhenUsingGenericViewModelAnalyzerShouldShowOneDiagnostic()
        {
            var expectedDiagnostic = new DiagnosticResult
            {
                Id = DiagnosticIds.AddArgumentToShowViewModelWhenUsingGenericViewModelId,
                Message =
                    "When calling 'ShowViewModel<T>()', if T inherits from the generic 'MvxViewModel<TU>', then the method call to ShowViewModel should be passing a TU argument",
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[]
                    {
                        new DiagnosticResultLocation("Test0.cs", 17, 13)
                    }
            };

            VerifyCSharpDiagnostic(
                new[]
                {
                    new MvxTestFileSource(Test1, MvxProjType.Core)
                },
                expectedDiagnostic);
        }

        [Test]
        public void AddArgumentToShowViewModelWhenUsingGenericViewModelAnalyzerShouldShowNotShowAnyDiagnosticsIfNonGenericViewModel()
        {
            VerifyCSharpDiagnostic(
                new[]
                {
                    new MvxTestFileSource(CorrectCode1, MvxProjType.Core)
                });
        }

        [Test]
        public void AddArgumentToShowViewModelWhenUsingGenericViewModelAnalyzerShouldShowNotShowAnyDiagnosticsIfAlreadyWithParameter()
        {
            VerifyCSharpDiagnostic(
                new[]
                {
                    new MvxTestFileSource(CorrectCode2, MvxProjType.Core)
                });
        }

        [Test]
        public void AddArgumentToShowViewModelWhenUsingGenericViewModelAnalyzerShouldShowOneDiagnosticIfArgumentTypeIsMismatch()
        {
            var expectedDiagnostic = new DiagnosticResult
            {
                Id = DiagnosticIds.AddArgumentToShowViewModelWhenUsingGenericViewModelId,
                Message =
                    "When calling 'ShowViewModel<T>()', if T inherits from the generic 'MvxViewModel<TU>', then the method call to ShowViewModel should be passing a TU argument",
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[]
                    {
                        new DiagnosticResultLocation("Test0.cs", 17, 13)
                    }
            };

            VerifyCSharpDiagnostic(
                new[]
                {
                    new MvxTestFileSource(Test2, MvxProjType.Core)
                },
                expectedDiagnostic);
        }

        [Test]
        public void AddArgumentToShowViewModelWhenUsingGenericViewModelAnalyzerShouldNotShowAnyDiagnosticsIfAlreadyWithParameterAndInherited()
        {
            var expectedDiagnostic = new DiagnosticResult
            {
                Id = DiagnosticIds.AddArgumentToShowViewModelWhenUsingGenericViewModelId,
                Message =
                    "When calling 'ShowViewModel<T>()', if T inherits from the generic 'MvxViewModel<TU>', then the method call to ShowViewModel should be passing a TU argument",
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[]
                    {
                        new DiagnosticResultLocation("Test0.cs", 17, 13)
                    }
            };

            VerifyCSharpDiagnostic(
                new[]
                {
                    new MvxTestFileSource(Test3, MvxProjType.Core)
                },
                expectedDiagnostic);
        }

        [Test]
        public void AddArgumentToShowViewModelWhenUsingGenericViewModelAnalyzerShouldNotShowAnyDiagnosticsIfInheritedFromNonGeneric()
        {
            VerifyCSharpDiagnostic(
                new[]
                {
                    new MvxTestFileSource(CorrectCode3, MvxProjType.Core)
                });
        }
    }
}
