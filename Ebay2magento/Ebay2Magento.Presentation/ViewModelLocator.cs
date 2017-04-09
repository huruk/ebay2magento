using System;
using System.Collections.Generic;
using System.Text;
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
		}

		public SettingsViewModel Settings => ServiceLocator.Current.GetInstance<SettingsViewModel>();
	}
}
