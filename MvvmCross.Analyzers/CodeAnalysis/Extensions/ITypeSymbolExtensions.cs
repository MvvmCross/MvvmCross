// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

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
