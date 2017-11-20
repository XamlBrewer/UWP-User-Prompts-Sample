using Mvvm.Services;
using Windows.UI.Xaml.Controls;
using XamlBrewer.Uwp.UserPromptsSample.Services.Activation.FirstUse;
using System;
using Microsoft.Toolkit.Uwp.Helpers.VNext;
using XamlBrewer.Uwp.UserPromptsSample.Services.Activation.NewRelease;

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
    }
}
