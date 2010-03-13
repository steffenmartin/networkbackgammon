using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using NetworkBackgammonLib;

namespace NetworkBackgammonGameLogic
{
    public class NetworkBackgammonGameSession : INetworkBackgammonNotifier, INetworkBackgammonListener
    {
        INetworkBackgammonNotifier defaultNotifier = null;
        INetworkBackgammonListener defaultListener = new NetworkBackgammonListener();
        NetworkBackgammonPlayer player1 = null;
        NetworkBackgammonPlayer player2 = null;
        // Thread which runs the event based state machine
        Thread threadStateMachine = null;
        // Semaphore to signal events to the state machine (wake-up calls)
        Semaphore semStateMachine = new Semaphore(1, 1);
        // Flag to allow shutting down of state machine
        bool bStateMachineKeepRunning = true;

        public class EventQueueElement
        {
            INetworkBackgammonEvent gameSessionEvent;
            INetworkBackgammonNotifier gameSessionNotifier;

            public EventQueueElement(INetworkBackgammonEvent _event, INetworkBackgammonNotifier _notifier)
            {
                gameSessionEvent = _event;
                gameSessionNotifier = _notifier;
            }

            public INetworkBackgammonEvent Event
            {
                get
                {
                    return gameSessionEvent;
                }
            }

            public INetworkBackgammonNotifier Notifier
            {
                get
                {
                    return gameSessionNotifier;
                }
            }
        }

        Queue<EventQueueElement> eventQueue = new Queue<EventQueueElement>();

        // The dice for this Game Session
        NetworkBackgammonDice[] dice = new NetworkBackgammonDice[] { new NetworkBackgammonDice(1), new NetworkBackgammonDice(2) };

        public NetworkBackgammonDice[] CurrentDice
        {
            get
            {
                return dice;
            }
        }

        enum GameSessionState
        {
            InitialDiceRoll,
            MoveExpected,
            GameWon
        };

        public NetworkBackgammonGameSession(NetworkBackgammonPlayer _player1, NetworkBackgammonPlayer _player2)
        {
            defaultNotifier = new NetworkBackgammonNotifier(this);

            player1 = _player1;
            // Game Session is listening for events from Player 1
            player1.AddListener(this);
            // Player 1 is listening for events from Game Session
            AddListener(player1);

            player2 = _player2;
            // Game Session is listening for events form Player 2
            player2.AddListener(this);
            // Player 2 is listening for events from Game Session
            AddListener(player2);
        }

        ~NetworkBackgammonGameSession()
        {
            Stop();
        }

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
        /// State Machine (worker thread)
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

                // Read the event element queue to get information on the next event to be processed
                if (eventQueue.Count > 0)
                {
                    eventQueueElement = eventQueue.Dequeue();
                }

                switch (currentState)
                {
                    case GameSessionState.InitialDiceRoll:
                        // Use random number generator to figure out which player starts
                        RollDice();

                        // Roll dice until they're both different (no tie)
                        while (dice[0].CurrentValue == dice[1].CurrentValue)
                        {
                            RollDice();
                        }

                        player1.Active = false;
                        player2.Active = false;

                        if (dice[0].CurrentValueUInt32 > dice[1].CurrentValueUInt32)
                            player1.Active = true;
                        else
                            player2.Active = true;

                        // Roll dice for the first move of active player
                        RollDice();

                        // Calculate number of moves left for active player (based on dice values)
                        activePlayerMoveDoubles = dice[0].CurrentValue == dice[1].CurrentValue;
                        activePlayerMovesLeft = (UInt32) (activePlayerMoveDoubles ? 4 : 2);

                        // Calculate possible moves for active player
                        NetworkBackgammonGameEngine.CalculatePossibleMoves(ref player1, ref player2, activePlayerMoveDoubles ? new NetworkBackgammonDice[] { dice[0] } : dice);
                        // Send initial checkers with positions (and possible valid moves
                        // for the active player) to both players
                        Broadcast(new NetworkBackgammonGameSessionEvent(NetworkBackgammonGameSessionEvent.GameSessionEventType.CheckerUpdated));
                        // Set next iteration's state
                        currentState = GameSessionState.MoveExpected;
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
                            catch (Exception ex)
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

        public bool ContainsPlayer( NetworkBackgammonPlayer player )
        {
            return ((player == player1) || (player == player2));
        }

        public NetworkBackgammonPlayer Player1
        {
            get
            {
                return player1;
            }
        }
        public NetworkBackgammonPlayer Player2
        {
            get
            {
                return player2;
            }
        }

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
