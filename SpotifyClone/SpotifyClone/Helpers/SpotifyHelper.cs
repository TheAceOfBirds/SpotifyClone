using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Http;
using Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Diagnostics;


namespace SpotifyClone.Helpers
{
    public class SpotifyHelper
    {
        private string ClientId { get; set; }
        public string PlaylistId { get; set; }
        public string ClientSecret { get; set; }

        private readonly IAPIConnector _apiConnector;
        private static EmbedIOAuthServer _server;
        private SpotifyClient client;
        public SpotifyHelper(string clientId, string playlistId, string clientSecret)
        {
            this.ClientId = clientId;
            this.PlaylistId = playlistId;
            this.ClientSecret = clientSecret;
            ServerInit();
            while(client == null)
            {
                Thread.Sleep(500);
            }
        }

        public async Task<bool> ServerInit()
        {
            try
            {
                // Make sure "http://localhost:8888/callback" is in your spotify application as redirect uri!
                _server = new EmbedIOAuthServer(new Uri("http://localhost:8888/callback"), 8888);
                await _server.Start();

                _server.AuthorizationCodeReceived += OnAuthorizationCodeReceived;
                _server.ErrorReceived += OnErrorReceived;

                Type t = typeof(Scopes);
                var request = new LoginRequest(_server.BaseUri, ClientId, LoginRequest.ResponseType.Code)
                {
                    Scope = t.GetFields().Select(x => (string)x.GetRawConstantValue()).ToList()
                };
                BrowserUtil.Open(request.ToUri());
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Aborting task \'ServerInit\', error received: {e.Message}");
                return false;
            }

        }
        private async Task OnAuthorizationCodeReceived(object sender, AuthorizationCodeResponse response)
        {
            try
            {
                await _server.Stop();

                var config = SpotifyClientConfig.CreateDefault();
                var tokenResponse = await new OAuthClient(config).RequestToken(
                  new AuthorizationCodeTokenRequest(
                    ClientId, ClientSecret, response.Code, new Uri("http://localhost:8888/callback")
                  )
                );
                client = new SpotifyClient(tokenResponse.AccessToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Aborting task  'OnAuthorizationCodeReceived', error received: {ex.Message}");
            }
        }
        private static async Task OnErrorReceived(object sender, string error, string state)
        {
            Console.WriteLine($"Aborting authorization, error received: {error}");
            await _server.Stop();
        }
        public List<FullTrack>  GetPlayList()
        {
            try
            {
                List< FullTrack> tracks = new List<FullTrack>();
                PlaylistGetItemsRequest playlistGetItemsRequest = new PlaylistGetItemsRequest();

                //spotify limits the total items of its playlists to 100, but the "Total" field works as intended
                int? playlistLength = client.Playlists.GetItems(PlaylistId).Result.Total;
                int? finalLoopLimit = playlistLength % 100;
                for (int i = 1; i<playlistLength; i=i+100)
                {
                    playlistGetItemsRequest.Offset = i-1;
                    playlistGetItemsRequest.Limit = 100;
                    if (playlistLength - (i+100) <0)
                    {
                        playlistGetItemsRequest.Limit = finalLoopLimit;
                    }
                    tracks.AddRange(client.Playlists.GetItems(PlaylistId, playlistGetItemsRequest, default).Result.Items.Select(x => (FullTrack)x.Track));

                }
                return tracks;
            }
            catch(Exception e)
            {
                Console.WriteLine($"Aborting task \'GetPlayList\', error received: {e.Message}");
                return null;
            }
        }

        public List<FullTrack> GetQueue()
        {
            try
            {
                List<IPlayableItem> QueueList = client.Player.GetQueue().Result.Queue.ToList();
                return QueueList.Select(x => (FullTrack)x).ToList();
            }
            catch(Exception e)
            {
                Console.WriteLine($"Aborting task \'GetQueue\', error received: {e.Message}");
                return null;
            }
        }
        public bool ShuffleQueue()
        {
            try
            {
                PlayerShuffleRequest request = new PlayerShuffleRequest(false);
                client.Player.SetShuffle(request);
                request = new PlayerShuffleRequest(true);
                Thread.Sleep(500);
                client.Player.SetShuffle(request);
                Thread.Sleep(500);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Aborting task \'ShuffleQueue\', error received: {e.Message}");
                return false;
            }
        }
    }
}
