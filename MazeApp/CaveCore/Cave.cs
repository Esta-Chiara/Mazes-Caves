using System.Text;

using CommonCore;

namespace CaveCore;

/// <summary>
/// Represents a cave with a matrix structure.
/// </summary>
public class Cave {
  private Size _caveSize;

  /// <summary>
  /// Gets the number of rows in the cave.
  /// </summary>
  public int RowsCount => _caveSize.RowsCount;

  /// <summary>
  /// Gets the number of columns in the cave.
  /// </summary>
  public int ColumnsCount => _caveSize.ColumnsCount;

  private int[,] CaveMatrix { get; init; }

  /// <summary>
  /// Initializes a new instance of the <see cref="Cave"/> class with the specified matrix.
  /// </summary>
  /// <param name="caveMatrix">A two-dimensional integer array representing the cave matrix.</param>
  public Cave(int[,] caveMatrix) {
    _caveSize = new Size(caveMatrix.GetLength(0), caveMatrix.GetLength(1));
    CaveMatrix = (int[,])caveMatrix.Clone();
  }

  /// <summary>
  /// Gets or sets the value at the specified position in the cave matrix.
  /// </summary>
  /// <param name="i">The row index.</param>
  /// <param name="j">The column index.</param>
  /// <returns>The value at the specified position in the cave matrix.</returns>
  public int this[int i, int j] {
    get => CaveMatrix[i, j];
    set => CaveMatrix[i, j] = value;
  }

  /// <summary>
  /// Determines whether the specified object is equal to the current <see cref="Cave"/> object.
  /// </summary>
  /// <param name="obj">The object to compare with the current <see cref="Cave"/> object.</param>
  /// <returns>true if the specified object is equal to the current <see cref="Cave"/> object;
  /// otherwise, false.</returns>
  public override bool Equals(object? obj) {
    var other = obj as Cave;
    if (other == null || RowsCount != other.RowsCount || ColumnsCount != other.ColumnsCount ||
        !CaveMatrix.SequenceEqual(other.CaveMatrix))
      return false;

    return true;
  }

  /// <summary>
  /// Returns a string that represents the current <see cref="Cave"/> object.
  /// </summary>
  /// <returns>A string that represents the current <see cref="Cave"/> object.</returns>
  public override string ToString() {
    StringBuilder result = new($"{RowsCount} {ColumnsCount}\n");
    for (int i = 0; i < RowsCount; i++) {
      for (int j = 0; j < ColumnsCount - 1; j++) {
        result.Append($"{this[i, j]} ");
      }
      result.Append($"{this[i, ColumnsCount - 1]}\n");
    }

    return result.ToString();
  }
}
