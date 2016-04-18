using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using MvvmCross.CodeAnalysis.Core;
using System.Collections.Immutable;
using System.Linq;

namespace MvvmCross.CodeAnalysis.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class MvxMessengerSubscriptionDoesntStoreTokenInVariableAnalyzer : DiagnosticAnalyzer
    {
        internal static readonly LocalizableString Title = "You must store the the token returned from a Subscribe method.";
        internal static readonly LocalizableString MessageFormat = "You need to store the token returned from '{0}'.";
        internal const string Category = Categories.Usage;

        internal static readonly DiagnosticDescriptor Rule =
            new DiagnosticDescriptor(DiagnosticIds.MvxMessengerSubscriptionDoesntStoreTokenInVariableId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        private static readonly string[] SubscribeMethods = new []
        {
            "Subscribe",
            "SubscribeOnMainThread",
            "SubscribeOnThreadPoolThread"
        };

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(Analyzer, SyntaxKind.InvocationExpression);
        }

        private static void Analyzer(SyntaxNodeAnalysisContext context)
        {
            //Value is not being stored 
            var invocationExpressionSyntax = context.Node as InvocationExpressionSyntax;
            if (!(invocationExpressionSyntax?.Parent is ExpressionStatementSyntax)) return;

            //Method is one of IMvxMessenger's Subscribe Methods
            var memberAccessExpression = (MemberAccessExpressionSyntax) invocationExpressionSyntax.Expression;
            var genericNameSyntax = memberAccessExpression.Name;
            var isCallToSubscribe = SubscribeMethods.Contains(genericNameSyntax?.Identifier.Text);
            if (!isCallToSubscribe) return;

            //Caller implements IMvxMessenger
            if (!CallerTypeImplementsIMvxMessenger()) return;

            context.ReportDiagnostic(Diagnostic.Create(Rule, invocationExpressionSyntax.GetLocation(), invocationExpressionSyntax.ToFullString().Trim()));
        }

        private static bool CallerTypeImplementsIMvxMessenger()
        {
            //TODO: Find a way to verify this
            return true;
        }
    }
}