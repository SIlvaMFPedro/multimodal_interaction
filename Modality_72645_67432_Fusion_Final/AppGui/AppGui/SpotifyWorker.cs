using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using mmisharp;

using OpenQA.Selenium;
using OpenQA.Selenium.Internal;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Chrome;

namespace AppGui
{
    class SpotifyWorker
    {
        private TTS tts;
        private String base_url;
        private IWebDriver web_driver;

        private int volume_percentage;

        private String anzol_ID = "00xcrNRhDx1HDdpfhnnDhI";
        private String setemares_ID = "6VyE2O7F3wBoDbs9LuxM60";
        private String engate_ID = "6h6Lb92WzzhHXOpZ9GZ0nH";
        private String cavalos_ID = "4FEm0mzYXKngGDlWFXmmLu";
        private String circo_ID = "00ILPz7zUhws80p3Yp10mG";
        private String paixao_ID = "0npmxblyvUaGYRtRo8JuSe";

        public SpotifyWorker()
        {
            this.tts = new TTS();
            base_url = "https://open.spotify.com/";
            //change directory for your chrome driver
            //web_driver = new ChromeDriver("C:\\Users\\filip\\Desktop\\Curso\\IM\\Fusion\\wetransfer-8250d0\\AppGui");
            web_driver = new ChromeDriver("D:\\Modality_72645_67432_Fusion_Final\\AppGui\\AppGui");

            goToURL("https://accounts.spotify.com/en/login");
            //goToURL(base_url);
            this.volume_percentage = 10; //the volume percentage has to be small apparently
        }

        public void sample_Url()
        {
            goToURL(base_url);
        }

        public void goToURL(String url)
        {
            //web_driver.Manage().Window.Maximize(); // Deu erro aqui
            web_driver.Navigate().GoToUrl(url);
        }

        public void goToHomeFolder()
        {
            tts.Speak("Apresentando a página principal.");
            String url = base_url;
            goToURL(url);

            Thread.Sleep(2000);
        }

        public void browseFeatured()
        {
            tts.Speak("Apresentando relevantes");
            String url = base_url + "browse/featured";
            goToURL(url);

            Thread.Sleep(2000);

        }

        public void browsePodcasts()
        {
            tts.Speak("Apresentando podcasts");
            String url = base_url + "browse/podcasts";
            goToURL(url);

            Thread.Sleep(2000);

        }

        public void browseCharts()
        {
            tts.Speak("Apresentando cartas");
            String url = base_url + "browse/charts";
            goToURL(url);

            Thread.Sleep(2000);

        }

        public void browseGenreMoods()
        {
            tts.Speak("Apresentando géneros");
            String url = base_url + "browse/genres";
            goToURL(url);

            Thread.Sleep(2000);

        }

        public void browseNewReleases()
        {
            tts.Speak("Apresentando novos lançamentos");
            String url = base_url + "browse/newreleases";
            goToURL(url);

            Thread.Sleep(2000);

        }

        public void browseDiscover()
        {
            tts.Speak("Descobrindo música");
            String url = base_url + "browse/discover";
            goToURL(url);

            Thread.Sleep(2000);

        }

        public void goToLibrary()
        {
            tts.Speak("Apresentando a sua colecção");
            String url = base_url + "collection/";
            goToURL(url);

            Thread.Sleep(2000);

        }

        public void showPlaylists()
        {
            tts.Speak("Apresentando as suas listas");
            String url = base_url + "collection/playlists";
            goToURL(url);

            Thread.Sleep(2000);

        }

        public void showMadeForYou()
        {
            tts.Speak("Apresentando feito para si");
            String url = base_url + "made-for-you";
            goToURL(url);

            Thread.Sleep(2000);

        }

        public void showSongs()
        {
            String url = base_url + "collection/tracks";
            goToURL(url);

            Thread.Sleep(2000);

        }

