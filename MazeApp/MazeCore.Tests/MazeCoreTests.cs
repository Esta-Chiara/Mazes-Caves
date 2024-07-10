using System.Diagnostics;

using CaveCore;

using CommonCore;

using Core.Tests;

namespace MazeCore.Tests;

public class MazeCoreTests {
  [Theory]
  [InlineData(0, 0)]
  [InlineData(-1, 10)]
  [InlineData(-30, -30)]
  [InlineData(0, 10)]
  [InlineData(10, 0)]
  [InlineData(51, -51)]
  public void EllersMazeGenerator_WrongSize_ThrowArgumentOutOfRangeException(int rowsCount,
                                                                             int colsCount) {
    Assert.Throws<ArgumentOutOfRangeException>(
        () => new EllersMazeGenerator(new Size(rowsCount, colsCount)));
  }

  [Theory]
  [InlineData(0, 0)]
  [InlineData(2, 1)]
  [InlineData(1, 2)]
  [InlineData(1, 1)]
  [InlineData(100, 10)]
  [InlineData(10, 100)]
  [InlineData(51, 51)]
  [InlineData(51, 10)]
  [InlineData(10, 51)]
  public void MazeConstructor_WrongSize_ThrowArgumentOutOfRangeException(int rowsCount,
                                                                         int colsCount) {
    int[,] borders = new int[rowsCount, colsCount];
    Assert.Throws<ArgumentOutOfRangeException>(() => new Maze(borders, borders));
  }

  [Theory]
  [InlineData(2, 3)]
  [InlineData(10, 11)]
  [InlineData(11, 10)]
  [InlineData(3, 2)]
  [InlineData(50, 5)]
  [InlineData(5, 50)]
  public void MazeConstructor_IncompatibleSize_ThrowArgumentOutOfRangeException(int size1,
                                                                                int size2) {
    int[,] borders1 = new int[size1, size1];
    int[,] borders2 = new int[size2, size2];
    Assert.Throws<ArgumentOutOfRangeException>(() => new Maze(borders1, borders2));
  }

  [Fact]
  public void MazeConstructor_NullBorders_ThrowArgumentNullException1() {
    Assert.Throws<ArgumentNullException>(() => new Maze(null, new int[3, 3]));
  }

  [Fact]
  public void MazeConstructor_NullBorders_ThrowArgumentNullException2() {
    Assert.Throws<ArgumentNullException>(() => new Maze(new int[3, 3], null));
  }

  [Fact]
  public void MazeConstructor_NullBorders_ThrowArgumentNullException3() {
    Assert.Throws<ArgumentNullException>(() => new Maze(null, null));
  }

  private bool MazeHasNonVisitedCells(Maze maze) {
    var countVisits = CountVisits(maze);

        int nonVisitedCount = (from int item in countVisits
                               where item == 0
                               select item).Count();
#region DebugInfo
        if (nonVisitedCount > 0) {
          Debug.WriteLine(
              "*************************MazeHasNonVisitedCells*********************************");
          Debug.WriteLine("----------------------------------------------------------");
          for (int i = 0; i < maze.RowsCount; i++) {
            for (int j = 0; j < maze.ColsCount; j++) Debug.Write(countVisits[i, j]);
            Debug.WriteLine("");
          }
          Debug.WriteLine(
              "--------------------------VerticalBorders--------------------------------");
          for (int k = 0; k < maze.RowsCount; k++) {
            for (int j = 0; j < maze.ColsCount; j++) Debug.Write($"{maze.VerticalBorders[k, j]} ");
            Debug.WriteLine("");
          }
          Debug.WriteLine(
              "--------------------------HorizontalBorders--------------------------------");
          for (int k = 0; k < maze.RowsCount; k++) {
            for (int j = 0; j < maze.ColsCount; j++)
              Debug.Write($"{maze.HorizontalBorders[k, j]} ");
            Debug.WriteLine("");
          }
          Debug.WriteLine("----------------------------------------------------------");
        }
#endregion

        return nonVisitedCount > 0;
  }

