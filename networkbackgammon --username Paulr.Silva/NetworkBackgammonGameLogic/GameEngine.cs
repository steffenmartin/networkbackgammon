using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkBackgammonGameLogic
{
    public class GameEngine
    {
        public static bool CalculatePossibleMoves(ref Player player1, ref Player player2, Dice [] dice)
        {
            Player activePlayer = player1.Active ? player1 : player2;
            Player waitingPlayer = player1.Active ? player2 : player1;

            bool bMovesCalulationDone = false;

            Dictionary<Position.GameBoardPosition, UInt32> activePlayerCheckerHistogram = new Dictionary<Position.GameBoardPosition, uint>();
            Dictionary<Position.GameBoardPosition, UInt32> waitingPlayerCheckerHistogram = new Dictionary<Position.GameBoardPosition, uint>();

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

            Dictionary<Checker, Position[]> potentialPositionHistogram = new Dictionary<Checker, Position[]>();

            // Create potential positions for every checker
            foreach (Checker checkerAP in activePlayer.Checkers)
            {
                if (!potentialPositionHistogram.ContainsKey(checkerAP))
                {
                    potentialPositionHistogram.Add(
                        checkerAP, 
                        new Position[] 
                        {
                            checkerAP.CurrentPosition + dice[0],
                            checkerAP.CurrentPosition + dice[1]
                        });
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

                    Position[] potentialPosition = new Position[] 
                    {
                        checkerAP.CurrentPosition + dice[0],
                        checkerAP.CurrentPosition + dice[1]
                    };

                    // Now, see whether opponents checker position allow us to move
                    if (waitingPlayerCheckerHistogram[potentialPosition[0].CurrentPosition] <= 1)
                    {
                        checkerAP.PossibleMoves.Add(dice[0].CurrentValue);
                    }

                    if (waitingPlayerCheckerHistogram[potentialPosition[1].CurrentPosition] <= 1)
                    {
                        checkerAP.PossibleMoves.Add(dice[1].CurrentValue);
                    }
                }
            }

            if (!bMovesCalulationDone)
            {
                // Second pass: Check whether potential moves are valid
                foreach (KeyValuePair<Checker, Position[]> kvp in potentialPositionHistogram)
                {
                    if (waitingPlayerCheckerHistogram[kvp.Value[0].CurrentPosition] <= 1)
                    {
                        kvp.Key.PossibleMoves.Add(dice[0].CurrentValue);
                    }

                    if (waitingPlayerCheckerHistogram[kvp.Value[1].CurrentPosition] <= 1)
                    {
                        kvp.Key.PossibleMoves.Add(dice[1].CurrentValue);
                    }
                }
            }

            return true;
        }
    }
}
