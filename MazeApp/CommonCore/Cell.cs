namespace CommonCore;

/// <summary>
/// Represents a cell with row and column coordinates.
/// </summary>
public struct Cell
(int row, int col) {
  /// <summary>
  /// Gets the row coordinate of the cell.
  /// </summary>
  public int Row => row;

  /// <summary>
  /// Gets the column coordinate of the cell.
  /// </summary>
  public int Col => col;

  /// <summary>
  /// Gets the cell adjacent to the current cell in the specified direction.
  /// </summary>
  /// <param name="direction">The direction in which to find the adjacent cell.</param>
  /// <returns>The adjacent cell.</returns>
  public Cell GetNextByDirection(Directions direction) {
    int row = Row;
    int col = Col;

    switch (direction) {
      case Directions.Left:
        col--;
        break;
      case Directions.Up:
        row--;
        break;
      case Directions.Right:
        col++;
        break;
      case Directions.Down:
        row++;
        break;
      default:
        break;
    }

    return new Cell(row, col);
  }

  /// <summary>
  /// Gets an array of all neighboring cells around the current cell.
  /// </summary>
  /// <returns>An array of neighboring cells.</returns>
  public Cell[] GetAllNeighbours() {
    return [
      new Cell(Row, Col - 1),
      new Cell(Row - 1, Col - 1),
      new Cell(Row - 1, Col),
      new Cell(Row - 1, Col + 1),
      new Cell(Row, Col + 1),
      new Cell(Row + 1, Col + 1),
      new Cell(Row + 1, Col),
      new Cell(Row + 1, Col - 1),
    ];
  }

  /// <summary>
  /// Returns a string that represents the current cell.
  /// </summary>
  /// <returns>A string representation of the cell in the format (Row, Col).</returns>
  public override string ToString() {
    return $"({Row}, {Col})";
  }
}
