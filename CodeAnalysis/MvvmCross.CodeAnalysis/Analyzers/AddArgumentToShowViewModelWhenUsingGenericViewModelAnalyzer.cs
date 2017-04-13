using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using MvvmCross.CodeAnalysis.Core;
using MvvmCross.Core.ViewModels;

namespace MvvmCross.CodeAnalysis.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class AddArgumentToShowViewModelWhenUsingGenericViewModelAnalyzer : DiagnosticAnalyzer
    {
        internal static readonly LocalizableString Title = "Consider passing a parameter to ShowViewModel<T> for the generic form of MvxViewModel<T>";
        internal static readonly LocalizableString MessageFormat = "'{0}' inherits from generic 'MvxViewModel<TInit>' or implements 'IMvxViewModelInitializer<TInit>' and requires an argument of type {1}";
        internal static readonly LocalizableString Description = "When calling 'ShowViewModel<TViewModel, TInit>()', if TViewModel inherits from the generic 'MvxViewModel<TInit>' or implements 'IMvxViewModelInitializer<TInit>', then the method call to ShowViewModel should be passing a TInit argument";
        internal const string Category = Categories.Usage;

        internal static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticIds.AddArgumentToShowViewModelWhenUsingGenericViewModelId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, true, "Required Matching Type");
        private static readonly string MvxViewModelInitializerName = typeof(IMvxViewModelInitializer<>).Name;

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
            => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(AnalyzeSyntaxNode, SyntaxKind.InvocationExpression);
        }

        private static void AnalyzeSyntaxNode(SyntaxNodeAnalysisContext context)
        {
            var invocationExpressionSyntax = context.Node as InvocationExpressionSyntax;

            var genericNameSyntax = invocationExpressionSyntax?.Expression as GenericNameSyntax;

            if (genericNameSyntax == null) return;

            if (genericNameSyntax.Identifier.Text != "ShowViewModel") return;

            var namedSymbol = context.SemanticModel.GetSymbolInfo(genericNameSyntax).Symbol as IMethodSymbol;

            if (namedSymbol != null &&
                namedSymbol.IsGenericMethod &&
                namedSymbol.TypeArguments.Length == 1)
            {
                var baseInterface = namedSymbol.TypeArguments[0].AllInterfaces.FirstOrDefault(x => x.MetadataName == MvxViewModelInitializerName);

                if (baseInterface != null &&
                    baseInterface.TypeArguments.Length == 1)
                {
                    var arguments = invocationExpressionSyntax.ArgumentList?.Arguments;

                    if (arguments != null &&
                        arguments.Value.Count > 0 &&
                        context.SemanticModel.GetTypeInfo(arguments.Value[0].Expression).Type.Equals(baseInterface.TypeArguments[0]))
                    {
                        return;
                    }

                    context.ReportDiagnostic(Diagnostic.Create(Rule, genericNameSyntax.GetLocation(), namedSymbol.TypeArguments[0].Name, baseInterface.TypeArguments[0].Name));
                }
            }
        }
    }
}
