using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetworkBackgammonLib;

namespace NetworkBackgammonGameLogic
{
    public class NetworkBackgammonGameEngine
    {
        /// <summary>
        /// Calculates possible moves for the checkers of the active player.
        /// </summary>
        /// <param name="player1">Player 1</param>
        /// <param name="player2">Player 2</param>
        /// <param name="dice">Dice</param>
        /// <returns>Success (true) if at least one move of the active players checkers is possible, otherwise false.</returns>
        /// <remarks>
        /// Updates the active players list of possible moves.
        /// </remarks>
        public static bool CalculatePossibleMoves(ref NetworkBackgammonPlayer player1, ref NetworkBackgammonPlayer player2, NetworkBackgammonDice[] _dice)
        {
            // Abbreviations:
            //
            // AP: Active Player (e.g. checkerHistogramAP -> histogram of checkers of active player)
            // WP: Waiting Player (e.g. checkerWP -> checkers of the waiting player)
            // kvp: key-value-pair

            // Determine the active player
            NetworkBackgammonPlayer activePlayer = player1.Active ? player1 : player2;
            NetworkBackgammonPlayer waitingPlayer = player1.Active ? player2 : player1;

            bool bMovesCalulationDone = false;
            bool bAllCheckersHomeOrOffBoard = true;
            bool bActivePlayerHasMoves = false;

            #region Preparation

            // Delete possible moves for both, active ...
            foreach (NetworkBackgammonChecker checkerAP in activePlayer.Checkers)
            {
                checkerAP.PossibleMoves.Clear();
            }

            // ... and waiting player (just to be sure)
            foreach (NetworkBackgammonChecker checkerWP in waitingPlayer.Checkers)
            {
                checkerWP.PossibleMoves.Clear();
            }

            // Check whether all checkers of active player are home or off the board
            // This is necessary to determine whether active player is able to bear off
            // checkers (off the board)
            foreach (NetworkBackgammonChecker checkerAP in activePlayer.Checkers)
            {
                if (!((checkerAP.CurrentPosition.CurrentPosition >= NetworkBackgammonPosition.GameBoardPosition.HOME_START &&
                    checkerAP.CurrentPosition.CurrentPosition <= NetworkBackgammonPosition.GameBoardPosition.HOME_END) ||
                    checkerAP.CurrentPosition.CurrentPosition == NetworkBackgammonPosition.GameBoardPosition.OFFBOARD))
                {
                    bAllCheckersHomeOrOffBoard = false;
                    break;
                }
            }

            Dictionary<NetworkBackgammonPosition.GameBoardPosition, UInt32> checkerHistogramAP = new Dictionary<NetworkBackgammonPosition.GameBoardPosition, uint>();
            Dictionary<NetworkBackgammonPosition.GameBoardPosition, UInt32> checkerHistogramWP = new Dictionary<NetworkBackgammonPosition.GameBoardPosition, uint>();

            // Add checker position histogram containers for all positions checkers are sitting on (for active and waiting player)
            foreach (NetworkBackgammonPosition.GameBoardPosition positionValue in Enum.GetValues(typeof(NetworkBackgammonPosition.GameBoardPosition)))
            {
                if (!checkerHistogramAP.ContainsKey(positionValue))
                {
                    checkerHistogramAP.Add(positionValue, 0);
                }
                if (!checkerHistogramWP.ContainsKey(positionValue))
                {
                    checkerHistogramWP.Add(positionValue, 0);
                }
            }

            // Create checker position histograms
            foreach (NetworkBackgammonChecker checkerAP in activePlayer.Checkers)
            {
                checkerHistogramAP[checkerAP.CurrentPosition.CurrentPosition] += 1;
            }

            foreach (NetworkBackgammonChecker checkerWP in waitingPlayer.Checkers)
            {
                if (checkerWP.CurrentPosition.GetOppositePosition().CurrentPosition != NetworkBackgammonPosition.GameBoardPosition.INVALID)
                {
                    checkerHistogramWP[checkerWP.CurrentPosition.GetOppositePosition().CurrentPosition] += 1;
                }
                else
                {
                    checkerHistogramWP[checkerWP.CurrentPosition.CurrentPosition] += 1;
                }
            }

            Dictionary<NetworkBackgammonChecker, KeyValuePair<NetworkBackgammonDice, NetworkBackgammonPosition>[]> potentialPositionHistogram = new Dictionary<NetworkBackgammonChecker, KeyValuePair<NetworkBackgammonDice, NetworkBackgammonPosition>[]>();

            // Create potential positions for every checker
            foreach (NetworkBackgammonChecker checkerAP in activePlayer.Checkers)
            {
                if (!potentialPositionHistogram.ContainsKey(checkerAP))
                {
                    // List of potential positions associated associated with a certain dice value
                    List<KeyValuePair<NetworkBackgammonDice, NetworkBackgammonPosition>> potPosList = new List<KeyValuePair<NetworkBackgammonDice, NetworkBackgammonPosition>>();

                    foreach (NetworkBackgammonDice dice in _dice)
                    {
                        // Moving a checker off the board (bear off) is only allowed if all checkers of active player are in the home position
                        // (or off the board already)
                        if ((bAllCheckersHomeOrOffBoard) ||
                            (checkerAP.CurrentPosition + dice).CurrentPosition < NetworkBackgammonPosition.GameBoardPosition.OFFBOARD)
                        {
                            potPosList.Add(new KeyValuePair<NetworkBackgammonDice, NetworkBackgammonPosition>(dice, checkerAP.CurrentPosition + dice));
                        }
                    }

                    potentialPositionHistogram.Add(
                        checkerAP,
                        potPosList.ToArray());
                }
            }

            #endregion

            // First pass: Check whether any checkers are on the bar
            // as they'll have to be moved before anything else
            foreach (NetworkBackgammonChecker checkerAP in activePlayer.Checkers)
            {
                // Found a checker that's sitting on the bar
                if (checkerAP.CurrentPosition.CurrentPosition == NetworkBackgammonPosition.GameBoardPosition.BAR)
                {
                    bMovesCalulationDone = true;

                    foreach (NetworkBackgammonDice dice in _dice)
                    {
                        // Now, see whether opponents checker position allow us to move
                        if (checkerHistogramWP[(checkerAP.CurrentPosition + dice).CurrentPosition] <= 1)
                        {
                            checkerAP.PossibleMoves.Add(dice);

                            bActivePlayerHasMoves = true;
                        }
                    }
                }
            }

            if (!bMovesCalulationDone)
            {
                // Second pass: Check whether potential moves are valid
                foreach (KeyValuePair<NetworkBackgammonChecker, KeyValuePair<NetworkBackgammonDice, NetworkBackgammonPosition>[]> kvp in potentialPositionHistogram)
                {
                    foreach (KeyValuePair<NetworkBackgammonDice, NetworkBackgammonPosition> kvpDicePos in kvp.Value)
                    {
                        // Check whether waiting player occupies the potential position for active players checker
                        // (i.e. waiting player has more than 1 checker on the potential position)
                        // or potential position is off board (which is allways possible)
                        if (checkerHistogramWP[kvpDicePos.Value.CurrentPosition] <= 1 ||
                            kvpDicePos.Value.CurrentPosition == NetworkBackgammonPosition.GameBoardPosition.OFFBOARD)
                        {
                            kvp.Key.PossibleMoves.Add(kvpDicePos.Key);

                            bActivePlayerHasMoves = true;
                        }
                    }
                }
            }

            return bActivePlayerHasMoves;
        }

