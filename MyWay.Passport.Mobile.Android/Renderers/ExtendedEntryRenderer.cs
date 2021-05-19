using Android.Content;
using Android.Views;
using MyWay.Passport.Mobile.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Entry), typeof(ExtendedEntryRenderer))]
namespace MyWay.Passport.Mobile.Droid.Renderers
{
    /// <summary>
    /// Disables border on Android entry.
    /// </summary>
    public class ExtendedEntryRenderer : EntryRenderer
    {
        public ExtendedEntryRenderer(Context context) : base(context) { }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null && Element != null)
            {
                SetBorderColour();
            }
        }

        /// <summary>
        /// Hides the default border for custom entries.
        /// </summary>
        private void SetBorderColour()
        {
            if (Control == null)
            {
                return;
            }

            // Hide border and center text
            Control.Background.Alpha = 0;
            Control.SetPadding(Control.PaddingLeft, 0, Control.PaddingRight, 0);
            Control.Gravity = GravityFlags.CenterVertical;
        }
    }
}
