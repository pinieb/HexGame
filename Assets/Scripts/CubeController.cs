//-----------------------------------------------------------------------
// <copyright file="CubeController.cs" company="Pete Biencourt">
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
    /// Cube controller
    /// </summary>
    public class CubeController : Unit
    {
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
        /// Animates the cube's attack
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
