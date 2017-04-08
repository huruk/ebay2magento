using Ebay2Magento.ApplicationFramework.Contracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Ebay2Magento.ApplicationFramework.Services
{
	public class SettingsService : ISettingsService
	{
		private Dictionary<string, object> _localSettings;

		private static string Path =
			System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDoc‌​uments), "ebay2magento", "settings.json");

		public SettingsService()
		{
			if (Directory.Exists(Path))
			{
				var settings = File.ReadAllText(Path);
				_localSettings = JsonConvert.DeserializeObject<Dictionary<string, object>>(settings);
			}
			else
			{
				_localSettings = new Dictionary<string, object>();
			}
		}

		public void Save()
		{
			var settings = JsonConvert.SerializeObject(_localSettings);
			File.WriteAllText(Path, settings);
		}

		public bool TryGetValue<T>(string key, out T value)
		{
			if (_localSettings.ContainsKey(key))
			{
				var data = (string)_localSettings[key];
				value = JsonConvert.DeserializeObject<T>(data);
				return true;
			}

			value = default(T);
			return false;
		}

		public T GetValue<T>(string key, Func<T> factory = null)
		{
			if (TryGetValue(key, out T value))
			{
				return value;
			}

			return factory != null ? factory.Invoke() : default(T);
		}

		public void SetValue<T>(string key, T value)
		{
			var serialized = JsonConvert.SerializeObject(value);

			_localSettings[key] = serialized;
		}

		public void RemoveValue(string key)
		{
			if (_localSettings.ContainsKey(key))
			{
				_localSettings.Remove(key);
			}
		}
	}
}
