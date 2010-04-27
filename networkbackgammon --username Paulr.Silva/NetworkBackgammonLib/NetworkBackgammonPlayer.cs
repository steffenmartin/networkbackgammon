using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetworkBackgammonLib;
using System.Xml.Serialization;
using System.IO;

namespace NetworkBackgammonLib
{
    [Serializable]
    public class NetworkBackgammonPlayer : MarshalByRefObject,
                                           INetworkBackgammonNotifier, 
                                           INetworkBackgammonListener
    {
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
        /// List of checkers of this player
        /// </summary>
        List<NetworkBackgammonChecker> checkers = new List<NetworkBackgammonChecker>();

        /// <summary>
        /// Name of this player
        /// </summary>
        string strPlayerName = ""; 

        /// <summary>
        /// Status of this player
        /// </summary>
        bool bActive = false;

        /// <summary>
        /// Initial dice for this player
        /// </summary>
        NetworkBackgammonDice initialDice = null;

        #endregion

        #region Methods

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="_strPlayerName">Name of this player</param>
        public NetworkBackgammonPlayer(string _strPlayerName)
        {
            defaultNotifier = new NetworkBackgammonNotifier(this);

            strPlayerName = _strPlayerName;
        }

        /// <summary>
        /// Destructor
        /// </summary>
        /// <remarks>
        /// For debugging purposes: Try to serialize the checkers of this player
        /// </remarks>
        ~NetworkBackgammonPlayer()
        {
            TextWriter textWriter = null;

            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<NetworkBackgammonChecker>));

                textWriter = new StreamWriter(strPlayerName + ".out.xml");

                xmlSerializer.Serialize(textWriter, checkers);

