using System;
using System.Collections.Generic;

namespace MatrixOperations
{
    class Matrix
    {
        public Matrix(int row, int col)
        {
            _matr = new double[row, col];
            this._col = col;
            this._row = row;
        }

        public Matrix(double[,] matr)
        {
            _matr = matr;
            _row = matr.GetLength(0);
            _col = matr.GetLength(1);
        }

        private double[,] _matr;
        private int _col;
        private int _row;

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

        #region Operations

        public Matrix Power(int n)
        {
            Matrix X = this;
            while (n > 1)
            {
                X = MultiplyByMatrix(this, X);
                if (X == null) { return null; }
                n--;
            }
            return X;
        }

        private static Matrix MultiplyByMatrix(Matrix A, Matrix B)
        {
            bool canMultiply = A._col == B._row ? true : false;
            if (!canMultiply) { throw new IncompabilityOfColumnsAndRows("Number of columns in A is not equal number of rows in B"); }
            else
            {
                Matrix X = new Matrix(A._row, B._col);
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
                return X;
            }
        }

        private static Matrix MultiblyByNumber(Matrix M, double n)
        {
            Matrix X = new Matrix(M._row, M._col);
            for (int i = 0; i < M._row; i++)
            {
                for (int j = 0; j < M._col; j++)
                {
                    X._matr[i, j] = M._matr[i, j] * n;
                }
            }
            return X;
        }

        private static Matrix AddMatrix(Matrix A, Matrix B)
        {
            bool canAdd = (A._col == B._col)&&(A._row==B._row) ? true : false;
            if (!canAdd) { throw new IncompabilityOfColumnsAndRows("Number of columns and rows in both of matrixs must be equal"); }
            else
            {
                Matrix X = new Matrix(A._row, A._col);
                for(int i=0;i<A._row;i++)
                {
                    for(int j=0; j<A._col;j++)
                    {
                        X._matr[i, j] = A._matr[i, j] + B._matr[i, j];
                    }
                }
                return X;
            }         
        }

        private static Matrix Subtract(Matrix A, Matrix B)
        {
            bool canAdd = (A._col == B._col) && (A._row == B._row) ? true : false;
            if (!canAdd) { throw new IncompabilityOfColumnsAndRows("Number of columns and rows in both of matrixs must be equal"); }
            else
            {
                Matrix X = new Matrix(A._row, A._col);
                for (int i = 0; i < A._row; i++)
                {
                    for (int j = 0; j < A._col; j++)
                    {
                        X._matr[i, j] = A._matr[i, j] - B._matr[i, j];
                    }
                }
                return X;
            }
        }

        public Matrix Transpose()
        {
            Matrix X = new Matrix(this._col, this._row);
            for(int i=0; i<this._row; i++)
            {
                for(int j=0; j<this._col; j++)
                {
                    X._matr[j, i] = this._matr[i, j];
                }
            }
            return X;
        }

        public double Determinant()
        {
            if (this._col != this._row) { throw new IncompabilityOfColumnsAndRows("It is not square matrix"); }
            else { return Determinant(this); }
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
                for (int j=0; j<X._col; j++)
                {
                    Matrix J = X.Minor(0, j);
                    double minor = Determinant(J);
                    res = res + Math.Pow(-1, j+2) * X._matr[0, j] * minor;
                }
                return res;
            }
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
            return X;
        }

        public void SwapRows(int r1, int r2)
        {
            if ((this._row < r1) | (this._row < r2) | (r1 < 0) | (r2 < 0) | (r1 == r2)) { throw new WrongNumberOfRows(); }
            else
            {
                for(int j=0; j<_col; j++)
                {
                    double x1 = this._matr[r1, j];
                    double x2 = this._matr[r2, j];
                    this._matr[r1, j] = x2;
                    this._matr[r2, j] = x1;
                }
            }
        }

