using System.Diagnostics;

using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace MazeDesktop.Controls;

public class OnlyNumbersUpDown : NumericUpDown {
  public OnlyNumbersUpDown() {
    this.AddHandler(TextInputEvent, TextInputHandler, RoutingStrategies.Tunnel);
  }

  private void TextInputHandler(object sender, TextInputEventArgs e) {
    if (!IsTextNumeric(e.Text)) {
      e.Handled = true;
    }
  }

  private bool IsTextNumeric(string text) {
    foreach (char c in text) {
      if (!char.IsDigit(c) && c != '.' && c != '-') {
        return false;
      }
    }
    return true;
  }
}
