//-----------------------------------------------------------------------
// <copyright file="WorldToGameConverter.cs" company="Pete Biencourt">
//     Copyright (c) Pete Biencourt. All rights reserved.
// </copyright>
// <author>Pete Biencourt</author>
//-----------------------------------------------------------------------

namespace HexGame
{
    using System;
    using UnityEngine;

    /// <summary>
    /// World point to game point converter
    /// </summary>
    public static class WorldToGameConverter
    {
        /// <summary>
        /// Convert a world point to a game point
        /// </summary>
        /// <param name="worldPoint">Point to convert</param>
        /// <param name="scale">Conversion scale</param>
        /// <returns>Game point</returns>
        public static CellCoordinate ConvertWorldToCell(Vector3 worldPoint, float scale)
        {
            double y = Math.Round(worldPoint.z / (double)scale);
            double x = Math.Round(worldPoint.x - (.5 * y));
            return new CellCoordinate((int)x, (int)y);
        }
    }
}
