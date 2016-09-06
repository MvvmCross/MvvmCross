using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using MvvmCross.CodeAnalysis.Analyzers;
using MvvmCross.CodeAnalysis.Core;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Formatting;

namespace MvvmCross.CodeAnalysis.CodeFixes
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(ConsiderUsingGenericBaseViewCodeFix)), Shared]
    public class ConsiderUsingGenericBaseViewCodeFix : CodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds =>
            ImmutableArray.Create(DiagnosticIds.UseGenericBaseClassRuleId);

        public sealed override FixAllProvider GetFixAllProvider() =>
            WellKnownFixAllProviders.BatchFixer;

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context
                .Document
                .GetSyntaxRootAsync(context.CancellationToken)
                .ConfigureAwait(false);

            var diagnostic = context.Diagnostics.First();

            // Find the type declaration identified by the diagnostic.
            var typeDeclaration = root.FindNode(context.Span)
                .FirstAncestorOrSelf<TypeDeclarationSyntax>();

            var viewModelProperty = typeDeclaration.Members
                .OfType<PropertyDeclarationSyntax>()
                .First(x => x.Identifier.Text == ConsiderUsingGenericBaseViewAnalyzer.ViewModelPropertyName);

            var derivingClass = typeDeclaration.BaseList.Types.First();

            // Register a code action that will invoke the fix.
            context.RegisterCodeFix(
                CodeAction.Create(
                    $@"To {derivingClass.Type}<{viewModelProperty.Type}>"
                    , cancelToken =>
                        ApplyFix(
                            context.Document
                            , derivingClass
                            , viewModelProperty
                            , cancelToken)
                            , $"{derivingClass.Type}<{viewModelProperty.Type}>")
                , diagnostic);
        }

        private static async Task<Document> ApplyFix(
            Document document
            , BaseTypeSyntax derivingClass
            , BasePropertyDeclarationSyntax viewModelProperty
            , CancellationToken cancellationToken)
        {
            var genericClassDeclaration = SyntaxFactory.SimpleBaseType(
                SyntaxFactory.GenericName(
                    SyntaxFactory.Identifier(
                            derivingClass.Type.ToString()
                        )
                    )
                    .WithTypeArgumentList(
                        SyntaxFactory.TypeArgumentList(
                            SyntaxFactory.SingletonSeparatedList(viewModelProperty.Type)
                        )
                    )
                ).WithAdditionalAnnotations(Formatter.Annotation);

            var editor = await DocumentEditor.CreateAsync(document, cancellationToken);
            editor.RemoveNode(viewModelProperty);
            editor.ReplaceNode(derivingClass, genericClassDeclaration);

            return editor.GetChangedDocument();
        }
    }
}