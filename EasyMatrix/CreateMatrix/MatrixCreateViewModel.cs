using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows;
using System.Windows.Data;
using System.Globalization;
using System.Windows.Controls;

namespace EasyMatrix
{
    class MatrixCreateViewModel : ViewModelBase
    {
        public MatrixCreateViewModel()
        {
            if (MatrixManager.StandartName.Count > 0) { CurrentChar = MatrixManager.StandartName.Peek().ToString(); }          
            this.OK = new RelayCommand<Window>(this.OkCommand);
            this.Cancel = new RelayCommand<Window>(this.CancelCommand);
            Rows = 1;
            Col = 1;
            _matrix = InitialGrid();
        }

        private Grid _matrix;

        #region Public Properties

        public Grid Matrix
        {
            get { return _matrix; }
        }

        public int Rows
        {
            get { return _row; }
            set
            {
                _row = value;
                _matrix = RebuildMatrix(Rows, Col);
                RaisePropertyChanged(() => Matrix);
            }
        }

        public int Col
        {
            get { return _col; }
            set
            {
                _col = value;
                _matrix = RebuildMatrix(Rows, Col);
                RaisePropertyChanged(() => Matrix);
            }
        }

        public string CurrentChar { get; set; }

        #endregion

        #region Commands

        public RelayCommand<Window> OK { get; private set; }

        public RelayCommand<Window> Cancel { get; private set; }

        private void OkCommand(Window wind)
        {
            if (NameCheck())
            {
                ReadMatrix();
                if ((MatrixManager.StandartName.Count > 0)&(CurrentChar!="")) { MatrixManager.StandartName.Dequeue(); }
                if (wind != null) { wind.Close(); }
            }
        }

        private void CancelCommand(Window wind)
        {
            if (wind != null) { wind.Close(); }
        }

        #endregion

        #region Fields

        private int _row;

        private int _col;

        #endregion

        #region Methods

        private string Validation(string val)
        {
            char[] ch = val.ToCharArray();
            string validVal = null;
            foreach(char c in ch)
            {
                if (Char.IsDigit(c)) { validVal += c; }
            }
            return validVal;
        }

        private Grid InitialGrid()
        {
            Grid g = new Grid();
            ColumnDefinition col1 = new ColumnDefinition();
            RowDefinition row1 = new RowDefinition();
            g.ColumnDefinitions.Add(col1);
            g.RowDefinitions.Add(row1);
            TextBox tBlock = new TextBox();
            MyControls.MyTextBox.SetMask(tBlock, MyControls.MyTextBox.MaskType.Decimal);
            tBlock.Text = "0";
            tBlock.FontSize = 20;
            tBlock.FontStyle = FontStyles.Normal;
            tBlock.HorizontalAlignment = HorizontalAlignment.Center;
            tBlock.VerticalAlignment = VerticalAlignment.Center;
            g.Children.Add(tBlock);
            Grid.SetColumn(tBlock, 0);
            Grid.SetRow(tBlock, 0);
            return g;
        }

        private bool NameCheck()
        {
            if (CurrentChar == "") { MessageBox.Show("Введите имя матрицы"); return false; }
            foreach (Matrix.Matrix x in MatrixManager._matrixCollection)
            {
                if (x.Name == CurrentChar) { MessageBox.Show("Матрица с таким именем уже существует"); return false; }
            }
            return true;
        }

        private void ReadMatrix()
        {
            Queue<string> values = new Queue<string>();
            double[,] matrix = new double[Rows, Col];
            foreach (TextBox tb in _matrix.Children)
            {
                if (tb.Text == "") { values.Enqueue("0"); }
                else { values.Enqueue(tb.Text); }
            }
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Col; j++)
                {
                    try
                    {
                        matrix[i, j] = Convert.ToDouble(values.Peek());
                    }
                    catch
                    {
                        matrix[i, j] = double.Parse(values.Peek(), CultureInfo.InvariantCulture);
                    }
                    finally
                    {
                        values.Dequeue();
                    }
                }
            }
            Matrix.Matrix X = new Matrix.Matrix(matrix);
            X.Name = CurrentChar;
            MatrixManager.AddMatrix(X);
        }

        private Grid RebuildMatrix(int Row, int Col)
        {
            if (Col == 0 | Row == 0) { return new Grid(); }
            Grid grid = new Grid();
            grid.ClipToBounds = false;
            grid.HorizontalAlignment = HorizontalAlignment.Center;
            grid.VerticalAlignment = VerticalAlignment.Center;
            List<TextBox> listBox = new List<TextBox>();
            for (int i = 0; i < Row; i++)
            {
                for (int j = 0; j < Col; j++)
                {
                    TextBox tb = new TextBox();
                    tb.FontSize = 20;
                    tb.MinWidth = 30;
                    tb.MinHeight = 30;
                    tb.TextWrapping = TextWrapping.NoWrap;
                    tb.TextAlignment = TextAlignment.Center;
                    MyControls.MyTextBox.SetMask(tb, MyControls.MyTextBox.MaskType.Decimal);
                    listBox.Add(tb);
                }
            }

            for (int i = 0; i < Row; i++)
            {
                RowDefinition row = new RowDefinition();
                grid.RowDefinitions.Add(row);
            }
            for (int j = 0; j < Col; j++)
            {
                ColumnDefinition col = new ColumnDefinition();
                grid.ColumnDefinitions.Add(col);
            }

            foreach (TextBox t in listBox)
            {
                grid.Children.Add(t);
            }

            int z = 0;
            for (int i = 0; i < Row; i++)
            {
                {
                    for (int j = 0; j < Col; j++)
                    {
                        Grid.SetRow(grid.Children[z], i);
                        Grid.SetColumn(grid.Children[z], j);
                        z++;
                    }
                }
            }
            return grid;
        }

        #endregion
    }

    [ValueConversion(typeof(double), typeof(double))]
    public class ButtonOK : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double width = (double)value / 2;
            return width;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