        public void showAlbuns()
        {
            String url = base_url + "collection/albums";
            goToURL(url);

            Thread.Sleep(2000);
        }

        public void showArtists()
        {
            String url = base_url + "collection/artists";
            goToURL(url);

            Thread.Sleep(2000);
        }

        public void showPodcasts()
        {
            String url = base_url + "collection/podcasts";
            goToURL(url);

            Thread.Sleep(2000);

        }

        public void createPlayList()
        {
            try
            {
                String url = base_url + "collection/playlists";
                goToURL(url);
                Thread.Sleep(2000);

                IWebElement elem_button = web_driver.FindElement(By.ClassName("asideButton-button"));
                elem_button.Click();
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("InvalidOperationException error");
                return;
            }
            catch (NoSuchElementException e)
            {
                Console.WriteLine("NoSuchElementException error");
                return;

            }

        }

        public void playSong()
        {
            try
            {
                tts.Speak("Reproduzindo a música");
                IWebElement elem_button = web_driver.FindElement(By.ClassName("spoticon-play-16"));
                elem_button.Click();
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("InvalidOperationException e");
                return;
            }
            catch (NoSuchElementException e)
            {
                Console.WriteLine("NoSuchElementException error");
                return;
            }
        }

        public void pauseSong()
        {
            try
            {
                tts.Speak("Pausando a música");
                IWebElement elem_button = web_driver.FindElement(By.ClassName("spoticon-pause-16"));
                elem_button.Click();
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("InvalidOperationException e");
                return;
            }
            catch (NoSuchElementException e)
            {
                Console.WriteLine("NoSuchElementException error");
                return;
            }
        }

        public void previousSong()
        {
            try
            {
                tts.Speak("Música Anterior");
                IWebElement elem_button = web_driver.FindElement(By.ClassName("spoticon-skip-back-16"));
                elem_button.Click();
                elem_button.Click(); //double-click 
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("InvalidOperationException e");
                return;
            }
            catch (NoSuchElementException e)
            {
                Console.WriteLine("NoSuchElementException error");
                return;
            }
        }

        public void goBackSong()
        {
            try
            {
                tts.Speak("Retrocendendo na Música");
                IWebElement elem_button = web_driver.FindElement(By.ClassName("spoticon-skip-back-16"));
                elem_button.Click();
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("InvalidOperationException e");
                return;
            }
            catch (NoSuchElementException e)
            {
                Console.WriteLine("NoSuchElementException error");
                return;
            }
            
        }

        public void nextSong()
        {
            try
            {
                
                tts.Speak("Avançando a Música");
                IWebElement elem_button = web_driver.FindElement(By.ClassName("spoticon-skip-forward-16"));
                elem_button.Click();
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("InvalidOperationException e");
                return;
            }
            catch (NoSuchElementException e)
            {
                Console.WriteLine("NoSuchElementException error");
                return;
            }
        }

        public void repeatSong()
        {
            try
            {
                tts.Speak("Repetindo a Música");
                IWebElement elem_button = web_driver.FindElement(By.ClassName("spoticon-repeat-16"));
                elem_button.Click();
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("InvalidOperationException e");
                return;
            }
            catch (NoSuchElementException e)
            {
                Console.WriteLine("NoSuchElementException error");
                return;
            }
        }

        public void repeatSongOnce()
        {
            try
            {
                tts.Speak("Repetindo a Música");
                IWebElement elem_button = web_driver.FindElement(By.ClassName("spoticon-repeat-16"));
                elem_button.Click();
                elem_button.Click(); //double-click
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("InvalidOperationException e");
                return;
            }
            catch (NoSuchElementException e)
            {
                Console.WriteLine("NoSuchElementException error");
                return;
            }

        }

