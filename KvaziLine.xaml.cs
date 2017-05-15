using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Drawing;
using Microsoft.Win32;
using System.IO;

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

        private ImageReader _imageReader;
        private BitmapImage _image;
        private List<System.Drawing.PointF> points = new List<System.Drawing.PointF>();

        bool isMove = false;

        public BitmapImage Image
        {
            get { return _image; }
            set { _image = value; }
        }

        public KvaziLine(BitmapImage image)
        {
            if(image == null)
            {

                InitializeComponent();
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.ShowDialog();
                Uri uri = new Uri(dialog.FileName);
                BitmapImage bitmap = new BitmapImage(uri);
                testImage.Source = bitmap;

                _image = bitmap;
            }
           
            else
            {
                InitializeComponent();
                UpdateImage(image);
            }

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

        private void inkCanvas_MouseDown(object sender, MouseButtonEventArgs e) 
        {

            try
            {
                if (points.Count < 3)
                {
                    points.Add(new System.Drawing.PointF((float)e.GetPosition(e.Device.Target).X, (float)e.GetPosition(e.Device.Target).Y));
                }
                else
                {
                    points.Clear();
                    inkCanvas.Strokes.Clear();
                }

                if (points.Count == 3)
                {
                    Console.WriteLine("All points have been set. Calc..");
                    Console.WriteLine(points);
                    KvaziParams _params = KvaziTools.GetParams(points);
                    
                    DrawLines(points, _params);
                    
                }

            }

            catch (Exception ee)
            {
                Console.WriteLine("{0} inkCanvas_MouseDown.", ee);
            }
        }

        private void DrawLines(List<PointF> points, KvaziParams _params)
        {
            try
            {
                System.Windows.Point A = new System.Windows.Point(points[0].X, points[0].Y);
                System.Windows.Point B = new System.Windows.Point(points[1].X, points[1].Y);
                System.Windows.Point C = new System.Windows.Point(points[2].X, points[2].Y);
                
                DrawLine(A, B, true);
                DrawLine(B, C, true);

                System.Windows.Point centerAB = new System.Windows.Point(_params.D.X, _params.D.Y);
                System.Windows.Point centerBC = new System.Windows.Point(_params.E.X, _params.E.Y);
                System.Windows.Point center = new System.Windows.Point(_params.CircleCentr.X, _params.CircleCentr.Y);

                DrawLine(centerAB, center, true);
                DrawLine(centerBC, center, true);

                DrawLine(A, center, true);
                DrawLine(B, center, true);
                DrawLine(C, center, true);

            }

            catch (Exception ee)
            {
                Console.WriteLine("{0} DrawLine", ee);
            }
            
        }


        public void DrawLine(System.Windows.Point start, System.Windows.Point stop, bool move)
        {
            try
            {
                Line line = new Line();
                line.Stroke = System.Windows.Media.Brushes.Red;
                line.X1 = start.X;
                line.X2 = stop.X;
                line.Y1 = start.Y;
                line.Y2 = stop.Y;
                if (!move)
                {
                    inkCanvas.Children.Add(line);
                }
                else
                if (inkCanvas.Children.Count > 10 && !isMove)
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

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(e);
            int marg = int.Parse(this.inkCanvas.Margin.Left.ToString());
            RenderTargetBitmap renderTargetBitmap =
                new RenderTargetBitmap((int)this.inkCanvas.ActualWidth - marg,
                    (int)this.inkCanvas.ActualHeight - marg, 0, 0, PixelFormats.Pbgra32);
            renderTargetBitmap.Render(this.inkCanvas);
            PngBitmapEncoder pngImage = new PngBitmapEncoder();
            pngImage.Frames.Add(BitmapFrame.Create(renderTargetBitmap));
            //using (Stream fileStream = File.Create(@"C:\Users\rld\kvazi.bmp"))
            //{
            //    pngImage.Save(fileStream);
            //    Clipboard.SetImage.
            //}
        }
    }
}
