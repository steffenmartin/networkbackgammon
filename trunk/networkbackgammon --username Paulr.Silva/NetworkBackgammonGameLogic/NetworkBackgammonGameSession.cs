using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using NetworkBackgammonLib;

namespace NetworkBackgammonGameLogic
{
    [Serializable]
    public class NetworkBackgammonGameSession : INetworkBackgammonNotifier, INetworkBackgammonListener
    {
        #region Declarations

        /// <summary>
        /// Container for collecting data posted via events (asynchronous)
        /// </summary>
        [Serializable]
        public class EventQueueElement
        {
            #region Members

            /// <summary>
            /// Event that has been posted
            /// </summary>
            INetworkBackgammonEvent gameSessionEvent;

            /// <summary>
            /// Notifier (sender) who posted the event
            /// </summary>
            INetworkBackgammonNotifier gameSessionNotifier;

            #endregion

            #region Methods

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="_event">Event that has been posted</param>
            /// <param name="_notifier">Notifier (sender) who posted the event</param>
            public EventQueueElement(INetworkBackgammonEvent _event, INetworkBackgammonNotifier _notifier)
            {
                gameSessionEvent = _event;
                gameSessionNotifier = _notifier;
            }

            #endregion

            #region Properties

            /// <summary>
            /// Gets the event that has been posted
            /// </summary>
            public INetworkBackgammonEvent Event
            {
                get
                {
                    return gameSessionEvent;
                }
            }

            /// <summary>
            /// Gets the notifier (sender) who posted the event
            /// </summary>
            public INetworkBackgammonNotifier Notifier
            {
                get
                {
                    return gameSessionNotifier;
                }
            }

            #endregion
        }

        /// <summary>
        /// Enumeration of (possible) states of the game session state machine
        /// </summary>
        enum GameSessionState
        {
            InitialDiceRoll,
            InitialDiceRollAcknowledgeExpected,
            MoveExpected,
            GameWon
        };

        #endregion

        #region Members

        /// <summary>
        /// Instance of the default notifier implementation
        /// </summary>
        /// <remarks>
        /// Created and set during construction time. Used for delegating function
        /// calls on this class' notifier interface implementation.
        /// </remarks>
        INetworkBackgammonNotifier defaultNotifier = null;

        /// <summary>
        /// Instance of the default listener implementation
        /// </summary>
        /// <remarks>
        /// Used for delegating function calls on this class' listener interface implementation.
        /// </remarks>
        INetworkBackgammonListener defaultListener = new NetworkBackgammonListener();

        /// <summary>
        /// Backgammon player 1
        /// </summary>
        NetworkBackgammonPlayer player1 = null;

        /// <summary>
        /// Backgammon player 2
        /// </summary>
        NetworkBackgammonPlayer player2 = null;

        /// <summary>
        /// Thread, which runs the event based state machine
        /// </summary>
        [NonSerialized]
        Thread threadStateMachine = null;

        /// <summary>
        /// Semaphore to signal events to the state machine (wake-up calls)
        /// </summary>
        /// <remarks>
        /// Starts with an initial count of 1 for performing the initial dice roll.
        /// </remarks>
        Semaphore semStateMachine = new Semaphore(1, 1);

        /// <summary>
        /// Flag to allow shutting down of state machine
        /// </summary>
        bool bStateMachineKeepRunning = true;

        /// <summary>
        /// Queue for collection events and associated data to be processed by the game session state machine
        /// </summary>
        Queue<EventQueueElement> eventQueue = new Queue<EventQueueElement>();

        /// <summary>
        /// The dice for this game session
        /// </summary>
        NetworkBackgammonDice[] dice = new NetworkBackgammonDice[] { new NetworkBackgammonDice(1), new NetworkBackgammonDice(2) };

        #endregion

        #region Methods

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="_player1">Network backgammon player 1 to be part of this game session</param>
        /// <param name="_player2">Network backgammon player 2 to be part of this game session</param>
        public NetworkBackgammonGameSession(NetworkBackgammonPlayer _player1, NetworkBackgammonPlayer _player2)
        {
            defaultNotifier = new NetworkBackgammonNotifier(this);

            player1 = _player1;
            player2 = _player2;
        }

        /// <summary>
        /// Destructor
        /// </summary>
        /// <remarks>
        /// Stops the current game session state machine.
        /// </remarks>
        ~NetworkBackgammonGameSession()
        {
            Stop();
        }

        /// <summary>
        /// Rolls the dice of this game session
        /// </summary>
        private void RollDice()
        {
            foreach (NetworkBackgammonDice d in dice)
            {
                d.Roll();
            }
        }

        /// <summary>
        /// Starts the state machine
        /// </summary>
        public void Start()
        {
            bStateMachineKeepRunning = true;

            threadStateMachine = new Thread(new ThreadStart(Run));

            threadStateMachine.Start();
        }

