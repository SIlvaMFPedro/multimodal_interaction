using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Text;
using System.Threading.Tasks;

using SpotifyAPI.Local;


namespace AppGui
{
    class SpotifyLocalWorker
    {
        private SpotifyLocalAPI _spotify_local_api;
        private string _currentPlaySong = "";
        private String currentPlayArtistID = null;

        public SpotifyLocalWorker()
        {
            SpotifyConnect();
            SpotifyUpdateTrack();
        }

        public void CheckSpotify()
        {
            if (!SpotifyLocalAPI.IsSpotifyInstalled()) //make sure the spotify app is installed.
            {
                MessageBox.Show(@"Spotify is not installed in the Windows System! Application will now close.", @"Not Running", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(0);
            }
            if (!SpotifyLocalAPI.IsSpotifyRunning()) //make sure the spotify app is running
            {
                MessageBox.Show(@"Spotify application is not running! Application will now close.", @"Not Running", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(0);
            }
            if (SpotifyLocalAPI.IsSpotifyWebHelperRunning()) //make sure the webhelper is running
            {
                return;
            }
            //MessageBox.Show(@"Spotify is not running! Application will now close.", @"Not Running", MessageBoxButton.OK, MessageBoxImage.Error);
            //Environment.Exit(0);
        }

        public bool SpotifyConnected()
        {
            try
            {
                return _spotify_local_api.Connect();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + @" Please restart Spotify Desktop Application.", @"Connection Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(0);
                return false;
            }
        }

        public void SpotifyConnect()
        {
            //var config = new SpotifyLocalAPIConfig { Port = 4371, HostUrl = "https://127.0.0.1" };
            //_spotify_local_api = new SpotifyLocalAPI(config);


            _spotify_local_api = new SpotifyLocalAPI();
            var status = _spotify_local_api.GetStatus();
            Console.WriteLine(status);

            //_spotify_local_api = new SpotifyLocalAPI();
            CheckSpotify();
            if (!SpotifyConnected())
            {
                MessageBox.Show(@"Error connecting to Spotify Desktop Application! Application will now close.", @"Connection Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(0);
            }
            else
            {
                MessageBox.Show(@"Connection to Spotify Desktop Application sucessfull!", @"Spotify", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            _spotify_local_api.ListenForEvents = true; //listen for events in spotify application
        }

        public void SpotifyUpdateTrack()
        {
            _spotify_local_api.OnTrackChange += _spotify_OnTrackChange;

        }

        public void _spotify_OnTrackChange(object Sender, TrackChangeEventArgs Event)
        {
            try
            {
                _currentPlaySong = Event.NewTrack.TrackResource.Name + " de " + Event.NewTrack.ArtistResource.Name;
                Console.WriteLine("Changing track - OnTrackChange");
                this.currentPlayArtistID = Event.NewTrack.ArtistResource.Uri.Split(new string[] { ":" }, StringSplitOptions.None)[2];
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void getMyPlayLists(List<String> play_lst)
        {
            try
            {
                String tmp_play_lst = null;
                foreach (String playlist in play_lst)
                {
                    tmp_play_lst += " - " + playlist.ToUpper() + "\n";
                }
                MessageBox.Show(@"" + tmp_play_lst, @"As suas Playlists são:", MessageBoxButton.OK);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public async void PlayTrack()
        {
            try
            {
                await _spotify_local_api.Play();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public async void PauseTrack()
        {
            try
            {
                await _spotify_local_api.Pause();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void NextTrack()
        {
            try
            {
                _spotify_local_api.Skip();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void PreviousTrack()
        {
            try
            {
                _spotify_local_api.Previous();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public float GetVolume()
        {
            try
            {
                return _spotify_local_api.GetSpotifyVolume();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return 1.0F;
            }
        }

        public void SetVolume(float volume)
        {
            try
            {
                _spotify_local_api.SetSpotifyVolume(volume);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void MuteVolume()
        {
            try
            {
                _spotify_local_api.Mute();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public Boolean MutedSpotify()
        {
            try
            {
                return _spotify_local_api.IsSpotifyMuted();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
        }

        public void UnMuteVolume()
        {
            try
            {
                if (MutedSpotify())
                {
                    _spotify_local_api.UnMute();
                }
                else
                {
                    Console.WriteLine("Spotify Desktop Application is not muted!\n");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public Boolean IsPlayingSong()
        {
            try
            {
                return _spotify_local_api.GetStatus().Playing;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
        }

        public string GetSongArtist()
        {
            try
            {
                return string.IsNullOrEmpty(_spotify_local_api.GetStatus().Track.ArtistResource.Name) ? "" : _spotify_local_api.GetStatus().Track.ArtistResource.Name;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }

        public string GetSongTrack()
        {
            try
            {
                return string.IsNullOrEmpty(_spotify_local_api.GetStatus().Track.TrackResource.Name) ? "" : _spotify_local_api.GetStatus().Track.TrackResource.Name;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }

        public String getSongTrackInfo()
        {
            try
            {
                String track_name = _spotify_local_api.GetStatus().Track.TrackResource.Name;
                String artist_name = _spotify_local_api.GetStatus().Track.ArtistResource.Name;
                String album_name = _spotify_local_api.GetStatus().Track.AlbumResource.Name;
                String str = "Música: " + track_name + "; Artista: " + artist_name + "; Album: " + album_name + "\n";
                return str;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }

        public string GetPlayingSong()
        {
            try
            {
                return !string.IsNullOrEmpty(_currentPlaySong) ? _currentPlaySong : $@"{GetSongTrack()} de {GetSongArtist()}";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
        } 
        
        public bool RepeatStatus()
        {
            try
            {
                return _spotify_local_api.GetStatus().Repeat;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
        }

        public void RepeatTrack()
        {
            try
            {
                if (!RepeatStatus())
                {
                    _spotify_local_api.GetStatus().Repeat = true;
                }
                else
                {
                    Console.WriteLine("Repeat Toggle is already on!\n");
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void UnRepeatTrack()
        {
            try
            {
                if (RepeatStatus())
                {
                    _spotify_local_api.GetStatus().Repeat = false;
                }
                else
                {
                    Console.WriteLine("Repeat toggle is not on!\n");
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void PlayMyPlayListByID(String play_lst_ID)
        {
            try
            {
                String URL = "https://open.spotify.com/user/" + play_lst_ID;
                Console.WriteLine(URL);
                _spotify_local_api.PlayURL(URL);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            try
            {
                String URL = "https://open.spotify.com/user/spotify/" + play_lst_ID;
                Console.WriteLine(URL);
                _spotify_local_api.PlayURL(URL);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void PlayFeaturedPlayListByID(String play_lst_ID)
        {
            try
            {
                String URL = "https://open.spotify.com/user/spotify/playlist/" + play_lst_ID;
                Console.WriteLine(URL);
                _spotify_local_api.PlayURL(URL);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void PlayMyArtistByID(String artist_ID)
        {
            try
            {
                String URL = "https://open.spotify.com/artist/" + artist_ID;
                Console.WriteLine(URL);
                _spotify_local_api.PlayURL(URL, "");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public String getPlayingContentID(String content)
        {
            try
            {
                switch (content)
                {
                    case "Artist":
                        return _spotify_local_api.GetStatus().Track.ArtistResource.Uri.Split(new string[] { ":" }, StringSplitOptions.None)[2];
                    case "Track":
                        return _spotify_local_api.GetStatus().Track.TrackResource.Uri.Split(new string[] { ":" }, StringSplitOptions.None)[2];
                    default:
                        return _spotify_local_api.GetStatus().Track.ArtistResource.Uri.Split(new string[] { ":" }, StringSplitOptions.None)[2];
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }

        public String getCurrentPlayArtistID()
        {
            try
            {
                return this.currentPlayArtistID;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }

        public string getCurrentPlaySong()
        {
            try
            {
                return this._currentPlaySong;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }
    }
}
