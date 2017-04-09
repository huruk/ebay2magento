using System;
using System.Collections.Generic;
using System.Text;

namespace Ebay2Magento.ApplicationFramework.Contracts
{
	public interface ISettingsService
	{
		bool TryGetValue<T>(string key, out T value);

		void SetValue<T>(string key, T value);

		T GetValue<T>(string key, Func<T> defaultFactory = null);

		void RemoveValue(string key);

		void Save();
	}
}
