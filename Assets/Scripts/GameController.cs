//-----------------------------------------------------------------------
// <copyright file="GameController.cs" company="Pete Biencourt">
//     Copyright (c) Pete Biencourt. All rights reserved.
// </copyright>
// <author>Pete Biencourt</author>
//-----------------------------------------------------------------------

namespace HexGame
{
    using System.Diagnostics.CodeAnalysis;
    using UnityEngine;

    /// <summary>
    /// Game controller
    /// </summary>
    public class GameController : MonoBehaviour
    {
        /// <summary>
        /// Map controller
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public MapController MapController;

        /// <summary>
        /// Start the controller
        /// </summary>
        public void Start()
        {
            // var spawnCoord = new CellCoordinate(1, 1); 
            // this.mapController.SpawnUnit(UnitType.Cube, spawnCoord);
        }
    }
}