using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Linq;
using mmisharp;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SpotifyAPI.Web.Models;

namespace AppGui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MmiCommunication mmiC;
        //private SpotifyWorker worker;
        private SpotifyLocalWorker local_worker;
        private SpotifyWebWorker web_worker;

        //constants -> artist_IDs
        private String xutos_ID = "1lQnDEcvFAWaUjbyZiHKih";
        private String GNR_ID = "6zo2w5Asb1DMFZErdRDrrK";
        private String joaopedropais_ID = "3Pjj7heoGNSFE6S3kPQsex";

        private TTS tts;

        public MainWindow()
        {
            /* Start API controllers */
            //this.worker = new SpotifyWorker();
            this.local_worker = new SpotifyLocalWorker();
            this.web_worker = new SpotifyWebWorker();

            /* Start Text to Speech Module - TTS */
            this.tts = new TTS();

            mmiC = new MmiCommunication("localhost", 8000, "User1", "GUI");
            mmiC.Message += MmiC_Message;
            mmiC.Start();

        }

        private void MmiC_Message(object sender, MmiEventArgs e)
        {
            var doc = XDocument.Parse(e.Message);
            dynamic json = null;
            try
            {
                var com = doc.Descendants("command").FirstOrDefault().Value;
                json = JsonConvert.DeserializeObject(com);
            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine(ex.ToString());
            }

            string command = null;
            if (json != null)
            {
                command = json.recognized[1].ToString();
                Console.WriteLine(command);
                Console.WriteLine("Received Command: " + command + "\n");
            }
            else
            {
                Console.WriteLine("Invalid command input!\n");
            }

            int volume = 50;
            List<String> tmp_playlist_IDs = this.web_worker.GetMyPlayListsUris();
            Dictionary<String, String> tmp_playlist_Dict = this.web_worker.GetMyPlayListDictionary();

            switch (command)
            {
                case "REPRODUZIR_MUSICA":
                    if (!this.local_worker.IsPlayingSong())
                    {
                        this.local_worker.PlayTrack();
                    }
                    else
                    {
                        tts.Speak("A música já esta a ser reproduzida!");
                    }
                    break;

                case "PARAR_MUSICA":
                    if (this.local_worker.IsPlayingSong())
                    {
                        this.local_worker.PauseTrack();
                    }
                    else{
                        tts.Speak("Não é possível parar a música!");
                    }
                    break;

                case "AVANÇAR_MUSICA":
                    this.local_worker.NextTrack();
                    //tts.Speak("Avançando esta música!");
                    break;

                case "RETROCEDER_MUSICA":
                    this.local_worker.PreviousTrack();
                    //tts.Speak("Voltando à música anterior!");
                    break;

                case "AUMENTAR_VOLUME":
                    volume = (int)this.local_worker.GetVolume();
                    volume += 10;
                    if (volume <= 100)
                    {
                        Console.WriteLine("Volume Actual: " + volume + "\n");
                        this.local_worker.SetVolume(volume);
                    }
                    else
                    {
                        Console.WriteLine("Volume Actual: " + volume + "\n");
                        tts.Speak("O volume já se encontra no máximo!");
                    }
                    break;

                case "BAIXAR_VOLUME":
                    volume = (int)this.local_worker.GetVolume();
                    volume -= 10;
                    if (volume >= 0)
                    {
                        Console.WriteLine("Volume Actual: " + volume + "\n");
                        this.local_worker.SetVolume(volume);
                    }
                    else
                    {
                        Console.WriteLine("Volume Actual: " + volume + "\n");
                        tts.Speak("O volume já se encontra no mínimo!");
                    }
                    break;

                case "RETIRAR_VOLUME":
                    if (!this.local_worker.MutedSpotify())
                    {
                        this.local_worker.MuteVolume();
                    }
                    else
                    {
                        tts.Speak("O volume já se encontra sem som!");
                    }
                    break;

                case "REPOR_VOLUME":
                    if (this.local_worker.MutedSpotify())
                    {
                        this.local_worker.UnMuteVolume();
                    }
                    else
                    {
                        tts.Speak("O volume já foi reposto!");
                    }
                    break;

                case "METER_VOLUME":
                    dynamic json_msg = null;
                    try
                    {
                        var command_msg = doc.Descendants("command").LastOrDefault().Value;
                        json_msg = JsonConvert.DeserializeObject(command_msg);
                    }
                    catch (NullReferenceException ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }

                    volume = json_msg.volume;
                    Console.WriteLine("Volume: " + volume);
                    this.local_worker.SetVolume(volume);
                    break;

                case "MUSICA_NOME":
                    Console.WriteLine("O nome da música é : " + this.local_worker.GetSongTrack());
                    tts.Speak("O nome da música é " + this.local_worker.GetSongTrack());
                    break;

                case "MUSICA_ARTISTA":
                    Console.WriteLine("O artista da música é : " + this.local_worker.GetSongArtist());
                    tts.Speak("O artista da música é " + this.local_worker.GetSongArtist());
                    break;

                case "MUSICA_ACTUAL":
                    Console.WriteLine("A música actual é : " + this.local_worker.GetPlayingSong());
                    tts.Speak("A música actual é " + this.local_worker.GetPlayingSong());
                    break;

                case "REPETIR_MUSICA":
                    if (!this.local_worker.RepeatStatus())
                    {
                        this.local_worker.RepeatTrack();
                        tts.Speak("Repetir música activado!");
                    }
                    else
                    {
                        this.local_worker.UnRepeatTrack();
                        tts.Speak("Repetir música desactivado!");
                    }
                    break;

                case "REPRODUZIR_PLAYLISTS":
                    if (tmp_playlist_IDs.Count != 0)
                    {
                        this.local_worker.PlayMyPlayListByID(tmp_playlist_IDs.ElementAt(0));
                    }
                    else{
                        tts.Speak("Não existem playlists para reproduzir!");
                    }
                    break;

                case "AVANÇAR_PLAYLIST":
                    if (tmp_playlist_IDs.Count != 0)
                    {
                        this.local_worker.PlayMyPlayListByID(tmp_playlist_IDs.ElementAt(this.web_worker.GetTargetPlayList("NextTrack")));
                    }
                    else
                    {
                        tts.Speak("Não existem playlists para avançar!");
                    }
                    break;

                case "RETROCEDER_PLAYLIST":
                    if (tmp_playlist_IDs.Count != 0)
                    {
                        this.local_worker.PlayMyPlayListByID(tmp_playlist_IDs.ElementAt(this.web_worker.GetTargetPlayList("PreviousTrack")));
                    }
                    else
                    {
                        tts.Speak("Não existem playlists para retroceder!");
                    }
                    break;

                case "MUSICAS_SEMELHANTES":
                    string artist_ID = this.web_worker.GetRelatedArtistByID(this.local_worker.getCurrentPlayArtistID());
                    if (artist_ID == null)
                    {
                        artist_ID = this.web_worker.GetRelatedArtistByID(this.local_worker.getPlayingContentID("Artist"));
                    }
                    Console.WriteLine("Similar Artist ID: " + artist_ID + "\n");
                    this.local_worker.PlayMyArtistByID(artist_ID);
                    break;

                case "INFO_MUSICA_ACTUAL":
                    string music_info = this.local_worker.getSongTrackInfo();
                    tts.Speak("Você está a ouvir " + music_info);
                    break;

                case "OUVIR_XUTOS_E_PONTAPÉS":
                    Console.WriteLine("A ouvir Xutos e Pontapés.\n");
                    this.local_worker.PlayMyArtistByID(this.xutos_ID);
                    //tts.Speak("Você está a ouvir Xutos e Pontapés!");
                    break;

                case "OUVIR_JOAO_PEDRO_PAIS":
                    Console.WriteLine("A ouvir João Pedro Pais...\n");
                    this.local_worker.PlayMyArtistByID(this.joaopedropais_ID);
                    //tts.Speak("Você está a ouvir João Pedro Pais!");
                    break;

                case "OUVIR_GNR":
                    Console.WriteLine("A ouvir GNR...\n");
                    this.local_worker.PlayMyArtistByID(this.GNR_ID);
                    //tts.Speak("Você está a ouvir João Pedro Pais!");
                    break;

                case "OUVIR_NOVIDADES":
                    Console.WriteLine("A ouvir Novidades...\n");
                    String featured_playlist = this.web_worker.GetFeaturedPlayLists();
                    this.local_worker.PlayFeaturedPlayListByID(featured_playlist);
                    break;

                case "OUVIR_PLAYLISTS":
                    List<SimplePlaylist> playlists = this.web_worker.GetMyPlayLists();
                    List<string> name_playlists = new List<string>();
                    playlists.ForEach(playlist => name_playlists.Add(playlist.Name));
                    this.local_worker.getMyPlayLists(name_playlists);
                    break;

                case "MUDAR_VOZ_MASCULINA":
                    tts.ChangeGenderVoice(Microsoft.Speech.Synthesis.VoiceGender.Male);
                    tts.Speak("Voz masculina");
                    break;

                case "MUDAR_VOZ_FEMININA":
                    tts.ChangeGenderVoice(Microsoft.Speech.Synthesis.VoiceGender.Female);
                    tts.Speak("Voz feminina");
                    break;

                default:
                    Console.WriteLine("Invalid message command input! Try again\n");
                    break;
            }

            //switch (command)
            //{
            //    case "menuprincipal":
            //        worker.goToHomeFolder();
            //        break;
            //    case "pesquisar":
            //        worker.search(json.Musica.ToString());
            //        break;

            //    case "navegar":
            //        switch (json.EscolherNavegaçao.ToString())
            //        {
            //            case "relevantes":
            //                worker.browseFeatured();
            //                break;
            //            case "podcast":
            //                worker.browsePodcasts();
            //                break;
            //            case "cartas":
            //                worker.browseCharts();
            //                break;
            //            case "genero":
            //                worker.browseGenreMoods();
            //                break;
            //            case "novo":
            //                worker.browseNewReleases();
            //                break;
            //            case "descoberta":
            //                worker.browseDiscover();
            //                break;
            //        }
            //        break;

            //    case "biblioteca":
            //        worker.goToLibrary();
            //        break;

            //    case "mostrar":
            //        switch (json.EscolherTema.ToString())
            //        {
            //            case "playlists":
            //                worker.showPlaylists();
            //                break;

            //            case "feitasparamim":
            //                worker.showMadeForYou();
            //                break;

            //            case "músicas":
            //                worker.showSongs();
            //                break;

            //            case "albuns":
            //                worker.showAlbuns();
            //                break;

            //            case "artistas":
            //                worker.showArtists();
            //                break;

            //            case "podcasts":
            //                worker.showPodcasts();
            //                break;
            //        }
            //        break;

            //    case "criar":
            //        switch (json.EscolherObjecto.ToString())
            //        {
            //            case "playlists":
            //                worker.createPlayList(json.EscolherPlaylist.ToString());
            //                break;
            //        }
            //        break;
            //}

            //Dynamic Playlists command
            try
            {
                this.local_worker.PlayMyPlayListByID(tmp_playlist_Dict[command]);
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine("Key not found exception!");
            }
        }
    }
}
