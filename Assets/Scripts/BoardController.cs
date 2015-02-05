//-----------------------------------------------------------------------
// <copyright file="BoardController.cs" company="Pete Biencourt">
//     Copyright (c) Pete Biencourt. All rights reserved.
// </copyright>
// <author>Pete Biencourt</author>
//-----------------------------------------------------------------------

namespace HexGame
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// Board controller
    /// </summary>
    public class BoardController : MonoBehaviour
    {
        /// <summary>
        /// Hex prefab
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public CellController HexCellPrefab;

        /// <summary>
        /// Cube prefab
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public CubeController CubeUnitPrefab;

        /// <summary>
        /// Pyramid prefab
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public PyramidController PyramidUnitPrefab;

        /// <summary>
        /// X scale
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public float XScale = 1.0f;

        /// <summary>
        /// Y scale
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public float YPosition = -1.0f;

        /// <summary>
        /// Grid size
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public int GridSize = 5;

        /// <summary>
        /// Unit height
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public float UnitHeight = 0f;

        /// <summary>
        /// Turn controller
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public TurnController TurnController;

        /// <summary>
        /// Unit list
        /// </summary>
        private Unit[,] units;

        /// <summary>
        /// Selected unit
        /// </summary>
        private Unit selection;

        /// <summary>
        /// Highlighted cells
        /// </summary>
        private List<CellController> highlightedCells;

        /// <summary>
        /// Cells list
        /// </summary>
        private CellController[,] cells;

        /// <summary>
        /// Gets Z Scale
        /// </summary>
        public float ZScale
        {
            get
            {
                return (float)(this.XScale * Math.Cos(Math.PI / 6.0));
            }
        }

        /// <summary>
        /// Gets or sets the cells list
        /// </summary>
        private CellController[,] Cells
        {
            get
            {
                if (this.cells == null)
                {
                    this.cells = new CellController[(this.GridSize * 2) + 1, (this.GridSize * 2) + 1];
                }

                return this.cells;
            }

            set
            {
                this.cells = value;
            }
        }

        /// <summary>
        /// Gets or sets the unit list
        /// </summary>
        private Unit[,] Units
        {
            get
            {
                if (this.units == null)
                {
                    this.units = new Unit[(this.GridSize * 2) + 1, (this.GridSize * 2) + 1];
                }

                return this.units;
            }

            set
            {
                this.units = value;
            }
        }

        /// <summary>
        /// Initialize the board controller
        /// </summary>
        public void Start()
        {
            this.Cells = new CellController[(this.GridSize * 2) + 1, (this.GridSize * 2) + 1];
            this.Units = new Unit[(this.GridSize * 2) + 1, (this.GridSize * 2) + 1];

            for (int i = -1 * this.GridSize; i <= this.GridSize; i++)
            {
                for (int j = -1 * this.GridSize; j <= this.GridSize; j++)
                {
                    if (HexDistance.Distance(0, 0, i, j) <= this.GridSize)
                    {
                        float x = i;
                        if (Math.Abs(j) % 2 == 1)
                        {
                            x = i + 0.5f;
                        }

                        var hex = (CellController)Instantiate(this.HexCellPrefab);
                        hex.transform.position = new Vector3(x * this.XScale, this.YPosition, j * this.ZScale);

                        CellCoordinate cellCoord = WorldToGameConverter.ConvertWorldToCell(hex.transform.position, this.ZScale);
                        hex.gameObject.name = "(" + cellCoord.X + ", " + cellCoord.Y + ")";
                        this.SetCell(cellCoord.X, cellCoord.Y, hex);
                        CellController cellController = hex.GetComponent<CellController>();
                        cellController.BoardController = this;
                        cellController.Coordinate = cellCoord;
                    }
                }
            }
        }

        /// <summary>
        /// Updates at a fixed interval
        /// </summary>
        public void HandleWin(PlayerController player)
        {
            if (!this.TurnController.GameOver && this.CheckForWin(player))
            {
                EditorUtility.DisplayDialog("Game over!", "Player " + player.Id + " is the winner!", "OK");
                this.TurnController.GameOver = true;
            }
        }

        /// <summary>
        /// Move a unit
        /// </summary>
        /// <param name="source">Cell to move unit from</param>
        /// <param name="target">Cell to move unit to</param>
        public void Move(CellCoordinate source, CellCoordinate target)
        {
            Unit unit = this.GetUnit(source.X, source.Y);
            if (unit != null && unit.CanMove(target))
            {
                this.SetUnit(target.X, target.Y, unit);
                this.SetUnit(source.X, source.Y, null);
                Vector3 cellPosition = this.GetCell(target.X, target.Y).transform.position;
                Vector3 unitPosition = new Vector3(cellPosition.x, this.UnitHeight, cellPosition.z);
                unit.MoveTo(target, unitPosition);
            }
        }

        /// <summary>
        /// Finds out if the given cell is occupied
        /// </summary>
        /// <param name="coord">Coordinate of the cell</param>
        /// <returns>Whether or not the cell is occupied</returns>
        public bool IsOccupied(CellCoordinate coord)
        {
            if (this.GetUnit(coord.X, coord.Y) != null)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Spawns a unit
        /// </summary>
        /// <param name="unitType">Type of unit to spawn</param>
        /// <param name="coord">Location to spawn it</param>
        /// <param name="owner">Owner of the unit</param>
        public void SpawnUnit(UnitType unitType, CellCoordinate coord, PlayerController owner)
        {
            Unit prefab;
            switch (unitType)
            {
                case UnitType.Cube:
                    prefab = this.CubeUnitPrefab;
                    break;

                case UnitType.Pyramid:
                    prefab = this.PyramidUnitPrefab;
                    break;

                default:
                    prefab = null;
                    break;
            }

            CellController cell = this.GetCell(coord.X, coord.Y);
            if (prefab != null && cell != null && !this.IsOccupied(coord))
            {
                Vector3 cellLoc = cell.transform.position;
                var unit = (Unit)Instantiate(prefab);
                unit.transform.position = new Vector3(cellLoc.x, this.UnitHeight, cellLoc.z);
                unit.BoardController = this;
                unit.Coordinate = coord;
                unit.Mesh.renderer.material.color = owner.Color;
                this.SetUnit(coord.X, coord.Y, unit);
                owner.AddUnit(unit);
            }
        }

        /// <summary>
        /// Selects a unit
        /// </summary>
        /// <param name="unit">Unit to select</param>
        public void ProcessSelection(Unit unit)
        {
            if (unit != null)
            {
                if (this.selection == null && this.TurnController.TurnToPlay == unit.Owner)
                {
                    this.selection = unit;
                    unit.Select();

                    var cellList = this.GetCellsAsList();

                    // if the unit can still move, highlight the possible moves
                    if (this.TurnController.UnitMoved == null)
                    {
                        List<CellController> cellsToHighlightForMove = cellList.Where(cell => unit.CanMove(cell.Coordinate)).ToList();
                        this.HighlightCells(cellsToHighlightForMove, CellHighlightMode.Move);
                    }

                    // highlight possible attacks
                    List<CellController> cellsToHighlightForAttack = cellList.Where(cell => unit.CanAttack(this.GetUnit(cell))).ToList();
                    this.HighlightCells(cellsToHighlightForAttack, CellHighlightMode.Attack);

                    // if the unit can heal, highlight the possible heals
                    var pyramid = unit as PyramidController;
                    if (pyramid != null)
                    {
                        List<CellController> cellsToHighlightForHeal = cellList.Where(cell => pyramid.CanHeal(this.GetUnit(cell))).ToList();
                        this.HighlightCells(cellsToHighlightForHeal, CellHighlightMode.Heal);
                    }
                }
                else if (this.selection != null && this.selection != unit)
                {
                    if (this.selection.ActOn(unit))
                    {
                        this.TurnController.HandleUnitAction();
                    }

                    // in case it just died
                    if (unit != null)
                    {
                        unit.Deselect();
                    }

                    this.CancelSelection();
                }
                else
                {
                    // Only allow selection cancelling if a unit hasn't already moved
                    if (this.TurnController.UnitMoved == null)
                    {
                        this.CancelSelection();
                    }
                }
            }
        }

        /// <summary>
        /// Selects a cell
        /// </summary>
        /// <param name="cell">Cell to select</param>
        public void ProcessSelection(CellController cell)
        {
            if (this.IsOccupied(cell.Coordinate))
            {
                var unit = this.GetUnit(cell);
                this.ProcessSelection(unit);
                return;
            }
            
            if (this.selection != null && this.TurnController.UnitMoved == null)
            {
                this.Move(this.selection.Coordinate, cell.Coordinate);
                this.TurnController.HandleUnitMove(this.selection);
                var temp = this.selection;
                this.CancelSelection();
                this.ProcessSelection(temp);
            }
        }

        /// <summary>
        /// Clears selection
        /// </summary>
        public void CancelSelection()
        {
            if (this.selection != null)
            {
                this.selection.Deselect();
                this.selection = null;

                this.ClearHighlightedCells();
            }
        }

        /// <summary>
        /// Highlights a list of cells
        /// </summary>
        /// <param name="list">List of cells</param>
        /// <param name="mode">Cell highlight mode</param>
        public void HighlightCells(IList<CellController> list, CellHighlightMode mode)
        {
            if (this.highlightedCells == null)
            {
                this.highlightedCells = new List<CellController>();
            }

            foreach (var cell in list)
            {
                if (cell != null)
                {
                    this.highlightedCells.Add(cell);
                    cell.HighlightCellForMode(mode);
                }
            }
        }

        /// <summary>
        /// Highlights a list of cells
        /// </summary>
        /// <param name="list">List of coordinates</param>
        /// <param name="mode">Cell highlight mode</param>
        public void HighlightCells(IList<CellCoordinate> list, CellHighlightMode mode)
        {
            List<CellController> cells = new List<CellController>();
            foreach (CellCoordinate c in list)
            {
                CellController cell = this.GetCell(c);
                if (cell != null)
                {
                    cells.Add(cell);
                }
            }

            this.HighlightCells(cells, mode);
        }

        /// <summary>
        /// Clears all highlighted cells
        /// </summary>
        public void ClearHighlightedCells()
        {
            if (this.highlightedCells != null)
            {
                foreach (var cell in this.highlightedCells)
                {
                    cell.HighlightCellForMode(CellHighlightMode.None);
                }

                this.highlightedCells.Clear();
            }

            this.highlightedCells = null;
        }

        /// <summary>
        /// Gets all cells as a list
        /// Removes the null cells
        /// </summary>
        /// <returns>List of cells</returns>
        public List<CellController> GetCellsAsList()
        {
            List<CellController> list = new List<CellController>();
            foreach (CellController c in this.Cells)
            {
                if (c != null)
                {
                    list.Add(c);
                }
            }

            return list;
        }

        /// <summary>
        /// Gets all units as a list
        /// Removes the null units
        /// </summary>
        /// <returns>List of units</returns>
        public List<Unit> GetUnitsAsList()
        {
            List<Unit> list = new List<Unit>();
            foreach (Unit u in this.Units)
            {
                if (u != null)
                {
                    list.Add(u);
                }
            }

            return list;
        }

        /// <summary>
        /// Remove a unit from the board
        /// </summary>
        /// <param name="unit">Unit to remove</param>
        public void RemoveUnit(Unit unit)
        {
            this.SetUnit(unit.Coordinate, null);
        }
        
        /// <summary>
        /// Assigns a cell controller to a certain coordinate
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="cell">Cell to assign</param>
        private void SetCell(int x, int y, CellController cell)
        {
            this.Cells[x + this.GridSize, y + this.GridSize] = cell;
        }

        /// <summary>
        /// Gets the cell controller from the given coordinate
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <returns>Cell controller for the coordinate</returns>
        private CellController GetCell(int x, int y)
        {
            return this.Cells[x + this.GridSize, y + this.GridSize];
        }

        /// <summary>
        /// Assigns a cell controller to a certain coordinate
        /// </summary>
        /// <param name="coord">Cell coordinate</param>
        /// <param name="cell">Cell to assign</param>
        private void SetCell(CellCoordinate coord, CellController cell)
        {
            this.SetCell(coord.X, coord.Y, cell);
        }

        /// <summary>
        /// Gets the cell controller from the given coordinate
        /// </summary>
        /// <param name="coord">Cell coordinate</param>
        /// <returns>Cell controller for the coordinate</returns>
        private CellController GetCell(CellCoordinate coord)
        {
            return this.GetCell(coord.X, coord.Y);
        }

        /// <summary>
        /// Sets the unit on a given cell
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="unit">Unit to place</param>
        private void SetUnit(int x, int y, Unit unit)
        {
            this.Units[x + this.GridSize, y + this.GridSize] = unit;
        }

        /// <summary>
        /// Gets any units on a given cell
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <returns>Any units standing on the cell</returns>
        private Unit GetUnit(int x, int y)
        {
            return this.Units[x + this.GridSize, y + this.GridSize];
        }

        /// <summary>
        /// Gets any units on a given cell
        /// </summary>
        /// <param name="cell">Cell to inspect</param>
        /// <returns>Any units standing on the cell</returns>
        private Unit GetUnit(CellController cell)
        {
            return this.GetUnit(cell.Coordinate);
        }

        /// <summary>
        /// Gets any units on a given cell
        /// </summary>
        /// <param name="coord">Coordinate to inspect</param>
        /// <returns>Any units standing on the cell</returns>
        private Unit GetUnit(CellCoordinate coord)
        {
            return this.GetUnit(coord.X, coord.Y);
        }

        /// <summary>
        /// Sets the unit on a given cell
        /// </summary>
        /// <param name="coord">Coodinate to set</param>
        /// <param name="unit">Unit to place</param>
        private void SetUnit(CellCoordinate coord, Unit unit)
        {
            this.SetUnit(coord.X, coord.Y, unit);
        }

        /// <summary>
        /// Checks to see if the given player has any enemy units left on the board
        /// </summary>
        /// <param name="player">The player to check</param>
        /// <returns>Whether or not the player has won</returns>
        private bool CheckForWin(PlayerController player)
        {
            foreach (Unit u in this.GetUnitsAsList())
            {
                if (u.Owner != player)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
