using Microsoft.Speech;
using Microsoft.Speech.Recognition;
using Microsoft.Kinect;
using Microsoft.Kinect.Input;
using Microsoft.Kinect.Tools;
using Microsoft.Kinect.VisualGestureBuilder;
using Microsoft.Kinect.Wpf.Controls;
using mmisharp;
using Paelife.KinectFramework;
using Paelife.KinectFramework.FaceRecognition;
using Paelife.KinectFramework.FaceTracking;
using Paelife.KinectFramework.Gestures;
using Paelife.KinectFramework.Postures;
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

namespace kinectModality
{
    /// <summary>
    /// Lógica interna para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        KinectManager kinectManager;
        KinectSensor sensor;
        BodyFrameReader bodyReader;
        VisualGestureBuilderFrameSource gestureSource;
        VisualGestureBuilderFrameReader gestureReader;

        private WriteableBitmap colorBitmap;
        private byte[] colorPixels;

        private DateTime backgroundHighlightTime = DateTime.MinValue;
        private DateTime faceRecoDisplayTime = DateTime.MinValue;
        private DateTime centerMessageDisplayTime = DateTime.MinValue;
        private System.Windows.Media.Brush backgroudBrush;

        bool kinectStartedSucessfully = false;
        bool playMusic = false;


        private LifeCycleEvents lce;
        private MmiCommunication mmic;
        private TTS tts;

        /* Gesture database */
        VisualGestureBuilderDatabase _gesture_database;
        /*
        VisualGestureBuilderDatabase _open_hand_database;
        VisualGestureBuilderDatabase _swipe_right_database;
        VisualGestureBuilderDatabase _swipe_left_database;
        VisualGestureBuilderDatabase _swipe_up_database;
        VisualGestureBuilderDatabase _swipe_down_database;
        VisualGestureBuilderDatabase _rotate_clockwise_database;
        VisualGestureBuilderDatabase _rotate_counter_clockwise_database;
        VisualGestureBuilderDatabase _cross_database;
        */
        /* Gestures from database */
        Gesture closeHandGesture;
        Gesture openHandGesture;
        Gesture swipeRightGesture;
        Gesture swipeLeftGesture;
        Gesture swipeUpGesture;
        Gesture swipeDownGesture;
        Gesture rotateClockWiseGesture;
        Gesture rotateCounterClockWiseGesture;
        Gesture crossGesture;
        Gesture closeEarsGesture;

        /* Gesture variables */

        static CloseHandGesture _close_hand_gesture = new CloseHandGesture();
        static OpenHandGesture _open_hand_gesture = new OpenHandGesture();
        static SwipeRightGesture _swipe_right_gesture = new SwipeRightGesture();
        static SwipeLeftGesture _swipe_left_gesture = new SwipeLeftGesture();
        static SwipeUpGesture _swipe_up_gesture = new SwipeUpGesture();
        static SwipeDownGesture _swipe_down_gesture = new SwipeDownGesture();
        static RotateClockWiseGesture _rotate_clockwise_gesture = new RotateClockWiseGesture();
        static RotateCounterClockWiseGesture _rotate_counter_clockwise_gesture = new RotateCounterClockWiseGesture();
        static CrossGesture _cross_gesture = new CrossGesture();
        static CloseEarsGesture _close_ears_gesture = new CloseEarsGesture();


        public MainWindow()
        {
            //init LifeCycleEvents..
            lce = new LifeCycleEvents("ASR", "IM", "speech-1", "acoustic", "command");
            mmic = new MmiCommunication("localhost", 8000, "User1", "ASR");
            tts = new TTS();

            mmic.Send(lce.NewContextRequest());
            InitializeComponent();

            var sensor = KinectSensor.GetDefault();

            if(sensor != null)
            {
                _close_hand_gesture.GestureRecognized += Gesture_Recognized;
                _open_hand_gesture.GestureRecognized += Gesture_Recognized;
                _swipe_right_gesture.GestureRecognized += Gesture_Recognized;
                _swipe_left_gesture.GestureRecognized += Gesture_Recognized;
                _swipe_down_gesture.GestureRecognized += Gesture_Recognized;
                _swipe_up_gesture.GestureRecognized += Gesture_Recognized;
                _rotate_clockwise_gesture.GestureRecognized += Gesture_Recognized;
                _rotate_counter_clockwise_gesture.GestureRecognized += Gesture_Recognized;
                _cross_gesture.GestureRecognized += Gesture_Recognized;
                _close_ears_gesture.GestureRecognized += Gesture_Recognized;

                sensor.Open();

            }
            this.Loaded += OnLoaded;
        }

