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
			Messenger.Default.Register<NotificationMessage>(this, NotificationMessageReceived);
		}

		private void SettingsView_Loaded(object sender, System.Windows.RoutedEventArgs e)
		{
			(this.DataContext as ViewModelBase)?.OnLoaded();
		}

		private void NotificationMessageReceived(NotificationMessage msg)
		{
			if (msg.Notification == "ShowWebView")
			{
				var webview = new WebView();
				(webview as MetroWindow)?.Show();
			}
		}
	}
}
