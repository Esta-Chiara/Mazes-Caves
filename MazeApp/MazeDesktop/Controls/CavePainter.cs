using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

using CaveCore;

namespace MazeDesktop.Controls;

public class CavePainter : Control {
  const double _fieldSize = 500;
  static CavePainter() {
    AffectsRender<CavePainter>(CavePuzzleProperty);
  }

  public CavePainter() {}

  public static readonly StyledProperty<Cave> CavePuzzleProperty =
      AvaloniaProperty.Register<CavePainter, Cave>(nameof(CavePuzzle));

  public Cave CavePuzzle {
    get => GetValue(CavePuzzleProperty);
    set => SetValue(CavePuzzleProperty, value);
  }

  public override void Render(DrawingContext drawingContext) {
    base.Render(drawingContext);

    if (CavePuzzle == null)
      return;

    int rowsCount = CavePuzzle.RowsCount;
    int columnsCount = CavePuzzle.ColumnsCount;
    double cellWidth = _fieldSize * 1.0 / columnsCount;
    double cellHeight = _fieldSize * 1.0 / rowsCount;

    for (int row = 0; row < rowsCount; row++) {
      for (int col = 0; col < columnsCount; col++) {
        if (CavePuzzle[row, col] == 1) {
          var rect = new Rect(col * cellWidth, row * cellHeight, cellWidth, cellHeight);
          drawingContext.FillRectangle(Brushes.Black, rect);
        }
      }
    }
  }
}
