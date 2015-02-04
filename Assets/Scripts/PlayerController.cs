//-----------------------------------------------------------------------
// <copyright file="PlayerController.cs" company="Pete Biencourt">
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
    /// Player controller
    /// </summary>
    public class PlayerController
    {
        /// <summary>
        /// Player's id
        /// </summary>
        private readonly int id;

        /// <summary>
        /// List of the units the player owns
        /// </summary>
        private List<Unit> units;

        /// <summary>
        /// Player's color
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public Color Color;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerController"/> class
        /// </summary>
        /// <param name="id">Player id</param>
        public PlayerController(int id)
        {
            this.id = id;
            this.units = new List<Unit>();
        }

        /// <summary>
        /// Gets the player's id
        /// </summary>
        public int Id
        {
            get
            {
                return this.id;
            }
        }

        /// <summary>
        /// Add a unit to the units list
        /// </summary>
        /// <param name="u">Unit to add</param>
        public void AddUnit(Unit u)
        {
            u.Owner = this;
            this.units.Add(u);
        }
    }
}
