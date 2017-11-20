using Microsoft.Toolkit.Uwp.Helpers;
using VNext = Microsoft.Toolkit.Uwp.Helpers.VNext;
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Microsoft.Toolkit.Uwp.Helpers.VNext;

namespace XamlBrewer.Uwp.UserPromptsSample.Services.Activation.NewRelease
{
    public static class NewReleaseActivationService
    {
        private const string HasShown = "NewReleaseActivation_HasShown";
        private static readonly LocalObjectStorageHelper _localSettings = new LocalObjectStorageHelper();

        internal static async Task ShowIfAppropriateAsync()
        {
            var currentVersion = VNext.SystemInformation.ApplicationVersion;

            if (currentVersion.ToFormattedString() == VNext.SystemInformation.FirstVersionInstalled.ToFormattedString())
            {
                // Original version. Ignore.
                return;
            }

            var hasShown = _localSettings.Read(HasShown, false);

            if (!hasShown)
            {
                // New release dialog.
                var dialog = new NewReleaseDialog();
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
