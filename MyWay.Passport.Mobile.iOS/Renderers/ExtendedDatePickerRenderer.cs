using System;
using CoreAnimation;
using CoreGraphics;
using MyWay.Passport.Mobile.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(DatePicker), typeof(ExtendedDatePickerRenderer))]
namespace MyWay.Passport.Mobile.iOS.Renderers
{
    public class ExtendedDatePickerRenderer : DatePickerRenderer
    {
        private CALayer borderLayer;

        protected override void OnElementChanged(ElementChangedEventArgs<DatePicker> e)
        {
            base.OnElementChanged(e);

            if (Element == null)
                return;

            // Set border colour
            SetBorderColor();
        }

        /// <summary>
        /// Set border colour to transparent.
        /// </summary>
        private void SetBorderColor()
        {
            if (Control == null || Element == null)
            {
                return;
            }

            // single line border
            if (borderLayer == null)
            {
                borderLayer = new CALayer
                {
                    MasksToBounds = true,
                    Frame = new CGRect(0f, Frame.Height / 2, Frame.Width, 1f),
                    BorderWidth = 1.0f
                };
                Control.Layer.AddSublayer(borderLayer);
                Control.BorderStyle = UITextBorderStyle.None;
            }

            // disable autocapitalization
            borderLayer.BorderColor = UIColor.FromRGBA(0, 0, 0, 0).CGColor;
        }
    }
}
