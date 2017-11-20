using Microsoft.CodeAnalysis;
using MvvmCross.CodeAnalysis.Analyzers;
using MvvmCross.CodeAnalysis.CodeFixes;
using MvvmCross.CodeAnalysis.Core;
using NUnit.Framework;

namespace MvvmCross.CodeAnalysis.Test
{
    [TestFixture]
    public class ApplyMustBeCalledWhenUsingFluentBindingSetAnalyzerTests : CodeFixVerifier<ApplyMustBeCalledWhenUsingFluentBindingSetAnalyzer, ApplyMustBeCalledWhenUsingFluentBindingSetCodeFix>
    {
        private const string ViewModel = @"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using MvvmCross.Core.ViewModels;

namespace IOSApp.Core.ViewModels
{
    public class FirstViewModel : MvxViewModel
    {
        private string _hello = ""Hello MvvmCross"";
        public string Hello
        {
            get { return _hello; }
            set { SetProperty(ref _hello, value); }
        }
    }
}";

        private const string GeneratedDesigner = @"// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace IOSApp.IOS.Views
{
    [Register (""FirstView"")]

    partial class FirstView
        {
            [Outlet]
            [GeneratedCode(""iOS Designer"", ""1.0"")]
            UILabel Label { get; set; }

            [Outlet]
            [GeneratedCode(""iOS Designer"", ""1.0"")]
            UITextField TextField { get; set; }

            void ReleaseDesignerOutlets()
            {
                if (Label != null)
                {
                    Label.Dispose();
                    Label = null;
                }
                if (TextField != null)
                {
                    TextField.Dispose();
                    TextField = null;
                }
            }
        }
    }
";

        private const string Expected = @"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using MvvmCross.iOS.Views;
using MvvmCross.Core.ViewModels;
using IOSApp.Core.ViewModels;

namespace IOSApp.IOS.Views
{
    public partial class FirstView : MvxViewController
    {
        public FirstView() : base(""FirstView"", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var set = this.CreateBindingSet<FirstView, Core.ViewModels.FirstViewModel>();

            set.Bind(Label).To(vm => vm.Hello);
            set.Bind(TextField).To(vm => vm.Hello);
            set.Apply();
        }
    }
}";

        private const string Test = @"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using MvvmCross.iOS.Views;
using MvvmCross.Core.ViewModels;
using IOSApp.Core.ViewModels;

namespace IOSApp.IOS.Views
{
    public partial class FirstView : MvxViewController
    {
        public FirstView() : base(""FirstView"", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var set = this.CreateBindingSet<FirstView, Core.ViewModels.FirstViewModel>();

            set.Bind(Label).To(vm => vm.Hello);
            set.Bind(TextField).To(vm => vm.Hello);
        }
    }
}";

        [Test]
        public void ApplyMustBeCalledWhenUsingFluentBindingSetShoudShowOneDiagnosticIfApplyIsNotCalled()
        {
            var expectedDiagnostic = new DiagnosticResult
            {
                Id = DiagnosticIds.ApplyMustBeCalledWhenUsingFluentBindingSetId,
                Message = "Call set.Apply() method to apply your bindings",
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[]
                    {
                        new DiagnosticResultLocation("Test0.cs", 23, 17)
                    }
            };

            var project = new[]
            {
                new MvxTestFileSource(Test, MvxProjType.Ios),
                new MvxTestFileSource(GeneratedDesigner, MvxProjType.Ios), 
                new MvxTestFileSource(ViewModel, MvxProjType.Core)
            };

            VerifyCSharpDiagnostic(project, expectedDiagnostic);
        }

        [Test]
        public void ApplyMustBeCalledWhenUsingFluentBindingSetShoudShowNoDiagnosticIfApplyIsCalled()
        {
            var project = new[]
            {
                new MvxTestFileSource(Expected, MvxProjType.Ios),
                new MvxTestFileSource(GeneratedDesigner, MvxProjType.Ios),
                new MvxTestFileSource(ViewModel, MvxProjType.Core)
            };

            VerifyCSharpDiagnostic(project);
        }

        [Test]
        public void ApplyMustBeCalledWhenUsingFluentBindingSetAnalyzerShouldFixTheCode()
        {
            var solution = new[]
            {
                new MvxTestFileSource(GeneratedDesigner, MvxProjType.Ios),
                new MvxTestFileSource(ViewModel, MvxProjType.Core)
            };
            VerifyCSharpFix(solution, Test, MvxProjType.Ios, Expected);
        }
    }
}