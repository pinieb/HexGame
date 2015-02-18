//-----------------------------------------------------------------------
// <copyright file="BulletController.cs" company="Pete Biencourt">
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
    /// Bullet controller
    /// </summary>
    public class BulletController : MonoBehaviour
    {
        /// <summary>
        /// Bullet speed
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public float BulletSpeed = 5f;

        /// <summary>
        /// Bullet target
        /// </summary>
        private Vector3 target;

        /// <summary>
        /// Sends the bullet to the target location
        /// </summary>
        /// <param name="target">Target position</param>
        /// <param name="completion">Completion block</param>
        /// <returns>Enumerator for co-routine</returns>
        public IEnumerator SendTo(Vector3 target, Action completion)
        {
            this.target = target;

            while (this.transform.position != target)
            {
                float step = this.BulletSpeed * Time.deltaTime;
                this.transform.position = Vector3.MoveTowards(this.transform.position, this.target, step);
                yield return null;
            }

            if (completion != null)
            {
                completion();
            }

            this.Destroy(this);
        }
    }
}
