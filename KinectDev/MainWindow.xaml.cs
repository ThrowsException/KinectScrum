using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit;
using System.Diagnostics;

namespace KinectDev
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Point startPoint;
        private Point selectedElementOrigins;
        private bool IsDragging;
        private UIElement selectedElement;
        private List<TaskCards> taskList = new List<TaskCards>();

        public MainWindow()
        {
            TaskCards taskCard = new TaskCards() { Story = "Header 1" };

            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.Manual;
            Left = SystemParameters.VirtualScreenLeft;
            Top = SystemParameters.VirtualScreenTop;
            Width = SystemParameters.VirtualScreenWidth;
            Height = SystemParameters.VirtualScreenHeight;
            this.ResizeMode = ResizeMode.NoResize;
            
            WindowStyle = WindowStyle.None;
            Topmost = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //WindowState = WindowState.Maximized;
            kinectSensorChooserUI.KinectSensorChooser = new KinectSensorChooser();
            kinectSensorChooserUI.KinectSensorChooser.Start();
            Random rand = new Random();
            for (int i = 0; i < 10; i++)
            {
                TaskCards task = new TaskCards() { Story = "Header " + i };
                int left = rand.Next(0, (int)SystemParameters.VirtualScreenWidth - 150);
                Debug.WriteLine(left);
                int top =  rand.Next(0, (int)SystemParameters.VirtualScreenHeight - 300);
                Debug.WriteLine(top);
                Canvas.SetLeft(task,left);
                Canvas.SetTop(task,top);
                theCanvas.Children.Add(task);
            }
            /*if (KinectSensor.KinectSensors.Count > 0)
            {
                _sensor = KinectSensor.KinectSensors[0];

                if (_sensor.Status == KinectStatus.Connected)
                {
                    _sensor.ColorStream.Enable();
                    _sensor.DepthStream.Enable();
                    _sensor.SkeletonStream.Enable();
                    _sensor.AllFramesReady += new EventHandler<AllFramesReadyEventArgs>(_sensor_AllFramesReady);
                    _sensor.Start();
                }
            }*/
        }

        void _sensor_AllFramesReady(object sender, AllFramesReadyEventArgs e)
        {

        }

        /*void _sensor_AllFramesReady(object sender, AllFramesReadyEventArgs e)
        {
            using (ColorImageFrame colorFrame = e.OpenColorImageFrame())
            {
                if (colorFrame != null)
                {
                    byte[] pixels = new byte[colorFrame.PixelDataLength];
                    colorFrame.CopyPixelDataTo(pixels);
                    
                    int stride = colorFrame.Width * 4;

                    kinectImage.Source = BitmapSource.Create(colorFrame.Width, colorFrame.Height,
                        96, 96, PixelFormats.Bgr32, null, pixels, stride);
                }
            }
        }*/

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            kinectSensorChooserUI.KinectSensorChooser.Stop();
        }

        private void TextBlock_MouseEnter(object sender, MouseEventArgs e)
        {
            CloseWindow.Background = new SolidColorBrush(Colors.Purple);
        }

        private void TextBlock_MouseLeave(object sender, MouseEventArgs e)
        {
            CloseWindow.Background = new SolidColorBrush(Colors.White);
        }

        private void TextBlock_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void theCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //Dont act apon events from parent element
            if (e.Source == theCanvas)
                return;

            if (!IsDragging)
            {
                startPoint = e.GetPosition(theCanvas);
                selectedElement = e.Source as UIElement;
                theCanvas.Children.Remove(selectedElement);
                theCanvas.Children.Add(selectedElement);
                theCanvas.CaptureMouse();

                IsDragging = true;

                selectedElementOrigins =
                  new Point(
                        Canvas.GetLeft(selectedElement),
                      Canvas.GetTop(selectedElement));
            }
            e.Handled = true;
        }

        private void theCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {

            if (theCanvas.IsMouseCaptured)
            {
                IsDragging = false;
                theCanvas.ReleaseMouseCapture();
                e.Handled = true;
            }
        }

        private void theCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (theCanvas.IsMouseCaptured)
            {
                //if dragging, get the delta and add it to selected 
                //element origin
                if (IsDragging)
                {
                    
                    Point currentPosition = e.GetPosition(theCanvas);
                    
                    double xOrigin = Double.IsNaN(selectedElementOrigins.X) ? 0 : selectedElementOrigins.X;
                    double yOrigin = Double.IsNaN(selectedElementOrigins.Y) ? 0 : selectedElementOrigins.Y; 
                    
                    double elementLeft = (currentPosition.X - startPoint.X) +
                       xOrigin;
                    double elementTop = (currentPosition.Y - startPoint.Y) +
                        yOrigin;

                    Canvas.SetLeft(selectedElement, elementLeft);
                    Canvas.SetTop(selectedElement, elementTop);
                }
            }
        }




    }
}