                textWriter.Close();
            }
            catch (Exception)
            {
            }
            finally
            {
                if (textWriter != null)
                {
                    textWriter.Close();
                }
            }
        }

        /// <summary>
        /// Initializes list of checkers in their initial positions.
        /// </summary>
        /// <remarks>
        /// For debugging purposes: Try to de-serialize players checkers if a respective XML file can be found.
        /// </remarks>
        public void InitCheckers()
        {
            TextReader textReader = null;

            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<NetworkBackgammonChecker>));

                textReader = new StreamReader(strPlayerName + ".in.xml");

                checkers = (List<NetworkBackgammonChecker>)xmlSerializer.Deserialize(textReader);

                textReader.Close();

                if (checkers != null)
                {
                    if (checkers.Count != 15)
                    {
                        throw new Exception();
                    }
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {
                checkers.Clear();

                // 2 checkers on 1
                checkers.Add(new NetworkBackgammonChecker(new NetworkBackgammonPosition(NetworkBackgammonPosition.GameBoardPosition.ONE)));
                checkers.Add(new NetworkBackgammonChecker(new NetworkBackgammonPosition(NetworkBackgammonPosition.GameBoardPosition.ONE)));

                // 5 checkers on 12
                checkers.Add(new NetworkBackgammonChecker(new NetworkBackgammonPosition(NetworkBackgammonPosition.GameBoardPosition.TWELVE)));
                checkers.Add(new NetworkBackgammonChecker(new NetworkBackgammonPosition(NetworkBackgammonPosition.GameBoardPosition.TWELVE)));
                checkers.Add(new NetworkBackgammonChecker(new NetworkBackgammonPosition(NetworkBackgammonPosition.GameBoardPosition.TWELVE)));
                checkers.Add(new NetworkBackgammonChecker(new NetworkBackgammonPosition(NetworkBackgammonPosition.GameBoardPosition.TWELVE)));
                checkers.Add(new NetworkBackgammonChecker(new NetworkBackgammonPosition(NetworkBackgammonPosition.GameBoardPosition.TWELVE)));

                // 3 checkers on 17
                checkers.Add(new NetworkBackgammonChecker(new NetworkBackgammonPosition(NetworkBackgammonPosition.GameBoardPosition.SEVENTEEN)));
                checkers.Add(new NetworkBackgammonChecker(new NetworkBackgammonPosition(NetworkBackgammonPosition.GameBoardPosition.SEVENTEEN)));
                checkers.Add(new NetworkBackgammonChecker(new NetworkBackgammonPosition(NetworkBackgammonPosition.GameBoardPosition.SEVENTEEN)));

                // 5 checkers on 19
                checkers.Add(new NetworkBackgammonChecker(new NetworkBackgammonPosition(NetworkBackgammonPosition.GameBoardPosition.NINETEEN)));
                checkers.Add(new NetworkBackgammonChecker(new NetworkBackgammonPosition(NetworkBackgammonPosition.GameBoardPosition.NINETEEN)));
                checkers.Add(new NetworkBackgammonChecker(new NetworkBackgammonPosition(NetworkBackgammonPosition.GameBoardPosition.NINETEEN)));
                checkers.Add(new NetworkBackgammonChecker(new NetworkBackgammonPosition(NetworkBackgammonPosition.GameBoardPosition.NINETEEN)));
                checkers.Add(new NetworkBackgammonChecker(new NetworkBackgammonPosition(NetworkBackgammonPosition.GameBoardPosition.NINETEEN)));
            }
            finally
            {
                if (textReader != null)
                {
                    textReader.Close();
                }
            }
        }

        /// <summary>
        /// Respond to a game challenge
        /// </summary>
        /// <param name="acceptChallenge">"True" if challenge has been accepted, "false" if challenge is rejected</param>
        public void RespondToChallenge(bool acceptChallenge, string challengingPlayerName)
        {
            Broadcast(new NetworkBackgammonChallengeResponseEvent(acceptChallenge, PlayerName, challengingPlayerName));
        }

        /// <summary>
        /// Acknowledge the initial dice roll
        /// </summary>
        public void AcknowledgeInitialDiceRoll()
        {
            Broadcast(new GameSessionInitialDiceRollAcknowledgeEvent(PlayerName));
        }

        /// <summary>
        /// Make a move
        /// </summary>
        /// <param name="_checkerSelected">(Selected) checker to be a move performed with</param>
        /// <param name="_moveSelected">(Selected) move to be performed</param>
        public void MakeMove(NetworkBackgammonChecker _checkerSelected, NetworkBackgammonDice _moveSelected)
        {
            Broadcast(new GameSessionMoveSelectedEvent(_checkerSelected, _moveSelected));
        }

        /// <summary>
        /// Acknowledges the fact that the player (even though active) has no (valid) moves
        /// </summary>
        public void AcknowledgeNoMoves()
        {
            Broadcast(new GameSessionNoPossibleMovesAcknowledgeEvent(PlayerName));
        }

        /// <summary>
        /// Resign from the current game (forfeit)
        /// </summary>
        public void ResignFromGame()
        {
            Broadcast(new GameSessionPlayerResignationEvent(PlayerName));
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets this player's name
        /// </summary>
        public string PlayerName
        {
            get
            {
                return strPlayerName;
            }
        }

        /// <summary>
        /// Gets this player's status
        /// </summary>
        public bool Active
        {
            get
            {
                return bActive;
            }
            set
            {
                bActive = value;
            }
        }

        /// <summary>
        /// Gets list of checkers of this player
        /// </summary>
        public List<NetworkBackgammonChecker> Checkers
        {
            get
            {
                return checkers;
            }
        }

        /// <summary>
        /// Gets or sets initial dice for this player
        /// </summary>
        public NetworkBackgammonDice InitialDice
        {
            get
            {
                return initialDice;
            }
            set
            {
                initialDice = value;
            }
        }

        /// <summary>
        /// Get the players current pip count
        /// </summary>
        public Int32 PipCount
        {
            get
            {
                Int32 pipCount = 0;

                // Loop through all the checkers the current player has...
                foreach (NetworkBackgammonChecker playerChecker in this.Checkers)
                {
                    // Determine whether or not the opponent game position is in one of the "normal" positions
                    bool normalPosition = ((playerChecker.CurrentPosition.CurrentPosition >= NetworkBackgammonPosition.GameBoardPosition.NORMAL_START &&
                                            playerChecker.CurrentPosition.CurrentPosition <= NetworkBackgammonPosition.GameBoardPosition.NORMAL_END) ? true : false);

                    if (normalPosition)
                    {
                        pipCount += Convert.ToInt32(playerChecker.CurrentPosition.GetOppositePosition().CurrentPosition);
                    }
                    else if (playerChecker.CurrentPosition.CurrentPosition == NetworkBackgammonPosition.GameBoardPosition.BAR)
                    {
                        pipCount += Convert.ToInt32(NetworkBackgammonPosition.GameBoardPosition.HOME_END) + 1;
                    }
                }

                return pipCount;
            }
        }
        #endregion

        #region Overrides

        public override string ToString()
        {
            return strPlayerName;
        }

        /// <summary>
        /// Override Equals to compare the Player name
        /// instead of the Player object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>True if matches PlayerName</returns>
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            string inPlayerName = ((NetworkBackgammonPlayer)obj).PlayerName;
            if (inPlayerName != null)
                return PlayerName.ToLower().Equals(inPlayerName.ToLower());
            else
                return false;
        }

        public override int GetHashCode()
        {
            return PlayerName.GetHashCode();
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
                // Forward event
                Broadcast(e, sender);
            }
        }

        #endregion
    }
}
