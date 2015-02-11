//-----------------------------------------------------------------------
// <copyright file="CameraController.cs" company="Pete Biencourt">
//     Copyright (c) Pete Biencourt. All rights reserved.
// </copyright>
// <author>Pete Biencourt</author>
//-----------------------------------------------------------------------

namespace HexGame
{
    using UnityEngine;

    /// <summary>
    /// Camera controller
    /// </summary>
    public class CameraController : MonoBehaviour
    {
        /// <summary>
        /// Track radius
        /// </summary>
        public float TrackRadius = 7.5f;

        /// <summary>
        /// Rotation step size
        /// </summary>
        public float RotationStepSize = 0.05f;

        /// <summary>
        /// Zoom step size
        /// </summary>
        public float ZoomStepSize = 0.1f;

        /// <summary>
        /// Minimum camera height
        /// </summary>
        public float MinYValue = 2f;

        /// <summary>
        /// Maximum camera height
        /// </summary>
        public float MaxYValue = 10f;

        /// <summary>
        /// Tracks the value of the radian parameter for the camera's track
        /// </summary>
        private float parameterValue;

        /// <summary>
        /// Starts the camera
        /// </summary>
        void Start()
        {
            this.parameterValue = 0f;
            this.UpdateParameterAndGetNewPosition(0f);
        }

        /// <summary>
        /// Updates the camera
        /// </summary>
        public void Update()
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                this.UpdateParameterAndGetNewPosition((-1) * this.RotationStepSize);
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                this.UpdateParameterAndGetNewPosition(this.RotationStepSize);
            }

            if (Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow))
            {
                this.Zoom((-1) * this.ZoomStepSize);
            }

            if (!Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.DownArrow))
            {
                this.Zoom(this.ZoomStepSize);
            }

            Camera.main.transform.LookAt(new Vector3(0f, 0f, 0f));
        }

        /// <summary>
        /// Handles keeping the camera on its track
        /// </summary>
        /// <param name="parameterDelta">How much to move it by</param>
        private void UpdateParameterAndGetNewPosition(float parameterDelta)
        {
            this.parameterValue += parameterDelta;
            Vector3 pos = Camera.main.transform.position;
            pos.z = (-1) * this.TrackRadius * Mathf.Cos(this.parameterValue);
            pos.x = (-1) * this.TrackRadius * Mathf.Sin(this.parameterValue);
            Camera.main.transform.position = pos;
        }

        /// <summary>
        /// Zooms the camera in or out
        /// </summary>
        /// <param name="zoomDelta">How much to zoom</param>
        private void Zoom(float zoomDelta)
        {
            Vector3 pos = Camera.main.transform.position;
            pos.y += zoomDelta;
            if (pos.y < this.MinYValue)
            {
                pos.y = this.MinYValue;
            }
            if (pos.y > this.MaxYValue)
            {
                pos.y = this.MaxYValue;
            }
            Camera.main.transform.position = pos;
        }
    }
}