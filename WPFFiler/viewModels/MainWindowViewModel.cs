namespace WPFFiler.ViewModels
{
    using System;
    using System.IO;
    using WPFFiler.Models;
    using Prism.Commands;
    using Prism.Mvvm;
    using Prism.Services.Dialogs;
    using System.ComponentModel;

    public class MainWindowViewModel : BindableBase
    {
        private FileList fileList = new FileList(@"C:\");
        private FileList mainFileListStorage = null;
        private FileList subFileList = new FileList(@"C:\");
        private FileList subFileListStorage = null;
        private FileListControlCommands fileListControlCommands;
        private IDialogService dialogService;
        private DelegateCommand changeToMirrorModeCommand;
        private DelegateCommand changeToTwoScreenModeCommand;

        public MainWindowViewModel(IDialogService dialogService)
        {
            this.dialogService = dialogService;
            FileListControlCommands = new FileListControlCommands(dialogService, FileList, SubFileList);

            PropertyChangedEventHandler pcEventHandler = (Object sender, PropertyChangedEventArgs p) =>
            {
                if (p.PropertyName == nameof(FileList.CurrentDirectoryPath))
                {
                    RaisePropertyChanged(nameof(CurrentDirectoriesPath));
                }
            };

            FileList.PropertyChanged += pcEventHandler;
            SubFileList.PropertyChanged += pcEventHandler;
        }


        public FileList FileList
        {
            get => fileList;
            private set => SetProperty(ref fileList, value);
        }

        public FileList SubFileList
        {
            get => subFileList;
            private set => SetProperty(ref subFileList, value);
        }

        public FileListControlCommands FileListControlCommands
        {
            get => fileListControlCommands;
            set => SetProperty(ref fileListControlCommands, value);
        }

        public String CurrentDirectoriesPath
        {
            get
            {
                DirectoryInfo di1 = new DirectoryInfo(FileList.CurrentDirectoryPath);
                DirectoryInfo di2 = new DirectoryInfo(SubFileList.CurrentDirectoryPath);
                return (FileList.BothViewBinding || SubFileList.BothViewBinding)
                    ? "[複製] " + di1.Name : "[２画面] " + di1.Name + " / " + di2.Name;
            }
        }

        public DelegateCommand ChangeToMirrorModeCommand
        {
            get => changeToMirrorModeCommand ?? (changeToMirrorModeCommand = new DelegateCommand(
                () =>
                {
                    if (mainFileListStorage == null && subFileListStorage == null)
                    {
                        mainFileListStorage = FileList;
                        subFileListStorage = SubFileList;
                        SubFileList = FileList;
                        FileList.BothViewBinding = true;
                        RaisePropertyChanged(nameof(CurrentDirectoriesPath));
                    }
                }));
        }

        public DelegateCommand ChangeToTwoScreenModeCommand
        {
            get => changeToTwoScreenModeCommand ?? (changeToTwoScreenModeCommand = new DelegateCommand(
                () =>
                {
                    if (mainFileListStorage != null || subFileListStorage != null)
                    {
                        FileList = mainFileListStorage;
                        FileList.BothViewBinding = false;
                        SubFileList = subFileListStorage;
                        SubFileList.BothViewBinding = false;

                        mainFileListStorage = null;
                        subFileListStorage = null;
                        RaisePropertyChanged(nameof(CurrentDirectoriesPath));
                    }
                }));
        }
    }
}
