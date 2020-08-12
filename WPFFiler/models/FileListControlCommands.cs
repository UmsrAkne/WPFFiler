using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFFiler.models {
    public class FileListControlCommands {

        private FileList mainFileList;

        public FileListControlCommands(FileList main) {
            mainFileList = main;
        }

        private DelegateCommand moveCursorToEndCommand;
        public DelegateCommand MoveCursorToEndCommand {
            get => moveCursorToEndCommand ?? (moveCursorToEndCommand = new DelegateCommand(
                () => {
                    mainFileList.SelectedIndex = mainFileList.Files.Count - 1;
                }
            ));
        }

        private DelegateCommand moveCursorToHeadCommand;
        public DelegateCommand MoveCursorToHeadCommand {
            get => moveCursorToHeadCommand ?? (moveCursorToHeadCommand = new DelegateCommand(
                () => {
                    mainFileList.SelectedIndex = 0;
                }
            ));
        }

        private DelegateCommand downCursorCommand;
        public DelegateCommand DownCursorCommand {
            get => downCursorCommand ?? (downCursorCommand = new DelegateCommand(
                () => {
                    mainFileList.SelectedIndex++;
                }
            ));
        }

        private DelegateCommand upCursorCommand;
        public DelegateCommand UpCursorCommand {
            get => upCursorCommand ?? (upCursorCommand = new DelegateCommand(
                () => {
                    mainFileList.SelectedIndex--;
                }
            ));
        }

        private DelegateCommand reloadCommand;
        public DelegateCommand ReloadCommand {
            get => reloadCommand ?? (reloadCommand = new DelegateCommand(
                () => {
                    mainFileList.reload();
                }
            ));
        }
    }
}
