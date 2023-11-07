﻿namespace BlazorAppMaybePoc.Shared.Common;

public static class DictionaryExtensions
{
    public static Func<TK, TV?> ToLookupWithDefault<TK, TV>(this IDictionary<TK, TV> @this) =>
        x =>
            @this.ContainsKey(x) ? @this[x] : default;
}