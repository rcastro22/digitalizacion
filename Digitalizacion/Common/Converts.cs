using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Digitalizacion.Common
{
    public class ScenarioBindingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Scenario s = value as Scenario;
            return (MainPage.Current.Scenarios.IndexOf(s) + 1) + ") " + s.Title;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return true;
        }
    }

    public class UIntToVisibilityConverter : IValueConverter
    {
        virtual public Object Convert(Object value, Type targetType, Object parameter, String language)
        {
            return ((int)value) > 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        virtual public Object ConvertBack(Object value, Type targetType, Object parameter, String language)
        {
            throw new NotImplementedException("UIntToVisibilityConverter::ConvertBack not implemented");
        }
    }

    public class OutputHeightConverter : IValueConverter
    {
        virtual public Object Convert(Object value, Type targetType, Object parameter, String language)
        {
            // Return 85% of the input value
            return ((double)(value) * 0.85);
        }

        virtual public Object ConvertBack(Object value, Type targetType, Object parameter, String language)
        {
            throw new NotImplementedException("OutputHeightConverter::ConvertBack not implemented");
        }
    }

    public class BooleanToVisibilityConverter : IValueConverter
    {
        virtual public Object Convert(Object value, Type targetType, Object parameter, String language)
        {
            return ((bool)value) == true ? Visibility.Visible : Visibility.Collapsed;
        }

        virtual public Object ConvertBack(Object value, Type targetType, Object parameter, String language)
        {
            throw new NotImplementedException("BooleanToVisibilityConverter::ConvertBack not implemented");
        }
    }

    public class LongValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, Object parameter, String language)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, Object parameter, String language)
        {
            if (string.IsNullOrWhiteSpace(value.ToString()))
            {
                return null;
            }

            return System.Convert.ToInt64(value);
        }
    }

    public class DateValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, Object parameter, String language)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, Object parameter, String language)
        {
            if (string.IsNullOrWhiteSpace(value.ToString()))
            {
                return DateTime.Today;
            }

            DateTimeOffset Fecha = (DateTimeOffset)value;

            return Fecha.DateTime;
        }
    }
}
