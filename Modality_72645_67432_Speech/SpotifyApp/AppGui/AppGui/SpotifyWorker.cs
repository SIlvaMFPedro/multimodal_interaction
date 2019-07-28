using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using mmisharp;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace AppGui
{
    class SpotifyWorker
    {
        private TTS tts;
        private String base_url;
        private IWebDriver web_driver;

        public SpotifyWorker()
        {
            tts = new TTS();
            base_url = "https://open.spotify.com/";
            web_driver = new ChromeDriver("C:\\Users\\silva\\OneDrive - Universidade de Aveiro\\IM\\Modality_72645_67432\\SpotifyApp\\AppGui\\AppGui");
        }

        public void sample_Url()
        {
            goToURL(base_url);
        }

        public void goToURL(String url)
        {
            web_driver.Manage().Window.Maximize();
            web_driver.Navigate().GoToUrl(url);
        }

        public void goToHomeFolder()
        {
            String url = base_url;
            goToURL(url);

            Thread.Sleep(2000);
        }

        public void browseFeatured()
        {
            String url = base_url + "browse/featured";
            goToURL(url);

            Thread.Sleep(2000);

        }

        public void browsePodcasts()
        {
            String url = base_url + "browse/podcasts";
            goToURL(url);

            Thread.Sleep(2000);

        }

        public void browseCharts()
        {
            String url = base_url + "browse/charts";
            goToURL(url);

            Thread.Sleep(2000);

        }

        public void browseGenreMoods()
        {
            String url = base_url + "browse/genres";
            goToURL(url);

            Thread.Sleep(2000);

        }

        public void browseNewReleases()
        {
            String url = base_url + "browse/newreleases";
            goToURL(url);

            Thread.Sleep(2000);

        }

        public void browseDiscover()
        {
            String url = base_url + "browse/discover";
            goToURL(url);

            Thread.Sleep(2000);

        }

        public void goToLibrary()
        {
            String url = base_url + "collection/";
            goToURL(url);

            Thread.Sleep(2000);

        }

        public void showPlaylists()
        {
            String url = base_url + "collection/playlists";
            goToURL(url);

            Thread.Sleep(2000);

        }

        public void showMadeForYou()
        {
            String url = base_url + "collection/made-for-you";
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

        public void createPlayList(String name)
        {
            try
            {
                IWebElement elem_button = web_driver.FindElement(By.ClassName("widget-new-playlist"));
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

        public void search(String str)
        {
            String url = base_url + "search/recent" + str;
            goToURL(url);

            Thread.Sleep(2000);

        }



    }
}
