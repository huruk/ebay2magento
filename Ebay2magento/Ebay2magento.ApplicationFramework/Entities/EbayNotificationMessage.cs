using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ebay2magento.ApplicationFramework.Entities
{
	public class EbayNotificationMessage : MessageBase
	{
		public Action Callback { get; set; }

		public string Message { get; set; }

		public Uri Url { get; set; }

		public EbayNotificationMessage(string message, Uri url, Action callback) : base()
		{
			Message = message;
			Url = url;
			Callback = callback;
		}
	}
}
