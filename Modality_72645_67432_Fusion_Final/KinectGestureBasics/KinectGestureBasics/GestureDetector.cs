using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
using Microsoft.Kinect.VisualGestureBuilder;
using mmisharp;

namespace KinectGestureBasics
{
    /// <summary>
    /// Gesture Detector class which listens for VisualGestureBuilderFrame events from the service
    /// and updates the associated GestureResultView object with the latest results for the 'Seated' gesture
    /// </summary>
    public class GestureDetector : IDisposable
    {
        /// <summary> Path to the gesture database that was trained with VGB </summary>
        //private readonly string gestureDatabase = @"Database\GestureSolution_v2.gbd";
        //private readonly string gestureDatabase = "C:\\Users\\filip\\Desktop\\Curso\\IM\\Fusion\\wetransfer-8250d0\\KinectGestureBasics\\KinectGestureBasics\\Database\\GestureSolution_v2.gbd";
        private readonly string gestureDatabase = "D:\\Modality_72645_67432_Fusion_Final\\KinectGestureBasics\\KinectGestureBasics\\Database\\GestureSolution_v2.gbd";
        /// <summary> Name of the discrete gesture in the database that we want to track </summary>
        private readonly string gesture_GoToHomeScreen = "GoToHomeScreen";
        private readonly string gesture_musicaAnterior = "musicaAnterior_Left";
        private readonly string gesture_musicaMute = "musicaMute";
        private readonly string gesture_musicaReproduzir = "musicaReproduzir_Right";
        private readonly string gesture_musicaSeguinte = "musicaSeguinte_Right";
        private readonly string gesture_musicaParar = "musicaParar_Left";

        bool homeScreenDetected;
        bool anteriorDetected;
        bool muteDetected;
        bool reproduzirDetected;
        bool seguinteDetected;
        bool pararDetected;

        float homeScreenConf;
        float anteriorConf;
        float muteConf;
        float reproduzirConf;
        float seguinteConf;
        float pararConf;
        
        bool altHomeScreen = false;
        bool altAnterior = false;
        bool altMute = false;
        bool altReproduzir = false;
        bool altSeguinte = false;
        bool altParar = false;

        private LifeCycleEvents LCE;
        private MmiCommunication MMIC;

        /// <summary> Gesture frame source which should be tied to a body tracking ID </summary>
        private VisualGestureBuilderFrameSource vgbFrameSource = null;

        /// <summary> Gesture frame reader which will handle gesture events coming from the sensor </summary>
        private VisualGestureBuilderFrameReader vgbFrameReader = null;

        /// <summary>
        /// Initializes a new instance of the GestureDetector class along with the gesture frame source and reader
        /// </summary>
        /// <param name="kinectSensor">Active sensor to initialize the VisualGestureBuilderFrameSource object with</param>
        /// <param name="gestureResultView">GestureResultView object to store gesture results of a single body to</param>
        public GestureDetector(KinectSensor kinectSensor, GestureResultView gestureResultView)
        {
            //LCE = new LifeCycleEvents("ASR", "FUSION", "speech-1", "acoustic", "command");
            //MMIC = new MmiCommunication("localhost", 9876, "User1", "ASR");
            LCE = new LifeCycleEvents("TOUCH", "FUSION", "touch-1", "touch", "command");
            MMIC = new MmiCommunication("localhost", 9876, "User1", "TOUCH");           //CHANGED TO USER1
            //LCE = new LifeCycleEvents("KINECT", "FUSION", "kinect-1", "kinect", "command");
            //MMIC = new MmiCommunication("localhost", 9876, "User1", "KINECT");
            MMIC.Send(LCE.NewContextRequest());

            if (kinectSensor == null)
            {
                throw new ArgumentNullException("kinectSensor");
            }

            if (gestureResultView == null)
            {
                throw new ArgumentNullException("gestureResultView");
            }

            this.GestureResultView = gestureResultView;

            // create the vgb source. The associated body tracking ID will be set when a valid body frame arrives from the sensor.
            this.vgbFrameSource = new VisualGestureBuilderFrameSource(kinectSensor, 0);
            this.vgbFrameSource.TrackingIdLost += this.Source_TrackingIdLost;

            // open the reader for the vgb frames
            this.vgbFrameReader = this.vgbFrameSource.OpenReader();
            if (this.vgbFrameReader != null)
            {
                this.vgbFrameReader.IsPaused = true;
                this.vgbFrameReader.FrameArrived += this.Reader_GestureFrameArrived;
            }

            // load the gestures from the gesture database
            using (VisualGestureBuilderDatabase database = new VisualGestureBuilderDatabase(this.gestureDatabase))
            {
                this.vgbFrameSource.AddGestures(database.AvailableGestures); //load all gestures from the database
            }
        }

