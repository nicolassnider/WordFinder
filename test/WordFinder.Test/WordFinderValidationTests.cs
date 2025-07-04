namespace WordFinder.Test;
/// <summary>
/// Unit tests for WordFinder matrix validation logic.
/// </summary>
public class WordFinderValidationTests
{
    #region Constructor_ThrowsException_WhenMatrixIsLargerThan64x64
    /// <summary>
    /// Verifies that the WordFinder constructor throws an exception when the matrix exceeds 64 rows or any row exceeds 64 columns.
    /// </summary>
    [Fact]
    public void Constructor_ThrowsException_WhenMatrixIsLargerThan64x64()
    {
        // Arrange: Create a matrix with 65 rows, each with 64 columns (exceeds row limit)
        var tooManyRows = new List<string>();
        for (int i = 0; i < 65; i++)
            tooManyRows.Add(new string('a', 64));

        // Arrange: Create a matrix with 64 rows, but each row has 65 columns (exceeds column limit)
        var tooManyCols = new List<string>();
        for (int i = 0; i < 64; i++)
            tooManyCols.Add(new string('a', 65));

        // Act & Assert: Both should throw ArgumentException
        Assert.Throws<ArgumentException>(() => new WordFinderLib.WordFinder(tooManyRows));
        Assert.Throws<ArgumentException>(() => new WordFinderLib.WordFinder(tooManyCols));
    }
    #endregion

    #region Constructor_ThrowsException_WhenRowsHaveDifferentLengths
    /// <summary>
    /// Verifies that the WordFinder constructor throws an exception when the matrix rows have different lengths.
    /// </summary>
    [Fact]
    public void Constructor_ThrowsException_WhenRowsHaveDifferentLengths()
    {
        // Arrange: Create a matrix with uneven row lengths
        var unevenMatrix = new List<string>
        {
            "abcd",
            "efg",   // shorter row
            "hijk",
            "lmno"
        };

        // Act & Assert: Should throw ArgumentException
        Assert.Throws<ArgumentException>(() => new WordFinderLib.WordFinder(unevenMatrix));
    }
    #endregion

    #region Constructor_ThrowsException_WhenMatrixIsEmpty
    /// <summary>
    /// Verifies that the WordFinder constructor throws an exception when the matrix is empty.
    /// </summary>
    [Fact]
    public void Constructor_ThrowsException_WhenMatrixIsEmpty()
    {
        // Arrange: Create an empty matrix
        var emptyMatrix = new List<string>();

        // Act & Assert: Should throw ArgumentException
        Assert.Throws<ArgumentException>(() => new WordFinderLib.WordFinder(emptyMatrix));
    }
    #endregion

    #region Constructor_ThrowsException_WhenMatrixIsNull
    /// <summary>
    /// Verifies that the WordFinder constructor throws an exception when the matrix is null.
    /// </summary>
    [Fact]
    public void Constructor_ThrowsException_WhenMatrixIsNull()
    {
        // Act & Assert: Should throw ArgumentException when matrix is null
        Assert.Throws<ArgumentException>(() => new WordFinderLib.WordFinder(null));
    }
    #endregion

    #region Constructor_ThrowsException_WhenMatrixHasMoreThan64Columns
    /// <summary>
    /// Verifies that the WordFinder constructor throws an exception when any row in the matrix has more than 64 columns.
    /// </summary>
    [Fact]
    public void Constructor_ThrowsException_WhenMatrixHasMoreThan64Columns()
    {
        // Arrange: Create a matrix with one row longer than 64 characters
        var tooManyColumnsMatrix = new List<string>
        {
            new string('a', 65), // 65 columns
            new string('b', 65)
        };

        // Act & Assert: Should throw ArgumentException
        Assert.Throws<ArgumentException>(() => new WordFinderLib.WordFinder(tooManyColumnsMatrix));
    }
    #endregion

    #region Constructor_ThrowsException_WithMultipleValidationErrors
    /// <summary>
    /// Verifies that the WordFinder constructor throws an exception containing all validation errors when multiple issues are present in the matrix.
    /// </summary>
    [Fact]
    public void Constructor_ThrowsException_WithMultipleValidationErrors()
    {
        // Arrange: Create a matrix with too many columns, uneven row lengths, too many rows, a null row, and an empty row
        var invalidMatrix = new List<string>
        {
            new string('a', 65), // 65 columns (too many)
            "short",             // uneven length
            null,                // null row
            "",                  // empty row
            new string('b', 65), // 65 columns (too many)
        };
        // Add more than 64 rows to exceed the row limit
        while (invalidMatrix.Count < 66)
            invalidMatrix.Add(new string('c', 65));

        // Act & Assert: Should throw ArgumentException with all relevant error messages
        var ex = Assert.Throws<ArgumentException>(() => new WordFinderLib.WordFinder(invalidMatrix));
        Assert.Contains("Matrix cannot have more than 64 rows.", ex.Message);
        Assert.Contains("Matrix cannot have more than 64 columns.", ex.Message);
        Assert.Contains("All rows in the matrix must have the same length.", ex.Message);
        Assert.Contains("Matrix cannot contain null rows.", ex.Message);
        Assert.Contains("Matrix cannot contain empty rows.", ex.Message);
    }
    #endregion
}

