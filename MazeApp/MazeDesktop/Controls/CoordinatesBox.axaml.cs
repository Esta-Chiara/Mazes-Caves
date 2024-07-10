using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;

namespace MazeDesktop.Controls;

public class CoordinatesBox : OnlyNumbersUpDown {
  public static readonly StyledProperty<int> ValueProperty =
      AvaloniaProperty.Register<LimitedSizeUpDown, int>(nameof(Value),
                                                        defaultBindingMode: BindingMode.TwoWay);

  public int Value {
    get => GetValue(ValueProperty);
    set => SetValue(ValueProperty, value);
  }

  public static readonly StyledProperty<int> MaximumProperty =
      AvaloniaProperty.Register<LimitedSizeUpDown, int>(nameof(Maximum),
                                                        defaultBindingMode: BindingMode.TwoWay);

  public int Maximum {
    get => GetValue(MaximumProperty);
    set => SetValue(MaximumProperty, value);
  }
}
