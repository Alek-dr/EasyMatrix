using System;
using System.Windows;
using System.Windows.Controls;
using System.Globalization;
using System.Text.RegularExpressions;

namespace EasyMatrix
{
    #region Documentation Tags
    /// <summary>
    ///     WPF Maskable TextBox class. Just specify the TextBoxMaskBehavior.Mask attached property to a TextBox. 
    ///     It protect your TextBox from unwanted non numeric symbols and make it easy to modify your numbers.
    /// </summary>
    /// <remarks>
    /// <para>
    ///     Class Information:
    ///	    <list type="bullet">
    ///         <item name="authors">Authors: Ruben Hakopian</item>
    ///         <item name="date">February 2009</item>
    ///         <item name="originalURL">http://www.rubenhak.com/?p=8</item>
    ///     </list>
    /// </para>
    /// </remarks>
    #endregion
    public class TextBoxMaskBehavior
    {
        //Сюда тоже можно пределать регулярные выражения

        #region MinimumValue Property

        public static double GetMinimumValue(DependencyObject obj)
        {
            return (double)obj.GetValue(MinimumValueProperty);
        }

        public static void SetMinimumValue(DependencyObject obj, double value)
        {
            obj.SetValue(MinimumValueProperty, value);
        }

        public static readonly DependencyProperty MinimumValueProperty =
            DependencyProperty.RegisterAttached(
                "MinimumValue",
                typeof(double),
                typeof(TextBoxMaskBehavior),
                new FrameworkPropertyMetadata(double.NaN, MinimumValueChangedCallback)
                );

        private static void MinimumValueChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TextBox _this = (d as TextBox);
            ValidateTextBox(_this);
        }
        #endregion

        #region MaximumValue Property

        public static double GetMaximumValue(DependencyObject obj)
        {
            return (double)obj.GetValue(MaximumValueProperty);
        }

        public static void SetMaximumValue(DependencyObject obj, double value)
        {
            obj.SetValue(MaximumValueProperty, value);
        }

        public static readonly DependencyProperty MaximumValueProperty =
            DependencyProperty.RegisterAttached(
                "MaximumValue",
                typeof(double),
                typeof(TextBoxMaskBehavior),
                new FrameworkPropertyMetadata(double.NaN, MaximumValueChangedCallback)
                );

