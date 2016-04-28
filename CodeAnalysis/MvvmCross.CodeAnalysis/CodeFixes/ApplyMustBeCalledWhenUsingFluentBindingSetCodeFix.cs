using System.Collections.Generic;
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
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(ApplyMustBeCalledWhenUsingFluentBindingSetCodeFix)), Shared]
    public class ApplyMustBeCalledWhenUsingFluentBindingSetCodeFix : CodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds =>
            ImmutableArray.Create(DiagnosticIds.ApplyMustBeCalledWhenUsingFluentBindingSetId);

        public sealed override FixAllProvider GetFixAllProvider() =>
            WellKnownFixAllProviders.BatchFixer;

        private static readonly string MessageFormat = "Add {0}.Apply()";

        public sealed override Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var diagnostic = context.Diagnostics.First();

            context.RegisterCodeFix(
                CodeAction.Create(
                    string.Format(MessageFormat, GetBindingSetIdentifier(diagnostic)),
                    cancelToken =>
                        ApplyFix(
                            context.Document,
                            context.Span, cancelToken),
                    nameof(ApplyMustBeCalledWhenUsingFluentBindingSetCodeFix))
                , diagnostic);

            return Task.FromResult(0);
        }

        private static string GetBindingSetIdentifier(Diagnostic diagnostic)
        {
            return diagnostic.Properties["bindingSetIdentifier"];
        }

        private static async Task<Document> ApplyFix(Document document, TextSpan span, CancellationToken cancellationToken)
        {
            var root = await document
                .GetSyntaxRootAsync(cancellationToken)
                .ConfigureAwait(false);

            var variableDeclarationSyntax = root.FindNode(span) as VariableDeclaratorSyntax;

            var parentBlock = variableDeclarationSyntax?.FirstAncestorOrSelf<BlockSyntax>();

            if (parentBlock == null) return document;

            var bindingSetIdentifier = variableDeclarationSyntax.Identifier;

            var lastBindCall = parentBlock.DescendantNodes()
                    .OfType<MemberAccessExpressionSyntax>()
                    .LastOrDefault(n => IsInvocationOfBind(n, bindingSetIdentifier.ValueText));

            if (lastBindCall != null)
            {
                var applyCallNode = SyntaxFactory.ExpressionStatement(
                    SyntaxFactory.InvocationExpression(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.IdentifierName(bindingSetIdentifier),
                            SyntaxFactory.IdentifierName("Apply"))));

                root = root.InsertNodesAfter(lastBindCall.FirstAncestorOrSelf<ExpressionStatementSyntax>(),
                    new List<SyntaxNode> { applyCallNode.WithLeadingTrivia(lastBindCall.GetLeadingTrivia()) });

                document = document.WithSyntaxRoot(root);
            }

            return document;
        }

        private static bool IsInvocationOfBind(MemberAccessExpressionSyntax memberAccessSyntax, string bindingSetIdentifierString)
        {
            var identifierName = GetIdentifierRecursive(memberAccessSyntax);

            if (identifierName?.Identifier.ValueText != bindingSetIdentifierString) return false;

            return memberAccessSyntax.Name.Identifier.ValueText == "Bind";
        }

        private static IdentifierNameSyntax GetIdentifierRecursive(MemberAccessExpressionSyntax memberAccessSyntax)
        {
            while (memberAccessSyntax.Expression is MemberAccessExpressionSyntax)
            {
                memberAccessSyntax = (MemberAccessExpressionSyntax)memberAccessSyntax.Expression;
            }
            var identifierNameSyntax = memberAccessSyntax.Expression as IdentifierNameSyntax;
            return identifierNameSyntax;
        }
    }
}