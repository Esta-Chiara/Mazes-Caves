using CommonCore;
namespace CaveCore;

/// <summary>
/// Provides extension methods for the <see cref="Cell"/> class related to cave operations.
/// </summary>
public static class CellCaveEstantions {
  public static bool IsAlive(this Cell cell, Cave cave) {
    return cell.Col < 0 || cave.ColumnsCount <= cell.Col || cell.Row < 0 ||
           cave.RowsCount <= cell.Row || cave[cell.Row, cell.Col] == 1;
  }

  /// <summary>
  /// Determines whether the cell is alive within the specified cave.
  /// </summary>
  /// <param name="cell">The cell to check.</param>
  /// <param name="cave">The cave in which to check the cell's status.</param>
  /// <returns><c>true</c> if the cell is alive; otherwise, <c>false</c>.</returns>
  public static int GetCountOfLivingNigtbours(this Cell cell, Cave cave) {
    int livingsCount = 0;

    foreach (var c in cell.GetAllNeighbours()) {
      if (c.IsAlive(cave)) {
        livingsCount++;
      }
    }

    return livingsCount;
  }
}
