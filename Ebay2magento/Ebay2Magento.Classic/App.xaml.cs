using ApplicationFramework.Extensions;
using Ebay2Magento.ApplicationFramework.Contracts;
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

		protected override void OnExit(ExitEventArgs e)
		{
			var settingsService = _container.ResolveLazy<ISettingsService>();
			settingsService.Save();
			base.OnExit(e);
		}
	}
}