  private bool MazeHasLoops(Maze maze) {
    var countVisits = CountVisits(maze);

        int nonVisitedCount = (from int item in countVisits
                               where item > 1
                               select item).Count();
#region DebugInfo
        if (nonVisitedCount > 0) {
          Debug.WriteLine("*************************MazeHasLoops*********************************");
          Debug.WriteLine("----------------------------------------------------------");
          for (int i = 0; i < maze.RowsCount; i++) {
            for (int j = 0; j < maze.ColsCount; j++) Debug.Write(countVisits[i, j]);
            Debug.WriteLine("");
          }
          Debug.WriteLine(
              "--------------------------VerticalBorders--------------------------------");
          for (int k = 0; k < maze.RowsCount; k++) {
            for (int j = 0; j < maze.ColsCount; j++) Debug.Write($"{maze.VerticalBorders[k, j]} ");
            Debug.WriteLine("");
          }
          Debug.WriteLine(
              "--------------------------HorizontalBorders--------------------------------");
          for (int k = 0; k < maze.RowsCount; k++) {
            for (int j = 0; j < maze.ColsCount; j++)
              Debug.Write($"{maze.HorizontalBorders[k, j]} ");
            Debug.WriteLine("");
          }
          Debug.WriteLine("----------------------------------------------------------");
        }
#endregion
        return nonVisitedCount > 0;
  }

  private int[,] CountVisits(Maze maze) {
    HashSet<Directions>[
      ,
    ] directions = maze.CreateDirectionsMap();
    Stack<Cell> path = new();
    var currentCell = new Cell(0, 0);
    path.Push(currentCell);
    int[,] visited = new int[maze.RowsCount, maze.ColsCount];
    visited[currentCell.Row, currentCell.Col] += 1;

    while (true) {
      if (directions[currentCell.Row, currentCell.Col].Count == 0) {
        if (path.Count <= 1) {
          break;
        }

        path.Pop();
        currentCell = path.Peek();
        continue;
      }

      Directions d = directions[currentCell.Row, currentCell.Col].First();

      directions[currentCell.Row, currentCell.Col].Remove(d);
      Cell nextCell = currentCell.GetNextByDirection(d);
      visited[nextCell.Row, nextCell.Col] += 1;
      directions[nextCell.Row, nextCell.Col].Remove(d.GetOppositeDirection());
      path.Push(nextCell);

      currentCell = nextCell;
    }

    return visited;
  }

  [Theory]
  [InlineData(3, 3)]
  [InlineData(10, 10)]
  [InlineData(2, 2)]
  [InlineData(2, 10)]
  [InlineData(10, 2)]
  [InlineData(50, 50)]
  public void EllersMazeGenerator_ReturnsIdealMaze(int rowsCount, int colsCount) {
    for (int i = 0; i < 10; i++) {
      Maze maze = new EllersMazeGenerator(new Size(rowsCount, colsCount)).Create();

      Assert.Equal(rowsCount, maze.RowsCount);
      Assert.Equal(colsCount, maze.ColsCount);
      Assert.Equal(rowsCount * colsCount, maze.HorizontalBorders.Length);
      Assert.Equal(rowsCount * colsCount, maze.VerticalBorders.Length);
      Assert.False(MazeHasNonVisitedCells(maze));
      Assert.False(MazeHasLoops(maze));
    }
  }

  [Fact]
  public void GenerateMaze_RandomSizes_ReturnsIdealMaze() {
    Random random = new Random();
    int limit = 51;
    for (int k = 0; k < 100; k++) {
      int rowsCount = random.Next(2, limit);
      int colsCount = random.Next(2, limit);
      for (int i = 0; i < rowsCount * colsCount / 2; i++) {
        Maze maze = new EllersMazeGenerator(new Size(rowsCount, colsCount)).Create();
        Assert.Equal(rowsCount, maze.RowsCount);
        Assert.Equal(colsCount, maze.ColsCount);
        Assert.Equal(rowsCount * colsCount, maze.HorizontalBorders.Length);
        Assert.Equal(rowsCount * colsCount, maze.VerticalBorders.Length);
        Assert.False(MazeHasNonVisitedCells(maze));
        Assert.False(MazeHasLoops(maze));
      }
    }
  }

#region DataInit
  private static Maze maze2_2 =>
      new Maze(new int[,] { { 0, 1 }, { 1, 1 } }, new int[,] { { 0, 0 }, { 1, 1 } });

