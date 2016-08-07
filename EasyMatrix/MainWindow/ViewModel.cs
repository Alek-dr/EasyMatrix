using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Media;
using System.Text.RegularExpressions;

namespace EasyMatrix
{
    class ViewModel : ViewModelBase
    {
        public ViewModel()
        {
            this.Enter = new RelayCommand<TextBox>(this.EnterCommand);
            this.Determinant = new RelayCommand<TextBox>(this.DetCommand);
            this.Transpose = new RelayCommand<TextBox>(this.TransposeCommand);
            this.REF = new RelayCommand<TextBox>(this.REFCommand);
            this.Rang = new RelayCommand<TextBox>(this.RangCommand);
            this.Power = new RelayCommand<TextBox>(this.PowerCommand);
            this.Add = new RelayCommand<TextBox>(this.AddCommand);
            this.Mult = new RelayCommand<TextBox>(this.MultCommand);
            this.Sub = new RelayCommand<TextBox>(this.SubCommand);
            this.Delete = new RelayCommand(this.DeleteCommand);
            this.Gauss_Jordan = new RelayCommand<TextBox>(this.Gauss_Jordan_Command);
            this.Inverse = new RelayCommand<TextBox>(this.Inverse_Command);
            this.InverseCof = new RelayCommand<TextBox>(this.InverseCof_Command);
            this.ClearWorkspace = new RelayCommand<TextBox>(this.ClearWorkspaceCommand);
            this.ClearMatrixCollection = new RelayCommand(this.ClearMatrixCollectionCommand);
            this.SettingWind = new RelayCommand<TextBox>(this.SettingsStart);
            this.ButtonEnable = true;
            this.StepByStep = false;
            SetFiles();
        }

        private bool StepByStep;

        #region Properties

        #region files
        public string file1 { get; set; }
        public string file2 { get; set; }
        public string file3 { get; set; }
        public string file4 { get; set; }
        public string file5 { get; set; }
        public string file6 { get; set; }
        public string file7 { get; set; }
        public string file8 { get; set; }
        public string file9 { get; set; }
        public string file10 { get; set; }
        #endregion

        private ICommand _createNewMatrix
        {
            get { return new RelayCommand(() => MatrixCreateStart()); }
        }

        public FontFamily CurrentFont
        {
            get { return Fonts.SystemFontFamilies.ElementAt(SettingsControl.Font); }
        }

        public int FontSize
        {
            get { return SettingsControl.FontSize; }
        }

        public int Round
        {
            get { return SettingsControl.Round; }
        }

        public ICommand CreateNewMatrix
        {
            get { return _createNewMatrix; }
        }

        public ObservableCollection<Matrix.Matrix> MatrixCollection
        {
            get { return MatrixManager._matrixCollection; }
        }

        public Matrix.Matrix CurrentMatrix { get; set; }

        public RelayCommand<TextBox> Determinant { get; private set; }

        public RelayCommand<TextBox> Transpose { get; private set; }

        public RelayCommand<TextBox> Enter { get; private set; }

        public RelayCommand<TextBox> REF { get; private set; }

        public RelayCommand<TextBox> Rang { get; private set; }

        public RelayCommand<TextBox> Power { get; private set; }

        public RelayCommand<TextBox> Gauss_Jordan { get; private set; }

        public RelayCommand<TextBox> Inverse { get; private set; }

        public RelayCommand<TextBox> InverseCof { get; private set; }

        public RelayCommand<TextBox> Add { get; private set; }

        public RelayCommand<TextBox> Mult { get; private set; }

        public RelayCommand<TextBox> Sub { get; private set; }

        public RelayCommand Delete { get; private set; }

        public RelayCommand<TextBox> ClearWorkspace { get; private set; }

        public RelayCommand ClearMatrixCollection { get; private set; }

        public RelayCommand<TextBox> SettingWind { get; private set; }

        public bool STEP
        {
            get { return StepByStep; }
            set { StepByStep = value; }
        }

        public bool ButtonEnable { get; private set; }

        #endregion

        #region Commands

        private void RangCommand(TextBox tb)
        {
            ButtonEnable = false;
            RaisePropertyChanged(() => ButtonEnable);
            if (SettingsControl.TextBox == null) { SettingsControl.TextBox = tb; }
            double res = 0;
            if (CurrentMatrix == null)
            {
                ButtonEnable = true;
                RaisePropertyChanged(() => ButtonEnable);
                return;
            }
            else
            {
                try
                {
                    res = CurrentMatrix.Rang();
                }
                catch (Matrix.IncompabilityOfColumnsAndRows)
                {
                    ButtonEnable = true;
                    RaisePropertyChanged(() => ButtonEnable);
                    return;
                }
                //если все хорошо           
                StringBuilder str = new StringBuilder();
                str = new StringBuilder("Rang(" + CurrentMatrix.Name + ")=" + res.ToString());
                tb.AppendText(str + "\n");            
                CurrentMatrix.Message = null;
                ButtonEnable = true;
                RaisePropertyChanged(() => ButtonEnable);
            }
        }

