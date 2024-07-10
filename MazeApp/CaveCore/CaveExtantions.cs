namespace CaveCore;

/// <summary>
/// Provides extension methods for the <see cref="Cave"/> class.
/// </summary>
internal static class CaveExtantions {
  /// <summary>
  /// Determines whether the specified 2D integer array is equal to the specified <see cref="Cave"/>
  /// object.
  /// </summary>
  /// <param name="target1">The 2D integer array to compare.</param>
  /// <param name="target2">The <see cref="Cave"/> object to compare.</param>
  /// <returns>
  ///   <c>true</c> if the specified 2D integer array is equal to the specified <see cref="Cave"/>
  ///   object; otherwise, <c>false</c>.
  /// </returns>
  internal static bool SequenceEqual(this int[,] target1, Cave target2) {
    if (target1 == null || target2 == null || target1.GetLength(0) != target2.RowsCount ||
        target1.GetLength(1) != target2.ColumnsCount)

      return false;

    for (int i = 0; i < target2.RowsCount; i++)
      for (int j = 0; j < target2.ColumnsCount; j++)
        if (target1[i, j] != target2[i, j])
          return false;

    return true;
  }
}