  private static Maze maze4_4 =>
      new Maze(new int[,] { { 0, 0, 0, 1 }, { 1, 0, 1, 1 }, { 0, 1, 0, 1 }, { 0, 0, 0, 1 } },
               new int[,] { { 1, 0, 1, 0 }, { 0, 0, 1, 0 }, { 1, 1, 0, 1 }, { 1, 1, 1, 1 } });

  private static Cell cell0_0 => new Cell(0, 0);

  private static Cell cell0_1 => new Cell(0, 1);

  private static Cell cell0_2 => new Cell(0, 2);

  private static Cell cell0_3 => new Cell(0, 3);

  private static Cell cell1_0 => new Cell(1, 0);

  private static Cell cell1_1 => new Cell(1, 1);
  private static Cell cell1_3 => new Cell(1, 3);

  private static Cell cell2_3 => new Cell(2, 3);

  private static Cell cell2_2 => new Cell(2, 2);

  private static Cell cell2_0 => new Cell(2, 0);
  private static Cell cell2_1 => new Cell(2, 1);
  private static Cell cell3_2 => new Cell(3, 2);
  private static Cell cell3_3 => new Cell(3, 3);

  public static IEnumerable<object[]> SolveMazeData2_2_cell0_0_cell1_1 =>
      new List<object[]> { new object[] { maze2_2, cell0_0, cell1_1,
                                          new Cell[] { cell0_0, cell0_1, cell1_1 } } };

  public static IEnumerable<object[]> SolveMazeData2_2_cell0_0_cell1_0 =>
      new List<object[]> { new object[] { maze2_2, cell0_0, cell1_0,
                                          new Cell[] { cell0_0, cell1_0 } } };

  public static IEnumerable<object[]> SolveMazeData2_2_cell0_0_cell0_1 =>
      new List<object[]> { new object[] { maze2_2, cell0_0, cell0_1,
                                          new Cell[] { cell0_0, cell0_1 } } };

  public static IEnumerable<object[]> SolveMazeData2_2_cell0_0_cell0_0 =>
      new List<object[]> { new object[] { maze2_2, cell0_0, cell0_0, new Cell[] { cell0_0 } } };

  public static IEnumerable<object[]> SolveMazeData4_4_cell0_0_cell3_3 =>
      new List<object[]> { new object[] { maze4_4, cell0_0, cell3_3,
                                          new Cell[] { cell0_0, cell0_1, cell0_2, cell0_3, cell1_3,
                                                       cell2_3, cell2_2, cell3_2, cell3_3 } } };

  public static IEnumerable<object[]> SolveMazeData4_4_cell0_0_cell1_1 =>
      new List<object[]> { new object[] { maze4_4, cell0_0, cell1_1,
                                          new Cell[] { cell0_0, cell0_1, cell1_1 } } };

  public static IEnumerable<object[]> SolveMazeData4_4_cell0_0_cell1_0 => new List<object[]> {
    new object[] { maze4_4, cell0_0, cell1_0,
                   new Cell[] { cell0_0, cell0_1, cell1_1, cell2_1, cell2_0, cell1_0 } }
  };

  public static IEnumerable<object[]> SolveMazeData4_4_cell0_0_cell0_1 =>
      new List<object[]> { new object[] { maze4_4, cell0_0, cell0_1,
                                          new Cell[] { cell0_0, cell0_1 } } };
#endregion

