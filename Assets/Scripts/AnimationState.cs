//-----------------------------------------------------------------------
// <copyright file="AnimationState.cs" company="Pete Biencourt">
//     Copyright (c) Pete Biencourt. All rights reserved.
// </copyright>
// <author>Pete Biencourt</author>
//-----------------------------------------------------------------------

namespace HexGame
{
    /// <summary>
    /// Animation states
    /// </summary>
    public enum AnimationState
    {
        /// <summary>
        /// Idle state
        /// </summary>
        Idle,

        /// <summary>
        /// Move state
        /// </summary>
        Move,

        /// <summary>
        /// Attack state
        /// </summary>
        Attack,

        /// <summary>
        /// Heal state
        /// </summary>
        Heal
    }
}
