using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;
using Microsoft.Speech.AudioFormat;
using Microsoft.Speech.Synthesis;

namespace kinectModality
{
    class TTS
    {
        SpeechSynthesizer tts = null;
        static SoundPlayer player = new SoundPlayer();

        /*
         * Text to Speech constructor
         */
        public TTS()
        {
            Console.WriteLine("TTS constructor called");

            //create new speech synthesizer
            tts = new SpeechSynthesizer();

            //show voices
            //initialize a new instance of the SpeechSynthesizer
            using (SpeechSynthesizer synth = new SpeechSynthesizer())
            {
                //output information for all installed voices
                Console.WriteLine("Installed voices -> ");
                foreach (InstalledVoice voice in synth.GetInstalledVoices())
                {
                    VoiceInfo voice_info = voice.VoiceInfo;
                    string AudioFormats = "";
                    foreach (SpeechAudioFormatInfo format in voice_info.SupportedAudioFormats)
                    {
                        AudioFormats += String.Format("{0}\n", format.EncodingFormat.ToString());
                        format.EncodingFormat.ToString();
                    }

                    //write voice information
                    Console.WriteLine("<--------- VOICE INFO --------->");
                    Console.WriteLine("Name: " + voice_info.Name);
                    Console.WriteLine("Culture: " + voice_info.Culture);
                    Console.WriteLine("Age: " + voice_info.Age);
                    Console.WriteLine("Gender: " + voice_info.Gender);
                    Console.WriteLine("Description: " + voice_info.Description);
                    Console.WriteLine("ID: " + voice_info.Id);
                    Console.WriteLine("Enabled: " + voice.Enabled);

                    //write voice audio formats
                    if (voice_info.SupportedAudioFormats.Count != 0)
                    {
                        Console.WriteLine("Audio Formats: " + AudioFormats);
                    }
                    else
                    {
                        Console.WriteLine("No Supported Audio Formats Found!");
                    }

                    //write additional information
                    string add_info = "";
                    foreach (string key in voice_info.AdditionalInfo.Keys)
                    {
                        add_info += String.Format("{0}:{1}\n", key, voice_info.AdditionalInfo[key]);
                    }
                    Console.WriteLine("Additional Information -> " + add_info);
                    Console.WriteLine();
                }
            }
            //quit 
            //Console.WriteLine("Press any key to quit...");
            //Console.ReadKey();

            //set voice
            tts.SelectVoiceByHints(VoiceGender.Male, VoiceAge.NotSet, 0, new System.Globalization.CultureInfo("pt-PT"));

            //set voice function to play audio
            tts.SpeakCompleted += new EventHandler<SpeakCompletedEventArgs>(tts_SpeakCompleted);
        }

        /*
        * tts_SpeakCompleted
        */
        void tts_SpeakCompleted(object sender, SpeakCompletedEventArgs e)
        {
            if (player.Stream != null)
            {
                //play voice stream
                player.Stream.Position = 0;
                player.Play();
                player.Stream = null;
            }
        }

        /*
         * Speak
         * 
         * @param text - text to convert
         */
        public void Speak(string text)
        {
            while (player.Stream != null)
            {
                Console.WriteLine("Waiting for player voice stream...");
            }

            //create audio stream for voice stream
            player.Stream = new System.IO.MemoryStream();
            tts.SetOutputToWaveStream(player.Stream);
            tts.SpeakAsync(text);
        }

        /*
         * Speak
         * 
         * @param text - text to convert
         */
        public void Speak(string text, int sample_rate)
        {
            Console.WriteLine("Speak method called, version with sample rate parameter");

            while (player.Stream != null)
            {
                Console.WriteLine("Waiting for player stream...");
            }

            //create audio stream for voice stream
            player.Stream = new System.IO.MemoryStream();
            tts.SetOutputToWaveStream(player.Stream);
            tts.Rate = sample_rate;
            //tts.SpeakAsync(text);
            tts.SpeakSsmlAsync(text);
        }

        public void ChangeGenderVoice(VoiceGender vg = VoiceGender.NotSet)
        {
            tts.SelectVoiceByHints(vg, VoiceAge.NotSet, 0, new System.Globalization.CultureInfo("pt-PT"));
        }

        public Boolean CheckInstalledGenderVoice(string gender)
        {
            bool check_gender = false;
            // Initialize new instance of SpeechSynthesizer;
            using (SpeechSynthesizer synth = new SpeechSynthesizer())
            {
                // Output information for all installed voices;
                foreach (InstalledVoice voice in synth.GetInstalledVoices())
                {
                    VoiceInfo voice_info = voice.VoiceInfo;
                    if (voice_info.Gender.Equals(gender) || voice_info.Gender.ToString().Equals(gender))
                    {
                        check_gender = true;
                    }
                    else
                    {
                        check_gender = false;
                    }
                }
            }
            return check_gender;
        }
    }
}
