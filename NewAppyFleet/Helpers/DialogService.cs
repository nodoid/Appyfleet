using System;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Views;
using Xamarin.Forms;

namespace NewAppyFleet
{
    public class DialogService : IDialogService
    {
        Page _dialogPage;

        public void Initialize(Page dialogPage)
        {
            _dialogPage = dialogPage;
        }

        public async Task ShowError(string message, string title, string buttonText, Action afterHideCallback)
        {
            await Task.Factory.StartNew(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                            {
                                await _dialogPage.DisplayAlert(title, message, buttonText);

                                if (afterHideCallback != null)
                                {
                                    afterHideCallback();
                                }
                            });
            });
        }

        public async Task ShowError(Exception error, string title, string buttonText, Action afterHideCallback)
        {
            await Task.Factory.StartNew(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await _dialogPage.DisplayAlert(title, error.Message, buttonText);

                    if (afterHideCallback != null)
                    {
                        afterHideCallback();
                    }
                });
            });
        }

        public async Task ShowMessage(string message, string title)
        {
            await Task.Factory.StartNew(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await _dialogPage.DisplayAlert(title, message, "OK");
                });
            });
        }

        public async Task ShowMessage(string message, string title, string buttonText, Action afterHideCallback)
        {
            await Task.Factory.StartNew(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await _dialogPage.DisplayAlert(title, message, buttonText);

                    if (afterHideCallback != null)
                    {
                        afterHideCallback();
                    }
                });
            });
        }

        public async Task<bool> ShowMessage(string message, string title, string buttonConfirmText, string buttonCancelText, Action<bool> afterHideCallback)
        {
            int changed = 0;
            bool result = false;

            await Task.Factory.StartNew(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    result = await _dialogPage.DisplayAlert(title, message, buttonConfirmText, buttonCancelText);

                    if (afterHideCallback != null)
                    {
                        afterHideCallback(result);
                    }

                    changed = -1;
                });
            });
            while (changed == 0)
            { }
            return result;
        }

        public async Task ShowMessageBox(string message, string title)
        {
            await Task.Factory.StartNew(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await _dialogPage.DisplayAlert(title, message, "OK");
                });
            });
        }
    }
}
