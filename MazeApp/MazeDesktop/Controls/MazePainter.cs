using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

using CommonCore;

using MazeCore;

namespace MazeDesktop.Controls;

public class MazePainter : Control {
  const int _thickness = 2;
  const double _fieldSize = 500;
  private Pen _borderPen = new(Brushes.Black, _thickness, lineCap: PenLineCap.Square);
  private Pen _solvePen = new(Brushes.Red, _thickness, lineCap: PenLineCap.Square);

  static MazePainter() {
    AffectsRender<MazePainter>(MazePuzzleProperty);
    AffectsRender<MazePainter>(RouteProperty);
  }

  public MazePainter() {}

  public Maze MazePuzzle {
    get => GetValue(MazePuzzleProperty);
    set => SetValue(MazePuzzleProperty, value);
  }
  public static readonly StyledProperty<Maze> MazePuzzleProperty =
      AvaloniaProperty.Register<MazePainter, Maze>(nameof(MazePuzzle));

  public List<Cell> Route {
    get => GetValue(RouteProperty);
    set => SetValue(RouteProperty, value);
  }
  public static readonly StyledProperty<List<Cell>> RouteProperty =
      AvaloniaProperty.Register<MazePainter, List<Cell>>(nameof(Route));

  public override void Render(DrawingContext drawingContext) {
    base.Render(drawingContext);
    if (MazePuzzle is null) {
      return;
    }

    double cellWidth = CellWidth();
    double cellHeight = CellHeight();

    DrawVerticalBorders(drawingContext, cellWidth, cellHeight);
    DrawHorizontalBorders(drawingContext, cellWidth, cellHeight);
    DrawOutsideBorders(drawingContext);

    DrawRoute(drawingContext, cellWidth, cellHeight);
  }

  private void DrawRoute(DrawingContext drawingContext, double cellWidth, double cellHeight) {
    if (Route is not null) {
      Cell startCell = Route[0];
      Cell finishCell = Route[^1];
      var startPoint = new Point(startCell.Col * cellWidth + cellWidth / 2,
                                 startCell.Row * cellHeight + cellHeight / 2);
      var finishPoint = new Point(finishCell.Col * cellWidth + cellWidth / 2,
                                  finishCell.Row * cellHeight + cellHeight / 2);

      drawingContext.DrawEllipse(Brushes.Red, _solvePen, startPoint, _thickness, _thickness);
      drawingContext.DrawEllipse(Brushes.Red, _solvePen, finishPoint, _thickness, _thickness);

      for (int i = 0; i < Route.Count - 1; i++) {
        startCell = Route[i];
        finishCell = Route[i + 1];
        startPoint = new Point(startCell.Col * cellWidth + cellWidth / 2,
                               startCell.Row * cellHeight + cellHeight / 2);
        finishPoint = new Point(finishCell.Col * cellWidth + cellWidth / 2,
                                finishCell.Row * cellHeight + cellHeight / 2);

        drawingContext.DrawLine(_solvePen, startPoint, finishPoint);
      }
    }
  }

  private void DrawHorizontalBorders(DrawingContext drawingContext, double cellWidth,
                                     double cellHeight) {
    if (MazePuzzle is null) {
      return;
    }
    var horizontalBorders = MazePuzzle.HorizontalBorders;

    for (int i = 0; i < horizontalBorders.GetLength(0); i++) {
      for (int j = 0; j < horizontalBorders.GetLength(1); j++) {
        if (horizontalBorders[i, j] == 1) {
          var startPoint = new Point(j * cellWidth, cellHeight + i * cellHeight);
          var endPoint = new Point((j + 1) * cellWidth, cellHeight + i * cellHeight);

          drawingContext.DrawLine(_borderPen, startPoint, endPoint);
        }
      }
    }
  }

  private void DrawVerticalBorders(DrawingContext drawingContext, double cellWidth,
                                   double cellHeight) {
    if (MazePuzzle is null) {
      return;
    }

    var verticalBorders = MazePuzzle.VerticalBorders;
    for (int i = 0; i < verticalBorders.GetLength(0); i++) {
      for (int j = 0; j < verticalBorders.GetLength(1); j++) {
        if (verticalBorders[i, j] == 1) {
          var startPoint = new Point(cellWidth + j * cellWidth, i * cellHeight);
          var endPoint = new Point(cellWidth + j * cellWidth, (i + 1) * cellHeight);

          drawingContext.DrawLine(_borderPen, startPoint, endPoint);
        }
      }
    }
  }

  private void DrawOutsideBorders(DrawingContext drawingContext) {
    // upper Point(col, row)
    drawingContext.DrawLine(_borderPen, new Point(_fieldSize, 0), new Point(0, 0));
    // left
    drawingContext.DrawLine(_borderPen, new Point(0, 0), new Point(0, _fieldSize));
    // bottom
    drawingContext.DrawLine(_borderPen, new Point(0, _fieldSize - _thickness / 2),
                            new Point(_fieldSize, _fieldSize - _thickness / 2));
    // right
    drawingContext.DrawLine(_borderPen, new Point(_fieldSize, _fieldSize),
                            new Point(_fieldSize, 0));
  }

  private double CellWidth() {
    return _fieldSize / MazePuzzle.VerticalBorders.GetLength(1);
  }

  private double CellHeight() {
    return _fieldSize / MazePuzzle.HorizontalBorders.GetLength(0);
  }
}
