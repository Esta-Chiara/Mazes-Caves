using System.Text;
using CommonCore;

namespace CaveCore;

/// <summary>
/// Provides methods for reading cave data from a text file.
/// </summary>
public static class CaveTxtReader {
  /// <summary>
  /// Reads cave data from the specified text file.
  /// </summary>
  /// <param name="filePath">The path to the text file containing cave data.</param>
  /// <returns>A <see cref="Cave"/> object representing the cave read from the file.</returns>
  public static Cave Read(string filePath) {
    using FileStream stream = new(filePath, FileMode.Open, FileAccess.Read);
    using StreamReader reader = new(stream, Encoding.UTF8);
    Size size = PuzzleReaderHelpers.ReadSize(reader);
    int[,] matrix = PuzzleReaderHelpers.ReadMatrix(reader, size);
    return new Cave(matrix);
  }
}
