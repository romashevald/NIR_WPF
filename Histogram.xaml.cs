using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using LiveCharts;
using LiveCharts.Geared;
using LiveCharts.Wpf;


namespace NIR_WPF
{
    /// <summary>
    /// Логика взаимодействия для Histogram.xaml
    /// </summary>
    public partial class Histogram : Window
    {
        enum btnChanged { nothing, btnLine, btnRect };

        Point pN, pK;
        bool isMove = false;
        btnChanged btn = btnChanged.nothing;

        private BitmapImage _image;
        public BitmapImage Image
        {
            get { return _image; }
            set { _image = value; }
        }

        public Histogram(BitmapImage image)
        {
            InitializeComponent();

            UpdateImage(image);

            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Series 1",
                    Values = new ChartValues<double> { 4, 6, 5, 2 ,4 }
                },
                new LineSeries
                {
                    Title = "Series 2",
                    Values = new ChartValues<double> { 6, 7, 3, 4 ,6 },
                    PointGeometry = null
                },
                new LineSeries
                {
                    Title = "Series 3",
                    Values = new ChartValues<double> { 4,2,7,2,7 },
                    PointGeometry = DefaultGeometries.Square,
                    PointGeometrySize = 15
                }
            };

            Labels = new[] { "Jan", "Feb", "Mar", "Apr", "May" };
            YFormatter = value => value.ToString("C");

            //modifying the series collection will animate and update the chart
            SeriesCollection.Add(new LineSeries
            {
                Title = "Series 4",
                Values = new ChartValues<double> { 5, 3, 2, 4 },
                LineSmoothness = 0, //0: straight lines, 1: really smooth lines
                PointGeometry = Geometry.Parse("m 25 70.36218 20 -28 -20 22 -8 -6 z"),
                PointGeometrySize = 50,
                PointForeround = Brushes.Gray
            });

            //modifying any series values will also animate and update the chart
            SeriesCollection[3].Values.Add(5d);

            DataContext = this;
        }

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }

        private void UpdateImage(BitmapImage image)
        {
            Image = image;
            testImage.Source = Image;
        }


        private void inkCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                if (btn == btnChanged.btnLine)
                {
                    // DrawLine(pN, pK, true);
                    Point p = Mouse.GetPosition(inkCanvas);
                    DrawLine(pN, p, true);
                }
        }

        private void DrawLine(Point start, Point stop, bool move)
        {
            Line line = new Line();
            line.Stroke = Brushes.Red;
            line.X1 = start.X;
            line.X2 = stop.X;
            line.Y1 = start.Y;
            line.Y2 = stop.Y;
            if (!move)
                inkCanvas.Children.Add(line);
            else
                if (inkCanvas.Children.Count > 2 && !isMove)
            {
                inkCanvas.Children.RemoveAt(inkCanvas.Children.Count - 1);
                inkCanvas.Children.Add(line);
            }
            else
            {
                inkCanvas.Children.Add(line);
                isMove = false;
            }
        }

        private void inkCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            pN = Mouse.GetPosition(inkCanvas);
        }

        private void inkCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            pK = Mouse.GetPosition(inkCanvas);
            if (btn == btnChanged.btnLine)
            {
                DrawLine(pN, pK, false);
                isMove = true;
            }
        }

        private void ExitProject(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void LineClick(object sender, RoutedEventArgs e)
        {
            btn = btnChanged.btnLine;
        }
    }

}
