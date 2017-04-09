using ApplicationFramework;
using Ebay2Magento.ApplicationFramework.Contracts;
using Ebay2Magento.Business.Contracts;
using Ebay2Magento.Client.Entities;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ebay2Magento.Presentation
{
	public class SettingsViewModel : ViewModelBase
	{
		private Func<ISettingsService> _settingsService;
		private Func<IEbayService> _ebayService;
		private Func<IMagentoService> _magentoService;

		public string AppID
		{
			get { return GetValue(() => AppID); }
			set { SetValue(() => AppID, value); }
		}

		public string CertID
		{
			get { return GetValue(() => CertID); }
			set { SetValue(() => CertID, value); }
		}

		public string RuName
		{
			get { return GetValue(() => RuName); }
			set { SetValue(() => RuName, value); }
		}

		public string DevID
		{
			get { return GetValue(() => DevID); }
			set { SetValue(() => DevID, value); }
		}

		public UserTokenData UserToken
		{
			get { return GetValue(() => UserToken); }
			set { SetValue(() => UserToken, value); }
		}

		public ApplicationTokenData ApplicationToken
		{
			get { return GetValue(() => ApplicationToken); }
			set { SetValue(() => ApplicationToken, value); }
		}

		public bool IsCallbackServerRunning
		{
			get { return GetValue(() => IsCallbackServerRunning); }
			set { SetValue(() => IsCallbackServerRunning, value); }
		}

		public SettingsViewModel()
		{
			_settingsService = Resolve<ISettingsService>;
			_ebayService = Resolve<IEbayService>;
			_magentoService = Resolve<IMagentoService>;
		}

		public override void OnLoaded()
		{
			AppID = _settingsService().GetValue<string>(Constants.Settings.AppID);
			CertID = _settingsService().GetValue<string>(Constants.Settings.CertID);
			RuName = _settingsService().GetValue<string>(Constants.Settings.RuName);
			DevID = _settingsService().GetValue<string>(Constants.Settings.DevID);

			UserToken = _settingsService().GetValue<UserTokenData>(Constants.Settings.UserToken);
			ApplicationToken = _settingsService().GetValue<ApplicationTokenData>(Constants.Settings.ApplicationToken);
		}

		public ICommand Save
		{
			get
			{
				return new RelayCommand(() =>
				{
					SaveSettings();
				});
			}
		}

		public ICommand GetUserToken
		{
			get
			{
				return new RelayCommand(async () =>
				{
					var sessionId = await Task.Run(() => _ebayService().GetSessionId(CancellationToken));
					Messenger.Default.Send(new NotificationMessage("ShowWebView"));
				});
			}
		}

		public ICommand GetApplicationToken
		{
			get
			{
				return new RelayCommand(async () =>
				{
				});
			}
		}

		private void SaveSettings()
		{
			_settingsService().SetValue(Constants.Settings.AppID, AppID);
			_settingsService().SetValue(Constants.Settings.CertID, CertID);
			_settingsService().SetValue(Constants.Settings.RuName, RuName);
			_settingsService().SetValue(Constants.Settings.DevID, DevID);
		}
	}
}
