using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using MvvmCross.CodeAnalysis.Core;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis.CSharp;

namespace MvvmCross.CodeAnalysis.Analyzers
{
    using MvvmCross.Core.ViewModels;

    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class AddArgumentToShowViewModelWhenUsingGenericViewModelAnalyzer : DiagnosticAnalyzer
    {
        internal static readonly LocalizableString Title = "Consider passing a parameter to ShowViewModel<T> for the generic form of MvxViewModel<T>";
        internal static readonly LocalizableString MessageFormat = "When calling 'ShowViewModel<T>()', if T inherits from the generic 'MvxViewModel<TU>', then the method call to ShowViewModel should be passing a TU argument";
        internal const string Category = Categories.Usage;

        internal static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticIds.AddArgumentToShowViewModelWhenUsingGenericViewModelId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, true);
        private static readonly string MvxViewModelGenericFullName = typeof(MvxViewModel<>).FullName;

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
                namedSymbol.TypeArguments != null &&
                namedSymbol.TypeArguments.Length == 1)
            {
                var genericViewModelMetadataName = context.SemanticModel.Compilation.GetTypeByMetadataName(MvxViewModelGenericFullName);
                var baseType = GetBaseGenericViewModelType(namedSymbol.TypeArguments[0], genericViewModelMetadataName);
                if (baseType != null)
                {
                    if (baseType.TypeArguments != null && baseType.TypeArguments.Length == 1)
                    {
                        var arguments = invocationExpressionSyntax.ArgumentList?.Arguments;
                        if (arguments != null &&
                            arguments.Value.Count > 0 &&
                            context.SemanticModel.GetTypeInfo(arguments.Value[0].Expression).Type.Equals(baseType.TypeArguments[0]))
                        {
                            return;
                        }

                        context.ReportDiagnostic(
                            Diagnostic.Create(
                                Rule
                                , genericNameSyntax.GetLocation())
                        );
                    }
                }
            }
        }

        private static INamedTypeSymbol GetBaseGenericViewModelType(ITypeSymbol typeSymbol, INamedTypeSymbol genericViewModelMetadataName)
        {
            if (typeSymbol.BaseType == null || typeSymbol.BaseType.ConstructedFrom.Equals(genericViewModelMetadataName))
            {
                return typeSymbol.BaseType;
            }
            return GetBaseGenericViewModelType(typeSymbol.BaseType, genericViewModelMetadataName);
        }
    }
}
