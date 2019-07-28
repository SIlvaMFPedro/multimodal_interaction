using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Linq;
using mmisharp;
using System;
using Newtonsoft.Json;

namespace AppGui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MmiCommunication mmiC;
        private SpotifyWorker worker;
        public MainWindow()
        {
            worker = new SpotifyWorker();

            mmiC = new MmiCommunication("localhost", 8000, "User1", "GUI");
            mmiC.Message += MmiC_Message;
            mmiC.Start();

        }

        private void MmiC_Message(object sender, MmiEventArgs e)
        {
            var doc = XDocument.Parse(e.Message);
            var com = doc.Descendants("command").FirstOrDefault().Value;
            dynamic json = JsonConvert.DeserializeObject(com);
            // VER A MENSAGEM JSON PARA PERCEBER ESTRUTURA
            // IMPORTANT TO KEEP THE FORMAT {"recognized":["blabla","blablabla"]}
            System.Threading.Thread.Sleep(100); //sleep de meio segundo
            switch (json.recognized[0].ToString())
            {
                case "menuprincipal":
                    worker.goToHomeFolder();
                    break;
                case "pesquisar":
                    //worker.search(json.Musica.ToString());
                    worker.search(json.recognized[1].ToString());
                    break;

                case "navegar":
                    //switch (json.EscolherNavegaçao.ToString())
                    switch (json.recognized[1].ToString())
                    {
                        case "relevantes":
                            worker.browseFeatured();
                            break;
                        case "podcast":
                            worker.browsePodcasts();
                            break;
                        case "cartas":
                            worker.browseCharts();
                            break;
                        case "genero":
                            worker.browseGenreMoods();
                            break;
                        case "novo":
                            worker.browseNewReleases();
                            break;
                        case "descoberta":
                            worker.browseDiscover();
                            break;
                        case "biblioteca":
                            worker.goToLibrary();
                            break;
                    }
                    break;

                case "adicionar":
                    switch (json.recognized[1].ToString())
                    {
                        case "biblioteca": //add songs to library
                            worker.addSongToLibrary();
                            break;
                        case "fila":    //add songs to wait line -> Not Implemented yet!
                            break;
                        case "playlist": //add songs to my playlist -> Not Implement yet!
                            break;
                    }
                    break;

                case "reproduzir":
                    worker.playSong();
                    break;

                case "parar":
                    worker.pauseSong();
                    break;

                case "retroceder":
                    worker.goBackSong();
                    break;

                case "musicaanterior":
                    worker.previousSong();
                    break;

                case "musicaseguinte":
                    worker.nextSong();
                    break;

                case "repetir":
                    switch (json.recognized[1].ToString())
                    {
                        case "repetir":
                            worker.repeatSong();
                            break;
                        case "naorepetir":
                            worker.unrepeatSong();
                            //worker.unrepeatSongOnce();
                            break;
                        case "repetirumavez":
                            worker.repeatSongOnce();
                            break;
                        case "naorepetirumavez":
                            worker.unrepeatSongOnce();
                            break;
                    }
                    break;

                case "baralhar":
                    worker.shuffleSong();
                    break;

                case "mostrar":
                    //switch (json.EscolherTema.ToString())
                    switch (json.recognized[1].ToString())
                    {
                        case "playlists":
                            worker.showPlaylists();
                            break;

                        case "feitasparamim":
                            worker.showMadeForYou();
                            break;

                        case "musicas":
                            worker.showSongs();
                            break;

                        case "albuns":
                            worker.showAlbuns();
                            break;

                        case "artistas":
                            worker.showArtists();
                            break;

                        case "podcasts":
                            worker.showPodcasts();
                            break;

                        case "fila":
                            worker.showSongInQueue();
                            break;
                    }
                    break;

                case "criar":
                    //switch (json.EscolherObjecto.ToString())
                    switch (json.recognized[1].ToString())
                    {
                        case "playlists":
                            //worker.createPlayList(json.EscolherPlaylist.ToString());
                            worker.createPlayList();
                            break;
                    }
                    break;

                case "tocar":
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        switch (json.recognized[1].ToString())
                        {
                            case "tocarAnzol":
                                //worker.playAnzol();
                                if(json.recognized[2].ToString() == "reproduzir")
                                {
                                    worker.playAnzol();
                                }
                                break;
                            case "tocarSeteMares":
                                worker.playSeteMares();
                                break;
                            case "tocarEngate":
                                //worker.playEngate();
                                if(json.recognized[2].ToString() == "reproduzir")
                                {
                                    worker.playEngate();
                                }
                                break;
                            case "tocarCavalos":
                                worker.playCavalos();
                                break;
                            case "tocarCirco":
                                //worker.playCirco();
                                if(json.recognized[2].ToString() == "reproduzir")
                                {
                                    worker.playCirco();
                                }
                                break;
                            case "tocarPaixao":
                                worker.playPaixao();
                                break;
                        }
                    });
                    break;

                case "volume":
                    switch (json.recognized[1].ToString())
                    {
                        case "volumeup":
                            worker.VolumeUp();
                            break;
                        case "volumedown":
                            worker.VolumeDown();
                            break;
                        case "mutevolume":
                            worker.MuteVolume();
                            break;
                        case "resetvolume":
                            worker.ResetVolume();
                            break;
                        case "minvolume":
                            worker.MinVolume();
                            break;
                        case "maxvolume":
                            worker.MaxVolume();
                            break;
                        case "volume30":
                            worker.PutVolumeTo30();
                            break;
                        case "volume50":
                            worker.PutVolumeTo50();
                            break;
                        case "volume80":
                            worker.PutVolumeTo80();
                            break;
                    }
                    break;

                case "fecharSpotify": 
                    worker.quitSpotify();
                    break;
            }
        }
    }
}
