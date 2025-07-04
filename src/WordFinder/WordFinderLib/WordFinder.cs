namespace WordFinder.WordFinderLib;

/// <summary>
/// Represents a node in the Trie data structure.
/// Each node stores a character and a dictionary of its children nodes.
/// It also indicates if a word ends at this node.
/// </summary>
internal class TrieNode
{
    // Children nodes, mapping character to TrieNode
    public Dictionary<char, TrieNode> Children { get; } = new Dictionary<char, TrieNode>();
    // Indicates if a word ends at this node
    public bool IsEndOfWord { get; set; }
    // Stores the actual word if it ends here (useful for retrieving found words)
    public string Word { get; set; } = null!;

    /// <summary>
    /// Adds a word to the Trie.
    /// </summary>
    /// <param name="word">The word to add.</param>
    public void AddWord(string word)
    {
        TrieNode current = this; // Start from the root
        foreach (char c in word.ToLowerInvariant()) // Convert to lower for case-insensitive search
        {
            if (!current.Children.TryGetValue(c, out TrieNode? node))
            {
                node = new TrieNode();
                current.Children.Add(c, node);
            }
            current = node;
        }
        current.IsEndOfWord = true;
        current.Word = word; // Store the original word (case-preserved)
    }
}

/// <summary>
/// Provides functionality to search for words in a character matrix.
/// Words can be found horizontally (left-to-right) or vertically (top-to-bottom).
/// Uses a Trie for efficient multi-word search.
/// </summary>
public class WordFinder
{
    private readonly string[] _matrixRows;
    private readonly string[] _matrixCols;
    private readonly int _rowCount;
    private readonly int _colCount;

    /// <summary>
    /// Initializes a new instance of the <see cref="WordFinder"/> class with the specified character matrix.
    /// Collects all validation errors and throws an ArgumentException with all error messages if any are found.
    /// </summary>
    /// <param name="matrix">
    /// The character matrix, represented as an enumerable of strings.
    /// Each string represents a row; all rows must have the same length.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown if the matrix is null, empty, contains rows of differing lengths, or exceeds 64x64 size.
    /// All validation errors are reported together.
    /// </exception>
    public WordFinder(IEnumerable<string> matrix)
    {
        var errors = ValidateMatrix(matrix);

        if (errors.Count > 0)
        {
            throw new ArgumentException(string.Join(" ", errors));
        }

        _matrixRows = matrix?.ToArray() ?? Array.Empty<string>();
        _rowCount = _matrixRows.Length;
        _colCount = _rowCount > 0 ? _matrixRows[0].Length : 0;

        // Precompute columns for efficient vertical search
        _matrixCols = new string[_colCount];
        for (int col = 0; col < _colCount; col++)
        {
            char[] colChars = new char[_rowCount];
            for (int row = 0; row < _rowCount; row++)
            {
                colChars[row] = _matrixRows[row][col];
            }
            _matrixCols[col] = new string(colChars);
        }
    }

    /// <summary>
    /// Validates the input matrix and returns a set of error messages for any validation failures.
    /// <para>
    /// <b>Validation rules:</b>
    /// <list type="bullet">
    /// <item><description>Matrix cannot be null.</description></item>
    /// <item><description>Matrix cannot be empty.</description></item>
    /// <item><description>Matrix cannot have more than 64 rows.</description></item>
    /// <item><description>Matrix cannot contain null rows.</description></item>
    /// <item><description>Matrix cannot contain empty rows.</description></item>
    /// <item><description>Matrix cannot have more than 64 columns.</description></item>
    /// <item><description>All rows in the matrix must have the same length.</description></item>
    /// </list>
    /// Returns a set of all validation error messages found. If the set is empty, the matrix is valid.
    /// </para>
    /// </summary>
    /// <param name="matrix">The character matrix to validate.</param>
    /// <returns>A set of error messages, or an empty set if the matrix is valid.</returns>
    private static HashSet<string> ValidateMatrix(IEnumerable<string> matrix)
    {
        var errors = new HashSet<string>();

        if (matrix == null)
        {
            errors.Add("Matrix cannot be null.");
            return errors;
        }

        var rows = matrix.ToArray();
        int rowCount = rows.Length;

        if (rowCount == 0)
        {
            errors.Add("Matrix cannot be empty.");
            return errors;
        }

        if (rowCount > 64)
        {
            errors.Add("Matrix cannot have more than 64 rows.");
        }

        // Check for null or empty rows within the matrix
        if (rows.Any(row => row == null))
        {
            errors.Add("Matrix cannot contain null rows.");
        }
        if (rows.Any(row => string.IsNullOrEmpty(row)))
        {
            // Note: If a row is null, string.IsNullOrEmpty will also return true.
            // The previous check handles null explicitly, so this focuses on actual empty strings.
            errors.Add("Matrix cannot contain empty rows.");
        }

        // Filter out null or empty rows before calculating column count and checking consistency
        var nonNullEmptyRows = rows.Where(row => !string.IsNullOrEmpty(row)).ToArray();

        if (nonNullEmptyRows.Length > 0)
        {
            int colCount = nonNullEmptyRows[0].Length;

            if (colCount > 64)
            {
                errors.Add("Matrix cannot have more than 64 columns.");
            }

            if (nonNullEmptyRows.Any(row => row.Length != colCount))
            {
                errors.Add("All rows in the matrix must have the same length.");
            }
        }
        else if (errors.Count == 0 && rowCount > 0) // Case: Matrix has rows, but all are null or empty.
        {
            // If we reached here, and no errors yet, but all rows are invalid,
            // then effectively the matrix has no valid columns, which should be an error.
            // This case might be implicitly handled by `Matrix cannot contain empty rows` or `null rows`
            // but for safety, ensure that if all rows are invalid, we count it as invalid.
            // For example, if input is {"", "", ""}, it should be invalid.
            // The existing `Matrix cannot contain empty rows` handles this correctly.
        }

        return errors;
    }

