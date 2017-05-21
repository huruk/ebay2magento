using ApplicationFramework;
using Ebay2magento.ApplicationFramework.Entities;
using Ebay2Magento.ApplicationFramework.Contracts;
using Ebay2Magento.Business.Contracts;
using Ebay2Magento.Client.Entities;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Reactive.Linq;
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
			set { _settingsService().SetValue(Constants.Settings.AppID, value); SetValue(() => AppID, value); }
		}

		public string CertID
		{
			get { return GetValue(() => CertID); }
			set { _settingsService().SetValue(Constants.Settings.CertID, value); SetValue(() => CertID, value); }
		}

		public string RuName
		{
			get { return GetValue(() => RuName); }
			set { _settingsService().SetValue(Constants.Settings.RuName, value); SetValue(() => RuName, value); }
		}

		public string DevID
		{
			get { return GetValue(() => DevID); }
			set { _settingsService().SetValue(Constants.Settings.DevID, value); SetValue(() => DevID, value); }
		}

		public UserTokenData UserToken
		{
			get { return GetValue(() => UserToken); }
			set { SetValue(() => UserToken, value); }
		}

		public string MagentoUsername
		{
			get { return GetValue(() => MagentoUsername); }
			set { SetValue(() => MagentoUsername, value); }
		}

		public string MagentoPassword
		{
			get { return GetValue(() => MagentoPassword); }
			set { SetValue(() => MagentoPassword, value); }
		}

		public string MagentoUrl
		{
			get { return GetValue(() => MagentoUrl); }
			set { _settingsService().SetValue(Constants.Settings.MagentoUrl, value); SetValue(() => MagentoUrl, value); }
		}

		public string MagentoToken
		{
			get { return GetValue(() => MagentoToken); }
			set { _settingsService().SetValue(Constants.Settings.MagentoToken, value); SetValue(() => MagentoToken, value); }
		}

		public SettingsViewModel()
		{
			_settingsService = Resolve<ISettingsService>;
			_ebayService = Resolve<IEbayService>;
			_magentoService = Resolve<IMagentoService>;
		}

		public override async Task OnLoaded()
		{
			AppID = _settingsService().GetValue<string>(Constants.Settings.AppID);
			CertID = _settingsService().GetValue<string>(Constants.Settings.CertID);
			RuName = _settingsService().GetValue<string>(Constants.Settings.RuName);
			DevID = _settingsService().GetValue<string>(Constants.Settings.DevID);
			UserToken = _settingsService().GetValue<UserTokenData>(Constants.Settings.UserToken);
			MagentoToken = _settingsService().GetValue<string>(Constants.Settings.MagentoToken);
			MagentoUrl = _settingsService().GetValue<string>(Constants.Settings.MagentoUrl);
		}

		public ICommand GetEbayToken
		{
			get
			{
				return new RelayCommand(async () =>
				{
					var sessionId = await Task.Run(() => _ebayService().GetSessionId(CancellationToken));
					Messenger.Default.Send(new EbayNotificationMessage(
						"ShowWebView",
						new Uri(Constants.Ebay.SignInUrl + RuName + "&SessID=" + Uri.EscapeDataString(sessionId)),
						async () =>
						{
							var userToken = await Task.Run(() => _ebayService().GetUserToken(CancellationToken, sessionId));
							UserToken = userToken;
						}
					));
				},
				() => UserToken == null);
			}
		}

		public ICommand GetMagentoToken
		{
			get
			{
				return new RelayCommand(async () =>
				{
					var magentoToken = await Task.Run(() => _magentoService().GetBearerToken(
						CancellationToken,
						MagentoUrl,
						MagentoUsername,
						MagentoPassword
					));

					MagentoToken = magentoToken;
				},
				() => MagentoUsername != null && MagentoPassword != null);
			}
		}
	}
}
