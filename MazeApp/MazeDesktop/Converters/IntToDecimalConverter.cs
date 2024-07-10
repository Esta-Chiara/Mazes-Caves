using System;
using System.Globalization;

using Avalonia;
using Avalonia.Data.Converters;

namespace MazeDesktop.Converters;

internal class IntToDecimalConverter : IValueConverter {
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
    if (value is int intValue) {
      return (decimal)intValue;
    }
    return AvaloniaProperty.UnsetValue;
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
    if (value is decimal decimalValue) {
      return (int)decimalValue;
    }
    return AvaloniaProperty.UnsetValue;
  }
}
