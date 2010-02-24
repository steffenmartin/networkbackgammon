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
        Semaphore semStateMachine = new Semaphore(0, 1);
        // Flag to allow shutting down of state machine
        bool bStateMachineKeepRunning = true;

        public class GameSessionEventQueueElement
        {
            private GameSessionEvent gameSessionEvent;
            private GameSessionSubject gameSessionSubject;

            public GameSessionEventQueueElement(GameSessionEvent _event, GameSessionSubject _subject)
            {
                gameSessionEvent = _event;
                gameSessionSubject = _subject;
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
        }

        Queue<GameSessionEventQueueElement> eventQueue = new Queue<GameSessionEventQueueElement>();

        // The dice for this Game Session
        Dice []dice = new Dice[] {new Dice(), new Dice()};

        enum GameSessionState
        {
            InitialDiceRoll,
            MoveExpected
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

            while (bStateMachineKeepRunning)
            {
                // Wait for another event
                semStateMachine.WaitOne();

                switch (currentState)
                {
                    case GameSessionState.InitialDiceRoll:
                        // Use random number generator to figure out which player starts
                        RollDice();

                        while (dice[0] == dice[1])
                        {
                            RollDice();
                        }

                        player1.Active = false;
                        player2.Active = false;

                        if (dice[0].CurrentValueUInt32 > dice[1].CurrentValueUInt32)
                            player1.Active = true;
                        else
                            player2.Active = true;

                        // Calculate possible moves for active player
                        GameEngine.CalculatePossibleMoves(player1, player2);
                        // Send initial checkers with positions (and possible valid moves
                        // for the active player) to both players
                        Broadcast(new GameSessionEvent(GameSessionEvent.GameSessionEventType.CheckerUpdated), this);
                        // Inform active player to make a move
                        currentState = GameSessionState.MoveExpected;
                        break;

                    case GameSessionState.MoveExpected:
                        // Perform the selected move of the active player
                        // Figure out the next active player (could be the current
                        // active player since up to 4 moves are allowed per turn)
                        // Calculate possible moves for active player
                        // Send updated checkers with positions (and possible valid moves
                        // for the active player) to both players
                        // Inform active player to make a move
                        break;

                    default:
                        break;
                }
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

        public void Notify(GameSessionEvent _event, GameSessionSubject _subject)
        {
            eventQueue.Enqueue(new GameSessionEventQueueElement(_event, _subject));

            semStateMachine.Release();
        }

        #endregion
    }
}
