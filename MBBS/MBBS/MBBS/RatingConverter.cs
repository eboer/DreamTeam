using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MBBS
{
    class RatingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var rating = (int) value;
            if (rating == 0)
                return 0;
            if (rating == 1)
                return 1;
            if (rating == 2)
                return 2;
            if (rating == 3)
                return 3;
            if (rating == 4)
                return 4;
            if (rating == 5)
                return 5;
            if (rating > 5)
                rating = rating % 5;
            if (rating == 0)
                rating = 5;
            return rating;

            /*if (rating > 20)
                rating = (int)value - 20;
            if (rating > 15)
                rating = (int)value - 15;
            if (rating > 10)
                rating = (int)value - 10;
            if (rating > 5)
                rating = (int)value - 5;
            return rating;*/
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class RatingVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

/*#if DEBUG || ENABLE_TEST_CLOUD
            return true;
#endif

            //var session = value as Session;
            if (session == null)
                return false;

            if (!session.StartTime.HasValue)
                return false;

            //if it has started or is about to start
            if (session.StartTime.Value.AddMinutes(-15) < DateTime.UtcNow)
                return true;

            return false;*/
            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
