namespace WordFinder.Test;

/// <summary>
/// Unit tests for WordFinder matrix validation logic.
/// </summary>
public class WordFinderValidationTests
{
    // Good practice: Use private constants or members for common test data/values
    private const int MaxMatrixDimension = 64;

    #region Constructor_MatrixSizeValidation
    /// <summary>
    /// Verifies that the WordFinder constructor throws an ArgumentException
    /// when the matrix exceeds 64 rows.
    /// </summary>
    [Fact]
    public void Ctor_ThrowsArgumentException_WhenMatrixHasTooManyRows()
    {
        // Arrange: Create a matrix with 65 rows, each with 10 columns
        var matrix = Enumerable.Repeat(new string('a', 10), MaxMatrixDimension + 1).ToList();

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => new WordFinderLib.WordFinder(matrix));
        Assert.Contains("Matrix cannot have more than 64 rows.", ex.Message);
        Assert.DoesNotContain("Matrix cannot have more than 64 columns.", ex.Message); // Ensure only relevant error
        Assert.DoesNotContain("All rows in the matrix must have the same length.", ex.Message);
    }

    /// <summary>
    /// Verifies that the WordFinder constructor throws an ArgumentException
    /// when any row in the matrix has more than 64 columns.
    /// </summary>
    [Fact]
    public void Ctor_ThrowsArgumentException_WhenMatrixHasTooManyColumns()
    {
        // Arrange: Create a matrix with 1 row, but it has 65 columns
        var matrix = new List<string> { new string('a', MaxMatrixDimension + 1) };

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => new WordFinderLib.WordFinder(matrix));
        Assert.Contains("Matrix cannot have more than 64 columns.", ex.Message);
        Assert.DoesNotContain("Matrix cannot have more than 64 rows.", ex.Message); // Ensure only relevant error
        Assert.DoesNotContain("All rows in the matrix must have the same length.", ex.Message);
    }

    /// <summary>
    /// Verifies that the WordFinder constructor throws an ArgumentException
    /// when the matrix exceeds both row and column limits.
    /// (This is covered by the multiple errors test, but good to have a focused one if the combined one is too complex)
    /// </summary>
    [Fact]
    public void Ctor_ThrowsArgumentException_WhenMatrixExceedsBothRowAndColumnLimits()
    {
        // Arrange: 65x65 matrix
        var matrix = Enumerable.Repeat(new string('a', MaxMatrixDimension + 1), MaxMatrixDimension + 1).ToList();

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => new WordFinderLib.WordFinder(matrix));
        Assert.Contains("Matrix cannot have more than 64 rows.", ex.Message);
        Assert.Contains("Matrix cannot have more than 64 columns.", ex.Message);
        // This test should ideally only contain these two, but your code might report "inconsistent length"
        // if it processes rows with varying lengths while trying to hit max column for other rows.
        // For a perfectly square too-large matrix, only row/col messages should appear.
    }
    #endregion

    #region Constructor_RowConsistencyValidation
    /// <summary>
    /// Verifies that the WordFinder constructor throws an ArgumentException
    /// when the matrix rows have different lengths.
    /// </summary>
    [Fact]
    public void Ctor_ThrowsArgumentException_WhenRowsHaveDifferentLengths()
    {
        // Arrange
        var unevenMatrix = new List<string>
        {
            "abcd",
            "efg",    // shorter row
            "hijk"
        };

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => new WordFinderLib.WordFinder(unevenMatrix));
        Assert.Contains("All rows in the matrix must have the same length.", ex.Message);
        Assert.DoesNotContain("Matrix cannot be null.", ex.Message);
        Assert.DoesNotContain("Matrix cannot be empty.", ex.Message);
        Assert.DoesNotContain("Matrix cannot have more than 64 rows.", ex.Message);
        Assert.DoesNotContain("Matrix cannot have more than 64 columns.", ex.Message);
        Assert.DoesNotContain("Matrix cannot contain null rows.", ex.Message);
        Assert.DoesNotContain("Matrix cannot contain empty rows.", ex.Message);
    }
    #endregion

    #region Constructor_NullOrEmptyMatrixValidation
    /// <summary>
    /// Verifies that the WordFinder constructor throws an ArgumentException when the matrix is null.
    /// </summary>
    [Fact]
    public void Ctor_ThrowsArgumentException_WhenMatrixIsNull()
    {
        // Arrange
        IEnumerable<string> matrix = null;

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => new WordFinderLib.WordFinder(matrix));
        Assert.Contains("Matrix cannot be null.", ex.Message);
        Assert.Equal("Matrix cannot be null.", ex.Message.Trim());
    }

    /// <summary>
    /// Verifies that the WordFinder constructor throws an ArgumentException when the matrix is empty.
    /// </summary>
    [Fact]
    public void Ctor_ThrowsArgumentException_WhenMatrixIsEmpty()
    {
        // Arrange
        var emptyMatrix = new List<string>();

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => new WordFinderLib.WordFinder(emptyMatrix));
        Assert.Contains("Matrix cannot be empty.", ex.Message);
        Assert.Equal("Matrix cannot be empty.", ex.Message.Trim());
    }
    #endregion

    #region Constructor_NullOrEmptyRowsValidation
    /// <summary>
    /// Verifies that the constructor throws ArgumentException when matrix contains null rows.
    /// </summary>
    [Fact]
    public void Ctor_ThrowsArgumentException_WhenMatrixContainsNullRows()
    {
        // Arrange
        var matrix = new List<string> { "abcd", null, "efgh" };

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => new WordFinderLib.WordFinder(matrix));
        Assert.Contains("Matrix cannot contain null rows.", ex.Message);
        Assert.DoesNotContain("Matrix cannot have more than 64 columns.", ex.Message); // Ensure no other errors if input size is small
    }

    /// <summary>
    /// Verifies that the constructor throws ArgumentException when matrix contains empty rows.
    /// </summary>
    [Fact]
    public void Ctor_ThrowsArgumentException_WhenMatrixContainsEmptyRows()
    {
        // Arrange
        var matrix = new List<string> { "abcd", "", "efgh" };

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => new WordFinderLib.WordFinder(matrix));
        Assert.Contains("Matrix cannot contain empty rows.", ex.Message);
        Assert.DoesNotContain("Matrix cannot have more than 64 columns.", ex.Message); // Ensure no other errors if input size is small
    }

    /// <summary>
    /// Verifies that the constructor throws ArgumentException when matrix contains *only* null rows.
    /// (This also covers empty matrix if all rows are null)
    /// </summary>
    [Fact]
    public void Ctor_ThrowsArgumentException_WhenMatrixContainsOnlyNullRows()
    {
        // Arrange
        var matrix = new List<string> { null, null };

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => new WordFinderLib.WordFinder(matrix));
        Assert.Contains("Matrix cannot contain null rows.", ex.Message);
        // Note: Depending on your validation order, "Matrix cannot be empty." might also appear
        // if all rows are null and you early-exit for empty after converting to array.
        // Your current code should produce "Matrix cannot contain null rows." as the primary,
        // and potentially "Matrix cannot contain empty rows." if it treats null as empty for that specific check.
        // The most important thing is that a relevant error is reported.
    }

    /// <summary>
    /// Verifies that the constructor throws ArgumentException when matrix contains *only* empty rows.
    /// </summary>
    [Fact]
    public void Ctor_ThrowsArgumentException_WhenMatrixContainsOnlyEmptyRows()
    {
        // Arrange
        var matrix = new List<string> { "", "" };

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => new WordFinderLib.WordFinder(matrix));
        Assert.Contains("Matrix cannot contain empty rows.", ex.Message);
        // Depending on validation order, "All rows in the matrix must have the same length." might also appear
        // if it processes "" and then checks length consistency against itself or another "".
        // The core issue ("empty rows") should be asserted.
    }

    /// <summary>
    /// Verifies that the constructor throws ArgumentException when matrix contains a mix of valid and invalid rows
    /// leading to length inconsistency from valid rows (e.g., all invalid rows are filtered, then valid ones are inconsistent).
    /// This tests a specific path in your `ValidateMatrix` where `nonNullEmptyRows` might be handled.
    /// </summary>
    [Fact]
    public void Ctor_ThrowsArgumentException_WhenValidRowsHaveInconsistentLength_AmidstInvalidRows()
    {
        // Arrange
        var matrix = new List<string>
        {
            "abc",
            null,
            "",
            "abcd" // This makes it inconsistent with "abc" after null/empty are filtered
        };

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => new WordFinderLib.WordFinder(matrix));
        Assert.Contains("All rows in the matrix must have the same length.", ex.Message);
        Assert.Contains("Matrix cannot contain null rows.", ex.Message);
        Assert.Contains("Matrix cannot contain empty rows.", ex.Message);
    }
    #endregion

    #region Constructor_MultipleValidationErrors
    /// <summary>
    /// Verifies that the WordFinder constructor throws an exception containing all validation errors when multiple issues are present in the matrix.
    /// This is a comprehensive test for the multi-error reporting feature.
    /// </summary>
    [Fact]
    public void Ctor_ThrowsArgumentException_WithAllValidationErrors()
    {
        // Arrange: Create a matrix that violates all rules possible at once
        var invalidMatrix = new List<string>
        {
            new string('a', MaxMatrixDimension + 1), // Too many columns
            "short",                                 // Inconsistent length
            null,                                    // Null row
            "",                                      // Empty row
            new string('b', MaxMatrixDimension + 1)  // Another too many columns
        };
        // Add more than 64 rows to exceed the row limit
        while (invalidMatrix.Count <= MaxMatrixDimension + 1) // Ensure it's slightly more than 64 rows
            invalidMatrix.Add(new string('c', 5)); // Add short rows to not cause new column errors

        // Act & Assert: Should throw ArgumentException with all relevant error messages
        var ex = Assert.Throws<ArgumentException>(() => new WordFinderLib.WordFinder(invalidMatrix));

        // Assert that all expected error messages are present
        Assert.Contains("Matrix cannot have more than 64 rows.", ex.Message);
        Assert.Contains("Matrix cannot have more than 64 columns.", ex.Message);
        Assert.Contains("All rows in the matrix must have the same length.", ex.Message);
        Assert.Contains("Matrix cannot contain null rows.", ex.Message);
        Assert.Contains("Matrix cannot contain empty rows.", ex.Message);
    }
    #endregion

    #region Constructor_ValidMatrix
    /// <summary>
    /// Verifies that the WordFinder constructor initializes successfully with a valid matrix.
    /// This is a "happy path" test to ensure no unexpected exceptions.
    /// </summary>
    [Fact]
    public void Ctor_DoesNotThrowException_WhenMatrixIsValid()
    {
        // Arrange: A perfectly valid matrix
        var validMatrix = new List<string>
        {
            "abcd",
            "efgh",
            "ijkl"
        };

        // Act & Assert
        var exception = Record.Exception(() => new WordFinderLib.WordFinder(validMatrix));
        Assert.Null(exception); // Assert no exception was thrown
    }

    /// <summary>
    /// Verifies that the WordFinder constructor initializes successfully with a 64x64 valid matrix.
    /// </summary>
    [Fact]
    public void Ctor_DoesNotThrowException_WhenMatrixIsMaxValidSize()
    {
        // Arrange: A 64x64 matrix
        var maxMatrix = Enumerable.Repeat(new string('a', MaxMatrixDimension), MaxMatrixDimension).ToList();

        // Act & Assert
        var exception = Record.Exception(() => new WordFinderLib.WordFinder(maxMatrix));
        Assert.Null(exception); // Assert no exception was thrown
    }
    #endregion
}