    /// <summary>
    /// Finds the top 10 most repeated words from the word stream that are present in the matrix.
    /// Each word is counted based on its frequency in the word stream, but a word found in the matrix
    /// is only considered "found" once for the purpose of being in the result set.
    /// </summary>
    /// <param name="wordstream">A stream of words to search for in the matrix.</param>
    /// <returns>
    /// An enumerable of up to 10 words found in the matrix, ordered by their frequency in the word stream (descending).
    /// If no words are found, returns an empty enumerable.
    /// </returns>
    public IEnumerable<string> Find(IEnumerable<string> wordstream)
    {
        if (wordstream == null)
            return Enumerable.Empty<string>();

        // Step 1: Process wordstream to get unique words, their frequencies, and build the Trie.
        // Using a custom StringComparer.OrdinalIgnoreCase for consistent case-insensitive behavior.
        var wordFrequency = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        var searchTrie = new TrieNode();

        foreach (var word in wordstream)
        {
            if (string.IsNullOrWhiteSpace(word))
                continue;

            // Trim and normalize for consistency and to avoid adding empty strings to Trie
            var normalizedWord = word.Trim();
            if (normalizedWord.Length == 0) continue;

            // Update frequency count (case-insensitive)
            wordFrequency.TryGetValue(normalizedWord, out int currentCount);
            wordFrequency[normalizedWord] = currentCount + 1;

            // Add the word to the Trie (stores original casing, but search will be case-insensitive)
            searchTrie.AddWord(normalizedWord);
        }

        // Handle case where no valid words in wordstream after filtering
        if (wordFrequency.Count == 0)
        {
            return Enumerable.Empty<string>();
        }

        // Step 2: Search the matrix using the Trie
        // A HashSet to store words found in the matrix for efficient de-duplication
        var foundWordsInMatrix = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        // Search horizontally
        foreach (var rowText in _matrixRows)
        {
            SearchTextWithTrie(rowText, searchTrie, foundWordsInMatrix);
        }

        // Search vertically
        foreach (var colText in _matrixCols)
        {
            SearchTextWithTrie(colText, searchTrie, foundWordsInMatrix);
        }

        // Step 3: Filter, order, and take top 10
        // Filter found words to include only those present in the original wordstream
        // and order by their frequency from the wordstream, then by alphabetical for ties.
        return foundWordsInMatrix
            .Where(wordFrequency.ContainsKey) // Ensure the word was in the original stream
            .OrderByDescending(w => wordFrequency[w]) // Order by frequency from wordstream
            .ThenBy(w => w, StringComparer.OrdinalIgnoreCase) // Secondary sort for deterministic tie-breaking
            .Take(10)
            .ToList();
    }

    /// <summary>
    /// Searches a given text string for words present in the Trie.
    /// When a word is found, it's added to the collection of found words.
    /// </summary>
    /// <param name="text">The text (row or column) to search within.</param>
    /// <param name="trieRoot">The root of the Trie containing words to search for.</param>
    /// <param name="results">A HashSet to add the found words to.</param>
    private void SearchTextWithTrie(string text, TrieNode trieRoot, HashSet<string> results)
    {
        // Iterate through the text, starting a new Trie traversal from each character
        for (int i = 0; i < text.Length; i++)
        {
            TrieNode? current = trieRoot;
            for (int j = i; j < text.Length; j++)
            {
                char c = text[j];
                // Convert character to lower for case-insensitive matching with Trie nodes
                if (!current.Children.TryGetValue(char.ToLowerInvariant(c), out current))
                {
                    // No path in the Trie for this character, break from inner loop
                    break;
                }

                // If a word ends at the current Trie node, add it to results
                if (current.IsEndOfWord)
                {
                    results.Add(current.Word);
                }
            }
        }
    }
}