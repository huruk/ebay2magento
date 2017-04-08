using Microsoft.Practices.Unity;
using System.Windows;

namespace Ebay2Magento.Classic
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		private UnityContainer _container;

		public App()
		{
			_container = new UnityContainer();
			Module.Bootstrap(_container);
		}
	}
}
