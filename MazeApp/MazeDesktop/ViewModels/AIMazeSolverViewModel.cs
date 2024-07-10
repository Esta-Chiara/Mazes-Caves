using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive;
using System.Threading;
using System.Threading.Tasks;

using AIMazeSolver;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;

using CommonCore;

using MazeCore;

using MazeDesktop.Views;

using ReactiveUI;
namespace MazeDesktop.ViewModels;

public class AIMazeSolverViewModel : ViewModelBase {
  private QLearningSolver? _solver;

  private Maze _maze;
  public Maze MazePuzzle {
    get => _maze;
    set => this.RaiseAndSetIfChanged(ref _maze, value);
  }

  private int _rowsCount = 5;
  public int RowsCount {
    get => _rowsCount;
    set {
      this.RaiseAndSetIfChanged(ref _rowsCount, value);
      this.RaisePropertyChanged(nameof(IsCheckBoxEnabled));
    }
  }

  private int _collsCount = 5;
  public int ColsCount {
    get => _collsCount;
    set {
      this.RaiseAndSetIfChanged(ref _collsCount, value);
      this.RaisePropertyChanged(nameof(IsCheckBoxEnabled));
    }
  }

  public bool IsCheckBoxEnabled => ColsCount < 7 && RowsCount < 7;

  private int _startRow = 1;
  public int StartRow {
    get => _startRow;
    set => this.RaiseAndSetIfChanged(ref _startRow, value);
  }

  private int _startCol = 1;
  public int StartCol {
    get => _startCol;
    set => this.RaiseAndSetIfChanged(ref _startCol, value);
  }

  private int _finishRow = 2;
  public int FinishRow {
    get => _finishRow;
    set => this.RaiseAndSetIfChanged(ref _finishRow, value);
  }

  private int _finishCol = 2;
  public int FinishCol {
    get => _finishCol;
    set => this.RaiseAndSetIfChanged(ref _finishCol, value);
  }

  private Cell _finishCell;
  public Cell FinishCell {
    get => _finishCell;
    set => this.RaiseAndSetIfChanged(ref _finishCell, value);
  }

  private Cell _startCell;
  public Cell StartCell {
    get => _startCell;
    set => this.RaiseAndSetIfChanged(ref _startCell, value);
  }

  private bool _demonstrate;
  public bool Demonstrate {
    get => _demonstrate;
    set => this.RaiseAndSetIfChanged(ref _demonstrate, value);
  }

  private List<Cell> _route;
  public List<Cell> Route {
    get => _route;
    set => this.RaiseAndSetIfChanged(ref _route, value);
  }

  public ReactiveCommand<Unit, Unit> OpenFileCommand { get; }
  public ReactiveCommand<Unit, Unit> FitModelCommand { get; }
  public ReactiveCommand<Unit, Unit> SolveMazeCommand { get; }

  public AIMazeSolverViewModel() {
    OpenFileCommand = ReactiveCommand.CreateFromTask(OpenMazeAsync);
    SolveMazeCommand = ReactiveCommand.CreateFromTask(SolveMazeAsync);
    FitModelCommand = ReactiveCommand.CreateFromTask(FitModelAsync);
  }

  private async Task OpenMazeAsync() {
    var filePath = await SelectFile();

    try {
      if (filePath is null) {
        return;
      }
      MazeTxtReader _mazeReader = new(filePath);
      MazePuzzle = _mazeReader.Read();

      if (MazePuzzle is null) {
        return;
      }
      RowsCount = MazePuzzle.RowsCount;
      ColsCount = MazePuzzle.ColsCount;
      Route = null;
      _solver = null;
    } catch {
      await ShowOpenFileErrorMessageBox();
    }
  }

  private async Task FitModelAsync() {
    if (MazePuzzle is not null) {
      FinishRow = Math.Min(MazePuzzle.RowsCount, FinishRow);
      FinishCol = Math.Min(MazePuzzle.ColsCount, FinishCol);

      FinishCell = new Cell(FinishRow - 1, FinishCol - 1);
      _solver = new(MazePuzzle, FinishCell, randomSeed: 21, saveLogs: Demonstrate);
      _solver.Fit();
      if (Demonstrate) {
        foreach (var route in _solver.Routes) {
          Route = route;
          await Task.Delay(50);
        }
      }
    }
  }

  private async Task SolveMazeAsync() {
    if (MazePuzzle is not null && _solver is not null) {
      StartRow = Math.Min(MazePuzzle.RowsCount, StartRow);
      StartCol = Math.Min(MazePuzzle.ColsCount, StartCol);

      StartCell = new Cell(StartRow - 1, StartCol - 1);
      Route = _solver.Solve(StartCell);
    }
  }
}