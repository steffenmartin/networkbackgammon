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
        public static bool CalculatePossibleMoves(ref Player player1, ref Player player2, Dice [] _dice)
        {
            // Determine the active player
            Player activePlayer = player1.Active ? player1 : player2;
            Player waitingPlayer = player1.Active ? player2 : player1;

            bool bMovesCalulationDone = false;
            bool bAllCheckersHome = false;

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

            // Check whether all checkers of active player are home
            foreach (Checker checkerAP in activePlayer.Checkers)
            {
                if (!((checkerAP.CurrentPosition.CurrentPosition >= Position.GameBoardPosition.HOME_START &&
                    checkerAP.CurrentPosition.CurrentPosition <= Position.GameBoardPosition.HOME_END) ||
                    checkerAP.CurrentPosition.CurrentPosition == Position.GameBoardPosition.OFFBOARD))
                {
                    bAllCheckersHome = false;
                    break;
                }
            }

            Dictionary<Position.GameBoardPosition, UInt32> activePlayerCheckerHistogram = new Dictionary<Position.GameBoardPosition, uint>();
            Dictionary<Position.GameBoardPosition, UInt32> waitingPlayerCheckerHistogram = new Dictionary<Position.GameBoardPosition, uint>();

            // Add checker position histogram containers for all positions checkers are sitting on (for active and waiting player)
            foreach (Position.GameBoardPosition positionValue in Enum.GetValues(typeof(Position.GameBoardPosition)))
            {
                if (!activePlayerCheckerHistogram.ContainsKey(positionValue))
                {
                    activePlayerCheckerHistogram.Add(positionValue, 0);
                }
                if (!waitingPlayerCheckerHistogram.ContainsKey(positionValue))
                {
                    waitingPlayerCheckerHistogram.Add(positionValue, 0);
                }
            }

            // Create checker position histograms
            foreach (Checker checkerAP in activePlayer.Checkers)
            {
                activePlayerCheckerHistogram[checkerAP.CurrentPosition.CurrentPosition] += 1;
            }

            foreach (Checker checkerWP in activePlayer.Checkers)
            {
                waitingPlayerCheckerHistogram[checkerWP.CurrentPosition.CurrentPosition] += 1;
            }

            Dictionary<Checker, KeyValuePair<Dice, Position>[]> potentialPositionHistogram = new Dictionary<Checker, KeyValuePair<Dice, Position>[]>();

            // Create potential positions for every checker
            foreach (Checker checkerAP in activePlayer.Checkers)
            {
                if (!potentialPositionHistogram.ContainsKey(checkerAP))
                {

                    List<KeyValuePair<Dice, Position>> potPosList = new List<KeyValuePair<Dice, Position>>();

                    foreach (Dice dice in _dice)
                    {
                        if ((bAllCheckersHome) || 
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

            // First pass: Check whether any checkers are on the bar
            // as they'll have to be move before anything else
            // (AP: Active Player, WP: Waiting Player)
            foreach (Checker checkerAP in activePlayer.Checkers)
            {
                // Found a checker that's sitting on the bar
                if (checkerAP.CurrentPosition.CurrentPosition == Position.GameBoardPosition.BAR)
                {
                    bMovesCalulationDone = true;

                    foreach (Dice dice in _dice)
                    {
                        // Now, see whether opponents checker position allow us to move
                        if (waitingPlayerCheckerHistogram[(checkerAP.CurrentPosition + dice).CurrentPosition] <= 1)
                        {
                            checkerAP.PossibleMoves.Add(dice);
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
                        if (waitingPlayerCheckerHistogram[kvpDicePos.Value.CurrentPosition] <= 1)
                        {
                            kvp.Key.PossibleMoves.Add(kvpDicePos.Key);
                        }
                    }

                    
                    /*
                    if (waitingPlayerCheckerHistogram[kvp.Value[1].CurrentPosition] <= 1)
                    {
                        kvp.Key.PossibleMoves.Add(dice[1]);
                    }
                     * */
                }
            }

            return true;
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
            foreach (Checker checker in activePlayer.Checkers)
            {
                // Found checker, now find checker's move that has been selected
                if (checker == _checker)
                {
                    foreach (Dice move in checker.PossibleMoves)
                    {
                        if (move == _move)
                        {
                            checker.MoveChecker(_move);
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
