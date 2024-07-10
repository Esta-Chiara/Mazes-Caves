using CommonCore;
namespace MazeCore;

/// <summary>
/// Generates a maze using the Eller's algorithm.
/// </summary>
public class EllersMazeGenerator {
  private Random _random;
  private Size _size;
  private int[,] _verticalBorders;
  private int[,] _horizontalBorders;
  private int _unusedGroupId = 0;

  /// <summary>
  /// Initializes a new instance of the <see cref="EllersMazeGenerator"/> class with a specified
  /// size.
  /// </summary>
  /// <param name="size">The size (number of rows and columns) of the maze to generate.</param>
  public EllersMazeGenerator(Size size) {
    _random = new Random();
    Initialize(size);
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="EllersMazeGenerator"/> class with a specified
  /// size and random seed.
  /// </summary>
  /// <param name="size">The size (number of rows and columns) of the maze to generate.</param>
  /// <param name="randomSeed">The seed value used to initialize the random number
  /// generator.</param>
  public EllersMazeGenerator(Size size, int randomSeed) {
    _random = new Random(randomSeed);
    Initialize(size);
  }

  /// <summary>
  /// Creates a maze using the Eller's algorithm.
  /// </summary>
  /// <returns>The generated maze as a <see cref="Maze"/> object.</returns>
  public Maze Create() {
    _unusedGroupId = 0;

    int[] row = Enumerable.Repeat(-1, _size.ColumnsCount).ToArray();

    for (int i = 0; i < _size.RowsCount - 1; i++) {
      InitNewRow(ref row);
      SetVerticalBorders(ref row, i);
      SetHorizontalBorders(ref row, i);
    }

    GenerateLastRowBorders(ref row, _size.RowsCount - 1);

    return new Maze(_verticalBorders, _horizontalBorders);
  }

  /// <summary>
  /// Initializes the maze size and border arrays.
  /// </summary>
  /// <param name="size">The size (number of rows and columns) of the maze.</param>
  private void Initialize(Size size) {
    _size = size;

    _verticalBorders = new int[_size.RowsCount, _size.ColumnsCount];
    _horizontalBorders = new int[_size.RowsCount, _size.ColumnsCount];
  }

  /// <summary>
  /// Generates borders for the last row in the maze.
  /// </summary>
  /// <param name="row">The array representing the row groups.</param>
  /// <param name="rowNumber">The index of the current row.</param>
  /// <returns>The modified array representing the row groups.</returns>
  private int[] GenerateLastRowBorders(ref int[] row, int rowNumber) {
    InitNewRow(ref row);
    SetVerticalBorders(ref row, rowNumber);
    ModifyLastRowBorders(ref row, rowNumber);
    return row;
  }

  /// <summary>
  /// Modifies the borders of the last row in the maze.
  /// </summary>
  /// <param name="row">The array representing the row groups.</param>
  /// <param name="rowNumber">The index of the current row.</param>
  /// <returns>The modified array representing the row groups.</returns>
  private int[] ModifyLastRowBorders(ref int[] row, int rowNumber) {
    for (int i = 0; i < row.Length - 1; i++) {
      if (row[i] != row[i + 1]) {
        _verticalBorders[rowNumber, i] = 0;
        UnionGroups(ref row, row[i], row[i + 1]);
      }

      _horizontalBorders[rowNumber, i] = 1;
    }

    _verticalBorders[rowNumber, row.Length - 1] = 1;
    _horizontalBorders[rowNumber, row.Length - 1] = 1;

    return row;
  }

  /// <summary>
  /// Sets the vertical borders for the specified row in the maze.
  /// </summary>
  /// <param name="row">The array representing the row groups.</param>
  /// <param name="rowNumber">The index of the current row.</param>
  /// <returns>The modified array representing the row groups.</returns>
  private int[] SetVerticalBorders(ref int[] row, int rowNumber) {
    for (int i = 0; i < row.Length - 1; i++) {
      int currentGoupId = row[i];
      int nextGroupId = row[i + 1];

      if (currentGoupId == nextGroupId || _random.Next(0, 2) == 1) {
        _verticalBorders[rowNumber, i] = 1;
      } else {
        UnionGroups(ref row, currentGoupId, nextGroupId);
      }
    }
    _verticalBorders[rowNumber, row.Length - 1] = 1;
    return row;
  }

  /// <summary>
  /// Unions two groups in the row array.
  /// </summary>
  /// <param name="row">The array representing the row groups.</param>
  /// <param name="newGoupId">The ID of the new group to union.</param>
  /// <param name="oldGroupId">The ID of the old group to union.</param>
  /// <returns>The modified array representing the row groups.</returns>
  private int[] UnionGroups(ref int[] row, int newGoupId, int oldGroupId) {
    for (int k = 0; k < row.Length; k++) {
      if (row[k] == oldGroupId) {
        row[k] = newGoupId;
      }
    }

    return row;
  }

  /// <summary>
  /// Sets the horizontal borders for the specified row in the maze.
  /// </summary>
  /// <param name="row">The array representing the row groups.</param>
  /// <param name="rowNumber">The index of the current row.</param>
  /// <returns>The modified array representing the row groups.</returns>
  private int[] SetHorizontalBorders(ref int[] row, int rowNumber) {
    for (int i = 0; i < row.Length; i++) {
      int groupId = row[i];

      if (_random.Next(0, 2) == 1 && row.Where(x => x == groupId).Count() > 1) {
        row[i] = -1;  // impossible groupID
        _horizontalBorders[rowNumber, i] = 1;
      }
    }
    return row;
  }

  /// <summary>
  /// Initializes a new row in the maze with unique group IDs.
  /// </summary>
  /// <param name="row">The array representing the row groups.</param>
  /// <returns>The initialized array representing the row groups.</returns>
  private int[] InitNewRow(ref int[] row) {
    for (int i = 0; i < row.Length; i++) {
      if (row[i] < 0) {
        row[i] = _unusedGroupId++;
      }
    }
    return row;
  }
}
