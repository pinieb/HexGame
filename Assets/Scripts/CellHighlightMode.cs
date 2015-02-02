//-----------------------------------------------------------------------
// <copyright file="CellHighlightMode.cs" company="Pete Biencourt">
//     Copyright (c) Pete Biencourt. All rights reserved.
// </copyright>
// <author>Pete Biencourt</author>
//-----------------------------------------------------------------------

namespace HexGame
{
    /// <summary>
    /// Cell highlight mode
    /// </summary>
    public enum CellHighlightMode
    {
        /// <summary>
        /// Highlight mode for selected cell
        /// </summary>
        Selected,

        /// <summary>
        /// Highlight mode for movement
        /// </summary>
        Move,
 
        /// <summary>
        /// Highlight mode for attack
        /// </summary>
        Attack,

        /// <summary>
        /// No highlight
        /// </summary>
        None
    }
}
