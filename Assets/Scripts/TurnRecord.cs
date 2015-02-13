//-----------------------------------------------------------------------
// <copyright file="TurnRecord.cs" company="Pete Biencourt">
//     Copyright (c) Pete Biencourt. All rights reserved.
// </copyright>
// <author>Pete Biencourt</author>
//-----------------------------------------------------------------------

namespace HexGame
{
    using System.Collections.Generic;

    /// <summary>
    /// Turn record
    /// </summary>
    public class TurnRecord
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TurnRecord"/> class 
        /// </summary>
        /// <param name="turnNumber">Turn number</param>
        /// <param name="player">Player whose turn it is</param>
        public TurnRecord(int turnNumber, PlayerController player)
        {
            this.Moves = new List<ActionRecord>(2);
            this.TurnNumber = turnNumber;
            this.Player = player;
        }

        /// <summary>
        /// Gets the moves performed this turn
        /// </summary>
        public List<ActionRecord> Moves
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the turn number
        /// </summary>
        public int TurnNumber
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the player for the turn
        /// </summary>
        public PlayerController Player
        {
            get;
            private set;
        }
    }
}
