---
layout: documentation
title: Code Analysis
category: Fundamentals
---
# Background

With the release of C# version 6 Microsoft has rewritten their C# and VB compilers from the ground up. They are known as the Microsoft .NET Compiler Platform (aka Roslyn). These compilers provide library builders to provide the compiler with a set of so-called Analyzers and CodeFixes. These can be shipped through a nuget package, and will light up in an IDE with support for these Roslyn based compilers.

# What is this (aka TLDR)?

[Pull-request #1117](https://github.com/MvvmCross/MvvmCross/pull/1117) is the first attempt to introduce such analyzers and code fix providers for the MvvmCross library. This fixes #1040.

# What does it do?

In short it helps developers use the MvvmCross library the way it was intended to be used.

This first analyzer will identify the following code:

```c#
[Activity(...)]
class FirstView : MvxActivity
{
    public new FirstViewModel ViewModel => base.ViewModel as FirstViewModel;
}
```

and provide you with a IDE tooltip to transform the code into:

```c#
[Activity(...)]
class FirstView : MvxActivity<FirstViewModel> {
    ...
}
```

# What's in it?
- A new project type under the CodeAnalysis solution folder named `MvvmCrossCodeAnalysis` (there's no `.` in the name for reasons - no tooling support) I'm wondering what you guys think about that?
- A Vsix package to enable easy debugging of Code Analyzers/Fixes.

# Installation Steps
- To use the Analyzers and Code Fixes, just add the NuGet package to your Core project, [`MvvmCross.CodeAnalysis`](https://www.nuget.org/packages/MvvmCross.CodeAnalysis/).

# Testing
- If you would like to create a new Analyzer/CodeFix, please consider adding tests to it. To do so, add a new class with your tests to the project `MvvmCross.CodeAnalysis.Test`, follow this guidelines:

## Test class declaration

- If your code contains only a Diagnostic, your test classe should inherit from 
[`DiagnosticVerifier`].
There is also a generic version, [`DiagnosticVerifier<T>`], where T should be your analyzer([`DiagnosticAnalyzer`]). The generic version is preferred because it already associate the test class with the Analyzer being tested.

## Test setup

- Your test methods should be quite simple. Provide test cases (strings containing full sample code) for the Analyzer and call method `VerifyCSharpDiagnostic`. This method receives your sample code and a `DiagnosticResult` instance, which should specify where and which analyzer should be triggered for this sample code, including the message, the DiagnosticId, severity and possible locations (column/line).

- If your Analyzer also contains a code fix, your test classe should inherit from 
[`CodeFixVerifier`]. This class inherits from [`DiagnosticVerifier`], and adds a few methods related to testing code fixes. There is also a generic version for this, [`CodeFixVerifier<T, TU>`], where T should inherit from [`DiagnosticAnalyzer`] and TU should inherit from [`CodeFixProvider`].

- To test your code fix, you should add another test case that calls method [`VerifyCSharpFix`], passing the original source sample and another string containing the final result of how the code should look after the code fix executes. Remember about line endings and blank spaces. Tabs (	) are not spaces.

## More info
- For additional information about analyzers and code fixes, follow this article:
https://msdn.microsoft.com/en-us/magazine/dn904670.aspx

