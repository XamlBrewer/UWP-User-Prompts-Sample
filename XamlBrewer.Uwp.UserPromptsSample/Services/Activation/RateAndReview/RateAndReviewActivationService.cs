using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml.Controls;

namespace XamlBrewer.Uwp.UserPromptsSample.Services.Activation.RateAndReview
{
    public static class RateAndReviewActivationService
    {
        private const string HasShown = "RateAndReviewActivation_HasShown";
        private static readonly LocalObjectStorageHelper _localSettings = new LocalObjectStorageHelper();

        internal static async Task PreLaunchAsync(LaunchActivatedEventArgs e)
        {
            // Start tracking app usage (launch count, uptime, ...)
            try
            {
                // Make the AppUptime cumulative over sessions.

                // v1.0 
                // Does not work because AppUptime returns TimeSpan.MinValue too often.
                // var uptimeSoFar = SystemInformation.AppUptime;
                // if (uptimeSoFar == TimeSpan.MinValue)
                // {
                //     uptimeSoFar = TimeSpan.FromSeconds(0);
                // }

                // v2.0
                var uptimeSoFar = TimeSpan.FromTicks(new LocalObjectStorageHelper().Read<long>("AppUptime", 0));

                SystemInformation.TrackAppUse(e); // Resets AppUptime to 0.
                SystemInformation.AddToAppUptime(uptimeSoFar);
            }
            catch (Exception)
            {
                SystemInformation.TrackAppUse(e);
            }

            await Task.CompletedTask;
        }

        internal static async Task ShowIfAppropriateAsync(LaunchActivatedEventArgs e)
        {
            bool hasShownRateAndReview = _localSettings.Read(HasShown, false);

            if (hasShownRateAndReview)
            {
                return;
            }

            if (SystemInformation.LaunchCount > 5)
            {
                var dialog = new RateAndReviewDialog();
                var response = await dialog.ShowAsync();
                if (response == ContentDialogResult.Secondary)
                {
                    Deactivate();
                }
            }

            await Task.CompletedTask;
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
