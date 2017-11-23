using Microsoft.Toolkit.Uwp.Helpers;
using Windows.UI.Xaml.Controls;

namespace XamlBrewer.Uwp.UserPromptsSample.Services.Activation.RateAndReview
{
    public sealed partial class RateAndReviewDialog : ContentDialog
    {
        public RateAndReviewDialog()
        {
            this.InitializeComponent();
        }

        private async void Hyperlink_Click(Windows.UI.Xaml.Documents.Hyperlink sender, Windows.UI.Xaml.Documents.HyperlinkClickEventArgs args)
        {
            await SystemInformation.LaunchStoreForReviewAsync();
        }
    }
}
