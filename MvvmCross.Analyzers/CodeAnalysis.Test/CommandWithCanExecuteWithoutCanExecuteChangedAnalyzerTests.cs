using Microsoft.CodeAnalysis;
using MvvmCross.CodeAnalysis.Analyzers;
using MvvmCross.CodeAnalysis.CodeFixes;
using MvvmCross.CodeAnalysis.Core;
using NUnit.Framework;

namespace MvvmCross.CodeAnalysis.Test
{
    [TestFixture]
    public class CommandWithCanExecuteWithoutCanExecuteChangedAnalyzerTests : CodeFixVerifier<CommandWithCanExecuteWithoutCanExecuteChangedAnalyzer, CommandWithCanExecuteWithoutCanExecuteChangedCodeFix>
    {
        private const string Expected = @"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;

namespace CoreApp
{
    public class FirstViewModel : MvxViewModel
    {
        public bool ShouldSubmit { get; set; }

        private MvxCommand _submitCommand;
        public MvxCommand SubmitCommand
        {
            get
            {
                _submitCommand = _submitCommand ?? new MvxCommand(DoSubmit);
                return _submitCommand;
            }
        }

        private void DoSubmit() { }

        private bool CanDoSubmit()
        {
            return ShouldSubmit;
        }
    }
}";

        private const string TestWithLazyLoading = @"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;

namespace CoreApp
{
    public class FirstViewModel : MvxViewModel
    {
        public bool ShouldSubmit { get; set; }

        private MvxCommand _submitCommand;
        public MvxCommand SubmitCommand
        {
            get
            {
                _submitCommand = _submitCommand ?? new MvxCommand(DoSubmit, CanDoSubmit);
                return _submitCommand;
            }
        }

        private void DoSubmit() { }

        private bool CanDoSubmit()
        {
            return ShouldSubmit;
        }
    }
}";

        private const string TestWithProperty = @"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;

namespace CoreApp
{
    public class FirstViewModel : MvxViewModel
    {
        public bool ShouldSubmit { get; set; }

        public MvxCommand SubmitCommand { get; set; }
        
        public FirstViewModel()
        {
            SubmitCommand = new MvxCommand(DoSubmit, CanDoSubmit);
        }
        
        private void DoSubmit() { }

        private bool CanDoSubmit()
        {
            return ShouldSubmit;
        }
    }
}";

        private const string TestWithPropertyButCalling = @"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;

namespace CoreApp
{
    public class FirstViewModel : MvxViewModel
    {
        private bool _shouldSubmit;
        public bool ShouldSubmit
        {
            get
            {
                return _shouldSubmit;
            }
            set
            {
                _shouldSubmit = value;
                SubmitCommand.RaiseCanExecuteChanged()
            }
        }

        public MvxCommand SubmitCommand { get; set; }
        
        public FirstViewModel()
        {
            SubmitCommand = new MvxCommand(DoSubmit, CanDoSubmit);
        }
        
        private void DoSubmit() { }

        private bool CanDoSubmit()
        {
            return ShouldSubmit;
        }
    }
}";

        private const string TestWithPropertyButCallingWithElvis = @"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;

namespace CoreApp
{
    public class FirstViewModel : MvxViewModel
    {
        private bool _shouldSubmit;
        public bool ShouldSubmit
        {
            get
            {
                return _shouldSubmit;
            }
            set
            {
                _shouldSubmit = value;
                SubmitCommand?.RaiseCanExecuteChanged()
            }
        }

        public MvxCommand SubmitCommand { get; set; }
        
        public FirstViewModel()
        {
            SubmitCommand = new MvxCommand(DoSubmit, CanDoSubmit);
        }
        
        private void DoSubmit() { }

        private bool CanDoSubmit()
        {
            return ShouldSubmit;
        }
    }
}";

        private const string TestWithPropertyCreatedWithCs6 = @"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;

namespace CoreApp
{
    public class FirstViewModel : MvxViewModel
    {
        public bool ShouldSubmit { get; set; }

        public MvxCommand SubmitCommand => new MvxCommand(DoSubmit, CanDoSubmit);

        public FirstViewModel()
        {
        }

        private void DoSubmit() { }

        private bool CanDoSubmit()
        {
            return ShouldSubmit;
        }
    }
}";

        [Test]
        public void CommandWithCanExecuteWithoutCanExecuteChangedAnalyzerShouldShowOneDiagnostic()
        {
            var expectedDiagnostic = new DiagnosticResult
            {
                Id = DiagnosticIds.CommandWithCanExecuteWithoutCanExecuteChangedRuleId,
                Message = "Remove 'CanDoSubmit' from the constructor or call 'SubmitCommand.RaiseCanExecuteChanged' from your code",
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[]
                    {
                        new DiagnosticResultLocation("Test0.cs", 21, 54)
                    }
            };

            VerifyCSharpDiagnostic(TestWithProperty, MvxProjType.Core, expectedDiagnostic);
        }

        [TestCase(TestWithPropertyButCalling)]
        [TestCase(TestWithPropertyButCallingWithElvis)]
        [Test]
        public void CommandWithCanExecuteWithoutCanExecuteChangedAnalyzerShouldNotShowAnyDiagnostic(string testCase)
        {
            VerifyCSharpDiagnostic(testCase, MvxProjType.Core);
        }

        [Test]
        public void CommandWithCanExecuteWithCs6WithoutCanExecuteChangedAnalyzerShouldShowOneDiagnostic()
        {
            var expectedDiagnostic = new DiagnosticResult
            {
                Id = DiagnosticIds.CommandWithCanExecuteWithoutCanExecuteChangedRuleId,
                Message = "Remove 'CanDoSubmit' from the constructor or call 'SubmitCommand.RaiseCanExecuteChanged' from your code",
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[]
                    {
                        new DiagnosticResultLocation("Test0.cs", 17, 69)
                    }
            };

            VerifyCSharpDiagnostic(TestWithPropertyCreatedWithCs6, MvxProjType.Core, expectedDiagnostic);
        }

        [Test]
        public void CommandWithCanExecuteWithoutCanExecuteChangedAnalyzerWithLazyLoadingShouldShowOneDiagnostic()
        {
            var expectedDiagnostic = new DiagnosticResult
            {
                Id = DiagnosticIds.CommandWithCanExecuteWithoutCanExecuteChangedRuleId,
                Message = "Remove 'CanDoSubmit' from the constructor or call 'SubmitCommand.RaiseCanExecuteChanged' from your code",
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[]
                    {
                        new DiagnosticResultLocation("Test0.cs", 22, 77)
                    }
            };

            VerifyCSharpDiagnostic(TestWithLazyLoading, MvxProjType.Core, expectedDiagnostic);
        }

        [Test]
        public void CommandWithCanExecuteWithoutCanExecuteChangedAnalyzerShouldFixTheCode()
        {
            VerifyCSharpFix(TestWithLazyLoading, MvxProjType.Core, Expected);
        }
    }
}