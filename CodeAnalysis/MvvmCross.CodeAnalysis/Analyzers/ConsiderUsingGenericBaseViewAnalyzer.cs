using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using MvvmCross.CodeAnalysis.Core;
using MvvmCross.CodeAnalysis.Extensions;
using System.Collections.Immutable;
using System.Linq;

namespace MvvmCross.CodeAnalysis.Analyzers
{
    using MvvmCross.Core.ViewModels;
    using MvvmCross.Core.Views;

    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class ConsiderUsingGenericBaseViewAnalyzer : DiagnosticAnalyzer
    {
        public const string ViewModelPropertyName = "ViewModel";
        internal static readonly LocalizableString Title = "Consider deriving from generic base class with strongly typed ViewModel property";
        internal static readonly LocalizableString MessageFormat = "Derive from '{0}<T>', which already implements a strongly typed ViewModel property";
        internal const string Category = Categories.Usage;

        internal static DiagnosticDescriptor Rule =
            new DiagnosticDescriptor(DiagnosticIds.UseGenericBaseClassRuleId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
            => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.NamedType);
        }

        private static void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            var namedSymbol = context.Symbol as INamedTypeSymbol;
            var iMvxViewType = context.Compilation.GetTypeByMetadataName(typeof(IMvxView).FullName);

            if (namedSymbol != null &&
                namedSymbol.ImplementsSymbol(iMvxViewType) &&
                namedSymbol.GetMembers(ViewModelPropertyName).Any())
            {
                var viewModelProperty = namedSymbol.GetMembers(ViewModelPropertyName).FirstOrDefault() as IPropertySymbol;
                var viewModelReturnType = viewModelProperty?.Type;

                if (!IsViewModelType(context, viewModelReturnType))
                    return;

                var syntax = namedSymbol.DeclaringSyntaxReferences.First().GetSyntax() as ClassDeclarationSyntax;

                var baseType = syntax?.BaseList.Types.First();

                if (baseType?.Type is GenericNameSyntax) return;

                context.ReportDiagnostic(
                    Diagnostic.Create(
                        Rule
                        , baseType?.GetLocation()
                        , baseType?.ToString())
                );
            }
        }

        private static bool IsViewModelType(SymbolAnalysisContext context, ITypeSymbol symbol)
        {
            var iMvxViewModelType = context.Compilation.GetTypeByMetadataName(typeof(IMvxViewModel).FullName);
            var iNotifyPropertyChangedType = context.Compilation.GetTypeByMetadataName(typeof(System.ComponentModel.INotifyPropertyChanged).FullName);

            return symbol.ImplementsSymbol(iMvxViewModelType) ||
                   symbol.ImplementsSymbol(iNotifyPropertyChangedType);
        }
    }
}