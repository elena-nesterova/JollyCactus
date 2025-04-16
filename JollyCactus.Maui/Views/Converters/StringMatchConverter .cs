
namespace JollyCactus.Maui.Views.Converters
{
    public class StringMatchConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            if (values.Length < 2)
            {
                return false;
            }
            if (values[0] == null)
            {
                return false;
            }

            for (int i = 1; i < values.Length; i++)
            {
                if (values[i] == null)
                {
                    return false;
                }
                if (!(values[0] as string).Equals(values[i] as string))
                {
                    return false;
                }
            }

            return true;

        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }

    public class StringNotMatchConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            if (values.Length < 2)
            {
                return true;
            }
            if (values[0] == null)
            {
                return true;
            }

            for (int i = 1; i < values.Length; i++)
            {
                if (values[i] == null)
                {
                    return true;
                }
                if (!(values[0] as string).Equals(values[i] as string))
                {
                    return true;
                }
            }

            return false;

        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
