using Avalonia;
using Avalonia.Data;

namespace MazeDesktop.Controls;

public class LimitedSizeUpDown : OnlyNumbersUpDown {
  public static readonly StyledProperty<int> ValueProperty =
      AvaloniaProperty.Register<LimitedSizeUpDown, int>(nameof(Value),
                                                        defaultBindingMode: BindingMode.TwoWay);

  public int Value {
    get => GetValue(ValueProperty);
    set => SetValue(ValueProperty, value);
  }
}
