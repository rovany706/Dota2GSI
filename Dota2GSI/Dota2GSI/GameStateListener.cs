#nullable enable

using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Dota2GSI.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

// ReSharper disable UnusedType.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Dota2GSI
{
    public class GameStateListener : IDisposable
    {
        private readonly ILogger<GameStateListener> logger;
        private readonly HttpListener netListener;
        private readonly AutoResetEvent waitForConnection = new(false);
        private GameState? currentGameState;
        private readonly Serializer serializer = new();
        private readonly CancellationTokenSource cts = new();

        public GameState? CurrentGameState
        {
            get => this.currentGameState;
            private set
            {
                this.currentGameState = value;
                RaiseOnNewGameState();
            }
        }

        /// <summary>
        /// Gets the port that is being listened
        /// </summary>
        public int Port { get; }

        /// <summary>
        /// Returns whether or not the listener is running
        /// </summary>
        public bool Running { get; private set; }

        /// <summary>
        ///  Event for handing a newly received game state
        /// </summary>
        public event EventHandler<GameState?> NewGameState;

        /// <summary>
        /// A GameStateListener that listens for connections on http://localhost:port/
        /// </summary>
        /// <param name="port"></param>
        /// <param name="loggerFactory">Logger</param>
        public GameStateListener(int port, ILoggerFactory? loggerFactory = null)
            : this($"http://127.0.0.1:{port}/", loggerFactory)
        {
        }

        /// <summary>
        /// A GameStateListener that listens for connections to the specified URI
        /// </summary>
        /// <param name="uri">The URI to listen to</param>
        /// <param name="loggerFactory">Logger</param>
        public GameStateListener(string uri, ILoggerFactory? loggerFactory = null)
        {
            this.logger = loggerFactory?.CreateLogger<GameStateListener>()
                          ?? NullLoggerFactory.Instance.CreateLogger<GameStateListener>();

            if (!uri.EndsWith("/"))
                uri += "/";

            var port = GetPortFromUri(uri);
            Port = port;

            this.netListener = new HttpListener();
            this.netListener.Prefixes.Add(uri);
        }

        private static int GetPortFromUri(string uriString)
        {
            try
            {
                var uri = new Uri(uriString);

                return uri.Port;
            }
            catch (Exception)
            {
                throw new ArgumentException("Not a valid URI: " + uriString);
            }
        }

        /// <summary>
        /// Starts listening for GameState requests
        /// </summary>
        public bool Start()
        {
            if (Running) return false;
            
            try
            {
                this.netListener.Start();
            }
            catch (HttpListenerException)
            {
                this.netListener.Close();
                this.logger.LogError("Could not establish connection");
                return false;
            }

            Running = true;

            Task.Run(async () => await Run(this.cts.Token), this.cts.Token);
            
            return true;
        }

        /// <summary>
        /// Stops listening for GameState requests
        /// </summary>
        public void Stop()
        {
            Running = false;
            this.cts.Cancel();
        }

        private async Task Run(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var context = await this.netListener.GetContextAsync();
                var gameState = await ReceiveGameStateAsync(context);
                CurrentGameState = gameState;
            }

            this.netListener.Stop();
        }

        private async Task<GameState?> ReceiveGameStateAsync(HttpListenerContext context)
        {
            try
            {
                var request = context.Request;
                var json = await GetTextFromRequestBodyAsync(request);

                using (var response = context.Response)
                {
                    response.StatusCode = (int)HttpStatusCode.OK;
                    response.StatusDescription = "OK";
                    response.Close();
                }

                return TryDeserializeCurrentGameState(json);
            }
            catch (ObjectDisposedException)
            {
                // Intentionally left blank, when the Listener is closed.
            }

            return null;
        }

        private static async Task<string> GetTextFromRequestBodyAsync(HttpListenerRequest request)
        {
            await using var inputStream = request.InputStream;
            using var sr = new StreamReader(inputStream);

            return await sr.ReadToEndAsync();
        }

        private GameState? TryDeserializeCurrentGameState(string json)
        {
            try
            {
                return this.serializer.Deserialize<GameState>(json);
            }
            catch (Exception e)
            {
                this.logger.LogError("Deserialization failed:\n{e}\n{json}", e, json);
                return null;
            }
        }

        private void RaiseOnNewGameState()
        {
            foreach (var d in NewGameState.GetInvocationList())
            {
                if (d.Target is ISynchronizeInvoke invoker)
                    invoker.BeginInvoke(d, new object[] { this, CurrentGameState });
                else
                    d.DynamicInvoke(this, CurrentGameState);
            }
        }

        public void Dispose()
        {
            Stop();
            this.waitForConnection.Dispose();
            this.netListener.Close();
            GC.SuppressFinalize(this);
        }
    }
}