        private static void MaximumValueChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TextBox _this = (d as TextBox);
            ValidateTextBox(_this);
        }
        #endregion

        #region Mask Property

        public static MaskType GetMask(DependencyObject obj)
        {
            return (MaskType)obj.GetValue(MaskProperty);
        }

        public static void SetMask(DependencyObject obj, MaskType value)
        {
            obj.SetValue(MaskProperty, value);
        }

        public static readonly DependencyProperty MaskProperty =
            DependencyProperty.RegisterAttached(
                "Mask",
                typeof(MaskType),
                typeof(TextBoxMaskBehavior),
                new FrameworkPropertyMetadata(MaskChangedCallback)
                );

        private static void MaskChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
       {
            if (e.OldValue is TextBox)
            {
                (e.OldValue as TextBox).PreviewTextInput -= TextBox_PreviewTextInput;
                DataObject.RemovePastingHandler((e.OldValue as TextBox), (DataObjectPastingEventHandler)TextBoxPastingEventHandler);
            }

            TextBox _this = (d as TextBox);
            if (_this == null)
                return;

            if ((MaskType)e.NewValue != MaskType.Any)
            {
                _this.PreviewTextInput += TextBox_PreviewTextInput;
                DataObject.AddPastingHandler(_this, (DataObjectPastingEventHandler)TextBoxPastingEventHandler);
            }

            ValidateTextBox(_this);
        }

        #endregion

        #region Private Static Methods

        private static void ValidateTextBox(TextBox _this)
        {
            if (GetMask(_this) != MaskType.Any)
            {
                _this.Text = ValidateValue(GetMask(_this), _this.Text, GetMinimumValue(_this), GetMaximumValue(_this));
            }
        }

        private static void TextBoxPastingEventHandler(object sender, DataObjectPastingEventArgs e)
        {
            //если вставляем
            TextBox _this = (sender as TextBox);
            string clipboard = e.DataObject.GetData(typeof(string)) as string;
            int caret = _this.CaretIndex;
            MaskType Mask = GetMask(_this);
            if (_this.SelectionLength > 0)
            {
                //не тронутый текст
                string substr = _this.Text.Substring(0, _this.Text.Length - _this.SelectionLength);
                if (substr.Length > 0)
                {
                    e.Handled = Adding(_this, Mask, clipboard, _this.Text) == true ? false : true;
                    return;
                }
                else
                {
                    e.Handled = AddToEmpty(_this, Mask, clipboard, _this.Text) == true ? false : true;
                    return;
                }
            }

            if (caret > 0)
            {
                //если текст добавляется к существующему
                e.Handled = Adding(_this, Mask, clipboard, _this.Text) == true ? false : true;
            }
            else
            {
                //если заменяем либо пишем в пустой строке
                e.Handled = AddToEmpty(_this, Mask, clipboard, _this.Text) == true ? false : true;
            }
            e.CancelCommand();
            e.Handled = true;
        }

        
        private static void TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            TextBox _this = sender as TextBox;
            int caret = _this.CaretIndex;
            MaskType Mask = GetMask(_this);

            if(_this.SelectionLength>0)
            {
                //не тронутый текст
                string substr = _this.Text.Substring(0, _this.Text.Length - _this.SelectionLength);
                //текст под замену
                string repl = _this.Text.Substring(caret, _this.SelectionLength);
                //не тронутый текст
                int n = _this.Text.Length - _this.SelectionLength;
                string stat = _this.Text.Substring(0, caret);
                string part1 = stat + e.Text;
                string res = part1 + _this.Text.Substring(n);
                if (substr.Length>0)
                {
                    e.Handled = Adding(_this, Mask, e.Text, substr) == true ? false : true;
                    return;
                }
                else
                {
                    e.Handled = AddToEmpty(_this, Mask, e.Text, substr) == true ? false : true;
                    return;
                }
            }

            if (caret > 0)
            {
                //если текст добавляется к существующему
                e.Handled = Adding(_this, Mask, e.Text, _this.Text) == true ? false : true;
            }
            else
            {
                //если заменяем либо пишем в пустой строке
                e.Handled = AddToEmpty(_this, Mask, e.Text, _this.Text) == true ? false : true;
            }
        }

        private static bool AddToEmpty(TextBox tb, MaskType Mask, string EText, string text)
        {
            TextBox _this = tb;
            switch (Mask)
            {
                case (MaskType.Var):
                    {
                        Regex valid = new Regex(@"[A-z]");
                        Match v = valid.Match(EText);
                        if (!v.Success)
                        {
                            return false;
                        }
                        return true;
                    }
                case (MaskType.PositiveInteger):
                    {
                        Regex valid = new Regex(@"\d");
                        Match v = valid.Match(EText);
                        string str = text + EText;

                        if (!v.Success)
                        {
                            return false;
                        }
                        //проверили границы
                        if (Convert.ToInt32(str) > GetMaximumValue(_this))
                        {
                            return false;
                        }
                        if (Convert.ToInt32(str) < GetMinimumValue(_this))
                        {
                            return false;
                        }
                        //в начале не может быть нуля
                        if (Int32.Parse(EText) == 0)
                        {
                            return false;
                        }
                        return true;
                    }
                case (MaskType.Decimal):
                    {
                        if (_this.Text.Length == 0)
                        {
                            Regex valid = new Regex(@"[\d|\-]");
                            Match m = valid.Match(EText);
                            if (!m.Success)
                            {
                                return false;
                            }
                            return true;
                        }
                        else
                        {
                            Regex valid = new Regex(@"(^\-?[0-9]+[,.]?[0-9]*$)|
                                                    (^[0-9]+[,.]?[0-9]*$)|
                                                     (^[0-9]+[,.]?$)|(^\d+$)|(^\d+[,.]?\d*$)");
                            string str = text + EText;
                            Match m = valid.Match(str);
                            if (!m.Success)
                            {
                                return false;
                            }
                            return true;
                        }
                    }
            }
            return false;
        }

        private static bool Adding(TextBox tb, MaskType Mask, string EText, string text)
        {
            TextBox _this = tb;
            switch (Mask)
            {
                case (MaskType.Var):
                    {
                        Regex valid = new Regex(@"\w");
                        Match v = valid.Match(EText);
                        if (!v.Success)
                        {
                            return false;
                        }
                        Regex regex = new Regex(@"\A[A-z]+[0-9]*\Z");
                        string str = text + EText;
                        Match m = regex.Match(str);
                        if (!m.Success)
                        {
                            return false;
                        }
                        return true;
                    }
                case (MaskType.PositiveInteger):
                    {
                        Regex valid = new Regex(@"\d");
                        Match v = valid.Match(EText);
                        if (!v.Success)
                        {
                            return false;
                        }
                        string str = text + EText;
                        if (Convert.ToInt32(str) > GetMaximumValue(_this))
                        {
                            return false;
                        }
                        if (Convert.ToInt32(str) < GetMinimumValue(_this))
                        {
                            return false;
                        }
                        return true;
                    }
                case (MaskType.Decimal):
                    {
                        Regex valid = new Regex(@"(^\-?[0-9]+[,.]?[0-9]*$)|
                                                    (^[0-9]+[,.]?[0-9]*$)|
                                                     (^[0-9]+[,.]?$)|(^\d+$)|(^\d+[,.]?\d*$)");
                        string str = text + EText;
                        Match m = valid.Match(str);
                        if (!m.Success)
                        {
                            return false;
                        }
                        return true;
                       
                    }
            }
            return false;
        }


        /*
        private static void TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            TextBox _this = (sender as TextBox);
            bool isValid = IsSymbolValid(GetMask(_this), e.Text, _this.Text);
            e.Handled = !isValid;
            if (isValid)
            {
                int caret = _this.CaretIndex;
                string text = _this.Text;
                bool textInserted = false;
                int selectionLength = 0;

                if (_this.SelectionLength > 0)
                {
                    text = text.Substring(0, _this.SelectionStart) +
                            text.Substring(_this.SelectionStart + _this.SelectionLength);
                    caret = _this.SelectionStart;
                }

                if (e.Text == NumberFormatInfo.CurrentInfo.NumberDecimalSeparator)
                {
                    while (true)
                    {
                        int ind = text.IndexOf(NumberFormatInfo.CurrentInfo.NumberDecimalSeparator);
                        if (ind == -1)
                            break;

                        text = text.Substring(0, ind) + text.Substring(ind + 1);
                        if (caret > ind)
                            caret--;
                    }

                    if (caret == 0)
                    {
                        text = "0" + text;
                        caret++;
                    }
                    else
                    {
                        if (caret == 1 && string.Empty + text[0] == NumberFormatInfo.CurrentInfo.NegativeSign)
                        {
                            text =  NumberFormatInfo.CurrentInfo.NegativeSign + "0" + text.Substring(1);
                            caret++;
                        }
                    }

                    if (caret == text.Length)
                    {
                        selectionLength = 1;
                        textInserted = true;
                        text = text + NumberFormatInfo.CurrentInfo.NumberDecimalSeparator + "0";
                        caret++;
                    }
                }
                else if (e.Text == NumberFormatInfo.CurrentInfo.NegativeSign)
                {
                    textInserted = true;
                    if (_this.Text.Contains(NumberFormatInfo.CurrentInfo.NegativeSign))
                    {
                        text = text.Replace(NumberFormatInfo.CurrentInfo.NegativeSign, string.Empty);
                        if (caret != 0)
                            caret--;
                    }
                    else
                    {
                        text = NumberFormatInfo.CurrentInfo.NegativeSign + _this.Text;
                        caret++;
                    }
                }

                if (!textInserted)
                {
                    text = text.Substring(0, caret) + e.Text +
                        ((caret < _this.Text.Length) ? text.Substring(caret) : string.Empty);

                    caret++;
                }

                try
                {
                    //здесь надо подправить
                    double val = Convert.ToDouble(text);
                    double newVal = ValidateLimits(GetMinimumValue(_this), GetMaximumValue(_this), val);
                    if (val != newVal)
                    {
                        text = newVal.ToString();
                    }
                    else if (val == 0)
                    {
                        if (!text.Contains(NumberFormatInfo.CurrentInfo.NumberDecimalSeparator))
                            text = "0";
                    }
                }
                catch
                {
                    if(text==NumberFormatInfo.CurrentInfo.NegativeSign)
                    {
                        text = "-";
                    }
                    else if (GetMask(_this) == MaskType.Var) { }
                    else { text = "0"; }                    
               }

                while (text.Length > 1 && text[0] == '0' && string.Empty + text[1] != NumberFormatInfo.CurrentInfo.NumberDecimalSeparator)
                {
                    text = text.Substring(1);
                    if (caret > 0)
                        caret--;
                }

                while (text.Length > 2 && string.Empty + text[0] == NumberFormatInfo.CurrentInfo.NegativeSign && text[1] == '0' && string.Empty + text[2] != NumberFormatInfo.CurrentInfo.NumberDecimalSeparator)
                {
                    text = NumberFormatInfo.CurrentInfo.NegativeSign + text.Substring(2);
                    if (caret > 1)
                        caret--;
                }

                if (caret > text.Length)
                    caret = text.Length;

                _this.Text = text;
                _this.CaretIndex = caret;
                _this.SelectionStart = caret;
                _this.SelectionLength = selectionLength;
                e.Handled = true;
            }
        }*/

        private static string ValidateValue(MaskType mask, string value, double min, double max)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            value = value.Trim();
            switch (mask)
            {
                case MaskType.Integer:
                    try
                    {
                        Convert.ToInt64(value);
                        return value;
                    }
                    catch
                    {
                    }
                    return string.Empty;

                case MaskType.Decimal:
                    try
                    {
                        Convert.ToDouble(value);

                        return value;
                    }
                    catch
                    {
                    }
                    return string.Empty;
            }

            return value;
        }

        private static double ValidateLimits(double min, double max, double value)
        {
            if (!min.Equals(double.NaN))
            {
                if (value < min)
                    return min;
            }

            if (!max.Equals(double.NaN))
            {
                if (value > max)
                    return max;
            }

            return value;
        }

        private static bool IsSymbolValid(MaskType mask, string str, string text)
        {
            switch (mask)
            {
                case MaskType.Any:
                    return true;

                case MaskType.Integer:
                    if (str == NumberFormatInfo.CurrentInfo.NegativeSign)
                        return true;
                    break;

                case MaskType.Decimal:
                    if (str == NumberFormatInfo.CurrentInfo.NumberDecimalSeparator ||
                        str == NumberFormatInfo.CurrentInfo.NegativeSign)
                        return true;
                    break;     
            }

            if (mask.Equals(MaskType.Integer) || mask.Equals(MaskType.Decimal))
            {
                foreach (char ch in str)
                {
                    if (!Char.IsDigit(ch))
                        return false;
                }

                return true;
            }

            return false;
        }

        private static bool Valid(string str)
        {
            char[] chars = str.ToCharArray();
            foreach(char c in chars)
            {
                if (!(Char.IsLetterOrDigit(c))){ return false; }
            }
            return true;
        }

        #endregion
    }

    public enum MaskType
    {
        Any,
        Integer,
        PositiveInteger,
        Decimal,
        Var
    }  
}