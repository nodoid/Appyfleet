using System;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using mvvmframework.Languages;
using GalaSoft.MvvmLight.Messaging;

namespace mvvmframework.ViewModels
{
    public class ForgottenPasswordViewModel : BaseViewModel
    {
        IWebSevices webService;
        IConnection connectService;
        IDialogService diaService;

        public ForgottenPasswordViewModel(IWebSevices web, IConnection connect, IDialogService dia)
        {
            webService = web;
            connectService = connect;
            diaService = dia;
            CanSubmit = false;
        }

        string emailAddress;
        public string EmailAddress
        {
            get => emailAddress;
            set 
            {
                if (value.IsValidEmailAddress())
                {
                    Set(() => EmailAddress, ref emailAddress, value, true);
                    CheckSubmit();
                }
            }
        }

        string yob;
        public string YOB
        {
            get => yob;
            set 
            {
                if (value.Length == 4)
                {
                    Set(() => YOB, ref yob, value, true);
                    CheckSubmit();
                }
            }
        }

        bool done;
        public bool Done
        {
            get => done;
            set { Set(() => Done, ref done, value, true); }
        }

        bool canSubmit;
        public bool CanSubmit
        {
            get => canSubmit;
            set { Set(() => CanSubmit, ref canSubmit, value, true); }
        }

        void CheckSubmit()
        {
            if (!string.IsNullOrEmpty(YOB) && !string.IsNullOrEmpty(EmailAddress))
                CanSubmit = true;
        }

        RelayCommand resetPasswordCommand;
        public RelayCommand ResetPasswordCommand
        {
            get
            {
                return resetPasswordCommand ??
                    (
                        resetPasswordCommand = new RelayCommand(async()=>
                {
                    if (connectService.IsConnected)
                    {
                        IsBusy = true;
                        await webService.ForgottenPasswordRequest(EmailAddress, YOB).ContinueWith((t) =>
                        {
                            if (t.IsCompleted)
                            {
                                IsBusy = false;
                                if (!t.IsFaulted && !t.IsCanceled)
                                {
                                    diaService.ShowMessage(Langs.Const_Msg_Password_Send, "");
                                    Done = true;
                                }
                                else
                                {
                                    Messenger.Default.Send(new NotificationMessage(t.Result.Status.Message));
                                }
                            }
                        });
                    }
                })
                    );
            }
        }
    }
}
