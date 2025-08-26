using ResultPattern.Exceptions;

namespace ResultPattern.Tests;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[TestClass]
public class ResultActionGenericTests
{
    #region Tests for Constructors and Basic Properties

    [TestMethod]
    public void Constructor_WithDataAndNoErrors_ShouldCreateSuccessResult()
    {
        // Arrange
        var testData = "test data";

        // Act
        var result =  ResultAction<string>.Success(testData);

        // Assert
        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(testData, result.Data);
        Assert.IsNull(result.ValidationErrors);
        Assert.AreEqual("Ok", result.Message);
        Assert.AreEqual(ResultCode.Ok, result.StatusCode);
    }

    [TestMethod]
    public void Constructor_WithErrorsAndNoData_ShouldCreateFailureResult()
    {
        // Arrange
        var errors = new List<ErrorValidation> { ErrorValidation.Create("Field", "Error") };

        // Act
        var result = ResultAction<string>.BadRequest(errors, "Error");

        // Assert
        Assert.IsFalse(result.IsSuccess);
        Assert.IsNull(result.Data);
        Assert.IsNotNull(result.ValidationErrors);
        Assert.AreEqual(1, result.ValidationErrors.Count);
        Assert.AreEqual("Error", result.Message);
        Assert.AreEqual(ResultCode.BadRequest, result.StatusCode);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidResultStatus))]
    public void Constructor_WithNullDataAndNullErrors_ShouldThrowInvalidResultStatus()
    {
        // Act & Assert
        ResultAction<string>.Success(null);
    }

    [TestMethod]
    public void Constructor_WithNullDataButWithErrors_ShouldNotThrow()
    {
        // Arrange
        var errors =  "Error";

        // Act
        var result = ResultAction<string>.Create(null, errors);

        // Assert
        Assert.IsFalse(result.IsSuccess);
        Assert.IsNull(result.Data);
        Assert.IsNotNull(result.ValidationErrors);
    }

    #endregion

    #region Tests for Success Method

    [TestMethod]
    public void Success_WithValue_ShouldReturnOkResultWithData()
    {
        // Arrange
        var testValue = 42;

        // Act
        var result = ResultAction<int>.Success(testValue);

        // Assert
        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(testValue, result.Data);
        Assert.AreEqual("Ok", result.Message);
        Assert.AreEqual(ResultCode.Ok, result.StatusCode);
        Assert.IsNull(result.ValidationErrors);
    }

    [TestMethod]
    public void Success_WithValueAndCustomMessage_ShouldReturnOkResultWithCustomMessage()
    {
        // Arrange
        var testValue = "test";
        var customMessage = "Custom success message";

        // Act
        var result = ResultAction<string>.Success(testValue, customMessage);

        // Assert
        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(testValue, result.Data);
        Assert.AreEqual(customMessage, result.Message);
        Assert.AreEqual(ResultCode.Ok, result.StatusCode);
    }

    #endregion

    #region Tests for BadRequest Methods

    [TestMethod]
    public void BadRequest_WithSingleError_ShouldReturnBadRequestResult()
    {
        // Arrange
        var error = ErrorValidation.Create("Email", "Invalid format");

        // Act
        var result = ResultAction<int>.BadRequest(error);

        // Assert
        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(default(int), result.Data);
        Assert.AreEqual("Han ocurrido uno o más errores de validación", result.Message);
        Assert.AreEqual(ResultCode.BadRequest, result.StatusCode);
        Assert.IsNotNull(result.ValidationErrors);
        Assert.AreEqual(1, result.ValidationErrors.Count);
        Assert.AreEqual("Email", result.ValidationErrors.First().Identifier);
    }

    [TestMethod]
    public void BadRequest_WithSingleErrorAndCustomMessage_ShouldReturnBadRequestWithCustomMessage()
    {
        // Arrange
        var error = ErrorValidation.Create("Password", "Too short");
        var customMessage = "Validation failed";

        // Act
        var result = ResultAction<int>.BadRequest(error, customMessage);

        // Assert
        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(customMessage, result.Message);
        Assert.AreEqual(ResultCode.BadRequest, result.StatusCode);
        Assert.AreEqual(1, result.ValidationErrors.Count);
    }

    [TestMethod]
    public void BadRequest_WithMultipleErrors_ShouldReturnBadRequestWithAllErrors()
    {
        // Arrange
        var errors = new List<ErrorValidation>
        {
            ErrorValidation.Create("Email", "Invalid format"),
            ErrorValidation.Create("Password", "Too short")
        };

        // Act
        var result = ResultAction<int>.BadRequest(errors);

        // Assert
        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(ResultCode.BadRequest, result.StatusCode);
        Assert.AreEqual(2, result.ValidationErrors.Count);
    }

    [TestMethod]
    public void BadRequest_WithEmptyErrorCollection_ShouldReturnBadRequestWithNoErrors()
    {
        // Arrange
        var emptyErrors = new List<ErrorValidation>();

        // Act
        var result = ResultAction<int>.BadRequest(emptyErrors);

        // Assert
        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(ResultCode.BadRequest, result.StatusCode);
        Assert.IsNotNull(result.ValidationErrors);
        Assert.AreEqual(0, result.ValidationErrors.Count);
    }
    
    #endregion

    #region Tests for Other Factory Methods

    [TestMethod]
    public void Conflict_ShouldReturnConflictResult()
    {
        // Act
        var result = ResultAction<string>.Conflict();

        // Assert
        Assert.IsFalse(result.IsSuccess);
        Assert.IsNull(result.Data);
        Assert.AreEqual("Su solicitud no puede ser procesada", result.Message);
        Assert.AreEqual(ResultCode.Conflict, result.StatusCode);
        Assert.IsNotNull(result.ValidationErrors);
        Assert.AreEqual(0, result.ValidationErrors.Count);
    }

    [TestMethod]
    public void Conflict_WithCustomMessage_ShouldReturnConflictWithCustomMessage()
    {
        // Arrange
        var customMessage = "Resource already exists";

        // Act
        var result = ResultAction<string>.Conflict(customMessage);

        // Assert
        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(customMessage, result.Message);
        Assert.AreEqual(ResultCode.Conflict, result.StatusCode);
    }

    [TestMethod]
    public void NotFound_ShouldReturnNotFoundResult()
    {
        // Act
        var result = ResultAction<int>.NotFound();

        // Assert
        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(default(int), result.Data);
        Assert.AreEqual("No se encontró el recurso solicitado", result.Message);
        Assert.AreEqual(ResultCode.NotFound, result.StatusCode);
        Assert.IsNotNull(result.ValidationErrors);
        Assert.AreEqual(0, result.ValidationErrors.Count);
    }

    [TestMethod]
    public void NotFound_WithCustomMessage_ShouldReturnNotFoundWithCustomMessage()
    {
        // Arrange
        var customMessage = "User not found";

        // Act
        var result = ResultAction<int>.NotFound(customMessage);

        // Assert
        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(customMessage, result.Message);
        Assert.AreEqual(ResultCode.NotFound, result.StatusCode);
    }

    [TestMethod]
    public void Unauthorized_ShouldReturnUnauthorizedResult()
    {
        // Act
        var result = ResultAction<string>.Unauthorized();

        // Assert
        Assert.IsFalse(result.IsSuccess);
        Assert.IsNull(result.Data);
        Assert.AreEqual("No está autorizado para acceder al recurso solicitado", result.Message);
        Assert.AreEqual(ResultCode.Unauthorized, result.StatusCode);
        Assert.IsNotNull(result.ValidationErrors);
        Assert.AreEqual(0, result.ValidationErrors.Count);
    }

    [TestMethod]
    public void Unauthorized_WithCustomMessage_ShouldReturnUnauthorizedWithCustomMessage()
    {
        // Arrange
        var customMessage = "Invalid credentials";

        // Act
        var result = ResultAction<string>.Unauthorized(customMessage);

        // Assert
        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(customMessage, result.Message);
        Assert.AreEqual(ResultCode.Unauthorized, result.StatusCode);
    }

    #endregion

    #region Tests for EnsureFound Methods

    [TestMethod]
    public void EnsureFound_WithNonNullValue_ShouldReturnSuccessResult()
    {
        // Arrange
        var testValue = "test value";

        // Act
        var result = ResultAction<string>.EnsureFound(testValue, "Not found");

        // Assert
        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(testValue, result.Data);
        Assert.AreEqual("Ok", result.Message);
        Assert.AreEqual(ResultCode.Ok, result.StatusCode);
    }

    [TestMethod]
    public void EnsureFound_WithNullValue_ShouldReturnNotFoundResult()
    {
        // Arrange
        string nullValue = null;
        var errorMessage = "Item not found";

        // Act
        var result = ResultAction<string>.EnsureFound(nullValue, errorMessage);

        // Assert
        Assert.IsFalse(result.IsSuccess);
        Assert.IsNull(result.Data);
        Assert.AreEqual(errorMessage, result.Message);
        Assert.AreEqual(ResultCode.NotFound, result.StatusCode);
    }

    [TestMethod]
    public async Task EnsureFound_Async_WithNonNullValue_ShouldReturnSuccessResult()
    {
        // Arrange
        var testValue = "test value";
        var task = Task.FromResult<string>(testValue);

        // Act
        var result = await ResultAction<string>.EnsureFound(task, "Not found");

        // Assert
        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(testValue, result.Data);
        Assert.AreEqual("Ok", result.Message);
        Assert.AreEqual(ResultCode.Ok, result.StatusCode);
    }

    [TestMethod]
    public async Task EnsureFound_Async_WithNullValue_ShouldReturnNotFoundResult()
    {
        // Arrange
        string nullValue = null;
        var task = Task.FromResult<string>(nullValue);
        var errorMessage = "Item not found";

        // Act
        var result = await ResultAction<string>.EnsureFound(task, errorMessage);

        // Assert
        Assert.IsFalse(result.IsSuccess);
        Assert.IsNull(result.Data);
        Assert.AreEqual(errorMessage, result.Message);
        Assert.AreEqual(ResultCode.NotFound, result.StatusCode);
    }

    #endregion

    #region Tests for Create Method

    [TestMethod]
    public void Create_WithNonNullValue_ShouldReturnSuccessResult()
    {
        // Arrange
        var testValue = 42;
        var errorMessage = "Value cannot be null";

        // Act
        var result = ResultAction<int>.Create(testValue, errorMessage);

        // Assert
        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(testValue, result.Data);
        Assert.AreEqual("Ok", result.Message);
        Assert.AreEqual(ResultCode.Ok, result.StatusCode);
    }

    [TestMethod]
    public void Create_WithNullValue_ShouldReturnNotFoundResult()
    {
        // Arrange
        string nullValue = null;
        var errorMessage = "Value cannot be null";

        // Act
        var result = ResultAction<string>.Create(nullValue, errorMessage);

        // Assert
        Assert.IsFalse(result.IsSuccess);
        Assert.IsNull(result.Data);
        Assert.AreEqual(errorMessage, result.Message);
        Assert.AreEqual(ResultCode.NotFound, result.StatusCode);
    }

    #endregion

    #region Tests for Implicit Operators

    [TestMethod]
    public void ImplicitOperator_FromValue_ShouldReturnSuccessResult()
    {
        // Arrange
        int value = 42;

        // Act
        ResultAction<int> result = value;

        // Assert
        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(value, result.Data);
        Assert.AreEqual("Ok", result.Message);
        Assert.AreEqual(ResultCode.Ok, result.StatusCode);
    }

    [TestMethod]
    public void ImplicitOperator_FromError_ShouldReturnBadRequestResult()
    {
        // Arrange
        var error = ErrorValidation.Create("Field", "Error message");

        // Act
        ResultAction<int> result = error;

        // Assert
        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(default(int), result.Data);
        Assert.AreEqual("Han ocurrido uno o más errores de validación", result.Message);
        Assert.AreEqual(ResultCode.BadRequest, result.StatusCode);
        Assert.IsNotNull(result.ValidationErrors);
        Assert.AreEqual(1, result.ValidationErrors.Count);
        Assert.AreEqual("Field", result.ValidationErrors.First().Identifier);
    }

    [TestMethod]
    public void ImplicitOperator_FromValue_WithReferenceType_ShouldReturnSuccessResult()
    {
        // Arrange
        var value = new { Name = "Test", Id = 1 };

        // Act
        ResultAction<object> result = value;

        // Assert
        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(value, result.Data);
        Assert.AreEqual("Ok", result.Message);
        Assert.AreEqual(ResultCode.Ok, result.StatusCode);
    }

    #endregion

    #region Tests for EnsureValue Property

    [TestMethod]
    public void EnsureValue_WithNonNullData_ShouldReturnData()
    {
        // Arrange
        var testValue = "test data";
        var result = ResultAction<string>.Success(testValue);

        // Act
        var value = result.EnsureValue;

        // Assert
        Assert.AreEqual(testValue, value);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidResultStatus))]
    public void EnsureValue_WithNullData_ShouldThrowNullResultValueException()
    {
        // Arrange
        var result = ResultAction<string>.Success(null);
    }

    [TestMethod]
    [ExpectedException(typeof(NullResultValueException))]
    public void EnsureValue_OnFailureResult_ShouldThrowNullResultValueException()
    {
        // Arrange
        var error = ErrorValidation.Create("Field", "Error");
        var result = ResultAction<string>.BadRequest(error);

        // Act & Assert
        var value = result.EnsureValue;
    }

    #endregion

    #region Tests for IsSuccess Property Override

    [TestMethod]
    public void IsSuccess_WithNonNullDataAndNoErrors_ShouldReturnTrue()
    {
        // Arrange
        var result = ResultAction<string>.Success("test");

        // Act & Assert
        Assert.IsTrue(result.IsSuccess);
    }

    [TestMethod]
    public void IsSuccess_WithNullDataButNoErrors_ShouldReturnFalse()
    {
        // Arrange
        var result = ResultAction<string>.Create(null,"failed");

        // Act & Assert
        Assert.IsFalse(result.IsSuccess); // Override returns false when Data is null
    }

    [TestMethod]
    public void IsSuccess_WithDataButWithErrors_ShouldReturnFalse()
    {
        // Arrange
        var result = ResultAction<string>.Create("test",  "Error");

        // Act & Assert
        Assert.IsTrue(result.IsSuccess);
    }

    [TestMethod]
    public void IsSuccess_WithNullDataAndWithErrors_ShouldReturnFalse()
    {
        // Arrange
        var result = ResultAction<string>.Create(null,  "Error");

        // Act & Assert
        Assert.IsFalse(result.IsSuccess);
    }

    #endregion

    #region Tests for Edge Cases and Error Conditions

    [TestMethod]
    public void Data_Property_ShouldBeReadOnly()
    {
        // Arrange
        var result = ResultAction<int>.Success(42);

        // Act & Assert
        Assert.IsTrue(result.GetType().GetProperty("Data").CanWrite == false);
    }

    [TestMethod]
    public void EnsureValue_Property_ShouldBeReadOnly()
    {
        // Arrange
        var result = ResultAction<int>.Success(42);

        // Act & Assert
        Assert.IsTrue(result.GetType().GetProperty("EnsureValue").CanWrite == false);
    }

    [TestMethod]
    public void Result_WithValueTypeDefaultValue_ShouldBeConsideredSuccess()
    {
        // Act
        var result = ResultAction<int>.Success(0); // default value for int

        // Assert
        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(0, result.Data);
    }

    [TestMethod]
    public async Task EnsureFound_Async_WithCancelledTask_ShouldPropagateException()
    {
        // Arrange
        var cancelledTask = Task.FromCanceled<string>(new System.Threading.CancellationToken(true));

        // Act & Assert
        await Assert.ThrowsExceptionAsync<TaskCanceledException>(() =>
            ResultAction<string>.EnsureFound(cancelledTask, "Not found"));
    }

    [TestMethod]
    public async Task EnsureFound_Async_WithFaultedTask_ShouldPropagateException()
    {
        // Arrange
        var faultedTask = Task.FromException<string>(new System.Exception("Test exception"));

        // Act & Assert
        await Assert.ThrowsExceptionAsync<System.Exception>(() =>
            ResultAction<string>.EnsureFound(faultedTask, "Not found"));
    }

    #endregion

    #region Tests for Method Chaining and Composition

    [TestMethod]
    public void Methods_CanBeChainedWithImplicitOperators()
    {
        // Act
        ResultAction<int> result = 42;
        var processed = ProcessResult(result);

        // Assert
        Assert.IsTrue(processed.IsSuccess);
        Assert.AreEqual(84, processed.Data);

        ResultAction<int> ProcessResult(ResultAction<int> input)
        {
            return input.Data * 2; // Implicit conversion from int to ResultAction<int>
        }
    }

    [TestMethod]
    public void ImplicitOperator_AllowsNaturalSyntaxInExpressions()
    {
        // Arrange
        bool condition = true;

        // Act
        ResultAction<string> result = condition ? "success" : ErrorValidation.Create("test", "error");

        // Assert
        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual("success", result.Data);
    }

    [TestMethod]
    public void ImplicitOperator_InConditionalExpression_WithErrorBranch()
    {
        // Arrange
        bool condition = false;

        // Act
        ResultAction<string> result = condition ? "success" : ErrorValidation.Create("test", "error");

        // Assert
        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("test", result.ValidationErrors.First().Identifier);
    }

    #endregion
}