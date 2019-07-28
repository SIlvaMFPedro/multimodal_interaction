using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Speech.Recognition;
using mmisharp;

namespace kinectModality
{
    class SpeechMod
    {
        private SpeechRecognitionEngine SRE;
        private Grammar gr;
        public event EventHandler<SpeechEventArg> Recognized;
        protected virtual void onRecognized(SpeechEventArg msg)
        {
            EventHandler<SpeechEventArg> handler = Recognized;
            if (handler != null)
            {
                handler(this, msg);
            }
        }

        private LifeCycleEvents LCE;
        private MmiCommunication MMIC;

        public SpeechMod()
        {
            //Initialize LifeCycleEvents.
            LCE = new LifeCycleEvents("ASR", "IM", "speech-1", "acoustic", "command");
            MMIC = new MmiCommunication("localhost", 8000, "User1", "ASR");

            MMIC.Send(LCE.NewContextRequest());

            //load pt recognizer
            SRE = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("pt-PT"));
            gr = new Grammar(Environment.CurrentDirectory + "\\ptG.grxml", "basicCmd");
            SRE.LoadGrammar(gr);

            SRE.SetInputToDefaultAudioDevice();
            SRE.RecognizeAsync(RecognizeMode.Multiple);
            SRE.SpeechRecognized += SRE_SpeechRecognized;
            SRE.SpeechHypothesized += SRE_SpeechHypothesized;
        }

        private void SRE_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            onRecognized(new SpeechEventArg() { Text = e.Result.Text, Confidence = e.Result.Confidence, Final = true });

            //SEND JSON MESSAGE
            string json = "{";
            foreach (var resultSemantic in e.Result.Semantics)
            {
                json += "\"" + resultSemantic.Key + "\":\"" + resultSemantic.Value.Value + "\", ";
            }
            json.Substring(0, json.Length - 2);
            json += " }";

            var exNot = LCE.ExtensionNotification(e.Result.Audio.StartTime + "", e.Result.Audio.StartTime.Add(e.Result.Audio.Duration) + "", e.Result.Confidence, json);
            if (e.Result.Confidence > 0.85)
            {
                Console.WriteLine(exNot);
                MMIC.Send(exNot);
            }

        }

        private void SRE_SpeechHypothesized(object sender, SpeechHypothesizedEventArgs e)
        {
            onRecognized(new SpeechEventArg() { Text = e.Result.Text, Confidence = e.Result.Confidence, Final = false });
        }
    }
}
