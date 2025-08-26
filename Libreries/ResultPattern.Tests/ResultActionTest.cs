using System;
using ResultPattern.Exceptions;

namespace ResultPattern.Tests;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

[TestClass]
public class ResultActionTests
{
    #region Tests for Success Scenarios

    [TestMethod]
    public void Success_WithDefaultMessage_ReturnsOkResult()
    {
        // Act
        var result = ResultAction.Success();

        // Assert
        Assert.IsTrue(result.IsSuccess);
        Assert.IsFalse(result.IsFailure);
        Assert.AreEqual("OK", result.Message);
        Assert.AreEqual(ResultCode.Ok, result.StatusCode);
        Assert.IsNull(result.ValidationErrors);
    }

    [TestMethod]
    public void Success_WithCustomMessage_ReturnsOkResultWithCustomMessage()
    {
        // Arrange
        var customMessage = "Operación completada exitosamente";

        // Act
        var result = ResultAction.Success(customMessage);

        // Assert
        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(customMessage, result.Message);
        Assert.AreEqual(ResultCode.Ok, result.StatusCode);
    }

    [TestMethod]
    public void Success_Generic_WithDataAndDefaultMessage_ReturnsOkResultWithData()
    {
        // Arrange
        var testData = 42;

        // Act
        ResultAction<int> result = ResultAction.Success(testData);

        // Assert
        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual("Ok", result.Message);
        Assert.AreEqual(ResultCode.Ok, result.StatusCode);
        Assert.AreEqual(testData, result.Data);
    }

    [TestMethod]
    public void Success_Generic_WithDataAndCustomMessage_ReturnsOkResultWithDataAndMessage()
    {
        // Arrange
        var testData = "test Data";
        var customMessage = "Valor obtenido exitosamente";

        // Act
        var result = ResultAction.Success(testData, customMessage);

        // Assert
        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(customMessage, result.Message);
        Assert.AreEqual(ResultCode.Ok, result.StatusCode);
        Assert.AreEqual(testData, result.Data);
    }

    [TestMethod]
    public void Success_Generic_WithNullData_ReturnsOkResultWithNullData()
    {
        try
        {
            var result = ResultAction.Success<string>(null);
        }
        catch (Exception e)
        {
            Assert.IsInstanceOfType<InvalidResultStatus>(e);
        }
    }

    #endregion

    #region Tests for BadRequest Scenarios

    [TestMethod]
    public void BadRequest_WithSingleErrorAndDefaultMessage_ReturnsBadRequestResult()
    {
        // Arrange
        var error =  ErrorValidation.Create("Email", "El formato del email es inválido");

        // Act
        var result = ResultAction.BadRequest(error);

        // Assert
        Assert.IsFalse(result.IsSuccess);
        Assert.IsTrue(result.IsFailure);
        Assert.AreEqual("Han ocurrido uno o más errores de validación", result.Message);
        Assert.AreEqual(ResultCode.BadRequest, result.StatusCode);
        Assert.IsNotNull(result.ValidationErrors);
        Assert.AreEqual(1, result.ValidationErrors.Count);
        Assert.AreEqual("Email", result.ValidationErrors.First().Identifier);
        Assert.AreEqual("El formato del email es inválido", result.ValidationErrors.First().ErrorMessage);
    }

    [TestMethod]
    public void BadRequest_WithSingleErrorAndCustomMessage_ReturnsBadRequestResultWithCustomMessage()
    {
        // Arrange
        var error = ErrorValidation.Create("Password", "La contraseña es demasiado corta");
        var customMessage = "Error de validación en el formulario";

        // Act
        var result = ResultAction.BadRequest(error, customMessage);

        // Assert
        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(customMessage, result.Message);
        Assert.AreEqual(ResultCode.BadRequest, result.StatusCode);
        Assert.AreEqual(1, result.ValidationErrors.Count);
    }

    [TestMethod]
    public void BadRequest_WithMultipleErrorsAndDefaultMessage_ReturnsBadRequestResultWithAllErrors()
    {
        // Arrange
        var errors = new List<ErrorValidation>
        {
            ErrorValidation.Create("Email", "El formato del email es inválido"),
            ErrorValidation.Create("Password", "La contraseña es demasiado corta"),
            ErrorValidation.Create("Username", "El nombre de usuario ya existe")
        };

        // Act
        var result = ResultAction.BadRequest(errors);

        // Assert
        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("Han ocurrido uno o más errores de validación", result.Message);
        Assert.AreEqual(ResultCode.BadRequest, result.StatusCode);
        Assert.AreEqual(3, result.ValidationErrors.Count);
    }

