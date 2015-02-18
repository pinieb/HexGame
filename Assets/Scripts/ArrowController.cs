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
        /// Bullet prefab
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public GameObject Bullet;

        /// <summary>
        /// Bullet spawn location
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public GameObject BulletSpawn;

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
            this.LookAt(worldPosition);
            base.MoveTo(coord, worldPosition);    
        }

        /// <summary>
        /// Turns the unit to face the given position
        /// </summary>
        /// <param name="worldPosition">Position to face</param>
        /// <param name="shouldSnap">Whether or not the look should snap to certain angles</param>
        public override void LookAt(Vector3 worldPosition, bool shouldSnap = false)
        {
            Quaternion healthBarRotation = this.HealthBarManager.transform.rotation;

            Vector3 lookPos = new Vector3(worldPosition.x, this.transform.position.y, worldPosition.z);

            if (shouldSnap)
            {
                this.transform.LookAt(lookPos, Vector3.up);
                int snap = (60 * (((int)this.transform.eulerAngles.y) / 60)) + 30;
                Quaternion rotation = Quaternion.Euler(0f, snap, 0f);
                this.transform.rotation = rotation;
            }
            else
            {
                this.StartCoroutine(this.TurnToFace(lookPos));
            }

            this.HealthBarManager.transform.rotation = healthBarRotation;
        }

        /// <summary>
        /// Attacks another unit
        /// </summary>
        /// <param name="unit">Unit to attack</param>
        protected override void Attack(Unit unit)
        {
            GameLogger.Instance.AddMove(ActionRecord.AttackAction(this, this.Coordinate, unit, unit.Coordinate));
            this.LookAt(unit.transform.position);

            this.AnimationState = AnimationState.Attack;
            var bullet = (GameObject)Instantiate(this.Bullet);

            bullet.transform.position = this.BulletSpawn.transform.position;
            this.StartCoroutine(bullet.GetComponent<BulletController>().SendTo(
                unit.transform.position, 
                () => 
                { 
                    this.AnimationState = AnimationState.Idle;
                    unit.Health -= this.AttackPower;
                }));
        }
    }
}
