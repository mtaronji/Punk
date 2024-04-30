﻿
using MathNet.Numerics.LinearAlgebra;
using System.Text.RegularExpressions;


namespace Punk.Types
{
    public class MatrixType
    {
        public double Determinant { get; private set; }

        public Matrix<double> Value { get; set; }
      
        public string Syntax { get; set; }
        public static Regex SplitonWhitespace = new Regex(@"\s+");
        public MatrixType(string syntax)
        {
            Syntax = syntax;
            Value = TryParseMatrixSyntax();

        }
        public void CalculateDeterminant()
        {
            Determinant = Value.Determinant();
        }
        public MatrixType(Matrix<double> m)
        {
            Value = m;
            this.Syntax = string.Empty;
        }


        private Matrix<double> TryParseMatrixSyntax()
        {

            string BarsRemoved = Syntax.Replace("|", "").Trim();
            var rows = BarsRemoved.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            double[][] matrixdata = new double[rows.Length][];
            for (int i = 0; i < rows.Length; i++)
            {

                string[] colvalues = SplitonWhitespace.Split(rows[i].Trim()).Where(c => c != string.Empty).ToArray();

                matrixdata[i] = new double[colvalues.Length];
                for (int j = 0; j < colvalues.Length; j++)
                {
                    var success = double.TryParse(colvalues[j].Trim(), out double value);
                    if (!success) { throw new Exceptions.PunkMatrixParseException(); }
                    matrixdata[i][j] = value;
                }

            }
            return Matrix<double>.Build.DenseOfRowArrays(matrixdata);
        }

    }
}