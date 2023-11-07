namespace BlazorAppMaybePoc.Shared.Common;

public static class FunctionalExtensions
{
    public static T IterateUntil<T>(this T @this, Func<T, T> updateFunction, Func<T, bool> endCondition)
    {
        var currentThis = @this;

        while (!endCondition(currentThis))
        {
            currentThis = updateFunction(currentThis);
        }

        return currentThis;
    }

    public static T IterateUntil_<T>(this T @this, Func<T, T> updateFunction, Func<T, bool> endCondition)
    {
        var currentThis = @this;

        LoopBeginning:

        if (endCondition(currentThis))
            goto LoopEnding;

        currentThis = updateFunction(currentThis);
        goto LoopBeginning;

        LoopEnding:

        return currentThis;
    }

    public static TOut Map<TIn, TOut>(this TIn @this, Func<TIn, TOut> f) =>
        f(@this);
}