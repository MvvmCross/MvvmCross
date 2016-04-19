using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using MvvmCross.CodeAnalysis.Core;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace MvvmCross.CodeAnalysis.CodeFixes
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(MvxMessengerSubscriptionDoesntStoreTokenInVariableCodeFix)), Shared]
    public class MvxMessengerSubscriptionDoesntStoreTokenInVariableCodeFix : CodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds =>
            ImmutableArray.Create(DiagnosticIds.MvxMessengerSubscriptionDoesntStoreTokenInVariableId);

        public sealed override FixAllProvider GetFixAllProvider() =>
            WellKnownFixAllProviders.BatchFixer;

        private static readonly string Message = "Store the returned token in a field";

        public sealed override Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var diagnostic = context.Diagnostics.First();

            context.RegisterCodeFix(
                CodeAction.Create(Message, cancelToken => ApplyFix(context.Document, context.Span, cancelToken),
                    nameof(CommandWithCanExecuteWithoutCanExecuteChangedCodeFix))
                , diagnostic);

            return Task.FromResult(0);
        }

        private static async Task<Document> ApplyFix(Document document, TextSpan span, CancellationToken cancellationToken)
        {
            var root = await document
                .GetSyntaxRootAsync(cancellationToken)
                .ConfigureAwait(false);

            var spanNode = root.FindNode(span);

            var tokenField = SyntaxFactory.FieldDeclaration(SyntaxFactory.VariableDeclaration(SyntaxFactory.IdentifierName("MvxSubscriptionToken"))
                .WithVariables(SyntaxFactory.SingletonSeparatedList(SyntaxFactory.VariableDeclarator(SyntaxFactory.Identifier("_token")))))
                .WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PrivateKeyword), SyntaxFactory.Token(SyntaxKind.ReadOnlyKeyword)));

            var ignoredSubscribeCall = spanNode.FirstAncestorOrSelf<InvocationExpressionSyntax>();
            var identifierSyntax = SyntaxFactory.IdentifierName("_token");
            var storeResultInTokenCall = SyntaxFactory.AssignmentExpression(SyntaxKind.SimpleAssignmentExpression, identifierSyntax, ignoredSubscribeCall);

            var classDeclarationId = spanNode.FirstAncestorOrSelf<ClassDeclarationSyntax>().Identifier;

            root = root
                .ReplaceNode(ignoredSubscribeCall, storeResultInTokenCall);

            var classDeclarationSyntax =
                root
                    .DescendantNodes()
                    .OfType<ClassDeclarationSyntax>()
                    .FirstOrDefault(c => c.Identifier.ValueText == classDeclarationId.ValueText);

            if (classDeclarationSyntax != null)
            {
                var newMembers = classDeclarationSyntax.Members.Insert(0, tokenField);

                var newClassDeclarationSyntax = classDeclarationSyntax
                    .WithMembers(newMembers)
                    .ReplaceNode(ignoredSubscribeCall, storeResultInTokenCall);

                root = root
                    .ReplaceNode(classDeclarationSyntax, newClassDeclarationSyntax);
            }

            document = document.WithSyntaxRoot(root.NormalizeWhitespace());
            return document;
        }
    }
}