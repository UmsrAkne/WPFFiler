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
using Prism.Commands;

namespace WPFFiler.ViewModels {
    class MainWindowViewModel : BindableBase {
        private FileList fileList = new FileList(@"C:\");
        public FileList FileList {
            get => fileList;
            private set => SetProperty(ref fileList, value);
        }

        private FileList mainFileListStorage = null;

        private FileList subFileList = new FileList(@"C:\");
        public FileList SubFileList {
            get => subFileList;
            private set => SetProperty(ref subFileList, value);
        }

        private FileList subFileListStorage = null;

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

        private DelegateCommand changeToMirrorModeCommand;
        public DelegateCommand ChangeToMirrorModeCommand {
            get => changeToMirrorModeCommand ?? (changeToMirrorModeCommand = new DelegateCommand(
                () => {
                    if(mainFileListStorage == null && subFileListStorage == null) {
                        mainFileListStorage = FileList;
                        subFileListStorage = SubFileList;
                        SubFileList = FileList;
                    }
                }
            ));
        }

        private DelegateCommand changeToTwoScreenModeCommand;
        public DelegateCommand ChangeToTwoScreenModeCommand {
            get => changeToTwoScreenModeCommand ?? (changeToTwoScreenModeCommand = new DelegateCommand(
                () => {
                    if(mainFileListStorage != null || subFileListStorage != null) {
                        FileList = mainFileListStorage;
                        SubFileList = subFileListStorage;
                        mainFileListStorage = null;
                        subFileListStorage = null;
                    }
                }
            ));
        }
    }
}