        public void unrepeatSong()
        {
            try
            {
                tts.Speak("Repetir Desactivado");
                IWebElement elem_button = web_driver.FindElement(By.ClassName("spoticon-repeat-16"));
                elem_button.Click();
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("InvalidOperationException e");
                return;
            }
            catch (NoSuchElementException e)
            {
                Console.WriteLine("NoSuchElementException error");
                return;
            }
        }

        public void unrepeatSongOnce() {
            try
            {
                tts.Speak("Repetir Desactivado");
                IWebElement elem_button = web_driver.FindElement(By.ClassName("spoticon-repeatonce-16"));
                elem_button.Click();
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("InvalidOperationException e");
                return;
            }
            catch (NoSuchElementException e)
            {
                Console.WriteLine("NoSuchElementException error");
                return;
            }
        }

        public void shuffleSong()
        {
            try
            {
                tts.Speak("Modo aleatório");
                IWebElement elem_button = web_driver.FindElement(By.ClassName("spoticon-shuffle-16"));
                elem_button.Click();
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("InvalidOperationException e");
                return;
            }
            catch (NoSuchElementException e)
            {
                Console.WriteLine("NoSuchElementException error");
                return;
            }
        }

        public void showSongInQueue()
        {
            try
            {
                tts.Speak("Músicas em Fila de Espera");
                IWebElement elem_button = web_driver.FindElement(By.ClassName("spoticon-queue-16"));
                elem_button.Click();
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("InvalidOperationException e");
                return;
            }
            catch (NoSuchElementException e)
            {
                Console.WriteLine("NoSuchElementException error");
                return;
            }
        }

        public void addSongToLibrary()
        {
            try
            {
                tts.Speak("Adicionada Música à Biblioteca");
                IWebElement elem_button = web_driver.FindElement(By.ClassName("spoticon-add-16"));
                elem_button.Click();
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("InvalidOperationException e");
                return;
            }
            catch (NoSuchElementException e)
            {
                Console.WriteLine("NoSuchElementException error");
                return;
            }
        }

        /*
        public void playSongNow()
        {
            try
            {
                tts.Speak("Reproduzindo Música");
                IWebElement elem_button = web_driver.FindElement(By.ClassName("spoticon-add-16"));
                elem_button.Click();
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("InvalidOperationException e");
                return;
            }
            catch (NoSuchElementException e)
            {
                Console.WriteLine("NoSuchElementException error");
                return;
            }
        }

        public void playSongLaterInQueue()
        {
            try
            {
                tts.Speak("Reproduzindo Música em fila de espera");
                IWebElement elem_button = web_driver.FindElement(By.ClassName("spoticon-add-16"));
                elem_button.Click();
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("InvalidOperationException e");
                return;
            }
            catch (NoSuchElementException e)
            {
                Console.WriteLine("NoSuchElementException error");
                return;
            }
        }
        */

        public void search(String str)
        {
            tts.Speak("Resultado da pesquisa");
            String url = base_url + "search/results/" + str;
            goToURL(url);

            Thread.Sleep(2000);
        }

        public void playAnzol()
        {
            tts.Speak("Boa escolha.");
            String url = base_url + "track/" + anzol_ID;
            goToURL(url);

            Thread.Sleep(2000);
        }
        public void playSeteMares()
        {
            tts.Speak("A dar Sete Mares");
            String url = base_url + "track/" + setemares_ID;
            goToURL(url);

            Thread.Sleep(2000);
        }
        public void playEngate()
        {
            tts.Speak("Reproduzindo canção de engate por António Variações");
            String url = base_url + "track/" + engate_ID;
            goToURL(url);

            Thread.Sleep(2000);
        }
        public void playCavalos()
        {
            tts.Speak("Com certeza");
            String url = base_url + "track/" + cavalos_ID;
            goToURL(url);

            Thread.Sleep(2000);
        }
        public void playCirco()
        {
            tts.Speak("Xutos, sempre.");
            String url = base_url + "track/" + circo_ID;
            goToURL(url);

            Thread.Sleep(2000);
        }
        public void playPaixao()
        {
            tts.Speak("Paixão de Heróis do Mar");
            String url = base_url + "track/" + paixao_ID;
            goToURL(url);

            Thread.Sleep(2000);
        }

