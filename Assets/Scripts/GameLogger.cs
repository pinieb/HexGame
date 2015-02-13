//-----------------------------------------------------------------------
// <copyright file="GameLogger.cs" company="Pete Biencourt">
//     Copyright (c) Pete Biencourt. All rights reserved.
// </copyright>
// <author>Pete Biencourt</author>
//-----------------------------------------------------------------------

namespace HexGame
{
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Game logger
    /// </summary>
    public class GameLogger
    {
        /// <summary>
        /// Game logger instance
        /// </summary>
        private static GameLogger instance;

        /// <summary>
        /// Turn record list
        /// </summary>
        private List<TurnRecord> turns;

        /// <summary>
        /// Prevents a default instance of the <see cref="GameLogger"/> class from being created
        /// </summary>
        private GameLogger()
        {
            this.turns = new List<TurnRecord>();
        }

        /// <summary>
        /// Gets the static instance
        /// </summary>
        public static GameLogger Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameLogger();
                }

                return instance;
            }
        }

        /// <summary>
        /// Gets the list of moves
        /// </summary>
        public List<TurnRecord> Turns
        {
            get
            {
                return this.turns.GetRange(0, this.turns.Count);
            }
        }

        /// <summary>
        /// Gets the current turn
        /// Returns null if there is no current turn 
        /// This happens at the end of the game and at the beginning
        /// </summary>
        public TurnRecord CurrentTurn
        {
            get
            {
                if (this.turns.Count > 0)
                {
                    return this.turns[this.turns.Count - 1];
                }

                return null;
            }
        }

        /// <summary>
        /// Add a move to the log
        /// </summary>
        /// <param name="move">Move to add</param>
        public void AddMove(ActionRecord move)
        {
            this.CurrentTurn.Moves.Add(move);
        }

        /// <summary>
        /// Add a new turn to the log
        /// </summary>
        /// <param name="turn">Turn to add</param>
        public void AddTurn(TurnRecord turn)
        {
            this.turns.Add(turn);
        }
    }
}
