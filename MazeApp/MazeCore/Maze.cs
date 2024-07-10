using CommonCore;
namespace MazeCore;

/// <summary>
/// Represents a maze with vertical and horizontal borders.
/// </summary>
public class Maze {
  private const int _minSize = 2;
  private const int _maxSize = 50;

  /// <summary>
  /// Gets the number of rows in the maze.
  /// </summary>
  public int RowsCount { get; private set; }

  /// <summary>
  /// Gets the number of columns in the maze.
  /// </summary>
  public int ColsCount { get; private set; }

  /// <summary>
  /// Gets the matrix of vertical borders in the maze.
  /// </summary>
  public int[,] VerticalBorders { get; private set; }

  /// <summary>
  /// Gets the matrix of horizontal borders in the maze.
  /// </summary>
  public int[,] HorizontalBorders { get; private set; }

  /// <summary>
  /// Initializes a new instance of the <see cref="Maze"/> class with specified vertical and
  /// horizontal borders.
  /// </summary>
  /// <param name="verticalBorders">The matrix of vertical borders.</param>
  /// <param name="horizontalBorders">The matrix of horizontal borders.</param>
  /// <exception cref="ArgumentNullException">Thrown when either <paramref name="verticalBorders"/>
  /// or <paramref name="horizontalBorders"/> is null.</exception> <exception
  /// cref="ArgumentOutOfRangeException">Thrown when the dimensions of <paramref
  /// name="verticalBorders"/> and <paramref name="horizontalBorders"/> do not match or when they
  /// are outside the valid size range.</exception>
  public Maze(int[,] verticalBorders, int[,] horisontalBorders) {
    ThrowIfWrongBordersMatrices(verticalBorders, horisontalBorders);

    RowsCount = horisontalBorders.GetLength(0);
    ColsCount = horisontalBorders.GetLength(1);
    VerticalBorders = (int[,])verticalBorders.Clone();
    HorizontalBorders = (int[,])horisontalBorders.Clone();
  }

  /// <summary>
  /// Determines whether the current object is equal to another object.
  /// </summary>
  /// <param name="obj">The object to compare with the current object.</param>
  /// <returns>true if the current object is equal to the <paramref name="obj"/> parameter;
  /// otherwise, false.</returns>
  public override bool Equals(object? obj) {
    var other = obj as Maze;
    if (other == null || RowsCount != other.RowsCount || ColsCount != other.ColsCount ||
        !HorizontalBorders.SequenceEqual(other.HorizontalBorders) ||
        !VerticalBorders.SequenceEqual(other.VerticalBorders))
      return false;

    return true;
  }

  /// <summary>
  /// Checks if the provided matrices of borders are valid.
  /// </summary>
  /// <param name="verticalBorders">The vertical borders matrix.</param>
  /// <param name="horisontalBorders">The horizontal borders matrix.</param>
  /// <exception cref="ArgumentNullException">Thrown when either <paramref name="verticalBorders"/>
  /// or <paramref name="horisontalBorders"/> is null.</exception> <exception
  /// cref="ArgumentOutOfRangeException">Thrown when the matrices do not have the same size or when
  /// their size is outside the allowed range.</exception>
  private static void ThrowIfWrongBordersMatrices(int[,] verticalBorders,
                                                  int[,] horisontalBorders) {
    if (verticalBorders is null)
      throw new ArgumentNullException("Vertical borders not exist.");
    if (horisontalBorders is null)
      throw new ArgumentNullException("Horisontal borders not exist.");
    if (horisontalBorders.GetLength(0) != verticalBorders.GetLength(0) ||
        horisontalBorders.GetLength(1) != verticalBorders.GetLength(1))
      throw new ArgumentOutOfRangeException(
          "Vertical borders and horisontal borders must have the same size.");
    if (horisontalBorders.GetLength(0) < _minSize || horisontalBorders.GetLength(1) < _minSize ||
        verticalBorders.GetLength(0) < _minSize || verticalBorders.GetLength(1) < _minSize ||
        horisontalBorders.GetLength(0) > _maxSize || horisontalBorders.GetLength(1) > _maxSize ||
        verticalBorders.GetLength(0) > _maxSize || verticalBorders.GetLength(1) > _maxSize)
      throw new ArgumentOutOfRangeException(
          $"Wrong  size of the Maze. It must be >= {_minSize} and <= {_maxSize}");
  }
}
