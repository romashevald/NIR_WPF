using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using System.IO;

namespace NIR_WPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static Logger LOG = Logger.GetInstance(typeof(MainWindow));
        private BitmapImage _image;


        public MainWindow()
        {
            LOG.Info("components initialization");
            Console.WriteLine(String.Join(",", KvaziTools.solve(new float[,] { { 2, -1}, {2, 1 } }, new float[] {0, 4})));
            InitializeComponent();
        }



        private void ExitProject(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void LoadPhoto(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.ShowDialog();

            Uri uri = new Uri(dialog.FileName);
            BitmapImage bitmap = new BitmapImage(uri);
            testImage.Source = bitmap;

            _image = bitmap;
        }


        private void SelectionAreaClick(object sender, RoutedEventArgs e)
        {
            inkCanvas.EditingMode = InkCanvasEditingMode.Select;
        }

        private void SaveAreaClick(object sender, RoutedEventArgs e)
        {
            try
            {
                int marg = int.Parse(this.inkCanvas.Margin.Left.ToString());
                RenderTargetBitmap rtb =
                        new RenderTargetBitmap((int)this.inkCanvas.ActualWidth - marg,
                                (int)this.inkCanvas.ActualHeight - marg, 0, 0,
                            PixelFormats.Default);
                rtb.Render(this.inkCanvas);
                BmpBitmapEncoder encoder = new BmpBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(rtb));

                FileStream fs = File.Open(@"D:\test.bmp", FileMode.Create);

                encoder.Save(fs);
                fs.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DrawClick(object sender, RoutedEventArgs e)
        {
            LOG.Info("Draw Click Called");
            inkCanvas.DefaultDrawingAttributes.Color = Colors.Red;
        }

        private void EraseClick(object sender, RoutedEventArgs e)
        {
            inkCanvas.EditingMode = InkCanvasEditingMode.EraseByPoint;
        }

        private void ClearClick(object sender, RoutedEventArgs e)
        {
            inkCanvas.Strokes.Clear();
        }

        static byte[] GetBytesFromBitmapSource(BitmapSource bmp)
        {
            int width = bmp.PixelWidth;
            int height = bmp.PixelHeight;
            int stride = width * ((bmp.Format.BitsPerPixel + 7) / 8);

            byte[] pixels = new byte[height * stride];
            bmp.CopyPixels(pixels, stride, 0);
            return pixels;
        }

        private void AreaClick(object sender, RoutedEventArgs e)
        {
            try
            {
                int marg = int.Parse(this.inkCanvas.Margin.Left.ToString());
                RenderTargetBitmap rtb =
                        new RenderTargetBitmap((int)this.inkCanvas.ActualWidth - marg,
                                (int)this.inkCanvas.ActualHeight - marg, 0, 0,
                            PixelFormats.Default);
                rtb.Render(this.inkCanvas);
                BmpBitmapEncoder encoder = new BmpBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(rtb));


                byte[] bytesBmp = GetBytesFromBitmapSource(encoder.Frames.FirstOrDefault());
                List<Tuple<int, int>> pointList = new List<Tuple<int, int>>();
                var frame = encoder.Frames.FirstOrDefault();

                for (int i = 0; i < bytesBmp.Length - 2; i++)
                {
                    if ((bytesBmp[i] == 255) && (bytesBmp[i + 1] == 0) && (bytesBmp[i + 2] == 0))
                        pointList.Add(new Tuple<int, int>((i) / (frame.PixelWidth * 4), ((i) % (frame.PixelWidth * 4)) / 4));
                    //pointList.Add(new Tuple<int, int>((i % frame.PixelWidth) / 4,i / frame.PixelWidth));
                }

                byte[,] checkInside = new byte[frame.PixelHeight, frame.PixelWidth];
                bool rowPixelsBool = false;
                int cc = 0;
                for (int i = 0; i < frame.PixelHeight; i++)
                {
                    Tuple<int, int> checkTuple = new Tuple<int, int>(0, 0);
                    bool flag = false;

                    for (int j = 0; j < frame.PixelWidth; j++)
                    {
                        if (pointList.Contains(new Tuple<int, int>(i, j)))
                        {
                            if (!rowPixelsBool)
                            {
                                rowPixelsBool = true;
                                checkInside[i, j] += 1;
                                if (!flag)
                                {
                                    flag = true;
                                    checkTuple = new Tuple<int, int>(i, j);
                                }
                                else
                                {
                                    for (int k = checkTuple.Item2; k < j; k++)
                                    {
                                        checkInside[i, k] += 1;
                                        cc++;
                                    }
                                    flag = false;
                                }
                            }
                        }
                        else
                            rowPixelsBool = false;
                    }
                }

                for (int i = 0; i < frame.PixelWidth; i++)
                {
                    Tuple<int, int> checkTuple = new Tuple<int, int>(0, 0);
                    bool flag = false;

                    for (int j = 0; j < frame.PixelHeight; j++)
                    {
                        if (pointList.Contains(new Tuple<int, int>(j, i)))
                        {
                            if (!rowPixelsBool)
                            {
                                rowPixelsBool = true;
                                checkInside[j, i] += 1;
                                if (!flag)
                                {
                                    flag = true;
                                    checkTuple = new Tuple<int, int>(j, i);
                                }
                                else
                                {
                                    for (int k = checkTuple.Item1; k < j; k++)
                                        checkInside[k, i] += 1;
                                    flag = false;
                                }
                            }
                        }
                        else
                            rowPixelsBool = false;
                    }
                }

                int count = 0;
                for (int i = 0; i < frame.PixelHeight; i++)
                    for (int j = 0; j < frame.PixelWidth; j++)
                        if (checkInside[i, j] > 1)
                            count++;

                MessageBox.Show("Площадь занимаемого явления = " + count.ToString() + " пикс");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GetWindowHistogram(object sender, RoutedEventArgs e)
        {
            Histogram histogram = new Histogram(_image);
            histogram.Show();
        }

        private void GetWindowKvaziLine(object sender, RoutedEventArgs e)
        {
            KvaziLine kvaziline = new KvaziLine(_image);
            kvaziline.Show();
        }
    }
}

