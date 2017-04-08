using Ebay2Magento.Presentation;
using System.Windows;
using System.Windows.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Ebay2Magento.Views.Content
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class Settings : Page
	{
		public Settings()
		{
			this.InitializeComponent();

			this.Loaded += OnLoaded;
		}

		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			(this.DataContext as ViewModelBase)?.OnLoaded();
		}
	}
}
