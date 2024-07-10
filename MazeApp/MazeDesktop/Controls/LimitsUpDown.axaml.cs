using Avalonia;
using Avalonia.Data;

namespace MazeDesktop.Controls;

public class LimitsUpDown : OnlyNumbersUpDown {
  public static readonly StyledProperty<int> ValueProperty =
      AvaloniaProperty.Register<LimitsUpDown, int>(nameof(Value),
                                                   defaultBindingMode: BindingMode.TwoWay);

  public int Value {
    get => GetValue(ValueProperty);
    set => SetValue(ValueProperty, value);
  }
}