        private void ClearWorkspaceCommand(TextBox tb)
        {
            if (SettingsControl.TextBox == null) { SettingsControl.TextBox = tb; }
            tb.Text = String.Empty;
        }

        private void ClearMatrixCollectionCommand()
        {
            MatrixManager.DeleteAll();
        }

        private void DeleteCommand()
        {
            MatrixManager.Delete(CurrentMatrix);
        }

        private void AddCommand(TextBox tb)
        {
            tb.Text += "+";
        }

        private void MultCommand(TextBox tb)
        {
            tb.Text += "*";
        }

        private void SubCommand(TextBox tb)
        {
            tb.Text += "-";
        }

        private async void REFCommand(TextBox tb)
        {
            ButtonEnable = false;
            RaisePropertyChanged(() => ButtonEnable);
            if (SettingsControl.TextBox == null) { SettingsControl.TextBox = tb; }
            if (CurrentMatrix == null)
            {
                ButtonEnable = true;
                RaisePropertyChanged(() => ButtonEnable);
                return;
            }
            else
            {
                if (StepByStep) { tb.AppendText(CurrentMatrix.Name + "="); }
                Matrix.Matrix X = new Matrix.Matrix(1, 1);               
                CurrentMatrix.Round = SettingsControl.Round;
                await Task.Run(() => X = StepByStep == false ? CurrentMatrix.GetStepMatrix() : CurrentMatrix.GetStepMatrix(WritingSteps));               
                X.Name = "Result";
                MatrixManager.AddMatrix(X);
                if(StepByStep)
                {
                    tb.AppendText("\n");
                }
                tb.AppendText(X.Name);
                Write(X, tb);
                ButtonEnable = true;
                RaisePropertyChanged(() => ButtonEnable);
            }
        }

        private async void Gauss_Jordan_Command(TextBox tb)
        {
            ButtonEnable = false;
            RaisePropertyChanged(() => ButtonEnable);
            if (SettingsControl.TextBox == null) { SettingsControl.TextBox = tb; }
            if (CurrentMatrix == null)
            {
                ButtonEnable = true;
                RaisePropertyChanged(() => ButtonEnable);
                return;
            }
            else
            {
                if (StepByStep) { tb.AppendText(CurrentMatrix.Name + "="); }
                Matrix.Matrix X = new Matrix.Matrix(1, 1);
                CurrentMatrix.Round = SettingsControl.Round;
                try
                {
                    await Task.Run(() => X = StepByStep == false ? CurrentMatrix.Gauss_Jordan() : CurrentMatrix.Gauss_Jordan(WritingSteps));
                }
                catch (Matrix.IncompabilityOfColumnsAndRows ex)
                {
                    tb.AppendText(ex.Message + "\n");
                    ButtonEnable = true;
                    RaisePropertyChanged(() => ButtonEnable);
                    return;
                }
                X.Name = "Result";
                MatrixManager.AddMatrix(X);
                if(StepByStep)
                {
                    tb.AppendText("\n");
                }              
                tb.AppendText(X.Name);
                Write(X, tb);
                ButtonEnable = true;
                RaisePropertyChanged(() => ButtonEnable);
            }
        }

        private async void Inverse_Command(TextBox tb)
        {
            ButtonEnable = false;
            RaisePropertyChanged(() => ButtonEnable);
            if (SettingsControl.TextBox == null) { SettingsControl.TextBox = tb; }
            if (CurrentMatrix == null)
            {
                ButtonEnable = true;
                RaisePropertyChanged(() => ButtonEnable);
                return;
            }
            else
            {
                
                Matrix.Matrix X = new Matrix.Matrix(1, 1);
                CurrentMatrix.Round = SettingsControl.Round;
                try
                {
                    if (StepByStep) { tb.AppendText(CurrentMatrix.Name + "="); }
                    await Task.Run(() => X = StepByStep == false ? CurrentMatrix.InverseByGauss_Gordan() : CurrentMatrix.InverseByGauss_Gordan(WritingSteps));                   
                }
                
                catch (Matrix.IncompabilityOfColumnsAndRows ex)
                {
                    if (StepByStep)
                    {
                        tb.AppendText("\n");
                    }
                    tb.AppendText(ex.Message + "\n");
                    ButtonEnable = true;
                    RaisePropertyChanged(() => ButtonEnable);
                    return;
                }
                
                X.Name = "Result";
                MatrixManager.AddMatrix(X);
                if(StepByStep)
                {
                    tb.AppendText("\n");
                }                
                tb.AppendText(X.Name);
                Write(X, tb);
                ButtonEnable = true;
                RaisePropertyChanged(() => ButtonEnable);
            }
        }

