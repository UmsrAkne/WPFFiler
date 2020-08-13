using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WPFFiler.models {
    public class FileListControlCommands {

        private int repeatCount = 0;
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
                    if(repeatCount == 0) {
                        mainFileList.SelectedIndex = 0;
                    }
                    else {
                        mainFileList.SelectedIndex = repeatCount;
                        repeatCount = 0;
                    }
                }
            ));
        }

        private DelegateCommand downCursorCommand;
        public DelegateCommand DownCursorCommand {
            get => downCursorCommand ?? (downCursorCommand = new DelegateCommand(
                () => {
                    if(repeatCount == 0) {
                        mainFileList.SelectedIndex++;
                    }
                    else {
                        repeatCommand(() => mainFileList.SelectedIndex++);
                    }
                }
            ));
        }

        private DelegateCommand upCursorCommand;
        public DelegateCommand UpCursorCommand {
            get => upCursorCommand ?? (upCursorCommand = new DelegateCommand(
                () => {
                    if(repeatCount == 0) {
                        mainFileList.SelectedIndex--;
                    }
                    else {
                        repeatCommand(() => mainFileList.SelectedIndex--);
                    }
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

        private DelegateCommand<object> focusCommand;
        public DelegateCommand<object> FocusCommand { 
            get => focusCommand ?? (focusCommand = new DelegateCommand<object>(
                (object param) => {
                    var view = (System.Windows.Controls.Control)param;
                    view.Focus();
                }
            ));
        }

        private DelegateCommand<object> setRepeatCountCommand;
        public DelegateCommand<object> SetRepeatCountCommand {
            get => setRepeatCountCommand ?? (setRepeatCountCommand = new DelegateCommand<object>(
                (object param) => {

                    string numberString = param.ToString().Substring(1);
                    // パラメーターに入ってくる文字列は "d0" から "d9" までの10種類。
                    // dxの数字部分が、数字キーと対応している。

                    if(repeatCount == 0) {
                        repeatCount = int.Parse(numberString);
                    }
                    else {
                        string stringNumbers = repeatCount.ToString() + numberString;
                        repeatCount = int.Parse(stringNumbers);
                    }

                    if(repeatCount > 100) {
                        repeatCount = 100;
                    }

                }
            ));
        }

        /// <summary>
        /// repeatCount の回数だけ action を実行し、実行後に repeatCount を 0 にセットします
        /// </summary>
        /// <param name="action"></param>
        private void repeatCommand(Action action) {
            for(int i = 0; i < repeatCount; i++) {
                action();
            }

            repeatCount = 0;
        }

    }
}