        void OnLoaded(object sender, RoutedEventArgs e)
        {
            
        }

        void Gesture_Recognized(object sender, EventArgs e)
        {
            if (sender.ToString().Equals("kinectModality.CloseHandGesture"))
            {
                if(sender == (object) closeHandGesture)
                {
                    Console.WriteLine("Close Hand Gesture Input!");
                    tts.Speak("Menu Principal!");
                    CloseHandGesture_OnGestureDetected("menuprincipal");
                }
                else
                {
                    Console.WriteLine("Invalid Input!");
                }
            }
            else if (sender.ToString().Equals("kinectModality.OpenHandGesture"))
            {
                if(sender == (object) openHandGesture)
                {
                    Console.WriteLine("Open Hand Gesture Input!");
                    tts.Speak("Reproduzindo Música!");
                    if (!playMusic)
                    {
                        OpenHandGesture_OnGestureDetected("reproduzir");
                        playMusic = true;
                    }
                    else
                    {
                        OpenHandGesture_OnGestureDetected("parar");
                        playMusic = false;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid Input!");
                }
            }
            else if (sender.ToString().Equals("kinectModality.SwipeRightGesture"))
            {
                if (sender == (object) swipeRightGesture)
                {
                    Console.WriteLine("Swipe Right Gesture Input!");
                    tts.Speak("Avançando Música!");
                    SwipeRightGesture_OnGestureDetected("avançar");
                }
                else
                {
                    Console.WriteLine("Invalid Input!");
                }

            }
            else if (sender.ToString().Equals("kinectModality.SwipeLeftGesture"))
            {
                if(sender == (object) swipeLeftGesture)
                {
                    Console.WriteLine("Swipe Left Gesture Input!");
                    tts.Speak("Voltar à música anterior!");
                    SwipeLeftGesture_OnGestureDetected("musicaanterior");
                }
                else
                {
                    Console.WriteLine("Invalid Input!");
                }

            }
            else if (sender.ToString().Equals("kinectModality.SwipeUpGesture"))
            {
                if(sender == (object) swipeUpGesture)
                {
                    Console.WriteLine("Swipe Up Gesture Input!");
                    tts.Speak("Aumentando volume!");
                    SwipeUpGesture_OnGestureDetected("volumeup");

                }
                else
                {
                    Console.WriteLine("Invalid Input!");
                }

            }
            else if (sender.ToString().Equals("kinectModality.SwipeDownGesture"))
            {
                if(sender == (object) swipeDownGesture)
                {
                    Console.WriteLine("Swipe Down Gesture Input");
                    tts.Speak("Diminuindo volume!");
                    SwipeDownGesture_OnGestureDetected("volumedown");
                }
                else
                {
                    Console.WriteLine("Invalid Input!");
                }
            }
            else if (sender.ToString().Equals("kinectModality.RotateClockWiseGesture"))
            {
                if(sender == (object) rotateClockWiseGesture)
                {
                    Console.WriteLine("Rotate Clockwise Gesture Input");
                    tts.Speak("Repetindo música");
                    RotateClockWiseGesture_OnGestureDetected("repetir");
                }
                else
                {
                    Console.WriteLine("Invalid Input!");
                }

            }
            else if (sender.ToString().Equals("kinectModality.RotateCounterClockWiseGesture"))
            {
                if(sender == (object) rotateCounterClockWiseGesture)
                {
                    Console.WriteLine("Rotate Counter Clockwise Gesture Input");
                    tts.Speak("Baralhando música");
                    RotateCounterClockWiseGesture_OnGestureDetected("baralhar");
                }
                else
                {
                    Console.WriteLine("Invalid Input!");
                }
            }
            else if (sender.ToString().Equals("kinectModality.CrossGesture"))
            {
                if(sender == (object) crossGesture)
                {
                    Console.WriteLine("Cross Gesture Input");
                    tts.Speak("Fechando aplicação.");
                    CrossGesture_OnGestureDetected("fecharSpotify");
                }
                else
                {
                    Console.WriteLine("Invalid Input!");
                }

            }
            else if (sender.ToString().Equals("kinectModality.CloseEarsGesture"))
            {
                if(sender == (object) closeEarsGesture)
                {
                    Console.WriteLine("Close Ears Gesture Input!");
                    tts.Speak("Tirando som");
                    CloseEarsGesture_OnGestureDetected("mute");
                }
                else
                {
                    Console.WriteLine("Invalid Input!");
                }
            }
            else
            {
                Console.WriteLine(sender.ToString());
                Console.WriteLine(e.ToString());
            }
        }

        void OnLoadGesturesFromDb(object sender, RoutedEventArgs e)
        {
            // we assume that the database files exist and that they will load
            _gesture_database = new VisualGestureBuilderDatabase(@"KinectGestures\\reproduzirSolutionGBD.gbd");
          
            // load gestures from database
            this.openHandGesture = _gesture_database.AvailableGestures.Where(g => g.Name == "reproduzir").Single();
            this.swipeRightGesture = _gesture_database.AvailableGestures.Where(g => g.Name == "SRHavancar_Right").Single();
            this.swipeLeftGesture = _gesture_database.AvailableGestures.Where(g => g.Name == "SLHretroceder_Left").Single();
            this.swipeUpGesture = _gesture_database.AvailableGestures.Where(g => g.Name == "aumentarVol_Right").Single();
            this.swipeDownGesture = _gesture_database.AvailableGestures.Where(g => g.Name == "diminuirVol_Right").Single();
            this.rotateClockWiseGesture = _gesture_database.AvailableGestures.Where(g => g.Name == "repetir").Single();
            this.rotateCounterClockWiseGesture = _gesture_database.AvailableGestures.Where(g => g.Name == "swipeForwardProgress").Single();
            this.crossGesture = _gesture_database.AvailableGestures.Where(g => g.Name == "fechar").Single();
            this.closeEarsGesture = _gesture_database.AvailableGestures.Where(g => g.Name == "mute").Single();
        }

        void OnOpenSensor(object sender, RoutedEventArgs e)
        {
            // assuming that the sensor is available and that it's not already open - 
            // i.e. assuming that the "user" will only press the buttons in a sensible
            // order which is not perhaps the best idea but then the user is primarily
            // me.
            this.sensor = KinectSensor.GetDefault();

            // since I added a KinectRegion/KinectUserViewer this is probably open
            // before we make this call...
            this.sensor.Open();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        void CloseHandGesture_OnGestureDetected(string gesture)
        {
            string json_msg = "{ \"recognized\": [";

            json_msg += "\"" + "right-hand" + "\", ";
            json_msg += "\"" + "gesto" + "\":\"" + gesture + "\"";
            json_msg.Substring(0, json_msg.Length - 2);
            json_msg += "] }";

            var exNot = lce.ExtensionNotification("", "", 0.0f, json_msg);
            Console.WriteLine(exNot);
            mmic.Send(exNot);
        }

        void OpenHandGesture_OnGestureDetected(string gesture)
        {
            string json_msg = "{ \"recognized\": [";

            json_msg += "\"" + "right-hand" + "\", ";
            json_msg += "\"" + "gesto" + "\":\"" + gesture + "\"";
            json_msg.Substring(0, json_msg.Length - 2);
            json_msg += "] }";

            var exNot = lce.ExtensionNotification("", "", 0.0f, json_msg);
            Console.WriteLine(exNot);
            mmic.Send(exNot);
        }

        void SwipeRightGesture_OnGestureDetected(string gesture)
        {
            string json_msg = "{ \"recognized\": [";

            json_msg += "\"" + "right-hand" + "\", ";
            json_msg += "\"" + "gesto" + "\":\"" + gesture + "\"";
            json_msg.Substring(0, json_msg.Length - 2);
            json_msg += "] }";

            var exNot = lce.ExtensionNotification("", "", 0.0f, json_msg);
            Console.WriteLine(exNot);
            mmic.Send(exNot);

        }

        void SwipeLeftGesture_OnGestureDetected(string gesture)
        {
            string json_msg = "{ \"recognized\": [";

            json_msg += "\"" + "left-hand" + "\", ";
            json_msg += "\"" + "gesto" + "\":\"" + gesture + "\"";
            json_msg.Substring(0, json_msg.Length - 2);
            json_msg += "] }";

            var exNot = lce.ExtensionNotification("", "", 0.0f, json_msg);
            Console.WriteLine(exNot);
            mmic.Send(exNot);

        }

        void SwipeUpGesture_OnGestureDetected(string gesture)
        {
            string json_msg = "{ \"recognized\": [";

            json_msg += "\"" + "right-hand" + "\", ";
            json_msg += "\"" + "gesto" + "\":\"" + gesture + "\"";
            json_msg.Substring(0, json_msg.Length - 2);
            json_msg += "] }";

            var exNot = lce.ExtensionNotification("", "", 0.0f, json_msg);
            Console.WriteLine(exNot);
            mmic.Send(exNot);

        }

        void SwipeDownGesture_OnGestureDetected(string gesture)
        {
            string json_msg = "{ \"recognized\": [";

            json_msg += "\"" + "right-hand" + "\", ";
            json_msg += "\"" + "gesto" + "\":\"" + gesture + "\"";
            json_msg.Substring(0, json_msg.Length - 2);
            json_msg += "] }";

            var exNot = lce.ExtensionNotification("", "", 0.0f, json_msg);
            Console.WriteLine(exNot);
            mmic.Send(exNot);
        }

        void RotateClockWiseGesture_OnGestureDetected(string gesture)
        {
            string json_msg = "{ \"recognized\": [";

            json_msg += "\"" + "right-hand" + "\", ";
            json_msg += "\"" + "gesto" + "\":\"" + gesture + "\"";
            json_msg.Substring(0, json_msg.Length - 2);
            json_msg += "] }";

            var exNot = lce.ExtensionNotification("", "", 0.0f, json_msg);
            Console.WriteLine(exNot);
            mmic.Send(exNot);
        }

        void RotateCounterClockWiseGesture_OnGestureDetected(string gesture)
        {
            string json_msg = "{ \"recognized\": [";

            json_msg += "\"" + "right-hand" + "\", ";
            json_msg += "\"" + "gesto" + "\":\"" + gesture + "\"";
            json_msg.Substring(0, json_msg.Length - 2);
            json_msg += "] }";

            var exNot = lce.ExtensionNotification("", "", 0.0f, json_msg);
            Console.WriteLine(exNot);
            mmic.Send(exNot);

        }

        void CloseEarsGesture_OnGestureDetected(string gesture)
        {
            string json_msg = "{ \"recognized\": [";

            json_msg += "\"" + "close-ears" + "\", ";
            json_msg += "\"" + "gesto" + "\":\"" + gesture + "\"";
            json_msg.Substring(0, json_msg.Length - 2);
            json_msg += "] }";

            var exNot = lce.ExtensionNotification("", "", 0.0f, json_msg);
            Console.WriteLine(exNot);
            mmic.Send(exNot);

        }

        void CrossGesture_OnGestureDetected(string gesture)
        {
            string json_msg = "{ \"recognized\": [";

            json_msg += "\"" + "cross-arms" + "\", ";
            json_msg += "\"" + "gesto" + "\":\"" + gesture + "\"";
            json_msg.Substring(0, json_msg.Length - 2);
            json_msg += "] }";

            var exNot = lce.ExtensionNotification("", "", 0.0f, json_msg);
            Console.WriteLine(exNot);
            mmic.Send(exNot);
        }
    }
}
