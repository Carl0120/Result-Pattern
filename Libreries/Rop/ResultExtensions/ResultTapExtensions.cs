using Rop.Results;

namespace Rop.ResultExtensions;

public static class ResultTapExtensions
{
    public static ResultAction<TI> Tap<TI>(
        this ResultAction<TI> resultAction,
        Action<TI> predicate)
    {
        if (resultAction.IsFailure)
            return resultAction;

        predicate(resultAction.EnsureValue);
        return resultAction;
    }

    public static async Task<ResultAction<TI>> Tap<TI>(
        this Task<ResultAction<TI>> resultAction,
        Action<TI> predicate)
    {
        var result = await resultAction;
        if (result.IsFailure)
            return result;

        predicate(result.EnsureValue);
        return result;
    }


    public static async Task<ResultAction<TI>> Tap<TI>(
        this ResultAction<TI> resultAction,
        Func<TI, Task> predicate)
    {
        if (resultAction.IsFailure)
            return resultAction;

        await predicate(resultAction.EnsureValue);
        return resultAction;
    }


    public static async Task<ResultAction<TI>> TapIfFiled<TI>(
        this ResultAction<TI> resultAction,
        Action predicate)
    {
        if (!resultAction.IsFailure) return resultAction;
       
        predicate();
        return resultAction;

    }

    public static async Task<ResultAction<TI>> TapIfFiled<TI>(
        this Task<ResultAction<TI>> resultAction,
        Action predicate)
    {
        var result = await resultAction;
       
        if (!result.IsFailure) return result;
      
        predicate();
        return result;

    }
}