        /// <summary>
        /// Stops the game session state machine (thread)
        /// </summary>
        public void Stop()
        {
            if (threadStateMachine != null)
            {
                bStateMachineKeepRunning = false;

                // Wake-up state machine to allow for proper shutdown
                // (without forcing an abort)
                semStateMachine.Release();

                if (!threadStateMachine.Join(100))
                {
                    threadStateMachine.Abort();
                }

                threadStateMachine = null;
            }

            eventQueue.Clear();
           
            player1 = null;
            player2 = null;
        }

        /// <summary>
        /// Game session state machine (worker thread)
        /// </summary>
        private void Run()
        {
            GameSessionState currentState = GameSessionState.InitialDiceRoll;

            EventQueueElement eventQueueElement = null;

            // Number of moves left for the active player
            UInt32 activePlayerMovesLeft = 0;
            bool activePlayerMoveDoubles = false;

            while (bStateMachineKeepRunning)
            {
                // Wait for another event
                semStateMachine.WaitOne();

                // HACK: Add function here to deal with the big switch statement 
                if (!bStateMachineKeepRunning) break;

                // Read the event element queue to get information on the next event to be processed
                if (eventQueue.Count > 0)
                {
                    eventQueueElement = eventQueue.Dequeue();
                }

                switch (currentState)
                {
                    case GameSessionState.InitialDiceRoll:
                        {
                            // Use random number generator to figure out which player starts
                            RollDice();

                            // Roll dice until they're both different (no tie)
                            while (dice[0].CurrentValue == dice[1].CurrentValue)
                            {
                                RollDice();
                            }

                            player1.Active = false;
                            player2.Active = false;

                            player1.InitialDice = dice[0];
                            player2.InitialDice = dice[1];

                            Broadcast(new NetworkBackgammonGameSessionEvent(NetworkBackgammonGameSessionEvent.GameSessionEventType.InitialDiceRolled));

                            currentState = GameSessionState.InitialDiceRollAcknowledgeExpected;
                        }
                        break;

                    case GameSessionState.InitialDiceRollAcknowledgeExpected:
                        {
                            if (eventQueueElement != null)
                            {
                                try
                                {
                                    NetworkBackgammonGameSessionEvent gameSessionEvent = (NetworkBackgammonGameSessionEvent)eventQueueElement.Event;

                                    // Latch (flag) acknowledge of initial dice roll from respective player
                                    if (gameSessionEvent.EventType == NetworkBackgammonGameSessionEvent.GameSessionEventType.InitialDiceRolledAcknowledged)
                                    {
                                        NetworkBackgammonPlayer sendingPlayer = (NetworkBackgammonPlayer)eventQueueElement.Notifier;

                                        sendingPlayer.InitialDice.FlagUsed = true;
                                    }

                                    // Check whether both players have acknowledged initial dice roll
                                    if (player1.InitialDice.FlagUsed && player2.InitialDice.FlagUsed)
                                    {
                                        // Determine active player (the one who won the initial dice roll
                                        if (dice[0].CurrentValueUInt32 > dice[1].CurrentValueUInt32)
                                            player1.Active = true;
                                        else
                                            player2.Active = true;

                                        // Roll dice for the first move of active player
                                        RollDice();

                                        // Calculate number of moves left for active player (based on dice values)
                                        activePlayerMoveDoubles = dice[0].CurrentValue == dice[1].CurrentValue;
                                        activePlayerMovesLeft = (UInt32)(activePlayerMoveDoubles ? 4 : 2);

                                        // Calculate possible moves for active player
                                        NetworkBackgammonGameEngine.CalculatePossibleMoves(ref player1, ref player2, activePlayerMoveDoubles ? new NetworkBackgammonDice[] { dice[0] } : dice);
                                        // Send initial checkers with positions (and possible valid moves
                                        // for the active player) to both players
                                        Broadcast(new NetworkBackgammonGameSessionEvent(NetworkBackgammonGameSessionEvent.GameSessionEventType.CheckerUpdated));
                                        // Set next iteration's state
                                        currentState = GameSessionState.MoveExpected;
                                    }
                                }
                                catch (Exception)
                                {
                                    Broadcast(new NetworkBackgammonGameSessionEvent(NetworkBackgammonGameSessionEvent.GameSessionEventType.Error));
                                }
                            }
                        }
                        break;

                    case GameSessionState.MoveExpected:
                        if (eventQueueElement != null)
                        {
                            try
                            {
                                NetworkBackgammonPlayer sendingPlayer = (NetworkBackgammonPlayer)eventQueueElement.Notifier;
                                GameSessionMoveSelectedEvent moveSelectedEvent = (GameSessionMoveSelectedEvent)eventQueueElement.Event;

                                // Check whether the active player attempted to move
                                if (sendingPlayer.Active)
                                {
                                    // Perform the selected move of the active player (and check whether the active player won the game
                                    if (!NetworkBackgammonGameEngine.ExecuteMove(ref player1, ref player2, moveSelectedEvent.CheckerSelected, moveSelectedEvent.MoveSelected))
                                    {
                                        // Figure out the next active player (could be the current
                                        // active player since up to 4 moves are allowed per turn)
                                        if (--activePlayerMovesLeft <= 0)
                                        {
                                            player1.Active = !player1.Active;
                                            player2.Active = !player2.Active;

                                            // Roll dice for active player
                                            RollDice();

                                            // Calculate number of moves left for active player (based on dice values)
                                            activePlayerMoveDoubles = dice[0].CurrentValue == dice[1].CurrentValue;
                                            activePlayerMovesLeft = (UInt32)(activePlayerMoveDoubles ? 4 : 2);
                                        }

                                        // Calculate possible moves for active player
                                        NetworkBackgammonGameEngine.CalculatePossibleMoves(
                                            ref player1,
                                            ref player2,
                                            activePlayerMoveDoubles ?
                                                new NetworkBackgammonDice[] { dice[0] } :
                                                activePlayerMovesLeft == 2 ?
                                                dice :
                                                new NetworkBackgammonDice[] { dice[0] == moveSelectedEvent.MoveSelected ? dice[1] : dice[0] });
                                        // Send updated checkers with positions (and possible valid moves
                                        // for the active player) to both players
                                        Broadcast(new NetworkBackgammonGameSessionEvent(NetworkBackgammonGameSessionEvent.GameSessionEventType.CheckerUpdated));
                                    }
                                    else
                                    {
                                        // Inform both players that the game has been won by the active player
                                    }
                                }
                                else
                                {
                                    Broadcast(new NetworkBackgammonGameSessionEvent(NetworkBackgammonGameSessionEvent.GameSessionEventType.Error));
                                }
                            }
                            catch (Exception)
                            {
                                Broadcast(new NetworkBackgammonGameSessionEvent(NetworkBackgammonGameSessionEvent.GameSessionEventType.Error));
                            }
                        }
                        break;

                    default:
                        break;
                }

                eventQueueElement = null;
            }
        }

