using Rop.Results;

namespace Rop.ResultExtensions;

public static class ResultBindExtensions
{
    /// <summary>
    /// Ejecuta una funcion que debuelve un ResultAction de tipo diferente al de entrada
    /// </summary>
    /// <param name="resultAction"></param>
    /// <param name="predicate"></param>
    /// <typeparam name="TO"></typeparam>
    /// <typeparam name="TI"></typeparam>
    /// <returns></returns>
    public static ResultAction<TO> Bind<TO, TI>(
        this ResultAction<TI> resultAction,
        Func<TI, ResultAction<TO>> predicate)
    {
        return resultAction.IsSuccess
            ? predicate(resultAction.EnsureValue)
            : new ResultAction<TO>(default, resultAction.Deconstruct());
    }

    /// <summary>
    /// Ejecuta una funcion que debuelve un ResultAction de tipo diferente al de entrada.
    /// Recive un ResultAction y una funcion asyncronos ambos
    /// </summary>
    /// <param name="resultAction"></param>
    /// <param name="predicate"></param>
    /// <typeparam name="TO"></typeparam>
    /// <typeparam name="TI"></typeparam>
    /// <returns></returns>
    public static async Task<ResultAction<TO>> Bind<TO, TI>(
        this Task<ResultAction<TI>> resultAction,
        Func<TI, Task<ResultAction<TO>>> predicate)
    {
        var result = await resultAction;
        return result.IsSuccess
            ? await predicate(result.EnsureValue)
            : new ResultAction<TO>(default, result.Deconstruct());
    }

    /// <summary>
    /// Ejecuta una funcion que debuelve un ResultAction de tipo diferente al de entrada.
    /// Recive un ResultAction Asyncrono y una funcion Sincrona
    /// </summary>
    /// <param name="resultAction"></param>
    /// <param name="predicate"></param>
    /// <typeparam name="TO"></typeparam>
    /// <typeparam name="TI"></typeparam>
    /// <returns></returns>
    public static async Task<ResultAction<TO>> Bind<TO, TI>(
        this Task<ResultAction<TI>> resultAction,
        Func<TI, ResultAction<TO>> predicate)
    {
        var result = await resultAction;
        return result.IsSuccess
            ? predicate(result.EnsureValue)
            : new ResultAction<TO>(default, result.Deconstruct());
    }


    /// <summary>
    /// Ejecuta una funcion que debuelve un ResultAction de tipo diferente al de entrada.
    /// Recive un ResultAction Asyncrono y una funcion Sincrona
    /// </summary>
    /// <param name="resultAction"></param>
    /// <param name="predicate"></param>
    /// <typeparam name="TO"></typeparam>
    /// <typeparam name="TI"></typeparam>
    /// <returns></returns>
    public static async Task<ResultAction<TO>> Bind<TO, TI>(
        this ResultAction<TI> resultAction,
        Func<TI, Task<ResultAction<TO>>> predicate)
    {
        var result = resultAction;
        return result.IsSuccess
            ? await predicate(result.EnsureValue)
            : new ResultAction<TO>(default, result.Deconstruct());
    }


    /// <summary>
    /// Ejecuta una funcion que debuelve un ResultAction sin tipo
    /// </summary>
    /// <param name="resultAction"></param>
    /// <param name="predicate"></param>
    /// <typeparam name="TI"></typeparam>
    /// <returns></returns>
    public static ResultAction Bind<TI>(
        this ResultAction<TI> resultAction,
        Func<TI, ResultAction> predicate)
    {
        return resultAction.IsSuccess
            ? predicate(resultAction.EnsureValue)
            : new ResultAction(resultAction.Deconstruct());
    }

    /// <summary>
    /// Ejecuta una funcion que debuelve un ResultAction sin tipo
    /// </summary>
    /// <param name="resultAction"></param>
    /// <param name="predicate"></param>
    /// <typeparam name="TI"></typeparam>
    /// <typeparam name="TO"></typeparam>
    /// <returns></returns>
    public static async Task<ResultAction<TO>> Bind<TI, TO>(
        this ResultAction<TI> resultAction,
        Func<Task<ResultAction<TO>>> predicate)
    {
        return resultAction.IsSuccess
            ? await predicate()
            : new ResultAction<TO>(default, resultAction.Deconstruct());
    }

    /// <summary>
    /// Ejecuta una funcion que debuelve un ResultAction sin tipo
    /// </summary>
    /// <param name="resultAction"></param>
    /// <param name="predicate"></param>
    /// <typeparam name="TI"></typeparam>
    /// <returns></returns>
    public static async Task<ResultAction> Bind<TI>(
        this ResultAction<TI> resultAction,
        Func<TI, Task<ResultAction>> predicate)
    {
        return resultAction.IsSuccess
            ? await predicate(resultAction.EnsureValue)
            : new ResultAction(resultAction.Deconstruct());
    }


