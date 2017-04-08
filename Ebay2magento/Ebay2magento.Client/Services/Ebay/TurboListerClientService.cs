using Ebay2magento.Client.Contracts.Ebay;
using Ebay2magento.Client.Entities;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ebay2magento.Client.Services.Ebay
{
	class TurboListerClientService : ITurboListerClientService
	{
		//public async Task ReadFile(CancellationToken ct, StorageFile file)
		//{
		//	var text = await FileIO.ReadTextAsync(file);

		//	using (var textReader = new StringReader(text))
		//	{
		//		var csv = new CsvReader(textReader);
		//		var data = csv.GetRecords<EbayItemData>().ToList();
		//	}
		//}
	}
}
