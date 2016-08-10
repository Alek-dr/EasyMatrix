using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matrix
{
    public class Matrix
    {
        public Matrix(int row, int col)
        {
            _matr = new double[row, col];
            this._col = col;
            this._row = row;
            Round = 3;
        }

        public Matrix(double[,] matr)
        {
            _matr = matr;
            _row = matr.GetLength(0);
            _col = matr.GetLength(1);
            Round = 3;
        }

        private double[,] _matr;

        private int _col;

        private int _row;

        public StringBuilder Message;

        public delegate void Writing(Matrix X);

        public delegate void InvCofWrite(string name, double n);

        #region Properties

        public int Columns
        {
            get { return _matr.GetLength(1); }
        }

        public int Rows
        {
            get { return _matr.GetLength(0); }
        }

        public bool IsZero
        {
            get { return Zero(); }
        }

        public double[,] Matr
        {
            get { return _matr; }
        }

        public override string ToString()
        {
            return this.Name;
        }

        public string Name { get; set; }

        public int Round { get; set; }

        #endregion

        #region Operations

        public Matrix Power(int n)
        {
            if(n<0)
            {
                //в отрицательную степень тоже надо возводить
                Matrix X1 = new Matrix(1, 1);
                X1.Round = this.Round; 
                X1 = this.InverseByGauss_Gordan();
                Matrix X2 = new Matrix(X1._row, X1._col);
                for (int i = 0; i < X1._row; i++)
                {
                    for (int j = 0; j < X1._col; j++)
                    {
                        X2._matr[i, j] = X1._matr[i, j];
                    }
                }
                X2.Round = this.Round;
                while (n<-1)
                {
                    X2 = X1 * X2;
                    n++;
                }
                return X2;
            }
            else
            {
                Matrix X = new Matrix(this._row, this._col);
                for (int i = 0; i < X._row; i++)
                {
                    for (int j = 0; j < X._col; j++)
                    {
                        X._matr[i, j] = this._matr[i, j];
                    }
                }
                X.Round = this.Round;
                while (n > 1)
                {
                    X = MultiplyByMatrix(this, X);
                    if (X == null) { return null; }
                    n--;
                }
                return X;
            }
           
        }

        public Matrix Power(int n, Writing W)
        {
            int wn = 1;
            if (n<0)
            {
                Matrix X1 = new Matrix(1, 1);
                X1.Round = this.Round;
                X1.Name = this.Name;
                X1 = this.InverseByGauss_Gordan();
               
                X1.Message = new StringBuilder("Обратная матрица:");
                W(X1);
                Matrix X2 = new Matrix(X1._row, X1._col);
                for (int i = 0; i < X1._row; i++)
                {
                    for (int j = 0; j < X1._col; j++)
                    {
                        X2._matr[i, j] = X1._matr[i, j];
                    }
                }
                X2.Round = this.Round;
                while (n < -1)
                {
                    wn++;
                    X2 = X1 * X2;
                    X2.Name = this.Name;
                    X2.Message = new StringBuilder("Матрица " + this.Name + " в степени " + -wn);
                    W(X2);
                    n++;
                }
                return X2;
            }
            else
            {
                Matrix X = new Matrix(this._row, this._col);
                for (int i = 0; i < X._row; i++)
                {
                    for (int j = 0; j < X._col; j++)
                    {
                        X._matr[i, j] = this._matr[i, j];
                    }
                }
                X.Round = this.Round;
                while (n > 1)
                {
                    wn++;
                    X = this * X;
                    X.Name = this.Name;
                    X.Message = new StringBuilder("Матрица " + this.Name + " в степени " + wn);
                    W(X);
                    if (X == null) { return null; }
                    n--;
                }
                return X;
            }          
        }

        private void Rounding()
        {
            for(int i=0; i<this._row; i++)
            {
                for(int j=0; j<this._col;j++)
                {
                    this._matr[i, j] = Math.Round(_matr[i, j], this.Round, MidpointRounding.ToEven);
                }
            }
        }

        private static Matrix MultiplyByMatrix(Matrix A, Matrix B)
        {
            bool canMultiply = A._col == B._row ? true : false;
            if (!canMultiply) { throw new MultException("Invalid multiplication"); }
            else
            {
                Matrix X = new Matrix(A._row, B._col);
                if (A.Round > B.Round) { X.Round = A.Round; } else { X.Round = B.Round; }
                for (int i = 0; i < A._row; i++)
                {
                    for (int j = 0; j < B._col; j++)
                    {
                        for (int k = 0; k < B._row; k++)
                        {
                            X._matr[i, j] = X._matr[i, j] + A._matr[i, k] * B._matr[k, j];
                        }
                    }
                }
                X.Rounding();
                return X;
            }
        }

        private static Matrix MultiblyByNumber(Matrix M, double n)
        {
            Matrix X = new Matrix(M._row, M._col);
            X.Round = M.Round;
            for (int i = 0; i < M._row; i++)
            {
                for (int j = 0; j < M._col; j++)
                {
                    X._matr[i, j] = M._matr[i, j] * n;
                }
            }
            X.Rounding();
            return X;
        }

        private static Matrix AddMatrix(Matrix A, Matrix B)
        {
            bool canAdd = (A._col == B._col) && (A._row == B._row) ? true : false;
            if (!canAdd) { throw new SumExceptiom("Invalid summation"); }
            else
            {
                Matrix X = new Matrix(A._row, A._col);
                if (A.Round > B.Round) { X.Round = A.Round; } else { X.Round = B.Round; }
                for (int i = 0; i < A._row; i++)
                {
                    for (int j = 0; j < A._col; j++)
                    {
                        X._matr[i, j] = A._matr[i, j] + B._matr[i, j];
                    }
                }
                X.Rounding();
                return X;
            }
        }

        private static Matrix Subtract(Matrix A, Matrix B)
        {
            bool canAdd = (A._col == B._col) && (A._row == B._row) ? true : false;
            if (!canAdd) { throw new SumExceptiom("Invalid subtraction"); }
            else
            {
                Matrix X = new Matrix(A._row, A._col);
                if (A.Round > B.Round) { X.Round = A.Round; } else { X.Round = B.Round; }
                for (int i = 0; i < A._row; i++)
                {
                    for (int j = 0; j < A._col; j++)
                    {
                        X._matr[i, j] = A._matr[i, j] - B._matr[i, j];
                    }
                }
                X.Rounding();
                return X;
            }
        }

        public Matrix Transpose()
        {
            Matrix X = new Matrix(this._col, this._row);
            X.Round = this.Round;
            for (int i = 0; i < this._row; i++)
            {
                for (int j = 0; j < this._col; j++)
                {
                    X._matr[j, i] = this._matr[i, j];
                }
            }
            X.Rounding();
            return X;
        }

        public Matrix Minor(int row, int col)
        {
            //Разложение по строке
            bool flagRow = false;
            bool flagCol = false;
            Matrix X = new Matrix(_row - 1, _col - 1);
            for (int i = 0; i < _row; i++)
            {
                int z = i;
                //Разложение идет по строке, поэтому каждый при просмотре каждой новой строки
                //флаг столбца надо сбрасывать
                flagCol = false;
                if (i == row)
                {
                    flagRow = true;
                    continue;
                }
                if (flagRow) { z = i - 1; }

                for (int j = 0; j < _col; j++)
                {
                    int s = j;
                    if (j == col)
                    {
                        flagCol = true;
                        continue;
                    }
                    if (flagCol) { s = j - 1; }
                    X._matr[z, s] = _matr[i, j];
                }
            }
            X.Rounding();
            return X;
        }

        #region Determinant

        public double Determinant()
        {
            if (this._col != this._row) { throw new IncompabilityOfColumnsAndRows("It is not square matrix"); }
            else { return Determinant(this); }
        }

        public double Determinant(bool write)
        {
            if (this._col != this._row) { throw new IncompabilityOfColumnsAndRows("It is not square matrix"); }
            else { return Determinant(this, true); }
        }

        private double Determinant(Matrix X)
        {
            double res = 0;
            if ((X._col == 2) && (X._row == 2))
            {
                return X._matr[0, 0] * X._matr[1, 1] - X._matr[0, 1] * X._matr[1, 0];
            }
            else if ((X._col == 1) && (X._row == 1)) { return X._matr[0, 0]; }
            else
            {
                for (int j = 0; j < X._col; j++)
                {
                    Matrix J = X.Minor(0, j);
                    double minor = Determinant(J);
                    res = res + Math.Pow(-1, j + 2) * X._matr[0, j] * minor;
                }
                return res;
            }
        }

        private double Determinant(Matrix X, bool write)
        {
            double res = 0;
            string dot = ((char)183).ToString();
            if (X._col == 3)
            {
                X.Message = new StringBuilder(_matr[0, 0] + dot + _matr[1, 1] + dot + _matr[2, 2] + " + "
                    + _matr[0, 1] + dot + _matr[1, 2] + dot + _matr[2, 0] + " + "
                    + _matr[1, 0] + dot + _matr[2, 1] + dot + _matr[0, 2] + " - "
                    + _matr[0, 2] + dot + _matr[1, 1] + dot + _matr[2, 0] + " - "
                    + _matr[1, 0] + dot + _matr[0, 1] + dot + _matr[2, 2] + " - "
                    + _matr[2, 1] + dot + _matr[1, 2] + dot + _matr[0, 0]);
            }
            if (X._col == 2)
            {
                X.Message = new StringBuilder(_matr[0, 0] + dot + _matr[1, 1] +
                    " - " + _matr[0, 1] + dot + _matr[1, 0]);
            }
            if ((X._col == 2) && (X._row == 2))
            {
                return X._matr[0, 0] * X._matr[1, 1] - X._matr[0, 1] * X._matr[1, 0];
            }
            else if ((X._col == 1) && (X._row == 1)) { return X._matr[0, 0]; }
            else
            {
                for (int j = 0; j < X._col; j++)
                {
                    Matrix J = X.Minor(0, j);
                    double minor = Determinant(J);
                    res = res + Math.Pow(-1, j + 2) * X._matr[0, j] * minor;
                }
                return res;
            }
        }

        #endregion

        #region Rows Operations 

        public void SwapRows(int r1, int r2)
        {
            if ((this._row < r1) | (this._row < r2) | (r1 < 0) | (r2 < 0)) { throw new IncompabilityOfColumnsAndRows("Invalid index"); }
            if (r1 == r2) { return; }
            else
            {
                double[,] matr = new double[this._row, this._col];
                matr = this._matr.Clone() as Double[,];
                for (int j = 0; j < _col; j++)
                {
                    double x1 = matr[r1, j];
                    double x2 = matr[r2, j];
                    matr[r1, j] = x2;
                    matr[r2, j] = x1;
                }
                this._matr = matr;
            }
        }

        public void AddRowMultiplyedByNumber(int row1, double n, int row2)
        {
            for (int j = 0; j < _col; j++)
            {
                _matr[row2, j] = _matr[row2, j] + _matr[row1, j] * n;
            }
        }

        public void MultiplyRowByNumber(int r, double n)
        {
            if ((this._row < r) | (r < 0)) { throw new IncompabilityOfColumnsAndRows("Invalid index"); }
            else
            {
                for (int j = 0; j < _col; j++)
                {
                    this._matr[r, j] = this._matr[r, j] * n;
                }
            }
        }

        public void DivRowByNumber(int r, double n)
        {
            if ((this._row < r) | (r < 0)) { throw new IncompabilityOfColumnsAndRows("Invalid index"); }
            else
            {
                for (int j = 0; j < _col; j++)
                {
                    this._matr[r, j] = (this._matr[r, j]) * (1 / n);
                }
            }
        }

        public void AddNumberToRow(int r, double n)
        {
            if ((this._row < r) | (r < 0)) { throw new IncompabilityOfColumnsAndRows("Invalid index"); }
            else
            {
                for (int j = 0; j < _col; j++)
                {
                    this._matr[r, j] = this._matr[r, j] + n;
                }
            }
        }

        public void SubNumberFromRow(int r, double n)
        {
            if ((this._row < r) | (r < 0)) { throw new IncompabilityOfColumnsAndRows("Invalid index"); }
            else
            {
                for (int j = 0; j < _col; j++)
                {
                    this._matr[r, j] = this._matr[r, j] - n;
                }
            }
        }

        public void DeleteRow(int r)
        {
            if ((r > _row) | (r < 0)) { throw new IncompabilityOfColumnsAndRows("Row number is negative or more then number of rows in matrix"); }
            Matrix X = new Matrix(_row - 1, _col);
            bool flag = false;
            for (int i = 0; i < _row; i++)
            {
                int z = i;
                if (i == r) { flag = true; continue; }
                if (flag) { z = z - 1; }
                for (int j = 0; j < _col; j++)
                {
                    X._matr[z, j] = this._matr[i, j];
                }
            }
            this._matr = X._matr;
            this._col = X._col;
            this._row = X._row;
        }

        public void ExcludeZeroRow()
        {
            if (this.IsZero) { this._matr = new double[1, 1] { { 0 } }; return; }
            List<int> RowToDelete = new List<int>();
            Matrix V = new Matrix(_col, 1);
            for (int i = 0; i < V._row; i++)
            {
                V._matr[i, 0] = 1;
            }
            Matrix zRow = this * V;
            for (int i = 0; i < zRow._row; i++)
            {
                if (zRow._matr[i, 0] == 0) { RowToDelete.Add(i); }
            }
            foreach (int i in RowToDelete)
            {
                this.DeleteRow(i);
            }
        }

        #endregion

        #region Column Operations

        public void SwapColumns(int col1, int col2)
        {
            if ((this._col < col1) | (this._col < col2) | (col1 < 0) | (col2 < 0) | (col1 == col2)) { throw new IncompabilityOfColumnsAndRows("Invalid index"); }
            else
            {
                for (int i = 0; i < _row; i++)
                {
                    double x1 = this._matr[i, col1];
                    double x2 = this._matr[i, col2];
                    this._matr[i, col1] = x2;
                    this._matr[i, col2] = x1;
                }
            }
        }

        public void MultiplyColByNumber(int col, double n)
        {
            if ((this._col < col) | (col < 0)) { throw new MultException("Invalid multiplication"); }
            else
            {
                for (int i = 0; i < _row; i++)
                {
                    this._matr[i, col] = this._matr[i, col] * n;
                }
            }
        }

        public void DivColByNumber(int col, double n)
        {
            if ((this._col < col) | (col < 0)) { throw new IncompabilityOfColumnsAndRows("Invalid index"); }
            else
            {
                for (int i = 0; i < _row; i++)
                {
                    this._matr[i, col] = this._matr[i, col] / n;
                }
            }
        }

        public void AddNumberToCol(int col, double n)
        {
            if ((this._col < col) | (col < 0)) { throw new IncompabilityOfColumnsAndRows("Invalid index"); }
            else
            {
                for (int i = 0; i < _row; i++)
                {
                    this._matr[i, col] = this._matr[i, col] + n;
                }
            }
        }

        public void SubNumberFromCol(int col, double n)
        {
            if ((this._col < col) | (col < 0)) { throw new IncompabilityOfColumnsAndRows("Invalid index"); }
            else
            {
                for (int i = 0; i < _row; i++)
                {
                    this._matr[i, col] = this._matr[i, col] - n;
                }
            }
        }

        public void DeleteCol(int col)
        {
            if ((col > _col) | (col < 0)) { throw new IncompabilityOfColumnsAndRows("Col number is negative or more then number of columns in matrix"); }
            Matrix X = new Matrix(_row, _col - 1);
            bool flag = false;
            for (int j = 0; j < _col; j++)
            {
                int z = j;
                if (j == col) { flag = true; continue; }
                if (flag) { z = z - 1; }
                for (int i = 0; i < _row; i++)
                {
                    X._matr[i, z] = this._matr[i, j];
                }
            }
            this._matr = X._matr;
            this._col = X._col;
            this._row = X._row;
        }

        public void AddColMultiplyedByNumber(int col1, double n, int col2)
        {
            for (int i = 0; i < _row; i++)
            {
                _matr[i, col2] = _matr[i, col2] + _matr[i, col1] * n;
            }
        }

        public void ExcludeZeroCol()
        {
            if (this.IsZero) { this._matr = new double[1, 1] { { 0 } }; return; }
            List<int> ColToDelete = new List<int>();
            Matrix Trans = this.Transpose();
            Matrix V = new Matrix(Trans._col, 1);
            for (int i = 0; i < V._row; i++)
            {
                V._matr[i, 0] = 1;
            }
            Matrix zCol = Trans * V;
            for (int i = 0; i < zCol._row; i++)
            {
                if (zCol._matr[i, 0] == 0) { ColToDelete.Add(i); }
            }
            foreach (int j in ColToDelete)
            {
                this.DeleteCol(j);
            }
        }
        #endregion

        #region Row Echelon Form

        public Matrix GetStepMatrix(Writing Write)
        {
            if (this.IsZero) { Write(this); return new Matrix(1, 1); }
            if ((this._col == 1) | (this._row == 1))
            {
                Matrix Res = new Matrix(this._row, this._col);
                Res.Matr[0, 0] = 1;
                Write(this);
                return Res;
            }
            this.ExcludeZeroColRow();
            int row = 0;
            double divBy = 0;
            double leadElement = 0;
            int strI = 0;
            int strZ = 0;
            Matrix X = new Matrix(_row, _col);
            X.Name = this.Name;
            X.Round = this.Round;
            X._matr = this._matr.Clone() as double[,];
            Write(X);
            switch (_col >= _row)
            {
                case (false):
                    {
                        for (int z = 0; z < _col; z++)
                        {
                            strZ = z + 1;
                            leadElement = X.FindLeadElem(z, z, out row);
                            if (leadElement == 0) { continue; }
                            if (row != 0)
                            {
                                X.SwapRows(z, row); X.Rounding();
                                if (z != row)
                                {
                                    X.Message = new StringBuilder("Поменяли местами строки " + z + " и " + row);
                                    Write(X);
                                }
                            }
                            if(leadElement!=1)
                            {
                                X.DivRowByNumber(z, leadElement);
                                X.Message = new StringBuilder("Поделили строку " + strZ + " на " + leadElement);
                                X.Rounding();
                                Write(X);
                            }                           
                            for (int i = z + 1; i < _row; i++)
                            {
                                if (X._matr[i, z] == 0) { continue; }
                                strI = i + 1;
                                divBy = -X._matr[i, z];
                                X.AddRowMultiplyedByNumber(z, -X._matr[i, z], i);
                                X.Message = new StringBuilder("Добавили к строке " + strI + " строку " + strZ + " умноженную на " + divBy);
                                X.Rounding();
                                Write(X);
                            }
                        }
                        X.Rounding();
                        return X;
                    }
                case (true):
                    {
                        for (int z = 0; z < _row; z++)
                        {
                            strZ = z + 1;
                            leadElement = X.FindLeadElem(z, z, out row);
                            if (leadElement == 0) { continue; }
                            if (row != 0)
                            {
                                X.SwapRows(z, row); X.Rounding();
                                if(z!=row)
                                {
                                    X.Message = new StringBuilder("Поменяли местами строки "+z+" и " + row);
                                    Write(X);
                                }                               
                            }
                            if (leadElement != 1)
                            {
                                X.DivRowByNumber(z, leadElement);
                                X.Message = new StringBuilder("Поделили строку " + strZ + " на " + leadElement);
                                X.Rounding();
                                Write(X);
                            }
                            for (int i = z + 1; i < _row; i++)
                            {
                                if (X._matr[i, z] == 0) { continue; }
                                strI = i + 1;
                                divBy = -X._matr[i, z];
                                X.AddRowMultiplyedByNumber(z, -X._matr[i, z], i);                                
                                X.Message = new StringBuilder("Добавили к строке " + strI + " строку " + strZ + " умноженную на " + divBy);
                                X.Rounding();
                                Write(X);
                            }
                        }
                        X.Rounding();
                        return X;
                    }
            }
            X.Rounding();
            return X;
        }

        public Matrix GetStepMatrix()
        {
            if (this.IsZero) { return new Matrix(1, 1); }
            if ((this._col == 1) | (this._row == 1))
            {
                Matrix Res = new Matrix(this._row, this._col);
                Res.Matr[0, 0] = 1;
                return Res;
            }
            this.ExcludeZeroColRow();
            int row = 0;
            double divBy = 0;
            double leadElement = 0;
            int strI = 0;
            int strZ = 0;
            Matrix X = new Matrix(_row, _col);
            X.Name = this.Name;
            X.Round = this.Round;
            X._matr = this._matr.Clone() as double[,];
            switch (_col >= _row)
            {
                case (false):
                    {
                        for (int z = 0; z < _col; z++)
                        {
                            strZ = z + 1;
                            leadElement = X.FindLeadElem(z, z, out row);
                            if (leadElement == 0) { continue; }
                            if (row != 0)
                            {
                                X.SwapRows(z, row); X.Rounding();
                            }
                            if (leadElement != 1)
                            {
                                X.DivRowByNumber(z, leadElement);
                                X.Rounding();
                            }
                            for (int i = z + 1; i < _row; i++)
                            {
                                if (X._matr[i, z] == 0) { continue; }
                                strI = i + 1;
                                divBy = -X._matr[i, z];
                                X.AddRowMultiplyedByNumber(z, -X._matr[i, z], i);
                                X.Rounding();
                            }
                        }
                        X.Rounding();
                        return X;
                    }
                case (true):
                    {
                        for (int z = 0; z < _row; z++)
                        {
                            strZ = z + 1;
                            leadElement = X.FindLeadElem(z, z, out row);
                            if (leadElement == 0) { continue; }
                            if (row != 0)
                            {
                                X.SwapRows(z, row); X.Rounding();
                            }
                            if (leadElement != 1)
                            {
                                X.DivRowByNumber(z, leadElement);
                                X.Rounding();
                            }
                            for (int i = z + 1; i < _row; i++)
                            {
                                if (X._matr[i, z] == 0) { continue; }
                                strI = i + 1;
                                divBy = -X._matr[i, z];
                                X.AddRowMultiplyedByNumber(z, -X._matr[i, z], i);
                                X.Rounding();
                            }
                        }
                        X.Rounding();
                        return X;
                    }
            }
            X.Rounding();
            return X;
        }

        public void ThisToStepMatrix()
        {
            if (this.IsZero) { return; }
            if ((this._col == 1) | (this._row == 1))
            {
                _matr[0, 0] = 1;
                return;
            }
            this.ExcludeZeroColRow();
            int row = 0;
            double divBy = 0;
            double leadElement = 0;
            int strI = 0;
            int strZ = 0;
            switch (_col >= _row)
            {
                case (false):
                    {
                        for (int z = 0; z < _col; z++)
                        {
                            strZ = z + 1;
                            leadElement = FindLeadElem(z, z, out row);
                            if (leadElement == 0) { continue; }
                            if (row != 0)
                            {
                                SwapRows(z, row); Rounding();
                            }
                            if (leadElement != 1)
                            {
                                DivRowByNumber(z, leadElement);
                                Rounding();
                            }
                            for (int i = z + 1; i < _row; i++)
                            {
                                if (_matr[i, z] == 0) { continue; }
                                strI = i + 1;
                                divBy = -_matr[i, z];
                                AddRowMultiplyedByNumber(z, -_matr[i, z], i);
                                Rounding();
                            }
                        }
                        Rounding();
                        break;
                    }
                case (true):
                    {
                        for (int z = 0; z < _row; z++)
                        {
                            strZ = z + 1;
                            leadElement = FindLeadElem(z, z, out row);
                            if (leadElement == 0) { continue; }
                            if (row != 0)
                            {
                                SwapRows(z, row); Rounding();
                            }
                            if (leadElement != 1)
                            {
                                DivRowByNumber(z, leadElement);
                                Rounding();
                            }
                            for (int i = z + 1; i < _row; i++)
                            {
                                if (_matr[i, z] == 0) { continue; }
                                strI = i + 1;
                                divBy = -_matr[i, z];
                                AddRowMultiplyedByNumber(z, -_matr[i, z], i);
                                Rounding();
                            }
                        }
                        Rounding();
                        break;
                    }
            }
        }

        public void ThisToStepMatrix(Writing Write)
        {
            if (this.IsZero) { return; }
            if ((this._col == 1) | (this._row == 1))
            {
                _matr[0, 0] = 1;
                return;
            }
            this.ExcludeZeroColRow();
            int row = 0;
            double divBy = 0;
            double leadElement = 0;
            int strI = 0;
            int strZ = 0;
            Write(this);
            switch (_col >= _row)
            {
                case (false):
                    {
                        for (int z = 0; z < _col; z++)
                        {
                            strZ = z + 1;
                            leadElement = FindLeadElem(z, z, out row);
                            if (leadElement == 0) { continue; }
                            if (row != 0)
                            {
                                SwapRows(z, row);
                                Rounding();
                                if (z != row)
                                {
                                    Message = new StringBuilder("Поменяли местами строки " + z + " и " + row);
                                    Write(this);
                                }
                            }
                            if (leadElement != 1)
                            {
                                DivRowByNumber(z, leadElement);
                                Message = new StringBuilder("Поделили строку " + strZ + " на " + leadElement);
                                Rounding();
                                Write(this);
                            }
                            for (int i = z + 1; i < _row; i++)
                            {
                                if (_matr[i, z] == 0) { continue; }
                                strI = i + 1;
                                divBy = -_matr[i, z];
                                AddRowMultiplyedByNumber(z, -_matr[i, z], i);
                                Message = new StringBuilder("Добавили к строке " + strI + " строку " + strZ + " умноженную на " + divBy);
                                Rounding();
                                Write(this);
                            }
                        }
                        Rounding();
                        break;
                    }
                case (true):
                    {
                        for (int z = 0; z < _row; z++)
                        {
                            strZ = z + 1;
                            leadElement = FindLeadElem(z, z, out row);
                            if (leadElement == 0) { continue; }
                            if (row != 0)
                            {
                                SwapRows(z, row); Rounding();
                                if (z != row)
                                {
                                    Message = new StringBuilder("Поменяли местами строки " + z + " и " + row);
                                    Write(this);
                                }
                            }
                            if (leadElement != 1)
                            {
                                DivRowByNumber(z, leadElement);
                                Message = new StringBuilder("Поделили строку " + strZ + " на " + leadElement);
                                Rounding();
                                Write(this);
                            }
                            for (int i = z + 1; i < _row; i++)
                            {
                                if (_matr[i, z] == 0) { continue; }
                                strI = i + 1;
                                divBy = -_matr[i, z];
                                AddRowMultiplyedByNumber(z, -_matr[i, z], i);
                                Message = new StringBuilder("Добавили к строке " + strI + " строку " + strZ + " умноженную на " + divBy);
                                Rounding();
                                Write(this);
                            }
                        }
                        Rounding();
                        break;
                    }
            }
        }

        #endregion

        #region Gauss-Jordam

        public Matrix Gauss_Jordan()
        {
            if (this.IsZero) { return new Matrix(1, 1); }
            if ((this._col == 1) | (this._row == 1))
            {
                Matrix Res = new Matrix(this._row, this._col);
                Res.Matr[0, 0] = 1;
                return Res;
            }
            this.ExcludeZeroColRow();
            int row = 0;
            double divBy = 0;
            double leadElement = 0;
            int strI = 0;
            int strZ = 0;
            Matrix X = new Matrix(_row, _col);
            X.Name = this.Name;
            X.Round = this.Round;
            X._matr = this._matr.Clone() as double[,];
            switch (_col >= _row)
            {
                case (false):
                    {
                        for (int z = 0; z < _col; z++)
                        {
                            strZ = z + 1;
                            leadElement = X.FindLeadElem(z, z, out row);
                            if (leadElement == 0) { continue; }
                            if (row != 0)
                            {
                                X.SwapRows(z, row); X.Rounding();
                            }
                            if (leadElement != 1)
                            {
                                X.DivRowByNumber(z, leadElement);
                                X.Rounding();
                            }
                            for (int i = 0; i < _row; i++)
                            {
                                if (i == z) { continue; }
                                if (X._matr[i, z] == 0) { continue; }
                                strI = i + 1;
                                divBy = -X._matr[i, z];
                                X.AddRowMultiplyedByNumber(z, -X._matr[i, z], i);
                                X.Rounding();
                            }
                        }
                        X.Rounding();
                        return X;
                    }
                case (true):
                    {
                        for (int z = 0; z < _row; z++)
                        {
                            strZ = z + 1;
                            leadElement = X.FindLeadElem(z, z, out row);
                            if (leadElement == 0) { continue; }
                            if (row != 0)
                            {
                                X.SwapRows(z, row); X.Rounding();
                            }
                            if (leadElement != 1)
                            {
                                X.DivRowByNumber(z, leadElement);
                                X.Rounding();
                            }
                            for (int i = 0; i < _row; i++)
                            {
                                if (i == z) { continue; }
                                if (X._matr[i, z] == 0) { continue; }
                                strI = i + 1;
                                divBy = -X._matr[i, z];
                                X.AddRowMultiplyedByNumber(z, -X._matr[i, z], i);
                                X.Rounding();
                            }
                        }
                        X.Rounding();
                        return X;
                    }
            }
            X.Rounding();
            return X;
        }

        public Matrix Gauss_Jordan(Writing Write)
        {
            if (this.IsZero) { Write(this); return new Matrix(1, 1); }
            if ((this._col == 1) | (this._row == 1))
            {
                Matrix Res = new Matrix(this._row, this._col);
                Res.Matr[0, 0] = 1;
                Write(this);
                return Res;
            }
            this.ExcludeZeroColRow();
            int row = 0;
            double divBy = 0;
            double leadElement = 0;
            int strI = 0;
            int strZ = 0;
            Matrix X = new Matrix(_row, _col);
            X.Name = this.Name;
            X.Round = this.Round;
            X._matr = this._matr.Clone() as double[,];
            Write(X);
            switch (_col >= _row)
            {
                case (false):
                    {
                        for (int z = 0; z < _col; z++)
                        {
                            strZ = z + 1;
                            leadElement = X.FindLeadElem(z, z, out row);
                            if (leadElement == 0) { continue; }
                            if (row != 0)
                            {
                                X.SwapRows(z, row); X.Rounding();
                                if (z != row)
                                {
                                    X.Message = new StringBuilder("Поменяли местами строки " + z + " и " + row);
                                    Write(X);
                                }
                            }
                            if (leadElement != 1)
                            {
                                X.DivRowByNumber(z, leadElement);
                                X.Message = new StringBuilder("Поделили строку " + strZ + " на " + leadElement);
                                X.Rounding();
                                Write(X);
                            }
                            for (int i = 0; i < _row; i++)
                            {
                                if (i == z) { continue; }
                                if (X._matr[i, z] == 0) { continue; }
                                strI = i + 1;
                                divBy = -X._matr[i, z];
                                X.AddRowMultiplyedByNumber(z, -X._matr[i, z], i);
                                X.Message = new StringBuilder("Добавили к строке " + strI + " строку " + strZ + " умноженную на " + divBy);
                                X.Rounding();
                                Write(X);
                            }
                        }
                        X.Rounding();
                        return X;
                    }
                case (true):
                    {
                        for (int z = 0; z < _row; z++)
                        {
                            strZ = z + 1;
                            leadElement = X.FindLeadElem(z, z, out row);
                            if (leadElement == 0) { continue; }
                            if (row != 0)
                            {
                                X.SwapRows(z, row); X.Rounding();
                                if (z != row)
                                {
                                    X.Message = new StringBuilder("Поменяли местами строки " + z + " и " + row);
                                    Write(X);
                                }
                            }
                            if (leadElement != 1)
                            {
                                X.DivRowByNumber(z, leadElement);
                                X.Message = new StringBuilder("Поделили строку " + strZ + " на " + leadElement);
                                X.Rounding();
                                Write(X);
                            }
                            for (int i = 0; i < _row; i++)
                            {
                                if (i == z) { continue; }
                                if (X._matr[i, z] == 0) { continue; }
                                strI = i + 1;
                                divBy = -X._matr[i, z];
                                X.AddRowMultiplyedByNumber(z, -X._matr[i, z], i);
                                X.Message = new StringBuilder("Добавили к строке " + strI + " строку " + strZ + " умноженную на " + divBy);
                                X.Rounding();
                                Write(X);
                            }
                        }
                        X.Rounding();
                        return X;
                    }
            }
            X.Rounding();
            return X;
        }

        #endregion

        #region Inverse Matrix

        public Matrix InverseByGauss_Gordan(Writing Write)
        {
            double det = 0;
            if (this._col != this._row)
            {
                throw new IncompabilityOfColumnsAndRows("Матрица не является квадратной");
            }
            det = this.Determinant();
            if (det == 0)
            {
                throw new IncompabilityOfColumnsAndRows("Матрица вырожденная - определитель равен 0");
            }
            else
            {
                int c = this._col;
                Matrix X = SpecialMAtrix(this);
                X.Round = this.Round;
                X.Name = this.Name + "_E";
                X = X.Gauss_Jordan(Write);
                OnlyInverse(ref X, c);
                return X;
            }
        }

        public Matrix InverseByGauss_Gordan()
        {
            double det = 0;
            if (this._col != this._row)
            {
                throw new IncompabilityOfColumnsAndRows("Матрица не является квадратной");
            }
            det = this.Determinant();
            if (det == 0)
            {
                throw new IncompabilityOfColumnsAndRows("Матрица вырожденная - определитель равен 0");
            }
            else
            {
                int c = this._col;
                Matrix X = SpecialMAtrix(this);
                X.Name = this.Name + "_E";
                X.Round = this.Round;
                X = X.Gauss_Jordan();
                OnlyInverse(ref X, c);
                return X;
            }
        }

        private Matrix SpecialMAtrix(Matrix This)
        {
            Matrix X = new Matrix(this._row, this._col * 2);
            for(int i=0; i<This._row; i++)
            {
                for(int j=0; j<This._col; j++)
                {
                    X._matr[i, j] = This._matr[i, j];
                }
            }
            int n = X._col - This._col;
            for(int i = 0; i<This._row; i++)
            {
                X._matr[i, i+n] = 1;
            }
            return X;
        }

        private void OnlyInverse(ref Matrix G, int n)
        {
            double[,] inv = new double [G._row, n];
            for(int i=0; i<n; i++)
            {
                for(int j=0; j<n; j++)
                {
                    inv[i, j] = G._matr[i, n + j];
                }
            }
            G._matr = inv;
            G._col = n;
            G._row = n;
        }

        public Matrix InverseByCofactor()
        {
            double det = 0;
            double d = 0;
            if (this._col != this._row)
            {
                throw new IncompabilityOfColumnsAndRows("Матрица не является квадратной");
            }
            det = this.Determinant();
            if (det == 0)
            {
                throw new IncompabilityOfColumnsAndRows("Матрица вырожденная - определитель равен 0");
            }
            else
            {
                Queue<double> Cof = new Queue<double>();
                for(int j=0; j<this._col; j++)
                {
                    for(int i=0; i<this._row; i++)
                    {
                        Matrix A = this.Minor(i, j);
                        A.Round = this.Round;
                        d = A.Determinant();
                        d = Math.Pow(-1, i + j) * d;
                        Cof.Enqueue(d);
                    }
                }
                Matrix X = new Matrix(this._row, this._col);
                X.Round = this.Round;
                for(int i=0; i<this._row; i++)
                {
                    for (int j = 0; j < this._col; j++)
                    {
                        X._matr[i, j] = Cof.Dequeue();
                    }
                }
                   
                for (int i = 0; i < this._row; i++)
                {
                    X.DivRowByNumber(i, det);
                }
                X.Rounding();
                return X;
            }
        }

        public Matrix InverseByCofactor(Writing Write, InvCofWrite C)
        {
            double det = 0;
            double d = 0;
            int I = 0;
            int J = 0;
            if (this._col != this._row)
            {
                throw new IncompabilityOfColumnsAndRows("Матрица не является квадратной");
            }
            det = this.Determinant();
            if (det == 0)
            {
                throw new IncompabilityOfColumnsAndRows("Матрица вырожденная - определитель равен 0");
            }
            else
            {
                Queue<double> Cof = new Queue<double>();
                for (int j = 0; j < this._col; j++)
                {
                    J = j + 1;
                    for (int i = 0; i < this._row; i++)
                    {
                        I = i + 1;
                        Matrix A = this.Minor(i, j);
                        A.Name = "M" + I.ToString() + J.ToString();
                        A.Round = this.Round;
                        A.Rounding();
                        d = A.Determinant();                       
                        C(A.Name, d);
                        d = Math.Pow(-1, i + j) * d;
                        Write(A);
                        Cof.Enqueue(d);                      
                    }
                }
                Matrix X = new Matrix(this._row, this._col);
                X.Message = new StringBuilder("Найдем присоединенную матрицу:");
                for (int i = 0; i < this._row; i++)
                {
                    for (int j = 0; j < this._col; j++)
                    {
                        X._matr[i, j] = Cof.Dequeue();
                    }
                }
                X.Name = "A+";
                Write(X);

                for (int i = 0; i < this._row; i++)
                {
                    X.DivRowByNumber(i, det);
                }
                X.Message =  new StringBuilder("Разделим каждый элемент матрицы на ее определитель = " + det);                
                X.Round = this.Round;
                X.Rounding();
                Write(X);
                return X;
            }
        }

        #endregion

        public int Rang()
        {
            Matrix X = this.GetStepMatrix();
            X.ExcludeZeroRow();
            return X.Rows;
        }

        private double FindLeadElem(int col, int sRow, out int row)
        {
            for (int i = sRow; i < _row; i++)
            {
                if (_matr[i, col] != 0) { row = i; return _matr[i, sRow]; }
            }
            row = 0;
            return 0;
        }

        private bool Zero()
        {
            foreach (double z in _matr)
            {
                if (z != 0) { return false; }
            }
            return true; ;
        }

        public void ExcludeZeroColRow()
        {
            if (this.IsZero) { this._matr = new double[1, 1] { { 0 } }; return; }
            List<int> RowToDelete = new List<int>();
            List<int> ColToDelete = new List<int>();
            Matrix V = new Matrix(_col, 1);
            for (int i = 0; i < V._row; i++)
            {
                V._matr[i, 0] = 1;
            }
            Matrix zRow = this * V;
            for (int i = 0; i < zRow._row; i++)
            {
                if (zRow._matr[i, 0] == 0) { RowToDelete.Add(i); }
            }
            Matrix Trans = this.Transpose();
            V = new Matrix(Trans._col, 1);
            for (int i = 0; i < V._row; i++)
            {
                V._matr[i, 0] = 1;
            }
            Matrix zCol = Trans * V;
            for (int i = 0; i < zCol._row; i++)
            {
                if (zCol._matr[i, 0] == 0) { ColToDelete.Add(i); }
            }
            this.DeleteColArray(ColToDelete);
            this.DeleteRowArray(RowToDelete);
        }

        private void DeleteRowArray(List<int> rows)
        {
            Matrix X = new Matrix(this._row - rows.Count, this._col);
            int xi = 0;
            int n = 0;
            try
            {
                for (int i = 0; i < this._row; i++)
                {
                    xi = i - n;
                    if (rows.Contains(i))
                    {
                        n++;
                        continue;
                    }
                    for (int j = 0; j < this._col; j++)
                    {
                        X._matr[xi, j] = this._matr[i, j];
                    }
                }
                this._matr = X._matr;
                this._col = X._col;
                this._row = X._row;
            }
            catch
            {
                throw new IncompabilityOfColumnsAndRows("Row number is negative or more then number of rows in matrix");
            }
        }

        private void DeleteColArray(List<int> cols)
        {
            Matrix X = new Matrix(this._row, this._col - cols.Count);
            int xj = 0;
            int n = 0;
            try
            {
                for (int j = 0; j < this._col; j++)
                {
                    xj = j - n;
                    if (cols.Contains(j))
                    {
                        n++;
                        continue;
                    }
                    for (int i = 0; i < this._row; i++)
                    {
                        X._matr[i, xj] = this._matr[i, j];
                    }
                }
                this._matr = X._matr;
                this._col = X._col;
                this._row = X._row;
            }
           catch
            {
                throw new IncompabilityOfColumnsAndRows("Col number is negative or more then number of columns in matrix");
            }
        }

        #endregion

        #region Override operations

        public static Matrix operator +(Matrix A, Matrix B)
        {
            return AddMatrix(A, B);
        }

        public static Matrix operator -(Matrix A, Matrix B)
        {
            return Subtract(A, B);
        }

        public static Matrix operator *(Matrix A, Matrix B)
        {
            return MultiplyByMatrix(A, B);
        }

        public static Matrix operator *(Matrix A, double K)
        {
            return MultiblyByNumber(A, K);
        }

        public static Matrix operator *(double K, Matrix A)
        {
            return MultiblyByNumber(A, K);
        }

        #endregion

        public void ConsoleWrite()
        {
            for (int i = 0; i < this._row; i++)
            {
                if (i != 0) { Console.WriteLine(); }
                for (int j = 0; j < this._col; j++)
                {
                    Console.Write(_matr[i, j] + " ");
                }
            }
        }
    }
}
