using CommonCore;
namespace MazeCore;

/// <summary>
/// Provides extension methods for solving a maze.
/// </summary>
public static class MazeSolverExtantion {
  /// <summary>
  /// Solves the maze using a depth-first search algorithm.
  /// </summary>
  /// <param name="maze">The maze to solve.</param>
  /// <param name="currentCell">The current cell to start solving from.</param>
  /// <param name="finishCell">The cell representing the finish point.</param>
  /// <returns>The path of cells from <paramref name="currentCell"/> to <paramref
  /// name="finishCell"/>.</returns>
  public static List<Cell> Solve(this Maze maze, Cell currentCell, Cell finishCell) {
    HashSet<Directions>[
      ,
    ] directions = maze.CreateDirectionsMap();
    Stack<Cell> path = new();
    path.Push(currentCell);

    while (true) {
      if (currentCell.Equals(finishCell))
        break;

      if (directions[currentCell.Row, currentCell.Col].Count == 0) {
        path.Pop();
        currentCell = path.Peek();
        continue;
      }

      Directions d = directions[currentCell.Row, currentCell.Col].First();

      directions[currentCell.Row, currentCell.Col].Remove(d);
      Cell nextCell = currentCell.GetNextByDirection(d);
      directions[nextCell.Row, nextCell.Col].Remove(d.GetOppositeDirection());
      path.Push(nextCell);

      currentCell = nextCell;
    }

    return path.Reverse().ToList();
  }

  /// <summary>
  /// Creates a map of allowed directions for each cell in the maze.
  /// </summary>
  /// <param name="maze">The maze to create directions map for.</param>
  /// <returns>A 2D array representing allowed directions for each cell.</returns>
  public static HashSet<Directions>[
    ,
  ] CreateDirectionsMap(this Maze maze) {
    var result = new HashSet<Directions>[maze.RowsCount, maze.ColsCount];

    for (int i = 0; i < maze.RowsCount; i++)
      for (int j = 0; j < maze.ColsCount; j++)
        result[i, j] = maze.GetAllowedDirections(new Cell(i, j));

    return result;
  }

  /// <summary>
  /// Retrieves the allowed directions for a given cell in the maze.
  /// </summary>
  /// <param name="maze">The maze containing the cell.</param>
  /// <param name="cell">The cell to get allowed directions for.</param>
  /// <returns>A set of directions that are allowed for the cell.</returns>
  private static HashSet<Directions> GetAllowedDirections(this Maze maze, Cell cell) {
    HashSet<Directions> directions = new();

    if (cell.Col > 0 && maze.VerticalBorders[cell.Row, cell.Col - 1] != 1)
      directions.Add(Directions.Left);
    if (cell.Row > 0 && maze.HorizontalBorders[cell.Row - 1, cell.Col] != 1)
      directions.Add(Directions.Up);
    if (cell.Col < maze.ColsCount && maze.VerticalBorders[cell.Row, cell.Col] != 1)
      directions.Add(Directions.Right);
    if (cell.Row < maze.RowsCount && maze.HorizontalBorders[cell.Row, cell.Col] != 1)
      directions.Add(Directions.Down);

    return directions;
  }

  // recursion version of Solve Throws Stack Overflow
  // private static bool FindPath(this Maze maze, Cell startCell, Cell finishCell, Stack<Cell> path,
  // HashSet<Directions>[,] directions)
  //{
  //    if (startCell.Equals(finishCell))
  //        return true;

  //    foreach (var d in directions[startCell.Row, startCell.Col])
  //    {
  //        Cell nextCell = startCell.GetNextByDirection(d);
  //        directions[nextCell.Row, nextCell.Col].Remove(GetOppositeDirection(d));
  //        path.Push(nextCell);

  //        if (maze.FindPath(nextCell, finishCell, path, directions))
  //            return true;
  //        path.Pop();
  //    }

  //    return false;
  //}
}
