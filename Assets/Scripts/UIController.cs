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
        /// GUI skin
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public GUISkin Skin;

        /// <summary>
        /// Instruction pane
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public GameObject InstructionPanePrefab;

        private GameObject instructionsPane;

        public void Update()
        {
            if (this.instructionsPane != null && Input.anyKey)
            {
                GameObject.Destroy(this.instructionsPane);
            }
        }

        /// <summary>
        /// Handles GUI events
        /// </summary>
        public void OnGUI()
        {
            if (this.instructionsPane == null)
            {
                GUI.skin = this.Skin;
                if (!this.TurnController.GameOver)
                {
                    GUI.Label(new Rect((Screen.width / 2) - 40, (Screen.height / 10) - 40, 100, 40), "Player " + this.TurnController.PlayerToMove.Id);

                    if (GUI.Button(new Rect((Screen.width / 2) - 70, Screen.height / 10, 140, 40), "End Turn"))
                    {
                        this.BoardController.CancelSelection();
                        this.TurnController.EndTurn();
                    }

                    if (GUI.Button(new Rect(Screen.width - 200, (Screen.height / 10) - 40, 200, 40), "Instructions"))
                    {
                        this.instructionsPane = (GameObject)Instantiate(this.InstructionPanePrefab);
                    }
                }
                else
                {
                    GUI.Label(new Rect((Screen.width / 2) - 68, (Screen.height / 10) - 40, 200, 40), "Game Over!");
                    if (GUI.Button(new Rect((Screen.width / 2) - 100, Screen.height / 10, 200, 40), "Play again"))
                    {
                        Application.LoadLevel(Application.loadedLevel);
                    }
                }
            }
        }
    }
}
