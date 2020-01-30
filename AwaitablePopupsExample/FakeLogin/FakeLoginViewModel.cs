﻿using System;
using System.Threading.Tasks;
using AwaitablePopups.AbstractClasses;
using AwaitablePopups.Interfaces;
using AwaitablePopups.PopupPages.DualResponsePopupPage;
using AwaitablePopups.PopupPages.SingleResponsePopupPage;
using AsyncAwaitBestPractices.MVVM;
using System.Runtime.InteropServices;

namespace AwaitablePopupsExample.FakeLogin
{
    public class FakeLoginViewModel : BaseViewModel
    {

        private string _mobile;
        public string Mobile
        {
            get => _mobile;
            set => SetValue(ref _mobile, value);
        }

        private string _password;
        public string Password
        {
            get => _password;
            set => SetValue(ref _password, value);
        }

        private bool _rememberMe;
        public bool RememberMe
        {
            get => _rememberMe;
            set => SetValue(ref _rememberMe, value);
        }

        private bool _loginEnabled = true;
        public bool LoginEnabled
        {
            get => _loginEnabled;
            set => SetValue(ref _loginEnabled, value);
        }


        public IAsyncCommand LoginAsync => new AsyncCommand(AttemptLogin);

        public FakeLoginViewModel(IPopupService popupService) : base(popupService)
        {
        }

        public async Task<bool> AttemptLogin()
        {
            LoginEnabled = false;
            var loginResult = false;
            try
            {
                if (string.IsNullOrEmpty(Mobile) || string.IsNullOrEmpty(Password))
                {
                    loginResult = await IncorrectLoginAsync();
                }
                else if (Mobile.Equals("Cheese") || Password.Equals("Cheese"))
                {
                    loginResult = await CheeseModeAsync();
                }
                else if (Mobile.Equals("Correct") && Password.Equals("Correct"))
                {
                    loginResult = await SuccessfulLoginAsync();
                }
            }
            catch (Exception)
            {
                loginResult = await GenericErrorAsync();
            }
            finally
            {
                RememberMe = loginResult;
                Mobile = loginResult ? "Returned a true" : "Returned a false";
                LoginEnabled = true;
            }
            return loginResult;
        }



        private async Task<bool> GenericErrorAsync()
        {
            var exceptionCaughtError = new SingleResponseViewModel(PopupService);
            exceptionCaughtError.SingleButtonCommand = new Xamarin.Forms.Command(() => exceptionCaughtError.SafeCloseModal(false));
            exceptionCaughtError.SingleButtonColour = Xamarin.Forms.Color.Coral;
            exceptionCaughtError.SingleButtonText = "Okay";
            exceptionCaughtError.MainPopupInformation = "There was a Device error. Dang.";
            exceptionCaughtError.MainPopupColour = Xamarin.Forms.Color.Gray;
            exceptionCaughtError.SingleDisplayImage = "NoSource.png";
            return await PopupService.PushAsync<SingleResponseViewModel, SingleResponsePopupPage, bool>(exceptionCaughtError);
        }


        private async Task<bool> IncorrectLoginAsync()
        {
            var incorrectLoginError = new SingleResponseViewModel(PopupService);
            incorrectLoginError.SingleButtonCommand = new Xamarin.Forms.Command(() => incorrectLoginError.SafeCloseModal(false));
            incorrectLoginError.SingleButtonColour = Xamarin.Forms.Color.Goldenrod;
            incorrectLoginError.SingleButtonText = "Try Again";
            incorrectLoginError.MainPopupInformation = "Your Phone Number or Pin is incorrect, please try again.";
            incorrectLoginError.MainPopupColour = Xamarin.Forms.Color.Honeydew;
            incorrectLoginError.SingleDisplayImage = "NoSource.png";
            return await PopupService.PushAsync<SingleResponseViewModel, SingleResponsePopupPage, bool>(incorrectLoginError);
        }

        private async Task<bool> SuccessfulLoginAsync()
        {
            var successfulLogin = new SingleResponseViewModel(PopupService);
            successfulLogin.SingleButtonCommand = new Xamarin.Forms.Command(() => successfulLogin.SafeCloseModal(true));
            successfulLogin.SingleButtonColour = Xamarin.Forms.Color.HotPink;
            successfulLogin.SingleButtonText = "I Accept";
            successfulLogin.MainPopupInformation = "Good Job";
            successfulLogin.MainPopupColour = Xamarin.Forms.Color.Gray;
            successfulLogin.SingleDisplayImage = "NoSource.png";
            return await PopupService.PushAsync<SingleResponseViewModel, SingleResponsePopupPage, bool>(successfulLogin);
        }

        private async Task<bool> CheeseModeAsync()
        {
            var EnjoyCheese = new DualResponseViewModel(PopupService);
            EnjoyCheese.RightButtonCommand = new Xamarin.Forms.Command(() => EnjoyCheese.SafeCloseModal(false));
            EnjoyCheese.RightButtonColour = Xamarin.Forms.Color.Gray;
            EnjoyCheese.RightButtonTextColour = Xamarin.Forms.Color.Green;
            EnjoyCheese.RightButtonText = "I dislike cheese";
            EnjoyCheese.LeftButtonCommand = new Xamarin.Forms.Command(() => EnjoyCheese.SafeCloseModal(true));
            EnjoyCheese.LeftButtonColour = Xamarin.Forms.Color.Gray;
            EnjoyCheese.LeftButtonTextColour = Xamarin.Forms.Color.GreenYellow;
            EnjoyCheese.LeftButtonText = "Yes please";
            EnjoyCheese.MainPopupInformation = "Do you wish to enter cheese mode?";
            EnjoyCheese.MainPopupColour = Xamarin.Forms.Color.Lavender;
            return await PopupService.PushAsync<DualResponseViewModel, DualResponsePopupPage, bool>(EnjoyCheese);
        }
    }
}