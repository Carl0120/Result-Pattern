using Rop.Factories;
using Rop.Results;

namespace Rop.ResultExtensions;

public static class ResultEnsureExtensions
{
    /// <summary>
    ///     Asegurarse que el tipo que contiene el ResultAction cumple cierta condicion
    /// </summary>
    /// <param name="resultAction"></param>
    /// <param name="predicate"></param>
    /// <param name="errorValidation"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static ResultAction<T> Ensure<T>(
        this ResultAction<T> resultAction,
        Func<T, bool> predicate,
        ErrorValidation errorValidation)
    {
        if (resultAction.IsFailure)
            return resultAction;

        return predicate(resultAction.EnsureValue) ? resultAction : Result<T>.BadRequest(errorValidation);
    }

    /// <summary>
    ///     Asegurarse que el tipo que contiene el ResultAction cumple cierta condicion
    /// </summary>
    /// <param name="resultAction"></param>
    /// <param name="predicate"></param>
    /// <param name="errorValidation"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static async Task<ResultAction<T>> Ensure<T>(
        this ResultAction<T> resultAction,
        Func<T, Task<bool>> predicate,
        ErrorValidation errorValidation)
    {
        if (resultAction.IsFailure)
            return resultAction;

        return await predicate(resultAction.EnsureValue) ? resultAction : Result<T>.BadRequest(errorValidation);
    }

    /// <summary>
    ///     Asegurarse que el tipo que contiene el ResultAction cumple cierta condicion
    /// </summary>
    /// <param name="resultAction"></param>
    /// <param name="predicate"></param>
    /// <param name="errorValidation"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static async Task<ResultAction<T>> Ensure<T>(
        this Task<ResultAction<T>> resultAction,
        Func<T, Task<bool>> predicate,
        ErrorValidation errorValidation)
    {
        var result = await resultAction;
        if (result.IsFailure)
            return result;

        return await predicate(result.EnsureValue) ? result : Result<T>.BadRequest(errorValidation);
    }


    /// <summary>
    ///     Asegurarse que el tipo que contiene el ResultAction cumple cierta condicion
    /// </summary>
    /// <param name="resultAction"></param>
    /// <param name="notFoundMessage"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static ResultAction<T> EnsureFound<T>(
        this ResultAction<T> resultAction,
        string notFoundMessage)
    {
        if (resultAction.IsFailure)
            return resultAction;

        return Result<T>.NotFound(notFoundMessage);
    }

    /// <summary>
    ///     Asegurarse que el tipo que contiene el ResultAction cumple cierta condicion
    /// </summary>
    /// <param name="resultAction"></param>
    /// <param name="predicate"></param>
    /// <param name="errorId"></param>
    /// <param name="errorValidation"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static ResultAction<T> Ensure<T>(
        this ResultAction<T> resultAction,
        Func<T, bool> predicate,
        string errorId,
        string errorValidation)
    {
        if (resultAction.IsFailure)
            return resultAction;

        return predicate(resultAction.EnsureValue)
            ? resultAction
            : Result<T>.BadRequest(new ErrorValidation(errorId, errorValidation));
    }

    /// <summary>
    ///     Asegurarse que el tipo que contiene cumple cierta condicion
    /// </summary>
    /// <param name="value"></param>
    /// <param name="predicate"></param>
    /// <param name="errorId"></param>
    /// <param name="errorDescription"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    private static ResultAction<T> Ensure<T>(
        this T? value,
        Func<T, bool> predicate,
        string errorId,
        string errorDescription)
    {
        if (value == null)
            return Result<T>.BadRequest(new ErrorValidation(errorId, errorDescription));

        if (predicate(value))
            Result<T>.Success(value);

        return Result<T>.BadRequest(new ErrorValidation(errorId, errorDescription));
    }

    /// <summary>
    ///     Asegurarse que el tipo que contiene cumple cierta condicion
    /// </summary>
    /// <param name="value"></param>
    /// <param name="predicate"></param>
    /// <param name="resultAction"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static async Task<ResultAction<T>> Ensure<T>(
        this Task<T?> value,
        Func<T?, bool> predicate,
        ResultAction resultAction)
    {
       
        var valueResult = await value;
        if (valueResult == null)
            return new ResultAction<T>(default, resultAction.Deconstruct());

        return predicate(valueResult)
            ? Result<T>.Success(valueResult)
            : new ResultAction<T>(default, resultAction.Deconstruct());
    }

    /// <summary>
    ///     Asegurarse que el tipo que contiene cumple cierta condicion
    /// </summary>
    /// <param name="value"></param>
    /// <param name="predicate"></param>
    /// <param name="resultAction"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static async Task<ResultAction<T>> Ensure<T>(
        this Task<ResultAction<T>> value,
        Func<T, bool> predicate,
        ResultAction resultAction)
    {
        var valueResult = await value;
        if (valueResult.IsFailure)
            return new ResultAction<T>(default, valueResult.Deconstruct());

        return predicate(valueResult.EnsureValue)
            ? Result<T>.Success(valueResult.EnsureValue)
            : new ResultAction<T>(default, resultAction.Deconstruct());
    }

    /// <summary>
    ///     Asegurarse que el tipo que contiene cumple con un grupo de condiciones
    /// </summary>
    /// <param name="value"></param>
    /// <param name="validators"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static ResultAction<T> Ensure<T>(
        this T value,
        params (Func<T, bool> predicate, string errorId, string errorDescription)[] validators)
    {
        return Combine(validators.Select(validator =>
            Ensure(value, validator.predicate, validator.errorId, validator.errorDescription)).ToArray());
    }

    /// <summary>
    ///     Combina varios ResultActions con tipo en uno
    /// </summary>
    /// <param name="resultActions"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    private static ResultAction<T> Combine<T>(params ResultAction<T>[] resultActions)
    {
        if (resultActions.All(e => e.IsSuccess))
            return Result<T>.Success(resultActions.First(e => e.IsSuccess).Value!);

        var errors = resultActions
            .Where(e => !e.IsSuccess)
            .SelectMany(e =>
                e.ValidationErrors!)
            .Distinct()
            .ToList();
        return Result<T>.BadRequest(errors);
    }

    /// <summary>
    ///     Asegurarse que el tipo que contiene cumple con un grupo de condiciones
    /// </summary>
    /// <returns></returns>
    public static ResultAction Next(
        this ResultAction resultAction,
        Func<ResultAction> predicate)
    {
        return resultAction.IsFailure ? resultAction : predicate();
    }

    /// <summary>
    ///     Asegurarse que el tipo que contiene cumple con un grupo de condiciones
    /// </summary>
    /// <returns></returns>
    public static async Task<ResultAction> Next(
        this Task<ResultAction> resultAction,
        Func<ResultAction> predicate)
    {
        var result = await resultAction;
        return result.IsFailure ? result : predicate();
    }

    /// <summary>
    ///     Asegurarse que el tipo que contiene cumple con un grupo de condiciones
    /// </summary>
    /// <returns></returns>
    public static async Task<ResultAction> Next(
        this Task<ResultAction> resultAction,
        Func<Task<ResultAction>> predicate)
    {
        var result = await resultAction;
        return result.IsFailure ? result : await predicate();
    }

    /// <summary>
    ///     Asegurarse que el tipo que contiene cumple con un grupo de condiciones
    /// </summary>
    /// <returns></returns>
    public static async Task<ResultAction> Next(
        this ResultAction resultAction,
        Func<Task<ResultAction>> predicate)
    {
        var result = resultAction;
        return result.IsFailure ? result : await predicate();
    }
}