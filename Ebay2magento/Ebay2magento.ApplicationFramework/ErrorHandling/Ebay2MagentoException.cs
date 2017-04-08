using System;
using System.Collections.Generic;
using System.Text;

namespace Ebay2magento.ApplicationFramework.ErrorHandling
{
	public class Ebay2MagentoException : Exception
	{
		public Ebay2MagentoException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