        /// <summary>
        /// Executes a valid move of the active player.
        /// </summary>
        /// <param name="player1">Player 1</param>
        /// <param name="player2">Player 2</param>
        /// <param name="_checker">Selected checker to be moved</param>
        /// <param name="_move">Selected move (according to one of the dice values)</param>
        /// <returns>Success (true) if the active player has won the game, otherwise false</returns>
        public static bool ExecuteMove(ref NetworkBackgammonPlayer player1, 
                                       ref NetworkBackgammonPlayer player2, 
                                       NetworkBackgammonChecker _checker, 
                                       NetworkBackgammonDice _move)
        {
            // Determine the active player
            NetworkBackgammonPlayer activePlayer = player1.Active ? player1 : player2;
            NetworkBackgammonPlayer waitingPlayer = player1.Active ? player2 : player1;

            bool bActivePlayerWon = true;

            // Find active player's checker that has been selected to move
            foreach (NetworkBackgammonChecker checkerAP in activePlayer.Checkers)
            {
                // Found checker, now find checker's move that has been selected
                if (checkerAP == _checker)
                {
                    foreach (NetworkBackgammonDice move in checkerAP.PossibleMoves)
                    {
                        // Found move
                        if (move == _move)
                        {
                            checkerAP.MoveChecker(_move);

                            if (checkerAP.CurrentPosition.CurrentPosition >= NetworkBackgammonPosition.GameBoardPosition.NORMAL_START &&
                                checkerAP.CurrentPosition.CurrentPosition <= NetworkBackgammonPosition.GameBoardPosition.NORMAL_END)
                            {
                                // Check whether active player kicked checker of the waiting player onto the bar
                                foreach (NetworkBackgammonChecker checkerWP in waitingPlayer.Checkers)
                                {
                                    if (checkerWP.CurrentPosition.GetOppositePosition().CurrentPosition == checkerAP.CurrentPosition.CurrentPosition)
                                    {
                                        checkerWP.CurrentPosition.Reset();
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // Check whether active player has won the game
            foreach (NetworkBackgammonChecker checker in activePlayer.Checkers)
            {
                if (checker.CurrentPosition.CurrentPosition != NetworkBackgammonPosition.GameBoardPosition.OFFBOARD)
                {
                    bActivePlayerWon = false;
                    break;
                }
            }

            return bActivePlayerWon;
        }
    }
}
