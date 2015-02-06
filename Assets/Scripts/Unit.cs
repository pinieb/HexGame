//-----------------------------------------------------------------------
// <copyright file="Unit.cs" company="Pete Biencourt">
//     Copyright (c) Pete Biencourt. All rights reserved.
// </copyright>
// <author>Pete Biencourt</author>
//-----------------------------------------------------------------------

namespace HexGame
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using UnityEngine;

    /// <summary>
    /// Unit class
    /// </summary>
    public class Unit : MonoBehaviour, ISelectable
    {
        /// <summary>
        /// Board controller
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public BoardController BoardController;

        /// <summary>
        /// Max move range
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public int MaxMoveRange;
        
        /// <summary>
        /// Max health
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public int MaxHealth;
        
        /// <summary>
        /// Attack power
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public int AttackPower;

        /// <summary>
        /// Attack range
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public int AttackRange;

        /// <summary>
        /// Unit coordinate
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public CellCoordinate Coordinate;

        /// <summary>
        /// Health bar offset
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public double HealthBarOffset = .53;

        /// <summary>
        /// Health bar
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public MeshRenderer HealthBar;

        /// <summary>
        /// Object mesh
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public MeshRenderer Mesh;

        /// <summary>
        /// Unit health
        /// </summary>
        private int health;

        /// <summary>
        /// Is the unit selected
        /// </summary>
        private bool selected;

        /// <summary>
        /// Gets or sets unit health
        /// </summary>
        public int Health
        {
            get
            {
                return this.health;
            }

            set
            {
                if (value <= 0)
                {
                    // die
                    this.health = 0;
                    Unit.DestroyImmediate(this.gameObject);
                    this.BoardController.RemoveUnit(this);
                    this.Owner.RemoveUnit(this);
                    return;
                }
                else if (value > this.MaxHealth)
                {
                    this.health = this.MaxHealth;
                }
                else
                {
                    this.health = value;
                }

                double offset = this.HealthBarOffset * (1 - (this.health / (double)this.MaxHealth));
                this.HealthBar.material.mainTextureOffset = new Vector2((float)offset, 0.0f);
            }
        }

        /// <summary>
        /// Gets or sets the owner of the unit
        /// </summary>
        public PlayerController Owner
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the unit can act
        /// </summary>
        public virtual bool CanAct
        {
            get;
            protected set;
        }

        /// <summary>
        /// Returns whether the unit can move to the given coordinate
        /// </summary>
        /// <param name="coord">Coordinate to check</param>
        /// <returns>Whether or not the unit can move to the coordinate</returns>
        public virtual bool CanMove(CellCoordinate coord)
        {
            if (HexDistance.Distance(this.Coordinate, coord) <= this.MaxMoveRange && !this.BoardController.IsOccupied(coord))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Determines whether or not an attack against the given unit is possible
        /// </summary>
        /// <param name="unit">Unit to attack</param>
        /// <returns>Whether or not the unit can be attacked</returns>
        public virtual bool CanAttack(Unit unit)
        {
            if (unit != null && unit != this && unit.Owner != this.Owner &&
                HexDistance.Distance(this.Coordinate, unit.Coordinate) <= this.AttackRange)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Performs an action on another unit
        /// </summary>
        /// <param name="unit">Unit to act on</param>
        /// <returns>Whether or not the action was successful</returns>
        public virtual bool ActOn(Unit unit)
        {
            if (this.CanAttack(unit))
            {
                this.Attack(unit);
                this.CanAct = false;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Selects the unit
        /// </summary>
        public void Select()
        {
            this.selected = true;
            this.BoardController.HighlightCells(new List<CellCoordinate>() { this.Coordinate }, CellHighlightMode.Selected);
            
            // this.renderer.material.shader = Shader.Find("Outlined/Silhouetted Diffuse");
        }

        /// <summary>
        /// Deselects the unit
        /// </summary>
        public void Deselect()
        {
            this.selected = false;
            this.renderer.material.shader = Shader.Find("Diffuse");
        }

        /// <summary>
        /// Moves to the given coordinate and update the unit's world position
        /// </summary>
        /// <param name="coord">Cell coordinate</param>
        /// <param name="worldPosition">World position</param>
        public void MoveTo(CellCoordinate coord, Vector3 worldPosition)
        {
            this.Coordinate = coord;
            this.transform.position = worldPosition;
        }

        /// <summary>
        /// Prepares the unit for the next turn
        /// </summary>
        public virtual void ResetUnit()
        {
            this.CanAct = true;
        }

        /// <summary>
        /// Starts the unit object
        /// </summary>
        protected void Start()
        {
            this.Health = this.MaxHealth;
        }

        /// <summary>
        /// On mouse down handler
        /// </summary>
        protected virtual void OnMouseDown()
        {
            if (!this.selected)
            {
                this.BoardController.ProcessSelection(this);
            }
            else
            {
                this.BoardController.ProcessSelection(this);
            }
        }

        /// <summary>
        /// Attacks another unit
        /// </summary>
        /// <param name="unit">Unit to attack</param>
        protected virtual void Attack(Unit unit)
        {
            unit.Health -= this.AttackPower;
        }
    }
}