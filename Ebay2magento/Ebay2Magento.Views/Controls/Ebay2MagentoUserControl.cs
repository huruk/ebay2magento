using Ebay2Magento.Presentation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace Ebay2Magento.Views.Controls
{
	public class Ebay2MagentoUserControl : UserControl
	{
		public Ebay2MagentoUserControl()
		{
			Loaded += OnLoaded;
		}

		private void OnLoaded(object sender, System.Windows.RoutedEventArgs e)
		{
			(this.DataContext as ViewModelBase)?.Loaded();
		}
	}
}