        private async void InverseCof_Command(TextBox tb)
        {
            ButtonEnable = false;
            RaisePropertyChanged(() => ButtonEnable);
            if (SettingsControl.TextBox == null) { SettingsControl.TextBox = tb; }
            if (CurrentMatrix == null)
            {
                ButtonEnable = true;
                RaisePropertyChanged(() => ButtonEnable);
                return;
            }
            else
            {
                Matrix.Matrix X = new Matrix.Matrix(1, 1);
                CurrentMatrix.Round = SettingsControl.Round;
                try
                {
                    await Task.Run(() => X = StepByStep == false ? CurrentMatrix.InverseByCofactor() : CurrentMatrix.InverseByCofactor(WritingInverseByStep, MinorDet));
                }
                catch (Matrix.IncompabilityOfColumnsAndRows ex)
                {
                    tb.AppendText(ex.Message + "\n");
                    ButtonEnable = true;
                    RaisePropertyChanged(() => ButtonEnable);
                    return;
                }
                X.Name = "Result";
                MatrixManager.AddMatrix(X);
                if(StepByStep)
                {
                    tb.AppendText("\n");
                }              
                tb.AppendText(X.Name);
                Write(X, tb);
                ButtonEnable = true;
                RaisePropertyChanged(() => ButtonEnable);
            }
        }

        private void PowerCommand(TextBox tb)
        {
            ButtonEnable = false;
            RaisePropertyChanged(() => ButtonEnable);
            if (SettingsControl.TextBox == null) { SettingsControl.TextBox = tb; }
            if (CurrentMatrix == null)
            {
                ButtonEnable = true;
                RaisePropertyChanged(() => ButtonEnable);
                return;
            }
            else
            {
                StringBuilder str = new StringBuilder();
                str = new StringBuilder(CurrentMatrix.Name + "^");
                tb.Text += str;
            }
            ButtonEnable = true;
            RaisePropertyChanged(() => ButtonEnable);
        }

        private void DetCommand(TextBox tb)
        {
            ButtonEnable = false;
            RaisePropertyChanged(() => ButtonEnable);
            if (SettingsControl.TextBox == null) { SettingsControl.TextBox = tb; }
            double res = 0;
            if (CurrentMatrix == null)
            {
                ButtonEnable = true;
                RaisePropertyChanged(() => ButtonEnable);
                return;
            }
            else
            {
                try
                {
                    if (StepByStep) { res = CurrentMatrix.Determinant(true); }
                    else { res = CurrentMatrix.Determinant(); }                 
                }
                catch (Matrix.IncompabilityOfColumnsAndRows)
                {
                    StringBuilder exc = new StringBuilder("=Матрица не явлется квадратной");
                    tb.AppendText(CurrentMatrix.Name + exc + "\n");
                    tb.SelectionStart = tb.Text.Length;
                    tb.ScrollToEnd();
                    CurrentMatrix.Message = null;
                    ButtonEnable = true;
                    RaisePropertyChanged(() => ButtonEnable);
                    return;
                }
                //если все хорошо           
                StringBuilder str = new StringBuilder();
                if (CurrentMatrix.Message!=null)
                {
                    str = new StringBuilder("det(" + CurrentMatrix.Name + ")=" + CurrentMatrix.Message + "=" + res.ToString() + "\n");
                }
                else
                {
                    str = new StringBuilder("det(" + CurrentMatrix.Name + ")=" + res.ToString() + "\n");
                }
                tb.AppendText(str.ToString());
                tb.SelectionStart = tb.Text.Length;
                tb.ScrollToEnd();
                CurrentMatrix.Message = null;
                ButtonEnable = true;
                RaisePropertyChanged(() => ButtonEnable);
            }
        }

        private void TransposeCommand(TextBox tb)
        {
            ButtonEnable = false;
            RaisePropertyChanged(() => ButtonEnable);
            if (SettingsControl.TextBox == null) { SettingsControl.TextBox = tb; }
            if (CurrentMatrix == null)
            {
                ButtonEnable = true;
                RaisePropertyChanged(() => ButtonEnable);
                return;
            }
            Matrix.Matrix X = CurrentMatrix.Transpose();
            X.Round = SettingsControl.Round;
            X.Name = "Result";
            MatrixManager.AddMatrix(X);
            tb.AppendText(X.Name);
            Write(X, tb);
            ButtonEnable = true;
            RaisePropertyChanged(() => ButtonEnable);
        }

