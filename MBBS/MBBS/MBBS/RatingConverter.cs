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
        // Convert the star rating and return its value
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
            //convert if rating is visible
            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
