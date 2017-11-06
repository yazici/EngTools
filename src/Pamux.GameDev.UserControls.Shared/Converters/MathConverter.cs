using NCalc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;


// https://github.com/sheetsync/NCalc
namespace Pamux.GameDev.UserControls.Converters
{
    public class MathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Parse value into equation and remove spaces
            var expressionString = parameter as string;
            if (string.IsNullOrWhiteSpace(expressionString))
            {
                return value;
            }
            expressionString = expressionString.Replace(" ", "");
            expressionString = expressionString.Replace("@VALUE", value.ToString());

            return new Expression(expressionString).Evaluate();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }    
}
