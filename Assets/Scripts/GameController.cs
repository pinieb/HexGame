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
            this.Players.Add(new PlayerController(1));
            this.Players[0].Color = HexGameColor.Red;
            this.Players.Add(new PlayerController(2));
            this.Players[1].Color = HexGameColor.Blue;

            // Initialize Turn Controller
            this.TurnController.Initialize(this.Players);

            // Spawn units
            // Player 1
            // Cubes
            this.BoardController.SpawnUnit(UnitType.Cube, new CellCoordinate(-3, -2), this.Players[0]);
            this.BoardController.SpawnUnit(UnitType.Cube, new CellCoordinate(-3, -1), this.Players[0]);
            this.BoardController.SpawnUnit(UnitType.Cube, new CellCoordinate(-3, 0), this.Players[0]);
            this.BoardController.SpawnUnit(UnitType.Cube, new CellCoordinate(-4, 1), this.Players[0]);
            this.BoardController.SpawnUnit(UnitType.Cube, new CellCoordinate(-5, 2), this.Players[0]);

            // Arrows
            this.BoardController.SpawnUnit(UnitType.Arrow, new CellCoordinate(-4, -1), this.Players[0]);
            this.BoardController.SpawnUnit(UnitType.Arrow, new CellCoordinate(-4, 0), this.Players[0]);
            this.BoardController.SpawnUnit(UnitType.Arrow, new CellCoordinate(-5, 1), this.Players[0]);

            // Pyramid
            this.BoardController.SpawnUnit(UnitType.Pyramid, new CellCoordinate(-5, 0), this.Players[0]);

            // Player 2
            // Cubes
            this.BoardController.SpawnUnit(UnitType.Cube, new CellCoordinate(3, 2), this.Players[1]);
            this.BoardController.SpawnUnit(UnitType.Cube, new CellCoordinate(3, 1), this.Players[1]);
            this.BoardController.SpawnUnit(UnitType.Cube, new CellCoordinate(3, 0), this.Players[1]);
            this.BoardController.SpawnUnit(UnitType.Cube, new CellCoordinate(4, -1), this.Players[1]);
            this.BoardController.SpawnUnit(UnitType.Cube, new CellCoordinate(5, -2), this.Players[1]);

            // Arrows
            this.BoardController.SpawnUnit(UnitType.Arrow, new CellCoordinate(4, 1), this.Players[1]);
            this.BoardController.SpawnUnit(UnitType.Arrow, new CellCoordinate(4, 0), this.Players[1]);
            this.BoardController.SpawnUnit(UnitType.Arrow, new CellCoordinate(5, -1), this.Players[1]);

            // Pyramid
            this.BoardController.SpawnUnit(UnitType.Pyramid, new CellCoordinate(5, 0), this.Players[1]);
        }
    }
}