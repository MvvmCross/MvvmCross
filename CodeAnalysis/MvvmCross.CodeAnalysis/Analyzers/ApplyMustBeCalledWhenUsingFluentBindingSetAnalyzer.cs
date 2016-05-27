using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using MvvmCross.CodeAnalysis.Core;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MvvmCross.CodeAnalysis.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class ApplyMustBeCalledWhenUsingFluentBindingSetAnalyzer : DiagnosticAnalyzer
    {
        private static readonly LocalizableString Title = "Apply method must be called when using MvxFluentBindingDescriptionSet";
        private static readonly LocalizableString MessageFormat = "Call {0}.Apply() method to apply your bindings";
        private const string Category = Categories.Usage;

        private static readonly DiagnosticDescriptor Rule =
            new DiagnosticDescriptor(DiagnosticIds.ApplyMustBeCalledWhenUsingFluentBindingSetId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
            => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(Analyzer, SyntaxKind.VariableDeclarator);
        }

        private static void Analyzer(SyntaxNodeAnalysisContext context)
        {
            //Value is not being stored 
            var variableDeclarationSyntax = context.Node as VariableDeclaratorSyntax;
            if (variableDeclarationSyntax == null) return;

            var invocationExpressionSyntax = variableDeclarationSyntax.Initializer.Value as InvocationExpressionSyntax;

            var memberAccessExpressionSyntax = invocationExpressionSyntax?.Expression as MemberAccessExpressionSyntax;

            var genericNameSyntax = memberAccessExpressionSyntax?.Name as GenericNameSyntax;
            if (genericNameSyntax == null) return;

            if (genericNameSyntax.Identifier.Text != "CreateBindingSet") return;

            var parentBlock = memberAccessExpressionSyntax.FirstAncestorOrSelf<BlockSyntax>();

            var bindingSetIdentifier = variableDeclarationSyntax.Identifier.ValueText;

            if (parentBlock?.DescendantNodes().OfType<MemberAccessExpressionSyntax>().Any(n => IsInvocationOfApply(n, bindingSetIdentifier)) == false)
            {
                var properties = new Dictionary<string, string> { { nameof(bindingSetIdentifier), bindingSetIdentifier } }.ToImmutableDictionary();

                context.ReportDiagnostic(Diagnostic.Create(Rule, variableDeclarationSyntax.Identifier.GetLocation(), properties, variableDeclarationSyntax.Identifier.ToFullString().Trim()));
            }
        }

        private static bool IsInvocationOfApply(MemberAccessExpressionSyntax memberAccessSyntax, string bindingSetIdentifierString)
        {
            var identifierName = memberAccessSyntax.Expression as IdentifierNameSyntax;

            if (identifierName?.Identifier.ValueText != bindingSetIdentifierString) return false;

            return memberAccessSyntax.Name.Identifier.ValueText == "Apply";
        }
    }
}
