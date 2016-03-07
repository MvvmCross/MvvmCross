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
        private const string Expected = @"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using MvvmCross.Droid.Views;
using MvvmCross.Core.ViewModels;

namespace AndroidApp
{
    class FirstViewModel : MvxViewModel { }
    
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

namespace AndroidApp
{
    class FirstViewModel : MvxViewModel { }
    
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
                        new DiagnosticResultLocation("Test0.cs", 16, 23)
                    }
            };

            VerifyCSharpDiagnostic(Test, MvxProjType.Droid, expectedDiagnostic);
        }

        [Test]
        public void ConsiderUsingGenericBaseViewAnalyzerShouldFixTheCode()
        {
            VerifyCSharpFix(Test, MvxProjType.Droid, Expected);
        }
    }
}