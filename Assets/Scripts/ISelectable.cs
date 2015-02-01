//-----------------------------------------------------------------------
// <copyright file="ISelectable.cs" company="Pete Biencourt">
//     Copyright (c) Pete Biencourt. All rights reserved.
// </copyright>
// <author>Pete Biencourt</author>
//-----------------------------------------------------------------------

namespace HexGame
{
    /// <summary>
    /// Selectable interface
    /// </summary>
    public interface ISelectable
    {
        /// <summary>
        /// Select the object
        /// </summary>
        void Select();

        /// <summary>
        /// Deselect the object
        /// </summary>
        void Deselect();
    }
}