using Rop.Results;

namespace Rop.ResultExtensions;

public static class ResultMapExtensions
{
    /// <summary>
    ///     Mappear de un tipo de valor a otro mediante una foncion de mapeo
    /// </summary>
    /// <param name="resultAction"></param>
    /// <param name="mapper"></param>
    /// <typeparam name="TI"></typeparam>
    /// <typeparam name="TO"></typeparam>
    /// <returns></returns>
    public static ResultAction<TO> Map<TI, TO>(
        this ResultAction<TI> resultAction,
        Func<TI, TO> mapper)
    {
        if (resultAction.IsFailure)
            return new ResultAction<TO>(default, resultAction.Deconstruct());

        var res = mapper(resultAction.EnsureValue);
        return new ResultAction<TO>(res, resultAction.Deconstruct());
    }

    /// <summary>
    ///     Mappear de un tipo de valor a otro mediante una foncion de mapeo asyncronamente
    /// </summary>
    /// <param name="resultAction"></param>
    /// <param name="mapper"></param>
    /// <typeparam name="TI"></typeparam>
    /// <typeparam name="TO"></typeparam>
    /// <returns></returns>
    public static async Task<ResultAction<TO>> Map<TI, TO>(
        this Task<ResultAction<TI>> resultAction,
        Func<TI, TO> mapper)
    {
        var result = await resultAction;
        if (result.IsFailure)
            return new ResultAction<TO>(default, result.Deconstruct());

        var res = mapper(result.EnsureValue);
        return new ResultAction<TO>(res, result.Deconstruct());
    }


    /// <summary>
    ///     Cambiar el tipo de un ResulAction a otro
    /// </summary>
    /// <param name="resultAction"></param>
    /// <param name="value"></param>
    /// <typeparam name="TI"></typeparam>
    /// <typeparam name="TO"></typeparam>
    /// <returns></returns>
    public static ResultAction<TO> Map<TI, TO>(
        this ResultAction<TI> resultAction,
        TO value)
    {
        if (value == null) throw new ArgumentNullException(nameof(value));
        return resultAction.IsFailure
            ? new ResultAction<TO>(default, resultAction.Deconstruct())
            : new ResultAction<TO>(value, resultAction.Deconstruct());
    }

    /// <summary>
    ///     Cambiar el tipo de un ResulAction a otro asyncronamente
    /// </summary>
    /// <param name="resultAction"></param>
    /// <param name="value"></param>
    /// <typeparam name="TI"></typeparam>
    /// <typeparam name="TO"></typeparam>
    /// <returns></returns>
    public static async Task<ResultAction<TO>> Map<TI, TO>(
        this Task<ResultAction<TI>> resultAction,
        TO value)
    {
        var result = await resultAction;

        if (value == null) throw new ArgumentNullException(nameof(value));

        return result.IsFailure
            ? new ResultAction<TO>(default, result.Deconstruct())
            : new ResultAction<TO>(value, result.Deconstruct());
    }

    /// <summary>
    ///     Mapear de un ResulAction con tipo a uno sin tipo
    /// </summary>
    /// <param name="resultAction"></param>
    /// <typeparam name="TI"></typeparam>
    /// <returns></returns>
    public static ResultAction Map<TI>(
        this ResultAction<TI> resultAction)
    {
        return new ResultAction(resultAction.Deconstruct());
    }

    /// <summary>
    ///     Mapear de un ResulAction con tipo a uno sin tipo
    /// </summary>
    /// <param name="resultAction"></param>
    /// <typeparam name="TI"></typeparam>
    /// <returns></returns>
    public static async Task<ResultAction> Map<TI>(
        this Task<ResultAction<TI>> resultAction)
    {
        var result = await resultAction;
        return new ResultAction(result.Deconstruct());
    }

    /// <summary>
    ///     Agregarle un tipo a un ResulAction
    /// </summary>
    /// <param name="resultAction"></param>
    /// <param name="value"></param>
    /// <typeparam name="TO"></typeparam>
    /// <returns></returns>
    public static ResultAction<TO> Map<TO>(
        this ResultAction resultAction,
        TO value)
    {
        if (value == null) throw new ArgumentNullException(nameof(value));

        return resultAction.IsSuccess
            ? new ResultAction<TO>(value, resultAction.Deconstruct())
            : new ResultAction<TO>(default, resultAction.Deconstruct());
    }

    /// <summary>
    ///     Agregarle un tipo a un ResulAction
    /// </summary>
    /// <param name="resultAction"></param>
    /// <param name="value"></param>
    /// <typeparam name="TO"></typeparam>
    /// <returns></returns>
    public static async Task<ResultAction<TO>> Map<TO>(
        this Task<ResultAction> resultAction,
        TO value)
    {
        var result = await resultAction;
        if (value == null) throw new ArgumentNullException(nameof(value));

        return result.IsSuccess
            ? new ResultAction<TO>(value, result.Deconstruct())
            : new ResultAction<TO>(default, result.Deconstruct());
    }
}