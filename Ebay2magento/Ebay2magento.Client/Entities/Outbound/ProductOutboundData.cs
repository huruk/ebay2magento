using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ebay2magento.Client.Entities.Outbound
{
	public class ProductOutboundData
	{
		[JsonProperty("sku")]
		public string SKU { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("price")]
		public double Price { get; set; }

		[JsonProperty("attribute_set_id")]
		public int AttributeSetId { get; set; }

		[JsonProperty("custom_attributes")]
		public CustomAttributes[] CustomAttributes { get; set; }

		[JsonProperty("extension_attributes")]
		public ExtensionAttributes ExtensionAttributes { get; set; }
	}

	public class CustomAttributes
	{
		[JsonProperty("attribute_code")]
		public string AttributeCode { get; set; }

		[JsonProperty("value")]
		public string Value { get; set; }
	}
	public class ExtensionAttributes
	{
		[JsonProperty("stock_item")]
		public StockItem StockItem { get; set; }
	}

	public class StockItem
	{
		[JsonProperty("qty")]
		public int Qty { get; set; }

		[JsonProperty("is_in_stock")]
		public bool IsInStock { get; set; }
	}
}
