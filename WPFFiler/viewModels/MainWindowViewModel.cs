namespace WPFFiler.ViewModels
{
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
    using System.ComponentModel;

    public class MainWindowViewModel : BindableBase
    {
        private FileList fileList = new FileList(@"C:\");
        public FileList FileList
        {
            get => fileList;
            private set => SetProperty(ref fileList, value);
        }

        private FileList mainFileListStorage = null;

        private FileList subFileList = new FileList(@"C:\");
        public FileList SubFileList
        {
            get => subFileList;
            private set => SetProperty(ref subFileList, value);
        }

        private FileList subFileListStorage = null;

        private FileListControlCommands fileListControlCommands;
        public FileListControlCommands FileListControlCommands
        {
            get => fileListControlCommands;
            set => SetProperty(ref fileListControlCommands, value);
        }

        private IDialogService dialogService;

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

        private DelegateCommand changeToMirrorModeCommand;
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

        private DelegateCommand changeToTwoScreenModeCommand;
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