        public void VolumeUp()
        {
            try
            {
               if(this.volume_percentage >= 100)
               {
                    tts.Speak("O volume já está no maximo");
               }
               else
               {
                    this.volume_percentage += 5;
                    //this.volume_percentage++;
                    //var sliderHandle = web_driver.FindElement(By.XPath("//*[@id='middle-align.progress-bar__bg.progress-bar__fg']"));
                    var sliderHandle = web_driver.FindElement(By.ClassName("volume-bar"));
                    //var sliderTrack = web_driver.FindElement(By.XPath("//*[@id='middle-align.progress-bar__bg.progress-bar__slider']"));
                    var sliderTrack = web_driver.FindElement(By.ClassName("volume-bar"));
                    var width = int.Parse(sliderTrack.GetCssValue("width").Replace("px", ""));
                    var dx = (int)(this.volume_percentage / 100.0 * width);
                    Actions action = new Actions(web_driver);

                    action.DragAndDropToOffset(sliderHandle, dx, 0).Build().Perform();

                    Console.WriteLine("Volume Actual: " + this.volume_percentage);
               }
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("InvalidOperationException e");
                return;
            }
            catch (NoSuchElementException e)
            {
                Console.WriteLine("NoSuchElementException error");
                return;
            }

        }

        public void VolumeDown()
        {
            try
            {
                if (this.volume_percentage <= 0)
                {
                    tts.Speak("O volume já está no mínimo");
                }
                else
                {
                    this.volume_percentage -= 5;
                    var sliderHandle = web_driver.FindElement(By.ClassName("volume-bar"));
                    var sliderTrack = web_driver.FindElement(By.ClassName("volume-bar"));
                    var width = int.Parse(sliderTrack.GetCssValue("width").Replace("px", ""));
                    var dx = (int)(this.volume_percentage / 100.0 * width);
                    Actions action = new Actions(web_driver);

                    action.DragAndDropToOffset(sliderHandle, dx, 0).Build().Perform();

                    Console.WriteLine("Volume Actual: " + this.volume_percentage);
                }
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("InvalidOperationException e");
                return;
            }
            catch (NoSuchElementException e)
            {
                Console.WriteLine("NoSuchElementException error");
                return;
            }
        }

        public void MuteVolume()
        {
            try
            {
                tts.Speak("Retirando o Som");
                IWebElement elem_button = web_driver.FindElement(By.ClassName("volume-bar__icon"));
                elem_button.Click();
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("InvalidOperationException e");
                return;
            }
            catch (NoSuchElementException e)
            {
                Console.WriteLine("NoSuchElementException error");
                return;
            }
        }

        public void ResetVolume()
        {
            try
            {
                tts.Speak("Repor o Som");
                IWebElement elem_button = web_driver.FindElement(By.ClassName("volume-bar__icon"));
                elem_button.Click();
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("InvalidOperationException e");
                return;
            }
            catch (NoSuchElementException e)
            {
                Console.WriteLine("NoSuchElementException error");
                return;
            }

        }

        public void MinVolume()
        {
            try
            {
                if (this.volume_percentage == 0)
                {
                    tts.Speak("O volume já está no mínimo possível!");
                }
                else
                {
                    this.volume_percentage = 0;
                    var sliderHandle = web_driver.FindElement(By.ClassName("volume-bar"));
                    var sliderTrack = web_driver.FindElement(By.ClassName("volume-bar"));
                    var width = int.Parse(sliderTrack.GetCssValue("width").Replace("px", ""));
                    var dx = (int)(this.volume_percentage / 100.0 * width);
                    Actions action = new Actions(web_driver);

                    action.DragAndDropToOffset(sliderHandle, dx, 0).Build().Perform();
                    tts.Speak("Minimizando o som");
                    Console.WriteLine("Volume Actual: " + this.volume_percentage);
                }
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("InvalidOperationException e");
                return;
            }
            catch (NoSuchElementException e)
            {
                Console.WriteLine("NoSuchElementException error");
                return;
            }
        }

