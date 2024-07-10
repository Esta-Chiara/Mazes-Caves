using CommonCore;

namespace CaveCore;

/// <summary>
/// Generates and evolves cave structures based on specified parameters.
/// </summary>
public class CaveGenerator {
  private int? _randomSeed = null;
  private int _rowsCount;
  private int _columnsCount;

  private int _limitOfLife;
  private int _limitOfDeath;
  private double _probabilityOfInit;

  /// <summary>
  /// Gets the current cave.
  /// </summary>
  public Cave CurrentCave { get; private set; }

  /// <summary>
  /// Gets a value indicating whether the cave has reached a stable state.
  /// </summary>
  public bool IsStable { get; private set; } = false;

  /// <summary>
  /// Initializes a new instance of the <see cref="CaveGenerator"/> class.
  /// </summary>
  /// <param name="size">The size of the cave.</param>
  /// <param name="limitOfLife">The limit of life for cells.</param>
  /// <param name="limitOfDeath">The limit of death for cells.</param>
  /// <param name="probabilityOfInit">The probability of initializing a cell as alive.</param>
  /// <param name="randomSeed">An optional seed for random number generation.</param>
  public CaveGenerator(Size size, int limitOfLife, int limitOfDeath, double probabilityOfInit,
                       int? randomSeed = null) {
    _rowsCount = size.RowsCount;
    _columnsCount = size.ColumnsCount;
    _limitOfLife = limitOfLife;
    _limitOfDeath = limitOfDeath;
    _probabilityOfInit = probabilityOfInit;
    _randomSeed = randomSeed;
    InitCave();
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="CaveGenerator"/> class with an existing cave.
  /// </summary>
  /// <param name="cave">The existing cave.</param>
  /// <param name="limitOfLife">The limit of life for cells.</param>
  /// <param name="limitOfDeath">The limit of death for cells.</param>
  public CaveGenerator(Cave cave, int limitOfLife, int limitOfDeath) {
    CurrentCave = cave;
    _rowsCount = cave.RowsCount;
    _columnsCount = cave.ColumnsCount;
    _limitOfLife = limitOfLife;
    _limitOfDeath = limitOfDeath;
  }

  /// <summary>
  /// Gets the next generation of the cave.
  /// </summary>
  /// <returns>The next generation <see cref="Cave"/>.</returns>
  public Cave GetNextGenerationCave() {
    int[,] tmp = new int[_rowsCount, _columnsCount];
    for (int i = 0; i < _rowsCount; i++) {
      for (int j = 0; j < _columnsCount; j++) {
        if (ShouldBeAlive(new Cell(i, j))) {
          tmp[i, j] = 1;
        }
      }
    }

    IsStable = tmp.SequenceEqual(CurrentCave);

    CurrentCave = new Cave(tmp);

    return CurrentCave;
  }

  /// <summary>
  /// Initializes the random number generator.
  /// </summary>
  /// <returns>A new instance of <see cref="Random"/>.</returns>
  private Random InitRandom() {
    return (_randomSeed is null) ? new Random() : new Random((int)_randomSeed);
  }

  /// <summary>
  /// Initializes the cave with random values based on the probability of initialization.
  /// </summary>
  private void InitCave() {
    var random = InitRandom();
    IsStable = false;
    int[,] caveMatrix = new int[_rowsCount, _columnsCount];

    for (int row = 0; row < _rowsCount; row++) {
      for (int col = 0; col < _columnsCount; col++) {
        caveMatrix[row, col] = random.Next(101) <= 100 * _probabilityOfInit ? 1 : 0;
      }
    }
    CurrentCave = new Cave(caveMatrix);
  }

  /// <summary>
  /// Determines whether a cell should be alive based on its neighbors.
  /// </summary>
  /// <param name="cell">The cell to check.</param>
  /// <returns><c>true</c> if the cell should be alive; otherwise, <c>false</c>.</returns>
  private bool ShouldBeAlive(Cell cell) {
    int livingsCount = cell.GetCountOfLivingNigtbours(CurrentCave);

    // If cell is "live"
    // the number of "live" GetAllNeighbours < the "death" limit, they "die".
    if (cell.IsAlive(CurrentCave)) {
      return livingsCount >= _limitOfDeath;
    }
    // if "dead" cells are next to "live" cells, the number of which > the "birth" limit, they
    // become "live".
    else {
      return livingsCount > _limitOfLife;
    }
  }

  /// <summary>
  /// Returns a string that represents the current <see cref="CaveGenerator"/>.
  /// </summary>
  /// <returns>A string that represents the current <see cref="CaveGenerator"/>.</returns>
  public override string ToString() {
    return $"{nameof(CaveGenerator)}:\n" + $"_randomSeed = {_randomSeed};\n" +
           $"_rowsCount = {_rowsCount};\n" + $"_columnsCount = {_columnsCount};\n" +
           $"_limitOfLife = {_limitOfLife};\n" + $"_limitOfDeath = {_limitOfDeath};\n" +
           $"_probabilityOfInit = {_probabilityOfInit};\n" + $"CurrentCave = {CurrentCave};\n" +
           $"IsStable = {IsStable};\n";
  }
}