  [Theory]
  [MemberData(nameof(SolveMazeData2_2_cell0_0_cell1_1))]
  [MemberData(nameof(SolveMazeData2_2_cell0_0_cell0_1))]
  [MemberData(nameof(SolveMazeData2_2_cell0_0_cell1_0))]
  [MemberData(nameof(SolveMazeData2_2_cell0_0_cell0_0))]
  [MemberData(nameof(SolveMazeData4_4_cell0_0_cell0_1))]
  [MemberData(nameof(SolveMazeData4_4_cell0_0_cell1_0))]
  [MemberData(nameof(SolveMazeData4_4_cell0_0_cell1_1))]
  [MemberData(nameof(SolveMazeData4_4_cell0_0_cell3_3))]
  public void SolveMaze_ReturnsExpectedRoute(Maze maze, Cell start, Cell finish, Cell[] expected) {
    var route = maze.Solve(start, finish);
    Assert.True(HelpersForTests.IsCorrectRoute(maze, route.ToArray(), start, finish));
    Assert.True(route.SequenceEqual(expected));
  }

  [Fact]
  public void SolveMaze_ReturnsCorrectRoute() {
    Random random = new Random();
    int limit = 51;
    for (int i = 0; i < 10000; i++) {
      int rowsCount = random.Next(2, limit);
      int colsCount = random.Next(2, limit);
      Maze maze = new EllersMazeGenerator(new Size(rowsCount, colsCount)).Create();
      Cell start = new Cell(random.Next(rowsCount), random.Next(colsCount));
      Cell finish = new Cell(random.Next(rowsCount), random.Next(colsCount));
      var route = maze.Solve(start, finish);
      Assert.True(HelpersForTests.IsCorrectRoute(maze, route.ToArray(), start, finish));
    }
  }

  private static bool IsStableCave(Cave cave, int limitOfLife, int limitOfDeath) {
    for (int i = 0; i < cave.RowsCount; i++) {
      for (int j = 0; j < cave.ColumnsCount; j++) {
        Cell cell = new(i, j);
        int livingsCount = cell.GetCountOfLivingNigtbours(cave);

        // If the number of "live" Neighbours < the "death" limit, cell should be "dead".
        // if the number of "dead" Neighbours > the "birth" limit, cell should be "live".
        if ((cell.IsAlive(cave) && livingsCount < limitOfDeath) ||
            (!cell.IsAlive(cave) && livingsCount > limitOfLife)) {
          return false;
        }
      }
    }

    return true;
  }

  [Fact]
  public void GenerateCave_ReturnsStableCave() {
    int randomSeed = 21;
    Random random = new Random(randomSeed);
    for (int i = 0; i < 100; i++) {
      int rowsCount = random.Next(2, 51);
      int colsCount = random.Next(2, 51);
      int limitOfLife = random.Next(0, 8);
      int limitOfDeath = random.Next(0, 8);
      double birthProbability = random.NextDouble();

      CaveGenerator caveGenerator = new(new Size(rowsCount, colsCount), limitOfLife, limitOfDeath,
                                        birthProbability, randomSeed);
      Debug.WriteLine(caveGenerator);
      int counter = 0;
      while (!caveGenerator.IsStable) {
        caveGenerator.GetNextGenerationCave();
        Debug.WriteLine($"******************\n{counter}");
        Debug.WriteLine(caveGenerator.CurrentCave);
        if (counter++ > 300) {
          // endles cycle. cave cannot be stable
          return;
        }
      }
      Cave cave = caveGenerator.CurrentCave;
      Assert.True(IsStableCave(cave, limitOfLife, limitOfDeath));
    }
  }

#region CaveGenerationData

  private static Cave InitCave1 => new Cave(new int[,] {
    { 0, 0, 0, 1 },  // {5, 5, 5, 1},  {0, 0, 0, 8},
    { 1, 0, 1, 1 },  // {1, 5, 1, 1},  {8, 0, 8, 8},
    { 0, 1, 0, 0 },  // {5, 1, 5, 5},  {0, 8, 0, 0},
    { 0, 0, 0, 1 }   // {5, 5, 5, 1}   {0, 0, 0, 8}
  });

