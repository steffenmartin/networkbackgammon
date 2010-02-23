using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace NetworkBackgammonGameLogic
{
    public class GameSession : IGameSessionListener
    {
        Thread thread = null;
        Semaphore threadSem = new Semaphore(0, 1);
        bool bKeepRunning = true;

        public GameSession(Player _player1, Player _player2)
        {
            player1 = _player1;
            player1.AddListener(this);

            player2 = _player2;
            player2.AddListener(this);
        }

        ~GameSession()
        {
            if (thread != null)
            {
                bKeepRunning = false;

                threadSem.Release();

                if (!thread.Join(100))
                {
                    thread.Abort();
                }

                thread = null;
            }
        }

        public void Start()
        {
            thread = new Thread(new ThreadStart(Run));

            thread.Start();
        }

        private void Run()
        {
            uint diceRolledCount = 0;

            while (bKeepRunning)
            {
                threadSem.WaitOne();

                diceRolledCount++;

                if (diceRolledCount == 2)
                {
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
            switch (_event.EventType)
            {
                case GameSessionEvent.GameSessionEventType.DiceRolled:
                    threadSem.Release();
                    break;
            }
        }

        #endregion
    }
}
