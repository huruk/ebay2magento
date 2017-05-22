using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ebay2magento.ApplicationFramework.Serialization
{
	public class StoreCustomCategoryTypeSerializer : JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			return true;
		}

		public override bool CanWrite => false;

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			return serializer.Deserialize(reader, objectType);
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			throw new NotImplementedException();
		}
	}
}
