using Ebay2magento.ApplicationFramework.Entities;
using Ebay2Magento.Presentation;
using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls;
using System.Windows.Controls;

namespace Ebay2Magento.Views.Content
{
	/// <summary>
	/// Interaction logic for AboutView.xaml
	/// </summary>
	public partial class SettingsView : UserControl
	{
		public SettingsView()
		{
			InitializeComponent();

			Loaded += SettingsView_Loaded;
			Messenger.Default.Register<EbayNotificationMessage>(this, NotificationMessageReceived);
		}

		private void SettingsView_Loaded(object sender, System.Windows.RoutedEventArgs e)
		{
			(this.DataContext as ViewModelBase)?.OnLoaded();
		}

		private void NotificationMessageReceived(EbayNotificationMessage msg)
		{
			if (msg.Message == "ShowWebView")
			{
				var webview = new WebView();

				webview.Closed += (s, e) =>
				{
					msg.Callback();
				};

				(webview as MetroWindow)?.Show();

				webview.Navigate(msg.Url);
			}
		}
	}
}