        /// <summary>
        /// Determines whether a backgammon player is part of this game session
        /// </summary>
        /// <param name="player">Backgammon player to be checked</param>
        /// <returns>"True" is backgammon player is part of this game session, otherwise "false"</returns>
        public bool ContainsPlayer( NetworkBackgammonPlayer player )
        {
            return ((player == player1) || (player == player2));
        }

        /// <summary>
        /// Get a players opponent
        /// </summary>
        /// <param name="player">Backgammon player to be checked</param>
        /// <returns>null if player does not exist</returns>
        public NetworkBackgammonPlayer GetOpponent(NetworkBackgammonPlayer player)
        {
            NetworkBackgammonPlayer opp = null;

            if (ContainsPlayer(player))
            {
                opp = (player == player1 ? player2 : player1);
            }

            return opp;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the dice of the game session
        /// </summary>
        public NetworkBackgammonDice[] CurrentDice
        {
            get
            {
                return dice;
            }
        }

        /// <summary>
        /// Gets backgammon player 1 of this game session
        /// </summary>
        public NetworkBackgammonPlayer Player1
        {
            get
            {
                return player1;
            }
        }

        /// <summary>
        /// /// Gets backgammon player 2 of this game session
        /// </summary>
        public NetworkBackgammonPlayer Player2
        {
            get
            {
                return player2;
            }
        }

        #endregion

        #region INetworkBackgammonNotifier Members

        public bool AddListener(INetworkBackgammonListener listener)
        {
            return defaultNotifier != null ? defaultNotifier.AddListener(listener) : false;
        }

        public bool RemoveListener(INetworkBackgammonListener listener)
        {
            return defaultNotifier != null ? defaultNotifier.RemoveListener(listener) : false;
        }

        public void Broadcast(INetworkBackgammonEvent notificationEvent)
        {
            defaultNotifier.Broadcast(notificationEvent);
        }

        public void Broadcast(INetworkBackgammonEvent notificationEvent, INetworkBackgammonNotifier notifier)
        {
            defaultNotifier.Broadcast(notificationEvent, notifier);
        }

        #endregion

        #region INetworkBackgammonListener Members

        public bool AddNotifier(INetworkBackgammonNotifier notifier)
        {
            return defaultListener.AddNotifier(notifier);
        }

        public bool RemoveNotifier(INetworkBackgammonNotifier notifier)
        {
            return defaultListener.RemoveNotifier(notifier);
        }

        public void OnEventNotification(INetworkBackgammonNotifier sender, INetworkBackgammonEvent e)
        {
            // Filter out our own broadcasts
            if (sender != this)
            {
                eventQueue.Enqueue(new EventQueueElement(e, sender));

                semStateMachine.Release();
            }
        }

        #endregion
    }
}
