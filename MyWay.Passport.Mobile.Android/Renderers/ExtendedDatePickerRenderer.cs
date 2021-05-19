using Android.Content;
using Android.Views;
using MyWay.Passport.Mobile.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(DatePicker), typeof(ExtendedDatePickerRenderer))]
namespace MyWay.Passport.Mobile.Droid.Renderers
{
    /// <summary>
    /// Disables border on Android date picker.
    /// </summary>
    public class ExtendedDatePickerRenderer : DatePickerRenderer
    {
        public ExtendedDatePickerRenderer(Context context) : base(context) { }

        protected override void OnElementChanged(ElementChangedEventArgs<DatePicker> e)
        {
            base.OnElementChanged(e);

            if (Control != null && Element != null)
            {
                SetBorderColour();
            }
        }

        /// <summary>
        /// Hides the default border for custom date pickers.
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
