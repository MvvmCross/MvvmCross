using Microsoft.CodeAnalysis;

namespace MvvmCross.CodeAnalysis.Extensions
{
    public static class ITypeSymbolExtensions
    {
        public static bool ImplementsSymbol(this ITypeSymbol symbol, INamedTypeSymbol interfaceSymbol)
        {
            return symbol.AllInterfaces.Contains(interfaceSymbol);
        }
    }
}
