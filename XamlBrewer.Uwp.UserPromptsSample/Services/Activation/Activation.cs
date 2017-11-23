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
using XamlBrewer.Uwp.UserPromptsSample.Services.Activation.RateAndReview;

namespace Mvvm.Services
{
    public class Activation
    {
        public async Task LaunchAsync(LaunchActivatedEventArgs e)
        {
            // Emergency procedure to clear all Local Settings.
            // var containerSettings = (ApplicationDataContainerSettings)ApplicationData.Current.LocalSettings.Values;
            // var keys = containerSettings.Keys;
            // foreach (var key in keys)
            // {
            //     Debug.WriteLine(key);
            //     ApplicationData.Current.LocalSettings.Values.Remove(key);
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
            Theme.ApplyToContainer(); // await ThemingService.PreLaunchAsync(e);

            await FirstUseActivationService.PreLaunchAsync(e);
            await NewReleaseActivationService.PreLaunchAsync(e);
            await TrialToPurchaseActivationService.PreLaunchAsync(e);
            await RateAndReviewActivationService.PreLaunchAsync(e);
        }

        /// <summary>
        /// Application Services after the launch.
        /// </summary>
        /// <returns></returns>
        private async Task PostLaunchAsync(LaunchActivatedEventArgs e)
        {
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true; // await ThemingService.PostLaunchAsync(e);

            await FirstUseActivationService.ShowIfAppropriateAsync(e);
            await NewReleaseActivationService.ShowIfAppropriateAsync(e);
            await TrialToPurchaseActivationService.ShowIfAppropriateAsync(e);
            await RateAndReviewActivationService.ShowIfAppropriateAsync(e);
        }
    }
}
