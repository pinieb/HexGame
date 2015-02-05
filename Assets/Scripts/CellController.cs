//-----------------------------------------------------------------------
// <copyright file="CellController.cs" company="Pete Biencourt">
//     Copyright (c) Pete Biencourt. All rights reserved.
// </copyright>
// <author>Pete Biencourt</author>
//-----------------------------------------------------------------------

namespace HexGame
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using UnityEngine;

    /// <summary>
    /// Cell Controller
    /// </summary>
    public class CellController : MonoBehaviour, ISelectable
    {
        /// <summary>
        /// Mesh for cell
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public GameObject HexMesh;

        /// <summary>
        /// Board controller
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public BoardController BoardController;

        /// <summary>
        /// Coordinate for the cell
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public CellCoordinate Coordinate;

        /// <summary>
        /// Mouse down handler
        /// </summary>
        public void OnMouseDown()
        {
            this.BoardController.ProcessSelection(this);
        }

        /// <summary>
        /// Gets the cells neighbors
        /// </summary>
        /// <returns>List of neighbors</returns>
        public List<CellCoordinate> GetNeighbors()
        {
            List<CellCoordinate> list = new List<CellCoordinate>();
            for (int i = -1 * this.BoardController.GridSize; i <= this.BoardController.GridSize; i++)
            {
                for (int j = -1 * this.BoardController.GridSize; j <= this.BoardController.GridSize; j++)
                {
                    if (HexDistance.Distance(0, 0, i, j) <= this.BoardController.GridSize
                        && HexDistance.Distance(this.Coordinate.X, this.Coordinate.Y, i, j) == 1)
                    {
                        list.Add(new CellCoordinate(i, j));
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// Select the cell
        /// </summary>
        public void Select()
        {
            this.BoardController.ProcessSelection(this);
        }

        /// <summary>
        /// Deselect the cell
        /// </summary>
        public void Deselect()
        {
        }

        /// <summary>
        /// Highlight the cell
        /// </summary>
        /// <param name="mode">Highlight mode</param>
        public void HighlightCellForMode(CellHighlightMode mode)
        {
            switch (mode)
            {
                case CellHighlightMode.Selected:
                    this.HighlightCell(Color.cyan);
                    break;

                case CellHighlightMode.Move:
                    this.HighlightCell(Color.green);
                    break;

                case CellHighlightMode.Attack:
                    this.HighlightCell(Color.red);
                    break;

                case CellHighlightMode.Heal:
                    this.HighlightCell(Color.yellow);
                    break;

                case CellHighlightMode.None:
                default:
                    this.UnhighlightCell();
                    break;
            }
        }

        /// <summary>
        /// Highlight the cell
        /// </summary>
        /// <param name="color">Color to highlight cell</param>
        private void HighlightCell(Color color)
        {
            this.HexMesh.renderer.materials[1].color = color;
        }

        /// <summary>
        /// Un-highlight the cell
        /// </summary>
        private void UnhighlightCell()
        {
            this.HexMesh.renderer.materials[1].color = Color.white;
        }
    }
}
