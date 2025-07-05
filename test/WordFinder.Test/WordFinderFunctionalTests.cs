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
        // Arrange
        var matrix = new List<string>
        {
            "coldy",
            "windy",
            "chill",
            "uvxyy"
        };
        var wordStream = new List<string>
        {
            "cold", "wind", "snow", "chill", "cold", "wind", "wind" // wind (3), cold (2), chill (1), snow (0)
        };
        var wordFinder = new WordFinderLib.WordFinder(matrix);

        // Act
        var result = wordFinder.Find(wordStream).ToList();

        // Assert: Only "wind", "cold", and "chill" are found, ordered by frequency in the stream.
        var expected = new List<string> { "wind", "cold", "chill" };
        Assert.Equal(expected, result);
        Assert.Equal(3, result.Count); // Explicitly assert count
    }
    #endregion

    #region Find_ReturnsEmpty_WhenNoWordsFound
    /// <summary>
    /// Verifies that Find returns an empty result when none of the words in the word stream are present in the matrix.
    /// </summary>
    [Fact]
    public void Find_ReturnsEmpty_WhenNoWordsFound()
    {
        // Arrange
        var matrix = new List<string>
        {
            "abcd",
            "efgh",
            "ijkl",
            "mnop"
        };
        var wordStream = new List<string> { "xyz", "uvw" };
        var wordFinder = new WordFinderLib.WordFinder(matrix);

        // Act
        var result = wordFinder.Find(wordStream).ToList();

        // Assert: No words should be found.
        Assert.Empty(result);
        Assert.Equal(0, result.Count);
    }
    #endregion

    #region Find_IgnoresDuplicateWordsInStream_CountsFrequencyCorrectly
    /// <summary>
    /// Verifies that duplicate words in the word stream are processed correctly for frequency counting,
    /// but only appear once in the distinct list of words to search.
    /// Ensures that the result correctly reflects the top N by stream frequency, not just presence in matrix.
    /// </summary>
    [Fact]
    public void Find_IgnoresDuplicateWordsInStream_CountsFrequencyCorrectly()
    {
        // Arrange
        var matrix = new List<string>
        {
            "abcd",
            "efgh"
        };
        // "abcd" appears 3 times, "efgh" appears 2 times
        var wordStream = new List<string> { "abcd", "efgh", "abcd", "efgh", "abcd" };
        var wordFinder = new WordFinderLib.WordFinder(matrix);

        // Act
        var result = wordFinder.Find(wordStream).ToList();

        // Assert: The result should be "abcd" then "efgh", ordered by frequency in stream.
        var expected = new List<string> { "abcd", "efgh" };
        Assert.Equal(expected, result);
        Assert.Equal(2, result.Count);
    }
    #endregion

    #region Find_ReturnsAtMost10Words
    /// <summary>
    /// Verifies that Find never returns more than 10 words, even if more are found in the matrix.
    /// Also tests correct frequency ordering when more than 10 words are found.
    /// </summary>
    [Fact]
    public void Find_ReturnsAtMost10Words()
    {
        // Arrange: Prepare a matrix and a word stream with many potential matches.
        var matrix = new List<string>
    {
        "word1a", "word2b", "word3c", "word4d", "word5e",
        "word6f", "word7g", "word8h", "word9i", "word10j",
        "word11k", "word12l", "word13m"
    };

        // Create 13 words, with varying frequencies to ensure correct top 10 selection
        var wordStream = new List<string>();
        for (int i = 1; i <= 13; i++)
        {
            string word = $"word{i}"; // base word
            wordStream.Add(word); // Add once

            // Add more repetitions for higher frequency
            if (i <= 5) wordStream.Add(word); // 2 times
            if (i <= 3) wordStream.Add(word); // 3 times
            if (i <= 1) wordStream.Add(word); // 4 times (word1 will be most frequent)
        }
        // Adjust matrix to ensure these words are present
        var actualMatrix = new List<string>();
        for (int i = 0; i < 13; i++)
        {
            actualMatrix.Add(new string('x', 64)); // Fill with dummy chars
        }
        actualMatrix[0] = "word1" + new string('a', 64 - 5);
        actualMatrix[1] = "word2" + new string('a', 64 - 5);
        actualMatrix[2] = "word3" + new string('a', 64 - 5);
        actualMatrix[3] = "word4" + new string('a', 64 - 5);
        actualMatrix[4] = "word5" + new string('a', 64 - 5);
        actualMatrix[5] = "word6" + new string('a', 64 - 5);
        actualMatrix[6] = "word7" + new string('a', 64 - 5);
        actualMatrix[7] = "word8" + new string('a', 64 - 5);
        actualMatrix[8] = "word9" + new string('a', 64 - 5);
        actualMatrix[9] = "word10" + new string('a', 64 - 6);
        actualMatrix[10] = "word11" + new string('a', 64 - 6);
        actualMatrix[11] = "word12" + new string('a', 64 - 6);
        actualMatrix[12] = "word13" + new string('a', 64 - 6);

        var wordFinder = new WordFinderLib.WordFinder(actualMatrix);

        // Act
        var result = wordFinder.Find(wordStream).ToList();

        // Assert: The result should contain exactly 10 words.
        Assert.Equal(10, result.Count);

        // Assert correct ordering for the top words
        var expected = new List<string>
    {
        "word1", "word2", "word3", "word4", "word5",
        "word10", "word11", "word12", "word13", "word6"
    };
        Assert.Equal(expected, result);
    }
    #endregion

    #region Find_IsCaseInsensitive
    /// <summary>
    /// Verifies that Find is case-insensitive when matching words in the matrix,
    /// and that output casing matches the case in the word stream's first occurrence.
    /// </summary>
    [Fact]
    public void Find_IsCaseInsensitive()
    {
        // Arrange
        var matrix = new List<string>
        {
            "AbCd",
            "EfGh",
            "IjKl",
            "MnOp"
        };
        // Mix casing in word stream to test output casing
        var wordStream = new List<string> { "abcd", "eFgH", "IJKL", "mnop" };
        var wordFinder = new WordFinderLib.WordFinder(matrix);

        // Act
        var result = wordFinder.Find(wordStream).ToList();

        // Assert: All words should be found regardless of case.
        // The output casing should match the original wordstream casing that had the highest frequency,
        // or the first encountered if frequencies are tied (your Trie implementation retains original casing).
        var expected = new List<string> { "abcd", "eFgH", "IJKL", "mnop" }; // Based on stream input
        Assert.Equal(expected, result); // This assumes the Trie returns the *exact* word from the stream
        Assert.Equal(4, result.Count);
    }

    /// <summary>
    /// Verifies case-insensitivity with mixed-case matrix and wordstream values.
    /// </summary>
    [Fact]
    public void Find_IsCaseInsensitive_MixedMatrixAndStream()
    {
        // Arrange
        var matrix = new List<string>
        {
            "cOLdy", // Matrix has mixed case
            "WINDy",
            "cHiLl"
        };
        var wordStream = new List<string>
        {
            "cold", "Wind", "chill" // Stream has mixed case
        };
        var wordFinder = new WordFinderLib.WordFinder(matrix);

        // Act
        var result = wordFinder.Find(wordStream).ToList();

        // Assert: Words should be found and returned with their casing from the word stream.
        var expected = new List<string> { "chill", "cold", "Wind" }; // Alphabetical order for ties
        Assert.Equal(expected, result);
    }
    #endregion

    #region Find_WorksWithLargeMatrix
    /// <summary>
    /// Verifies that WordFinder works correctly with a large 64x64 matrix and finds words horizontally and vertically.
    /// This test explicitly places words to ensure they are found by the Trie.
    /// </summary>
    [Fact]
    public void Find_WorksWithLargeMatrix()
    {
        // Arrange: Create a 64x64 matrix filled with 'a', but insert specific words horizontally and vertically.
        int size = 64;
        var matrix = new List<string>();
        string horizontalWord = "horizontl"; // Slightly shorter to fit in 64
        string verticalWord = "vertical";

        // Initialize matrix with 'a'
        for (int i = 0; i < size; i++)
        {
            matrix.Add(new string('a', size));
        }

        // Place "horizontl" at row 10, starting at column 5
        // Ensure it doesn't overlap with vertical word placement
        if (5 + horizontalWord.Length <= size)
        {
            char[] row10Chars = matrix[10].ToCharArray();
            for (int j = 0; j < horizontalWord.Length; j++)
                row10Chars[5 + j] = horizontalWord[j];
            matrix[10] = new string(row10Chars);
        }


        // Place "vertical" at column 20, starting at row 15
        if (15 + verticalWord.Length <= size)
        {
            for (int k = 0; k < verticalWord.Length; k++)
            {
                var rowChars = matrix[15 + k].ToCharArray();
                rowChars[20] = verticalWord[k]; // Insert char into correct column
                matrix[15 + k] = new string(rowChars);
            }
        }

        var wordStream = new List<string>
        {
            "horizontl", "vertical", "notfound", "horizontal", "vertical", "horizontl" // Test frequencies
        };
        var wordFinder = new WordFinderLib.WordFinder(matrix);

        // Act
        var result = wordFinder.Find(wordStream).ToList();

        // Assert: Only "horizontl" and "vertical" should be found, ordered by frequency.
        // "horizontl" appears twice, "vertical" twice. Order should be alphabetical by original word if frequencies tied.
        var expected = new List<string> { "horizontl", "vertical" };
        Assert.Equal(expected, result);
        Assert.Equal(2, result.Count);
    }
    #endregion

    #region Find_ReturnsEmpty_WhenWordStreamIsNull
    /// <summary>
    /// Verifies that Find returns an empty enumerable when the word stream is null.
    /// </summary>
    [Fact]
    public void Find_ReturnsEmpty_WhenWordStreamIsNull()
    {
        // Arrange
        var matrix = new List<string>
        {
            "abcd",
            "efgh"
        };
        var wordFinder = new WordFinderLib.WordFinder(matrix);

        // Act
        var result = wordFinder.Find(null).ToList();

        // Assert
        Assert.Empty(result);
    }
    #endregion

    #region Find_ReturnsEmpty_WhenWordStreamIsEmpty
    /// <summary>
    /// Verifies that Find returns an empty enumerable when the word stream is empty.
    /// </summary>
    [Fact]
    public void Find_ReturnsEmpty_WhenWordStreamIsEmpty()
    {
        // Arrange
        var matrix = new List<string> { "abc" };
        var wordStream = new List<string>();
        var wordFinder = new WordFinderLib.WordFinder(matrix);

        // Act
        var result = wordFinder.Find(wordStream).ToList();

        // Assert
        Assert.Empty(result);
    }
    #endregion

    #region Find_HandlesWhitespaceAndEmptyWordsInStream
    /// <summary>
    /// Verifies that Find correctly handles null, empty, or whitespace-only words in the word stream.
    /// They should be ignored and not cause errors or be counted.
    /// </summary>
    [Fact]
    public void Find_HandlesWhitespaceAndEmptyWordsInStream()
    {
        // Arrange
        var matrix = new List<string>
        {
            "word",
            "test"
        };
        var wordStream = new List<string>
        {
            "word", // Found
            null,   // Ignored
            "",     // Ignored
            "   ",  // Ignored (whitespace only)
            "\t\n", // Ignored (whitespace only)
            "test"  // Found
        };
        var wordFinder = new WordFinderLib.WordFinder(matrix);

        // Act
        var result = wordFinder.Find(wordStream).ToList();

        // Assert
        var expected = new List<string> { "test", "word" };
        Assert.Equal(expected, result);
        Assert.Equal(2, result.Count);
    }
    #endregion

    #region Find_WordsAtMatrixEdges
    /// <summary>
    /// Verifies that words are correctly found when they are located at the edges (first row/col, last row/col) of the matrix.
    /// </summary>
    [Fact]
    public void Find_WordsAtMatrixEdges()
    {
        // Arrange a 5x5 matrix to fit all edge words
        var matrix = new List<string>
    {
        ".....",
        "MID..", // "MID" (row 1)
        "LEFTH", // "LEFT" (row 2)
        "TOPA.", // "TOPA" (row 3)
        "....."
    };

        // Place vertical words:
        // "TML" in column 0 (rows 0,1,2)
        // "IDE" in column 1 (rows 0,1,2)
        var tempMatrix = matrix.Select(s => s.ToCharArray()).ToList();
        tempMatrix[0][0] = 'T'; // T (row 0, col 0)
        tempMatrix[1][0] = 'M'; // M (row 1, col 0)
        tempMatrix[2][0] = 'L'; // L (row 2, col 0)

        tempMatrix[0][1] = 'I'; // I (row 0, col 1)
        tempMatrix[1][1] = 'D'; // D (row 1, col 1)
        tempMatrix[2][1] = 'E'; // E (row 2, col 1)

        matrix = tempMatrix.Select(c => new string(c)).ToList();

        var wordStream = new List<string>
    {
        "TOP", "LEFT", "TML", "IDE", "MID", "TOPA"
    };
        var wordFinder = new WordFinderLib.WordFinder(matrix);

        // Act
        var result = wordFinder.Find(wordStream).ToList();

        // Assert
        var expected = new List<string> { "IDE", "LEFT", "TML", "TOP", "TOPA" };
        Assert.Equal(expected, result);
        Assert.Equal(5, result.Count);
    }
    #endregion

    #region Find_OverlappingWords
    /// <summary>
    /// Verifies that overlapping words are correctly found.
    /// </summary>
    [Fact]
    public void Find_OverlappingWords()
    {
        // Arrange
        var matrix = new List<string>
        {
            "ABABA"
        };
        var wordStream = new List<string>
        {
            "ABA", "BAB"
        };
        var wordFinder = new WordFinderLib.WordFinder(matrix);

        // Act
        var result = wordFinder.Find(wordStream).ToList();

        // Assert
        // "ABA" appears twice, "BAB" appears twice.
        // Your Trie-based `SearchTextWithTrie` will correctly find both occurrences.
        // The `foundWordsInMatrix` HashSet ensures each *unique* word is only added once.
        // So, the expected result depends on the order of addition and frequency.
        // Assuming "ABA" then "BAB" from the stream, with equal frequencies, then alphabetical order.
        var expected = new List<string> { "ABA", "BAB" };
        Assert.Equal(expected, result);
        Assert.Equal(2, result.Count);
    }
    #endregion

    #region Find_LongWords
    /// <summary>
    /// Verifies that long words (up to matrix dimension) are correctly found.
    /// </summary>
    [Fact]
    public void Find_LongWords()
    {
        // Arrange
        int size = 10;
        string longWordHorizontal = "abcdefghij"; // Length 10
        string longWordVertical = "blmnopqrst";   // Length 10

        var matrix = new List<string>();
        for (int i = 0; i < size; i++)
        {
            matrix.Add(new string('.', size)); // Fill with dots
        }

        // Place vertical word in column 1 (to avoid overlap with horizontal word in row 0)
        for (int i = 0; i < size; i++)
        {
            var chars = matrix[i].ToCharArray();
            chars[1] = longWordVertical[i];
            matrix[i] = new string(chars);
        }

        // Place horizontal word in row 0
        matrix[0] = longWordHorizontal;

        var wordStream = new List<string>
    {
        longWordHorizontal, longWordVertical, "short"
    };
        var wordFinder = new WordFinderLib.WordFinder(matrix);

        // Act
        var result = wordFinder.Find(wordStream).ToList();

        // Assert
        var expected = new List<string> { longWordHorizontal, longWordVertical }; // Alphabetical order as frequencies are equal
        Assert.Equal(expected, result);
        Assert.Equal(2, result.Count);
    }
    #endregion

    #region Find_WordsWithSpecialCharacters
    /// <summary>
    /// Verifies handling of words containing special characters (if supported by the matrix content).
    /// </summary>
    [Fact]
    public void Find_WordsWithSpecialCharacters()
    {
        // Arrange
        var matrix = new List<string>
        {
            "@#$%!",
            "&*( )"
        };
        var wordStream = new List<string>
        {
            "@#$%", "&*( )", "no!"
        };
        var wordFinder = new WordFinderLib.WordFinder(matrix);

        // Act
        var result = wordFinder.Find(wordStream).ToList();

        // Assert
        var expected = new List<string> { "&*( )", "@#$%" };
        Assert.Equal(expected, result);
    }
    #endregion

    #region Find_NoMatchingWordsInWordStream
    /// <summary>
    /// Verifies that if no words in the word stream are found in the matrix, an empty collection is returned.
    /// Differs from `Find_ReturnsEmpty_WhenNoWordsFound` by ensuring *some* words are in stream, just not found.
    /// </summary>
    [Fact]
    public void Find_NoMatchingWordsInWordStream()
    {
        // Arrange
        var matrix = new List<string>
        {
            "abc",
            "def"
        };
        var wordStream = new List<string>
        {
            "ghi", "jkl"
        };
        var wordFinder = new WordFinderLib.WordFinder(matrix);

        // Act
        var result = wordFinder.Find(wordStream).ToList();

        // Assert
        Assert.Empty(result);
    }
    #endregion
}
