namespace WPFFiler.ViewModels
{
    using System;
    using Prism.Commands;
    using Prism.Mvvm;
    using Prism.Services.Dialogs;

    public class InputDialogViewModel : BindableBase, IDialogAware
    {
        private string inputText = string.Empty;
        private DelegateCommand finishCommand;

        public string Title => "inputDialog";

        public string InputText
        {
            get => inputText;
            set => SetProperty(ref inputText, value);
        }

        public event Action<IDialogResult> RequestClose;

        public DelegateCommand FinishCommand
        {
            get => finishCommand ?? (finishCommand = new DelegateCommand(
                () =>
                {
                    var dialogParameters = new DialogParameters();
                    var ret = new DialogResult(ButtonResult.Yes, dialogParameters);
                    dialogParameters.Add("InputText", InputText);
                    this.RequestClose?.Invoke(ret);
                }));
        }

        public bool CanCloseDialog() => true;

        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
        }
    }
}
