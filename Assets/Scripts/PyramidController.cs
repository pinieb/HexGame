//-----------------------------------------------------------------------
// <copyright file="PyramidController.cs" company="Pete Biencourt">
//     Copyright (c) Pete Biencourt. All rights reserved.
// </copyright>
// <author>Pete Biencourt</author>
//-----------------------------------------------------------------------

namespace HexGame
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Pyramid controller
    /// </summary>
    public class PyramidController : Unit
    {
        /// <summary>
        /// Heal range
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public int HealRange;

        /// <summary>
        /// Heal power
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public int HealPower;

        /// <summary>
        /// Start the controller
        /// </summary>
        public new void Start()
        {
            base.Start();
        }

        /// <summary>
        /// Performs an action on another unit
        /// </summary>
        /// <param name="unit">Unit to act on</param>
        /// <returns>Whether or not the action was successful</returns>
        public override bool ActOn(Unit unit)
        {
            if (this.CanHeal(unit))
            {
                this.Heal(unit);
                this.CanAct = false;
                return true;
            }
            else if (this.CanAttack(unit))
            {
                this.Attack(unit);
                this.CanAct = false;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Determines whether or not healing the given unit is possible
        /// </summary>
        /// <param name="unit">Unit to heal</param>
        /// <returns>Whether or not the unit can be healed</returns>
        public bool CanHeal(Unit unit)
        {
            if (unit != null && unit != this && unit.Owner == this.Owner &&
                HexDistance.Distance(this.Coordinate, unit.Coordinate) <= this.HealRange)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Heals another unit
        /// </summary>
        /// <param name="unit">Unit to heal</param>
        protected void Heal(Unit unit)
        {
            GameLogger.Instance.AddMove(ActionRecord.HealAction(this, this.Coordinate, unit, unit.Coordinate));
            unit.Health += this.HealPower;
        }
    }
}