        #endregion

        #region Methods

        private void SetFiles()
        {
            file1 = @"Resources\Help\Kind.rtf";
            file2 = @"Resources\Help\act1.rtf";
            file3 = @"Resources\Help\Power.rtf";
            file4 = @"Resources\Help\act2.rtf";
            file5 = @"Resources\Help\M.rtf";
            file6 = @"Resources\Help\Determinant.rtf";
            file7 = @"Resources\Help\Rang.rtf";
            file8 = @"Resources\Help\Inverse.rtf";
            file9 = @"Resources\Help\Commands.rtf";
            file10 = @"Resources\Help\About.rtf";
        }

        void MatrixCreateStart()
        {
            MatrixCreateView window = new MatrixCreateView();
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.ShowDialog();
        }

        void SettingsStart(TextBox tb)
        {         
            if (SettingsControl.TextBox == null) { SettingsControl.TextBox = tb; }
            SettingWindow sw = new SettingWindow();
            sw.Closed += Sw_Closed;
            sw.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            sw.ShowDialog();
        }

        private void Sw_Closed(object sender, EventArgs e)
        {
            RaisePropertyChanged(()=>CurrentFont);
            RaisePropertyChanged(() => FontSize);
        }

        private void WritingSteps(Matrix.Matrix X)
        {
            if(SettingsControl.Comments)
            {
                if (X.Message != null)
                {
                    SettingsControl.TextBox.Dispatcher.Invoke((Action)delegate
                    {
                        SettingsControl.TextBox.AppendText("\n" + X.Message + "\n");
                    });
                }
            }
            Write(X, SettingsControl.TextBox);
        }

        private void MinorDet(string name, double n)
        {
            SettingsControl.TextBox.Dispatcher.Invoke((Action)delegate
            {
                SettingsControl.TextBox.AppendText(name + "=" + n + "\n");
            });
        }

        private void WritingInverseByStep(Matrix.Matrix X)
        {
            if (X.Message != null)
            {
                SettingsControl.TextBox.Dispatcher.Invoke((Action)delegate
                {
                    SettingsControl.TextBox.AppendText(X.Message + "\n");
                });
            }
            Write(X, SettingsControl.TextBox);
            SettingsControl.TextBox.Dispatcher.Invoke((Action)delegate
            {
                SettingsControl.TextBox.AppendText("\n");
            });
        }

        private void Write(Matrix.Matrix X, TextBox tb)
        {
            int max = 1;
            int leng = 0;
            int n = 0;
            //    /столбец - ширина
            Dictionary<int, int> MaxWidth = new Dictionary<int, int>();
            //в цикле находим максимальную длину строки в каждом столбце
            for (int j = 0; j < X.Columns; j++)
            {
                max = 1;
                MaxWidth.Add(j, max);
                for (int i = 0; i < X.Rows; i++)
                {
                    n = X.Matr[i, j].ToString().Length;
                    if (n > max)
                    {
                        max = n;
                        {
                            MaxWidth.Remove(j);
                            MaxWidth.Add(j, max);
                        }
                    }
                }
            }
            try
            {
                leng = X.Name.ToArray().Length;
            }
            catch { return; }

            string indent = new string(' ', leng + 4);
            int width = 0;
            int need = 0;
            for (int i = 0; i < X.Rows; i++)
            {
                tb.Dispatcher.Invoke((Action)delegate
                {
                    tb.AppendText("\n");
                });
                for (int j = 0; j < X.Columns; j++)
                {
                    if (j != 0)
                    {
                        //отступ в два пробела перед началом нового столбца
                        tb.Dispatcher.Invoke((Action)delegate
                        {
                            tb.AppendText("  ");
                        });
                    }
                    else
                    {
                        //отступ шириной в имя для первого столбца
                        tb.Dispatcher.Invoke((Action)delegate
                        {
                            tb.AppendText(indent);
                        });
                    }
                    width = MaxWidth[j];
                    need = width - X.Matr[i, j].ToString().Length;
                    {
                        for (int z = 0; z < need * 2; z++)
                        {
                            tb.Dispatcher.Invoke((Action)delegate
                            {
                                tb.AppendText(" ");
                            });
                        }
                        for (int k = 0; k < NumberOfSpace(X.Matr[i, j].ToString()); k++)
                        {
                            //дополнительные пробелы на запятую и минус
                            tb.Dispatcher.Invoke((Action)delegate
                            {
                                tb.AppendText(" ");
                            });
                        }
                    }

                    tb.Dispatcher.Invoke((Action)delegate
                    {
                        tb.AppendText(X.Matr[i, j].ToString());
                    });
                }
            }
            tb.Dispatcher.Invoke((Action)delegate
            {
                tb.AppendText("\n");
                tb.SelectionStart = tb.Text.Length;
                tb.ScrollToEnd();
            });    
        }

