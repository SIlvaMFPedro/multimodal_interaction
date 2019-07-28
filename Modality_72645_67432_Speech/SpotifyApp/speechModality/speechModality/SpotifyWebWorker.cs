using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Enums;
using SpotifyAPI.Web.Models;


namespace speechModality
{
    class SpotifyWebWorker
    {
        private static SpotifyWebAPI _spotify_web_api;
        private static String client_ID = "557247a76446458994a3abca03345843";

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

        public SpotifyWebWorker()
        {
            run_API_Authentication();
        }

        public String GetUserPrivateProfileID()
        {
            try
            {
                PrivateProfile user = _spotify_web_api.GetPrivateProfile();
                return user.Id;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }

        public List<String> GetUserPlayListsByProfileID(String profile_ID)
        {
            Paging<SimplePlaylist> user_playlists_pag = _spotify_web_api.GetUserPlaylists(profile_ID);
            List<SimplePlaylist> user_playlists_lst = user_playlists_pag.Items.ToList();
            List<string> user_playlists_name = new List<string>();

            while (user_playlists_pag.Next != null)
            {
                user_playlists_pag = _spotify_web_api.GetUserPlaylists(profile_ID, 20, user_playlists_pag.Offset + user_playlists_pag.Limit);
                user_playlists_lst.AddRange(user_playlists_pag.Items);
            }

            foreach(SimplePlaylist sp in user_playlists_lst)
            {
                user_playlists_name.Add(sp.Name);
            }

            return user_playlists_name;
        }
    }
}
