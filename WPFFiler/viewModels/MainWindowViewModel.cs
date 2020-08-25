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

        public FileList SubFileList {
            get;
            private set;
        } = new FileList(@"C:\");

        private FileListControlCommands fileListControlCommands;
        public FileListControlCommands FileListControlCommands {
            get => fileListControlCommands;
            set => SetProperty(ref fileListControlCommands, value);
        }

        private IDialogService dialogService;

        public MainWindowViewModel(IDialogService dialogService) {
            this.dialogService = dialogService;
            FileListControlCommands = new FileListControlCommands(dialogService, FileList, SubFileList);
        }

    }
}
