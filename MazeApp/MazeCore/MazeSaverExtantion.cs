namespace MazeCore;

/// <summary>
/// Provides extension methods for saving a Maze to a text file.
/// </summary>
public static class MazeSaverExtantion {
  /// <summary>
  /// Saves the maze to a text file.
  /// </summary>
  /// <param name="maze">The maze to save.</param>
  /// <param name="outputFile">The StreamWriter to write the maze data.</param>
  /// <returns>True if the maze was successfully saved; otherwise, false.</returns>
  public static bool SaveToTxt(this Maze maze, StreamWriter outputFile) {
    try {
      outputFile.WriteLine($"{maze.RowsCount} {maze.ColsCount}");
      WriteBorders(maze.VerticalBorders, outputFile);
      outputFile.WriteLine("");
      WriteBorders(maze.HorizontalBorders, outputFile);
      return true;
    } catch {
      return false;
    }

    // Local function to write borders to the output file
    void WriteBorders(int[,] borders, StreamWriter outputFile) {
      for (int i = 0; i < maze.RowsCount; i++) {
        for (int j = 0; j < maze.ColsCount; j++) {
          outputFile.Write(borders[i, j]);
          if (j < maze.ColsCount - 1)
            outputFile.Write(" ");
          else
            outputFile.Write('\n');
        }
      }
    }
  }
}
