using MahApps.Metro.Controls;
using System.Windows;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Ebay2Magento.Views.Content
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class Shell : MetroWindow
	{
		public Shell()
		{
			this.InitializeComponent();
		}

		private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
		{
			HamburgerMenuControl.SelectedIndex = -1;
			HamburgerMenuControl.SelectedOptionsIndex = -1;
		}
	}
}
