﻿using System.Drawing;
using CoreAnimation;
using CoreGraphics;
using MyWay.Passport.Mobile.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Entry), typeof(ExtendedEntryRenderer))]
namespace MyWay.Passport.Mobile.iOS.Renderers
{
    public class ExtendedEntryRenderer : EntryRenderer
    {
        private CALayer borderLayer;

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Element == null)
                return;

            // Add Done button
            AddDoneButton();

            // Set border colour
            SetBorderColor();
        }

		/// <summary>
		/// Add toolbar with Done button
		/// </summary>
		protected void AddDoneButton()
		{
            Control.InputAccessoryView = new UIToolbar(new RectangleF(0.0f, 0.0f, 50.0f, 44.0f))
			{
				Items = new UIBarButtonItem[]
				{
					new UIBarButtonItem (UIBarButtonSystemItem.FlexibleSpace),
					new UIBarButtonItem(UIBarButtonSystemItem.Done, delegate
					{
						Control.ResignFirstResponder();
						var baseEntry = Element.GetType();
						((IEntryController)Element).SendCompleted();
					})
				}
			};
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