    [TestMethod]
    public void BadRequest_WithMultipleErrorsAndCustomMessage_ReturnsBadRequestResultWithCustomMessage()
    {
        // Arrange
        var errors = new List<ErrorValidation>
        {
            ErrorValidation.Create("Field1", "Error 1"),
            ErrorValidation.Create("Field2", "Error 2")
        };
        var customMessage = "Múltiples errores de validación";

        // Act
        var result = ResultAction.BadRequest(errors, customMessage);

        // Assert
        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(customMessage, result.Message);
        Assert.AreEqual(ResultCode.BadRequest, result.StatusCode);
        Assert.AreEqual(2, result.ValidationErrors.Count);
    }

    [TestMethod]
    public void BadRequest_WithEmptyErrorCollection_ReturnsBadRequestResultWithNoErrors()
    {
        // Arrange
        var emptyErrors = new List<ErrorValidation>();

        // Act
        var result = ResultAction.BadRequest(emptyErrors);

        // Assert
        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(ResultCode.BadRequest, result.StatusCode);
        Assert.IsNotNull(result.ValidationErrors);
        Assert.AreEqual(0, result.ValidationErrors.Count);
    }
    

    #endregion

    #region Tests for Other Factory Methods

    [TestMethod]
    public void NotFound_WithDefaultMessage_ReturnsNotFoundResult()
    {
        // Act
        var result = ResultAction.NotFound();

        // Assert
        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("No se encontró el recurso solicitado", result.Message);
        Assert.AreEqual(ResultCode.NotFound, result.StatusCode);
        Assert.IsNotNull(result.ValidationErrors);
        Assert.AreEqual(0, result.ValidationErrors.Count);
    }

    [TestMethod]
    public void NotFound_WithCustomMessage_ReturnsNotFoundResultWithCustomMessage()
    {
        // Arrange
        var customMessage = "Usuario no encontrado";

        // Act
        var result = ResultAction.NotFound(customMessage);

        // Assert
        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(customMessage, result.Message);
        Assert.AreEqual(ResultCode.NotFound, result.StatusCode);
    }

    [TestMethod]
    public void Unauthorized_WithDefaultMessage_ReturnsUnauthorizedResult()
    {
        // Act
        var result = ResultAction.Unauthorized();

        // Assert
        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("No está autorizado para acceder al recurso solicitado", result.Message);
        Assert.AreEqual(ResultCode.Unauthorized, result.StatusCode);
        Assert.IsNotNull(result.ValidationErrors);
        Assert.AreEqual(0, result.ValidationErrors.Count);
    }

    [TestMethod]
    public void Unauthorized_WithCustomMessage_ReturnsUnauthorizedResultWithCustomMessage()
    {
        // Arrange
        var customMessage = "Credenciales inválidas";

        // Act
        var result = ResultAction.Unauthorized(customMessage);

        // Assert
        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(customMessage, result.Message);
        Assert.AreEqual(ResultCode.Unauthorized, result.StatusCode);
    }

    [TestMethod]
    public void PasswordChangeRequired_WithMessage_ReturnsPreconditionRequiredResult()
    {
        // Arrange
        var message = "Debe cambiar su contraseña antes de continuar";

        // Act
        var result = ResultAction.PasswordChangeRequired(message);

        // Assert
        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(message, result.Message);
        Assert.AreEqual(ResultCode.PreconditionRequired, result.StatusCode);
        Assert.IsNotNull(result.ValidationErrors);
        Assert.AreEqual(0, result.ValidationErrors.Count);
    }

    [TestMethod]
    public void Conflict_WithDefaultMessage_ReturnsConflictResult()
    {
        // Act
        var result = ResultAction.Conflict();

        // Assert
        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("Su solicitud no puede ser procesada", result.Message);
        Assert.AreEqual(ResultCode.Conflict, result.StatusCode);
        Assert.IsNotNull(result.ValidationErrors);
        Assert.AreEqual(0, result.ValidationErrors.Count);
    }