  public static IEnumerable<object[]> CaveGenerationData_3_4 =>
      new List<object[]> { new object[] { InitCave1, 3, 4,
                                          new Cave(new int[,] {
                                            { 1, 1, 1, 1 },  // {5, 5, 5, 1},  {1, 1, 1, 8},
                                            { 1, 0, 0, 1 },  // {1, 5, 0, 1},  {8, 0, 8, 8},
                                            { 1, 0, 1, 1 },  // {5, 0, 5, 5},  {1, 8, 1, 1},
                                            { 1, 1, 1, 1 }   // {5, 5, 5, 1}   {1, 1, 1, 8}
                                          }) } };

  public static IEnumerable<object[]> CaveGenerationData_0_5 =>
      new List<object[]> { new object[] { InitCave1, 0, 5,
                                          new Cave(new int[,] {
                                            { 1, 1, 1, 1 },  // {5, 5, 5, 1},  {0, 0, 0, 8},
                                            { 0, 1, 0, 1 },  // {0, 5, 0, 1},  {8, 0, 8, 8},
                                            { 1, 0, 1, 1 },  // {5, 0, 5, 5},  {0, 8, 0, 0},
                                            { 1, 1, 1, 1 }   // {5, 5, 5, 1}   {0, 0, 0, 8}
                                          }) } };

  public static IEnumerable<object[]> CaveGenerationData_7_4 =>
      new List<object[]> { new object[] { InitCave1, 7, 4,
                                          new Cave(new int[,] {
                                            { 0, 0, 0, 1 },  // {5, 5, 5, 1},  {0, 0, 0, 8},
                                            { 1, 0, 0, 1 },  // {1, 5, 0, 1},  {8, 0, 8, 8},
                                            { 0, 0, 0, 0 },  // {5, 0, 5, 5},  {0, 8, 0, 0},
                                            { 0, 0, 0, 1 }   // {5, 5, 5, 1}   {0, 0, 0, 8}
                                          }) } };

  public static IEnumerable<object[]> CaveGenerationData_3_0 =>
      new List<object[]> { new object[] { InitCave1, 3, 0,
                                          new Cave(new int[,] {
                                            { 1, 1, 1, 1 },  // {0, 0, 0, 1},  {1, 1, 1, 8},
                                            { 1, 0, 1, 1 },  // {1, 0, 1, 1},  {8, 0, 8, 8},
                                            { 1, 1, 1, 1 },  // {0, 1, 0, 0},  {1, 8, 1, 1},
                                            { 1, 1, 1, 1 }   // {0, 0, 0, 1}   {1, 1, 1, 8}
                                          }) } };

  public static IEnumerable<object[]> CaveGenerationData_3_7 => new List<object[]> { new object[] {
    InitCave1, 3, 7,
    new Cave(new int[,] { { 1, 1, 1, 1 }, { 0, 0, 0, 0 }, { 1, 0, 1, 1 }, { 1, 1, 1, 0 } })
  } };
#endregion

  [Theory]
  [MemberData(nameof(CaveGenerationData_3_4))]
  [MemberData(nameof(CaveGenerationData_0_5))]
  [MemberData(nameof(CaveGenerationData_7_4))]
  [MemberData(nameof(CaveGenerationData_3_0))]
  [MemberData(nameof(CaveGenerationData_3_7))]
  public void GetNextGenerationCave_ReturnsCorrectGeneration(Cave cave, int limitOfLife,
                                                             int limitOfDeath, Cave expected) {
    Debug.WriteLine("*************");
    CaveGenerator caveGenerator = new(cave, limitOfLife, limitOfDeath);
    Debug.WriteLine(caveGenerator);

    Cave current = caveGenerator.GetNextGenerationCave();
    Debug.WriteLine(current);
    Assert.Equal(expected, current);
  }
}