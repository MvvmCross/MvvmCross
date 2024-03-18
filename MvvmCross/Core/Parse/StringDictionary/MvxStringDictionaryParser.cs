// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

using MvvmCross.Base;
using MvvmCross.Exceptions;

namespace MvvmCross.Core.Parse.StringDictionary;

public class MvxStringDictionaryParser
    : MvxParser, IMvxStringDictionaryParser
{
    protected Dictionary<string, string?>? CurrentEntries { get; private set; }

    public IDictionary<string, string> Parse(string textToParse)
    {
        Reset(textToParse);

        while (!IsComplete)
        {
            ParseNextKeyValuePair();
            SkipWhitespaceAndCharacters(';');
        }

        return CurrentEntries!;
    }

    protected override void Reset(string? textToParse)
    {
        CurrentEntries = new Dictionary<string, string?>();
        base.Reset(textToParse);
    }

    private void ParseNextKeyValuePair()
    {
        SkipWhitespace();

        if (IsComplete)
        {
            return;
        }

        var key = ReadValue();
        if (key is not string keyString)
        {
            throw new MvxException($"Unexpected object in key for key/value pair {key?.GetType().Name} at position {CurrentIndex}");
        }

        SkipWhitespace();

        if (CurrentChar != '=')
        {
            throw new MvxException($"Unexpected character in key/value pair {CurrentChar} at position {CurrentIndex}");
        }

        MoveNext();
        SkipWhitespace();

        var value = ReadValue();
        if (value == null)
        {
            CurrentEntries![keyString] = null;
        }
        else if (value is string stringValue)
        {
            CurrentEntries![keyString] = stringValue;
        }
        else
        {
            throw new MvxException($"Unexpected object in value for key/value pair {value.GetType().Name} for key {key} at position {CurrentIndex}");
        }
    }
}
