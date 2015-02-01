//-----------------------------------------------------------------------
// <copyright file="HexDistance.cs" company="Pete Biencourt">
//     Copyright (c) Pete Biencourt. All rights reserved.
// </copyright>
// <author>Pete Biencourt</author>
//-----------------------------------------------------------------------

namespace HexGame
{
    using System;

    /// <summary>
    /// Hex distance
    /// </summary>
    public class HexDistance
    {
        /// <summary>
        /// Computes the distance between point 0 and point 1
        /// </summary>
        /// <param name="x0">X for point 0</param>
        /// <param name="y0">Y for point 0</param>
        /// <param name="x1">X for point 1</param>
        /// <param name="y1">Y for point 1</param>
        /// <returns>Distance between point 0 and point 1 in hex cells</returns>
        public static int Distance(int x0, int y0, int x1, int y1)
        {
            int dx = x1 - x0 - (int)Math.Floor(y0 / 2f) - (int)Math.Floor(y1 / 2f);
            int dy = y1 - y0;

            if (Math.Sign(dx) == Math.Sign(dy))
            {
                return Math.Abs(dx + dy);
            }
            else
            {
                return Math.Max(Math.Abs(dx), Math.Abs(dy));
            }
        }

        /// <summary>
        /// Computes the distance between two coordinates
        /// </summary>
        /// <param name="c1">First coordinate</param>
        /// <param name="c2">Second coordinate</param>
        /// <returns>Distance between the coordinates in hex cells</returns>
        public static int Distance(CellCoordinate c1, CellCoordinate c2)
        {
            int dx = c2.X - c1.X;
            int dy = c2.Y - c1.Y;

            if (Math.Sign(dx) == Math.Sign(dy))
            {
                return Math.Abs(dx + dy);
            }
            else
            {
                return Math.Max(Math.Abs(dx), Math.Abs(dy));
            }
        }
    }
}