        private int NumberOfSpace(string str)
        {
            int n = 0;
            if(str.Contains("-"))
            {
                n++;
            }
            if(str.Contains(","))
            {
                n++;
            }
            return n;
        }

        private void EnterCommand(TextBox tb)
        {
            ButtonEnable = false;
            RaisePropertyChanged(() => ButtonEnable);

            if (SettingsControl.TextBox == null) { SettingsControl.TextBox = tb; }

            string str = Substring(tb);

            if ((str == "") | (str.Contains("\n")))
            {
                ButtonEnable = true;
                RaisePropertyChanged(() => ButtonEnable);
                return;
            }
            #region Determinant
            Regex det = new Regex(@"^det\([A-z]+[0-9]*\)$");
            Match detMatch = det.Match(str);
            if (detMatch.Success)
            {
                for (int i = 0; i < MatrixManager._matrixCollection.Count; i++)
                {
                    if (str == "det(" + MatrixManager._matrixCollection[i].Name + ")")
                    {
                        try
                        {
                            MatrixManager._matrixCollection[i].Round = SettingsControl.Round;
                            double res = 0;
                            if (StepByStep) { res = MatrixManager._matrixCollection[i].Determinant(true); }
                            else { res = MatrixManager._matrixCollection[i].Determinant(); }
                            SettingsControl.TextBox.Dispatcher.Invoke((Action)delegate
                            {
                                if (StepByStep)
                                {
                                    SettingsControl.TextBox.AppendText("=" + MatrixManager._matrixCollection[i].Message + "=" + res + "\n");
                                }
                                else
                                {
                                    SettingsControl.TextBox.AppendText("=" + res + "\n");
                                }
                                tb.SelectionStart = tb.Text.Length;
                                tb.ScrollToEnd();

                            });
                            ButtonEnable = true;
                            RaisePropertyChanged(() => ButtonEnable);
                            return;
                        }
                        catch (Matrix.IncompabilityOfColumnsAndRows ex)
                        {
                            SettingsControl.TextBox.Dispatcher.Invoke((Action)delegate
                            {
                                SettingsControl.TextBox.AppendText("=Матрица не является квадратной" + "\n");
                                tb.SelectionStart = tb.Text.Length;
                                tb.ScrollToEnd();

                            });
                            ButtonEnable = true;
                            RaisePropertyChanged(() => ButtonEnable);
                            return;
                        }
                    }
                }
            }
            #endregion

            #region Power
            Regex pow = new Regex(@"^[A-z]+[0-9]*\^\-?\d+[,.]?\d*");
            Match powMatch = pow.Match(str);
            if (powMatch.Success)
            {
                Regex number = new Regex(@"-?\d+$");
                Match num = number.Match(powMatch.Value);
                int N = 0;
                if (num.Success)
                {
                    N = int.Parse(num.Value);
                }
                for (int i = 0; i < MatrixManager._matrixCollection.Count; i++)
                {
                    if (str == MatrixManager._matrixCollection[i].Name + "^" + N)
                    {
                        try
                        {
                            MatrixManager._matrixCollection[i].Round = SettingsControl.Round;
                            Matrix.Matrix Res = new Matrix.Matrix(1, 1);
                            if (StepByStep)
                            {
                                Res = MatrixManager._matrixCollection[i].Power(N, WritingSteps);
                            }
                            else
                            {
                                Res = MatrixManager._matrixCollection[i].Power(N);
                            }
                            Res.Name = "Result";
                            SettingsControl.TextBox.Dispatcher.Invoke((Action)delegate
                            {
                                SettingsControl.TextBox.AppendText("\n" + Res.Name);
                                Write(Res, tb);
                            });
                            ButtonEnable = true;
                            RaisePropertyChanged(() => ButtonEnable);
                            return;
                        }
                        catch
                        {
                            SettingsControl.TextBox.Dispatcher.Invoke((Action)delegate
                            {
                                SettingsControl.TextBox.AppendText("=Матрица не является квадратной" + "\n");
                                tb.SelectionStart = tb.Text.Length;
                                tb.ScrollToEnd();

                            });
                            ButtonEnable = true;
                            RaisePropertyChanged(() => ButtonEnable);
                            return;
                        }
                    }
                }
            }
            #endregion

            #region Gauss
            Regex Gauss = new Regex(@"^Ref\([A-z]+[0-9]*\)$");
            Match gMatch = Gauss.Match(str);
            if (gMatch.Success)
            {
                for (int i = 0; i < MatrixManager._matrixCollection.Count; i++)
                {
                    if (str == "Ref(" + MatrixManager._matrixCollection[i].Name + ")")
                    {
                        try
                        {
                            MatrixManager._matrixCollection[i].Round = SettingsControl.Round;
                            Matrix.Matrix Res = new Matrix.Matrix(1, 1);
                            if (StepByStep)
                            {
                                Res = MatrixManager._matrixCollection[i].GetStepMatrix(WritingSteps);
                            }
                            else
                            {
                                Res = MatrixManager._matrixCollection[i].GetStepMatrix();
                            }
                            Res.Name = "Result";
                            SettingsControl.TextBox.Dispatcher.Invoke((Action)delegate
                            {
                                SettingsControl.TextBox.AppendText("\n" + Res.Name);
                                Write(Res, tb);
                            });
                            ButtonEnable = true;
                            RaisePropertyChanged(() => ButtonEnable);
                            return;
                        }
                        catch (Exception)
                        {
                            ButtonEnable = true;
                            RaisePropertyChanged(() => ButtonEnable);
                            return;
                        }
                    }
                }
            }
            #endregion

            #region Gauss-Jordan
            Regex ident = new Regex(@"^Ident\([A-z]+[0-9]*\)$");
            Match idMatch = ident.Match(str);
            if (idMatch.Success)
            {
                for (int i = 0; i < MatrixManager._matrixCollection.Count; i++)
                {
                    if (str == "Ident(" + MatrixManager._matrixCollection[i].Name + ")")
                    {
                        try
                        {
                            MatrixManager._matrixCollection[i].Round = SettingsControl.Round;
                            Matrix.Matrix Res = new Matrix.Matrix(1, 1);
                            if (StepByStep)
                            {
                                Res = MatrixManager._matrixCollection[i].Gauss_Jordan(WritingSteps);
                            }
                            else
                            {
                                Res = MatrixManager._matrixCollection[i].Gauss_Jordan();
                            }
                            Res.Name = "Result";
                            SettingsControl.TextBox.Dispatcher.Invoke((Action)delegate
                            {
                                SettingsControl.TextBox.AppendText("\n" + Res.Name);
                                Write(Res, tb);
                            });
                            ButtonEnable = true;
                            RaisePropertyChanged(() => ButtonEnable);
                            return;
                        }
                        catch (Exception)
                        {
                            ButtonEnable = true;
                            RaisePropertyChanged(() => ButtonEnable);
                            return;
                        }
                    }
                }
            }
            #endregion

            #region Inverse
            Regex Invers = new Regex(@"^Inv\([A-z]+[0-9]*\)$|^Inv\([A-z]+[0-9]*\,Cof\)$");
            Match invMatch = Invers.Match(str);
            if (invMatch.Success)
            {
                for (int i = 0; i < MatrixManager._matrixCollection.Count; i++)
                {
                    try
                    {
                        if (str == "Inv(" + MatrixManager._matrixCollection[i].Name + ")")
                        {

                            MatrixManager._matrixCollection[i].Round = SettingsControl.Round;
                            Matrix.Matrix Res = new Matrix.Matrix(1, 1);
                            if (StepByStep)
                            {
                                Res = MatrixManager._matrixCollection[i].InverseByGauss_Gordan(WritingSteps);
                            }
                            else
                            {
                                Res = MatrixManager._matrixCollection[i].InverseByGauss_Gordan();
                            }
                            Res.Name = "Result";
                            SettingsControl.TextBox.Dispatcher.Invoke((Action)delegate
                            {
                                SettingsControl.TextBox.AppendText("\n" + Res.Name);
                                Write(Res, tb);
                            });
                            ButtonEnable = true;
                            RaisePropertyChanged(() => ButtonEnable);
                            return;
                        }
                        if (str == "Inv(" + MatrixManager._matrixCollection[i].Name + ",Cof" + ")")
                        {

                            MatrixManager._matrixCollection[i].Round = SettingsControl.Round;
                            Matrix.Matrix Res = new Matrix.Matrix(1, 1);
                            if (StepByStep)
                            {
                                SettingsControl.TextBox.Dispatcher.Invoke((Action)delegate
                                {
                                    SettingsControl.TextBox.AppendText("\n");
                                });
                                Res = MatrixManager._matrixCollection[i].InverseByCofactor(WritingInverseByStep, MinorDet);
                            }
                            else
                            {
                                Res = MatrixManager._matrixCollection[i].InverseByCofactor();
                            }
                            Res.Name = "Result";
                            SettingsControl.TextBox.Dispatcher.Invoke((Action)delegate
                            {
                                SettingsControl.TextBox.AppendText("\n" + Res.Name);
                                Write(Res, tb);
                            });
                            ButtonEnable = true;
                            RaisePropertyChanged(() => ButtonEnable);
                            return;
                        }
                    }
                    catch (Matrix.IncompabilityOfColumnsAndRows ex)
                    {
                        SettingsControl.TextBox.Dispatcher.Invoke((Action)delegate
                        {
                            SettingsControl.TextBox.AppendText("=Матрица не является квадратной" + "\n");
                            tb.SelectionStart = tb.Text.Length;
                            tb.ScrollToEnd();

                        });
                        ButtonEnable = true;
                        RaisePropertyChanged(() => ButtonEnable);
                        return;
                    }
                }
            }
            #endregion

            #region Rang
            Regex rang = new Regex(@"^Rang\([A-z]+[0-9]*\)$");
            Match rangMatch = rang.Match(str);
            if (rangMatch.Success)
            {
                for (int i = 0; i < MatrixManager._matrixCollection.Count; i++)
                {
                    if (str == "Rang(" + MatrixManager._matrixCollection[i].Name + ")")
                    {
                        try
                        {
                            MatrixManager._matrixCollection[i].Round = SettingsControl.Round;
                            double res = 0;
                            res = MatrixManager._matrixCollection[i].Rang();
                            SettingsControl.TextBox.Dispatcher.Invoke((Action)delegate
                            {
                                SettingsControl.TextBox.AppendText("=" + res + "\n");
                                tb.SelectionStart = tb.Text.Length;
                                tb.ScrollToEnd();
                            });
                            ButtonEnable = true;
                            RaisePropertyChanged(() => ButtonEnable);
                            return;
                        }
                        catch (Exception)
                        {
                            ButtonEnable = true;
                            RaisePropertyChanged(() => ButtonEnable);
                            return;
                        }
                    }
                }
            }
            #endregion

            #region Transpose
            Regex tr = new Regex(@"^Tr\([A-z]+[0-9]*\)$");
            Match trMatch = tr.Match(str);
            if (trMatch.Success)
            {
                for (int i = 0; i < MatrixManager._matrixCollection.Count; i++)
                {
                    if (str == "Tr(" + MatrixManager._matrixCollection[i].Name + ")")
                    {
                        try
                        {
                            MatrixManager._matrixCollection[i].Round = SettingsControl.Round;
                            Matrix.Matrix Res = new Matrix.Matrix(1, 1);
                            Res = MatrixManager._matrixCollection[i].Transpose();
                            Res.Name = "Result";
                            SettingsControl.TextBox.Dispatcher.Invoke((Action)delegate
                            {
                                SettingsControl.TextBox.AppendText("\n" + Res.Name);
                                Write(Res, tb);
                            });
                            ButtonEnable = true;
                            RaisePropertyChanged(() => ButtonEnable);
                            return;
                        }
                        catch (Exception)
                        {
                            SettingsControl.TextBox.Dispatcher.Invoke((Action)delegate
                            {
                                SettingsControl.TextBox.AppendText("\n");
                                tb.SelectionStart = tb.Text.Length;
                                tb.ScrollToEnd();

                            });
                            ButtonEnable = true;
                            RaisePropertyChanged(() => ButtonEnable);
                            return;
                        }
                    }
                }
            }
            #endregion

            #region Expession
            if (!(str.Contains('*') | str.Contains('+') | str.Contains('-') | str.Contains('^')))
            {
                SettingsControl.TextBox.Dispatcher.Invoke((Action)delegate
                {
                    SettingsControl.TextBox.AppendText("\n");
                    tb.SelectionStart = tb.Text.Length;
                    tb.ScrollToEnd();
                });
                ButtonEnable = true;
                RaisePropertyChanged(() => ButtonEnable);
                return;
            }
            Parser P = new Parser(str);
            if (P.Valid())
            {
                Matrix.Matrix Res = P.GetResult();
                if (Res.Message != null)
                {
                    SettingsControl.TextBox.Dispatcher.Invoke((Action)delegate
                    {
                        SettingsControl.TextBox.AppendText("\n" + Res.Message + "\n");
                        tb.SelectionStart = tb.Text.Length;
                        tb.ScrollToEnd();
                    });
                }
                else
                {
                    Res.Name = "Result";
                    MatrixManager.AddMatrix(Res);
                    tb.AppendText("\n" + Res.Name);
                    Write(Res, SettingsControl.TextBox);
                }
            }
            //если не прошли валидацию
            else
            {
                StringBuilder text = new StringBuilder(tb.Text);
                int pos = tb.CaretIndex;
                text = text.Insert(pos, "\n");
                tb.Clear();
                tb.AppendText(text.ToString());
                tb.CaretIndex = pos + 1;
                ButtonEnable = true;
                RaisePropertyChanged(() => ButtonEnable);
                return;
            }
            #endregion

            ButtonEnable = true;
            RaisePropertyChanged(() => ButtonEnable);
        }

