using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.UI.Xaml;
using XamlBrewer.Uwp.UserPromptsSample;
using XamlBrewer.Uwp.UserPromptsSample.Services.Activation.FirstUse;
using XamlBrewer.Uwp.UserPromptsSample.Services.Activation.NewRelease;
using VNext = Microsoft.Toolkit.Uwp.Helpers.VNext;

namespace Mvvm.Services
{
    public class Activation
    {
        public async Task LaunchAsync(LaunchActivatedEventArgs e)
        {
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

            if (VNext.SystemInformation.IsFirstRun)
            {
                // First-time app initialization.
                // ...
                FirstUseActivationService.Reset();
            }

            if (VNext.SystemInformation.IsAppUpdated)
            {
                // New release app initialization.
                // ...
                NewReleaseActivationService.Reset();
            }

            // Start tracking app usage (launch count, uptime, ...)
            VNext.SystemInformation.TrackAppUse(e);

            await Task.CompletedTask;
        }

        /// <summary>
        /// Application Services after the launch.
        /// </summary>
        /// <returns></returns>
        private async Task PostLaunchAsync(LaunchActivatedEventArgs e)
        {
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;

            await FirstUseActivationService.ShowIfAppropriateAsync();
            await NewReleaseActivationService.ShowIfAppropriateAsync();

            await Task.CompletedTask;
        }
    }
}
