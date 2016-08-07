using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EasyMatrix
{
    /// <summary>
    /// Логика взаимодействия для MaskedTextBox.xaml
    /// </summary>
    public partial class MaskedTextBox : UserControl
    {
        public MaskedTextBox()
        {
            InitializeComponent();
            TextProperty = DependencyProperty.Register("MyText", typeof(string), typeof(MaskedTextBox));
        }

        public static DependencyProperty TextProperty;

        public string MyText
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value);  TextBox.Text = value; }
        }

        public new double FontSize
        {
            get { return TextBox.FontSize; }
            set
            {
                TextBox.FontSize = value;
            }
        }

        public int MaxLength
        {
            get { return TextBox.MaxLength; }
            set { TextBox.MaxLength = value; }
        }
    }
}
