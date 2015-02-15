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
        /// Move speed
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public float MoveSpeed = 1f;

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
        /// Health bar manager
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public HealthBarManager HealthBarManager;

        /// <summary>
        /// Object mesh
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public MeshRenderer Mesh;

        /// <summary>
        /// Float animation speed
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public float FloatAnimationSpeed = .2f;

        /// <summary>
        /// Float max height
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public float FloatAnimationMaxHeight = .5f;

        /// <summary>
        /// Float min height
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public float FloatAnimationMinHeight = .25f;

        /// <summary>
        /// Float animation random range limits
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public float FloatAnimationRandomRangeLimits = .02f;

        /// <summary>
        /// Unit health
        /// </summary>
        private int health;

        /// <summary>
        /// Is the unit selected
        /// </summary>
        private bool selected;

        /// <summary>
        /// Target position
        /// </summary>
        private Vector3 targetPosition;

        /// <summary>
        /// Whether or not the animation is currently ascending or descending
        /// </summary>
        private bool ascending;

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
        /// Gets a value indicating whether or not the unit is selected
        /// </summary>
        public bool IsSelected
        {
            get
            {
                return this.selected;
            }
        }

        /// <summary>
        /// Gets a value indicating whether or not the unit is moving
        /// </summary>
        public bool IsMoving
        {
            get;
            private set;
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
            this.HealthBarManager.Highlight();
            this.BoardController.HighlightCells(new List<CellCoordinate>() { this.Coordinate }, CellHighlightMode.Selected);
        }

        /// <summary>
        /// Deselects the unit
        /// </summary>
        public void Deselect()
        {
            this.selected = false;
            this.HealthBarManager.Lowlight();
        }

        /// <summary>
        /// Moves to the given coordinate and update the unit's world position
        /// </summary>
        /// <param name="coord">Cell coordinate</param>
        /// <param name="worldPosition">World position</param>
        public virtual void MoveTo(CellCoordinate coord, Vector3 worldPosition)
        {
            GameLogger.Instance.AddMove(ActionRecord.MoveAction(this, this.Coordinate, coord));
            this.Coordinate = coord;
            this.IsMoving = true;
            this.targetPosition = worldPosition;
        }

        /// <summary>
        /// Runs on every frame
        /// </summary>
        public void Update()
        {
            if (this.IsMoving)
            {
                if (this.transform.position == this.targetPosition)
                {
                    this.IsMoving = false;
                }
                else
                {
                    float step = this.MoveSpeed * Time.deltaTime;
                    this.transform.position = Vector3.MoveTowards(this.transform.position, this.targetPosition, step);
                }
            }

            if (this.Mesh.transform.position.y == this.FloatAnimationMaxHeight && this.ascending)
            {
                this.ascending = false;
            }
            else if (this.Mesh.transform.position.y == this.FloatAnimationMinHeight && !this.ascending)
            {
                this.ascending = true;
            }

            float entropy = Random.Range(-1 * this.FloatAnimationRandomRangeLimits, this.FloatAnimationRandomRangeLimits);
            float animationStep = (this.FloatAnimationSpeed + entropy) * Time.deltaTime;
            if (this.ascending)
            {
                this.Mesh.transform.position = Vector3.MoveTowards(this.Mesh.transform.position, 
                    new Vector3(this.Mesh.transform.position.x, this.FloatAnimationMaxHeight, this.Mesh.transform.position.z), 
                    animationStep);
            }
            else 
            {
                this.Mesh.transform.position = Vector3.MoveTowards(this.Mesh.transform.position, 
                    new Vector3(this.Mesh.transform.position.x, this.FloatAnimationMinHeight, this.Mesh.transform.position.z), 
                    animationStep);
            }
        }

        /// <summary>
        /// Prepares the unit for the next turn
        /// </summary>
        public virtual void ResetUnit()
        {
            this.CanAct = true;
        }

        /// <summary>
        /// Turns the unit to face the given position
        /// </summary>
        /// <param name="worldPosition">Position to face</param>
        /// <param name="shouldSnap">Whether or not the look should snap to certain angles</param>
        public virtual void LookAt(Vector3 worldPosition, bool shouldSnap = false)
        {
            return;
        }

        /// <summary>
        /// Starts the unit object
        /// </summary>
        public void Start()
        {
            this.Health = this.MaxHealth;
        }

        /// <summary>
        /// On mouse over handler
        /// </summary>
        public virtual void OnMouseOver()
        {
            if (!this.selected)
            {
                this.HealthBarManager.Highlight();
            }
        }

        /// <summary>
        /// On mouse exit handler
        /// </summary>
        public virtual void OnMouseExit()
        {
            if (!this.selected)
            {
                this.HealthBarManager.Lowlight();
            }
        }

        /// <summary>
        /// On mouse down handler
        /// </summary>
        public virtual void OnMouseDown()
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
            GameLogger.Instance.AddMove(ActionRecord.AttackAction(this, this.Coordinate, unit, unit.Coordinate));
            unit.Health -= this.AttackPower;
        }
    }
}