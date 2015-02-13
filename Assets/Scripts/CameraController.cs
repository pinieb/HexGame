//-----------------------------------------------------------------------
// <copyright file="CameraController.cs" company="Pete Biencourt">
//     Copyright (c) Pete Biencourt. All rights reserved.
// </copyright>
// <author>Pete Biencourt</author>
//-----------------------------------------------------------------------

namespace HexGame
{
    using System.Diagnostics.CodeAnalysis;
    using UnityEngine;

    /// <summary>
    /// Camera controller
    /// </summary>
    public class CameraController : MonoBehaviour
    {
        /// <summary>
        /// Zoom step size
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public float ZoomStepSize = 5f;

        /// <summary>
        /// Minimum camera height
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public float MinYValue = 2f;

        /// <summary>
        /// Maximum camera height
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public float MaxYValue = 10f;

        /// <summary>
        /// Starts the camera
        /// </summary>
        public void Start()
        {
            Camera.main.transform.LookAt(Vector3.zero);
        }

        /// <summary>
        /// Updates the camera
        /// </summary>
        public void Update()
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                this.transform.RotateAround(Vector3.zero, Vector3.up, -20 * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                this.transform.RotateAround(Vector3.zero, Vector3.up, 10 * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow))
            {
                this.Zoom((-1) * this.ZoomStepSize);
            }

            if (!Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.DownArrow))
            {
                this.Zoom(this.ZoomStepSize);
            }
        }

        /// <summary>
        /// Zooms the camera in or out
        /// </summary>
        /// <param name="zoomDelta">How much to zoom</param>
        private void Zoom(float zoomDelta)
        {
            Vector3 pos = Camera.main.transform.position;
            pos.y += Time.deltaTime * zoomDelta;
            if (pos.y < this.MinYValue)
            {
                pos.y = this.MinYValue;
            }

            if (pos.y > this.MaxYValue)
            {
                pos.y = this.MaxYValue;
            }

            Camera.main.transform.position = pos;

            Camera.main.transform.LookAt(Vector3.zero);
        }
    }
}