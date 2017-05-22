using eBay.Service.Core.Soap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ebay2Magento.Business.Extensions
{
	public static class ItemTypeExtensions
	{
		public static StoreCustomCategoryType FindItemCategory(this ItemType item, StoreCustomCategoryType[] categories)
		{
			var categoryID = item.Storefront.StoreCategoryID;
			return FindCategoryById(categoryID, categories);
		}

		private static StoreCustomCategoryType FindCategoryById(long id, StoreCustomCategoryType[] categories)
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

		private static StoreCustomCategoryType TryGetcategory(long id, StoreCustomCategoryType target)
		{
			if (target.CategoryID == id)
			{
				return target;
			}

			if (target.ChildCategory != null)
			{
				var children = target.ChildCategory.ToArray();
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
