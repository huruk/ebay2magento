using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ebay2magento.Client.Entities.Outbound
{
	public class CategoryOutboundData
	{
		[JsonProperty("parent_id")]
		public int ParentId { get; set; }

		public string Name { get; set; }

		[JsonProperty("is_active")]
		public bool IsActive { get; set; }

		[JsonProperty("include_in_menu")]
		public bool IncludeInMenu { get; set; }
	}
}
