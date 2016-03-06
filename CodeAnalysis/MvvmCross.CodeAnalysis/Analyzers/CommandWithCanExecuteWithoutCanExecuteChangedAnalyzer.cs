using System.Collections.Generic;
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
        internal static readonly LocalizableString Title = "Commands with CanExecute should have the RaiseCanExecuteChanged method called at least once";
        internal static readonly LocalizableString MessageFormat = "Remove '{0}' from the constructor or call '{1}.RaiseCanExecuteChanged' from your code";
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
                        var mvxCommandType = context.SemanticModel.Compilation.GetTypeByMetadataName(typeof(MvxCommand).FullName);

                        var objectCreations = GetObjectCreations(context, classDeclaration, propertyDeclarationSyntax, propertySymbol);

                        foreach (var objectCreation in objectCreations)
                        {
                            var objectCreationSymbol = context.SemanticModel.GetSymbolInfo(objectCreation.Type).Symbol;
                            if (objectCreationSymbol != null && objectCreationSymbol.Equals(mvxCommandType))
                            {
                                if (objectCreation.ArgumentList.Arguments.Count == 2)
                                {
                                    // Is using CanExecute
                                    if (ClassIsCallingRaiseCanExecuteChanged(classDeclaration) == false)
                                    {
                                        var canExecute = objectCreation.ArgumentList.Arguments[1];

                                        var properties = new Dictionary<string, string> { { nameof(canExecute), canExecute.ToString() } }.ToImmutableDictionary();

                                        context.ReportDiagnostic(
                                            Diagnostic.Create(
                                                Rule
                                                , canExecute.GetLocation()
                                                , properties
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

        private static IEnumerable<ObjectCreationExpressionSyntax> GetObjectCreations(SyntaxNodeAnalysisContext context, ClassDeclarationSyntax classDeclaration, PropertyDeclarationSyntax propertyDeclarationSyntax, IPropertySymbol propertySymbol)
        {
            var objectCreations = classDeclaration.DescendantNodes()
                            .OfType<AssignmentExpressionSyntax>()
                            .Where(a => context.SemanticModel.GetSymbolInfo(a.Left).Symbol.Equals(propertySymbol) && a.Right is ObjectCreationExpressionSyntax)
                            .Select(a => a.Right as ObjectCreationExpressionSyntax).ToList();

            var arrowExpressionClause = propertyDeclarationSyntax.DescendantNodes()
                .OfType<ArrowExpressionClauseSyntax>()
                .SingleOrDefault(a => a.Expression is ObjectCreationExpressionSyntax)
                ?.Expression as ObjectCreationExpressionSyntax;
            if (arrowExpressionClause != null)
            {
                objectCreations.Add(arrowExpressionClause);
            }

            var getAcessorDeclararion = propertyDeclarationSyntax.DescendantNodes()
                .OfType<AccessorDeclarationSyntax>()
                .SingleOrDefault(a => a.IsKind(SyntaxKind.GetAccessorDeclaration));
            if (getAcessorDeclararion != null)
            {
                objectCreations.AddRange(getAcessorDeclararion.DescendantNodes()
                            .OfType<ObjectCreationExpressionSyntax>());
            }

            return objectCreations;
        }

        private static bool ClassIsCallingRaiseCanExecuteChanged(ClassDeclarationSyntax classDeclaration)
        {
            return classDeclaration.DescendantNodes()
                .OfType<InvocationExpressionSyntax>()
                .Any(i =>
                    (((i.Expression as MemberAccessExpressionSyntax)?.Name
                      ?? (i.Expression as MemberBindingExpressionSyntax)?.Name
                        ) as IdentifierNameSyntax)?.Identifier.Value.Equals("RaiseCanExecuteChanged") ?? false);
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