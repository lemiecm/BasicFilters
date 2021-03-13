using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using static CGHomework1.MainWindow;

namespace CGHomework1
{
    public class VerticesConverter : IValueConverter
    {
        public object Convert(
            object value, Type targetType, object parameter, CultureInfo culture)
        {
            var vertices = value as IEnumerable<Vertex>;

            return vertices != null
                ? new PointCollection(vertices.Select(v => v.Point))
                : null;
        }

        public object ConvertBack(
            object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
