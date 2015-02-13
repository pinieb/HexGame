//-----------------------------------------------------------------------
// <copyright file="ActionRecord.cs" company="Pete Biencourt">
//     Copyright (c) Pete Biencourt. All rights reserved.
// </copyright>
// <author>Pete Biencourt</author>
//-----------------------------------------------------------------------

namespace HexGame
{
    /// <summary>
    /// Action record
    /// </summary>
    public class ActionRecord
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ActionRecord"/> class
        /// </summary>
        /// <param name="unit">Unit that acted</param>
        /// <param name="source">Source cell of the action</param>
        /// <param name="action">Action type</param>
        /// <param name="targetUnit">Target unit of the action</param>
        /// <param name="targetCell">Target cell of the action</param>
        private ActionRecord(Unit unit, CellCoordinate source, ActionType action, Unit targetUnit, CellCoordinate targetCell)
        {
            this.Unit = unit;
            this.Source = source;
            this.Action = action;
            this.TargetCell = targetCell;
            this.TargetUnit = targetUnit;
        }

        /// <summary>
        /// Gets the unit that acted
        /// </summary>
        public Unit Unit
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the source cell coordinate
        /// </summary>
        public CellCoordinate Source
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the action that occurred
        /// </summary>
        public ActionType Action
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the target cell coordinate
        /// </summary>
        public CellCoordinate TargetCell
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the target unit
        /// </summary>
        public Unit TargetUnit
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates a record for movement
        /// </summary>
        /// <param name="unit">Unit that moved</param>
        /// <param name="source">Where it moved from</param>
        /// <param name="destination">Where it moved to</param>
        /// <returns>Action record for movement</returns>
        public static ActionRecord MoveAction(Unit unit, CellCoordinate source, CellCoordinate destination)
        {
            return new ActionRecord(unit, source, ActionType.Move, null, destination);
        }

        /// <summary>
        /// Creates a record for attack
        /// </summary>
        /// <param name="unit">Unit that attacked</param>
        /// <param name="source">Where it attacked from</param>
        /// <param name="targetUnit">Unit it attacked</param>
        /// <param name="targetCell">Cell it attacked</param>
        /// <returns>Action record for attack</returns>
        public static ActionRecord AttackAction(Unit unit, CellCoordinate source, Unit targetUnit, CellCoordinate targetCell)
        {
            return new ActionRecord(unit, source, ActionType.Attack, targetUnit, targetCell);
        }

        /// <summary>
        /// Creates a record for heal
        /// </summary>
        /// <param name="unit">Unit that healed</param>
        /// <param name="source">Where it healed from</param>
        /// <param name="targetUnit">Unit it healed</param>
        /// <param name="targetCell">Cell it healed</param>
        /// <returns>Action record for heal</returns>
        public static ActionRecord HealAction(Unit unit, CellCoordinate source, Unit targetUnit, CellCoordinate targetCell)
        {
            return new ActionRecord(unit, source, ActionType.Heal, targetUnit, targetCell);
        }
    }
}