using Avalonia.Controls;
using Avalonia.Markup.Xaml;

using MazeDesktop.ViewModels;

namespace MazeDesktop.Views;

public partial class AIMazeSolverView : UserControl {
  public AIMazeSolverView() {
    this.InitializeComponent();
    this.DataContext = new AIMazeSolverViewModel();
  }

  private void InitializeComponent() {
    AvaloniaXamlLoader.Load(this);
  }
}
