//-----------------------------------------------------------------------
// <copyright file="ArrowController.cs" company="Pete Biencourt">
//     Copyright (c) Pete Biencourt. All rights reserved.
// </copyright>
// <author>Pete Biencourt</author>
//-----------------------------------------------------------------------

namespace HexGame
{
    using System.Diagnostics.CodeAnalysis;
    using UnityEngine;

    /// <summary>
    /// Arrow controller
    /// </summary>
    public class ArrowController : Unit
    {
        /// <summary>
        /// Maximum number of attacks allowed
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public int MaxNumberOfAttacks;

        /// <summary>
        /// Attacks used this turn
        /// </summary>
        private int attacksUsedThisTurn = 0;

        /// <summary>
        /// Gets a value indicating whether or not the unit can act
        /// </summary>
        public override bool CanAct
        {
            get
            {
                return this.attacksUsedThisTurn < this.MaxNumberOfAttacks;
            }
        }

        /// <summary>
        /// Performs an action on another unit
        /// </summary>
        /// <param name="unit">Unit to act on</param>
        /// <returns>Whether or not the action was successful</returns>
        public override bool ActOn(Unit unit)
        {
            if (this.CanAttack(unit))
            {
                this.Attack(unit);
                this.attacksUsedThisTurn++;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Prepares the unit for the next turn
        /// </summary>
        public override void ResetUnit()
        {
            this.CanAct = true;
            this.attacksUsedThisTurn = 0;
        }

        /// <summary>
        /// Moves to the given coordinate and update the unit's world position
        /// </summary>
        /// <param name="coord">Cell coordinate</param>
        /// <param name="worldPosition">World position</param>
        public override void MoveTo(CellCoordinate coord, Vector3 worldPosition)
        {
            this.Coordinate = coord;
            this.LookAt(worldPosition);
            this.transform.position = worldPosition;
        }

        /// <summary>
        /// Attacks another unit
        /// </summary>
        /// <param name="unit">Unit to attack</param>
        protected override void Attack(Unit unit)
        {
            base.Attack(unit);
            this.LookAt(unit.transform.position);
        }

        /// <summary>
        /// Look at
        /// </summary>
        private void LookAt(Vector3 worldPosition)
        {
            Quaternion healthBarRotation = this.HealthBar.transform.rotation;
            this.transform.LookAt(worldPosition, Vector3.up);
            this.HealthBar.transform.rotation = healthBarRotation;
        }
    }
}
