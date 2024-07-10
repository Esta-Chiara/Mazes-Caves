using CommonCore;

using MazeCore;

namespace AIMazeSolver;

public class QLearningSolver {
  private const double NextStepReward = -2;
  private const double CrushReward = -500;
  private const double WinReward = 500;
  private const int IterationsCountCoeff = 50;
  // Main learning params
  private Maze _maze;
  private Cell _finishCell;
  private bool _saveLogs;
  private double _explorationRate;
  private double _gamma;
  private double _learningRate;
  private int _iterationsCount;
  private int? _randomSeed;

  private Random _random;
  private Dictionary<Cell, HashSet<Directions>> _mazeDirectionsMap;
  private Cell _startCell;

  // Learning results
  private Dictionary<Cell, Dictionary<Directions, double>> _qTable;

  // Environment states at Momemt
  private double _reward;
  private bool _gameIsFinisched;
  private double _currentScore;
  private Cell _currentCell;
  private Directions _currentDirection;
  private Cell _nextCell;
  private List<Cell> _route;

  // Learning Logs
  public List<double> Scores { get; private set; }
  public List<List<Cell>> Routes { get; private set; }

  public QLearningSolver(Maze maze, Cell finishCell, bool saveLogs = true,
                         double explorationRate = 0.2, double learningRate = 0.8,
                         double gamma = 0.9, int? iterationsCount = null, int? randomSeed = null) {
    ThrowIfFinishIsOutsideTheMaze(maze, finishCell);
    _maze = maze;
    _finishCell = finishCell;
    _saveLogs = saveLogs;
    _iterationsCount = iterationsCount ?? maze.RowsCount * maze.ColsCount * IterationsCountCoeff;
    _explorationRate = explorationRate;
    _learningRate = learningRate;
    _gamma = gamma;
    _randomSeed = randomSeed;
  }

  public void Fit() {
    InitStartState();
    int counter = 0;

    while (counter++ < _iterationsCount) {
      _startCell = ChooseRandomCell();
      ResetEnvironmentState();
      while (!_gameIsFinisched) {
        _currentDirection = ChooseDirection();
        MakeStep(_currentDirection);
        UpdateQTable();
        _currentCell = _nextCell;
      }
      if (_saveLogs) {
        Routes.Add(new List<Cell>(_route));
        Scores.Add(_currentScore);
      }
    }
  }

  public List<Cell> Solve(Cell startCell) {
    _startCell = startCell;

    ResetEnvironmentState();
    int counter = 0;
    while (!_gameIsFinisched && counter++ < _maze.ColsCount * _maze.RowsCount) {
      _currentDirection = ChooseDirectionWithMaxReward();
      MakeStep(_currentDirection);
      _currentCell = _nextCell;
    }

    return _route;
  }

  private Cell ChooseRandomCell() {
    return new Cell(_random.Next(0, _maze.RowsCount), _random.Next(0, _maze.ColsCount));
  }

  private Cell ChooseMaximumRemotePosition(Cell cell) {
    return new Cell(cell.Row < _maze.RowsCount / 2 ? _maze.RowsCount - 1 : 0,
                    cell.Col < _maze.ColsCount / 2 ? _maze.ColsCount - 1 : 0);
  }

  private void InitRandom() {
    _random = _randomSeed is null ? new Random() : new Random((int)_randomSeed);
  }

  private Directions ChooseDirection() {
    if (_random.NextDouble() < _explorationRate) {
      return (Directions)_random.Next(0, 4);
    } else {
      return ChooseDirectionWithMaxReward();
    }
  }

  // Choose direction with max reward
  private Directions ChooseDirectionWithMaxReward() {
    return _qTable[_currentCell].Aggregate((a, b) => a.Value > b.Value ? a : b).Key;
  }

  private void UpdateQTable() {
    var nextCellBestReward =
        CellIsOutsideTheMaze(_maze, _nextCell) ? _reward : _qTable[_nextCell].Values.Max();
    var target = _reward + _gamma * nextCellBestReward;
    var error = target - _qTable[_currentCell][_currentDirection];
    _qTable[_currentCell][_currentDirection] += _learningRate * error;
  }

  private void MakeStep(Directions direction) {
    _nextCell = _currentCell.GetNextByDirection(direction);
    _route.Add(_nextCell);
    if (AgentIsCrushed(direction)) {
      _reward = CrushReward;
      _gameIsFinisched = true;
    } else if (_nextCell.Equals(_finishCell)) {
      _reward = WinReward;
      _gameIsFinisched = true;
    } else {
      _reward = NextStepReward;
      _gameIsFinisched = false;
    }
    _currentScore += _reward;
  }

  private bool AgentIsCrushed(Directions direction) {
    return !_mazeDirectionsMap[_currentCell].Contains(direction);
  }
  private void ResetEnvironmentState() {
    _currentCell = _startCell;
    _route = new List<Cell>() { _currentCell };
    _gameIsFinisched = _currentCell.Equals(_finishCell) ? true : false;
    _currentScore = 0;
  }

  private void InitStartState() {
    InitRandom();
    _startCell = ChooseMaximumRemotePosition(_finishCell);
    var mazeDirectionsMatrix = _maze.CreateDirectionsMap();
    _mazeDirectionsMap = new Dictionary<Cell, HashSet<Directions>>();
    _qTable = new Dictionary<Cell, Dictionary<Directions, double>>();

    for (int i = 0; i < _maze.RowsCount; i++) {
      for (int j = 0; j < _maze.ColsCount; j++) {
        var cell = new Cell(i, j);
        _qTable.Add(
            cell,
            Enum.GetValues(typeof(Directions)).Cast<Directions>().ToDictionary(d => d, d => 0.0));
        _mazeDirectionsMap.Add(cell, mazeDirectionsMatrix[cell.Row, cell.Col]);
      }
    }

    if (_saveLogs) {
      Scores = new List<double>();
      Routes = new List<List<Cell>>();
    }
  }

  private static bool CellIsOutsideTheMaze(Maze maze, Cell cell) {
    return cell.Row < 0 || cell.Row >= maze.RowsCount || cell.Col < 0 || cell.Col >= maze.ColsCount;
  }
  private static void ThrowIfFinishIsOutsideTheMaze(Maze maze, Cell finish) {
    if (CellIsOutsideTheMaze(maze, finish)) {
      throw new ArgumentOutOfRangeException("Finish cell is outside the gameboard");
    }
  }
}
