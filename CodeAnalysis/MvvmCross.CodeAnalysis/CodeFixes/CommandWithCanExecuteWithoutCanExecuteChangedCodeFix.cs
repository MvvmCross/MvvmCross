using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Editing;
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
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(CommandWithCanExecuteWithoutCanExecuteChangedCodeFix)), Shared]
    public class CommandWithCanExecuteWithoutCanExecuteChangedCodeFix : CodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds =>
            ImmutableArray.Create(DiagnosticIds.CommandWithCanExecuteWithoutCanExecuteChangedRuleId);

        public sealed override FixAllProvider GetFixAllProvider() =>
            WellKnownFixAllProviders.BatchFixer;

        public static readonly string MessageFormat = "Remove method invocation: {0}.";

        public sealed override Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var diagnostic = context.Diagnostics.First();

            context.RegisterCodeFix(
                CodeAction.Create(
                    string.Format(MessageFormat, GetCanExecute(diagnostic)),
                    cancelToken =>
                        ApplyFix(
                            context.Document,
                            context.Span, cancelToken),
                    nameof(CommandWithCanExecuteWithoutCanExecuteChangedCodeFix))
                , diagnostic);

            return Task.FromResult(0);
        }

        private static string GetCanExecute(Diagnostic diagnostic)
        {
            return diagnostic.Properties["canExecute"];
        }

        private static async Task<Document> ApplyFix(Document document, TextSpan span, CancellationToken cancellationToken)
        {
            var root = await document
                .GetSyntaxRootAsync(cancellationToken)
                .ConfigureAwait(false);

            var argumentListSyntax = root.FindNode(span)
                .FirstAncestorOrSelf<ArgumentListSyntax>();

            var newArgumentListSyntax = SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(new[]
            {
                argumentListSyntax.Arguments[0]
            }));

            var editor = await DocumentEditor.CreateAsync(document, cancellationToken);
            editor.ReplaceNode(argumentListSyntax, newArgumentListSyntax);
            return editor.GetChangedDocument();
        }
    }
}