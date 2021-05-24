namespace MyWay.Passport.Mobile.Models
{
    public class CardColour : BaseModel
    {
        private ColourNames name;
        public ColourNames Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }

        private string backgroundColour;
        public string BackgroundColour
        {
            get { return backgroundColour; }
            set
            {
                backgroundColour = value;
                OnPropertyChanged();
            }
        }

        private string textColour;
        public string TextColour
        {
            get { return textColour; }
            set
            {
                textColour = value;
                OnPropertyChanged();
            }
        }
    }
}
