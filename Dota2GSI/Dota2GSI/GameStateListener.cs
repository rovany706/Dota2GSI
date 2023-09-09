using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;

namespace Dota2GSI
{
    public delegate void NewGameStateHandler(GameState gameState);

    public class GameStateListener : IDisposable
    {
        private bool isRunning;
        private int connectionPort;
        private HttpListener netListener;
        private AutoResetEvent waitForConnection = new(false);
        private GameState currentGameState;
        private Thread _listenerThread;

        public GameState CurrentGameState
        {
            get
            {
                return currentGameState;
            }
            private set
            {
                currentGameState = value;
                RaiseOnNewGameState();
            }
        }

        /// <summary>
        /// Gets the port that is being listened
        /// </summary>
        public int Port { get { return connectionPort; } }

        /// <summary>
        /// Returns whether or not the listener is running
        /// </summary>
        public bool Running { get { return isRunning; } }

        /// <summary>
        ///  Event for handing a newly received game state
        /// </summary>
        public event EventHandler<GameState> NewGameState;

        /// <summary>
        /// A GameStateListener that listens for connections on http://localhost:port/
        /// </summary>
        /// <param name="Port"></param>
        public GameStateListener(int Port)
        {
            connectionPort = Port;
            netListener = new HttpListener();
            netListener.Prefixes.Add($"http://127.0.0.1:{Port}/");
        }

        /// <summary>
        /// A GameStateListener that listens for connections to the specified URI
        /// </summary>
        /// <param name="URI">The URI to listen to</param>
        public GameStateListener(string URI)
        {
            if (!URI.EndsWith("/"))
                URI += "/";

            var URIPattern = new Regex(@"^https?:\/\/.+:([0-9]*)\/$", RegexOptions.IgnoreCase);
            var PortMatch = URIPattern.Match(URI);

            if (!PortMatch.Success)
                throw new ArgumentException("Not a valid URI: " + URI);

            connectionPort = Convert.ToInt32(PortMatch.Groups[1].Value);

            netListener = new HttpListener();
            netListener.Prefixes.Add(URI);
        }

        /// <summary>
        /// Starts listening for GameState requests
        /// </summary>
        public bool Start()
        {
            if (isRunning) return false;
            
            _listenerThread = new Thread(Run);
            try
            {
                netListener.Start();
            }
            catch (HttpListenerException)
            {
                netListener.Close();
                return false;
            }
            isRunning = true;

            // Set this to true, so when the program wants to terminate,
            // this thread won't stop the program from exiting.
            _listenerThread.IsBackground = true;

            _listenerThread.Start();
            return true;

        }

        /// <summary>
        /// Stops listening for GameState requests
        /// </summary>
        public void Stop()
        {
            isRunning = false;
        }

        private void Run()
        {
            while (isRunning)
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
                CurrentGameState = new GameState(json);
            }
            catch (ObjectDisposedException)
            {
                // Intentionally left blank, when the Listener is closed.
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
        }
    }
}
