# 📄 WordFinder Challenge Solution Documentation

## 1. Problem Overview 🧠

The challenge requires constructing a high-performance class that searches for words from a stream in a character matrix. Valid words appear **horizontally (left-to-right)** or **vertically (top-to-bottom)**. The matrix has a max size of 64x64, and the search should identify the **top 10 most repeated words from the stream** that exist in the matrix. Each found word is counted **once**, regardless of how many times it appears in the matrix.

---

## 2. Solution Summary 🔧

This implementation defines a class `WordFinder`, supported by an efficient **Trie data structure** (`TrieNode`) to enable multi-word pattern matching against matrix rows and columns.

**Key performance strategies:**

- A **Trie** to optimize search for multiple words.
- **Precomputed vertical columns** to avoid reconstructing during each lookup.
- **Case-insensitive search** using `ToLowerInvariant`.
- **Frequency-based ranking** of wordstream entries using a dictionary.

---

## 3. Class & Method Overview 📦

### 3.1. TrieNode

- Represents a node within the **Trie** (prefix tree).
- Tracks child nodes (`Children`) and whether a word ends at the node (`IsEndOfWord`).
- Stores the complete word when it terminates (`Word`).
- `AddWord(string word)`: Adds a word character-by-character into the Trie.

### 3.2. WordFinder

- Initializes with a validated character matrix (`matrix`).
- Splits matrix into `_matrixRows` and `_matrixCols` for horizontal and vertical scanning.
- Searches for valid words using `Find()` and returns the **top 10** most frequent words found.

#### 3.2.1. Methods

- `ValidateMatrix(IEnumerable<string>)`: Ensures matrix is not null or empty, verifies uniform row lengths and character count constraints (≤ 64 rows/cols), aggregates multiple errors into one `ArgumentException`.
- `SearchTextWithTrie(string text, TrieNode trieRoot, HashSet<string> results)`: Iterates over each substring within the row or column, traverses the Trie to detect word matches, adds detected words to result set.
- `Find(IEnumerable<string> wordstream)`: Builds frequency map of wordstream, loads all valid words into the Trie, scans both rows and columns for matches, returns top 10 found words ranked by stream frequency and alphabetically.

---

## 4. Performance Notes 📈

| Optimization         | Justification                                                           |
| -------------------- | ----------------------------------------------------------------------- |
| Trie Search          | Avoids O(n*m) per word lookup; supports simultaneous multi-word checks   |
| Matrix Preprocessing | Columns computed once to save redundant traversal during vertical scan    |
| HashSet for Results  | Prevents duplicate entries and ensures fast lookup                      |
| Frequency Dictionary | Ensures correct ranking of found words without reprocessing             |

All operations fall within **linear or sublinear complexity**, considering the bounded matrix and wordstream constraints.

---

## 5. Usage & Testing 🧪

### 5.1. Constructor Example

```csharp
var matrix = new[] {
  "chillwindstorm",
  "abcdefgghijklmn",
  ...
};
var finder = new WordFinder(matrix);
```

### 5.2. Edge Cases Handled

#### 5.2.1. Input Validation 🚫

- **Null Matrix**: Throws an error when the matrix is `null`.
- **Empty Matrix**: Throws an error if the matrix contains no rows.
- **Row Length Mismatch**: Ensures all rows have the same number of characters.
- **Null or Empty Rows**: Detects and rejects matrices with `null` or empty strings.
- **Matrix Size Limit**: Enforces a maximum of 64 rows and 64 columns.

#### 5.2.2. Wordstream Filtering 🔍

- **Null Wordstream**: Returns an empty result if the wordstream is `null`.
- **Whitespace Words**: Ignores entries that are blank or only whitespace.
- **Empty Strings**: Skips over entries with zero length after trimming.

#### 5.2.3. Casing and Duplicates 🔡

- **Case Insensitive Search**: Matches words regardless of their casing (`"Wind"` matches `"wind"`).
- **Stream Duplicates**: Words repeated in the stream are counted once per word.
- **Matrix Duplicates**: Word occurrences in the matrix are not counted more than once.

#### 5.2.4. Trie Optimization 🧠

- **Empty Trie**: Prevents search if no valid words were added.
- **Partial Matches**: Only complete word matches are considered; partial substrings are ignored.

#### 5.2.5. Fallback Behavior 🛑

- **No Words Found**: Returns an empty list if none of the words are detected.
- **Less Than 10 Matches**: Returns fewer than 10 results when not enough words are found in the matrix.
