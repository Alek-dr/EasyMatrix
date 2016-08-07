using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace EasyMatrix
{
    using Matrix = Matrix.Matrix;

    class Parser
    {
        private char[] _brack;

        private char[] _actions;

        private string _inputString;

        Regex justNumb;

        private List<Matrix> WorkingMatrix = new List<Matrix>();

        public Parser(string str)
        {
            _brack = new char[] { '(', ')' };
            _actions = new char[] { '*', '+', '-' };
            _inputString = str;
            justNumb = new Regex(@"^\-?\d+[,.]?\d*$");
            OwnMatrixCollection();
        }

        private void OwnMatrixCollection()
        {
            for (int i = 0; i < MatrixManager._matrixCollection.Count; i++)
            {
                WorkingMatrix.Add(MatrixManager._matrixCollection[i]);
            }
        }

        public Matrix GetResult()
        {
            //где то уже вызвали метод Valid и он вернул true
            if (BracketExpression(_inputString))
            {
                string s1 = _inputString;
                string s2 = String.Empty;
                string s3 = String.Empty;
                bool flag = true;
                try
                {
                    while (s1.Contains('('))
                    {
                        flag = true;
                        s2 = Bracket(s1);
                        s3 = s2;
                        while (s2.Contains('*'))
                        {
                            Match number = justNumb.Match(s2);
                            if (number.Success) { break; }
                            flag = false;
                            s2 = MultiplyString(s2);
                        }
                        while (s2.Contains('+'))
                        {
                            Match number = justNumb.Match(s2);
                            if (number.Success) { break; }
                            flag = false;
                            s2 = AddString(s2);
                        }
                        while (s2.Contains('-'))
                        {
                            Match number = justNumb.Match(s2);
                            if (number.Success) { break; }
                            flag = false;
                            s2 = SubString(s2);
                        }
                        if (flag)
                        {
                            s1 = DeleteBracket(s1);
                        }
                        s1 = s1.Replace(s3, s2);
                    }
                    while (s1.Contains('*'))
                    {
                        s1 = MultiplyString(s1);
                    }
                    while (s1.Contains('-'))
                    {
                        s1 = SubString(s1);
                    }
                    while (s1.Contains('+'))
                    {
                        s1 = AddString(s1);
                    }
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    return GetMatrixFromString(s1);
                }
                catch
                {
                    Matrix res = new Matrix(1, 1);
                    res.Message = new StringBuilder("При выполнении операции произошла ошибка");
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    return res;
                }
            }
            else
            {
                //если выражение не содержит скобки
                string s1 = _inputString;
                try
                {
                    while (s1.Contains('*'))
                    {
                        s1 = MultiplyString(s1);
                    }
                    while (s1.Contains('-'))
                    {
                        s1 = SubString(s1);
                    }
                    while (s1.Contains('+'))
                    {
                        s1 = AddString(s1);
                    }
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    return GetMatrixFromString(s1);
                }
                catch
                {
                    Matrix res = new Matrix(1, 1);
                    res.Message = new StringBuilder("При выполнении операции произошла ошибка");
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    return res;
                }
            }
        }

        private Matrix GetMatrixFromString(string str)
        {
            for (int i = WorkingMatrix.Count - 1; i > 0; i--)
            {
                if (WorkingMatrix[i].Name == str) { return WorkingMatrix[i]; }
            }
            Matrix Res = new Matrix(1, 1);
            Res.Message = new StringBuilder("При выполнении операции произошла ошибка");
            return Res;
        }

        private string DeleteBracket(string str)
        {
            Regex b = new Regex(@"\(|\)");
            //часть 1
            Regex brackExpr = new Regex(@"\([A-z]+[0-9]*\)");
            Match m = brackExpr.Match(str);
            if (m.Success)
            {
                //удаляет скобки вокруг переменной
                while (m.Success)
                {
                    String s = b.Replace(m.Value, "");
                    str = str.Replace(m.Value, s);
                    m = brackExpr.Match(str);
                }
                return str;
            }
            Regex brackExpr1 = new Regex(@"\(\-?\d+[,.]?\d*\)");
            Match m1 = brackExpr1.Match(str);
            if (m1.Success)
            {
                //удаляет скобки вокруг числа
                while (m1.Success)
                {
                    String s = b.Replace(m1.Value, "");
                    str = str.Replace(m1.Value, s);
                    m1 = brackExpr.Match(str);
                }
                Match m2 = brackExpr1.Match(str);
                if (m2.Success)
                {
                    while (m2.Success)
                    {
                        String s = b.Replace(m2.Value, "");
                        str = str.Replace(m2.Value, s);
                        m2 = b.Match(str);
                    }
                }
                return str;
            }
            else { return String.Empty; }
        }

        private string Bracket(string str)
        {
            int n = 0;
            int ind = str.LastIndexOf('(');
            for (int i = ind + 1; i < str.Length; i++)
            {
                n++;
                if (str[i] == ')')
                {
                    return str.Substring(ind + 1, n - 1);
                }
            }
            return String.Empty;
        }

        private string AddString(string str)
        {
            //сложение матриц
            Match m;
            Regex add = new Regex(@"\-?_??[A-z]+[0-9]*\+\-?_??[A-z]+[0-9]*");
            m = add.Match(str);
            string addString = m.Value;
            string result = String.Empty;
            if (m.Success)
            {
                for (int i = 0; i < WorkingMatrix.Count; i++)
                {
                    for (int j = 0; j < WorkingMatrix.Count; j++)
                    {
                        if (addString == WorkingMatrix[i].Name + "+" + WorkingMatrix[j].Name)
                        {
                            Matrix X = WorkingMatrix[i] + WorkingMatrix[j];
                            X.Name = "_w" + WorkingMatrix.Count.ToString();
                            WorkingMatrix.Add(X);
                            str = str.Replace(addString, X.Name);
                            return str;
                        }
                        if (addString == "-" + WorkingMatrix[i].Name + "+" + WorkingMatrix[j].Name)
                        {
                            Matrix X = WorkingMatrix[j] - WorkingMatrix[i];
                            X.Name = "_w" + WorkingMatrix.Count.ToString();
                            WorkingMatrix.Add(X);
                            str = str.Replace(addString, X.Name);
                            return str;
                        }
                        if (addString == "-" + WorkingMatrix[i].Name + "+" + "-" + WorkingMatrix[j].Name)
                        {
                            Matrix X = WorkingMatrix[j] + WorkingMatrix[i];
                            X = X * -1;
                            X.Name = "_w" + WorkingMatrix.Count.ToString();
                            WorkingMatrix.Add(X);
                            str = str.Replace(addString, X.Name);
                            return str;
                        }
                        if (addString == WorkingMatrix[i].Name + "+" + "-" + WorkingMatrix[j].Name)
                        {
                            Matrix X = WorkingMatrix[i] - WorkingMatrix[j];
                            X = X * -1;
                            X.Name = "_w" + WorkingMatrix.Count.ToString();
                            WorkingMatrix.Add(X);
                            str = str.Replace(addString, X.Name);
                            return str;
                        }
                    }
                }
            }
            // сложить 2 числа
            Regex mult3 = new Regex(@"\-?\d+[,.]?\d*\+\-?\d+[,.]?\d*");
            m = mult3.Match(str);
            if (m.Success)
            {
                string[] numbersString = m.Value.Split('+');
                string N1 = numbersString[0].Replace('.', ',');
                string N2 = numbersString[1].Replace('.', ',');
                double k1 = Double.Parse(N1);
                double k2 = Double.Parse(N2);
                double k = k1 + k2;
                str = str.Replace(m.Value, k.ToString());
                return str;
            }
            return str;
        }

        private string SubString(string str)
        {
            //вычесть матрицы
            Match m;
            Regex add = new Regex(@"\-?_??[A-z]+[0-9]*\-\-?_??[A-z]+[0-9]*");
            m = add.Match(str);
            string addString = m.Value;
            string result = String.Empty;
            if (m.Success)
            {
                for (int i = 0; i < WorkingMatrix.Count; i++)
                {
                    for (int j = 0; j < WorkingMatrix.Count; j++)
                    {
                        if (addString == WorkingMatrix[i].Name + "-" + WorkingMatrix[j].Name)
                        {
                            Matrix X = WorkingMatrix[i] - WorkingMatrix[j];
                            X.Name = "_w" + WorkingMatrix.Count.ToString();
                            WorkingMatrix.Add(X);
                            str = str.Replace(addString, X.Name);
                            return str;
                        }
                        if (addString == "-" + WorkingMatrix[i].Name + "-" + WorkingMatrix[j].Name)
                        {
                            Matrix X = WorkingMatrix[i] + WorkingMatrix[j];
                            X = X * -1;
                            X.Name = "_w" + WorkingMatrix.Count.ToString();
                            WorkingMatrix.Add(X);
                            str = str.Replace(addString, X.Name);
                            return str;
                        }
                        if (addString == WorkingMatrix[i].Name + "-" + "-" + WorkingMatrix[j].Name)
                        {
                            Matrix X = WorkingMatrix[i] + WorkingMatrix[j];
                            X.Name = "_w" + WorkingMatrix.Count.ToString();
                            WorkingMatrix.Add(X);
                            str = str.Replace(addString, X.Name);
                            return str;
                        }
                        if (addString == "-" + WorkingMatrix[i].Name + "-" + "-" + WorkingMatrix[j].Name)
                        {
                            Matrix X = WorkingMatrix[j] - WorkingMatrix[i];
                            X.Name = "_w" + WorkingMatrix.Count.ToString();
                            WorkingMatrix.Add(X);
                            str = str.Replace(addString, X.Name);
                            return str;
                        }
                    }
                }
            }
            // вычесть 2 числа
            //первое число любое, второе положительное
            Regex mult3 = new Regex(@"\-?\d+[,.]?\d*\-\d+[,.]?\d*");
            Regex numb1 = new Regex(@"^\-?\d+[,.]?\d*");
            Regex numb2 = new Regex(@"\-?\d+[,.]?\d*$");
            m = mult3.Match(str);
            Match M;
            if (m.Success)
            {
                M = numb1.Match(m.Value);

                string N1 = M.Value;
                N1 = N1.Replace('.', ',');
                double k1 = Double.Parse(N1);

                M = numb2.Match(m.Value);
                string N2 = M.Value;
                N2 = N2.Replace('.', ',');
                double k2 = Double.Parse(N2);
                double k = 0;
                if (k1 > 0)
                {
                    k = k1 + k2;
                }
                if (k1 < 0)
                {
                    k = k1 - k2;
                }
                str = str.Replace(m.Value, k.ToString());
                return str;
            }
            //первое число любое, перед вторым знак минус
            Regex mult4 = new Regex(@"\-?\d+[,.]?\d*\-\-\d+[,.]?\d*");
            m = mult4.Match(str);
            if (m.Success)
            {
                M = numb1.Match(m.Value);

                string N1 = M.Value;
                N1 = N1.Replace('.', ',');
                double k1 = Double.Parse(N1);

                M = numb2.Match(m.Value);
                string N2 = M.Value;
                N2 = N2.Replace('.', ',');
                double k2 = Double.Parse(N2);
                double k = 0;
                k = k1 - k2;
                str = str.Replace(m.Value, k.ToString());
                return str;
            }
            return str;
        }

        private string MultiplyString(string str)
        {
            //часть 1 - матрица на матрицу
            Match m;
            string multiplication = String.Empty;
            Regex mult = new Regex(@"\-?_??[A-z]+[0-9]*\*\-?_??[A-z]+[0-9]*");
            m = mult.Match(str);
            if (m.Success)
            {
                multiplication = m.Value;
                //проверка каждой матрицы с каждой, включая усножение на себя
                for (int i = 0; i < WorkingMatrix.Count; i++)
                {
                    for (int j = 0; j < WorkingMatrix.Count; j++)
                    {
                        if (multiplication == WorkingMatrix[i].Name + "*" + WorkingMatrix[j].Name)
                        {
                            Matrix X = WorkingMatrix[i] * WorkingMatrix[j];
                            X.Name = "_w" + WorkingMatrix.Count.ToString();
                            WorkingMatrix.Add(X);
                            str = str.Replace(multiplication, X.Name);
                            return str;
                        }
                        if (multiplication == "-" + WorkingMatrix[i].Name + "*" + WorkingMatrix[j].Name)
                        {
                            Matrix X = WorkingMatrix[i] * WorkingMatrix[j];
                            X = X * -1;
                            X.Name = "_w" + WorkingMatrix.Count.ToString();
                            WorkingMatrix.Add(X);
                            str = str.Replace(multiplication, X.Name);
                            return str;
                        }
                        if (multiplication == WorkingMatrix[i].Name + "*" + "-" + WorkingMatrix[j].Name)
                        {
                            Matrix X = WorkingMatrix[i] * WorkingMatrix[j];
                            X = X * -1;
                            X.Name = "_w" + WorkingMatrix.Count.ToString();
                            WorkingMatrix.Add(X);
                            str = str.Replace(multiplication, X.Name);
                            return str;
                        }
                        if (multiplication == "-" + WorkingMatrix[i].Name + "*" + "-" + WorkingMatrix[j].Name)
                        {
                            Matrix X = WorkingMatrix[i] * WorkingMatrix[j];
                            X.Name = "_w" + WorkingMatrix.Count.ToString();
                            WorkingMatrix.Add(X);
                            str = str.Replace(multiplication, X.Name);
                            return str;
                        }
                    }
                }
            }
            //часть 2 - матрица на число
            Regex mult1 = new Regex(@"\-?_??[A-z]+[0-9]*\*\-?\d+[,.]?\d*");
            Regex numb = new Regex(@"\-?\d+[,.]?\d*$");
            m = mult1.Match(str);
            if (m.Success)
            {
                multiplication = m.Value;
                string sn = numb.Match(multiplication).Value;
                string N = sn.Replace('.', ',');
                multiplication = multiplication.Replace(sn, N);
                str = str.Replace(sn, N);
                double n = Double.Parse(N);
                for (int i = 0; i < WorkingMatrix.Count; i++)
                {
                    if (multiplication == WorkingMatrix[i].Name + "*" + n)
                    {
                        Matrix X = WorkingMatrix[i] * n;
                        X.Name = "_w" + WorkingMatrix.Count.ToString();
                        WorkingMatrix.Add(X);
                        str = str.Replace(multiplication, X.Name);
                        return str;
                    }
                    if (multiplication == "-" + WorkingMatrix[i].Name + "*" + n)
                    {
                        Matrix X = WorkingMatrix[i] * n;
                        X = X * -1;
                        X.Name = "_w" + WorkingMatrix.Count.ToString();
                        WorkingMatrix.Add(X);
                        str = str.Replace(multiplication, X.Name);
                        return str;
                    }
                }
            }
            //часть 3 - число на матрицу
            Regex mult2 = new Regex(@"-?\d+[,.]?\d*\*\-?_??[A-z]+[0-9]*");
            m = mult2.Match(str);
            Regex numb2 = new Regex(@"^\-?\d+[,.]?\d*");
            if (m.Success)
            {
                multiplication = m.Value;
                string sn1 = numb2.Match(multiplication).Value;
                string N1 = sn1.Replace('.', ',');
                multiplication = multiplication.Replace(sn1, N1);
                str = str.Replace(sn1, N1);
                double n1 = Double.Parse(N1);

                for (int i = 0; i < WorkingMatrix.Count; i++)
                {
                    if (multiplication == n1 + "*" + WorkingMatrix[i].Name)
                    {
                        Matrix X = WorkingMatrix[i] * n1;
                        X.Name = "_w" + WorkingMatrix.Count.ToString();
                        WorkingMatrix.Add(X);
                        str = str.Replace(multiplication, X.Name);
                        return str;
                    }
                    if (multiplication == n1 + "*" + "-" + WorkingMatrix[i].Name)
                    {
                        Matrix X = WorkingMatrix[i] * n1;
                        X = X * -1;
                        X.Name = "_w" + WorkingMatrix.Count.ToString();
                        WorkingMatrix.Add(X);
                        str = str.Replace(multiplication, X.Name);
                        return str;
                    }
                }
            }
            //часть 4 число на число
            Regex mult3 = new Regex(@"\-?\d+[,.]?\d*\*\-?\d+[,.]?\d*");
            m = mult3.Match(str);
            if (m.Success)
            {
                string[] numbersString = m.Value.Split('*');
                string N1 = numbersString[0].Replace('.', ',');
                string N2 = numbersString[1].Replace('.', ',');
                double k1 = Double.Parse(N1);
                double k2 = Double.Parse(N2);
                double k = k1 * k2;
                str = str.Replace(m.Value, k.ToString());
                return str;
            }
            return String.Empty;
        }

        public bool Valid()
        {
            //предполагается, что во вненем коде проверяется, 
            //можно ли получить результат
            bool flag = false;
            for (int i = 0; i < WorkingMatrix.Count; i++)
            {
                if (_inputString.Contains(WorkingMatrix[i].Name))
                {
                    flag = true;
                }
            }
            if (!flag) { return false; }

            int br1 = CountOfChar(_inputString, '(');
            int br2 = CountOfChar(_inputString, ')');
            return br1 == br2 ? true : false;
        }

        private bool BracketExpression(string str)
        {
            if (str.Contains('(')) { return true; }
            return false;
        }

        private int CountOfChar(string str, char ch)
        {
            int n = 0;
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == ch) { n++; }
            }
            return n;
        }
    }
}
