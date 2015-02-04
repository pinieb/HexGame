//-----------------------------------------------------------------------
// <copyright file="MapController.cs" company="Pete Biencourt">
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
    using UnityEngine;

    /// <summary>
    /// Map controller
    /// </summary>
    public class MapController : MonoBehaviour
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
        /// Gets or sets the players list
        /// </summary>
        public List<PlayerController> Players
        {
            get;
            set;
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
        /// Initialize the map controller
        /// </summary>
        public void Start()
        {
            this.Players = new List<PlayerController>();
            this.Players.Add(new PlayerController(0));
            this.Players[0].Color = Color.red;
            this.Players.Add(new PlayerController(1));
            this.Players[1].Color = Color.cyan;

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
                        cellController.MapController = this;
                        cellController.Coordinate = cellCoord;
                    }
                }
            }

            var spawnCoord = new CellCoordinate(1, 2);
            this.SpawnUnit(UnitType.Cube, spawnCoord, this.Players[0]);

            spawnCoord = new CellCoordinate(1, 3);
            this.SpawnUnit(UnitType.Cube, spawnCoord, this.Players[0]);

            spawnCoord = new CellCoordinate(2, 2);
            this.SpawnUnit(UnitType.Cube, spawnCoord, this.Players[1]);
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
                unit.MapController = this;
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
                if (this.selection == null)
                {
                    this.selection = unit;
                    unit.Select();

                    var cellList = this.GetCellsAsList();

                    // highlight possible moves
                    List<CellController> cellsToHighlightForMove = cellList.Where(cell => unit.CanMove(cell.Coordinate)).ToList();
                    this.HighlightCells(cellsToHighlightForMove, CellHighlightMode.Move);

                    // highlight possible attacks
                    List<CellController> cellsToHighlightForAttack = cellList.Where(cell => unit.CanAttack(this.GetUnit(cell))).ToList();
                    this.HighlightCells(cellsToHighlightForAttack, CellHighlightMode.Attack);
                }
                else if (this.selection != null && this.selection != unit)
                {
                    this.selection.ActOn(unit);

                    // in case it just died
                    if (unit != null)
                    {
                        unit.Deselect();
                    }

                    this.CancelSelection();
                }
                else
                {
                    this.CancelSelection();
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
            
            if (this.selection != null)
            {
                this.Move(this.selection.Coordinate, cell.Coordinate);
            }

            this.CancelSelection();
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
    }
}