    /// <summary>
    /// Ejecuta una funcion que debuelve un ResultAction sin tipo
    /// </summary>
    /// <param name="resultAction"></param>
    /// <param name="predicate"></param>
    /// <typeparam name="TI"></typeparam>
    /// <returns></returns>
    public static async Task<ResultAction<TI>> BindAndConserveValue<TI>(
        this Task<ResultAction<TI>> resultAction,
        Func<TI, Task<ResultAction>> predicate)
    {
        var result = await resultAction;
        return result.IsSuccess
            ? await predicate(result.EnsureValue).Map(result.EnsureValue)
            : new ResultAction<TI>(default, result.Deconstruct());
    }


    /// <summary>
    /// Ejecuta una funcion que devuelve un ResultAction sin tipo
    /// </summary>
    /// <param name="resultAction"></param>
    /// <param name="predicate"></param>
    /// <typeparam name="TI"></typeparam>
    /// <typeparam name="TO"></typeparam>
    /// <returns></returns>
    public static async Task<ResultAction<(TI, TO)>> BindAndConserveValue<TI, TO>(
        this Task<ResultAction<TI>> resultAction,
        Func<TI, Task<ResultAction<TO>>> predicate)
    {
        var result = await resultAction;

        if (result.IsFailure) 
            return new ResultAction<(TI, TO)>(default, result.Deconstruct());
        
        var predicateResult = await predicate(result.EnsureValue);

        if (predicateResult.IsSuccess) 
            return predicateResult.Map((result.EnsureValue, predicateResult.EnsureValue));
       
        return predicateResult.Map((result.EnsureValue, predicateResult.EnsureValue));
    }


    /// <summary>
    /// Ejecuta una funcion que debuelve un ResultAction sin tipo
    /// Recive un ResultAction Asyncrono y una funcion Sincrona
    /// </summary>
    /// <param name="resultAction"></param>
    /// <param name="predicate"></param>
    /// <typeparam name="TI"></typeparam>
    /// <returns></returns>
    public static async Task<ResultAction> Bind<TI>(
        this Task<ResultAction<TI>> resultAction,
        Func<TI, Task<ResultAction>> predicate)
    {
        var result = await resultAction;
        return result.IsSuccess
            ? await predicate(result.EnsureValue)
            : new ResultAction(result.Deconstruct());
    }
  
    
    /// <summary>
    /// Ejecuta una funcion que debuelve un ResultAction de tipo diferente al de entrada
    /// y guarda el tipo de entrada en una variable para no perderlo
    /// </summary>
    /// <param name="resultAction"></param>
    /// <param name="predicate"></param>
    /// <param name="value"></param>
    /// <typeparam name="TO"></typeparam>
    /// <typeparam name="TI"></typeparam>
    /// <returns></returns>
    public static ResultAction<TO> Bind<TO, TI>(
        this ResultAction<TI> resultAction,
        Func<TI, ResultAction<TO>> predicate
        , out TI? value)
    {
        if (resultAction.IsFailure)
        {
            value = default;
            return new ResultAction<TO>(default, resultAction.Deconstruct());
        }

        value = resultAction.EnsureValue;
        return predicate(resultAction.EnsureValue);
    }

    /// <summary>
    /// Ejecuta una funcion que debuelve un ResultAction sin tipo
    /// y guarda el tipo de entrada en una variable para no perderlo
    /// </summary>
    /// <param name="resultAction"></param>
    /// <param name="value"></param>
    /// <typeparam name="TI"></typeparam>
    /// <returns></returns>
    public static ResultAction<TI> ExtractValue<TI>(
        this Task<ResultAction<TI>> resultAction
        , out TI? value)
    {
        value = resultAction.Result.Value;
        return resultAction.Result;
    }

    /// <summary>
    /// Ejecuta una funcion que debuelve un ResultAction sin tipo
    /// y guarda el tipo de entrada en una variable para no perderlo
    /// </summary>
    /// <param name="resultAction"></param>
    /// <param name="predicate"></param>
    /// <param name="value"></param>
    /// <typeparam name="TI"></typeparam>
    /// <returns></returns>
    public static ResultAction Bind<TI>(
        this ResultAction<TI> resultAction,
        Func<TI, ResultAction> predicate
        , out TI? value)
    {
        if (resultAction.IsFailure)
        {
            value = default;
            return new ResultAction(resultAction.Deconstruct());
        }

        value = resultAction.EnsureValue;
        return predicate(resultAction.EnsureValue);
    }
}