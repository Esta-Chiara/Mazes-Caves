namespace CommonCore;

/// <summary>
/// Represents the size (number of rows and columns) of a structure.
/// </summary>
public struct Size {
  /// <summary>
  /// The minimum number of rows allowed.
  /// </summary>
  public const int MinRowsCount = 2;

  /// <summary>
  /// The maximum number of rows allowed.
  /// </summary>
  public const int MaxRowsCount = 50;

  /// <summary>
  /// The minimum number of columns allowed.
  /// </summary>
  public const int MinColumnsCount = 2;

  /// <summary>
  /// The maximum number of columns allowed.
  /// </summary>
  public const int MaxColumnsCount = 50;

  /// <summary>
  /// Gets the number of rows in the size.
  /// </summary>
  public int RowsCount { get; private set; }

  /// <summary>
  /// Gets the number of columns in the size.
  /// </summary>
  public int ColumnsCount { get; private set; }

  /// <summary>
  /// Initializes a new instance of the <see cref="Size"/> struct with the specified number of rows
  /// and columns.
  /// </summary>
  /// <param name="rowsCount">The number of rows.</param>
  /// <param name="columnsCount">The number of columns.</param>
  /// <exception cref="ArgumentOutOfRangeException">Thrown when rowsCount or columnsCount are
  /// outside the allowed range.</exception>
  public Size(int rowsCount, int columnsCount) {
    if (rowsCount < MinRowsCount || MaxRowsCount < rowsCount) {
      throw new ArgumentOutOfRangeException(
          $"Rows count must be in range [{MinRowsCount}, {MaxRowsCount}].");
    }

    if (columnsCount < MinColumnsCount || MaxColumnsCount < columnsCount) {
      throw new ArgumentOutOfRangeException(
          $"Columns count must be in range [{MinColumnsCount}, {MaxColumnsCount}].");
    }

    RowsCount = rowsCount;
    ColumnsCount = columnsCount;
  }
}
