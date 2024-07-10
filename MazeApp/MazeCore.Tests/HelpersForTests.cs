using CommonCore;

using MazeCore;

namespace Core.Tests;

internal static class HelpersForTests {
  internal static Directions? UsedDirection(Cell start, Cell finish) {
    if (start.Col == finish.Col) {
      if (start.Row - 1 == finish.Row)
        return Directions.Up;
      else if (start.Row + 1 == finish.Row)
        return Directions.Down;
    } else if (start.Row == finish.Row) {
      if (start.Col - 1 == finish.Col)
        return Directions.Left;
      else if (start.Col + 1 == finish.Col)
        return Directions.Right;
    }

    return null;
  }
  internal static bool IsCorrectRoute(Maze maze, Cell[] route, Cell start, Cell finish) {
    if (!route.FirstOrDefault().Equals(start) || !route.LastOrDefault().Equals(finish) ||
        route.ToHashSet().Count != route.Length) {
      return false;
    }

    var directions = maze.CreateDirectionsMap();
    Cell current = route[0];

    for (var i = 1; i < route.Length; i++) {
      Cell next = route[i];
      var usedDirection = UsedDirection(current, next);
      if (usedDirection is null ||
          !directions[current.Row, current.Col].Contains((Directions)usedDirection)) {
        return false;
      }
      current = next;
    }

    return true;
  }
}
