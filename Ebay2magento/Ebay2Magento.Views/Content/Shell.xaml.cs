using System.Windows;
using System.Windows.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Ebay2Magento.Views.Content
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class Shell : Page
	{
		public Shell()
		{
			this.InitializeComponent();
		}

		private void HamburgerButton_Click(object sender, RoutedEventArgs e)
		{
			//MainMenu.IsPaneOpen = !MainMenu.IsPaneOpen;
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			//Container.Navigate(typeof(Settings));
		}
	}
}
