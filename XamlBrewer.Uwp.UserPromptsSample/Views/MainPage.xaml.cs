using Windows.UI.Xaml.Controls;
using XamlBrewer.Uwp.UserPromptsSample.Services.Activation.FirstUse;
using System;
using Microsoft.Toolkit.Uwp.Helpers;
using XamlBrewer.Uwp.UserPromptsSample.Services.Activation.NewRelease;
using XamlBrewer.Uwp.UserPromptsSample.Services.Activation.TrialToPurchase;
using XamlBrewer.Uwp.UserPromptsSample.Services.Activation.RateAndReview;

namespace XamlBrewer.Uwp.UserPromptsSample
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        public string ApplicationVersion => SystemInformation.ApplicationVersion.ToFormattedString();

        public long LaunchCount => SystemInformation.LaunchCount;

        public object Uptime => string.Format("{0} minutes", SystemInformation.AppUptime.Minutes);

        private async void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await new FirstUseDialog().ShowAsync();
        }

        private void Button_Click_1(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            FirstUseActivationService.Reset();
        }

        private async void Button_Click_3(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await new NewReleaseDialog().ShowAsync();
        }

        private void Button_Click_4(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            NewReleaseActivationService.Reset();
        }

        private async void Button_Click_5(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await new TrialToPurchaseDialog().ShowAsync();
        }

        private void Button_Click_6(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            TrialToPurchaseActivationService.Reset();
        }

        private async void Button_Click_7(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            try
            {
                await TrialToPurchaseActivationService.SimulatePurchase();

            }
            catch (Exception)
            {
                // Purchase failed.
            }

        }

        private async void Button_Click_8(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await new RateAndReviewDialog().ShowAsync();
        }

        private void Button_Click_9(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            RateAndReviewActivationService.Reset();
        }
    }
}
