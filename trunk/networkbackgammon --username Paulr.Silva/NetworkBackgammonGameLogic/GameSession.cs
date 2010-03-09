using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace NetworkBackgammonGameLogic
{
    public class GameSession : GameSessionSubject, IGameSessionListener
    {
        // Thread which runs the event based state machine
        Thread threadStateMachine = null;
        // Semaphore to signal events to the state machine (wake-up calls)
        Semaphore semStateMachine = new Semaphore(1, 1);
        // Flag to allow shutting down of state machine
        bool bStateMachineKeepRunning = true;

        public class GameSessionEventQueueElement
        {
            private GameSessionEvent gameSessionEvent;
            private GameSessionSubject gameSessionSubject;
            private IPlayerEventInfo playerEventInfo;

            public GameSessionEventQueueElement(GameSessionEvent _event, GameSessionSubject _subject, IPlayerEventInfo _playerInfo)
            {
                gameSessionEvent = _event;
                gameSessionSubject = _subject;
                playerEventInfo = _playerInfo;
            }

            public GameSessionEvent Event
            {
                get
                {
                    return gameSessionEvent;
                }
            }

            public GameSessionSubject Subject
            {
                get
                {
                    return gameSessionSubject;
                }
            }

            public IPlayerEventInfo PlayerInfo
            {
                get
                {
                    return playerEventInfo;
                }
            }
        }

        Queue<GameSessionEventQueueElement> eventQueue = new Queue<GameSessionEventQueueElement>();

        // The dice for this Game Session
        Dice []dice = new Dice[] {new Dice(1), new Dice(2)};

        public Dice[] CurrentDice
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

        public GameSession(Player _player1, Player _player2)
        {
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
        ~GameSession()
        {
            Stop();
        }

        private void RollDice()
        {
            foreach (Dice d in dice)
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

            GameSessionEventQueueElement eventQueueElement = null;

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
                        GameEngine.CalculatePossibleMoves(ref player1, ref player2, activePlayerMoveDoubles ? new Dice[] { dice[0] } : dice);
                        // Send initial checkers with positions (and possible valid moves
                        // for the active player) to both players
                        Broadcast(new GameSessionEvent(GameSessionEvent.GameSessionEventType.CheckerUpdated), this);
                        // Set next iteration's state
                        currentState = GameSessionState.MoveExpected;
                        break;

                    case GameSessionState.MoveExpected:
                        if (eventQueueElement != null)
                        {
                            try
                            {
                                Player sendingPlayer = (Player)eventQueueElement.Subject;

                                // Check whether the active player attempted to move
                                if (sendingPlayer.Active)
                                {
                                    // Perform the selected move of the active player (and check whether the active player won the game
                                    if (!GameEngine.ExecuteMove(ref player1, ref player2, eventQueueElement.PlayerInfo.GetSelectedChecker(), eventQueueElement.PlayerInfo.GetSelectedMove()))
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
                                        GameEngine.CalculatePossibleMoves(
                                            ref player1,
                                            ref player2,
                                            activePlayerMoveDoubles ?
                                                new Dice[] { dice[0] } :
                                                activePlayerMovesLeft == 2 ?
                                                dice :
                                                new Dice[] { dice[0] == eventQueueElement.PlayerInfo.GetSelectedMove() ? dice[1] : dice[0] });
                                        // Send updated checkers with positions (and possible valid moves
                                        // for the active player) to both players
                                        Broadcast(new GameSessionEvent(GameSessionEvent.GameSessionEventType.CheckerUpdated), this);
                                    }
                                    else
                                    {
                                        // Inform both players that the game has been won by the active player
                                    }
                                }
                                else
                                {
                                    Broadcast(new GameSessionEvent(GameSessionEvent.GameSessionEventType.Error), this);
                                }
                            }
                            catch (Exception ex)
                            {
                                Broadcast(new GameSessionEvent(GameSessionEvent.GameSessionEventType.Error), this);
                            }
                        }
                        break;

                    default:
                        break;
                }

                eventQueueElement = null;
            }
        }

        private Player player1 = null;
        private Player player2 = null;

        public Player Player1
        {
            get
            {
                return player1;
            }
        }
        public Player Player2
        {
            get
            {
                return player2;
            }
        }

        #region IGameSessionListener Members

        public void Notify(GameSessionEvent _event, GameSessionSubject _subject, IPlayerEventInfo _playerInfo)
        {
            // Filter out our own broadcasts
            if (_subject != this)
            {
                eventQueue.Enqueue(new GameSessionEventQueueElement(_event, _subject, _playerInfo));

                semStateMachine.Release();
            }
        }

        #endregion
    }
}
