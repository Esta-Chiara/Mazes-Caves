using System.Text;
using CommonCore;
namespace MazeCore;

/// <summary>
/// Reads a maze from a text file.
/// </summary>
public class MazeTxtReader {
  private string _filePath;

  /// <summary>
  /// Initializes a new instance of the <see cref="MazeTxtReader"/> class with the specified file
  /// path.
  /// </summary>
  /// <param name="filePath">The path to the maze text file.</param>
  public MazeTxtReader(string filePath) {
    _filePath = filePath;
  }

  /// <summary>
  /// Reads the maze from the specified text file.
  /// </summary>
  /// <returns>The maze read from the file.</returns>
  public Maze Read() {
    using FileStream stream = new(_filePath, FileMode.Open, FileAccess.Read);
    using StreamReader reader = new(stream, Encoding.UTF8);

    Size size = PuzzleReaderHelpers.ReadSize(reader);
    int[,] verticalBordersMatrix = PuzzleReaderHelpers.ReadMatrix(reader, size);
    reader.ReadLine();
    int[,] horizontalBordersMatrix = PuzzleReaderHelpers.ReadMatrix(reader, size);

    return new Maze(verticalBordersMatrix, horizontalBordersMatrix);
  }
}
