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
using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using System.Collections.ObjectModel;

namespace NIR_WPF
{
    /// <summary>
    /// Логика взаимодействия для Histogram.xaml
    /// </summary>
    public partial class Histogram : Window
    {
        public ObservableCollection<Point> _redChannelData { get; set; }

        private ImageReader _imageReader;

        private List<PixelLine> _redChannelLines = new List<PixelLine>();
        private List<PixelLine> _blueChannelLines = new List<PixelLine>();
        private List<PixelLine> _greenChannelLines = new List<PixelLine>();

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
            _redChannelData = new ObservableCollection<Point>();
            UpdateImage(image);


            Plotter.AddLineChart(_redChannelData);

            DataContext = this;
        }

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }

        private void UpdateImage(BitmapImage image)
        {
            Image = image;
            _imageReader = new ImageReader(image.UriSource.LocalPath);
            testImage.Source = Image;
        }

        bool DrawingFigure = false;
        private void inkCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DrawingFigure = true;
                switch (btn)
                {
                    case btnChanged.btnLine:
                        // DrawLine(pN, pK, true);
                        Point p = Mouse.GetPosition(inkCanvas);
                        DrawLine(pN, p, true);
                        break;
                }
            }
            else
            {
                if (DrawingFigure)
                {
                    inkCanvas_MouseUp(sender, new MouseButtonEventArgs(e.MouseDevice, e.Timestamp, MouseButton.Left));
                    DrawingFigure = false;
                }
            }
        }

        private void AddLineToCollection(Line line)
        {
            PixelLine pixelRedChannelLine = new PixelLine(line, _imageReader.RedChannel);
            PixelLine pixelBlueChannelLine = new PixelLine(line, _imageReader.BlueChannel);
            PixelLine pixelGreenChannelLine = new PixelLine(line, _imageReader.GreenChannel);

            _redChannelData.AddMany(pixelRedChannelLine.GetLine().Select(x => new Point(x.X,x.Value)));//todo
            //_blueChannelData.AddMany(pixelRedChannelLine.GetLine().Select(x => new Point(x.X, x.Value)));//todo
            //_greenChannelData.AddMany(pixelRedChannelLine.GetLine().Select(x => new Point(x.X, x.Value)));//todo


            _redChannelLines.Add(pixelRedChannelLine);
            _blueChannelLines.Add(pixelRedChannelLine);
            _greenChannelLines.Add(pixelRedChannelLine);
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
            {
                inkCanvas.Children.Add(line);
                AddLineToCollection(line);
            }
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
            pN = new Point(e.GetPosition(e.Device.Target).X, e.GetPosition(e.Device.Target).Y);
            Console.WriteLine(String.Format("Mouse Down Event at {0}", e.GetPosition(e.Device.Target)));
            Console.WriteLine(Mouse.GetPosition(inkCanvas));
            //pN = Mouse.GetPosition(inkCanvas);
        }

        private void inkCanvas_MouseUp(object sender, MouseButtonEventArgs e)//todo: fix
        {
            pK = new Point(e.GetPosition(e.Device.Target).X, e.GetPosition(e.Device.Target).Y);
            Console.WriteLine(String.Format("Mouse Up Event at {0}", e.GetPosition(e.Device.Target)));
            Console.WriteLine(Mouse.GetPosition(inkCanvas));
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
