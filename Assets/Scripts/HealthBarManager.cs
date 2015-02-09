//-----------------------------------------------------------------------
// <copyright file="HealthBarManager.cs" company="Pete Biencourt">
//     Copyright (c) Pete Biencourt. All rights reserved.
// </copyright>
// <author>Pete Biencourt</author>
//-----------------------------------------------------------------------

namespace HexGame
{
    using System.Diagnostics.CodeAnalysis;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// Health bar manager
    /// </summary>
    public class HealthBarManager : MonoBehaviour
    {
        /// <summary>
        /// Health bar's owner
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public Unit Owner;

        /// <summary>
        /// Fill image
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public Image FillImage;

        /// <summary>
        /// Background image
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public Image BackgroundImage;

        /// <summary>
        /// Left outline image
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public Image LeftOutlineImage;

        /// <summary>
        /// Right outline image
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public Image RightOutlineImage;

        /// <summary>
        /// Top outline image
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public Image TopOutlineImage;

        /// <summary>
        /// Bottom outline image
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public Image BottomOutlineImage;

        /// <summary>
        /// Highlight alpha
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public float HighlightAlpha = 0.8f;

        /// <summary>
        /// Lowlight alpha
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public float LowlightAlpha = 0.2f;

        /// <summary>
        /// Min health color
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public Color MinHealthColor = Color.red;

        /// <summary>
        /// Max health color
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public Color MaxHealthColor = Color.green;

        /// <summary>
        /// Starts the object
        /// </summary>
        public void Start()
        {
            this.Lowlight();
        }

        /// <summary>
        /// Updates the GUI on each GUI call
        /// </summary>
        public void OnGUI()
        {
            this.FillImage.rectTransform.localScale = new Vector3(this.Owner.Health / (float)this.Owner.MaxHealth, 1f, 1f);
            float alpha = this.FillImage.color.a;
            Color c = Color.Lerp(this.MinHealthColor, this.MaxHealthColor, Mathf.Lerp(0f, 1f, this.FillImage.rectTransform.localScale.x));
            c.a = alpha;
            this.FillImage.color = c;
        }

        /// <summary>
        /// Highlights the health bar
        /// </summary>
        public void Highlight()
        {
            this.SetImageAlpha(this.FillImage, this.HighlightAlpha);
            this.SetImageAlpha(this.BackgroundImage, this.HighlightAlpha);
            this.SetImageAlpha(this.LeftOutlineImage, this.HighlightAlpha);
            this.SetImageAlpha(this.RightOutlineImage, this.HighlightAlpha);
            this.SetImageAlpha(this.TopOutlineImage, this.HighlightAlpha);
            this.SetImageAlpha(this.BottomOutlineImage, this.HighlightAlpha);
        }

        /// <summary>
        /// Lowlights the health bar
        /// </summary>
        public void Lowlight()
        {
            this.SetImageAlpha(this.FillImage, this.LowlightAlpha);
            this.SetImageAlpha(this.BackgroundImage, this.LowlightAlpha);
            this.SetImageAlpha(this.LeftOutlineImage, this.LowlightAlpha);
            this.SetImageAlpha(this.RightOutlineImage, this.LowlightAlpha);
            this.SetImageAlpha(this.TopOutlineImage, this.LowlightAlpha);
            this.SetImageAlpha(this.BottomOutlineImage, this.LowlightAlpha);
        }

        /// <summary>
        /// Sets the alpha
        /// </summary>
        /// <param name="image">Image to adjust</param>
        /// <param name="alpha">Alpha value</param>
        private void SetImageAlpha(Image image, float alpha)
        {
            var color = image.color;
            color.a = alpha;
            image.color = color;
        }
    }
}
