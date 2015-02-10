//-----------------------------------------------------------------------
// <copyright file="HexGameColor.cs" company="Pete Biencourt">
//     Copyright (c) Pete Biencourt. All rights reserved.
// </copyright>
// <author>Pete Biencourt</author>
//-----------------------------------------------------------------------

namespace HexGame
{
    using UnityEngine;

    /// <summary>
    /// Custom colors for Hex
    /// </summary>
    public static class HexGameColor
    {
        /// <summary>
        /// Gets the color green
        /// </summary>
        public static Color Green
        {
            get
            {
                return new Color32(24, 190, 9, 1);
            }
        }

        /// <summary>
        /// Gets the color red
        /// </summary>
        public static Color Red
        {
            get
            {
                return new Color32(190, 9, 9, 1);
            }
        }

        /// <summary>
        /// Gets the color purple
        /// </summary>
        public static Color Purple
        {
            get
            {
                return new Color32(108, 9, 190, 1);
            }
        }

        /// <summary>
        /// Gets the color cyan
        /// </summary>
        public static Color Cyan
        {
            get
            {
                return new Color32(11, 219, 234, 1);
            }
        }

        /// <summary>
        /// Gets the color blue
        /// </summary>
        public static Color Blue
        {
            get
            {
                return new Color32(0, 94, 255, 1);
            }
        }
    }
}
