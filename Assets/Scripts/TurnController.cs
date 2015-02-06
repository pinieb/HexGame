//-----------------------------------------------------------------------
// <copyright file="TurnController.cs" company="Pete Biencourt">
//     Copyright (c) Pete Biencourt. All rights reserved.
// </copyright>
// <author>Pete Biencourt</author>
//-----------------------------------------------------------------------

namespace HexGame
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using UnityEngine;

    /// <summary>
    /// Turn controller
    /// </summary>
    public class TurnController : MonoBehaviour
    {
        /// <summary>
        /// Board controller
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public BoardController BoardController;

        /// <summary>
        /// List of players
        /// </summary>
        private List<PlayerController> players;

        /// <summary>
        /// Turn index
        /// </summary>
        private int turnIndex = -1;

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
        /// Gets the number of half turns that have occurred
        /// </summary>
        public int HalfTurnCount
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
        /// Gets or sets a value indicating whether or not the game has ended
        /// </summary>
        public bool GameOver
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes the turn controller
        /// </summary>
        /// <param name="players">List of players, in order of turn</param>
        public void Initialize(IList<PlayerController> players)
        {
            this.players = new List<PlayerController>(players);
            this.turnIndex = 0;
            this.TurnNumber = 0;
            this.HalfTurnCount = 0;
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
        /// <param name="unit">Unit that acted</param>
        public void HandleUnitAction(Unit unit)
        {
            if (!unit.CanAct)
            {
                this.EndTurn();
                unit.ResetUnit();
            }
        }

        /// <summary>
        /// Ends the current player's turn
        /// </summary>
        public void EndTurn()
        {
            this.BoardController.HandleWin(this.TurnToPlay);

            if (!this.GameOver)
            {
                this.UnitMoved = null;
                this.turnIndex = ++this.turnIndex % this.players.Count;
                this.HalfTurnCount++;
                this.TurnNumber = this.HalfTurnCount / 2; // we want this to truncate
            }
        }
    }
}