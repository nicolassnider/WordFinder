namespace WordFinder.Test;
/// <summary>
/// Unit tests for the WordFinder class.
/// </summary>
public class WordFinderFunctionalTests
{
    #region Find_ReturnsTop10MostFrequentWords_FoundInMatrix
    /// <summary>
    /// Verifies that Find returns the top 10 most frequent words from the word stream that are found in the matrix.
    /// Ensures correct ordering by frequency and that only words present in the matrix are returned.
    /// </summary>
    [Fact]
    public void Find_ReturnsTop10MostFrequentWords_FoundInMatrix()
    {
        // Arrange: Prepare a matrix that actually contains the expected words horizontally.
        var matrix = new List<string>
    {
        "coldy",
        "windy",
        "chill",
        "uvxyy"
    };
        var wordStream = new List<string>
    {
        "cold", "wind", "snow", "chill", "cold", "wind", "wind"
    };
        var wordFinder = new WordFinder.WordFinderLib.WordFinder(matrix);

        // Act: Search for the words in the matrix.
        var result = wordFinder.Find(wordStream);

        // Assert: Only "wind", "cold", and "chill" are found, ordered by frequency in the stream.
        var expected = new List<string> { "wind", "cold", "chill" };
        Assert.Equal(expected, result);
    }
    #endregion

    #region Find_ReturnsEmpty_WhenNoWordsFound
    /// <summary>
    /// Verifies that Find returns an empty result when none of the words in the word stream are present in the matrix.
    /// </summary>
    [Fact]
    public void Find_ReturnsEmpty_WhenNoWordsFound()
    {
        // Arrange: Prepare a matrix and a word stream with no matching words.
        var matrix = new List<string>
        {
            "abcd",
            "efgh",
            "ijkl",
            "mnop"
        };
        var wordStream = new List<string> { "xyz", "uvw" };
        var wordFinder = new WordFinder.WordFinderLib.WordFinder(matrix);

        // Act: Search for the words in the matrix.
        var result = wordFinder.Find(wordStream);

        // Assert: No words should be found.
        Assert.Empty(result);
    }
    #endregion

    #region Find_IgnoresDuplicateWordsInStream
    /// <summary>
    /// Verifies that duplicate words in the word stream are only counted once in the search results.
    /// </summary>
    [Fact]
    public void Find_IgnoresDuplicateWordsInStream()
    {
        // Arrange: Prepare a matrix and a word stream with the same word repeated.
        var matrix = new List<string>
        {
            "abcd",
            "efgh",
            "ijkl",
            "mnop"
        };
        var wordStream = new List<string> { "abcd", "abcd", "abcd" };
        var wordFinder = new WordFinder.WordFinderLib.WordFinder(matrix);

        // Act: Search for the words in the matrix.
        var result = wordFinder.Find(wordStream);

        // Assert: The result should only contain "abcd" once.
        var expected = new List<string> { "abcd" };
        Assert.Equal(expected, result);
    }
    #endregion

    #region Find_ReturnsAtMost10Words
    /// <summary>
    /// Verifies that Find never returns more than 10 words, even if more are found in the matrix.
    /// </summary>
    [Fact]
    public void Find_ReturnsAtMost10Words()
    {
        // Arrange: Prepare a matrix and a word stream with more than 10 possible matches.
        var matrix = new List<string>
        {
            "abcd",
            "efgh",
            "ijkl",
            "mnop"
        };
        var wordStream = new List<string>
        {
            "abcd", "efgh", "ijkl", "mnop", "bcde", "fghi", "jklm", "nopq", "cdef", "ghij", "klmn", "opqr"
        };
        var wordFinder = new WordFinder.WordFinderLib.WordFinder(matrix);

        // Act: Search for the words in the matrix.
        var result = wordFinder.Find(wordStream);

        // Assert: The result should contain at most 10 words.
        Assert.True(result is ICollection<string> collection && collection.Count <= 10);
    }
    #endregion

    #region Find_IsCaseInsensitive
    /// <summary>
    /// Verifies that Find is case-insensitive when matching words in the matrix.
    /// </summary>
    [Fact]
    public void Find_IsCaseInsensitive()
    {
        // Arrange: Prepare a matrix and a word stream with different casing.
        var matrix = new List<string>
        {
            "AbCd",
            "EfGh",
            "IjKl",
            "MnOp"
        };
        var wordStream = new List<string> { "abcd", "EFGH", "ijkl", "mnop" };
        var wordFinder = new WordFinder.WordFinderLib.WordFinder(matrix);

        // Act: Search for the words in the matrix.
        var result = wordFinder.Find(wordStream);

        // Assert: All words should be found regardless of case.
        var expected = new List<string> { "abcd", "EFGH", "ijkl", "mnop" };
        Assert.Equal(expected, result);
    }
    #endregion

    #region Find_WorksWithLargeMatrix
    /// <summary>
    /// Verifies that WordFinder works correctly with a large 64x64 matrix and finds words horizontally and vertically.
    /// </summary>
    [Fact]
    public void Find_WorksWithLargeMatrix()
    {
        // Arrange: Create a 64x64 matrix filled with 'a', but insert specific words horizontally and vertically.
        int size = 64;
        var matrix = new List<string>();
        string horizontalWord = "horizontal";
        string verticalWord = "vertical";
        for (int i = 0; i < size; i++)
        {
            // Place "horizontal" at row 10, starting at column 5
            if (i == 10)
            {
                var row = new char[size];
                for (int j = 0; j < size; j++)
                    row[j] = 'a';
                for (int j = 0; j < horizontalWord.Length; j++)
                    row[5 + j] = horizontalWord[j];
                matrix.Add(new string(row));
            }
            else
            {
                matrix.Add(new string('a', size));
            }
        }
        // Place "vertical" at column 20, starting at row 15
        for (int k = 0; k < verticalWord.Length; k++)
        {
            var rowChars = matrix[15 + k].ToCharArray();
            rowChars[20] = verticalWord[k];
            matrix[15 + k] = new string(rowChars);
        }

        var wordStream = new List<string>
    {
        "horizontal", "vertical", "notfound"
    };
        var wordFinder = new WordFinderLib.WordFinder(matrix);

        // Act: Search for the words in the large matrix.
        var result = wordFinder.Find(wordStream);

        // Assert: Only "horizontal" and "vertical" should be found.
        var expected = new List<string> { "horizontal", "vertical" };
        Assert.Equal(expected, result);
    }
    #endregion



    #region Find_ReturnsEmpty_WhenWordStreamIsNull
    /// <summary>
    /// Verifies that Find returns an empty enumerable when the word stream is null.
    /// </summary>
    [Fact]
    public void Find_ReturnsEmpty_WhenWordStreamIsNull()
    {
        // Arrange: Prepare a valid matrix
        var matrix = new List<string>
    {
        "abcd",
        "efgh",
        "ijkl",
        "mnop"
    };
        var wordFinder = new WordFinderLib.WordFinder(matrix);

        // Act: Call Find with a null wordstream
        var result = wordFinder.Find(null);

        // Assert: The result should be empty
        Assert.Empty(result);
    }
    #endregion
}

