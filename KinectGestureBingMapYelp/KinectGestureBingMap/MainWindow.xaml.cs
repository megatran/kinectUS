//Slash Hack 2016
//We used a lot of resources, especially http://kinect.github.io/tutorial/


using Microsoft.Kinect;
using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using YelpSharp;


namespace KinectBing
{

    public partial class MainWindow : Window
    {
        #region Members

        KinectSensor _sensor;
        MultiSourceFrameReader _reader;
        IList<Body> _bodies;

        bool _displayBody = false;



        //Yelp
        Options options = new Options()
        {
            AccessToken = Environment.GetEnvironmentVariable("jpGKElXkQJA7OsdJ3DHPv2d53a33tjdp", EnvironmentVariableTarget.Machine),
            AccessTokenSecret = Environment.GetEnvironmentVariable("ICM47fj0qkujqvXi-yuToKaqUNM", EnvironmentVariableTarget.Machine),
            ConsumerKey = Environment.GetEnvironmentVariable("IfBsWQRMe5vcTXK0kIQ2tw", EnvironmentVariableTarget.Machine),
            ConsumerSecret = Environment.GetEnvironmentVariable("ttHWtJCesnUxIivGfGbu4aFzYFo", EnvironmentVariableTarget.Machine)
        };

        //End Yelp

        // add for gesture
        int _stateCount = 0;
        int _optCount = 0;
        int _shakeCount = 0;
        int _zoomInShakeCount = 0;
        int _zoomOutShakeCount = 0;
        int _navigationDirection = 0;
        int maxFrame = 5;
        int _navigationPreprocessFrameNumber = 0;
        int _directionCount1 = 0;
        int _directionCount2 = 0;
        bool _processing = false;
        Joint _preHandRight = new Joint();
        double _preDistance = 0;
        double _zoomOutTrigerDistance = 0.5;
        double _zoomInTrigerDistance = 0.1;

        TextBlock textBlockZoomIn = new TextBlock();
        TextBlock textBlockZoomOut = new TextBlock();
        TextBlock textBlockNavigation = new TextBlock();

        enum _processType 
        {
            None, ZoomIn, ZoomOut, Navigation
        };

        int _processingType = (int)_processType.None;


        #endregion

        #region Constructor

        public MainWindow()
        {
            InitializeComponent();
            _navigationPreprocessFrameNumber = maxFrame;
            kinectMap.Focus();
        }

        #endregion

        #region Event handlers

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _sensor = KinectSensor.GetDefault();

            if (_sensor != null)
            {
                _sensor.Open();

                _reader = _sensor.OpenMultiSourceFrameReader(FrameSourceTypes.Color | FrameSourceTypes.Body);
                _reader.MultiSourceFrameArrived += Reader_MultiSourceFrameArrived;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (_reader != null)
            {
                _reader.Dispose();
            }

            if (_sensor != null)
            {
                _sensor.Close();
            }
        }

        void Reader_MultiSourceFrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
        {
            var reference = e.FrameReference.AcquireFrame();

            // Color
            using (var frame = reference.ColorFrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    camera.Source = frame.ToBitmap();   
                }
            }

