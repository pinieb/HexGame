//-----------------------------------------------------------------------
// <copyright file="GameController.cs" company="Pete Biencourt">
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
    /// Game controller
    /// </summary>
    public class GameController : MonoBehaviour
    {
        /// <summary>
        /// Board controller
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public BoardController BoardController;

        /// <summary>
        /// Turn controller
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public TurnController TurnController;

        /// <summary>
        /// UI controller
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public UIController UIController;

        /// <summary>
        /// Gets or sets the players list
        /// </summary>
        public List<PlayerController> Players
        {
            get;
            set;
        }

        /// <summary>
        /// Start the controller
        /// </summary>
        public void Start()
        {
            // Create players
            this.Players = new List<PlayerController>();
            this.Players.Add(new PlayerController(0));
            this.Players[0].Color = Color.red;
            this.Players.Add(new PlayerController(1));
            this.Players[1].Color = Color.cyan;

            // Initialize Turn Controller
            this.TurnController.Initialize(this.Players);

            // Spawn units
            var spawnCoord = new CellCoordinate(1, 2);
            this.BoardController.SpawnUnit(UnitType.Cube, spawnCoord, this.Players[0]);

            spawnCoord = new CellCoordinate(1, 3);
            this.BoardController.SpawnUnit(UnitType.Cube, spawnCoord, this.Players[0]);

            spawnCoord = new CellCoordinate(2, 2);
            this.BoardController.SpawnUnit(UnitType.Cube, spawnCoord, this.Players[1]);
        }
    }
}