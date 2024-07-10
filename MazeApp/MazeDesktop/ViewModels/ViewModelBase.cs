using Avalonia.Controls.ApplicationLifetimes;
using System.Collections.Generic;
using System.Threading.Tasks;

using ReactiveUI;

using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive;
using System.Threading;
using System.Threading.Tasks;

using Avalonia.Controls;
using Avalonia.Platform.Storage;

using CaveCore;

using CommonCore;

using MazeCore;

using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

using ReactiveUI;
using Avalonia;

namespace MazeDesktop.ViewModels;

public class ViewModelBase : ReactiveObject {
  protected const string OpenFileErrorMsg = "File cannot be read correctly.";

  protected async Task<string> SelectFile() {
    var openFileDialog =
        new OpenFileDialog { Title = "Select a file", AllowMultiple = false,
                             Filters = new List<FileDialogFilter> { new FileDialogFilter {
                               Name = "Text Documents", Extensions = new List<string> { "txt" }
                             } } };

    var mainWindow =
        (Application.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)
            ?.MainWindow;
    var result = await openFileDialog.ShowAsync(mainWindow);

    return result is null || result.Length == 0 ? null : result[0];
  }

  protected static async Task ShowOpenFileErrorMessageBox() {
    var box = MessageBoxManager.GetMessageBoxStandard("Caption", OpenFileErrorMsg, ButtonEnum.Ok);

    var result = await box.ShowAsync();
  }
}
