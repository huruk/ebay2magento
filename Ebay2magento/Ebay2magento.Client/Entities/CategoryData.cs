using Newtonsoft.Json;
using System.ComponentModel;

namespace Ebay2magento.Client.Entities
{
	[Bindable(BindableSupport.Yes)]
	public class CategoryData
	{
		[JsonProperty("id")]
		public int Id { get; set; }

		[JsonProperty("parent_id")]
		public int ParentId { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("is_active")]
		public bool IsActive { get; set; }

		[JsonProperty("position")]
		public int Position { get; set; }

		[JsonProperty("level")]
		public int Level { get; set; }

		[JsonProperty("product_count")]
		public int ProductCount { get; set; }

		[JsonProperty("children_data")]
		public CategoryData[] ChildrenData { get; set; }
	}
}
