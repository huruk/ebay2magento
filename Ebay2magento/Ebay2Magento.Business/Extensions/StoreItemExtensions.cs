using Ebay2magento.Client.Entities;
using Ebay2Magento.Presentation.Entities;

namespace Ebay2Magento.Business.Extensions
{
	public static class StoreItemExtensions
	{
		public static CategoryData FindMagentoCategory(this StoreItem item, CategoryData categories)
		{
			var targetName = item.Category;
			return FindCategoryById(targetName, categories.ChildrenData);
		}

		private static CategoryData FindCategoryById(string id, CategoryData[] categories)
		{
			foreach (var category in categories)
			{
				var result = TryGetcategory(id, category);
				if (result == null)
				{
					continue;
				}

				return result;
			}

			return null;
		}

		private static CategoryData TryGetcategory(string id, CategoryData target)
		{
			if (target.Name.Equals(id))
			{
				return target;
			}

			if (target.ChildrenData != null)
			{
				var children = target.ChildrenData;
				foreach (var child in children)
				{
					var innerCategory = TryGetcategory(id, child);
					if (innerCategory == null)
					{
						continue;
					}

					return innerCategory;
				}
			}

			return null;
		}
	}
}