        public void MaxVolume()
        {
            try
            {
                if (this.volume_percentage == 100)
                {
                    tts.Speak("O volume já está no máximo possível!");
                }
                else
                {
                    this.volume_percentage = 100;
                    var sliderHandle = web_driver.FindElement(By.ClassName("volume-bar"));
                    var sliderTrack = web_driver.FindElement(By.ClassName("volume-bar"));
                    var width = int.Parse(sliderTrack.GetCssValue("width").Replace("px", ""));
                    var dx = (int)(this.volume_percentage / 100.0 * width);
                    Actions action = new Actions(web_driver);

                    action.DragAndDropToOffset(sliderHandle, dx, 0).Build().Perform();
                    tts.Speak("Maximizando o som");
                    Console.WriteLine("Volume Actual: " + this.volume_percentage);
                }
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("InvalidOperationException e");
                return;
            }
            catch (NoSuchElementException e)
            {
                Console.WriteLine("NoSuchElementException error");
                return;
            }
        }

        public void PutVolumeTo30()
        {
            try
            {
                this.volume_percentage = 30;
                var sliderHandle = web_driver.FindElement(By.ClassName("volume-bar"));
                var sliderTrack = web_driver.FindElement(By.ClassName("volume-bar"));
                var width = int.Parse(sliderTrack.GetCssValue("width").Replace("px", ""));
                var dx = (int)(this.volume_percentage / 100.0 * width);
                Actions action = new Actions(web_driver);

                action.DragAndDropToOffset(sliderHandle, dx, 0).Build().Perform();

                Console.WriteLine("Volume Actual: " + this.volume_percentage);
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("InvalidOperationException e");
                return;
            }
            catch (NoSuchElementException e)
            {
                Console.WriteLine("NoSuchElementException error");
                return;
            }

        }

        public void PutVolumeTo50()
        {
            try
            {
                this.volume_percentage = 50;
                var sliderHandle = web_driver.FindElement(By.ClassName("volume-bar"));
                var sliderTrack = web_driver.FindElement(By.ClassName("volume-bar"));
                var width = int.Parse(sliderTrack.GetCssValue("width").Replace("px", ""));
                var dx = (int)(this.volume_percentage / 100.0 * width);
                Actions action = new Actions(web_driver);

                action.DragAndDropToOffset(sliderHandle, dx, 0).Build().Perform();

                Console.WriteLine("Volume Actual: " + this.volume_percentage);
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("InvalidOperationException e");
                return;
            }
            catch (NoSuchElementException e)
            {
                Console.WriteLine("NoSuchElementException error");
                return;
            }
        }

        public void PutVolumeTo80()
        {
            try
            {
                this.volume_percentage = 80;
                var sliderHandle = web_driver.FindElement(By.ClassName("volume-bar"));
                var sliderTrack = web_driver.FindElement(By.ClassName("volume-bar"));
                var width = int.Parse(sliderTrack.GetCssValue("width").Replace("px", ""));
                var dx = (int)(this.volume_percentage / 100.0 * width);
                Actions action = new Actions(web_driver);

                action.DragAndDropToOffset(sliderHandle, dx, 0).Build().Perform();

                Console.WriteLine("Volume Actual: " + this.volume_percentage);
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("InvalidOperationException e");
                return;
            }
            catch (NoSuchElementException e)
            {
                Console.WriteLine("NoSuchElementException error");
                return;
            }

        }

        public void quitSpotify()
        {
            tts.Speak("Fechando a aplicação.");
            web_driver.Quit();

            Thread.Sleep(2000);
        }
    }
}
