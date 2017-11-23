using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.UI.Xaml;
using XamlBrewer.Uwp.UserPromptsSample;
using XamlBrewer.Uwp.UserPromptsSample.Services.Activation.FirstUse;
using XamlBrewer.Uwp.UserPromptsSample.Services.Activation.NewRelease;
using Microsoft.Toolkit.Uwp.Helpers;
using Windows.Storage;
using System.Diagnostics;
using System;
using XamlBrewer.Uwp.UserPromptsSample.Services.Activation.TrialToPurchase;

namespace Mvvm.Services
{
    public class Activation
    {
        public async Task LaunchAsync(LaunchActivatedEventArgs e)
        {
            // Emergency procedure: clear all Local Settings
            // var containerSettings = (ApplicationDataContainerSettings)ApplicationData.Current.LocalSettings.Values;
            // var keys = containerSettings.Keys;
            // foreach (var key in keys)
            // {
            // Debug.WriteLine(key);
            // ApplicationData.Current.LocalSettings.Values.Remove(key);
            // }

            // Custom pre-launch service calls.
            await PreLaunchAsync(e);

            // Navigate
            Window.Current.EnsureRootFrame().NavigateIfAppropriate(typeof(Shell), e.Arguments).Activate();

            // Custom post-launch service calls.
            await PostLaunchAsync(e);
        }

        /// <summary>
        /// Application Services before the launch.
        /// </summary>
        private async Task PreLaunchAsync(LaunchActivatedEventArgs e)
        {
            Theme.ApplyToContainer();

            await FirstUseActivationService.PreLaunchAsync(e);
            await NewReleaseActivationService.PreLaunchAsync(e);
            await TrialToPurchaseActivationService.PreLaunchAsync(e);

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

        /// <summary>
        /// Application Services after the launch.
        /// </summary>
        /// <returns></returns>
        private async Task PostLaunchAsync(LaunchActivatedEventArgs e)
        {
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;

            await FirstUseActivationService.ShowIfAppropriateAsync(e);
            await NewReleaseActivationService.ShowIfAppropriateAsync(e);
            await TrialToPurchaseActivationService.ShowIfAppropriateAsync(e);

            await Task.CompletedTask;
        }
    }
}
