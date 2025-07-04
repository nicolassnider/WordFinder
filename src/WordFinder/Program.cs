/// <summary>
/// Entry point for the WordFinder demo.
/// 
/// Example usage:
/// <code>
/// dotnet run "abcd,efgh,ijkl,mnop" "abcd,ijkl,mnop"
/// </code>
/// The first argument is the matrix (comma-separated rows).
/// The second argument is the word stream (comma-separated words).
/// If arguments are omitted, defaults are used.
/// </summary>
static void Main(string[] args)
{

    // Accept matrix from command-line argument, or use default if not provided
    List<string> matrix;
    if (args.Length > 0 && !string.IsNullOrWhiteSpace(args[0]))
    {
        matrix = args[0].Split(',').ToList();
    }
    else
    {
        matrix = new List<string>
            {
                "abcd",
                "efgh",
                "ijkl",
                "mnop"
            };
    }

    // Accept word stream from command-line argument, or use default if not provided
    List<string> wordStream;
    if (args.Length > 1 && !string.IsNullOrWhiteSpace(args[1]))
    {
        wordStream = args[1].Split(',').ToList();
    }
    else
    {
        wordStream = new List<string>
            {
                "abcd", "efgh", "ijkl", "mnop", "bcde", "fghi", "jklm", "nopq", "cdef", "ghij", "klmn", "opqr"
            };
    }

    var wordFinder = new WordFinder.WordFinderLib.WordFinder(matrix);
    var foundWords = wordFinder.Find(wordStream);

    Console.WriteLine("Words found in matrix:");
    foreach (var word in foundWords)
    {
        Console.WriteLine(word);
    }
    Console.ReadKey();
}