        /// <summary> Gets the GestureResultView object which stores the detector results for display in the UI </summary>
        public GestureResultView GestureResultView { get; private set; }

        /// <summary>
        /// Gets or sets the body tracking ID associated with the current detector
        /// The tracking ID can change whenever a body comes in/out of scope
        /// </summary>
        public ulong TrackingId
        {
            get
            {
                return this.vgbFrameSource.TrackingId;
            }

            set
            {
                if (this.vgbFrameSource.TrackingId != value)
                {
                    this.vgbFrameSource.TrackingId = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not the detector is currently paused
        /// If the body tracking ID associated with the detector is not valid, then the detector should be paused
        /// </summary>
        public bool IsPaused
        {
            get
            {
                return this.vgbFrameReader.IsPaused;
            }

            set
            {
                if (this.vgbFrameReader.IsPaused != value)
                {
                    this.vgbFrameReader.IsPaused = value;
                }
            }
        }

        /// <summary>
        /// Disposes all unmanaged resources for the class
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the VisualGestureBuilderFrameSource and VisualGestureBuilderFrameReader objects
        /// </summary>
        /// <param name="disposing">True if Dispose was called directly, false if the GC handles the disposing</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.vgbFrameReader != null)
                {
                    this.vgbFrameReader.FrameArrived -= this.Reader_GestureFrameArrived;
                    this.vgbFrameReader.Dispose();
                    this.vgbFrameReader = null;
                }

                if (this.vgbFrameSource != null)
                {
                    this.vgbFrameSource.TrackingIdLost -= this.Source_TrackingIdLost;
                    this.vgbFrameSource.Dispose();
                    this.vgbFrameSource = null;
                }
            }
        }

        /// <summary>
        /// Handles gesture detection results arriving from the sensor for the associated body tracking Id
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void Reader_GestureFrameArrived(object sender, VisualGestureBuilderFrameArrivedEventArgs e)
        {
            using (var frame = this.vgbFrameReader.CalculateAndAcquireLatestFrame())
            {
                if (frame != null)
                {
                    //IReadOnlyDictionary<Gesture, DiscreteGestureResult> discreteResults = frame.DiscreteGestureResults;
                    var discreteResults = frame.DiscreteGestureResults;

                    if (discreteResults != null)
                    {
                        foreach (Gesture gesture in this.vgbFrameSource.Gestures)
                        {
                            if(gesture.GestureType == GestureType.Discrete)
                            {
                                DiscreteGestureResult result = null;
                                discreteResults.TryGetValue(gesture, out result);

                                if(result != null)
                                {
                                    if (gesture.Name.Equals(this.gesture_musicaAnterior))
                                    {
                                        anteriorDetected = result.Detected;
                                        anteriorConf = result.Confidence;
                                    }
                                    else if (gesture.Name.Equals(this.gesture_musicaSeguinte))
                                    {
                                        seguinteDetected = result.Detected;
                                        seguinteConf = result.Confidence;
                                    }
                                    else if (gesture.Name.Equals(this.gesture_musicaMute))
                                    {
                                        muteDetected = result.Detected;
                                        muteConf = result.Confidence;
                                    }
                                    else if (gesture.Name.Equals(this.gesture_musicaReproduzir))
                                    {
                                        reproduzirDetected = result.Detected;
                                        reproduzirConf = result.Confidence;
                                    }
                                    else if (gesture.Name.Equals(this.gesture_GoToHomeScreen))
                                    {
                                        homeScreenDetected = result.Detected;
                                        homeScreenConf = result.Confidence;
                                    }
                                    else if (gesture.Name.Equals(this.gesture_musicaParar))
                                    {
                                        pararDetected = result.Detected;
                                        pararConf = result.Confidence;
                                    }

                                }
                            }
                            
                            this.GestureResultView.UpdateGestureResult(true, false, false, false, muteDetected, muteConf, false, reproduzirDetected, reproduzirConf, anteriorDetected, anteriorConf, seguinteDetected, seguinteConf, homeScreenDetected, homeScreenConf, pararDetected, pararConf);

                            if (homeScreenDetected)
                            {
                                if (!altHomeScreen)
                                {
                                    string json = "{\"recognized\":[\"menuprincipal\"]}";
                                    var exNot = LCE.ExtensionNotification("", "", 0, json);
                                    if (homeScreenConf >= 0.98f) //&& homeScreenConf <= 1.0f) // Intervalo de confiança para evitar envio de vários JSON
                                    {
                                        MMIC.Send(exNot);
                                        altHomeScreen = true;
                                        altMute = false;
                                        altAnterior = false;
                                        altSeguinte = false;
                                        altReproduzir = false;
                                        altParar = false;
                                    }
                                }
                            }
                            else if (muteDetected)
                            {
                                if (!altMute){
                                    string json = "{\"recognized\":[\"volume\",\"mutevolume\"]}";
                                    var exNot = LCE.ExtensionNotification("", "", 0, json);
                                    if (muteConf >= 0.85f) //&& muteConf <= 0.85f) // Intervalo de confiança para evitar envio de vários JSON
                                    {
                                        MMIC.Send(exNot);
                                        altHomeScreen = false;
                                        altMute = true;
                                        altAnterior = false;
                                        altSeguinte = false;
                                        altReproduzir = false;
                                        altParar = false;
                                    }
                                }
                            }
                            else if (anteriorDetected)
                            {
                                if (!altAnterior)
                                {
                                    string json = "{\"recognized\":[\"musicaanterior\"]}";
                                    var exNot = LCE.ExtensionNotification("", "", 0, json);
                                    if (anteriorConf >= 0.60f)// && retrocederConf <= 0.85f)
                                    {
                                        MMIC.Send(exNot);
                                        altHomeScreen = false;
                                        altMute = false;
                                        altAnterior = true;
                                        altSeguinte = false;
                                        altReproduzir = false;
                                        altParar = false;
                                    }
                                }
                            }
                            else if (seguinteDetected)
                            {
                                if (!altSeguinte)
                                {
                                    string json = "{\"recognized\":[\"musicaseguinte\"]}";
                                    var exNot = LCE.ExtensionNotification("", "", 0, json);
                                    if (seguinteConf >= 0.75f) //&& avancarConf <= 0.85f)
                                    {
                                        MMIC.Send(exNot);
                                        altHomeScreen = false;
                                        altMute = false;
                                        altAnterior = false;
                                        altSeguinte = true;
                                        altReproduzir = false;
                                        altParar = false;
                                    }
                                }
                            }
                            else if (reproduzirDetected)
                            {
                                if (!altReproduzir)
                                {
                                    string json = "{\"recognized\":[\"reproduzir\"]}";
                                    var exNot = LCE.ExtensionNotification("", "", 0, json);
                                    if (reproduzirConf >= 0.65f) //&& avancarConf <= 0.85f)
                                    {
                                        MMIC.Send(exNot);
                                        altHomeScreen = false;
                                        altMute = false;
                                        altAnterior = false;
                                        altSeguinte = false;
                                        altReproduzir = true;
                                        altParar = false;
                                    }
                                }
                            }
                            else if (pararDetected)
                            {
                                if (!altParar)
                                {
                                    string json = "{\"recognized\":[\"parar\"]}";
                                    var exNot = LCE.ExtensionNotification("", "", 0, json);
                                    if (pararConf >= 0.50f) //&& avancarConf <= 0.85f)
                                    {
                                        MMIC.Send(exNot);
                                        altHomeScreen = false;
                                        altMute = false;
                                        altAnterior = false;
                                        altSeguinte = false;
                                        altReproduzir = false;
                                        altParar = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Handles the TrackingIdLost event for the VisualGestureBuilderSource object
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void Source_TrackingIdLost(object sender, TrackingIdLostEventArgs e)
        {
            // update the GestureResultView object to show the 'Not Tracked' image in the UI
            this.GestureResultView.UpdateGestureResult(false, false, false, false, false, 0.0f, false, false, 0.0f, false, 0.0f, false, 0.0f, false, 0.0f, false, 0.0f);
        }
    }
}
