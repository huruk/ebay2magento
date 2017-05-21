using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace Ebay2Magento.Presentation
{
	public class ViewModelLocator
	{
		public ViewModelLocator()
		{
			ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

			SimpleIoc.Default.Register<SettingsViewModel>();
			SimpleIoc.Default.Register<CategoryViewModel>();
			SimpleIoc.Default.Register<ProductsViewModel>();
		}

		public SettingsViewModel Settings => ServiceLocator.Current.GetInstance<SettingsViewModel>();

		public CategoryViewModel Categories => ServiceLocator.Current.GetInstance<CategoryViewModel>();

		public ProductsViewModel Products => ServiceLocator.Current.GetInstance<ProductsViewModel>();
	}
}
