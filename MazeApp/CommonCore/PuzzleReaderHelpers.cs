namespace CommonCore;

/// <summary>
/// Provides helper methods for reading puzzle data from a stream.
/// </summary>
public static class PuzzleReaderHelpers {
  /// <summary>
  /// Reads the size of the puzzle from the specified stream reader.
  /// </summary>
  /// <param name="source">The stream reader from which to read the size.</param>
  /// <returns>The size of the puzzle as a <see cref="Size"/> object.</returns>
  public static Size ReadSize(StreamReader source) {
    var parts = source.ReadLine()?.Split().Select(int.Parse).ToArray();
    return new Size(parts[0], parts[1]);
  }

  /// <summary>
  /// Reads the matrix of the puzzle from the specified stream reader using the given size.
  /// </summary>
  /// <param name="source">The stream reader from which to read the matrix.</param>
  /// <param name="size">The size of the matrix to read.</param>
  /// <returns>The matrix of the puzzle as a 2D array of integers.</returns>
  public static int[,] ReadMatrix(StreamReader source, Size size) {
    var counter = 0;
    var matrix = new int[size.RowsCount, size.ColumnsCount];

    while (counter < size.RowsCount) {
      var row = source.ReadLine()?.Split().Select(int.Parse).ToArray();
      Enumerable.Range(0, size.ColumnsCount).ToList().ForEach(j => matrix[counter, j] = row[j]);
      counter++;
    }

    return matrix;
  }
}
