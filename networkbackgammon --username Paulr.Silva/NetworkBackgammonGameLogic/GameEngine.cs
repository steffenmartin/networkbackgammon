using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkBackgammonGameLogic
{
    public class GameEngine
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
        public static bool CalculatePossibleMoves(ref Player player1, ref Player player2, Dice [] _dice)
        {
            // Abbreviations:
            //
            // AP: Active Player (e.g. checkerHistogramAP -> histogram of checkers of active player)
            // WP: Waiting Player (e.g. checkerWP -> checkers of the waiting player)
            // kvp: key-value-pair

            // Determine the active player
            Player activePlayer = player1.Active ? player1 : player2;
            Player waitingPlayer = player1.Active ? player2 : player1;

            bool bMovesCalulationDone = false;
            bool bAllCheckersHomeOrOffBoard = true;
            bool bActivePlayerHasMoves = false;

            #region Preparation

            // Delete possible moves for both, active ...
            foreach (Checker checkerAP in activePlayer.Checkers)
            {
                checkerAP.PossibleMoves.Clear();
            }

            // ... and waiting player (just to be sure)
            foreach (Checker checkerWP in waitingPlayer.Checkers)
            {
                checkerWP.PossibleMoves.Clear();
            }

            // Check whether all checkers of active player are home or off the board
            // This is necessary to determine whether active player is able to bear off
            // checkers (off the board)
            foreach (Checker checkerAP in activePlayer.Checkers)
            {
                if (!((checkerAP.CurrentPosition.CurrentPosition >= Position.GameBoardPosition.HOME_START &&
                    checkerAP.CurrentPosition.CurrentPosition <= Position.GameBoardPosition.HOME_END) ||
                    checkerAP.CurrentPosition.CurrentPosition == Position.GameBoardPosition.OFFBOARD))
                {
                    bAllCheckersHomeOrOffBoard = false;
                    break;
                }
            }

            Dictionary<Position.GameBoardPosition, UInt32> checkerHistogramAP = new Dictionary<Position.GameBoardPosition, uint>();
            Dictionary<Position.GameBoardPosition, UInt32> checkerHistogramWP = new Dictionary<Position.GameBoardPosition, uint>();

            // Add checker position histogram containers for all positions checkers are sitting on (for active and waiting player)
            foreach (Position.GameBoardPosition positionValue in Enum.GetValues(typeof(Position.GameBoardPosition)))
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
            foreach (Checker checkerAP in activePlayer.Checkers)
            {
                checkerHistogramAP[checkerAP.CurrentPosition.CurrentPosition] += 1;
            }

            foreach (Checker checkerWP in activePlayer.Checkers)
            {
                if (checkerWP.CurrentPosition.GetOppositePosition().CurrentPosition != Position.GameBoardPosition.INVALID)
                {
                    checkerHistogramWP[checkerWP.CurrentPosition.GetOppositePosition().CurrentPosition] += 1;
                }
                else
                {
                    checkerHistogramWP[checkerWP.CurrentPosition.CurrentPosition] += 1;
                }
            }

            Dictionary<Checker, KeyValuePair<Dice, Position>[]> potentialPositionHistogram = new Dictionary<Checker, KeyValuePair<Dice, Position>[]>();

            // Create potential positions for every checker
            foreach (Checker checkerAP in activePlayer.Checkers)
            {
                if (!potentialPositionHistogram.ContainsKey(checkerAP))
                {
                    // List of potential positions associated associated with a certain dice value
                    List<KeyValuePair<Dice, Position>> potPosList = new List<KeyValuePair<Dice, Position>>();

                    foreach (Dice dice in _dice)
                    {
                        // Moving a checker off the board (bear off) is only allowed if all checkers of active player are in the home position
                        // (or off the board already)
                        if ((bAllCheckersHomeOrOffBoard) || 
                            (checkerAP.CurrentPosition + dice).CurrentPosition < Position.GameBoardPosition.OFFBOARD)
                        {
                            potPosList.Add(new KeyValuePair<Dice,Position>(dice, checkerAP.CurrentPosition + dice));
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
            foreach (Checker checkerAP in activePlayer.Checkers)
            {
                // Found a checker that's sitting on the bar
                if (checkerAP.CurrentPosition.CurrentPosition == Position.GameBoardPosition.BAR)
                {
                    bMovesCalulationDone = true;

                    foreach (Dice dice in _dice)
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
                foreach (KeyValuePair<Checker, KeyValuePair<Dice, Position>[]> kvp in potentialPositionHistogram)
                {
                    foreach (KeyValuePair<Dice, Position> kvpDicePos in kvp.Value)
                    {
                        // Check whether waiting player occupies the potential position for active players checker
                        // (i.e. waiting player has more than 1 checker on the potential position)
                        if (checkerHistogramWP[kvpDicePos.Value.CurrentPosition] <= 1)
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
        public static bool ExecuteMove(ref Player player1, ref Player player2, Checker _checker, Dice _move)
        {
            // Determine the active player
            Player activePlayer = player1.Active ? player1 : player2;
            Player waitingPlayer = player1.Active ? player2 : player1;

            bool bActivePlayerWon = true;

            // Find active player's checker that has been selected to move
            foreach (Checker checkerAP in activePlayer.Checkers)
            {
                // Found checker, now find checker's move that has been selected
                if (checkerAP == _checker)
                {
                    foreach (Dice move in checkerAP.PossibleMoves)
                    {
                        // Found move
                        if (move == _move)
                        {
                            checkerAP.MoveChecker(_move);

                            if (checkerAP.CurrentPosition.CurrentPosition >= Position.GameBoardPosition.NORMAL_START &&
                                checkerAP.CurrentPosition.CurrentPosition <= Position.GameBoardPosition.NORMAL_END)
                            {
                                // Check whether active player kicked checker of the waiting player onto the bar
                                foreach (Checker checkerWP in waitingPlayer.Checkers)
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
            foreach (Checker checker in activePlayer.Checkers)
            {
                if (checker.CurrentPosition.CurrentPosition != Position.GameBoardPosition.OFFBOARD)
                {
                    bActivePlayerWon = false;
                    break;
                }
            }

            return bActivePlayerWon;
        }
    }
}
