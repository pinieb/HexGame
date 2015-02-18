//-----------------------------------------------------------------------
// <copyright file="PyramidController.cs" company="Pete Biencourt">
//     Copyright (c) Pete Biencourt. All rights reserved.
// </copyright>
// <author>Pete Biencourt</author>
//-----------------------------------------------------------------------

namespace HexGame
{
    using System;
    using System.Collections;
    using System.Diagnostics.CodeAnalysis;
    using UnityEngine;

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
        /// Heal particles
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public ParticleSystem HealParticles;

        /// <summary>
        /// Heal target particles
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public ParticleSystem HealTargetParticles;

        /// <summary>
        /// Attack rotation speed
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public float AttackRotationSpeed = 10f;

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
        /// Attacks another unit
        /// </summary>
        /// <param name="unit">Unit to attack</param>
        protected override void Attack(Unit unit)
        {
            GameLogger.Instance.AddMove(ActionRecord.AttackAction(this, this.Coordinate, unit, unit.Coordinate));

            this.AnimationState = AnimationState.Attack;
            this.StartCoroutine(this.AnimateAttack(
                unit,
                () =>
                {
                    this.AnimationState = AnimationState.Idle;
                    unit.Health -= this.AttackPower;
                }));
        }

        /// <summary>
        /// Heals another unit
        /// </summary>
        /// <param name="unit">Unit to heal</param>
        protected void Heal(Unit unit)
        {
            GameLogger.Instance.AddMove(ActionRecord.HealAction(this, this.Coordinate, unit, unit.Coordinate));

            this.AnimationState = AnimationState.Heal;
            this.HealParticles.Play();

            Vector3 particlePos = unit.transform.position;
            particlePos.y += 3;
            this.HealTargetParticles.transform.position = particlePos;
            this.HealTargetParticles.Play();

            unit.Health += this.HealPower;
            this.AnimationState = AnimationState.Idle;
        }

        /// <summary>
        /// Animates the pyramid's attack
        /// </summary>
        /// <param name="target">Unit to attack</param>
        /// <param name="completion">Completion block</param>
        /// <returns>Enumerator for co-routine</returns>
        private IEnumerator AnimateAttack(Unit target, Action completion)
        {
            var lookPos = target.transform.position;
            lookPos.y = this.transform.position.y;
            yield return this.StartCoroutine(this.TurnToFace(lookPos));

            var neededRotation = Quaternion.Euler(this.transform.eulerAngles.x + 45f, this.transform.eulerAngles.y, this.transform.eulerAngles.z);
            var originalRotation = this.transform.rotation;

            for (float t = 0f; t < 1f; t += this.AttackRotationSpeed * Time.deltaTime)
            {
                this.transform.rotation = Quaternion.Lerp(originalRotation, neededRotation, t);
                yield return null;
            }

            this.transform.rotation = neededRotation;

            for (float t = 0f; t < 1f; t += this.AttackRotationSpeed * Time.deltaTime)
            {
                this.transform.rotation = Quaternion.Lerp(neededRotation, originalRotation, t);
                yield return null;
            }

            this.transform.rotation = originalRotation;

            if (completion != null)
            {
                completion();
            }
        }
    }
}
