using System;
using MyWay.Passport.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyWay.Passport.Mobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        private MainViewModel viewModel;

        public MainPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new MainViewModel(Navigation);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.OnViewAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            viewModel.OnViewDisappearing();
        }

        private void CarouselViewContent_SizeChanged(object sender, System.EventArgs e)
        {
            var carouselView = (CarouselView)sender;

            if (carouselView == null)
            {
                throw new InvalidOperationException("Sender cannot be null");
            }

            if (carouselView.Height > carouselView.HeightRequest)
            {
                //carouselView.HeightRequest = CardFrame.Height;
            }
        }
    }
}
