using Microsoft.CodeAnalysis;
using MvvmCross.CodeAnalysis.Analyzers;
using MvvmCross.CodeAnalysis.CodeFixes;
using MvvmCross.CodeAnalysis.Core;
using NUnit.Framework;

namespace MvvmCross.CodeAnalysis.Test
{
    [TestFixture]
    public class ConsiderUsingGenericBaseViewAnalyzerTests : CodeFixVerifier<ConsiderUsingGenericBaseViewAnalyzer, ConsiderUsingGenericBaseViewCodeFix>
    {
        private const string ViewModel = @"
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
    public class FirstViewModel : MvxViewModel { }
}";

        private const string Expected = @"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using MvvmCross.Droid.Views;
using MvvmCross.Core.ViewModels;
using AndroidApp.Core.ViewModels;

namespace AndroidApp.Droid
{
    [Activity(FirstActivity)]
    class FirstView : MvxActivity<FirstViewModel>
    {
    }
}";

        private const string Test = @"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using MvvmCross.Droid.Views;
using MvvmCross.Core.ViewModels;
using AndroidApp.Core.ViewModels;

namespace AndroidApp.Droid
{
    [Activity(FirstActivity)]
    class FirstView : MvxActivity
    {
        public new FirstViewModel ViewModel => base.ViewModel as FirstViewModel;
    }
}";

        [Test]
        public void ConsiderUsingGenericBaseViewAnalyzerShouldShowOneDiagnostic()
        {
            var expectedDiagnostic = new DiagnosticResult
            {
                Id = DiagnosticIds.UseGenericBaseClassRuleId,
                Message = "Derive from 'MvxActivity<T>', which already implements a strongly typed ViewModel property",
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[]
                    {
                        new DiagnosticResultLocation("Test0.cs", 15, 23)
                    }
            };

            VerifyCSharpDiagnostic(
                new[]
                {
                    new MvxTestFileSource(Test, MvxProjType.Droid),
                    new MvxTestFileSource(ViewModel, MvxProjType.Core)
                },
                expectedDiagnostic);
        }

        [Test]
        public void ConsiderUsingGenericBaseViewAnalyzerShouldFixTheCode()
        {
            var solution = new[]
            {
                new MvxTestFileSource(ViewModel, MvxProjType.Core)
            };
            VerifyCSharpFix(solution, Test, MvxProjType.Droid, Expected);
        }
    }
}