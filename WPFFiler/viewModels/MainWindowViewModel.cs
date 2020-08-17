using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFFiler.models;
using Prism.Services.Dialogs;

namespace WPFFiler.ViewModels {
    class MainWindowViewModel : BindableBase {
        public FileList FileList {
            get;
            private set;
        } = new FileList(@"C:\");

        public FileListControlCommands FileListControlCommands {
            get;
            private set;
        }

        private IDialogService dialogService;

        public MainWindowViewModel(IDialogService dialogService) {
            this.dialogService = dialogService;
            FileListControlCommands = new FileListControlCommands(FileList);
        }

    }
}
