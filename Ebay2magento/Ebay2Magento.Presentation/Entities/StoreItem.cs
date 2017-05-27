using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Ebay2Magento.Presentation.Entities
{
	[Bindable(true)]
	public class StoreItem
	{
		public string Id { get; set; }

		public string Title { get; set; }

		public int Quantity { get; set; }

		public string SKU { get; set; }

		public string Category { get; set; }

		public double Price { get; set; }

		public string Description { get; set; }
	}
}
