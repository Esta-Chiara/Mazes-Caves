namespace CommonCore;

/// <summary>
/// Provides extension methods for comparing two-dimensional integer arrays.
/// </summary>
public static class SequencesExtantions {
  /// <summary>
  /// Determines whether two-dimensional integer arrays are equal.
  /// </summary>
  /// <param name="target1">The first array to compare.</param>
  /// <param name="target2">The second array to compare.</param>
  /// <returns><c>true</c> if the arrays are equal; otherwise, <c>false</c>.</returns>
  public static bool SequenceEqual(this int[,] target1, int[,] target2) {
    if (target1 == null || target2 == null || target1.GetLength(0) != target2.GetLength(0) ||
        target1.GetLength(1) != target2.GetLength(1))

      return false;

    for (int i = 0; i < target1.GetLength(0); i++)
      for (int j = 0; j < target1.GetLength(1); j++)
        if (target1[i, j] != target2[i, j])
          return false;

    return true;
  }
}
