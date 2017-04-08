using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Ebay2Magento.Views.Converters
{
	public class BoolToVisibilityConverter : IValueConverter
	{
		public Visibility VisibilityIfTrue { get; set; }

		public Visibility VisibilityIfFalse { get; set; }

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return (bool)value ? VisibilityIfTrue : VisibilityIfFalse;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
