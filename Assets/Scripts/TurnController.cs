//-----------------------------------------------------------------------
// <copyright file="TurnController.cs" company="Pete Biencourt">
//     Copyright (c) Pete Biencourt. All rights reserved.
// </copyright>
// <author>Pete Biencourt</author>
//-----------------------------------------------------------------------

namespace HexGame
{
    using System.Collections.Generic;

    /// <summary>
    /// Turn controller
    /// </summary>
    public class TurnController
    {
        /// <summary>
        /// List of players
        /// </summary>
        private List<PlayerController> players;

        /// <summary>
        /// Turn index
        /// </summary>
        private int turnIndex = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="TurnController"/> class
        /// </summary>
        /// <param name="players">List of players, in order of turn</param>
        public TurnController(IList<PlayerController> players)
        {
            this.players = new List<PlayerController>(players);
        }

        /// <summary>
        /// Gets whose turn it is to play
        /// </summary>
        public PlayerController TurnToPlay
        {
            get
            {
                return this.players[this.turnIndex];
            }
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
        /// Gets the unit that moved this turn (if any)
        /// </summary>
        public Unit UnitMoved
        {
            get;
            private set;
        }

        /// <summary>
        /// Handles a unit's movement
        /// </summary>
        /// <param name="unit">Unit that moved</param>
        public void HandleUnitMove(Unit unit)
        {
            this.UnitMoved = unit;
        }

        /// <summary>
        /// Handles a unit's action
        /// </summary>
        public void HandleUnitAction()
        {
            this.EndTurn();
        }

        /// <summary>
        /// Ends the current player's turn
        /// </summary>
        public void EndTurn()
        {
            this.UnitMoved = null;
            this.turnIndex = ++this.turnIndex % this.players.Count;
            this.TurnNumber++;
        }
    }
}