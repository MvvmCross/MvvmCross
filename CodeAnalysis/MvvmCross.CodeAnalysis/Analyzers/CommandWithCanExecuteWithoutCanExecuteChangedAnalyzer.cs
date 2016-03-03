using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using MvvmCross.CodeAnalysis.Core;
using MvvmCross.CodeAnalysis.Extensions;
using MvvmCross.Core.ViewModels;
using System.Collections.Immutable;
using System.Linq;
using System.Windows.Input;

namespace MvvmCross.CodeAnalysis.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class CommandWithCanExecuteWithoutCanExecuteChangedAnalyzer : DiagnosticAnalyzer
    {
        internal static readonly LocalizableString Title = "Commands with CanExecute should have the CanExecuteChanged method called at least once";
        internal static readonly LocalizableString MessageFormat = "Remove '{0}' from the constructor or call '{1}.CanExecuteChanged' from your code";
        internal const string Category = Categories.Usage;

        internal static readonly DiagnosticDescriptor Rule =
            new DiagnosticDescriptor(DiagnosticIds.CommandWithCanExecuteWithoutCanExecuteChangedRuleId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
            => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(Analyzer, SyntaxKind.PropertyDeclaration);
        }

        private static void Analyzer(SyntaxNodeAnalysisContext context)
        {
            var propertyDeclarationSyntax = context.Node as PropertyDeclarationSyntax;
            if (propertyDeclarationSyntax != null)
            {
                var propertySymbol = context.SemanticModel.GetDeclaredSymbol(propertyDeclarationSyntax);

                var iCommandType = context.SemanticModel.Compilation.GetTypeByMetadataName(typeof(ICommand).FullName);

                if (propertySymbol != null &&
                    (propertySymbol.Type.Equals(iCommandType) || propertySymbol.Type.ImplementsSymbol(iCommandType)) &&
                    propertySymbol.DeclaredAccessibility == Accessibility.Public &&
                    propertySymbol.GetMethod != null &&
                    propertySymbol.GetMethod.DeclaredAccessibility == Accessibility.Public &&
                    propertySymbol.ContainingType != null)
                {
                    var viewModelType = propertySymbol.ContainingType;

                    if (!IsViewModelType(context, viewModelType))
                        return;

                    var classDeclaration = context.Node.Ancestors().OfType<ClassDeclarationSyntax>().FirstOrDefault();
                    if (classDeclaration != null)
                    {
                        var containingAssignments = classDeclaration.DescendantNodes()
                            .OfType<AssignmentExpressionSyntax>()
                            .Where(a => context.SemanticModel.GetSymbolInfo(a.Left).Symbol.Equals(propertySymbol));

                        var mvxCommandType = context.SemanticModel.Compilation.GetTypeByMetadataName(typeof(MvxCommand).FullName);

                        foreach (var assignment in containingAssignments)
                        {
                            var objectCreationExpressionSyntax = assignment.Right as ObjectCreationExpressionSyntax;
                            if (objectCreationExpressionSyntax != null)
                            {
                                var objectCreationSymbol = context.SemanticModel.GetSymbolInfo(objectCreationExpressionSyntax.Type).Symbol;
                                if (objectCreationSymbol != null && objectCreationSymbol.Equals(mvxCommandType))
                                {
                                    if (objectCreationExpressionSyntax.ArgumentList.Arguments.Count == 2)
                                    {
                                        // Is using CanExecute
                                        if (CallingCanExecuteChanged(context, classDeclaration) == false)
                                        {
                                            var canExecute = objectCreationExpressionSyntax.ArgumentList.Arguments[1];

                                            context.ReportDiagnostic(
                                                Diagnostic.Create(
                                                    Rule
                                                    , canExecute.GetLocation()
                                                    , canExecute.ToString()
                                                    , propertySymbol.Name)
                                                );
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private static bool CallingCanExecuteChanged(SyntaxNodeAnalysisContext context, ClassDeclarationSyntax classDeclaration)
        {
            return classDeclaration.DescendantNodes()
                .OfType<InvocationExpressionSyntax>()
                .Any(i => ((i.Expression as MemberAccessExpressionSyntax)?.Name as IdentifierNameSyntax)?.Identifier.Value.Equals("RaiseCanExecuteChanged") ?? false);
        }

        private static bool IsViewModelType(SyntaxNodeAnalysisContext context, ITypeSymbol symbol)
        {
            var iMvxViewModelType = context.SemanticModel.Compilation.GetTypeByMetadataName(typeof(IMvxViewModel).FullName);
            var iNotifyPropertyChangedType = context.SemanticModel.Compilation.GetTypeByMetadataName(typeof(System.ComponentModel.INotifyPropertyChanged).FullName);

            return symbol.ImplementsSymbol(iMvxViewModelType) ||
                   symbol.ImplementsSymbol(iNotifyPropertyChangedType);
        }
    }
}