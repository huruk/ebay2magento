using Ebay2magento.ApplicationFramework.Entities;
using Ebay2Magento.Presentation;
using Ebay2Magento.Views.Controls;
using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls;
using System.Windows.Controls;

namespace Ebay2Magento.Views.Content
{
	/// <summary>
	/// Interaction logic for AboutView.xaml
	/// </summary>
	public partial class SettingsView : Ebay2MagentoUserControl
	{
		public SettingsView()
		{
			InitializeComponent();
			Messenger.Default.Register<EbayNotificationMessage>(this, NotificationMessageReceived);
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