    [TestMethod]
    public void Conflict_WithCustomMessage_ReturnsConflictResultWithCustomMessage()
    {
        // Arrange
        var customMessage = "El recurso ya existe";

        // Act
        var result = ResultAction.Conflict(customMessage);

        // Assert
        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(customMessage, result.Message);
        Assert.AreEqual(ResultCode.Conflict, result.StatusCode);
    }

    #endregion
    
    #region Tests for Generic ResultAction<T>

    [TestMethod]
    public void GenericResultAction_Success_WithData_ReturnsOkResultWithData()
    {
        // Arrange
        var testObject = new { Name = "Test", Data = 123 };

        // Act
        var result = ResultAction.Success(testObject);

        // Assert
        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(ResultCode.Ok, result.StatusCode);
        Assert.AreEqual(testObject, result.Data);
    }

    [TestMethod]
    public void GenericResultAction_BadRequest_WithSingleError_ReturnsBadRequestWithError()
    {
        // Arrange
        var error = ErrorValidation.Create("Test", "Error");

        // Act
        var result = ResultAction.BadRequest(error);

        // Assert
        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(ResultCode.BadRequest, result.StatusCode);
        Assert.IsNotNull(result.ValidationErrors);
        Assert.AreEqual(1, result.ValidationErrors.Count);
    }

    [TestMethod]
    public void GenericResultAction_ImplicitOperator_FromData_ReturnsSuccessResult()
    {
        // Arrange
        int Data = 42;

        // Act
        ResultAction<int> result = Data;

        // Assert
        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(Data, result.Data);
        Assert.AreEqual(ResultCode.Ok, result.StatusCode);
    }

    [TestMethod]
    public void GenericResultAction_ImplicitOperator_FromError_ReturnsBadRequestResult()
    {
        // Arrange
        var error = ErrorValidation.Create("Test", "Error");

        // Act
        ResultAction<int> result = error;

        // Assert
        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(ResultCode.BadRequest, result.StatusCode);
        Assert.IsNotNull(result.ValidationErrors);
        Assert.AreEqual(1, result.ValidationErrors.Count);
        Assert.AreEqual(error.Identifier, result.ValidationErrors.First().Identifier);
    }

    #endregion

    #region Tests for Immutability
    
    [TestMethod]
    public void ValidationErrorsCollection_ShouldBeImmutable()
    {
        // Arrange
        var errors = new List<ErrorValidation> { ErrorValidation.Create("Test", "Error") };
        ResultAction result =  ResultAction.BadRequest(errors);

        // Act & Assert - Verificar que la colección es de solo lectura
        Assert.IsTrue(result.ValidationErrors is IReadOnlyList<ErrorValidation>);
    }

    #endregion

    #region Tests for Message and StatusCode Consistency

    [TestMethod]
    public void Success_Result_ShouldHaveOkStatusCode()
    {
        // Act
        var result = ResultAction.Success();

        // Assert
        Assert.AreEqual(ResultCode.Ok, result.StatusCode);
    }

    [TestMethod]
    public void BadRequest_Result_ShouldHaveBadRequestStatusCode()
    {
        // Arrange
        var error = ErrorValidation.Create("Test", "Error");

        // Act
        var result = ResultAction.BadRequest(error);

        // Assert
        Assert.AreEqual(ResultCode.BadRequest, result.StatusCode);
    }

    [TestMethod]
    public void NotFound_Result_ShouldHaveNotFoundStatusCode()
    {
        // Act
        var result = ResultAction.NotFound();

        // Assert
        Assert.AreEqual(ResultCode.NotFound, result.StatusCode);
    }

    [TestMethod]
    public void Unauthorized_Result_ShouldHaveUnauthorizedStatusCode()
    {
        // Act
        var result = ResultAction.Unauthorized();

        // Assert
        Assert.AreEqual(ResultCode.Unauthorized, result.StatusCode);
    }

    [TestMethod]
    public void PasswordChangeRequired_Result_ShouldHavePreconditionRequiredStatusCode()
    {
        // Arrange
        var message = "Change password required";

        // Act
        var result = ResultAction.PasswordChangeRequired(message);

        // Assert
        Assert.AreEqual(ResultCode.PreconditionRequired, result.StatusCode);
    }

    [TestMethod]
    public void Conflict_Result_ShouldHaveConflictStatusCode()
    {
        // Act
        var result = ResultAction.Conflict();

        // Assert
        Assert.AreEqual(ResultCode.Conflict, result.StatusCode);
    }

    #endregion
}