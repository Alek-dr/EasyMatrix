using GalaSoft.MvvmLight;

namespace EasyMatrix
{
    class SettingsModel : ViewModelBase
    {
        public int FontSize
        {
            get { return SettingsControl.FontSize; }
            set { SettingsControl.FontSize = value; }
        }

        public int Font
        {
            get { return SettingsControl.Font; }
            set { SettingsControl.Font = value; }
        }

        public int Round
        {
            get { return SettingsControl.Round; }
            set { SettingsControl.Round = value; }
        }

        public bool Comments
        {
            get { return SettingsControl.Comments; }
            set { SettingsControl.Comments = value; }
        }
    }
}
