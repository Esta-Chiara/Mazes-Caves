namespace CommonCore;

/// <summary>
/// Provides extension methods for the <see cref="Directions"/> enum.
/// </summary>
public static class DirectionExtantions {
  /// <summary>
  /// Gets the opposite direction of the specified direction.
  /// </summary>
  /// <param name="direction">The direction for which to find the opposite.</param>
  /// <returns>The opposite direction.</returns>
  public static Directions GetOppositeDirection(this Directions direction) {
    switch (direction) {
      case Directions.Left:
        return Directions.Right;
      case Directions.Up:
        return Directions.Down;
      case Directions.Right:
        return Directions.Left;
      case Directions.Down:
        return Directions.Up;
      default:
        throw new NotImplementedException();
    }
  }
}
