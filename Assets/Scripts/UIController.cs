//-----------------------------------------------------------------------
// <copyright file="UIController.cs" company="Pete Biencourt">
//     Copyright (c) Pete Biencourt. All rights reserved.
// </copyright>
// <author>Pete Biencourt</author>
//-----------------------------------------------------------------------

namespace HexGame
{
    using System.Diagnostics.CodeAnalysis;
    using UnityEngine;

    /// <summary>
    /// UI controller
    /// </summary>
    public class UIController : MonoBehaviour
    {
        /// <summary>
        /// Board controller
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public BoardController BoardController;

        /// <summary>
        /// Turn controller
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public TurnController TurnController;

        /// <summary>
        /// Turn indicator
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public GUIText TurnIndicator;

        /// <summary>
        /// Handles GUI events
        /// </summary>
        public void OnGUI()
        {
            if (!this.TurnController.GameOver)
            {
                this.TurnIndicator.text = "Player " + this.TurnController.PlayerToMove.Id;

                if (GUI.Button(new Rect((Screen.width / 2) - 50, (Screen.height / 10) - 5, 100, 20), "End Turn"))
                {
                    this.BoardController.CancelSelection();
                    this.TurnController.EndTurn();
                }
            }
            else
            {
                this.TurnIndicator.text = "Game Over!";
            }
        }
    }
}
