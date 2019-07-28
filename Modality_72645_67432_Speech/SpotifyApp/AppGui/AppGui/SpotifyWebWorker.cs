using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Globalization;
using System.Threading.Tasks;

using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Models;
using SpotifyAPI.Web.Enums;


namespace AppGui
{
    class SpotifyWebWorker
    {
        private static SpotifyWebAPI _spotify_web_api;
        private static String client_ID = "557247a76446458994a3abca03345843";
        private String profile_ID;
        private int currentPlayListCount = 0;

        private PrivateProfile _my_profile;
        private List<SimplePlaylist> _my_play_lsts;
        private Dictionary<String, String> _my_play_lsts_dicts;

        public async void run_API_Authentication()
        {
            Console.WriteLine("Run Authentication Configuration...");

            WebAPIFactory web_api_factory = new WebAPIFactory("http://localhost", 9000, client_ID,
                                                               Scope.UserReadPrivate | Scope.UserReadEmail | Scope.PlaylistReadPrivate | Scope.UserLibraryRead |
                                                               Scope.UserReadPrivate | Scope.UserFollowRead | Scope.UserReadBirthdate | Scope.UserTopRead | Scope.PlaylistReadCollaborative |
                                                               Scope.UserReadRecentlyPlayed | Scope.UserReadPlaybackState | Scope.UserModifyPlaybackState);
            try
            {
                _spotify_web_api = await web_api_factory.GetWebApi();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            if (_spotify_web_api == null)
            {
                return;                         //If web api is null repeat authentication process;
            }
        }

        private async void Initial_Setup()
        {
            try
            {
                Console.WriteLine("Initial Setup Configuration.");
                _my_profile = await _spotify_web_api.GetPrivateProfileAsync();
                this.profile_ID = _my_profile.Id;

                _my_play_lsts = GetMyPlayLists();
                _my_play_lsts.ForEach(playlist => this._my_play_lsts_dicts[playlist.Name] = playlist.Id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private int incrementPlayListCount()
        {
            return ++this.currentPlayListCount;
        }

        private int decrementPlayListCount()
        {
            return ++this.currentPlayListCount;
        }

        public SpotifyWebWorker()
        {
            _my_play_lsts_dicts = new Dictionary<string, string>();
            run_API_Authentication();
            Initial_Setup();
        }

        public List<SimplePlaylist> GetMyPlayLists()
        {
            try
            {
                Paging<SimplePlaylist> play_lst_pag = _spotify_web_api.GetUserPlaylists(_my_profile.Id);
                List<SimplePlaylist> playlist = play_lst_pag.Items.ToList();

                while(play_lst_pag.Next != null)
                {
                    play_lst_pag = _spotify_web_api.GetUserPlaylists(_my_profile.Id, 20, play_lst_pag.Offset + play_lst_pag.Limit);
                    playlist.AddRange(play_lst_pag.Items);
                }
                return playlist;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }

        public List<String> GetMyPlayListsUris()
        {
            List<String> play_lst_IDs = null;
            try
            {
                List<SimplePlaylist> play_lsts = GetMyPlayLists();
                play_lst_IDs = new List<string>();

                foreach (SimplePlaylist sp in play_lsts)
                {
                    String play_lst_Uri = sp.Uri;
                    play_lst_Uri = play_lst_Uri.Replace("spotify:user:", "").Replace(":", "/");
                    play_lst_IDs.Add(play_lst_Uri);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return play_lst_IDs;
        }

        public Dictionary<String, String> GetMyPlayListDictionary()
        {
            Dictionary<String, String> tmp_my_play_lst_dict = null;
            try
            {
                tmp_my_play_lst_dict = new Dictionary<string, string>();
                List<SimplePlaylist> _my_play_lsts = GetMyPlayLists();
                _my_play_lsts.ForEach(playlist => tmp_my_play_lst_dict[playlist.Name] = playlist.Uri);

                foreach (SimplePlaylist sp in _my_play_lsts)
                {
                    String play_list_Uri = sp.Uri;
                    play_list_Uri = play_list_Uri.Replace("spotify:user:", "").Replace(":", "/");
                    tmp_my_play_lst_dict[Remove_Diacritics(sp.Name)] = play_list_Uri;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return tmp_my_play_lst_dict;
        }

        public string Remove_Diacritics(string txt)
        {
            if (string.IsNullOrWhiteSpace(txt))
            {
                return txt;
            }
            txt = txt.Normalize(NormalizationForm.FormD);
            var chars = txt.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray();
            return new string(chars).Normalize(NormalizationForm.FormC);
        }

        public int GetTargetPlayList(String op)
        {
            try
            {
                List<String> playlists_Uris = GetMyPlayListsUris();
                int count = (playlists_Uris.Count) - 1;
                if (count == -1)
                {
                    return 0;
                }
                switch (op)
                {
                    case "NextTrack":
                        if (incrementPlayListCount() <= count)
                        {
                            return currentPlayListCount;
                        }
                        else
                        {
                            currentPlayListCount = 0; //reset playlist counter;
                            return currentPlayListCount;
                        }
                    case "PreviousTrack":
                        if (decrementPlayListCount() >= 0)
                        {
                            return currentPlayListCount;
                        }
                        else
                        {
                            currentPlayListCount = count;
                            return currentPlayListCount;
                        }
                    default:
                        return 0;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return 0;
            }
        }

        public String GetFeaturedPlayLists()
        {
            try
            {
                FeaturedPlaylists feat_play_lsts = _spotify_web_api.GetFeaturedPlaylists(null, "PT");
                List<String> play_lsts_IDs = new List<string>();
                feat_play_lsts.Playlists.Items.ForEach(playlist => play_lsts_IDs.Add(playlist.Id));

                Random random = new Random();
                int count = play_lsts_IDs.Count - 1;
                int index = 0;
                if(count != -1)
                {
                    index = random.Next(0, play_lsts_IDs.Count - 1);
                }
                return play_lsts_IDs[index];
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }

        public String GetRelatedArtistByID(String artist_ID)
        {
            try
            {
                SeveralArtists artists = _spotify_web_api.GetRelatedArtists(artist_ID);

                Random random = new Random();
                int index = 0;
                int count = artists.Artists.Count - 1;
                if(count != index)
                {
                    index = random.Next(0, artists.Artists.Count - 1);
                }
                return artists.Artists[index].Id;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }
    }
}
