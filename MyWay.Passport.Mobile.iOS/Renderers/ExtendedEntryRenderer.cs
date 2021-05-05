using System.Drawing;
using MyWay.Passport.Mobile.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Entry), typeof(ExtendedEntryRenderer))]
namespace MyWay.Passport.Mobile.iOS.Renderers
{
    public class ExtendedEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Element == null)
                return;

            // Add Done button
            AddDoneButton();
        }

		/// <summary>
		/// <para>Add toolbar with Done button</para>
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
	}
}

