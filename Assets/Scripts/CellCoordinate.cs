//-----------------------------------------------------------------------
// <copyright file="CellCoordinate.cs" company="Pete Biencourt">
//     Copyright (c) Pete Biencourt. All rights reserved.
// </copyright>
// <author>Pete Biencourt</author>
//-----------------------------------------------------------------------

namespace HexGame
{
    /// <summary>
    /// Cell coordinate
    /// </summary>
    public class CellCoordinate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CellCoordinate" /> class
        /// </summary>
        /// <param name="x">X value</param>
        /// <param name="y">Y value</param>
        public CellCoordinate(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// Gets X value
        /// </summary>
        public int X
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets Y value
        /// </summary>
        public int Y
        {
            get;
            private set;
        }
    }
}
