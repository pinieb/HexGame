//-----------------------------------------------------------------------
// <copyright file="MenuController.cs" company="Pete Biencourt">
//     Copyright (c) Pete Biencourt. All rights reserved.
// </copyright>
// <author>Pete Biencourt</author>
//-----------------------------------------------------------------------

namespace HexGame
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using UnityEngine;

    /// <summary>
    /// Menu controller
    /// </summary>
    public class MenuController : MonoBehaviour
    {
        /// <summary>
        /// GUI skin
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public GUISkin Skin;

        /// <summary>
        /// Scene name
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public string SceneName;

        /// <summary>
        /// Handles GUI events
        /// </summary>
        public void OnGUI()
        {
            GUI.skin = this.Skin;

            GUI.Label(new Rect((Screen.width / 2) - 100, (Screen.height / 3) + 20, 200, 40), "Hex");
            if (GUI.Button(new Rect((Screen.width / 2) - 100, (Screen.height / 2) - 20, 200, 40), "Play"))
            {
                Application.LoadLevel(this.SceneName);
            }
        }
    }
}
