using MyWay.Passport.Mobile.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(NavigationPage), typeof(NavigationPageRenderer))]
namespace MyWay.Passport.Mobile.iOS.Renderers
{
    public class NavigationPageRenderer : NavigationRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            // Disable NavigationBard shadow
            NavigationBar.ShadowImage = new UIImage();
        }
    }
}
