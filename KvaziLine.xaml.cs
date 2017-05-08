using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.Generic;


namespace NIR_WPF
{
    /// <summary>
    /// Логика взаимодействия для KvaziLine.xaml
    /// </summary>
    public partial class KvaziLine : Window
    {
        enum btnChanged
        {
            nothing,
            btnPoint
        };

        btnChanged btn = btnChanged.nothing;

        Point pN, pK;
        private ImageReader _imageReader;
        private BitmapImage _image;
        private List<Point> points = new List<Point>();

        //  private List<PointF> _helperPoints = new List<PointF>();

        bool isMove = false;

        public BitmapImage Image
        {
            get { return _image; }
            set { _image = value; }
        }

        public KvaziLine(BitmapImage image)
        {
            InitializeComponent();
            UpdateImage(image);

        }

        private void UpdateImage(BitmapImage image)
        {
            Image = image;
            _imageReader = new ImageReader(image.UriSource.LocalPath);
            testImage.Source = Image;
        }

        private void DrawClick(object sender, RoutedEventArgs e)
        {
            inkCanvas.DefaultDrawingAttributes.Color = Colors.Red;
            btn = btnChanged.btnPoint;
        }

        private void EraseClick(object sender, RoutedEventArgs e)
        {
            inkCanvas.EditingMode = InkCanvasEditingMode.EraseByPoint;
        }

        private void inkCanvas_MouseDown(object sender, MouseButtonEventArgs e) //1
        {
            try
            {
                if (points.Count < 3)
                {
                    points.Add(new Point(e.GetPosition(e.Device.Target).X, e.GetPosition(e.Device.Target).Y));
                }
                if (points.Count == 3)
                {
                    Console.WriteLine("All points have been set. Calc..");
                    Console.WriteLine(points);
                }
            }

            catch (Exception ee)
            {
                Console.WriteLine("{0} inkCanvas_MouseDown.", ee);
            }
        }

        bool DrawingFigure = false;

        private void inkCanvas_MouseUp(object sender, MouseButtonEventArgs e) //2
        {
            //try
            //{
            //    pK = new Point(e.GetPosition(e.Device.Target).X, e.GetPosition(e.Device.Target).Y);
            //    _countPoints++;
            //    Console.WriteLine(String.Format("Mouse Up Event at {0}", e.GetPosition(e.Device.Target)));
            //    Console.WriteLine(Mouse.GetPosition(inkCanvas));

            //    if (btn == btnChanged.btnLine)
            //    {
            //        DrawLine(pN, pK, false);
            //        isMove = true;
            //    }
            //}
            //catch (Exception ee)
            //{
            //    Console.WriteLine("{0} inkCanvas_MouseUp.", ee);
            //}
        }


        private void inkCanvas_MouseMove(object sender, MouseEventArgs e) //3
        {
            try
            {
                switch (btn)
                {
                    case btnChanged.btnPoint:
                        Point p = Mouse.GetPosition(inkCanvas);
                        break;
                }
            }

            catch (Exception ee)
            {
                Console.WriteLine("{0} inkCanvas_MouseUp.", ee);
            }

        }

        private void DrawLine(Point start, Point stop, bool move)
        {
            try
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
                }
                else if (inkCanvas.Children.Count > 2 && !isMove)
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
            catch (Exception ee)
            {
                Console.WriteLine("{0} DrawLine", ee);
            }
        }
    }
}
