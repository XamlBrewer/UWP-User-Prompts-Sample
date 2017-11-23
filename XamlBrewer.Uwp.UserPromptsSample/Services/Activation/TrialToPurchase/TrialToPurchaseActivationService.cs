using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Store;
using Windows.UI.Xaml.Controls;

namespace XamlBrewer.Uwp.UserPromptsSample.Services.Activation.TrialToPurchase
{
    // Based on Windows.Application model, not Windows.Services.Store.
    // Check https://docs.microsoft.com/en-us/windows/uwp/monetize/in-app-purchases-and-trials.
    public static class TrialToPurchaseActivationService
    {
        private const string HasShown = "TrialToPurchaseActivation_HasShown";
        private static readonly LocalObjectStorageHelper _localSettings = new LocalObjectStorageHelper();
        private static LicenseInformation _licenseInformation;

        static TrialToPurchaseActivationService()
        {
            // Uncomment the following line in the release version of your app.
            // _licenseInformation = CurrentApp.LicenseInformation;

            // Initialize the license info for testing.
            // Comment the following line in the release version of your app.
            _licenseInformation = CurrentAppSimulator.LicenseInformation;
        }

        internal static async Task PreLaunchAsync(LaunchActivatedEventArgs e)
        {
            if (_licenseInformation.IsTrial)
            {
                // Trial: set HasShown to false to trigger detection.
                Reset();
                _licenseInformation.LicenseChanged += LicenseInformation_LicenseChanged;
            }
            else
            {
                // Purchased: set HasShown to true to inhibit detecting, but only if we were not detecting already.
                var hasShown = _localSettings.Read(HasShown, true);
                if (hasShown)
                {
                    Deactivate();
                }
            }

            await Task.CompletedTask;
        }

        internal static async Task ShowIfAppropriateAsync(LaunchActivatedEventArgs e)
        {
            if (_licenseInformation.IsTrial)
            {
                return;
            }

            var hasShown = _localSettings.Read(HasShown, true);

            if (!hasShown)
            {
                // New trial-to-purchase dialog.
                var dialog = new TrialToPurchaseDialog();
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

        internal async static Task SimulatePurchase()
        {
            try
            {
                var result = await CurrentAppSimulator.RequestAppPurchaseAsync(false);

                // Purchased.

            }
            catch (Exception)
            {
                // Purchase failed.
            }

            await Task.CompletedTask;
        }

        private async static void LicenseInformation_LicenseChanged()
        {
            await ShowIfAppropriateAsync(null);
        }
    }
}
