using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EasyMatrix
{
    using Matrix = Matrix.Matrix;

    public static class MatrixManager
    {
        public static ObservableCollection<Matrix> _matrixCollection = new ObservableCollection<Matrix>();

        public static void AddMatrix(double[,] m)
        {
            Matrix X = new Matrix(m);
            _matrixCollection.Add(X);
        }
        
        public static Matrix FindMatr(string Name)
        {
            for(int i=0; i<_matrixCollection.Count; i++)
            {
                if (_matrixCollection[i].Name == Name) { return _matrixCollection[i]; }
            }
            return null;
        }

        public static void Delete(Matrix X)
        {
            _matrixCollection.Remove(X);
        }

        public static void DeleteAll()
        {
            _matrixCollection.Clear();
            StandartName.Clear();
            StandartName = new Queue<char>(liters);
        }

        public static bool NameExist(string Name)
        {
            for (int i = 0; i < _matrixCollection.Count; i++)
            {
                if (_matrixCollection[i].Name == Name) { return true; }
            }
            return false;
        }

        public static void AddMatrix(Matrix m)
        {
            for(int i=0; i < _matrixCollection.Count;i++)
            {
                if(_matrixCollection[i].Name==m.Name)
                {
                    _matrixCollection.RemoveAt(i);
                }                
            }
            _matrixCollection.Add(m);
        }

        private static char[] liters = new char[] { 'A', 'B', 'C', 'D','F','G','H','I','K',
        'L','M','N','O','P','Q','R','S','T','V','X','Y','Z' };

        public static Queue<Char> StandartName = new Queue<char>(liters);
    }
}
