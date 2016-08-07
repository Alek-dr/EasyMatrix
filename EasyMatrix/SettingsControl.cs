using System.Windows.Controls;

namespace EasyMatrix
{
    static class SettingsControl
    {
        private static int _fontSize = EasyMatrix.Properties.Settings.Default.FontSize;

        private static int _font = EasyMatrix.Properties.Settings.Default.Font;

        private static int _round = EasyMatrix.Properties.Settings.Default.Round;

        private static bool _comments = EasyMatrix.Properties.Settings.Default.Comments;

        public static int FontSize
        {
            get { return _fontSize; }
            set
            {
                if (value > 0)
                {
                    _fontSize = value;
                    EasyMatrix.Properties.Settings.Default.FontSize = value;
                    Properties.Settings.Default.Save();
                }
            }
        }

        public static int Round
        {
            get { return _round; }
            set
            {
                if(value >= 0 )
                {
                    _round = value;
                    EasyMatrix.Properties.Settings.Default.Round = value;
                    Properties.Settings.Default.Save();
                }
            }
        }

        public static int Font
        {
            get { return _font; }
            set
            {
                if (value >= 0)
                {
                    _font = value;
                    EasyMatrix.Properties.Settings.Default.Font = value;
                    Properties.Settings.Default.Save();
                }
            }
        }

        public static bool Comments
        {
            get { return _comments; }
            set
            {
                _comments = value;
                EasyMatrix.Properties.Settings.Default.Comments = value;
                Properties.Settings.Default.Save();
            }
        }
           
        public static TextBox TextBox { get; set; }

    }
}
