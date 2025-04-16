using JollyCactus.Maui.Settings;
using Microsoft.Maui.Controls;
using System.Globalization;

namespace JollyCactus.Maui.Views.Converters
{
    class FileNameToFullFileNameInAccountDirConverter : IValueConverter
    {      
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value == null || value is not string)
                return null;

            var jcSettings = Application.Current.MainPage.Handler.MauiContext.Services.GetService<JCSettings>();
            if (jcSettings == null)
                return null;

            return jcSettings.GetFullFileNameByName(value as string);
        }       

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value == null || value is not string)
                return null;

            return Path.GetFileName(value as string);
        }
    }
}