            // Body
            using (var frame = reference.BodyFrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    canvas.Children.Clear();

                    _bodies = new Body[frame.BodyFrameSource.BodyCount];

                    frame.GetAndRefreshBodyData(_bodies);

                    foreach (var body in _bodies)
                    {
                        if (body != null)
                        {
                            if (body.IsTracked)
                            {
                                // Draw skeleton.
                                if (_displayBody)
                                {
                                    canvas.DrawSkeleton(body);
                                    detectZoomIn(body);
                                    detectZoomOut(body);
                                    detectNavigation(body);
                                }
                            }
                        }
                    }
                }
            }
        }

        // add for gesture 
        public double jointDistance(Joint jointA, Joint jointB)
        {
            return Math.Sqrt(Math.Pow(jointA.Position.X - jointB.Position.X, 2) + Math.Pow(jointA.Position.Y - jointB.Position.Y, 2));
        }

        // ZoomIn
        // Move your both hands close together to trigger the zoom in operation
        // onece the zoom in operation triggered, move your hands apart from each other to perform the zoom in operation
        public void detectZoomIn(Body body)
        {
            infoCanvas.Children.Clear();
            textBlockZoomIn.Text = "Zoom in more";
            textBlockZoomIn.FontSize = 20;
            textBlockZoomIn.Foreground = new SolidColorBrush(Colors.Red);
            Canvas.SetRight(textBlockZoomIn, 60);
            Canvas.SetTop(textBlockZoomIn, 0);
            infoCanvas.Children.Add(textBlockZoomIn);

            if (_processingType != (int)_processType.None && _processingType != (int)_processType.ZoomIn )
            {
                // currently in a gesture processing but not for ZoomIn operation
                textBlockZoomIn.Text = "Not in gesture ZoomIn" + _processingType.ToString();
                return;
            }

            // if handTip joints are below ElbowJoint, just return or canncel current operation
            if (body.Joints[JointType.HandLeft].Position.Y <= body.Joints[JointType.ElbowLeft].Position.Y || body.Joints[JointType.HandRight].Position.Y <= body.Joints[JointType.ElbowRight].Position.Y)
            {
                textBlockZoomIn.Text = "Elbow above hand. Operation fails";
                if (_processingType != (int)_processType.None)
                {
                    resetProcessing();
                }
                return;
            }

            double distance = jointDistance(body.Joints[JointType.HandTipLeft], body.Joints[JointType.HandTipRight]);

            if (_processingType == (int)_processType.None && distance > _zoomInTrigerDistance)
            {

                textBlockZoomIn.Text = distance.ToString() + "\n no gesture, and distance is not close enough";
                return;
            }


            if (_processing != true)
            {

                if (_processingType != (int)_processType.None && distance > _zoomInTrigerDistance)
                {

                    if (_shakeCount < 5)
                    {
                        _shakeCount += 1;
                        return;
                    }
                }

                if (distance <= _zoomInTrigerDistance)
                {

                    _shakeCount = 0;

                    if (_processingType == (int)_processType.None)
                    {

                        _processingType = (int)_processType.ZoomIn;
                        _stateCount = 1;
                        _preDistance = distance;

                        textBlockZoomIn.Text = distance.ToString() + "\n Let's begin";
                        return;
                    }

                    textBlockZoomIn.Text = distance.ToString() + "\n stand straight" + _stateCount.ToString();
                    _preDistance = distance;
                    _stateCount += 1;

                    if (_stateCount > 100)
                    {
                        textBlockZoomIn.Text = distance.ToString() + "\n long time" + _stateCount.ToString();
                        resetProcessing();
                    }

                    return;
                }

                if (_stateCount < 10)
                {
                    textBlockZoomIn.Text = distance.ToString() + "\n short time";
                    resetProcessing();
                    return;
                }
            }

            if (distance < _preDistance)
            {
                // stop moving both hands apart from each other but try to move together 
                // to stop the zoom in operation
                if (_zoomInShakeCount > 1)
                {
                    textBlockZoomIn.Text = distance.ToString() + "\n zooming halt";
                    resetProcessing();
                    return;
                }
                else
                {
                    _zoomInShakeCount++;
                    return;
                } 
            }

            // reset the shake count
            _zoomInShakeCount = 0;
            // do what you want to do for the zoom in operation

            // set the state to ture to indicate gesture moving phase 
            _processing = true;

            // start Detect the gesture move
            _optCount += 1;
            textBlockZoomIn.Text = distance.ToString() + "\n zooming " + _optCount.ToString();

            var locationCentter = kinectMap.Center;
            var zoomLevel = kinectMap.ZoomLevel;
            kinectMap.SetView(locationCentter, zoomLevel+0.1);

            _preDistance = distance;

        }

        // ZoomOut
        // Put your both hands up with some distance, keep this pose stable to trigger the zoom out operation
        // Once the zoom out operation triggered, move your hands close to each other (shorten the distance) to perform the zoom out operation 
        public void detectZoomOut(Body body)
        {
            //canvas.Children.Clear();
            textBlockZoomOut.Text = "Zoomout ";
            textBlockZoomOut.FontSize = 20;
            textBlockZoomOut.Foreground = new SolidColorBrush(Colors.Blue);
            Canvas.SetRight(textBlockZoomOut, 60);
            Canvas.SetTop(textBlockZoomOut, 100);
            infoCanvas.Children.Add(textBlockZoomOut);

            if (_processingType != (int)_processType.None && _processingType != (int)_processType.ZoomOut)
            {

                textBlockZoomOut.Text = "Not in gesture ZoomOut currently in gesture processing: " + _processingType.ToString();
                return;
            }

            if (body.Joints[JointType.HandLeft].Position.Y <= body.Joints[JointType.ElbowLeft].Position.Y || body.Joints[JointType.HandRight].Position.Y <= body.Joints[JointType.ElbowRight].Position.Y)
            {
                textBlockZoomOut.Text = "Hands below elbows, no gesture detection initiate, cancel current operation"
                    // + "\n HandLeft: " + body.Joints[JointType.HandLeft].Position.Y +
                    // "\n ElbowLeft: " + body.Joints[JointType.ElbowLeft].Position.Y +
                    // "\n HandRight: " + body.Joints[JointType.HandRight].Position.Y +
                    // "\n ElboRight: " + body.Joints[JointType.ElbowRight].Position.Y
                    ;

                if (_processingType != (int)_processType.None)
                {
                    resetProcessing();
                }
                return;
            }

            // check the distance between two handTip joint
            double distance = jointDistance(body.Joints[JointType.HandTipLeft], body.Joints[JointType.HandTipRight]);

            if (_processingType == (int)_processType.None && distance < _zoomOutTrigerDistance)
            {
                // currently no gesture processing but the distance is too close to triger the zoom out operation
                textBlockZoomOut.Text = distance.ToString() + "\n no gesture, but distance is too short to triger zoom out";
                return;
            }

            if (distance > _zoomOutTrigerDistance && _processingType == (int)_processType.None)
            {
                // initiate the zoom out detection
                _processingType = (int)_processType.ZoomOut;
                _stateCount = 1;
                _shakeCount = 0;
                _preDistance = distance;

                textBlockZoomOut.Text = distance.ToString() + "\n start detect .............";
                return;
            }

            // still in the phase of detecting the gesture
            if (!_processing)
            {
                if (Math.Abs(distance - _preDistance) > 0.01 && _shakeCount < 2)
                {
                    _shakeCount += 1;
                    return;
                }

                // should stay at this pose for a short time to initiate the zoom out operation
                if (Math.Abs(distance - _preDistance) < 0.01 && distance > _zoomOutTrigerDistance)
                {

                    textBlockZoomOut.Text = distance.ToString() + "\n keep the pose ...." + _stateCount.ToString() +
                        "\n shaking " + Math.Abs(distance - _preDistance);
                    //_preDistance = distance;
                    _stateCount += 1;

                    // reset pose count
                    _shakeCount = 0;

                    if (_stateCount > 100)
                    {
                        textBlockZoomOut.Text = distance.ToString() + "\n keep the pose too long ...." + _stateCount.ToString();
                        resetProcessing();
                    }

                    return;
                }


                if (_stateCount < 10)
                {
                    // keep the pose too short to triger the zoomout processing
                    // cancel to initiate the zoomout processing
                    textBlockZoomOut.Text = distance.ToString() + "\n keep pose too short to triger zoom out ......." + " stateCount: " + _stateCount.ToString();
                    resetProcessing();
                    return;
                }
            }

            if (distance > _preDistance)
            {
                // stop moving both hands together from each other and give up the zoom out by moving hands apart 
                if (_zoomOutShakeCount > 1)
                {
                    textBlockZoomOut.Text = distance.ToString() + "\n stop zoom out.......";
                    resetProcessing();
                }
                else
                {
                    _zoomOutShakeCount++;
                }
                return;
            }

            // reset the shake count
            _zoomOutShakeCount = 0;

            // do what you want to do for the zoom in operation

            // set the state to ture to indicate gesture moving phase 
            _processing = true;

            // start Detect the gesture move
            _optCount += 1;
            textBlockZoomOut.Text = distance.ToString() + "\n" + _optCount.ToString();

            _preDistance = distance;

            var locationCentter = kinectMap.Center;
            var zoomLevel = kinectMap.ZoomLevel;
            kinectMap.SetView(locationCentter, zoomLevel - 0.1);

        }

        // add for gesture
        public void detectNavigation(Body body)
        {
            //infoCanvas.Children.Clear();
            textBlockNavigation.FontSize = 20;
            textBlockNavigation.Foreground = new SolidColorBrush(Colors.DarkGreen);
            Canvas.SetRight(textBlockNavigation, 60);
            Canvas.SetTop(textBlockNavigation, 200);
            infoCanvas.Children.Add(textBlockNavigation);

            if (_processingType != (int)_processType.None && _processingType != (int)_processType.Navigation)
            {
                textBlockNavigation.Text = "gesture: " + _processingType.ToString();
                return;
            }


            if (body.Joints[JointType.HandRight].Position.Y < body.Joints[JointType.ElbowRight].Position.Y)
            {
                textBlockNavigation.Text = "Elbow is higher than right hand";
                resetProcessing();
                return;
            }

            if (body.Joints[JointType.HandLeft].Position.Y >= body.Joints[JointType.ElbowLeft].Position.Y)
            {
                textBlockNavigation.Text = "elbow is above both hands";
                resetProcessing();
                return;
            }


            double distance = jointDistance(body.Joints[JointType.WristRight], body.Joints[JointType.SpineMid]);

            if (Math.Abs(distance - _preDistance) > 0.001)
            {
                if (_shakeCount > 5 && !_processing)
                {
                    resetProcessing();
                    _preDistance = distance;
                    return;
                }
                else if(!_processing)
                {
                    _shakeCount += 1;
                    _preDistance = distance;
                    return;
                }
            }

            if (Math.Abs(distance - _preDistance) <= 0.002)
            {

                _shakeCount = 0;

                _stateCount += 1;

                if (!_processing)
                {
                    if (_stateCount < 10)
                    {
                        textBlockNavigation.Text = "stand straight " + _stateCount.ToString();
                        return;
                    }
                }
            }
            else
            {
                textBlockNavigation.Text = "stateCount = 0 .... ";
                _stateCount = 0;
            }


            _processing = true;
            _processingType = (int)_processType.Navigation;

            if (_stateCount != 0)
            {
                textBlockNavigation.Text = "move to a direction!" + _stateCount;
                return;
            }

            textBlockNavigation.Text = "calculating";

            if (_navigationPreprocessFrameNumber == maxFrame)
            {
                _preHandRight = body.Joints[JointType.HandRight];
            }

            if (_navigationPreprocessFrameNumber > 0)
            {
                if (distance > _preDistance)
                {
                    _directionCount1++;
                }
                else
                {
                    _directionCount2++;
                }

                _preDistance = distance;
                _navigationPreprocessFrameNumber--;
                return;
            }

            var Y = body.Joints[JointType.HandRight].Position.Y - _preHandRight.Position.Y;
            var X = body.Joints[JointType.HandRight].Position.X - _preHandRight.Position.X;

            if (_navigationDirection == 0)
            {
                _navigationDirection = (_directionCount1 > _directionCount2) ? 1 : 2;
            }



            if ((_navigationDirection == 1 && distance < _preDistance) || (_navigationDirection == 2 && distance > _preDistance))
            {
                textBlockNavigation.Text = "Nice gesture";
                _navigationDirection = 0;
                resetProcessing();
                return;
            }

            _preDistance = distance;
            _optCount++;

            textBlockNavigation.Text = "Going to"
                + "\n dir: " + geoDegree(4,-4)
                + "\n le: " + kinectMap.ZoomLevel 
                + "\n pro: " + _optCount;

            var locationCentter = kinectMap.Center;
            var zoomLevel = kinectMap.ZoomLevel;

            var ratio = zoomLevel >= 12 ? Math.Pow(0.5,zoomLevel-12) : Math.Pow(2, 12 - zoomLevel);

            locationCentter.Latitude -= (body.Joints[JointType.HandRight].Position.Y - _preHandRight.Position.Y) / _optCount * ratio;
            locationCentter.Longitude -= (body.Joints[JointType.HandRight].Position.X - _preHandRight.Position.X) / _optCount * ratio;
            kinectMap.SetView(locationCentter, zoomLevel);

        }

        public double geoDegree(double x, double y)
        {

            var degree = (x > 0 && y > 0) ? 0.0 : ((x>0&&y<0)?90.0:((x<0&&y>0)?270.0:180.0));
            
            x = Math.Abs(x);
            y = Math.Abs(y);

            var radians = Math.Atan2(y, x);
            return degree += radians * (180 / Math.PI);

        }

        // add for gesture
        public void resetProcessing()
        {
            _processingType = (int)_processType.None;
            _stateCount = 0;
            _preDistance = 0;
            _optCount = 0;
            _shakeCount = 0;
            _processing = false;
            _navigationPreprocessFrameNumber = maxFrame;
            _directionCount1 = 0;
            _directionCount2 = 0;
        }

 

        private void Body_Click(object sender, RoutedEventArgs e)
        {
            _displayBody = !_displayBody;
        }

        #endregion
    }

}
