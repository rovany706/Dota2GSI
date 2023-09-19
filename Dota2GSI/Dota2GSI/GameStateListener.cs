#nullable enable

using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using Dota2GSI.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

// ReSharper disable UnusedType.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Dota2GSI
{
    public delegate void NewGameStateHandler(GameState gameState);

    public class GameStateListener : IDisposable
    {
        private readonly ILogger<GameStateListener> logger;
        private readonly HttpListener netListener;
        private readonly AutoResetEvent waitForConnection = new(false);
        private GameState? currentGameState;
        private Thread listenerThread;
        private readonly Serializer serializer = new();

        public GameState? CurrentGameState
        {
            get => currentGameState;
            private set
            {
                currentGameState = value;
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

            netListener = new HttpListener();
            netListener.Prefixes.Add(uri);
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

            listenerThread = new Thread(Run);

            try
            {
                netListener.Start();
            }
            catch (HttpListenerException)
            {
                netListener.Close();
                logger.LogError("Could not establish connection");
                return false;
            }

            Running = true;

            // Set this to true, so when the program wants to terminate,
            // this thread won't stop the program from exiting.
            listenerThread.IsBackground = true;

            listenerThread.Start();
            return true;
        }

        /// <summary>
        /// Stops listening for GameState requests
        /// </summary>
        public void Stop()
        {
            Running = false;
        }

        private void Run()
        {
            while (Running)
            {
                netListener.BeginGetContext(ReceiveGameState, netListener);
                waitForConnection.WaitOne();
                waitForConnection.Reset();
            }

            netListener.Stop();
        }

        private void ReceiveGameState(IAsyncResult result)
        {
            try
            {
                var context = netListener.EndGetContext(result);
                var request = context.Request;
                string json;

                waitForConnection.Set();

                using (var inputStream = request.InputStream)
                {
                    using (var sr = new StreamReader(inputStream))
                        json = sr.ReadToEnd();
                }

                using (var response = context.Response)
                {
                    response.StatusCode = (int)HttpStatusCode.OK;
                    response.StatusDescription = "OK";
                    response.Close();
                }

                CurrentGameState = TryDeserializeCurrentGameState(json);
            }
            catch (ObjectDisposedException)
            {
                // Intentionally left blank, when the Listener is closed.
            }
        }

        private GameState? TryDeserializeCurrentGameState(string json)
        {
            try
            {
                return this.serializer.Deserialize<GameState>(json);
            }
            catch (Exception e)
            {
                logger.LogWarning("Deserialization failed:\n{e}", e);
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
            waitForConnection.Dispose();
            netListener.Close();
            GC.SuppressFinalize(this);
        }
    }
}