        public void SwapColumns(int col1, int col2)
        {
            if ((this._col < col1) | (this._col < col2) | (col1 < 0) | (col2 < 0) | (col1==col2)) { throw new WrongNumberOfColumns(); }
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

        public void MultiplyRowByNumber(int r, double n)
        {
            if ((this._row < r) | (r<0) ) { throw new WrongNumberOfRows(); }
            else
            {
                for(int j=0; j<_col; j++)
                {
                    this._matr[r, j] = this._matr[r, j] * n;
                }
            }
        }

        public void DivRowByNumber(int r, double n)
        {
            if ((this._row < r) | (r < 0)) { throw new WrongNumberOfRows(); }
            else
            {
                for (int j = 0; j < _col; j++)
                {
                    this._matr[r, j] = (this._matr[r, j]) * (1/ n);
                }
            }
        }

        public void AddNumberToRow(int r, double n)
        {
            if ((this._row < r) | (r < 0)) { throw new WrongNumberOfRows(); }
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
            if ((this._row < r) | (r < 0)) { throw new WrongNumberOfRows(); }
            else
            {
                for (int j = 0; j < _col; j++)
                {
                    this._matr[r, j] = this._matr[r, j] - n;
                }
            }
        }

        public void MultiplyColByNumber(int col, double n)
        {
            if ((this._col < col) | (col < 0)) { throw new WrongNumberOfColumns(); }
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
            if ((this._col < col) | (col < 0)) { throw new WrongNumberOfColumns(); }
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
            if ((this._col < col) | (col < 0)) { throw new WrongNumberOfColumns(); }
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
            if ((this._col < col) | (col < 0)) { throw new WrongNumberOfColumns(); }
            else
            {
                for (int i = 0; i < _row; i++)
                {
                    this._matr[i, col] = this._matr[i, col] - n;
                }
            }
        }

        public void DeleteRow(int r)
        {
            if((r>_row) | (r < 0)) { throw new WrongNumberOfRows("Row number is negative or more then number of rows in matrix"); }
            Matrix X = new Matrix(_row - 1, _col);
            bool flag = false;
            for(int i=0; i<_row;i++)
            {
                int z = i;
                if (i == r) { flag = true; continue; }
                if (flag) { z = z - 1; }
                for (int j=0; j<_col; j++)
                {
                    X._matr[z, j] = this._matr[i, j];
                }             
            }
            this._matr = X._matr;
            this._col = X._col;
            this._row = X._row;
        }

        public void DeleteCol(int col)
        {
            if ((col > _col) | (col < 0)) { throw new WrongNumberOfColumns("Col number is negative or more then number of columns in matrix"); }
            Matrix X = new Matrix(_row, _col-1);
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

        public void AddRowMultiplyedByNumber(int row1, double n, int row2)
        {
            for (int j = 0; j < _col; j++)
            {
                _matr[row2, j] = _matr[row2, j] + _matr[row1, j] * n;
            }
        }

        public void AddColMultiplyedByNumber(int col1, double n, int col2)
        {
            for(int i=0; i<_row; i++)
            {
                _matr[i, col2] = _matr[i, col2] + _matr[i, col1] * n;
            }
        }

        public Matrix GetStepMatrix()
        {
            if ((this._col == 1) | (this._row == 1)) { _matr[0, 0] = 1; return this; }
            if (this.IsZero) { return this; }
            this.ExcludeZeroColRow();
            int row = 0;
            double leadElement = 0;
            Matrix X = new Matrix(_row, _col);
            X._matr = this._matr.Clone() as double[,];
            switch (_col >= _row)
            {
                case (false):
                    {
                        for (int z = 0; z < _col; z++)
                        {
                            leadElement = X.FindLeadElem(z, z, out row);
                            if (leadElement == 0) { continue; }
                            if (row != 0) { X.SwapRows(z, row); }
                            X.DivRowByNumber(z, leadElement);
                            for (int i = z + 1; i < _row; i++)
                            {
                                X.AddRowMultiplyedByNumber(z, -_matr[i, z], i);
                            }
                        }
                        return X;
                    }
                case (true):
                    {
                        for (int z = 0; z < _row; z++)
                        {
                            leadElement = X.FindLeadElem(z, z, out row);
                            if (leadElement == 0) { continue; }
                            if (row != 0) { X.SwapRows(z, row); }
                            X.DivRowByNumber(z, leadElement);
                            for (int i = z + 1; i < _row; i++)
                            {
                                X.AddRowMultiplyedByNumber(z, -_matr[i, z], i);
                            }
                        }
                        return X;
                    }
            }
            return this;
        }

        public void ThisToStepMatrix()
        {
            if ((this._col == 1) | (this._row == 1)) { _matr[0, 0] = 1; return; }
            if (this.IsZero) { return; }
            this.ExcludeZeroColRow();
            int row = 0;
            double leadElement = 0;
            switch (_col>=_row)
            {
                case (false):
                    {
                        for (int z = 0; z < _col; z++)
                        {
                            leadElement = FindLeadElem(z, z, out row);
                            if (row != 0) { this.SwapRows(z, row); }
                            this.DivRowByNumber(z, leadElement);
                            for (int i = z + 1; i < _row; i++)
                            {
                                this.AddRowMultiplyedByNumber(z, -_matr[i, z], i);
                            }
                        }
                        break;
                    }
                case (true):
                    {
                        for (int z = 0; z < _row; z++)
                        {
                            leadElement = FindLeadElem(z, z, out row);
                            if (row != 0) { this.SwapRows(z, row); }
                            this.DivRowByNumber(z, leadElement);
                            for (int i = z + 1; i < _row; i++)
                            {
                                this.AddRowMultiplyedByNumber(z, -_matr[i, z], i);
                            }
                        }
                        break;
                    }                    
            }                           
        }

        public int Rang()
        {
            Matrix X = this.GetStepMatrix();
            X.ExcludeZeroRow();
            return X.Rows;
        }

        private double FindLeadElem(int col, int sRow, out int row)
        {
            for (int i = sRow; i< _row; i++)
            {
                if (_matr[i, col] != 0) { row = i - sRow ; return _matr[i, sRow]; }
            }
            row = 0;
            return 0;
        }

        private bool Zero()
        {
            foreach(double z in _matr)
            {
                if (z != 0) { return false; }
            }
            return true; ;
        }

        public void ExcludeZeroColRow()
        {
            if (this.IsZero) { this._matr = new double[1,1] { { 0 } }; return; }
            List<int> RowToDelete = new List<int>();
            List<int> ColToDelete = new List<int>();
            Matrix V = new Matrix(_col, 1);
            for(int i=0; i<V._row; i++)
            {
                V._matr[i, 0] = 1;
            }
            Matrix zRow = this * V;
            for (int i=0; i<zRow._row; i++)
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
            foreach(int i in RowToDelete)
            {
                this.DeleteRow(i);
            }
            foreach(int j in ColToDelete)
            {
                this.DeleteCol(j);
            }
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

        #region Override operations

        public static Matrix operator + (Matrix A, Matrix B)
        {
            return AddMatrix(A, B);
        }

        public static Matrix operator - (Matrix A, Matrix B)
        {
            return Subtract(A, B);
        }

        public static Matrix operator * (Matrix A, Matrix B)
        {
            return MultiplyByMatrix(A, B);
        }

        public static Matrix operator * (Matrix A, double K)
        {
            return MultiblyByNumber(A, K);
        }

        public static Matrix operator * (double K, Matrix A)
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