        private string Substring(TextBox tb)
        {
            int n = 0;
            for (int i = tb.CaretIndex; i > 0; i--)
            {
                n++;

                string s = tb.Text[i - 1].ToString();
                if (s == "\n")
                {
                    return tb.Text.Substring(i, n - 1);
                }
            }
            return tb.Text.Substring(0, tb.CaretIndex);
        }
        #endregion
    }

    #region ViewConvertors
    [ValueConversion(typeof(double), typeof(double))]
    public class BottomRowHeigh : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double heigh = (double)value + 10;
            return heigh;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(double), typeof(double))]
    public class HelpWidth : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double heigh = (double)value ;
            return heigh-60;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(double), typeof(double))]
    public class MaxWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double maxWidth = (double)value*4/5;
            return maxWidth;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(double), typeof(double))]
    public class MinWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double minWidth = (double)value / 5;
            return minWidth - 4;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(double), typeof(double))]
    public class One8 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double Width = (double)value / 8;
            return Width;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(double), typeof(double))]
    public class Conv45 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double Width = (double)value*4 / 5;
            return Width;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(double), typeof(double))]
    public class OneThird : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double w = (double)value / 3;
            return w;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(double), typeof(double))]
    public class Plus25 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double h = (double)value + 25;
            return h;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(double), typeof(double))]
    public class BtnWidth : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double minWidth = (double)value / 5;
            return minWidth;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(Matrix.Matrix), typeof(Grid))]
    public class MatrixWrite : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Matrix.Matrix X = (Matrix.Matrix)value;
            try
            {
                return RebuildMatrix(X.Rows, X.Columns, X);
            }
            catch { return new Grid(); }
        }

        private System.Windows.Controls.Grid RebuildMatrix(int Row, int Col, Matrix.Matrix X)
        {
            if (Col == 0 | Row == 0) { return new System.Windows.Controls.Grid(); }
            Grid grid = new Grid();
            grid.ClipToBounds = false;
            grid.HorizontalAlignment = HorizontalAlignment.Center;
            grid.VerticalAlignment = VerticalAlignment.Center;
            List<System.Windows.Controls.TextBlock> listBox = new List<System.Windows.Controls.TextBlock>();
            for (int i = 0; i < Row; i++)
            {
                for (int j = 0; j < Col; j++)
                {
                    System.Windows.Controls.TextBlock tb = new System.Windows.Controls.TextBlock();
                    tb.TextWrapping = TextWrapping.NoWrap;
                    tb.TextAlignment = TextAlignment.Center;
                    tb.MinHeight = 20;
                    tb.MinWidth = 20;
                    tb.Text = X.Matr[i, j].ToString();
                    MyControls.MyTextBox.SetMask(tb, MyControls.MyTextBox.MaskType.Decimal);
                    listBox.Add(tb);
                }
            }

            for (int i = 0; i < Row; i++)
            {
                System.Windows.Controls.RowDefinition row = new System.Windows.Controls.RowDefinition();
                grid.RowDefinitions.Add(row);
            }
            for (int j = 0; j < Col; j++)
            {
                System.Windows.Controls.ColumnDefinition col = new System.Windows.Controls.ColumnDefinition();
                grid.ColumnDefinitions.Add(col);
            }

            foreach (TextBlock t in listBox)
            {
                grid.Children.Add(t);
            }

            int z = 0;
            for (int i = 0; i < Row; i++)
            {
                {
                    for (int j = 0; j < Col; j++)
                    {
                        System.Windows.Controls.Grid.SetRow(grid.Children[z], i);
                        System.Windows.Controls.Grid.SetColumn(grid.Children[z], j);
                        z++;
                    }
                }
            }
            return grid;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    #endregion

}
