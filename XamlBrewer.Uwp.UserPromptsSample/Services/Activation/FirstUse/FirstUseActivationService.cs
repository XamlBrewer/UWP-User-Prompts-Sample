using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml.Controls;

namespace XamlBrewer.Uwp.UserPromptsSample.Services.Activation.FirstUse
{
    public static class FirstUseActivationService
    {
        private const string HasShown = "FirstUseActivation_HasShown";
        private static readonly LocalObjectStorageHelper _localSettings = new LocalObjectStorageHelper();

        internal static async Task PreLaunchAsync(LaunchActivatedEventArgs e)
        {
            if (SystemInformation.IsFirstRun)
            {
                // First-time app initialization.
                // ...
                Reset();
            }

            await Task.CompletedTask;
        }

        internal static async Task ShowIfAppropriateAsync(LaunchActivatedEventArgs e)
        {
            bool hasShownFirstRun = _localSettings.Read(HasShown, false);

            if (!hasShownFirstRun)
            {
                var dialog = new FirstUseDialog();
                var response = await dialog.ShowAsync();
                if (response == ContentDialogResult.Secondary)
                {
                    Deactivate();
                }
            }
        }

        internal static void Reset()
        {
            _localSettings.Save(HasShown, false);
        }

        internal static void Deactivate()
        {
            _localSettings.Save(HasShown, true);
        }
    }
}
