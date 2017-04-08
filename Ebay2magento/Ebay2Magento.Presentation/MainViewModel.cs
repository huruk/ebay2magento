using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using ApplicationFramework;
using Ebay2Magento.ApplicationFramework.Contracts;
using Ebay2Magento.Business.Contracts;
using Ebay2Magento.Client.Entities;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Threading;

namespace Ebay2Magento.Presentation
{
	public class MainViewModel : ViewModelBase
	{
		private IEbayService _ebayService;
		private ISettingsService _settingsService;

		public ObservableCollection<int> EbayItems
		{
			get { return GetValue(() => EbayItems); }
			set { SetValue(() => EbayItems, value); }
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

		public MainViewModel()
		{
			_ebayService = Resolve<IEbayService>();
			_settingsService = Resolve<ISettingsService>();

			UserToken = _settingsService.GetValue<UserTokenData>(Constants.Settings.UserToken);
			ApplicationToken = _settingsService.GetValue<ApplicationTokenData>(Constants.Settings.ApplicationToken);
		}

		public ICommand FetchUserToken
		{
			get
			{
				return new RelayCommand(async () =>
				{
					using (var tokenSource = new CancellationTokenSource())
					{
						var token = await Task.Run(() => _ebayService.GetUserToken(tokenSource.Token));
						UserToken = token;
					}
				});
			}
		}

		public ICommand FetchApplicationToken
		{
			get
			{
				return new RelayCommand(async () =>
				{
					using (var tokenSource = new CancellationTokenSource())
					{
						var token = await Task.Run(() => _ebayService.GetApplicationToken(tokenSource.Token));
						ApplicationToken = token;
					}
				});
			}
		}
	}
}
